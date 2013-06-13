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
using Tunynet.Common.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 跟踪在线用户
    /// </summary>
    public class TrackOnlineUserAttribute : IResultFilter
    {

        void IResultFilter.OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null)
            {
                new OnlineUserService().TrackUser(currentUser);
            }
            else if (DIContainer.Resolve<IUserSettingsManager>().Get().EnableTrackAnonymous)
            {
                System.Web.HttpBrowserCapabilitiesBase browserCapabilities = filterContext.HttpContext.Request.Browser;
                if (browserCapabilities != null)
                {
                    
                    if (browserCapabilities.Cookies && !browserCapabilities.Crawler)
                    {
                        HttpCookie cookie;
                        if (GetAnonymousCookie(filterContext.HttpContext, out cookie))
                        {
                            string anonymousId = cookie.Value;
                            new OnlineUserService().TrackAnonymous(anonymousId);
                        }
                    }
                }
            }
        }

        void IResultFilter.OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        /// <summary>
        /// 获取匿名用户HttpCookie
        /// </summary>
        private bool GetAnonymousCookie(HttpContextBase httpContext, out HttpCookie cookie)
        {
            string cookieName = "spb.AnonymousId";
            cookie = httpContext.Request.Cookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Value = Guid.NewGuid().ToString();
                cookie.Expires = DateTime.Now.AddHours(8);
                httpContext.Response.Cookies.Add(cookie);
                return false;
            }
            return true;
        }
    }
}
