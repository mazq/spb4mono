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

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展HtmlHelper
    /// </summary>
    public static class HtmlHelperDescriptionExtensions
    {
        /// <summary>
        /// 获取数据标记Display的Description属性值
        /// </summary>
        /// <param name="html">被扩展的htmlHelper实例</param>
        /// <param name="expression">选择实体中类别属性的lamda表达式</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return DescriptionHelper(html,ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        #region 私有方法
        /// <summary>
        /// 获取数据标记Display的Description属性值
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="metadata"></param>
        /// <returns>MvcHtmlString</returns>
        private static MvcHtmlString DescriptionHelper<TModel>(this HtmlHelper<TModel> htmlHelper, ModelMetadata metadata)
        {
            string resolvedLabelText = metadata.Description;
            if (String.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }
            return MvcHtmlString.Create(resolvedLabelText);
        }
        #endregion
    }
}
