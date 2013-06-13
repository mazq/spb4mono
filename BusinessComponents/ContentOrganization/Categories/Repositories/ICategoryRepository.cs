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
    /// 分类仓储接口，实现特殊方法
    /// </summary>
    public interface ICategoryRepository<T> : IRepository<T> where T : Category
    {
        /// <summary>
        /// 批量更新审核状态
        /// </summary>
        /// <param name="ids">评论Id列表</param>
        /// <param name="auditingStatus">审核状态</param>
        void UpdateAuditStatus(IEnumerable<long> ids, AuditStatus auditStatus);

        /// <summary>
        /// 从fromCategoryId并入到toCategoryId
        /// </summary>
        /// <remarks>
        /// 例如：将分类fromCategoryId合并到分类toCategoryId，那么fromCategoryId分类下的所有子分类和实体全部归到toCategoryId分类，同时删除fromCategoryId分类
        /// </remarks>
        /// <param name="fromCategoryId">合并分类源类别</param>
        /// <param name="toCategoryId">合并分类目标类别</param>
        void Merge(long fromCategoryId, long toCategoryId);

        /// <summary>
        /// 把fromCategoryId移动到toCategoryId
        /// </summary>
        /// <remarks>
        /// 将一个分类移动到另一个分类，并作为另一个分类的子分类
        /// </remarks>
        /// <param name="fromCategoryId">被移动类别</param>
        /// <param name="toCategoryId">目标类别</param>
        void Move(long fromCategoryId, long toCategoryId);

        /// <summary>
        /// 获取拥有者的类别列表
        /// </summary>
        /// <param name="ownerId">类别拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>按树状排序的</returns>
        IEnumerable<T> GetOwnerCategories(long ownerId, string tenantTypeId);

        /// <summary>
        /// 获取子类别
        /// </summary>
        /// <param name="parentCategoryId">父类别Id</param>
        IEnumerable<T> GetChildren(long parentCategoryId);

        /// <summary>
        /// 根据用户id删除用户类别
        /// </summary>
        /// <param name="userId">用户的id</param>
        void CleanByUser(long userId);

        /// <summary>
        /// 分页检索类别
        /// </summary>
        /// <returns>
        /// 按创建时间倒序排列的分页类别列表
        /// </returns>
        PagingDataSet<T> GetCategories(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, string keyword, int pageIndex);
                
        /// <summary>
        /// 分页检索用户类别（OwnerId<>0）
        /// </summary>
        /// <returns>
        /// 按创建时间倒序排列的分页用户类别列表
        /// </returns>
        PagingDataSet<T> GetOwnerCategories(PubliclyAuditStatus? publiclyAuditStatus, string tenantTypeId, string keyword,long ownerId,int pageSize, int pageIndex);
 
        /// <summary>
        /// 获取所有子分类的递归方法
        /// </summary>
        /// <param name="category">当前分类</param>
        /// <param name="treeCategories">最终要组装的Tree分类</param>
        /// <param name="orgCategoriesList">原始分类列表</param>
        void RecurseGetChildren(T category, IList<T> treeCategories, IList<T> orgCategoriesList);

        /// <summary>
        /// 仅更新实体属性
        /// </summary>
        /// <param name="category">要更新的分类</param>
        void UpdateItemCount(T category);
    }
}
