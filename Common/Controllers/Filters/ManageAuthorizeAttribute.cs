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
using System.Web;
using Tunynet.Common;
using Tunynet.Mvc;
using System.Web.Routing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 后台身份验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ManageAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private bool requireSystemAdministrator = false;
        /// <summary>
        /// 是否需要系统管理员权限
        /// </summary>
        public bool RequireSystemAdministrator
        {
            get { return requireSystemAdministrator; }
            set { requireSystemAdministrator = value; }
        }


        private bool checkCookie = true;
        /// <summary>
        /// 是否需要检查Cookie
        /// </summary>
        public bool CheckCookie
        {
            get { return this.checkCookie; }
            set { this.checkCookie = value; }
        }

        private bool checkApplication = true;
        /// <summary>
        /// 是否需要检查Cookie
        /// </summary>
        public bool CheckApplication
        {
            get { return this.checkApplication; }
            set { this.checkApplication = value; }
        }


        #region IAuthorizationFilter 成员

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!AuthorizeCore(filterContext))
            {
                // auth failed, redirect to login page
                // filterContext.Cancel = true;
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult() { Data = new StatusMessageData(StatusMessageType.Hint, "您必须先以管理员身份登录下后台，才能继续操作") };
                }
                else
                {
                    filterContext.Controller.TempData["StatusMessageData"] = new StatusMessageData(StatusMessageType.Hint, "请以管理员身份登录");
                    filterContext.Result = new RedirectResult(SiteUrls.Instance().ManageLogin());
                }
                return;
            }
        }

        #endregion

        // This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.
        protected virtual bool AuthorizeCore(AuthorizationContext filterContext)
        {

            if (CheckCookie)
            {
                HttpCookie adminCookie = filterContext.HttpContext.Request.Cookies["SpacebuilderAdminCookie"];
                if (adminCookie != null)
                {
                    bool isLoginMarked = false;
                    try
                    {
                        bool.TryParse(Utility.DecryptTokenForAdminCookie(adminCookie.Value), out isLoginMarked);
                    }
                    catch { }

                    if (!isLoginMarked)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            RoleService roleService = new RoleService();
            if (RequireSystemAdministrator)
            {
                if (roleService.IsUserInRoles(currentUser.UserId, RoleNames.Instance().SuperAdministrator()))
                    return true;
                else
                    return false;
            }
            else
            {
                if (roleService.IsUserInRoles(currentUser.UserId, RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator()))
                    return true;
            }

            if (checkApplication)
            {
                //是否为管理员
                ApplicationService applicationService = new ApplicationService();
                string applicationKey = GetAreaName(filterContext.RouteData);
                var application = applicationService.Get(applicationKey);
                var authorizer = new Authorizer();
                if (application != null && authorizer.IsAdministrator(application.ApplicationId))
                    return true;
                string tenantTypeId = filterContext.RequestContext.GetParameterFromRouteDataOrQueryString("tenantTypeId");
                if (!string.IsNullOrEmpty(tenantTypeId))
                {
                    TenantType tenantType = new TenantTypeService().Get(tenantTypeId);
                    if (tenantType != null)
                    {
                        if (authorizer.IsAdministrator(tenantType.ApplicationId))
                            return true;
                    }
                }
            }
            else
            {
                return currentUser.IsAllowEntryControlPannel();
            }
            return false;
        }

        /// <summary>
        /// 从路由数据获取AreaName
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        private string GetAreaName(RouteData routeData)
        {
            object area;
            if (routeData.DataTokens.TryGetValue("area", out area))
            {
                return area as string;
            }

            return GetAreaName(routeData.Route);
        }

        /// <summary>
        /// 从路由数据获取AreaName
        /// </summary>
        /// <param name="route"><see cref="RouteBase"/></param>
        /// <returns>返回路由中的AreaName，如果无AreaName则返回null</returns>
        private string GetAreaName(RouteBase route)
        {
            IRouteWithArea routeWithArea = route as IRouteWithArea;
            if (routeWithArea != null)
                return routeWithArea.Area;

            Route castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
                return castRoute.DataTokens["area"] as string;

            return null;
        }


    }
}