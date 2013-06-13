//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;
using System;
using Tunynet.Common;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Events;
using System.Linq;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///推荐内容数据访问仓储
    /// </summary>
    public class RecommendItemTypeRepository : Repository<RecommendItemType>, IRecommendItemTypeRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override object Insert(RecommendItemType entity)
        {
            object typeId = base.Insert(entity);
            RealTimeCacheHelper.IncreaseAreaVersion("TenantTypeId", string.Empty);
            return typeId;
        }

        /// <summary>
        /// 删除推荐类别
        /// </summary>
        /// <param name="entity">推荐类别实体</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public override int Delete(RecommendItemType entity)
        {
            int affectCount = base.Delete(entity);
            if (affectCount > 0)
            {
                Sql sql = Sql.Builder;
                sql.Append("delete from tn_RecommendItems where TypeId = @0", entity.TypeId);
                CreateDAO().Execute(sql);
            }
            return affectCount;
        }

        /// <summary>
        /// 获取推荐类别列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<RecommendItemType> GetRecommendTypes(string tenantTypeId)
        {
            //设计要点
            //1、需要维护缓存即时性，使用tenantTypeId分区版本
            string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId ?? string.Empty) + "RecommendItemTypes";

            IEnumerable<string> recommendItemTypeIds = cacheService.Get<IEnumerable<string>>(cacheKey);
            if (recommendItemTypeIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("TypeId")
                        .From("tn_RecommendItemTypes");
                if (!string.IsNullOrEmpty(tenantTypeId))
                {
                    sql.Where("TenantTypeId=@0", tenantTypeId);
                }
                recommendItemTypeIds = CreateDAO().Fetch<string>(sql);
                cacheService.Add(cacheKey, recommendItemTypeIds, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds<string>(recommendItemTypeIds);
        }
    }
}