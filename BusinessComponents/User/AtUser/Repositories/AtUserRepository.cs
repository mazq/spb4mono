//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Repositories;
using System;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// @用户Repository
    /// </summary>
    public class AtUserRepository : Repository<AtUserEntity>, IAtUserRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        private int pageSize = 20;

        /// <summary>
        /// 批量创建At用户
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public bool BatchCreateAtUser(List<long> userIds, long associateId, string tenantTypeId)
        {
            if (userIds == null)
                return false;

            IList<Sql> sqls = new List<Sql>();
            int affectCount = 0;
            List<long> tmp_UserIds = GetAtUserIds(associateId, tenantTypeId);
            if (tmp_UserIds != null && tmp_UserIds.Count() > 0)
            {
                ClearAtUsers(associateId, tenantTypeId);
            }


            foreach (var userId in userIds)
            {
                if (userId == 0)
                    continue;

                sqls.Add(Sql.Builder.Append("Insert tn_AtUsers (UserId,AssociateId,TenantTypeId) values (@0,@1,@2)", userId, associateId, tenantTypeId));
            }

            affectCount = CreateDAO().Execute(sqls);

            return affectCount > 0;
        }

        /// <summary>
        /// 获取用户关联内容的Id分页集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<long> GetPagingAssociateIds(long userId, string tenantTypeId, int pageIndex)
        {
            PagingEntityIdCollection peic = null;

            Sql sql = Sql.Builder;
            sql.Select("AssociateId")
               .From("tn_AtUsers")
               .Where("UserId = @0", userId)
               .Where("TenantTypeId = @0", tenantTypeId)
               .OrderBy("Id desc");

            PetaPocoDatabase dao = CreateDAO();
            if (pageIndex < CacheablePageCount)
            {
                string cacheKey = GetCacheKey_AtUserAssociateIds(userId, tenantTypeId);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null || peic.Count == 0)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "AssociateId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "AssociateId", sql);
            }

            if (peic != null)
            {
                PagingDataSet<long> pds = new PagingDataSet<long>(peic.GetPagingEntityIds(pageSize, pageIndex).Cast<long>());
                pds.PageSize = pageSize;
                pds.PageIndex = pageIndex;
                pds.TotalRecords = peic.TotalRecords;
                return pds;
            }

            return null;
        }

        /// <summary>
        /// 获取用户关联内容的用户名集合
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public List<long> GetAtUserIds(long associateId, string tenantTypeId)
        {
            string cacheKey = GetCacheKey_AllAtUserIds(associateId, tenantTypeId);
            List<long> userIds = cacheService.Get<List<long>>(cacheKey);

            if (userIds == null || userIds.Count() == 0)
            {
                Sql sql = Sql.Builder;
                sql.Select("UserId")
                   .From("tn_AtUsers")
                   .Where("AssociateId = @0", associateId)
                   .Where("TenantTypeId = @0", tenantTypeId)
                   .OrderBy("Id desc");

                userIds = CreateDAO().Fetch<long>(sql);

                cacheService.Set(cacheKey, userIds, CachingExpirationType.ObjectCollection);
            }

            return userIds;
        }

        /// <summary>
        /// 删除垃圾数据
        /// </summary>
        public void DeleteTrashDatas()
        {
            IEnumerable<TenantType> tenantTypes = new TenantTypeService().Gets(MultiTenantServiceKeys.Instance().AtUser());

            List<Sql> sqls = new List<Sql>();
            sqls.Add(Sql.Builder.Append("delete from tn_AtUsers where not exists (select 1 from tn_Users where UserId = tn_AtUsers.UserId)"));

            foreach (var tenantType in tenantTypes)
            {
                Type type = Type.GetType(tenantType.ClassType);
                if (type == null)
                    continue;

                var pd = PetaPoco.Database.PocoData.ForType(type);
                sqls.Add(Sql.Builder.Append("delete from tn_AtUsers")
                                    .Where("not exists (select 1 from " + pd.TableInfo.TableName + " where AssociateId = " + pd.TableInfo.PrimaryKey + ") and TenantTypeId = @0"
                                    , tenantType.TenantTypeId));
            }

            CreateDAO().Execute(sqls);
        }

        /// <summary>
        /// 清除关注用户
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public void ClearAtUsers(long associateId, string tenantTypeId)
        {
            Sql sql = Sql.Builder;
            
            //回复：已修改
            sql.Append("Delete from tn_AtUsers where AssociateId = @0 and tenantTypeId = @1", associateId, tenantTypeId);
            int affectCount = CreateDAO().Execute(sql);

            if (affectCount > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("AssociateId", tenantTypeId + "_" + associateId);
            }
        }

        #region Helper Method

        /// <summary>
        /// 获取全部提到用户名CacheKey
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCacheKey_AllAtUserIds(long associateId, string tenantTypeId)
        {
            return string.Format("AllAtUserIds{2}:AssociateId-{0}:TenantTypeId-{1}", associateId, tenantTypeId, RealTimeCacheHelper.GetAreaVersion("AssociateId", tenantTypeId + "_" + associateId));
        }

        /// <summary>
        /// 获取全部收藏CacheKey
        /// </summary>
        /// <param name="userId">At用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCacheKey_AtUserAssociateIds(long userId, string tenantTypeId)
        {

            string prefix = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId);
            return string.Format("PaingAtUserAssociateIds{0}::TenantTypeId-{1}", prefix, tenantTypeId);
        }

        #endregion Helper Method
    }
}
