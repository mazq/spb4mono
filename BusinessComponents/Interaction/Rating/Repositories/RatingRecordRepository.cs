//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 星级评价记录的数据访问
    /// </summary>
    public class RatingRecordRepository : Repository<RatingRecord>, IRatingRecordRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 删除用户的记录
        /// </summary>
        /// <param name="userId"></param>
        public void ClearByUser(long userId)
        {
            var sql = Sql.Builder;
            sql.Append("DELETE FROM tn_RatingRecords WHERE UserId = @0", userId);

            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 删除N天前的评价记录
        /// </summary>
        /// <param name="beforeDays">间隔天数</param>
        public void Clean(int? beforeDays)
        {
            var sql = Sql.Builder;
            if (beforeDays.HasValue && beforeDays.Value > 0)
            {
                sql.Append("DELETE FROM tn_RatingRecords WHERE DateCreated < @0", DateTime.UtcNow.AddDays(-(double)beforeDays));
            }
            else
            {
                sql.Append("DELETE FROM tn_RatingRecords");
            }

            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 清空相关联评价记录
        /// </summary>
        /// <param name="objectId">操作Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public void ClearRatingRecordsOfObjectId(long objectId, string tenantTypeId)
        {
            var sql = Sql.Builder;
            sql.Append("DELETE FROM tn_RatingRecords WHERE ObjectId = @0 and TenantTypeId = @1 ", objectId, tenantTypeId);

            CreateDAO().Execute(sql);
        }

        /// <summary>
        ///获取前N条用户的星级评价记录信息
        /// </summary>
        /// <param name="objectId"> 操作Id</param>
        /// <param name="tentanTypeId">操作类型Id</param>
        /// <param name="rateNumber">等级类型</param>
        /// <param name="topNumber">前N条</param>
        public IEnumerable<RatingRecord> GetTopRatingRecords(long objectId, string tentanTypeId, int? rateNumber, int topNumber)
        {
            return GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ObjectId", objectId));

                    cacheKey.Append("GetTopRatingRecords:");
                    if (!string.IsNullOrEmpty(tentanTypeId))
                        cacheKey.AppendFormat("TentanTypeId-{0}:RateNumber-{1}", tentanTypeId, rateNumber);
                    return cacheKey.ToString();
                },
                () =>
                {
                    var sql = Sql.Builder;
                    sql.Where("ObjectId = @0 and TenantTypeId = @1 ", objectId, tentanTypeId);
                    if (rateNumber.HasValue && rateNumber.Value >= 1 && rateNumber.Value <= 5)
                    {
                        sql.Where("RateNumber = @0", rateNumber);
                    }

                    sql.OrderBy("Id DESC");
                    return sql;
                });
        }
    }
}