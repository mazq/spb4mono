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
    public static class PresentAreaNavigationExtensions
    {


        /// <summary>
        /// 获取导航Url
        /// </summary>
        /// <param name="presentAreaNavigation">被扩展的PresentAreaNavigation</param>
        /// <param name="spaceKey">空间标识</param>
        /// <returns></returns>
        public static string GetUrl(this PresentAreaNavigation presentAreaNavigation, string spaceKey = "")
        {
            if (presentAreaNavigation.NavigationUrl != null && !string.IsNullOrEmpty(presentAreaNavigation.NavigationUrl.Trim()))
            {
                return presentAreaNavigation.NavigationUrl;
            }

            RouteValueDictionary routeValue = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
            {
                routeValue.Add("spaceKey", spaceKey);
            }

            return CachedUrlHelper.RouteUrl(presentAreaNavigation.UrlRouteName, routeValue);
        }


        /// <summary>
        /// 获取导航Url
        /// </summary>
        /// <param name="presentAreaNavigation">被扩展的navigation</param>
        /// <param name="spaceKey">空间标识</param>
        /// <param name="routeValueDictionary">路由数据集合</param>
        /// <returns></returns>
        public static string GetUrl(this PresentAreaNavigation presentAreaNavigation, string spaceKey, RouteValueDictionary routeValueDictionary = null)
        {
            if (presentAreaNavigation.NavigationUrl != null && !string.IsNullOrEmpty(presentAreaNavigation.NavigationUrl.Trim()))
            {
                return presentAreaNavigation.NavigationUrl;
            }

            RouteValueDictionary routeDatas = null;
            if (!string.IsNullOrEmpty(presentAreaNavigation.RouteDataName) && routeValueDictionary != null)
            {
                string[] routeNames = presentAreaNavigation.RouteDataName.Split(',');
                routeDatas = new RouteValueDictionary(routeValueDictionary.Where(n => routeNames.Contains(n.Key)).ToDictionary(n => n.Key, n => n.Value));

                if (!routeNames.Contains("userName") && !routeNames.Contains("spaceKey"))
                {
                    routeDatas.AddOrReplace("sapceKey", spaceKey);
                }

                return CachedUrlHelper.RouteUrl(presentAreaNavigation.UrlRouteName, routeDatas);
            }

            routeDatas = new RouteValueDictionary() { { "spaceKey", spaceKey } };

            return CachedUrlHelper.RouteUrl(presentAreaNavigation.UrlRouteName, routeDatas);
        }

        /// <summary>
        /// 获取导航Test
        /// </summary>
        /// <param name="presentAreaNavigation">被扩展的PresentAreaNavigation</param>
        /// <returns></returns>
        public static string GetText(this PresentAreaNavigation presentAreaNavigation)
        {
            if (!string.IsNullOrEmpty(presentAreaNavigation.NavigationText))
                return presentAreaNavigation.NavigationText;
            else if (!string.IsNullOrEmpty(presentAreaNavigation.ResourceName))
                return Tunynet.Globalization.ResourceAccessor.GetString(presentAreaNavigation.ResourceName);

            return string.Empty;
        }


        /// <summary>
        /// 判断浏览者是否有浏览权限
        /// </summary>
        /// <param name="navigation">被扩展的导航对象</param>
        /// <param name="currentUser">当前用户实体</param>
        /// <returns></returns>
        public static bool IsVisible(this PresentAreaNavigation navigation, IUser currentUser)
        {
            if (!navigation.IsEnabled)
                return false;

            if (!navigation.OnlyOwnerVisible)
                return true;

            if (currentUser == null || (navigation.OwnerId != 0 && navigation.OwnerId != currentUser.UserId))
                return false;

            return true;
        }
    }
}
