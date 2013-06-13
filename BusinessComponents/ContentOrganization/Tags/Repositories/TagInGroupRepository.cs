//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Linq;
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 标签与分组关系仓储的具体实现类
    /// </summary>
    public class TagInGroupRepository : Repository<TagInGroup>, ITagInGroupRepository
    {
        int pageSize = 20;
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 添加标签与分组关联
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="groupId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public int AddTagInGroup(string tagName, long groupId, string tenantTypeId)
        {
            PetaPocoDatabase dao = CreateDAO();

            int affectCount = 0;
            dao.OpenSharedConnection();

            var sql = PetaPoco.Sql.Builder;
            sql.Select("count(*)")
               .From("tn_TagsInGroups")
               .Where("GroupId = @0", groupId)
               .Where("TagName = @0", tagName)
               .Where("TenantTypeId = @0", tenantTypeId);

            affectCount = dao.ExecuteScalar<int>(sql);
            if (affectCount == 0)
            {
                sql = PetaPoco.Sql.Builder;
                sql.Append("INSERT INTO tn_TagsInGroups (GroupId, TagName,TenantTypeId) VALUES (@0,@1,@2)", groupId, tagName, tenantTypeId);
                affectCount = dao.Execute(sql);

                if (affectCount > 0)
                {
                    RealTimeCacheHelper.IncreaseAreaVersion("GroupId", groupId);
                }
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 批量添加分组给标签
        /// </summary>
        /// <param name="groupIds">分组Id集合</param>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public int BatchAddGroupsToTag(IEnumerable<long> groupIds, string tagName, string tenantTypeId)
        {

            int affectCount = 0;

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            dao.Execute(Sql.Builder.Append("delete from tn_TagsInGroups where tagName = @0 and TenantTypeId = @1", tagName, tenantTypeId));

            IList<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();

            foreach (var groupId in groupIds)
            {
                //if (groupId <= 0)
                //    continue;

                sqls.Add(Sql.Builder.Append("INSERT INTO tn_TagsInGroups (GroupId, TagName,TenantTypeId) VALUES (@0,@1,@2)", groupId, tagName, tenantTypeId));
            }
            affectCount = dao.Execute(sqls);

            if (affectCount > 0)
            {
                groupIds.ToList().ForEach(n =>
                {
                    RealTimeCacheHelper.IncreaseAreaVersion("GroupId", n);
                });

                EntityData.ForType(typeof(TagGroup)).RealTimeCacheHelper.IncreaseAreaVersion("TenantTypeId", tenantTypeId);
            }
            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 批量添加标签与分组关联
        /// </summary>
        /// <param name="tagNames">标签名</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public int BatchAddTagsInGroup(IEnumerable<string> tagNames, long groupId, string tenantTypeId)
        {
            int affectCount = 0;
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            dao.Execute(Sql.Builder.Append("delete from tn_TagsInGroups where GroupId = @0", groupId));

            IList<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();

            foreach (var tagName in tagNames)
            {
                if (string.IsNullOrEmpty(tagName))
                    continue;

                sqls.Add(Sql.Builder.Append("INSERT INTO tn_TagsInGroups (GroupId, TagName,TenantTypeId) VALUES (@0,@1,@2)", groupId, tagName, tenantTypeId));
            }
            affectCount = dao.Execute(sqls);

            if (affectCount > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("GroupId", groupId);
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 清除分组的所有标签
        /// </summary>
        /// <param name="groupId">分组Id</param>
        public int ClearTagsFromGroup(long groupId)
        {

            var sql = PetaPoco.Sql.Builder;
            sql.Append("delete from tn_TagsInGroups")
               .Where("GroupId = @0", groupId);

            int affectCount = CreateDAO().Execute(sql);
            if (affectCount > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("GroupId", groupId);
            }

            return affectCount;
        }

        /// <summary>
        /// 获取标签与分组关系
        /// </summary>
        /// <param name="groupId">分组Id</param>
        public IEnumerable<string> GetTagsOfGroup(long groupId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "GroupId", groupId));
            cacheKey.AppendFormat("TagsInGroup");

            IEnumerable<string> tagNames = cacheService.Get<IEnumerable<string>>(cacheKey.ToString());

            if (tagNames == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("TagName")
                .From("tn_TagsInGroups")
                .Where("GroupId = @0", groupId);

                tagNames = CreateDAO().Fetch<string>(sql);
                cacheService.Add(cacheKey.ToString(), tagNames, CachingExpirationType.ObjectCollection);
            }

            return tagNames;
        }
    }
}