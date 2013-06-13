//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///相关标签的仓储实现
    /// </summary>
    public class RelatedTagRepository : Repository<RelatedTag>, IRelatedTagRepository
    {
        /// <summary>
        /// 添加相关标签
        /// </summary>
        /// <remarks>
        /// 会为标签添加双向的关联关系,例如:
        /// TagA关联到TagB
        /// TagB关联到TagA
        /// </remarks>
        /// <param name="tagNames">相关标签名称集合</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tagId">标签Id</param>
        /// <returns> 影响行数</returns>
        public int AddRelatedTagsToTag(string[] tagNames, string tenantTypeId, long ownerId, long tagId)
        {
            PetaPocoDatabase dao = CreateDAO();

            int affectCount = 0;

            dao.OpenSharedConnection();

            foreach (string tagName in tagNames)
            {
                if (string.IsNullOrEmpty(tagName))
                    continue;

                IList<Sql> sqls = new List<Sql>();
                //创建标签
                var sql = Sql.Builder;
                sql.From("tn_Tags")
                   .Where("TenantTypeId = @0", tenantTypeId)
                   .Where("TagName = @0", tagName);
                var tag = dao.FirstOrDefault<Tag>(sql);
                if (tag == null)
                    sqls.Add(Sql.Builder.Append("insert into tn_Tags (TenantTypeId,TagName,DateCreated,ItemCount,OwnerCount) values (@0,@1,@2,1,1)", tenantTypeId, tagName, DateTime.UtcNow));
                //创建标签与用户的关联
                sql = Sql.Builder;
                sql.From("tn_TagsInOwners")
                  .Where("TenantTypeId = @0", tenantTypeId)
                  .Where("TagName = @0", tagName)
                  .Where("OwnerId = @0", ownerId);
                var tagInOwner = dao.FirstOrDefault<TagInOwner>(sql);
                if (tagInOwner == null)
                    sqls.Add(Sql.Builder.Append("insert into tn_TagsInOwners (TenantTypeId,TagName,OwnerId,ItemCount) values (@0,@1,@2,1)", tenantTypeId, tagName, ownerId));
                //创建标签与内容项的关联
                sqls.Add(Sql.Builder.Append("insert into tn_RelatedTags(TagId, RelatedTagId) select @0,TagId from tn_Tags where TenantTypeId = @1 and TagName = @2", tagId, tenantTypeId, tagName));
                sqls.Add(Sql.Builder.Append("insert into tn_RelatedTags(TagId, RelatedTagId) select TagId,@0 from tn_Tags where TenantTypeId = @1 and TagName = @2", tagId, tenantTypeId, tagName));

                //通过事务来控制多条语句执行时的一致性
                using (var transaction = dao.GetTransaction())
                {
                    affectCount = dao.Execute(sqls);
                    transaction.Complete();
                }
            }

            dao.CloseSharedConnection();

            if (affectCount > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("TagId", tagId);
            }

            return affectCount;
        }

        /// <summary>
        /// 清除关联的标签
        /// </summary>
        /// <remarks>会删除双向的关联关系</remarks>
        /// <param name="relatedTagId">关联的标签Id</param>
        /// <param name="tagId">被关联的标签Id</param>
        public int DeleteRelatedTagFromTag(long relatedTagId, long tagId)
        {
            List<Sql> sqls = new List<Sql>();
            sqls.Add(PetaPoco.Sql.Builder.Append("DELETE FROM tn_RelatedTags WHERE (TagId = @0) AND (RelatedTagId = @1)", relatedTagId, tagId));
            sqls.Add(PetaPoco.Sql.Builder.Append("DELETE FROM tn_RelatedTags WHERE (TagId = @0) AND (RelatedTagId = @1)", tagId, relatedTagId));

            int affectCount = CreateDAO().Execute(sqls);

            if (affectCount > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("TagId", tagId);
            }

            return affectCount;
        }

        /// <summary>
        /// 清除所有相关标签
        /// </summary>
        /// <param name="tagId">被关联的标签Id</param>
        public int ClearRelatedTagsFromTag(long tagId)
        {
            var sqls = Sql.Builder;
            sqls.Append("DELETE FROM tn_RelatedTags WHERE RelatedTagId = @0 or TagId = @0", tagId);
            int affectCount = CreateDAO().Execute(sqls);

            if (affectCount > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("TagId", tagId);
            }

            return affectCount;
        }

        /// <summary>
        /// 获取相关标签
        /// </summary>
        /// <param name="tagId">被关联的标签Id</param>
        /// <returns>获取相关联的Id集合</returns>
        public IEnumerable<long> GetRelatedTagIds(long tagId)
        {
            IEnumerable<RelatedTag> relatedTags = GetTopEntities(int.MaxValue, CachingExpirationType.ObjectCollection
            , () =>
            {
                //获取缓存
                StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TagId", tagId));
                cacheKey.AppendFormat("GetRelatedTagIds");
                return cacheKey.ToString();
            }
            , () =>
            {
                var sql = Sql.Builder;
                sql.Where("TagId = @0", tagId);
                return sql;
            });
            return relatedTags.Where(n => n != null).Select(n => n.RelatedTagId);
        }
    }
}