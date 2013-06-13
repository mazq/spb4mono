//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Linq;
using System.Web.Routing;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.UI;

namespace Spacebuilder.Common
{
    /// <summary>
    /// ApplicationManagementOperation扩展
    /// </summary>
    public static class ApplicationManagementOperationExtensions
    {
        /// <summary>
        /// 获取快捷操作Url
        /// </summary>
        /// <param name="applicationManagementOperation">被扩展的applicationManagementOperation</param>
        /// <param name="routeValueDictionary">路由数据字典</param>
        /// <returns></returns>
        public static string GetUrl(this ApplicationManagementOperation applicationManagementOperation, RouteValueDictionary routeValueDictionary = null)
        {
            if (applicationManagementOperation.NavigationUrl != null && !string.IsNullOrEmpty(applicationManagementOperation.NavigationUrl.Trim()))
            {
                return applicationManagementOperation.NavigationUrl;
            }

            if (!string.IsNullOrEmpty(applicationManagementOperation.RouteDataName) && routeValueDictionary != null)
            {
                string[] routeNames = applicationManagementOperation.RouteDataName.Split(',');
                RouteValueDictionary dic = new RouteValueDictionary(routeValueDictionary.Where(n => routeNames.Contains(n.Key)).ToDictionary(n => n.Key, n => n.Value));
                return CachedUrlHelper.RouteUrl(applicationManagementOperation.UrlRouteName, dic);
            }

            return CachedUrlHelper.RouteUrl(applicationManagementOperation.UrlRouteName);
        }

        /// <summary>
        /// 获取快捷操作Url
        /// </summary>
        /// <param name="applicationManagementOperation">被扩展的applicationManagementOperation</param>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="routeValueDictionary">路由数据字典</param>
        /// <returns></returns>
        public static string GetUrl(this ApplicationManagementOperation applicationManagementOperation, string spaceKey, RouteValueDictionary routeValueDictionary = null)
        {
            if (applicationManagementOperation.NavigationUrl != null && !string.IsNullOrEmpty(applicationManagementOperation.NavigationUrl.Trim()))
            {
                return applicationManagementOperation.NavigationUrl;
            }

            RouteValueDictionary routeDatas = null;
            if (!string.IsNullOrEmpty(applicationManagementOperation.RouteDataName) && routeValueDictionary != null)
            {
                string[] routeNames = applicationManagementOperation.RouteDataName.Split(',');
                routeDatas = new RouteValueDictionary(routeValueDictionary.Where(n => routeNames.Contains(n.Key)).ToDictionary(n => n.Key, n => n.Value));
                routeDatas.AddOrReplace("sapceKey", spaceKey);
                return CachedUrlHelper.RouteUrl(applicationManagementOperation.UrlRouteName, routeDatas);
            }

            RouteValueDictionary routeValues = new RouteValueDictionary() { { "spaceKey", spaceKey } };

            return CachedUrlHelper.RouteUrl(applicationManagementOperation.UrlRouteName, routeValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsVisible(this ApplicationManagementOperation applicationManagementOperation, IUser spaceUser)
        {
            if (!applicationManagementOperation.IsEnabled)
            {
                return false;
            }

            if (UserContext.CurrentUser == null)
                return false;

            if (applicationManagementOperation.OnlyOwnerVisible
                && (UserContext.CurrentUser == null
                && UserContext.CurrentUser.UserId != spaceUser.UserId))
            {
                return false;
            }

            return true;
        }
    }
}
