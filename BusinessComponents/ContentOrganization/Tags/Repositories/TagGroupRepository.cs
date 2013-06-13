//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 标签分组仓储的具体实现类
    /// </summary>
    public class TagGroupRepository : Repository<TagGroup>, ITagGroupRepository
    {

        ICacheService cacheService = DIContainer.Resolve<ICacheService>();


        /// <summary>
        /// 根据标签获取标签分组
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public IEnumerable<TagGroup> GetGroupsOfTag(string tagName, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
            cacheKey.Append("tagName-" + tagName);

            IEnumerable<TagGroup> tagGroups = cacheService.Get<IEnumerable<TagGroup>>(cacheKey.ToString());

            if (tagGroups == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                   .From("tn_TagGroups")
                   .Where("GroupId  in (select GroupId from tn_TagsInGroups where TagName = @0 and TenantTypeId = @1)", tagName, tenantTypeId);

                tagGroups = CreateDAO().Fetch<TagGroup>(sql);
                cacheService.Add(cacheKey.ToString(), tagGroups, CachingExpirationType.ObjectCollection);
            }

            return tagGroups;
        }

        /// <summary>
        /// 获取全部标签分组
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<TagGroup> GetGroups(string tenantTypeId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("*")
               .From("tn_TagGroups");

            if (!string.IsNullOrEmpty(tenantTypeId))
                sql.Where("TenantTypeId = @0", tenantTypeId);

            IEnumerable<TagGroup> tagGroups = CreateDAO().Fetch<TagGroup>(sql);

            return tagGroups;
        }
    }
}