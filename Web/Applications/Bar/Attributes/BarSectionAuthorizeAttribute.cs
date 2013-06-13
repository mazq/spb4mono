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

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 用于处理帖吧访问权限的过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BarSectionAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {

        #region IAuthorizationFilter 成员
        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="filterContext"></param>
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
            long sectionId = filterContext.RequestContext.GetParameterFromRouteDataOrQueryString<long>("sectionId");
            if (sectionId == 0)
                throw new ExceptionFacade("sectionId为0");
            BarSectionService barSectionService = new BarSectionService();
            BarSection section = barSectionService.Get(sectionId);
            if (section == null)
                throw new ExceptionFacade("找不到当前帖吧");
            if (section.IsEnabled)
                return;
            if (new Authorizer().BarSection_Manage(section))
                return;

            filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel
            {
                Title = "帖吧未启用",
                Body = "您访问的帖吧未启用，暂时不允许访问",
                StatusMessageType = Tunynet.Mvc.StatusMessageType.Error
            })/* 跳向无权访问页 */);
        }
    }
}