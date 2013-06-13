//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Tunynet.Mvc;
using Tunynet.Utilities;
using Spacebuilder;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Tunynet.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 类别联动下拉列表
    /// </summary>
    public static class HtmlHelperCategoryDropDownListExtensions
    {
        /// <summary>
        /// 类别下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="expression">选择实体中类别属性的lamda表达式</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者ID</param>
        /// <param name="exceptCategoryId">需要去掉的ID</param>
        /// <param name="categoryLevel">类别层级(默认取站点配置）</param>
        public static MvcHtmlString CategoryDropDownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, long>> expression, string tenantTypeId, long ownerId, long exceptCategoryId = 0, int? categoryLevel = null)
        {
            string getChildCategorysUrl = SiteUrls.Instance().GetChildCategories(exceptCategoryId);
            CategoryServiceHelper CategoryServiceHelper = new CategoryServiceHelper(tenantTypeId, ownerId, exceptCategoryId);
            if (categoryLevel == null)
            {
                
                categoryLevel = 4;
            }
            return htmlHelper.LinkageDropDownListFor<TModel, long>(expression, 0, categoryLevel.Value, CategoryServiceHelper.GetRootCategoryDictionary(), CategoryServiceHelper.GetParentId, CategoryServiceHelper.GetChildrenDictionary, getChildCategorysUrl);
        }

        /// <summary>
        /// 类别下拉列表
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="name">控件name属性</param>
        /// <param name="value">选中的类别Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者ID</param>
        /// <param name="exceptCategoryId">需要去掉的ID</param>
        /// <param name="categoryLevel">类别层级(默认取站点配置）</param>
        public static MvcHtmlString CategoryDropDownListFor(this HtmlHelper htmlHelper, string name, long value, string tenantTypeId, long ownerId, long exceptCategoryId = 0, int? categoryLevel = null)
        {
            string getChildCategorysUrl = SiteUrls.Instance().GetChildCategories(exceptCategoryId);
            CategoryServiceHelper CategoryServiceHelper = new CategoryServiceHelper(tenantTypeId, ownerId, exceptCategoryId);
            if (categoryLevel == null)
            {
                
                categoryLevel = 4;
            }
            return htmlHelper.LinkageDropDownList<long>(name, value, 0, categoryLevel.Value, CategoryServiceHelper.GetRootCategoryDictionary(), CategoryServiceHelper.GetParentId, CategoryServiceHelper.GetChildrenDictionary, getChildCategorysUrl);
        }

    }

    /// <summary>
    /// 分类业务逻辑扩展类
    /// </summary>
    internal class CategoryServiceHelper
    {
        private string tenantTypeId = null;
        private long ownerId = 0;
        private long exceptCategoryId = 0;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tenantTypeId"></param>
        public CategoryServiceHelper(string tenantTypeId, long ownerId, long exceptCategoryId)
        {
            this.tenantTypeId = tenantTypeId;
            this.ownerId = ownerId;
            this.exceptCategoryId = exceptCategoryId;
        }

        CategoryService CategoryService = new CategoryService();

        /// <summary>
        /// 获取父类别Id
        /// </summary>
        public long GetParentId(long categoryId)
        {
            Category category = CategoryService.Get(categoryId);
            if (category != null)
                return category.ParentId;
            return 0;
        }

        /// <summary>
        /// 获取子类别
        /// </summary>
        public Dictionary<long, string> GetChildrenDictionary(long categoryId)
        {
            Category Category = CategoryService.Get(categoryId);
            if (Category != null)
                return Category.Children.Where(n => n.CategoryId != exceptCategoryId).ToDictionary(n => n.CategoryId, n => StringUtility.Trim(n.CategoryName, 7));
            return null;
        }

        /// <summary>
        /// 获取一级类别
        /// </summary>
        public Dictionary<long, string> GetRootCategoryDictionary()
        {
            var categories = CategoryService.GetRootCategories(tenantTypeId);
            if (categories == null)
                return null;

            return categories.Where(n => n.CategoryId != exceptCategoryId).ToDictionary(n => n.CategoryId, n => StringUtility.Trim(n.CategoryName, 7));
        }
    }
}