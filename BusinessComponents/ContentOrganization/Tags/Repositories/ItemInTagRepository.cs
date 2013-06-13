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

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///标签与内容项关联的仓储实现
    /// </summary>
    public class ItemInTagRepository : Repository<ItemInTag>, IItemInTagRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 为多个内容项添加相同标签
        /// </summary>
        /// <param name="itemIds">内容项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tagName">标签名</param>
        public int AddItemsToTag(IEnumerable<long> itemIds, string tenantTypeId, long ownerId, string tagName)
        {
            PetaPocoDatabase dao = CreateDAO();

            IList<Sql> sqls = new List<Sql>();
            var sql = Sql.Builder;

            dao.OpenSharedConnection();
            //创建标签
            sql.From("tn_Tags")
               .Where("TenantTypeId = @0", tenantTypeId)
               .Where("TagName = @0", tagName);
            var tag = dao.FirstOrDefault<Tag>(sql);
            if (tag == null)
                sqls.Add(Sql.Builder.Append("insert into tn_Tags (TenantTypeId,TagName,DateCreated) values(@0,@1,@2)", tenantTypeId, tagName, DateTime.UtcNow));
            //创建标签与用户的关联
            sql = Sql.Builder;
            sql.From("tn_TagsInOwners")
              .Where("TenantTypeId = @0", tenantTypeId)
              .Where("TagName = @0", tagName)
              .Where("OwnerId = @0", ownerId);
            var tagInOwner = dao.FirstOrDefault<TagInOwner>(sql);
            if (tagInOwner == null)
                sqls.Add(Sql.Builder.Append("insert into tn_TagsInOwners (TenantTypeId,TagName,OwnerId) values(@0,@1,@2)", tenantTypeId, tagName, ownerId));

            int affectCount = 0, itemCount = 0;
            foreach (var itemId in itemIds)
            {
                if (itemId <= 0)
                    continue;

                //创建标签与内容项的关联
                sqls.Add(Sql.Builder.Append("insert into tn_ItemsInTags (TagName,TagInOwnerId,ItemId,TenantTypeId) select @0,Id,@1,@2 from tn_TagsInOwners", tagName, itemId, tenantTypeId)
                            .Where("TenantTypeId = @0", tenantTypeId)
                            .Where("OwnerId = @0", ownerId)
                            .Where("TagName = @0", tagName));

                itemCount++;
            }
            //增加标签相关统计
            sqls.Add(Sql.Builder.Append("update tn_Tags Set ItemCount = ItemCount + @2,OwnerCount = OwnerCount + 1 where TenantTypeId = @0 and TagName = @1", tenantTypeId, tagName, itemCount));
            //增加拥有者标签内容项统计
            sqls.Add(Sql.Builder.Append("update tn_TagsInOwners set ItemCount = ItemCount + @3 where TenantTypeId = @0 and TagName = @1 and OwnerId = @2", tenantTypeId, tagName, ownerId, itemCount));

            //通过事务来控制多条语句执行时的一致性
            using (var transaction = dao.GetTransaction())
            {
                dao.Execute(sqls);
                transaction.Complete();
            }

            if (affectCount > 0)
            {
                foreach (var itemId in itemIds)
                {
                    if (itemId <= 0)
                        continue;

                    RealTimeCacheHelper.IncreaseAreaVersion("ItemId", itemId);
                }

                sql = Sql.Builder;
                sql.Select("*")
                   .From("tn_TagsInOwners")
                   .Where("TenantTypeId = @0", tenantTypeId)
                   .Where("TagName = @0", tagName)
                   .Where("OwnerId = @0", ownerId);

                tagInOwner = dao.First<TagInOwner>(sql);

                EntityData entityData = EntityData.ForType(typeof(TagInOwner));
                if (tagInOwner != null)
                {
                    entityData.RealTimeCacheHelper.IncreaseEntityCacheVersion(tagInOwner.Id);
                }

                entityData.RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", ownerId);
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 为内容项批量设置标签
        /// </summary>
        /// <param name="tagNames">标签名称集合</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="itemId">内容项Id</param>
        public int AddTagsToItem(string[] tagNames, string tenantTypeId, long ownerId, long itemId)
        {
            int affectCount = 0;

            PetaPocoDatabase dao = CreateDAO();
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
                    sqls.Add(Sql.Builder.Append("insert into tn_Tags (TenantTypeId,TagName,DateCreated) values (@0,@1,@2)", tenantTypeId, tagName, DateTime.UtcNow));
                //增加标签相关统计
                sqls.Add(Sql.Builder.Append("update tn_Tags Set ItemCount = ItemCount + 1 where TenantTypeId = @0 and TagName = @1", tenantTypeId, tagName));
                //创建标签与用户的关联
                sql = Sql.Builder;
                sql.From("tn_TagsInOwners")
                   .Where("TenantTypeId = @0", tenantTypeId)
                   .Where("TagName = @0", tagName)
                   .Where("OwnerId = @0", ownerId);
                var tagInOwner = dao.FirstOrDefault<TagInOwner>(sql);
                if (tagInOwner == null)
                {
                    sqls.Add(Sql.Builder.Append("update tn_Tags Set OwnerCount = OwnerCount + 1 where TenantTypeId = @0 and TagName = @1", tenantTypeId, tagName));
                    dao.Execute(Sql.Builder.Append("insert into tn_TagsInOwners (TenantTypeId,TagName,OwnerId) values (@0,@1,@2)", tenantTypeId, tagName, ownerId));
                    tagInOwner = dao.FirstOrDefault<TagInOwner>(sql);
                }
                else
                {
                    //增加拥有者标签内容项统计
                    sqls.Add(Sql.Builder.Append("update tn_TagsInOwners set ItemCount = ItemCount + 1 where TenantTypeId = @0 and TagName = @1 and OwnerId = @2", tenantTypeId, tagName, ownerId));
                }

                sql = Sql.Builder;
                sql.From("tn_ItemsInTags")
                   .Where("TagName = @0", tagName)
                   .Where("TagInOwnerId = @0", tagInOwner.Id)
                   .Where("ItemId = @0", itemId);

                var itemInTag = dao.FirstOrDefault<ItemInTag>(sql);
                if (itemInTag == null)
                {
                    //创建标签与内容项的关联
                    sqls.Add(Sql.Builder.Append("insert into tn_ItemsInTags (TagName,TagInOwnerId,ItemId,tenantTypeId) select @0,Id,@1,@2 from tn_TagsInOwners", tagName, itemId, tenantTypeId)
                                        .Where("TenantTypeId = @0", tenantTypeId)
                                        .Where("TagName = @0", tagName)
                                        .Where("OwnerId = @0", ownerId));
                }

                //通过事务来控制多条语句执行时的一致性
                using (var transaction = dao.GetTransaction())
                {
                    affectCount = dao.Execute(sqls);
                    transaction.Complete();
                }

                if (affectCount > 0)
                {
                    sql = Sql.Builder;
                    sql.From("tn_TagsInOwners")
                       .Where("TenantTypeId = @0", tenantTypeId)
                       .Where("TagName = @0", tagName)
                       .Where("OwnerId = @0", ownerId);

                    tagInOwner = dao.First<TagInOwner>(sql);
                    if (tagInOwner != null)
                    {
                        EntityData.ForType(typeof(TagInOwner)).RealTimeCacheHelper.IncreaseEntityCacheVersion(tagInOwner.Id);
                    }
                }
            }

            if (tagNames.Length > 0)
            {
                EntityData.ForType(typeof(TagInOwner)).RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", ownerId);
                RealTimeCacheHelper.IncreaseAreaVersion("ItemId", itemId);
                foreach (var tagName in tagNames)
                {
                    if (string.IsNullOrEmpty(tagName))
                        continue;
                    RealTimeCacheHelper.IncreaseAreaVersion("TagName", tagName);
                }
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 删除标签与成员的关系实体
        /// </summary>
        /// <param name="entity">待处理的实体</param>
        /// <returns></returns>
        public override int Delete(ItemInTag entity)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            int affectCount = base.Delete(entity);
            if (entity != null && affectCount > 0)
            {
                List<Sql> sqls = new List<Sql>();
                sqls.Add(Sql.Builder.Append("update tn_TagsInOwners set ItemCount = ItemCount - 1")
                            .Where("ItemCount > 0 and Id = @0", entity.TagInOwnerId));
                sqls.Add(Sql.Builder.Append("update tn_Tags set ItemCount = ItemCount - 1")
                            .Where("ItemCount > 0 and TagName = @0 and TenantTypeId = @1", entity.TagName, entity.TenantTypeId));

                affectCount = dao.Execute(sqls);

                RealTimeCacheHelper.IncreaseAreaVersion("TagName", entity.TagName);
                RealTimeCacheHelper.IncreaseAreaVersion("ItemId", entity.ItemId);
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 删除内容项的标签
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="tagInOwnerId">标签与拥有者关联</param>
        public int DeleteTagFromItem(long itemId, long tagInOwnerId)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            var sql = Sql.Builder;
            sql.Select("TagName,")
               .From("tn_ItemsInTags")
               .Where("tagInOwnerId = @0", tagInOwnerId)
               .Where("ItemId = @0", itemId);

            dynamic tagInfo = dao.First<dynamic>(sql);
            int affectCount = 0;

            if (tagInfo != null)
            {
                sql = Sql.Builder.Append("delete from tn_ItemsInTags where itemId = @0 and TagInOwnerId = @1", itemId, tagInOwnerId);

                affectCount = dao.Execute(sql);

                if (affectCount > 0)
                {
                    List<Sql> sqls = new List<Sql>();
                    sqls.Add(Sql.Builder.Append("update tn_TagsInOwners set ItemCount = ItemCount - 1")
                                .Where("ItemCount > 0 and Id = @0", tagInOwnerId));
                    sqls.Add(Sql.Builder.Append("update tn_Tags set ItemCount = ItemCount - 1")
                                .Where("ItemCount > 0 and TagName = @0 and TenantTypeId = @1", tagInfo.TagName, tagInfo.TenantTypeId));

                    affectCount = dao.Execute(sqls);

                    RealTimeCacheHelper.IncreaseAreaVersion("TagName", tagInfo.TagName);
                    RealTimeCacheHelper.IncreaseAreaVersion("ItemId", itemId);
                }
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 清除内容项的所有标签
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        public int ClearTagsFromItem(long itemId, string tenantTypeId, long ownerId)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            var sql = PetaPoco.Sql.Builder;
            sql.Select("IT.TagName")
               .From("tn_ItemsInTags IT")
               .InnerJoin("tn_TagsInOwners TIO")
               .On("IT.TagInOwnerId = TIO.Id")
               .Where("TIO.TenantTypeId = @0", tenantTypeId)
               .Where("TIO.OwnerId = @0", ownerId)
               .Where("IT.ItemId = @0", itemId);

            List<string> tagNames = dao.Fetch<string>(sql);

            sql = Sql.Builder;
            sql.Append("delete from tn_ItemsInTags where ItemId = @0 and exists(select 1 from tn_TagsInOwners TIO where TIO.TenantTypeId = @1 and OwnerId = @2 and TIO.Id = TagInOwnerId)", itemId, tenantTypeId, ownerId);

            int affectCount = dao.Execute(sql);

            if (affectCount > 0)
            {
                List<Sql> sqls = new List<Sql>();
                foreach (string tagName in tagNames)
                {
                    sqls.Add(Sql.Builder.Append("update tn_TagsInOwners set ItemCount = ItemCount - 1")
                                        .Where("ItemCount > 0 and OwnerId = @0 and TagName = @1 and TenantTypeId = @2", ownerId, tagName, tenantTypeId));
                    sqls.Add(Sql.Builder.Append("update tn_Tags set ItemCount = ItemCount - 1")
                                        .Where("ItemCount > 0 and TagName = @0 and TenantTypeId = @1", tagName, tenantTypeId));

                    RealTimeCacheHelper.IncreaseAreaVersion("TagName", tagName);
                }

                dao.Execute(sqls);

                RealTimeCacheHelper.IncreaseAreaVersion("ItemId", itemId);
            }

            dao.CloseSharedConnection();

            return affectCount;
        }

        /// <summary>
        /// 获取标签的所有内容项集合
        /// </summary>
        /// <param name="tagName">标签名称</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>返回指定的内容项Id集合</returns>
        public IEnumerable<long> GetItemIds(string tagName, string tenantTypeId, long? ownerId)
        {
            PetaPocoDatabase dao = CreateDAO();
            //获取缓存
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TagName", tagName));
            cacheKey.AppendFormat(":ItemIds-TenantTypeId:{0}-OwnerId:{0}", tenantTypeId, ownerId.HasValue ? ownerId.ToString() : string.Empty);

            //组装sql语句
            var sql = PetaPoco.Sql.Builder;
            if (ownerId.HasValue && ownerId > 0)
            {
                sql.Select("ItemId")
                   .From("tn_ItemsInTags IT")
                   .InnerJoin("tn_TagsInOwners TIO")
                   .On("IT.TagInOwnerId = TIO.Id")
                   .Where("TIO.OwnerId = @0", ownerId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TIO.TenantTypeId = @0", tenantTypeId);
                if (!string.IsNullOrEmpty(tagName))
                    sql.Where("TIO.TagName = @0", tagName);
            }
            else
            {
                sql.Select("ItemId")
                   .From("tn_ItemsInTags");

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TenantTypeId = @0", tenantTypeId);
                if (!string.IsNullOrEmpty(tagName))
                    sql.Where("TagName = @0", tagName);
            }

            IEnumerable<long> itemIds = null;
            itemIds = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());
            if (itemIds == null)
            {
                itemIds = dao.FetchFirstColumn(sql).Cast<long>();
                cacheService.Add(cacheKey.ToString(), itemIds, CachingExpirationType.ObjectCollection);
            }

            return itemIds;
        }

        /// <summary>
        /// 获取标签的内容项集合
        /// </summary>
        /// <param name="tagName">标签名称</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页码(从1开始)</param>
        /// <returns>返回指定页码的内容项Id集合</returns>
        public PagingEntityIdCollection GetItemIds(string tagName, string tenantTypeId, long? ownerId, int pageSize, int pageIndex)
        {
            PetaPocoDatabase dao = CreateDAO();
            //获取缓存
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TagName", tagName));
            cacheKey.AppendFormat(":ItemIds-TenantTypeId:{0}-OwnerId:{0}", tenantTypeId, ownerId.HasValue ? ownerId.ToString() : string.Empty);

            //组装sql语句
            var sql = PetaPoco.Sql.Builder;
            if (ownerId.HasValue && ownerId > 0)
            {
                sql.Select("ItemId")
                   .From("tn_ItemsInTags IT")
                   .InnerJoin("tn_TagsInOwners TIO")
                   .On("IT.TagInOwnerId = TIO.Id")
                   .Where("TIO.OwnerId = @0", ownerId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TIO.TenantTypeId = @0", tenantTypeId);
                if (!string.IsNullOrEmpty(tagName))
                    sql.Where("TIO.TagName = @0", tagName);
            }
            else
            {
                sql.Select("ItemId")
                   .From("tn_ItemsInTags");

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TenantTypeId = @0", tenantTypeId);
                if (!string.IsNullOrEmpty(tagName))
                    sql.Where("TagName = @0", tagName);
            }


            PagingEntityIdCollection peic = null;
            if (pageIndex < CacheablePageCount)
            {
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey.ToString());
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "ItemId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey.ToString(), peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "ItemId", sql);
            }

            return peic;
        }

        /// <summary>
        /// 获取多个标签的内容项集合
        /// </summary>
        /// <param name="tagNames">标签名称列表</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页码(从1开始)</param>
        /// <returns>返回指定页码的内容项Id集合</returns>
        public PagingEntityIdCollection GetItemIds(IEnumerable<string> tagNames, string tenantTypeId, long? ownerId, int pageSize, int pageIndex)
        {
            //获取缓存
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TagNames", tagNames));
            cacheKey.AppendFormat(":ItemIds-TenantTypeId:{0}-OwnerId:{0}", tenantTypeId, ownerId.HasValue ? ownerId.ToString() : string.Empty);

            var sql = PetaPoco.Sql.Builder;
            if (ownerId.HasValue && ownerId > 0)
            {
                //组装sql语句
                sql.Select("ItemId")
                   .From("tn_ItemsInTags IT")
                   .InnerJoin("tn_TagsInOwners TIO")
                   .On("IT.TagInOwnerId = TIO.Id")
                   .Where("TIO.OwnerId = @0", ownerId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TIO.TenantTypeId = @0", tenantTypeId);
                if (tagNames != null && tagNames.Count() > 0)
                    sql.Where("TIO.TagName IN (@tagNames)", new { tagNames = tagNames });
            }
            else
            {
                //组装sql语句
                sql.Select("ItemId")
                   .From("tn_ItemsInTags");

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TenantTypeId = @0", tenantTypeId);
                if (tagNames != null && tagNames.Count() > 0)
                    sql.Where("TagName IN (@tagNames)", new { tagNames = tagNames });
            }

            PetaPocoDatabase dao = CreateDAO();
            PagingEntityIdCollection peic = null;
            if (pageIndex < CacheablePageCount)
            {
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey.ToString());
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "ItemId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey.ToString(), peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "ItemId", sql);
            }

            return peic;
        }

        /// <summary>
        /// 获取内容项的所有标签
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <returns>返回内容项的标签Id集合,无返回时返回空集合</returns>
        public IEnumerable<long> GetTagIdsOfItem(long itemId, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ItemId", itemId));
            cacheKey.Append("GetTagIdsOfItem");

            if (!string.IsNullOrEmpty(tenantTypeId))
                cacheKey.AppendFormat("-TenantTypeId:{0}", tenantTypeId);

            IEnumerable<long> tagIds = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());

            if (tagIds == null || tagIds.Count() == 0)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("T.TagId")
                   .From("tn_Tags T")
                   .InnerJoin("tn_TagsInOwners TIO")
                   .On("T.TagName = TIO.TagName and T.TenantTypeId = TIO.TenantTypeId")
                   .InnerJoin("tn_ItemsInTags IT")
                   .On("IT.TagInOwnerId = TIO.Id")
                   .Where("IT.ItemId = @0", itemId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TIO.TenantTypeId = @0", tenantTypeId);

                tagIds = CreateDAO().FetchFirstColumn(sql).Cast<long>();
                cacheService.Set(cacheKey.ToString(), tagIds, CachingExpirationType.UsualObjectCollection);
            }

            return tagIds;
        }

        /// <summary>
        /// 获取内容项与标签关联Id集合
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="tenantTypeId">租户类型</param>
        /// <returns>返回内容项的标签Id集合,无返回时返回空集合</returns>
        public IEnumerable<long> GetItemInTagIdsOfItem(long itemId, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ItemId", itemId));
            cacheKey.Append("GetItemInTagIdsOfItem");
            cacheKey.AppendFormat("-TenantTypeId:{0}", tenantTypeId);

            IEnumerable<long> itemInTagIds = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());

            if (itemInTagIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("IT.Id")
                   .From("tn_ItemsInTags IT")
                   .InnerJoin("tn_TagsInOwners TIO")
                   .On("IT.TagInOwnerId = TIO.Id")
                   .Where("IT.ItemId = @0", itemId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TIO.TenantTypeId = @0", tenantTypeId);

                itemInTagIds = CreateDAO().FetchFirstColumn(sql).Cast<long>();
                cacheService.Set(cacheKey.ToString(), itemInTagIds, CachingExpirationType.UsualObjectCollection);
            }

            return itemInTagIds;
        }

        /// <summary>
        /// 获取标签标签与拥有者关系Id集合
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<long> GetTagInOwnerIdsOfItem(long itemId, long ownerId, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
            cacheKey.AppendFormat("GetOwnerTagIdsOfItem-ItemId:{0}-TenantTypeId:{0}", itemId, tenantTypeId);

            IEnumerable<long> tagInOwnerIds = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());

            if (tagInOwnerIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("TIO.Id")
                   .From("tn_TagsInOwners TIO")
                   .InnerJoin("tn_ItemsInTags IT")
                   .On("IT.TagInOwnerId = TIO.Id")
                   .Where("IT.ItemId = @0", itemId)
                   .Where("TIO.OwnerId = @0", ownerId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TIO.TenantTypeId = @0", tenantTypeId);

                tagInOwnerIds = CreateDAO().FetchFirstColumn(sql).Cast<long>();
                cacheService.Set(cacheKey.ToString(), tagInOwnerIds, CachingExpirationType.UsualObjectCollection);
            }

            return tagInOwnerIds;
        }

        /// <summary>
        /// 根据用户ID列表获取用户tag，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTagNamesOfUsers(IEnumerable<long> userIds)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("ItemId as UserId,TagName")
                   .From("tn_ItemsInTags")
                   .Where("ItemId in (@userIds)", new { userIds = userIds });
            return CreateDAO().Fetch<dynamic>(sql);
        }

        /// <summary>
        /// 根据用户ID列表获取ItemInTag的ID列表，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns>ItemInTag的ID列表</returns>
        public IEnumerable<long> GetEntityIdsByUserIds(IEnumerable<long> userIds)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("Id")
                   .From("tn_ItemsInTags")
                   .Where("ItemId in (@userIds)", new { userIds = userIds });
            return CreateDAO().Fetch<long>(sql);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="itemId">成员Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="tagInOwnerId">标签与拥有者关联Id</param>
        /// <returns></returns>
        public Dictionary<string, long> GetTagNamesWithIdsOfItem(long itemId, string tenantTypeId, long? tagInOwnerId = null)
        {
            string cacheKey = string.Format("TagNamesWithIdsOfItem::ItemId:{0}-TenantTypeId:{1}-TagInOwnerId:{2}", itemId, tenantTypeId, tagInOwnerId);

            Dictionary<string, long> tagNames_Ids = cacheService.Get<Dictionary<string, long>>(cacheKey);

            if (tagNames_Ids == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("tn_Tags.TagName,tn_Tags.TagId")
                   .From("tn_Tags")
                   .InnerJoin("tn_ItemsInTags IIT")
                   .On("tn_Tags.TagName = IIT.TagName")
                   .Where("IIT.ItemId = @0", itemId)
                   .Where("tn_Tags.TenantTypeId = @0", tenantTypeId);

                if (tagInOwnerId.HasValue)
                    sql.Where("TagInOwnerId = @0", tagInOwnerId);

                IEnumerable<dynamic> results = CreateDAO().Fetch<dynamic>(sql);

                if (results != null)
                {
                    tagNames_Ids = results.ToDictionary<dynamic, string, long>(n => n.TagName, n => n.TagId);
                    cacheService.Add(cacheKey, tagNames_Ids, CachingExpirationType.ObjectCollection);
                }
            }

            return tagNames_Ids;
        }

    }
}