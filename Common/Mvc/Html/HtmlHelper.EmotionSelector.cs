//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tunynet.Common;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Html编辑器的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperSmileySelectorExtensions
    {
        /// <summary>
        /// 表情选择器
        /// </summary> 
        /// <param name="htmlHelper">被扩展对象</param>
        /// <param name="id">表情选择器id的组成部分</param>
        /// <param name="objectId">表情选择器作用对象的Id</param>
        /// <param name="directoryName">表情分类目录名</param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString EmotionSelector(this HtmlHelper htmlHelper)
        {
            return htmlHelper.EditorForModel("EmotionSelector");
        }
    }
}
