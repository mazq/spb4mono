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
using Tunynet.Common;
using Spacebuilder.Common;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对ActivityOperation的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperActivityOperationExtensions
    {

        /// <summary>
        /// 生成动态操作链接
        /// </summary>       
        public static MvcHtmlString ActivityOperation(this HtmlHelper htmlHelper, long activityId)
        {
            var activity = new ActivityService().Get(activityId);
            if (activity == null)
                return MvcHtmlString.Empty;
            bool isSiteActivity = htmlHelper.ViewContext.HttpContext.Request.QueryString.Get<bool>("isSiteActivity");
            bool isOwnerActivity = htmlHelper.ViewContext.HttpContext.Request.QueryString.Get<bool>("isOwnerActivity");
            return htmlHelper.DisplayForModel("ActivityOperation", new { activity = activity, isSiteActivity = isSiteActivity, isOwnerActivity = isOwnerActivity });
        }
    }
}
