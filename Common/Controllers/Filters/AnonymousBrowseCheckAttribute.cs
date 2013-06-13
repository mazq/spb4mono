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
using Tunynet.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于处理是否允许用户匿名访问的过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AnonymousBrowseCheckAttribute : FilterAttribute, IAuthorizationFilter
    {
        #region IAuthorizationFilter 成员

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.IsChildAction)
                return;

            IUser currentUser = UserContext.CurrentUser;

            if (currentUser == null)
            {
                ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
                SiteSettings siteSettings = siteSettingsManager.Get();
                if (!siteSettings.EnableAnonymousBrowse)
                {
                    if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                        filterContext.Result = new EmptyResult();
                    else
                        filterContext.Result = new RedirectResult(SiteUrls.Instance().Login(true));
                    return;
                }
            }
        }

        #endregion

    }
}
