//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 星级评价的数据访问
    /// </summary>
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private int pageSize = 20;
        private IRatingSettingsManager ratingSettingsManager = DIContainer.Resolve<IRatingSettingsManager>();
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取星级评价信息
        /// </summary>
        /// <param name="objectId">操作对象</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public Rating Get(long objectId, string tenantTypeId)
        {
            var sql = Sql.Builder;

            sql.Where("ObjectId = @0 and TenantTypeId = @1", objectId, tenantTypeId);
            string cacheKey = GetCacheKey_Rating(tenantTypeId, objectId);
            Rating entity = cacheService.Get<Rating>(cacheKey);
            if (entity == null)
            {
                entity = CreateDAO().FirstOrDefault<Rating>(sql);
                if (RealTimeCacheHelper.EnableCache)
                {
                    cacheService.Add(cacheKey, entity, CachingExpirationType.SingleObject);
                }
            }

            return entity;
        }

        /// <summary>
        /// 获取前N条操作对象Id
        /// </summary>
        /// <param name="topNumber">获取前N条</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        public IEnumerable<long> GetTopObjectIds(int topNumber, string tenantTypeId, long ownerId = 0)
        {
            //从评价信息表中查询并按综合评价倒序

            PagingEntityIdCollection peic = null;
            string cacheKey = GetCacheKey_ObjectIds(tenantTypeId, ownerId);
            peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (peic == null)
            {
                var sql = Sql.Builder;
                sql.Select("ObjectId")
                .From("tn_Ratings")
                .Where("TenantTypeId=@0", tenantTypeId);
                if (ownerId > 0)
                    sql.Where("OwnerId=@0", ownerId);
                sql.OrderBy("Comprehensive DESC");
                IEnumerable<object> entityIds = CreateDAO().FetchTopPrimaryKeys(SecondaryMaxRecords, "ObjectId", sql);
                peic = new PagingEntityIdCollection(entityIds);
                cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
            }
            IEnumerable<object> topEntityIds = peic.GetTopEntityIds(topNumber);
            return topEntityIds.Cast<long>();
        }

        /// <summary>
        /// 对操作对象进行星级评价操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <param name="rateNumber">星级类型</param>
        /// <param name="ownerId">拥有者Id</param>
        
        public bool Rated(long objectId, string tenantTypeId, long userId, int rateNumber, long ownerId = 0)
        {
            RatingSettings ratingSettings = ratingSettingsManager.Get();
            //执行Sql语句的集合
            List<Sql> sqls = new List<Sql>();
            //判断输入的等级数是否符合标准
            if (rateNumber < 1 || rateNumber > 5)
            {
                return false;
            }
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();
            //先判断是否存在评价信息
            var sql = Sql.Builder;
            sql.Select("Count(*)")
               .From("tn_Ratings")
               .Where("ObjectId = @0 AND TenantTypeId = @1", objectId, tenantTypeId);
            if (ownerId > 0)
                sql.Where("OwnerId = @0", ownerId);

            int affectCount = dao.ExecuteScalar<int>(sql);
            //如果记录为0则不存在则先创建评价信息
            if (affectCount == 0)
            {
                //创建 评价信息
                sqls.Add(Sql.Builder.Append("INSERT INTO tn_Ratings  (ObjectId, TenantTypeId, OwnerId, RateCount, Comprehensive , RateSum ) VALUES (@0, @1, @2,1,@3,@4)", objectId, tenantTypeId, ownerId, rateNumber, rateNumber));
                //创建时直接添加新记录数据
                for (int i = 1; i <= 5; i++)
                {
                    if (rateNumber == i)
                    {  //如果等级是用户所选择的等级则直接插入数据 并且创建记录
                        sqls.Add(Sql.Builder.Append("INSERT INTO tn_RatingGrades (ObjectId, TenantTypeId, RateNumber, RateCount) VALUES (@0,@1,@2,1)", objectId, tenantTypeId, i));
                        sqls.Add(Sql.Builder.Append("INSERT INTO tn_RatingRecords  (ObjectId, TenantTypeId, RateNumber, UserId, DateCreated) VALUES (@0,@1,@2,@3,@4)", objectId, tenantTypeId, i, userId, DateTime.UtcNow));

                        continue;
                    }
                    sqls.Add(Sql.Builder.Append("INSERT INTO tn_RatingGrades (ObjectId, TenantTypeId, RateNumber, RateCount) VALUES (@0,@1,@2,@3)", objectId, tenantTypeId, i, 0));
                }
            }
            else
            {
                //判断是都有过评价记录
                sql = Sql.Builder;
                sql.Select("*")
                .From("tn_RatingRecords")
                .Where("ObjectId = @0 AND TenantTypeId = @1 AND UserId = @2", objectId, tenantTypeId, userId);
                //获取评价的记录
                RatingRecord ratingRecord = dao.FirstOrDefault<RatingRecord>(sql);
                if (ratingRecord == null)
                {
                    sqls.Add(Sql.Builder.Append("INSERT INTO tn_RatingRecords (ObjectId, TenantTypeId, RateNumber, UserId, DateCreated) VALUES (@0,@1,@2,@3,@4)", objectId, tenantTypeId, rateNumber, userId, DateTime.UtcNow));
                    //更新信息评价数据
                    sqls.Add(Sql.Builder.Append("UPDATE tn_Ratings  SET RateCount = RateCount + 1, Comprehensive=(RateSum + @0)/(RateCount + 1.0) , RateSum=RateSum + @0 where ObjectId = @1 and TenantTypeId = @2 and OwnerId = @3", rateNumber, objectId, tenantTypeId, ownerId));
                    //更新评价等级统计
                    sqls.Add(Sql.Builder.Append("UPDATE tn_RatingGrades SET  RateCount = RateCount + 1 where ObjectId = @0 and  TenantTypeId = @1 and  RateNumber = @2", objectId, tenantTypeId, rateNumber));
                }
                else
                {
                    //用户 再次评价 先判断是否可修改
                    if (ratingSettings.IsModify)
                    {
                        //先检测是否是之前选过的等级
                        if (ratingRecord.RateNumber != rateNumber)
                        {
                            sqls.Add(Sql.Builder.Append("UPDATE tn_RatingRecords SET RateNumber = @0 WHERE ObjectId = @1 AND TenantTypeId = @2 AND UserId = @3", rateNumber, objectId, tenantTypeId, userId));
                            //先更新之前的等级统计
                            sqls.Add(Sql.Builder.Append("UPDATE tn_RatingGrades SET RateCount = RateCount - 1 where ObjectId = @0 and TenantTypeId = @1 and RateNumber = @2", objectId, tenantTypeId, ratingRecord.RateNumber));
                            //然后更新现在的最新等级统计
                            sqls.Add(Sql.Builder.Append("UPDATE tn_RatingGrades SET RateCount = RateCount + 1 where ObjectId = @0 and TenantTypeId = @1 and  RateNumber = @2", objectId, tenantTypeId, rateNumber));
                            //之后更新评价信息表
                            sqls.Add(Sql.Builder.Append("UPDATE tn_Ratings SET Comprehensive = (RateSum + @0 - @1)/RateCount , RateSum=RateSum + @0 - @1 where ObjectId=@2 and TenantTypeId = @3 and OwnerId = @4", (float)rateNumber, (float)ratingRecord.RateNumber, objectId, tenantTypeId, ownerId));
                        }
                    }
                }
            }
            if (sqls == null)
            {
                return false;
            }
            using (var transaction = dao.GetTransaction())
            {
                affectCount = dao.Execute(sqls);
                transaction.Complete();
            }
            if (affectCount > 0)
            {
                if (RealTimeCacheHelper.EnableCache)
                {
                    EntityData.ForType(typeof(RatingRecord)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);

                    EntityData.ForType(typeof(RatingGrade)).RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);

                    RealTimeCacheHelper.IncreaseAreaVersion("ObjectId", objectId);
                }
            }
            else
            {
                return false;
            }

            dao.CloseSharedConnection();

            return true;
        }

        /// <summary>
        /// 用户当前操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <returns>用户是否做过评价：True-评价过， False-没做过操作</returns>
        public bool IsRated(long objectId, string tenantTypeId, long userId)
        {
            bool returnValue = false;

            string cacheKey = GetCacheKey_IsRated(objectId, tenantTypeId);
            IList<long> userIds = cacheService.Get(cacheKey) as List<long>;

            if (userIds == null)
            {
                var sql = Sql.Builder;

                sql.Select("UserId")
                   .From("tn_RatingRecords")
                   .Where("ObjectId = @0 and TenantTypeId = @1", objectId, tenantTypeId);

                //获得是否有过评价
                userIds = CreateDAO().Fetch<long>(sql);
                if (userIds != null)
                {
                    cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
                    returnValue = userIds.Contains(userId);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 获取操作对象Id集合  用于分页
        /// </summary>
        /// <param name="tenantTypeId"> 租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="ownerId">拥有者Id</param>
        public PagingEntityIdCollection GetPagingObjectIds(string tenantTypeId, int pageIndex = 1, long ownerId = 0)
        {
            //从评价信息表中查询 并按综合评价倒序排序
            var sql = Sql.Builder;
            sql.Select("ObjectId")
               .From("tn_Ratings")
               .Where("TenantTypeId = @0", tenantTypeId);

            if (ownerId > 0)
                sql.Where("OwnerId = @0", ownerId);
            sql.OrderBy("Comprehensive DESC");

            PetaPocoDatabase dao = CreateDAO();
            PagingEntityIdCollection peic = null;
            if (pageIndex <= CacheablePageCount)
            {
                string cacheKey = GetCacheKey_ObjectIds(tenantTypeId, ownerId);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "ObjectId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "ObjectId", sql);
            }

            return peic;
        }

        #region 辅助方法

        /// <summary>
        /// 获取是否评价过CacheKey
        /// </summary>
        /// <param name="objectId">操作类型Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        private string GetCacheKey_IsRated(long objectId, string tenantTypeId)
        {
            return string.Format("IsRated::Object:-ObjectId:{0}-TenantTypeId:{1}", objectId, tenantTypeId);
        }

        /// <summary>
        /// 获取星级评价信息的CacheKey
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="objectId">操作对象</param>
        private string GetCacheKey_Rating(string tenantTypeId, long objectId)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("ObjectId", objectId);
            return string.Format("ObjectRating{0}::TenantTypeId:{1}", areaVersion, tenantTypeId);
        }

        /// <summary>
        /// 获取评价操作Id集合的CacheKey
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        private string GetCacheKey_ObjectIds(string tenantTypeId, long ownerId)
        {
            return string.Format("RatingObjectIds::TenantTypeId-{0}:OwnerId-{1}", tenantTypeId, ownerId);
        }

        #endregion 辅助方法

        #region 配置属性

        private int secondaryMaxRecords = 1000;

        /// <summary>
        /// 非主流查询最大允许返回记录数
        /// </summary>
        /// <remarks>
        /// 例如：排行数据
        /// </remarks>
        protected override int SecondaryMaxRecords
        {
            get { return secondaryMaxRecords; }
        }

        
        private int cacheablePageCount = 30;

        /// <summary>
        /// 可缓存的列表缓存页数
        /// </summary>
        protected override int CacheablePageCount
        {
            get { return cacheablePageCount; }
        }

        #endregion 配置属性
    }
}