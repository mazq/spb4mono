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

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 分类和内容关联项，需要的数据服务接口
    /// </summary>
    public interface IItemInCategoryRepository : IRepository<ItemInCategory>
    {
        /// <summary>
        /// 批量为内容项设置类别
        /// </summary>
        /// <param name="itemIds">内容项Id集合</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="ownerId">类别拥有者Id</param>
        int AddItemsToCategory(IEnumerable<long> itemIds, long categoryId,long ownerId);

        /// <summary>
        /// 为内容项批量设置类别
        /// </summary>
        /// <param name="categoryIds">类别Id集合</param>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">类别拥有者Id</param>
        void AddCategoriesToItem(IEnumerable<long> categoryIds, long itemId, long ownerId);

        /// <summary>
        /// 清除内容项的所有类别
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">分类所有者</param>
        /// <param name="tenantTypeId">租户Id</param>
        int ClearCategoriesFromItem(long itemId, long? ownerId, string tenantTypeId);

        /// <summary>
        /// 删除分类下的所有的关联项
        /// </summary>
        /// <param name="categoryId">分类Id</param>
        /// <param name="ownerId">拥有者Id</param>
        int ClearItemsFromCategory(long categoryId,long ownerId);

        /// <summary>
        /// 获取类别的内容项集合
        /// </summary>
        /// <param name="categoryId">分类的Id集合</param>
        /// <returns>内容项的ID集合</returns>
        IEnumerable<long> GetItemIds(long categoryId, IEnumerable<long> categorieIds);

        /// <summary>
        /// 获取类别的内容项集合
        /// </summary>
        /// <param name="categoryId">当前分类的Id</param>
        /// <param name="ids">分类的Id集合</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="totalRecords">输出参数：总记录数</param>
        /// <returns>当页内容项的ID集合</returns>
        IEnumerable<long> GetItemIds(long categoryId, IEnumerable<long> ids, int pageSize, int pageIndex, out long totalRecords);

        /// <summary>
        /// 获取内容项的所有类别Id集合
        /// </summary>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">分类所有者</param>
        /// <param name="tenantTypeId">租户Id</param>
        /// <returns>返回内容项的类别Id集合</returns>
        IEnumerable<long> GetCategoriesOfItem(long itemId, long? ownerId, string tenantTypeId);

        /// <summary>
        /// 删除分类同内容的关联项
        /// </summary>
        /// <param name="categoryId">分类Id</param>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">拥有者Id</param>
        int DeleteItemInCategory(long categoryId, long itemId,long ownerId);
    }
}
