//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Routing;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 页码下拉菜单
    /// </summary>
    public static class PageSizeDropdownListExtensions
    {
        /// <summary>
        /// 页码下拉菜单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">控件名称</param>
        /// <param name="htmlAttributes">样式</param>
        /// <returns>页码下来菜单</returns>
        public static MvcHtmlString PageSizeDropdownList(this HtmlHelper htmlHelper, string name, object htmlAttributes = null, List<int> pageSizes = null)
        {
            return pageSizeDropdownList(name, htmlAttributes, pageSizes);
        }

        private static MvcHtmlString pageSizeDropdownList(string name, object htmlAttributes = null, List<int> pageSizes = null)
        {
            IEnumerable<SelectListItem> items = GetSelectList("{0}条", pageSizes);

            TagBuilder select = new TagBuilder("select");
            select.MergeAttribute("id", "select-pagesize");
            select.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            foreach (var item in items)
            {
                TagBuilder option = new TagBuilder("option");
                option.InnerHtml = item.Text;
                option.MergeAttribute("value", item.Value);
                select.InnerHtml += option.ToString();
            }

            TagBuilder div = new TagBuilder("div");
            div.MergeAttribute("class", "tn-perpage-show");
            div.MergeAttribute("plugin", "pageSize");
            TagBuilder label = new TagBuilder("label");
            label.InnerHtml = "每页显示：";

            div.InnerHtml = label.ToString() + select.ToString();

            //TagBuilder script = new TagBuilder("script");
            //script.MergeAttribute("type", "text/javascript");
            //script.MergeAttribute("language", "javascript");


            //script.InnerHtml = "$(\"select#select-pagesize\").change(function(){$.cookie('"+name+"',$(this).val());});if($.cookie('"+name+"')==null){$(\"select#select-pagesize\").children().first().attr(\"selected\",\"selected\");}else{$(\"select#select-pagesize\").children().removeAttr(\"selected\");$(\"select#select-pagesize\").children().each(function(){if($.cookie('"+name+"')==$(this).val()){$(this).attr(\"selected\",\"selected\");}});}";

            return new MvcHtmlString(div.ToString());
        }

        /// <summary>
        /// 页码下拉菜单
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes">属性</param>
        /// <returns>页码下拉菜单</returns>
        public static MvcHtmlString PageSizeDropdownListFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, int?>> expression, object htmlAttributes = null, List<int> pageSizes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            string name = metadata.PropertyName;

            return pageSizeDropdownList(name, htmlAttributes, pageSizes);
        }

        private static List<SelectListItem> GetSelectList(string unit, List<int> pageSizes)
        {
            if (pageSizes == null || pageSizes.Count == 0)
                pageSizes = new List<int> { 20, 40, 60, 80, 100 };

            List<SelectListItem> selectLists = new List<SelectListItem>();
            foreach (var item in pageSizes)
            {
                selectLists.Add(new SelectListItem { Text = string.Format(unit, item), Value = item.ToString() });
            }
            return selectLists;
        }

    }
}
