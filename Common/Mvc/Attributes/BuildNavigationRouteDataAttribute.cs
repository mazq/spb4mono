//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Mvc;
using Tunynet;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于处理用户空间权限的过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BuildNavigationRouteDataAttribute : FilterAttribute, IResultFilter
    {
        private string presentAreaKey = PresentAreaKeysOfBuiltIn.Channel;

        /// <summary>
        /// 呈现区域标识
        /// </summary>
        public string PresentAreaKey
        {
            get { return presentAreaKey; }
            set { presentAreaKey = value; }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (PresentAreaKey == PresentAreaKeysOfBuiltIn.Channel)
            {
                if (UserContext.CurrentUser != null)
                {
                    filterContext.RouteData.Values["spaceKey"] = UserContext.CurrentUser.UserName;
                }
            }
            else if (PresentAreaKey == PresentAreaKeysOfBuiltIn.UserSpace)
            {
                filterContext.RouteData.Values["userName"] = filterContext.RouteData.Values["spaceKey"];
            }
        }
    }
}