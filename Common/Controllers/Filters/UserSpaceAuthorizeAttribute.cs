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
    public class UserSpaceAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private bool requireOwnerOrAdministrator = false;

        /// <summary>
        /// 是否需要空间主人或管理员权限
        /// </summary>
        public bool RequireOwnerOrAdministrator
        {
            get { return requireOwnerOrAdministrator; }
            set { requireOwnerOrAdministrator = value; }
        }

        #region IAuthorizationFilter 成员

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            AuthorizeCore(filterContext);
        }

        #endregion

        
        private void AuthorizeCore(AuthorizationContext filterContext)
        {
            string spaceKey = UserContext.CurrentSpaceKey(filterContext);

            if (string.IsNullOrEmpty(spaceKey))
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }
            IUserService userService = DIContainer.Resolve<IUserService>();
            User currentSpaceUser = userService.GetFullUser(spaceKey);
            if (currentSpaceUser == null)
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }
            IUser currentUser = UserContext.CurrentUser;
            //判断空间访问隐私
            PrivacyService privacyService = new PrivacyService();
            if (!privacyService.Validate(currentSpaceUser.UserId, currentUser != null ? currentUser.UserId : 0, PrivacyItemKeys.Instance().VisitUserSpace()))
            {
                if (currentUser == null)
                    filterContext.Result = new RedirectResult(SiteUrls.Instance().Login(true));
                else
                    filterContext.Result = new RedirectResult(SiteUrls.Instance().PrivacyHome(currentSpaceUser.UserName)/* 跳向无权访问页 */);
                return;
            }

            //判断该用户是否有访问该空间的权限
            if (!RequireOwnerOrAdministrator)
                return;
            //匿名用户要求先登录跳转
            if (currentUser == null)
            {
                filterContext.Result = new RedirectResult(SiteUrls.Instance().Login(true));
                return;
            }

            if (currentSpaceUser.UserId == currentUser.UserId)
            {
                if (currentUser.IsBanned)
                {
                    IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
                    authenticationService.SignOut();
                    filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
                      {
                          Title = "帐号被封禁！",
                          Body = "由于您的非法操作，您的帐号已被封禁，如有疑问，请联系管理员",
                          StatusMessageType = Tunynet.Mvc.StatusMessageType.Error
                      })/* 跳向无权访问页 */);
                }
                return;
            }
            if (currentUser.IsInRoles(RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator()))
                return;
            filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
            {
                Title = "无权访问",
                Body = "您无权访问此页面，只有空间主人或管理员才能访问",
                StatusMessageType = Tunynet.Mvc.StatusMessageType.Error
            })/* 跳向无权访问页 */);
        }
    }
}