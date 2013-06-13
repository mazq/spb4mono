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
using Spacebuilder.Common;
using Tunynet.Mvc;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 用于处理用户空间权限的过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class GroupSpaceAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private bool requireManager = false;

        /// <summary>
        /// 是否需要空间主人或管理员权限
        /// </summary>
        public bool RequireManager
        {
            get { return requireManager; }
            set { requireManager = value; }
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
                throw new ExceptionFacade("spaceKey为null");
            GroupService groupService = new GroupService();
            GroupEntity group = groupService.Get(spaceKey);
            if (group == null)
                throw new ExceptionFacade("找不到当前群组");

            IUser currentUser = UserContext.CurrentUser;



            //判断访问群组权限
            if (!new Authorizer().Group_View(group))
            {
                if (currentUser == null)
                    filterContext.Result = new RedirectResult(SiteUrls.Instance().Login(true));
                else
                {
                    if (group.AuditStatus != AuditStatus.Success)
                    {
                        filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
                        {
                            Title = "无权访问群组！",
                            Body = "该群组还没有通过审核，所以不能访问！",
                            StatusMessageType = StatusMessageType.Error
                        }, filterContext.HttpContext.Request.RawUrl)/* 跳向无权访问页 */);
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
                        {
                            Title = "无权访问群组！",
                            Body = "你没有访问该群组的权限",
                            StatusMessageType = StatusMessageType.Error
                        }, filterContext.HttpContext.Request.RawUrl)/* 跳向无权访问页 */);
                    }
                }
                return;
            }

            //判断该用户是否有访问该群组管理页面的权限
            if (!RequireManager)
                return;
            //匿名用户要求先登录跳转
            if (currentUser == null)
            {
                filterContext.Result = new RedirectResult(SiteUrls.Instance().Login(true));
                return;
            }

            if (new Authorizer().Group_Manage(group))
                return;
            filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
            {
                Title = "无权访问",
                Body = "您无权访问此页面，只有群主或管理员才能访问",
                StatusMessageType = Tunynet.Mvc.StatusMessageType.Error
            })/* 跳向无权访问页 */);
        }
    }
}