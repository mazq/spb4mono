//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spacebuilder.Common;
using Tunynet.Utilities;
using System.Web.Mvc.Html;

namespace Tunynet.Mvc
{
    public static class UrlHelperSpaceKeyExtensions
    {

        /// <summary>
        /// 获取当前访问地址中的SpaceKey
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <returns></returns>
        public static string SpaceKey(this UrlHelper urlHelper)
        {
            object resultValue = null;
            if (urlHelper.RequestContext.RouteData.Values.TryGetValue("spaceKey", out resultValue) && resultValue != null)
                return resultValue.ToString();
            return string.Empty;
        }
    }
}