//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 分类业务逻辑类
    /// </summary>
    public class CategoryService<T> where T : Category
    {

        //Category、ItemInCategory Repository
        private ICategoryRepository<T> categoryRepository;
        private IItemInCategoryRepository itemInCategoryRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CategoryService()
            : this(new CategoryRepository<T>(), new ItemInCategoryRepository())
        {
        }

        /// <summary>
        /// 可设置repository的构造函数
        /// </summary>
        public CategoryService(ICategoryRepository<T> categoryRepository, IItemInCategoryRepository itemInCategoryRepository)
        {
            this.categoryRepository = categoryRepository;
            this.itemInCategoryRepository = itemInCategoryRepository;
        }

        #region Create & Update & Delete & Merge & Move

        /// <summary>
        /// 创建类别
        /// </summary>
        /// <param name="category">待创建的类别</param>
        /// <returns>创建成功返回true，否则返回false</returns>
        public bool Create(T category)
        {
            //写入数据库前，触发相关事件
            EventBus<T>.Instance().OnBefore(category, new CommonEventArgs(EventOperationType.Instance().Create()));

            //将数据插入数据库
            long categoryId = Convert.ToInt64(categoryRepository.Insert(category));

            //若插入数据成功，则触发数据入库后的相关事件
            if (categoryId > 0)
            {
                EventBus<T>.Instance().OnAfter(category, new CommonEventArgs(EventOperationType.Instance().Create()));
                return true;
            }
            return false;

        }

        /// <summary>
        /// 更新类别，注意：不能更新ParentId属性！
        /// </summary>
        /// <param name="category">待更新的类别</param>
        public void Update(T category)
        {
            //更新数据前，触发的相关事件
            EventBus<T>.Instance().OnBefore(category, new CommonEventArgs(EventOperationType.Instance().Update()));

            //更新到数据库
            categoryRepository.Update(category);

            //更新数据后，触发的相关事件
            EventBus<T>.Instance().OnAfter(category, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="categoryId">待删除类别Id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(long categoryId)
        {
            //首先获取要删除的分类，若获取不到则返回false
            T category = categoryRepository.Get(categoryId);
            if (category == null)
                throw new ExceptionFacade(string.Format("Id为:{0}的分类不存在", categoryId));

            //删除数据前，触发相关事件
            EventBus<T>.Instance().OnBefore(category, new CommonEventArgs(EventOperationType.Instance().Delete()));

            #region 删除分类

            //删除所有子分类以及子分类下面的内容
            if (category.ChildCount > 0)
            {
                IEnumerable<T> childCategories = GetDescendants(categoryId);
                foreach (T childCategoy in childCategories)
                {
                    Delete(childCategoy.CategoryId);
                }
            }

            //删除分类下的内容项
            ClearItemsFromCategory(categoryId);
            //从数据库删除数据
            bool isDeleted = (categoryRepository.Delete(category) > 0);

            #endregion

            //若删除成功，触发删除后相关事件
            if (isDeleted)
                EventBus<T>.Instance().OnAfter(category, new CommonEventArgs(EventOperationType.Instance().Delete()));

            #region 处理父级缓存

            Tunynet.Caching.RealTimeCacheHelper cacheHelper = EntityData.ForType(typeof(T)).RealTimeCacheHelper;
            if (category.ParentId > 0)
            {
                //处理实体缓存
                cacheHelper.IncreaseEntityCacheVersion(category.ParentId);
                //处理区域缓存
                cacheHelper.IncreaseAreaVersion("ParentId", category.ParentId);
            }

            #endregion

            return isDeleted;
        }

        /// <summary>
        /// 根据用户删除用户类别（删除用户时使用）
        /// </summary>
        public void CleanByUser(long userId)
        {
            categoryRepository.CleanByUser(userId);
        }


        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="ids">分类Id列表</param>
        /// <param name="auditingStatus">审核状态</param>
        public void UpdateAuditStatus(IEnumerable<long> ids, AuditStatus auditStatus)
        {
            //将集合统一设置（在一次数据库连接中）
            //直接调用，数据仓储的方法实现
            categoryRepository.UpdateAuditStatus(ids, auditStatus);
        }

        /// <summary>
        /// 从fromCategoryId并入到toCategoryId
        /// </summary>
        /// <remarks>
        /// 例如：将分类fromCategoryId合并到分类toCategoryId，那么fromCategoryId分类下的所有子分类和实体全部归到toCategoryId分类，同时删除fromCategoryId分类
        /// </remarks>
        /// <param name="fromCategoryId">合并分类源类别</param>
        /// <param name="toCategoryId">合并分类目标类别</param>
        public void Merge(long fromCategoryId, long toCategoryId)
        {
            #region 验证输入参数的有效性

            //会影响到数据层，判断输入参数的合理性
            if (fromCategoryId < 1 || toCategoryId < 1)
                throw new ExceptionFacade(string.Format("输入参数fromCategoryId-{0}，或者toCategoryId-{1}，不合理！", fromCategoryId, toCategoryId));

            //若formCategory，在数据库中不存在则返回
            T formCategory = categoryRepository.Get(fromCategoryId);
            if (formCategory == null)
                throw new ExceptionFacade("fromCategory不存在！");

            //若toCategory，在数据库中不存在则返回
            T toCategory = categoryRepository.Get(toCategoryId);
            if (toCategory == null)
                throw new ExceptionFacade("toCategory不存在！");

            if (GetDescendants(fromCategoryId).Select(n => n.CategoryId).ToList().Contains(toCategoryId)
                || fromCategoryId == toCategoryId)
                throw new ExceptionFacade("不能合并到指定分类！");

            #endregion

            //直接调用仓储提供的方法，处理缓存
            categoryRepository.Merge(fromCategoryId, toCategoryId);

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
            #region 验证输入参数的有效性

            //会影响到数据层，判断输入参数的合理性
            if (fromCategoryId < 1 || toCategoryId < 1)
                throw new ExceptionFacade(string.Format("输入参数fromCategoryId-{0}，或者toCategoryId-{1}，不合理！", fromCategoryId, toCategoryId));

            //若formCategory，在数据库中不存在则返回
            T formCategory = categoryRepository.Get(fromCategoryId);
            if (formCategory == null)
                throw new ExceptionFacade("fromCategory不存在！");

            //若toCategory，在数据库中不存在则返回
            T toCategory = categoryRepository.Get(toCategoryId);
            if (toCategory == null)
                throw new ExceptionFacade("toCategory不存在！");

            //父及分类不能合并到子分类中
            if (GetDescendants(fromCategoryId).Select(n => n.CategoryId).ToList().Contains(toCategoryId)
                || fromCategoryId == toCategoryId)
                throw new ExceptionFacade("不能移动到指定分类下！");

            #endregion

            //直接调用仓储提供的方法，处理缓存
            categoryRepository.Move(fromCategoryId, toCategoryId);
        }

        #endregion

        #region Get & Gets

        /// <summary>
        /// 获取Category
        /// </summary>
        /// <param name="categoryId">CategoryId</param>
        public T Get(long categoryId)
        {
            //根据标识获取实体
            return categoryRepository.Get(categoryId);
        }

        /// <summary>
        /// 获取拥有者的类别列表
        /// </summary>
        /// <param name="ownerId">类别拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>按树状排序的</returns>
        public IEnumerable<T> GetOwnerCategories(long ownerId, string tenantTypeId)
        {
            //1 从数据库取出全部该用户的该应用的分类
            //2 取出所有的顶级节点
            //3 使用递归组织每个分类下的所有分类,可以直接显示为tree
            //4 将数据放入缓存 - 缓存分区：OwnerId
            //注意：从数据库中取出来的数据是同级按照DisplayOrder正排序，这样就不需要再进行排序了

            return categoryRepository.GetOwnerCategories(ownerId, tenantTypeId);
        }

        /// <summary>
        /// 获取Onwer的所有根类别
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">OwnerId</param>
        /// <returns></returns>
        public IEnumerable<T> GetRootCategories(string tenantTypeId, long ownerId = 0)
        {
            IEnumerable<T> categories = GetOwnerCategories(ownerId, tenantTypeId);
            return categories.Where(n => n.ParentId == 0).ToList();
        }

        /// <summary>
        /// 获取子类别
        /// </summary>
        /// <param name="parentCategoryId">父类别Id</param>
        /// <returns></returns>
        public IEnumerable<T> GetChildren(long parentCategoryId)
        {
            //根据parentCategoryId获取所有的直属子分类的ID，并且按照DisplayOrder正序
            //获取后放入缓存，使用ParentId进行分区
            return categoryRepository.GetChildren(parentCategoryId);
        }

        /// <summary>
        /// 获取后代类别
        /// </summary>
        /// <param name="parentCategoryId">父类别Id</param>
        /// <returns></returns>
        public IEnumerable<T> GetDescendants(long parentCategoryId)
        {
            //获取当前分类，若不存在则返回null
            T currentParentCategory = Get(parentCategoryId);
            if (currentParentCategory == null)
                throw new ExceptionFacade(string.Format("Id为{0}的分类不存在！", parentCategoryId));

            //从OwnerId、tenantTypeId对应的类别集合中获取，排序：DisplayOrder正序 
            IEnumerable<T> orgCategories = GetOwnerCategories(currentParentCategory.OwnerId, currentParentCategory.TenantTypeId);
            if (orgCategories == null)
                return null;
            IList<T> orgCategoriesList = new List<T>(orgCategories);
            if (orgCategoriesList == null || orgCategoriesList.Count < 1)
                return null;

            //调用递归获取所有子集
            IList<T> descendantsList = new List<T>();
            categoryRepository.RecurseGetChildren(currentParentCategory, descendantsList, orgCategoriesList);

            return descendantsList;
        }

        /// <summary>
        /// 分页检索类别
        /// </summary>
        /// <returns>
        /// 按创建时间倒序排列的分页类别列表
        /// </returns>
        public PagingDataSet<T> GetCategories(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, string keyword, int pageIndex)
        {
            //排序：CategoryId倒序
            //禁用列表缓存
            return categoryRepository.GetCategories(publiclyAuditStatus, tenantTypeId, keyword, pageIndex);
        }

        /// <summary>
        /// 分页检索用户类别（OwnerId<>0）
        /// </summary>
        /// <returns>
        /// 按创建时间倒序排列的分页用户类别列表
        /// </returns>
        public PagingDataSet<T> GetOwnerCategories(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, string keyword,long ownerId,int pageSize, int pageIndex)
        {
            //排序：CategoryId倒序
            //禁用列表缓存
            return categoryRepository.GetOwnerCategories(publiclyAuditStatus, tenantTypeId, keyword,ownerId,pageSize, pageIndex);
        }
        #endregion

        #region 类别与内容项关系

        /// <summary>
        /// 批量为内容项设置类别
        /// </summary>
        /// <param name="itemIds">内容项Id集合</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="ownerId">类别拥有者Id</param>
        public void AddItemsToCategory(IEnumerable<long> itemIds, long categoryId, long ownerId)
        {
            //首先输入参数是否合理
            if (itemIds == null || categoryId < 1)
                throw new ExceptionFacade("输入参数不合理！");

            //获取关联的分类
            T category = Get(categoryId);
            if (category == null)
                throw new ExceptionFacade(string.Format("Id为：{0}的分类不存在！", categoryId));

            //插入数据库
            int effectLineCount = itemInCategoryRepository.AddItemsToCategory(itemIds, categoryId, ownerId);

            //修改category的内容项
            category.ItemCount = category.ItemCount + effectLineCount;
            categoryRepository.UpdateItemCount(category);
        }

        /// <summary>
        /// 为内容项批量设置类别
        /// </summary>
        /// <param name="categoryIds">类别Id集合</param>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">类别拥有者Id</param>
        public void AddCategoriesToItem(IEnumerable<long> categoryIds, long itemId, long ownerId)
        {
            //若输入参数有问题，则直接返回
            if (categoryIds == null || itemId < 1)
                throw new ExceptionFacade("输入参数不合理！");

            EventBus<long, CategoryEventArgs>.Instance().OnBatchBefore(categoryIds, new CategoryEventArgs(EventOperationType.Instance().Create(), "100201", itemId));
            //数据库插入操作
            itemInCategoryRepository.AddCategoriesToItem(categoryIds, itemId, ownerId);
            EventBus<long, CategoryEventArgs>.Instance().OnBatchAfter(categoryIds, new CategoryEventArgs(EventOperationType.Instance().Create(), "100201", itemId));

            //处理涉及分类的ItemCount计数
            if (categoryIds.Count<long>() > 0)
            {
                foreach (long categoryId in categoryIds)
                {
                    T category = Get(categoryId);
                    if (category != null)
                    {
                        category.ItemCount++;
                        categoryRepository.UpdateItemCount(category);
                    }
                }
            }
        }

        /// <summary>
        /// 清除内容项的所有类别(某个租户和用户的)
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">分类所有者</param>
        /// <param name="tenantTypeId">租户Id</param>
        public void ClearCategoriesFromItem(long itemId, long? ownerId, string tenantTypeId)
        {
            //操作设计到的分类
            IEnumerable<T> categories = GetCategoriesOfItem(itemId, ownerId, tenantTypeId);
            if (categories != null && categories.Count() > 0)
            {
                foreach (T category in categories)
                {
                    if (category != null)
                    {
                        category.ItemCount--;
                        if (category.ItemCount < 0)
                            category.ItemCount = 0;
                        categoryRepository.UpdateItemCount(category);
                    }
                }
            }
            //操作数据库
            itemInCategoryRepository.ClearCategoriesFromItem(itemId, ownerId, tenantTypeId);
        }

        /// <summary>
        /// 删除分类下的所有的关联项
        /// </summary>
        /// <param name="category">要处理的分类</param>
        /// <param name="ownerId">拥有者Id</param>
        public void ClearItemsFromCategory(T category, long ownerId)
        {
            //获取关联的Category
            if (category == null)
                throw new ExceptionFacade("category不能为空！");

            //操作数据库
            int effectLineCount = itemInCategoryRepository.ClearItemsFromCategory(category.CategoryId, ownerId);

            //处理关联的category
            if (effectLineCount > 0)
            {
                category.ItemCount = category.ItemCount - effectLineCount;
                if (category.ItemCount < 0)
                    category.ItemCount = 0;
                categoryRepository.UpdateItemCount(category);
            }
        }

        /// <summary>
        /// 删除分类下的所有的关联项
        /// </summary>
        /// <param name="categoryId">分类Id</param>
        public void ClearItemsFromCategory(long categoryId)
        {
            //获取关联的Category
            T category = Get(categoryId);
            ClearItemsFromCategory(category, 1);
        }

        /// <summary>
        /// 删除分类同内容的关联项
        /// </summary>
        /// <param name="categoryId">分类Id</param>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">拥有者Id</param>
        public void DeleteItemInCategory(long categoryId, long itemId, long ownerId)
        {
            T category = categoryRepository.Get(categoryId);
            if (category == null)
                throw new ExceptionFacade(string.Format("Id为:{0}的分类不存在", categoryId));

            int effectLineCount = itemInCategoryRepository.DeleteItemInCategory(categoryId, itemId, ownerId);

            //处理分类的内容项个数
            if (effectLineCount > 0)
            {
                category.ItemCount = category.ItemCount - effectLineCount;
                if (category.ItemCount < 0)
                    category.ItemCount = 0;
                categoryRepository.UpdateItemCount(category);

            }
        }

        /// <summary>
        /// 将内容项从fromCategoryId转移到toCategoryId
        /// </summary>
        /// <param name="itemIds">要转移的内容项</param>
        /// <param name="toCategoryId">目标分类Id</param>
        /// <param name="ownerId">分类所有者</param>
        /// <param name="tenantTypeId">租户Id</param>
        public void MoveItemsToCategory(IEnumerable<long> itemIds, long toCategoryId, long? ownerId, string tenantTypeId)
        {
            //删除内容项原来的关联
            foreach (long itemId in itemIds)
            {
                ClearCategoriesFromItem(itemId, ownerId, tenantTypeId);
            }

            //将内容项关联到toCategoryId
            AddItemsToCategory(itemIds, toCategoryId, 2);
        }

        /// <summary>
        /// 获取类别的内容项集合
        /// </summary>
        /// <param name="categoryId">分类的Id集合</param>
        /// <returns>内容项的ID集合</returns>
        public IEnumerable<long> GetItemIds(long categoryId, bool includeDescendant)
        {
            #region 组装分类的ID列表

            List<long> ids = new List<long>();
            ids.Add(categoryId);

            //若包括子分类则获取所有子孙
            if (includeDescendant)
            {
                ids.AddRange(GetDescendants(categoryId).Select(n => n.CategoryId));
            }

            #endregion

            IEnumerable<long> itemIds = itemInCategoryRepository.GetItemIds(categoryId, ids);

            return itemIds;
        }

        /// <summary>
        /// 获取类别的内容项集合
        /// </summary>
        /// <param name="categoryId">类别Id</param>
        /// <param name="includeDescendant">是否包括所有后代类别节点的内容项</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页码(从1开始)</param
        /// <param name="totalRecords">符合查询条件的记录数</param>
        /// <returns>返回指定页码的内容项Id集合</returns>
        public IEnumerable<long> GetItemIds(long categoryId, bool includeDescendant, int pageSize, int pageIndex, out long totalRecords)
        {
            #region 组装分类的ID列表

            List<long> ids = new List<long>();
            ids.Add(categoryId);

            //若包括子分类则获取所有子孙
            if (includeDescendant)
            {
                ids.AddRange(GetDescendants(categoryId).Select(n => n.CategoryId));
            }

            #endregion

            //从数据库中获取数据
            
            

            IEnumerable<long> itemIds = itemInCategoryRepository.GetItemIds(categoryId, ids, pageSize, pageIndex, out totalRecords);

            return itemIds;
        }

        /// <summary>
        /// 获取内容项的所有类别
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <returns>返回内容项的类别集合</returns>
        /// <param name="ownerId">分类所有者</param>
        /// <param name="tenantTypeId">租户Id</param>
        public IEnumerable<T> GetCategoriesOfItem(long itemId, long? ownerId, string tenantTypeId)
        {
            //内容项所对应的分类的列表
            IList<T> categories = new List<T>();

            //从数据库中获取内容项对应的分类的ID列表
            IEnumerable<long> categoryIds = itemInCategoryRepository.GetCategoriesOfItem(itemId, ownerId, tenantTypeId);

            if (categoryIds == null)
                return categories;

            //从categoryIds 组装成 categories
            foreach (long categoryId in categoryIds)
            {
                T category = Get(categoryId);
                if (category != null)
                    categories.Add(category);
            }

            return categories;
        }

        #endregion

    }


}
