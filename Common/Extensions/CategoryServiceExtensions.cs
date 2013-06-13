//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Common;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// CategoryService扩展
    /// </summary>
    public static class CategoryServiceExtensions
    {
        /// <summary>
        /// 将分类类别转换为TreeNodes
        /// </summary>
        /// <param name="categoryService">扩展类：CategoryService</param>
        /// <param name="categories">要转换成节点分类集合</param>
        /// <param name="targetType">点击分类时打开新页还是在本页打开</param>
        /// <param name="idPrefix">节点ID前缀</param>
        /// <param name="checkedCategoryId">当前选中的CategoryId</param>
        /// <param name="openLevel">默认打开的层级</param>
        /// <returns>TreeNodes列表</returns>
        public static IList<TreeNode> CategoriesToTreeNodes(this CategoryService<Category> categoryService, IList<Category> categories, TargetType targetType, string idPrefix, long? checkedCategoryId, int? openLevel)
        {
            //本方法将分类转换成TreeNodes，其中，哪些节点展开还是闭合的逻辑较为复杂，如下：
            //关于哪些节点展开哪些节点闭合，逻辑实现如下：
            //1 首先，若checkedCategoryId有值，则这个分类的所有父级分类展开，其他分类全部折叠；
            //2 其次，若checkedCategoryId无值，openLevel有着，则深度小于等于openLevel的分类展开，其他分类折叠；
            //3 若若checkedCategoryId和openLevel都没有值，则所有分类折叠。

            IList<TreeNode> nodes = new List<TreeNode>();
            if (categories != null && categories.Count > 0)
            {
                //若当前有选中的分类，则获取其所有的父级分类Id集合
                List<long> checkedCategoryIds = new List<long>();
                if (checkedCategoryId.HasValue && checkedCategoryId.Value > 0)
                    GetParentCategoryIDs(categoryService, checkedCategoryId.Value, checkedCategoryIds);

                //通过Category组装TreeNode
                foreach (Category category in categories)
                {
                    TreeNode node = new TreeNode();
                    node.Id = idPrefix + category.CategoryId.ToString();
                    node.IsChecked = (checkedCategoryId == category.CategoryId);
                    node.IsOpened = IsOpenedNode(category, checkedCategoryIds, openLevel);
                    node.IsParent = category.ChildCount > 0;
                    node.Name = category.CategoryName;
                    node.ParentId = idPrefix + category.ParentId.ToString();
                    node.Target = targetType;
                    node.Url = string.Empty;
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// 递归调用，获取分类的所有父级分类的Id集合
        /// </summary>
        /// <param name="categoryService">逻辑类</param>
        /// <param name="categoryId">当前分类的Id</param>
        /// <param name="checkedCategoryIds">父级分类Id集合</param>
        private static void GetParentCategoryIDs(CategoryService<Category> categoryService, long categoryId, List<long> checkedCategoryIds)
        {
            Category category = categoryService.Get(categoryId);
            if (category!=null && category.ParentId > 0)
            {
                checkedCategoryIds.Add(category.ParentId);
                GetParentCategoryIDs(categoryService, category.ParentId, checkedCategoryIds);
            }
        }

        /// <summary>
        /// 获取当前分类是否是打开状态
        /// </summary>
        /// <param name="category">当前分类</param>
        /// <param name="checkedCategoryIds">被选中的分类Id集合</param>
        /// <param name="openLevel">默认展开的层级</param>
        /// <returns>是否打开</returns>
        private static bool IsOpenedNode(Category category, List<long> checkedCategoryIds, int? openLevel)
        {
            //有选中分类的情况；该分类的所有父级分类展开，其他分类关闭
            if (checkedCategoryIds != null && checkedCategoryIds.Count > 0)
            {
                return checkedCategoryIds.Contains(category.CategoryId);
            }
            //小于等于展开层级的分类展开，其他分类关闭
            else if (openLevel.HasValue && openLevel.Value > 0)
            {
                return category.Depth <= openLevel;
            }
            return false;
        }
    }
}