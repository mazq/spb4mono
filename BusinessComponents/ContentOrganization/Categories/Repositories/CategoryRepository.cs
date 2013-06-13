//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Caching;
using Tunynet.Utilities;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 分类仓储的具体实现类
    /// </summary>
    public class CategoryRepository<T> : Repository<T>, ICategoryRepository<T> where T : Category
    {
        
        //回复：在完成分页数据配置后，进行统一调整
        private int pageSize = 20;

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="category">待创建分类</param>
        /// <returns></returns>
        public override object Insert(T category)
        {
            PetaPocoDatabase dao = CreateDAO();

            dao.OpenSharedConnection();
            long primaryKey = Convert.ToInt64(dao.Insert(category));

            if (primaryKey > 0)
            {
                List<Sql> sqls = new List<Sql>();
                if (category.ParentId > 0)
                {
                    sqls.Add(Sql.Builder.Append("Update tn_Categories set ChildCount = ChildCount + 1 Where CategoryId = @0", category.ParentId));
                }

                sqls.Add(Sql.Builder.Append("update tn_Categories set DisplayOrder = @0 where CategoryId = @0", category.CategoryId));

                int affectCount = dao.Execute(sqls);
                category.DisplayOrder = category.CategoryId;

                base.OnInserted(category);
            }

            dao.CloseSharedConnection();

            return primaryKey;
        }

        /// <summary>
        /// 更新类别，注意：不能更新ParentId属性！
        /// </summary>
        /// <param name="category">待更新的类别</param>
        public void Update(T category)
        {
            //声明PetaPoco的SqlBuilder
            var sql = PetaPoco.Sql.Builder;

            //组装sql
            sql.Append("update tn_Categories  set CategoryName = @0 ,Description =@1,DisplayOrder=@2,Depth=@3,ChildCount=@4,ItemCount=@5,PrivacyStatus=@6,AuditStatus=@7,FeaturedItemId=@8,LastModified=@9,PropertyNames=@10,PropertyValues=@11", category.CategoryName, category.Description, category.DisplayOrder, category.Depth, category.ChildCount, category.ItemCount, category.PrivacyStatus, category.AuditStatus, category.FeaturedItemId, category.LastModified, category.PropertyNames, category.PropertyValues)
                .Where("CategoryId = @0", category.CategoryId);

            //执行语句
            int effectLineCount = CreateDAO().Execute(sql);

            #region 处理缓存

            RealTimeCacheHelper.IncreaseEntityCacheVersion(category);

            #endregion
        }

        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="ids">评论Id列表</param>
        /// <param name="auditingStatus">审核状态</param>
        public void UpdateAuditStatus(IEnumerable<long> ids, AuditStatus auditStatus)
        {
            //声明PetaPoco的SqlBuilder
            var sql = PetaPoco.Sql.Builder;

            //组装sql
            sql.Append("update tn_Categories set AuditStatus =@0", (int)auditStatus)
                .Where("CategoryId in (@ids)", new { ids = ids });

            //执行语句
            int effectLineCount = CreateDAO().Execute(sql);

            #region 处理缓存

            if (effectLineCount > 0)
            {
                //更新实体缓存
                foreach (long id in ids)
                {
                    T category = Get(id);
                    if (category != null)
                    {
                        category.AuditStatus = auditStatus;
                    }
                    RealTimeCacheHelper.IncreaseEntityCacheVersion(id);
                }

                //处理全局缓存 - 后台管理，需要即时显示
                RealTimeCacheHelper.IncreaseGlobalVersion();
            }
            #endregion

        }

        /// <summary>
        /// 根据userid删除访用户类别
        /// </summary>
        /// <param name="userId">用户的id</param>
        public void CleanByUser(long userId)
        {
            var sql_Delete = PetaPoco.Sql.Builder;
            sql_Delete.Append("delete from tn_Categories where OwnerId = @0", userId);            
            CreateDAO().Execute(sql_Delete);
        }

        /// <summary>
        /// 从fromCategoryId并入到toCategoryId
        /// </summary>
        /// <remarks>
        /// 例如：将分类fromCategoryId合并到分类toCategoryId，那么fromCategoryId分类下的所有子分类和实体全部归到toCategoryId分类，同时删除fromCategoryId分类
        /// </remarks>
        /// <param name="fromCategoryId">源类别</param>
        /// <param name="toCategoryId">目标类别</param>
        public void Merge(long fromCategoryId, long toCategoryId)
        {
            //为处理缓存做准备
            T fromCategory = Get(fromCategoryId);

            #region 组装sql

            //1 将所有的子分类合并到toCategoryId下
            var sql_ChildCategoryUpdate = PetaPoco.Sql.Builder;
            sql_ChildCategoryUpdate.Append("update tn_Categories set ParentId = @0", toCategoryId).Where("ParentId = @0", fromCategoryId);

            //2 将fromCategoryId下的所有直属实体类合并到toCategoryId下
            var sql_ChildItemsUpdate = PetaPoco.Sql.Builder;
            sql_ChildItemsUpdate.Append("update tn_ItemsInCategories set CategoryId = @0", toCategoryId).Where("CategoryId = @0", fromCategoryId);

            //3 更新后代分类深度
            //获得所有fromCategory的后代Category
            T toCategory = Get(toCategoryId);
            IList<T> fromChildCategories = new List<T>();
            IEnumerable<T> ownerCategories = GetOwnerCategories(toCategory.OwnerId, toCategory.TenantTypeId);
            this.RecurseGetChildren(fromCategory, fromChildCategories, ownerCategories.ToList());

            //循环更新fromCategory每一个后代Category的Depth
            IList<Sql> sql_UpdateFromChildCategoryDepths = new List<Sql>();
            foreach (T fromChildCategory in fromChildCategories)
            {
                int fromChildCategoryDepth = toCategory.Depth + fromChildCategory.Depth - fromCategory.Depth;
                fromChildCategory.Depth = fromChildCategoryDepth;
                sql_UpdateFromChildCategoryDepths.Add(Sql.Builder.Append("update tn_Categories set Depth = @0", fromChildCategoryDepth).Where("CategoryId=@0", fromChildCategory.CategoryId));
            }

            //4 修改toCategory 的属性：ChildCount，ItemCount
            //见下：sql_ToCategoryChildCountUpdate

            //5 最后删除fromCategory
            var sql_FromCategoryDelete = PetaPoco.Sql.Builder;
            sql_FromCategoryDelete.Append("delete tn_Categories").Where("CategoryId = @0", fromCategoryId);

            //6 修改原先from的父级分类的子分类数减一
            var sql_FromParentCategoryChildCountUpdate = PetaPoco.Sql.Builder;
            if (fromCategory.ParentId > 0)
                sql_FromParentCategoryChildCountUpdate.Append("update tn_Categories set ChildCount = ChildCount-1 ").Where("ChildCount > 0 and CategoryId = @0", fromCategory.ParentId);

            #endregion

            int childCategoryCount = 0/*子分类数*/,
                childItemCount = 0/*内容数*/,
                updateToCategoryCount = 0,
                deleteEffectLineCount = 0,
                affectCount = 0,
                updateDepthCount = 0;

            PetaPocoDatabase dao = CreateDAO();
            //在同一个事务中执行
            using (var scope = dao.GetTransaction())
            {
                //1
                childCategoryCount = dao.Execute(sql_ChildCategoryUpdate);
                //2
                childItemCount = dao.Execute(sql_ChildItemsUpdate);
                //3
                var sql_ToCategoryChildCountUpdate = PetaPoco.Sql.Builder;
                sql_ToCategoryChildCountUpdate.Append("update tn_Categories set ChildCount =ChildCount + @0,ItemCount=ItemCount + @1", childCategoryCount, childItemCount).Where("CategoryId = @0", toCategoryId);
                updateToCategoryCount = dao.Execute(sql_ToCategoryChildCountUpdate);
                //4
                updateDepthCount = dao.Execute(sql_UpdateFromChildCategoryDepths);
                //5
                deleteEffectLineCount = dao.Execute(sql_FromCategoryDelete);
                //6
                if (fromCategory.ParentId > 0)
                    affectCount = dao.Execute(sql_FromParentCategoryChildCountUpdate);

                //事务结束
                scope.Complete();
            }

            #region 缓存处理

            //标记删除
            if (deleteEffectLineCount > 0)
            {
                RealTimeCacheHelper.MarkDeletion(fromCategory);
            }

            if (updateToCategoryCount > 0)
            {
                //实体缓存，toCategory
                RealTimeCacheHelper.IncreaseEntityCacheVersion(toCategoryId);

                //处理所有的列表缓存toCategory 相关的列表
                RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", toCategory.OwnerId);
                RealTimeCacheHelper.IncreaseAreaVersion("ParentId", toCategory.CategoryId);

                //处理全局缓存（后台管理）
                RealTimeCacheHelper.IncreaseGlobalVersion();
            }

            if (updateDepthCount > 0 && ownerCategories.Count() > 1)
            {
                foreach (var ownerCategory in ownerCategories)
                {
                    if (ownerCategory.ParentId == fromCategoryId)
                    {
                        ownerCategory.ParentId = toCategoryId;
                    }

                    RealTimeCacheHelper.IncreaseEntityCacheVersion(ownerCategory.CategoryId);
                }
            }

            //处理分类下的内容项的缓存
            if (childItemCount > 0)
            {
                //toCategory以及父级
                List<long> toCategoryParentsIds = new List<long>();
                RecurseGetParents(toCategoryId, toCategoryParentsIds);
                toCategoryParentsIds.Add(toCategoryId);
                foreach (long categoryId in toCategoryParentsIds)
                {
                    EntityData.ForType(typeof(ItemInCategory)).RealTimeCacheHelper.IncreaseAreaVersion("CategoryId", toCategoryId);
                }

                //fromCategory父级
                List<long> fromCategoryParentsIds = new List<long>();
                RecurseGetParents(fromCategoryId, fromCategoryParentsIds);
                foreach (long categoryId in fromCategoryParentsIds)
                {
                    EntityData.ForType(typeof(ItemInCategory)).RealTimeCacheHelper.IncreaseAreaVersion("CategoryId", toCategoryId);
                }
            }

            #endregion
        }

        /// <summary>
        /// 把fromCategoryId移动到toCategoryId
        /// </summary>
        /// <remarks>
        /// 将一个分类移动到另一个分类，并作为另一个分类的子分类
        /// </remarks>
        /// <param name="fromCategoryId">被移动类别</param>
        /// <param name="toCategoryId">目标类别</param>
        public void Move(long fromCategoryId, long toCategoryId)
        {
            //初始分类实体
            T fromCategory = Get(fromCategoryId);

            //目标分类实体
            T toCategory = Get(toCategoryId);

            IList<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();

            //1 修改本分类的ParentID
            var sql_UpdateFromCategory = PetaPoco.Sql.Builder;
            sql_UpdateFromCategory.Append("update tn_Categories set ParentId = @0", toCategoryId).Where("CategoryId=@0", fromCategoryId);
            sqls.Add(sql_UpdateFromCategory);

            //2 更新本分类及所有后代分类的Depth
            //获得所有fromCategory的后代Category
            IList<T> fromChildCategories = new List<T>();
            IEnumerable<T> ownerCategories = GetOwnerCategories(toCategory.OwnerId, toCategory.TenantTypeId);
            this.RecurseGetChildren(fromCategory, fromChildCategories, ownerCategories.ToList());

            //更新fromCategory的Depth
            int fromCategoryDepth = toCategory.Depth + 1;

            var sql_UpdateFromCategoryDepth = PetaPoco.Sql.Builder;
            sql_UpdateFromCategoryDepth.Append("update tn_Categories set Depth = @0", fromCategoryDepth).Where("CategoryId=@0", fromCategory.CategoryId);
            sqls.Add(sql_UpdateFromCategoryDepth);

            //循环更新fromCategory每一个后代Category的Depth
            foreach (T fromChildCategory in fromChildCategories)
            {
                int fromChildCategoryDepth = fromCategoryDepth + fromChildCategory.Depth - fromCategory.Depth;
                fromChildCategory.Depth = fromChildCategoryDepth;
                var sql_UpdateFromChildCategoryDepth = PetaPoco.Sql.Builder;
                sql_UpdateFromChildCategoryDepth.Append("update tn_Categories set Depth = @0", fromChildCategoryDepth).Where("CategoryId=@0", fromChildCategory.CategoryId);
                sqls.Add(sql_UpdateFromChildCategoryDepth);
            }

            //3 toCategory的ChildCount+1
            var sql_UpdateToCategory = PetaPoco.Sql.Builder;
            sql_UpdateToCategory.Append("update tn_Categories set ChildCount = ChildCount+1 ").Where("CategoryId=@0", toCategoryId);
            sqls.Add(sql_UpdateToCategory);

            //4 fromCategory的Parent的ChildCount-1
            var sql_UpdateFromParentCategory = PetaPoco.Sql.Builder;
            if (fromCategory.ParentId > 0)
            {
                sql_UpdateFromParentCategory.Append("update tn_Categories set ChildCount = ChildCount-1 ").Where("ChildCount > 0 and CategoryId = @0", fromCategory.ParentId);
                sqls.Add(sql_UpdateFromParentCategory);
            }

            //影响的行数
            int effectLineCount = 0;

            PetaPocoDatabase dao = CreateDAO();
            //使用事务
            using (var scope = dao.GetTransaction())
            {
                effectLineCount = dao.Execute(sqls);

                //事务结束
                scope.Complete();
            }

            #region 处理缓存

            if (effectLineCount > 0)
            {
                //单体缓存fromCategory，toCategory，fromParentCategory
                RealTimeCacheHelper.IncreaseEntityCacheVersion(fromCategoryId);
                RealTimeCacheHelper.IncreaseEntityCacheVersion(toCategoryId);
                if (fromCategory.ParentId > 0)
                    RealTimeCacheHelper.IncreaseEntityCacheVersion(fromCategory.ParentId);
                fromCategory.ParentId = toCategory.CategoryId;
                fromCategory.Depth = toCategory.Depth + 1;

                //列表缓存
                RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", toCategory.OwnerId);//该用户的缓存
                RealTimeCacheHelper.IncreaseAreaVersion("ParentId", toCategory.ParentId);//父级分类列表缓存
                RealTimeCacheHelper.IncreaseAreaVersion("ParentId", toCategory.CategoryId);//本级分类列表缓存
                RealTimeCacheHelper.IncreaseAreaVersion("ParentId", fromCategory.CategoryId);

                if (ownerCategories.Count() > 1)
                {
                    foreach (var ownerCategory in ownerCategories)
                    {
                        RealTimeCacheHelper.IncreaseEntityCacheVersion(ownerCategory.CategoryId);
                    }
                }


                //全局缓存
                RealTimeCacheHelper.IncreaseGlobalVersion();

                //处理内容项对应的缓存
                //toCategory以及父级
                List<long> toCategoryParentsIds = new List<long>();
                RecurseGetParents(toCategoryId, toCategoryParentsIds);
                toCategoryParentsIds.Add(toCategoryId);
                foreach (long categoryId in toCategoryParentsIds)
                {
                    EntityData.ForType(typeof(ItemInCategory)).RealTimeCacheHelper.IncreaseAreaVersion("CategoryId", toCategoryId);
                }

                //fromCategory父级
                List<long> fromCategoryParentsIds = new List<long>();
                RecurseGetParents(fromCategoryId, fromCategoryParentsIds);
                foreach (long categoryId in fromCategoryParentsIds)
                {
                    EntityData.ForType(typeof(ItemInCategory)).RealTimeCacheHelper.IncreaseAreaVersion("CategoryId", toCategoryId);
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取拥有者的类别列表
        /// </summary>
        /// <param name="ownerId">类别拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>按树状排序的</returns>
        public IEnumerable<T> GetOwnerCategories(long ownerId, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
            cacheKey.AppendFormat("tenantTypeId-{0}", tenantTypeId);

            #region 1 从数据库取出全部该用户的该应用的分类

            IEnumerable<T> orgCategories =
            GetTopEntities(int.MaxValue, Caching.CachingExpirationType.ObjectCollection,
            () =>
            {
                //获取缓存
                return cacheKey.ToString();
            },
            () =>
            {
                //组装获取实体的sql语句
                var sql = PetaPoco.Sql.Builder;

                //注意：从数据库中取出来的数据是按照DisplayOrder正排序，这样就不需要再进行排序了
                sql.Where("OwnerId =@0 and TenantTypeId=@1 ", ownerId, tenantTypeId)
                    .OrderBy("Depth asc, DisplayOrder asc, CategoryId asc");

                return sql;
            }
           );

            #endregion

            //将IEnumerable<T> 转换成IList有助于下面的操作
            IList<T> orgCategoriesList = new List<T>(orgCategories);
            if (orgCategoriesList == null || orgCategoriesList.Count < 1)
                return orgCategoriesList;

            #region 2 取出所有的顶级节点

            IList<T> topCategories = new List<T>();
            foreach (T category in orgCategories)
            {
                if (category != null && category.ParentId == 0)
                {
                    //顶级节点取出
                    topCategories.Add(category);
                    //将取出的节点从列表中移除
                    orgCategoriesList.Remove(category);
                }
            }

            #endregion

            #region 3 使用递归组织每个分类下的所有分类

            IList<T> treeCategories = new List<T>();
            foreach (T category in topCategories)
            {
                if (category == null)
                    continue;

                treeCategories.Add(category);
                //递归获取所有的子分类
                RecurseGetChildren(category, treeCategories, orgCategoriesList);
            }

            #endregion

            #region 4 将数据放入缓存 - 缓存分区：OwnerId

            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            cacheService.Set(cacheKey.ToString(), treeCategories, CachingExpirationType.UsualObjectCollection);

            #endregion

            return treeCategories;
        }

        /// <summary>
        /// 获取子类别
        /// </summary>
        /// <param name="parentCategoryId">父类别Id</param>
        public IEnumerable<T> GetChildren(long parentCategoryId)
        {
            return GetTopEntities(int.MaxValue, Caching.CachingExpirationType.ObjectCollection,
                () =>
                {
                    //获取缓存
                    StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ParentId", parentCategoryId));
                    return cacheKey.ToString();
                },
                () =>
                {
                    //组装获取实体的sql语句
                    var sql = PetaPoco.Sql.Builder;

                    sql.Where(" ParentId = @0", parentCategoryId)
                        .OrderBy("DisplayOrder asc");

                    return sql;
                }
            );
        }

        /// <summary>
        /// 分页检索类别
        /// </summary>
        /// <returns>
        /// 按创建时间倒序排列的分页类别列表
        /// </returns>
        public PagingDataSet<T> GetCategories(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, string keyword, int pageIndex)
        {
            var sql = PetaPoco.Sql.Builder;
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Where("CategoryName like @0", "%" + keyword + "%");
            }

            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                sql.Where("TenantTypeId = @0", tenantTypeId);
            }

            if (publiclyAuditStatus.HasValue)
            {
                sql.Where("AuditStatus = @0", (int)publiclyAuditStatus);
            }

            sql.OrderBy("CategoryId desc");

            return GetPagingEntities(pageSize, pageIndex, sql);
        }

        /// <summary>
        /// 分页检索用户类别（OwnerId<>0）
        /// </summary>
        /// <returns>
        /// 按创建时间倒序排列的分页用户类别列表
        /// </returns>
        public PagingDataSet<T> GetOwnerCategories(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, string keyword, long ownerId = 0, int pageSize = 20, int pageIndex = 1)
        {
            var sql = PetaPoco.Sql.Builder;
            if (!string.IsNullOrEmpty(keyword))
            {
                //防sql注入
                keyword = StringUtility.StripSQLInjection(keyword);
                sql.Where("CategoryName like @0", "%" + keyword + "%");
            }

            if (!string.IsNullOrEmpty(tenantTypeId))
            {
                sql.Where("TenantTypeId = @0", tenantTypeId);
            }

            if (publiclyAuditStatus.HasValue)
            {
                sql.Where("AuditStatus = @0", (int)publiclyAuditStatus);
            }
            sql.Where("OwnerId > 0");
            if (ownerId > 0)
            {
                sql.Where("OwnerId = @0", ownerId);
            }

            sql.OrderBy("CategoryId desc");

            return GetPagingEntities(pageSize, pageIndex, sql);
        }

        /// <summary>
        /// 仅更新实体属性
        /// </summary>
        /// <param name="category">要更新的分类</param>
        public void UpdateItemCount(T category)
        {
            //声明PetaPoco的SqlBuilder
            var sql = PetaPoco.Sql.Builder;

            //组装sql
            sql.Append("update tn_Categories set ItemCount =@0", category.ItemCount)
                .Where("CategoryId = @0", category.CategoryId);

            //执行语句
            int effectLineCount = CreateDAO().Execute(sql);

            //更新缓存
            if (effectLineCount > 0)
                RealTimeCacheHelper.IncreaseEntityCacheVersion(category.CategoryId);
        }

        #region Help Method

        /// <summary>
        /// 获取所有子分类的递归方法
        /// </summary>
        /// <param name="category">当前分类</param>
        /// <param name="treeCategories">最终要组装的Tree分类</param>
        /// <param name="orgCategoriesList">原始分类列表</param>
        public void RecurseGetChildren(T category, IList<T> treeCategories, IList<T> orgCategoriesList)
        {
            //获取直属子分类
            IList<T> childCategories = orgCategoriesList.Where(n => n.ParentId.Equals(category.CategoryId)).ToList<T>();

            //子分类为null或者记录数小于1则返回
            if (childCategories == null || childCategories.Count < 1)
                return;

            //获取子分类的的子分类
            foreach (T childCategory in childCategories)
            {
                if (childCategory == null)
                    continue;

                //将该分类加入到列表
                treeCategories.Add(childCategory);

                //递归获取所有子分类
                RecurseGetChildren(childCategory, treeCategories, orgCategoriesList);
            }

        }

        /// <summary>
        /// 递归获取所有父级分类
        /// </summary>
        /// <param name="categoryId">当前分类Id</param>
        /// <param name="parentCategoryIdList">父级分类Id列表</param>
        public void RecurseGetParents(long categoryId, List<long> parentCategoryIdList)
        {
            if (categoryId > 0)
            {
                T category = Get(categoryId);
                if (category != null && category.ParentId > 0)
                {
                    parentCategoryIdList.Add(category.ParentId);
                    RecurseGetParents(category.ParentId, parentCategoryIdList);
                }
            }
        }


        #endregion


    }
}
