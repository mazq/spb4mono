//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Collections;
using System.Web.Helpers;
using Tunynet.Utilities;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperGenerateBodyAppCssClassExtensions
    {

        /// <summary>
        /// 生成body标签的和应用相关的cssClass
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        public static string GenerateBodyAppCssClass(this HtmlHelper htmlHelper)
        {
            RouteValueDictionary routeValueDictionary = htmlHelper.ViewContext.RouteData.DataTokens;
            string areaName = routeValueDictionary.Get<string>("area", string.Empty);
            string appCssClass = string.Format("spb-{0}-page", areaName.ToLower());
            if (htmlHelper.ViewData.ContainsKey("bodyCssClass"))
                appCssClass += " " + htmlHelper.ViewData["bodyCssClass"];
            return appCssClass;
        }
    }
}
