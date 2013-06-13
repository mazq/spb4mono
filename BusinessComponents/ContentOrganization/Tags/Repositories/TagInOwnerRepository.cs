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
using Tunynet.Utilities;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///标签与拥有者关联的仓储实现
    /// </summary>
    public class TagInOwnerRepository : Repository<TagInOwner>, ITagInOwnerRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 删除标签与拥有者关系
        /// </summary>
        /// <param name="entity">标签与拥有者关系实体</param>
        /// <returns>影响行数</returns>
        public override int Delete(TagInOwner entity)
        {
            PetaPocoDatabase dao = CreateDAO();

            IList<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();
            int affectCount = 0;

            dao.OpenSharedConnection();

            var sql_GetItemIds = PetaPoco.Sql.Builder;
            sql_GetItemIds.Select("ItemId")
                          .From("tn_ItemsInTags")
                          .Where("TenantTypeId = @0 and TagName = @1", entity.TenantTypeId, entity.TagName);

            IEnumerable<object> itemIds = dao.FetchFirstColumn(sql_GetItemIds);

            sqls.Add(PetaPoco.Sql.Builder.Append("delete from tn_TagsInOwners where id=@0", entity.Id));
            sqls.Add(PetaPoco.Sql.Builder.Append("delete from tn_ItemsInTags where TagInOwnerId = @0 and TagName = @1", entity.Id, entity.TagName));
            using (var transaction = dao.GetTransaction())
            {
                affectCount = dao.Execute(sqls);
                transaction.Complete();
            }

            if (affectCount > 0)
            {
                OnDeleted(entity);

                foreach (var itemId in itemIds)
                {
                    EntityData.ForType(typeof(ItemInTag)).RealTimeCacheHelper.IncreaseAreaVersion("ItemId", itemId);
                }

                EntityData.ForType(typeof(ItemInTag)).RealTimeCacheHelper.IncreaseAreaVersion("TagName", entity.TagName);
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 添加标签与拥有者关联
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>返回影响行数</returns>
        public int AddTagInOwner(string tagName, string tenantTypeId, long ownerId)
        {
            int affectCount = 0;

            if (!string.IsNullOrEmpty(tagName) && !string.IsNullOrEmpty(tenantTypeId) && ownerId >= 0)
            {
                PetaPocoDatabase dao = CreateDAO();
                dao.OpenSharedConnection();

                var sql = PetaPoco.Sql.Builder;
                sql.Select("count(*)")
                   .From("tn_TagsInOwners")
                   .Where("OwnerId = @0", ownerId)
                   .Where("TagName = @0", tagName)
                   .Where("TenantTypeId = @0", tenantTypeId);

                affectCount = dao.ExecuteScalar<int>(sql);
                if (affectCount == 0)
                {
                    sql = PetaPoco.Sql.Builder;
                    sql.Append("INSERT INTO tn_TagsInOwners (OwnerId, TagName, TenantTypeId) VALUES (@0,@1,@2)", ownerId, tagName, tenantTypeId);
                    affectCount = dao.Execute(sql);

                    if (affectCount > 0)
                    {
                        RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", ownerId);
                    }
                }
                else
                {
                    affectCount = 0;
                }

                dao.CloseSharedConnection();
            }

            return affectCount;
        }

        /// <summary>
        /// 添加标签与拥有者关联
        /// </summary>
        /// <param name="tagInOwner">待创建实体</param>
        /// <returns>返回实体主键</returns>
        public long AddTagInOwner(TagInOwner tagInOwner)
        {
            if (tagInOwner == null ||
                string.IsNullOrEmpty(tagInOwner.TagName) ||
                string.IsNullOrEmpty(tagInOwner.TenantTypeId) ||
                tagInOwner.OwnerId < 0)
            {
                return 0;
            }

            int affectCount = 0;
            long id = 0;

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            var sql = PetaPoco.Sql.Builder;
            sql.Select("count(*)")
               .From("tn_TagsInOwners")
               .Where("OwnerId = @0", tagInOwner.OwnerId)
               .Where("TagName = @0", tagInOwner.TagName)
               .Where("TenantTypeId = @0", tagInOwner.TenantTypeId);

            affectCount = dao.ExecuteScalar<int>(sql);
            if (affectCount == 0)
            {
                long.TryParse(base.Insert(tagInOwner).ToString(), out id);
            }

            dao.CloseSharedConnection();

            if (id > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", tagInOwner.OwnerId);
            }

            return id;
        }

        /// <summary>
        /// 清除拥有者的所有标签
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public int ClearTagsFromOwner(long ownerId, string tenantTypeId)
        {

            List<Sql> sqls = new List<Sql>();
            if (string.IsNullOrEmpty(tenantTypeId))
            {
                sqls.Add(Sql.Builder.Append("update tn_Tags set OwnerCount = OwnerCount - 1")
                                    .Where("OwnerCount > 0 and exists(select 1 from tn_TagsInOwners where TagName = tn_Tags.TagName and OwnerId = @0)", ownerId));
            }
            else
            {
                sqls.Add(Sql.Builder.Append("update tn_Tags set OwnerCount = OwnerCount - 1")
                                    .Where("OwnerCount > 0 and exists(select 1 from tn_TagsInOwners where TagName = tn_Tags.TagName and OwnerId = @0 and TenantTypeId = @1)", ownerId, tenantTypeId));
            }

            var sql = PetaPoco.Sql.Builder;
            sql.Append("delete from tn_TagsInOwners")
               .Where("OwnerId = @0", ownerId);

            if (!string.IsNullOrEmpty(tenantTypeId))
                sql.Where("TenantTypeId = @0", tenantTypeId);

            sqls.Add(sql);

            int affectCount = CreateDAO().Execute(sqls);
            if (affectCount > 0)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", ownerId);
            }

            return affectCount;
        }

        /// <summary>
        /// 获取拥有者的标签列表
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<TagInOwner> GetTagInOwners(long ownerId, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                cacheKey.AppendFormat("TenantTypeId-{0}", tenantTypeId);
            }

            IEnumerable<object> entityIds = cacheService.Get<IEnumerable<object>>(cacheKey.ToString());

            if (entityIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("*")
                .From("tn_TagsInOwners")
                .Where("OwnerId = @0", ownerId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TenantTypeId = @0", tenantTypeId);

                entityIds = CreateDAO().FetchFirstColumn(sql);
                cacheService.Add(cacheKey.ToString(), entityIds, CachingExpirationType.ObjectCollection);
            }

            return PopulateEntitiesByEntityIds(entityIds);
        }

        /// <summary>
        /// 获取拥有者的分页标签列表
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">页码</param>
        public IEnumerable<TagInOwner> GetTagInOwners(long ownerId, string tenantTypeId, int pageSize, int pageIndex)
        {

            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
                   () =>
                   {
                       StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
                       cacheKey.Append("TagInOwners");
                       if (!string.IsNullOrEmpty(tenantTypeId))
                       {
                           cacheKey.AppendFormat("TenantTypeId-{0}", tenantTypeId);
                       }
                       return cacheKey.ToString();
                   },
                   () =>
                   {
                       var sql = PetaPoco.Sql.Builder;
                       sql.Select("*")
                          .From("tn_TagsInOwners")
                          .Where("OwnerId = @0", ownerId);

                       if (string.IsNullOrEmpty(tenantTypeId))
                       {
                           sql.Where("TenantTypeId = @0", tenantTypeId);
                       }

                       return sql;
                   });
        }

        /// <summary>
        /// 分页获取tn_TagsInOwners表的数据(用于建索引)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagingDataSet<TagInOwner> GetTagInOwners(int pageIndex, int pageSize)
        {
            PagingDataSet<TagInOwner> tagInOwners = null;

            var sql = PetaPoco.Sql.Builder;
            sql.Select("*")
            .From("tn_TagsInOwners");

            tagInOwners = GetPagingEntities(pageSize, pageIndex, sql);

            return tagInOwners;
        }

        /// <summary>
        /// 获取拥有者的标签列表
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="keyword">标签关键字</param>
        /// <param name="topNumber">前N个标签</param>
        public IEnumerable<TagInOwner> GetTopTagInOwners(long ownerId, string tenantTypeId, string keyword, int topNumber)
        {
            IEnumerable<TagInOwner> tagInOwners = null;
            if (string.IsNullOrEmpty(keyword))
            {
                tagInOwners = GetTopEntities(topNumber, CachingExpirationType.ObjectCollection
             , () =>
             {
                 StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
                 if (!string.IsNullOrEmpty(tenantTypeId))
                 {
                     cacheKey.AppendFormat("TenantTypeId-{0}", tenantTypeId);
                 }
                 return cacheKey.ToString();
             }
             , () =>
             {
                 var sql = PetaPoco.Sql.Builder;
                 sql.Where("OwnerId = @0 and TenantTypeId = @1", ownerId, tenantTypeId);
                 return sql;
             });
            }
            else
            {
                var sql = PetaPoco.Sql.Builder;

                sql.Where("TagName like @0", "%" + StringUtility.StripSQLInjection(keyword) + "%");
                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("OwnerId = @0 and TenantTypeId = @1", ownerId, tenantTypeId);
                tagInOwners = PopulateEntitiesByEntityIds(CreateDAO().FetchTopPrimaryKeys<TagInOwner>(topNumber, sql));
            }

            return tagInOwners;
        }
    }
}