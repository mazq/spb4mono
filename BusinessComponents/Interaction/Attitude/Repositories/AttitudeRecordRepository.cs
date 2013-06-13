//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using Tunynet.Repositories;
using PetaPoco;

namespace Tunynet.Common
{
    /// <summary>
    /// 顶踩记录的数据访问
    /// </summary>
    public class AttitudeRecordRepository : Repository<AttitudeRecord>, IAttitudeRecordRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取参与用户的Id集合
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="IsSupport">用户是否支持（true为支持，false为反对）</param>
        /// <param name="topNumber">获取条数</param>
        public IEnumerable<long> GetTopOperatedUserIds(long objectId, string tenantTypeId, bool IsSupport, int topNumber)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ObjectId", objectId));
            cacheKey.AppendFormat("TenantTypeId-{0}:IsSupport-{1}", tenantTypeId, Convert.ToInt32(IsSupport));
            PagingEntityIdCollection topOperatedUserIds = cacheService.Get<PagingEntityIdCollection>(cacheKey.ToString());
            if (topOperatedUserIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("UserId")
                   .From("tn_AttitudeRecords")
                   .Where("ObjectId =@0", objectId)
                   .Where("TenantTypeId=@0", tenantTypeId)
                   .Where("IsSupport = @0", IsSupport);
                IEnumerable<object> entityIds = CreateDAO().FetchTopPrimaryKeys(SecondaryMaxRecords, "UserId", sql);
                topOperatedUserIds = new PagingEntityIdCollection(entityIds);
                cacheService.Add(cacheKey.ToString(), topOperatedUserIds, CachingExpirationType.ObjectCollection);
            }
            IEnumerable<long> topEntityIds = topOperatedUserIds.GetTopEntityIds(topNumber).Cast<long>();
            return topEntityIds;
        }

        /// <summary>
        /// 获取用户在某一租户下顶过的内容
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<param name="userId">用户Id</param>
        ///<param name="pageSize">每页的内容数</param>
        ///<param name="pageIndex">页码</param>
        public PagingEntityIdCollection GetObjectIds(string tenantTypeId, long userId, int pageSize, int pageIndex)
        {
            var sql = Sql.Builder;
            sql.Select("ObjectId")
               .From("tn_AttitudeRecords")
               .Where("TenantTypeId = @0", tenantTypeId)
               .Where("UserId=@0", userId)
               .Where("IsSupport =1");

            sql.OrderBy("Id DESC");

            PagingEntityIdCollection objectIds = null;
            string cacheKey = GetCacheKey_ObjectIds(tenantTypeId, userId, pageIndex);
            objectIds = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (objectIds == null)
            {
                objectIds = CreateDAO().FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "ObjectId", sql);
                cacheService.Add(cacheKey, objectIds, CachingExpirationType.ObjectCollection);
            }
            return objectIds;
        }

        /// <summary>
        /// 获取操作Id集合的CacheKey
        /// </summary>
        /// <param name="tenantTypeId"> 租户类型Id</param>
        /// <param name="sortBy">排序类型</param>
        /// <returns></returns>
        private string GetCacheKey_ObjectIds(string tenantTypeId, long userId, int pageIndex)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
            cacheKey.AppendFormat("GetObjectIds::UserId-{0}:PageIndex-{1}", userId, pageIndex);
            return cacheKey.ToString();
        }
    }
}