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

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperTopBottomExtensions
    {

        /// <summary>
        /// 生成链接标签
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="text">链接文本</param>
        /// <param name="url">链接地址</param>
        /// <param name="title">链接提示文字（必填）</param>
        /// <param name="htmlAttributes">链接的其他属性集合</param>
        /// <param name="navigateTarget">头衔图片链接的Target</param>
        /// <returns></returns>
        public static MvcHtmlString TopBottom(this HtmlHelper htmlHelper, string objId, string tenantTypeId, string userId,string mode)
        {
           
           
            TagBuilder builder = new TagBuilder("a");
            //builder.MergeAttribute("href", url);
            //builder.SetInnerText(text);
            //builder.MergeAttribute("title", title);
            //if (navigateTarget != HyperLinkTarget._self)
            //    builder.MergeAttribute("target", navigateTarget.ToString());
            //if (htmlAttributes != null)
            //    builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
