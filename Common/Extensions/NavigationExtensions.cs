//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Routing;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.UI;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Navigation扩展
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// 获取导航Url
        /// </summary>
        /// <param name="navigation">被扩展的navigation</param>
        /// <param name="routeValueDictionary">路由数据集合</param>
        /// <returns></returns>
        public static string GetUrl(this Navigation navigation, RouteValueDictionary routeValueDictionary = null)
        {
            if (navigation.NavigationUrl != null && !string.IsNullOrEmpty(navigation.NavigationUrl.Trim()))
            {
                return navigation.NavigationUrl;
            }

            if (!string.IsNullOrEmpty(navigation.RouteDataName) && routeValueDictionary != null)
            {
                string[] routeNames = navigation.RouteDataName.Split(',');
                return CachedUrlHelper.RouteUrl(navigation.UrlRouteName, new RouteValueDictionary(routeValueDictionary.Where(n => routeNames.Contains(n.Key)).ToDictionary(n => n.Key, n => n.Value)));
            }

            return CachedUrlHelper.RouteUrl(navigation.UrlRouteName);
        }

        /// <summary>
        /// 获取导航Url
        /// </summary>
        /// <param name="navigation">被扩展的navigation</param>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="routeValueDictionary">路由数据集合</param>
        /// <returns></returns>
        public static string GetUrl(this Navigation navigation, string spaceKey, RouteValueDictionary routeValueDictionary = null)
        {
            if (navigation.NavigationUrl != null && !string.IsNullOrEmpty(navigation.NavigationUrl.Trim()))
            {
                return navigation.NavigationUrl;
            }

            RouteValueDictionary routeDatas = null;
            if (!string.IsNullOrEmpty(navigation.RouteDataName) && routeValueDictionary != null)
            {
                string[] routeNames = navigation.RouteDataName.Split(',');
                routeDatas = new RouteValueDictionary(routeValueDictionary.Where(n => routeNames.Contains(n.Key)).ToDictionary(n => n.Key, n => n.Value));
                routeDatas.AddOrReplace("sapceKey", spaceKey);
                return CachedUrlHelper.RouteUrl(navigation.UrlRouteName, routeDatas);
            }

            routeDatas = new RouteValueDictionary() { { "spaceKey", spaceKey } };

            return CachedUrlHelper.RouteUrl(navigation.UrlRouteName, routeDatas);
        }

        /// <summary>
        /// 判断浏览者是否有浏览权限
        /// </summary>
        /// <param name="navigation">被扩展的导航对象</param>
        /// <param name="currentUser">当前用户实体</param>
        /// <returns></returns>
        public static bool IsVisible(this Navigation navigation, IUser currentUser)
        {
            if (!navigation.IsEnabled)
                return false;

            if (!navigation.OnlyOwnerVisible)
                return true;

            if (currentUser == null || (navigation.OwnerId != 0 && navigation.OwnerId != currentUser.UserId))
                return false;

            return true;
        }

        /// <summary>
        /// 判断浏览着是否具有管理权限
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static bool IsControlPanelManageable(this Navigation navigation, IUser currentUser)
        {
            if (currentUser.IsInRoles(RoleNames.Instance().SuperAdministrator()))
                return true;
            if (navigation.ApplicationId > 0)
            {
                if (new Authorizer().IsAdministrator(navigation.ApplicationId))
                    return true;
            }
            else if (currentUser.IsInRoles(RoleNames.Instance().ContentAdministrator()) && !navigation.UrlRouteName.StartsWith("ControlPanel_User") && !navigation.UrlRouteName.StartsWith("ControlPanel_Settings"))
                return true;
            if (navigation.UrlRouteName == "ControlPanel_Home" || navigation.UrlRouteName == "ControlPanel_Content_Home")
                return true;
            return false;
        }

        /// <summary>
        /// 判断浏览着是否具有管理权限
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static bool IsControlPanelManageable(this InitialNavigation navigation, IUser currentUser)
        {
            if (currentUser.IsInRoles(RoleNames.Instance().SuperAdministrator()))
                return true;
            if (navigation.ApplicationId > 0)
            {
                if (new Authorizer().IsAdministrator(navigation.ApplicationId))
                    return true;
            }
            else if (currentUser.IsInRoles(RoleNames.Instance().ContentAdministrator()) && !navigation.UrlRouteName.StartsWith("ControlPanel_User") && !navigation.UrlRouteName.StartsWith("ControlPanel_Settings"))
                return true;
            if (navigation.UrlRouteName == "ControlPanel_Home" || navigation.UrlRouteName == "ControlPanel_Content_Home")
                return true;
            return false;
        }
    }
}
