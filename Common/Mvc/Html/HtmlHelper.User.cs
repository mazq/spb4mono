//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tunynet.Common;
using Tunynet.Utilities;
using Tunynet.Globalization;
using Spacebuilder.Common;
using System.Web.Routing;
using Spacebuilder.Common.Configuration;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对User的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperUserExtensions
    {
        #region 获取DisplayName
        /// <summary>
        /// 获取DisplayName
        /// </summary>
        public static string ShowUserDisplayName(this HtmlHelper htmlHelper, long userId, int charLimit = 5)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.GetUser(userId);
            if (user == null)
                return ResourceAccessor.GetString("Common_AnonymousDisplayName");

            return htmlHelper.ShowUserDisplayName(user.DisplayName, charLimit);
        }

        /// <summary>
        /// 获取DisplayName,不输出身份标识
        /// </summary>
        public static string ShowUserDisplayName(this HtmlHelper htmlHelper, string displayName, int charLimit = 5)
        {
            return StringUtility.Trim(displayName, charLimit);
        }

        #endregion

        #region 显示用户头像

        /// <summary>
        /// 显示用户头像
        /// </summary>
        /// <param name="userId">userID</param>
        /// <param name="enableNavigate">是否允许链接到用户空间</param>
        /// <param name="navigateTarget">头衔图片链接的Target</param>
        /// <param name="avatarSizeType">头像尺寸类别</param>
        /// <param name="htmlAttributes">html属性，例如：new RouteValueDictionary{{"Class","editor"},{"width","90%"}}</param>
        public static MvcHtmlString ShowUserAvatar(this HtmlHelper htmlHelper, long userId, AvatarSizeType avatarSizeType = AvatarSizeType.Medium, bool enableNavigate = true, HyperLinkTarget navigateTarget = HyperLinkTarget._blank, bool enableCachingInClient = true, RouteValueDictionary htmlAttributes = null, bool isShowUserCard = true, bool isShowTitle = false)
        {
            UserService userService = new UserService();
            IUser user = userService.GetUser(userId);
            return ShowUserAvatar(htmlHelper, user, avatarSizeType, enableNavigate, navigateTarget, enableCachingInClient, htmlAttributes, isShowUserCard, isShowTitle);
        }

        /// <summary>
        /// 显示用户头像
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="enableNavigate">是否允许链接到用户空间</param>
        /// <param name="navigateTarget">头衔图片链接的Target</param>
        /// <param name="avatarSizeType">头像尺寸类别</param>
        /// <param name="enableCachingInClient">是否允许在客户端缓存</param>
        /// <param name="htmlAttributes">html属性，例如：new RouteValueDictionary{{"Class","editor"},{"width","90%"}}</param>
        public static MvcHtmlString ShowUserAvatar(this HtmlHelper htmlHelper, IUser user, AvatarSizeType avatarSizeType = AvatarSizeType.Medium, bool enableNavigate = true, HyperLinkTarget navigateTarget = HyperLinkTarget._blank, bool enableCachingInClient = true, RouteValueDictionary htmlAttributes = null, bool isShowUserCard = true, bool isShowTitle = false)
        {
            return ShowUserAvatar(htmlHelper, user, enableNavigate ? SiteUrls.Instance().SpaceHome(user == null ? 0 : user.UserId) : string.Empty, avatarSizeType, navigateTarget, enableCachingInClient, htmlAttributes, isShowUserCard, isShowTitle);
        }

        /// <summary>
        /// 显示头像
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="avatarSizeType">头像尺寸类别</param>
        /// <param name="link">链接到用户空间的地址</param>
        /// <param name="navigateTarget">链接类型</param>
        /// <param name="enableCachingInClient">是否允许在客户端缓存</param>
        /// <param name="htmlAttributes">html属性，例如：new RouteValueDictionary{{"Class","editor"},{"width","90%"}}</param>
        /// <returns></returns>
        public static MvcHtmlString ShowUserAvatar(this HtmlHelper htmlHelper, IUser user, string link, AvatarSizeType avatarSizeType = AvatarSizeType.Medium, HyperLinkTarget navigateTarget = HyperLinkTarget._blank, bool enableCachingInClient = true, RouteValueDictionary htmlAttributes = null, bool isShowUserCard = true, bool isShowTitle = false)
        {

            string avatarUrl = SiteUrls.Instance().UserAvatarUrl(user, avatarSizeType, enableCachingInClient);

            TagBuilder img = new TagBuilder("img");
            if (htmlAttributes != null)
                img.MergeAttributes(htmlAttributes);

            img.MergeAttribute("src", avatarUrl);
            if (user != null)
            {
                img.MergeAttribute("alt", user.DisplayName);
                if (isShowTitle)
                {
                    img.MergeAttribute("title", user.DisplayName);
                }
            }

            IUserProfileSettingsManager userProfileSettingsManager = DIContainer.Resolve<IUserProfileSettingsManager>();
            UserProfileSettings userProfileSettings = userProfileSettingsManager.GetUserProfileSettings();

            TagBuilder div = new TagBuilder("div");
            switch (avatarSizeType)
            {
                case AvatarSizeType.Big:
                    img.MergeAttribute("width", userProfileSettings.AvatarWidth.ToString());
                    div.AddCssClass("tn-avatar-big");
                    break;
                case AvatarSizeType.Medium:
                    img.MergeAttribute("width", userProfileSettings.MediumAvatarWidth.ToString());
                    div.AddCssClass("tn-avatar-medium");
                    break;
                case AvatarSizeType.Small:
                    img.MergeAttribute("width", userProfileSettings.SmallAvatarWidth.ToString());
                    div.AddCssClass("tn-avatar");
                    break;
                case AvatarSizeType.Micro:
                    img.MergeAttribute("width", userProfileSettings.MicroAvatarWidth.ToString());
                    div.AddCssClass("tn-avatar-mini");
                    break;
            }

            if (!string.IsNullOrEmpty(link) && user != null)
            {
                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", link);

                if (navigateTarget != HyperLinkTarget._self)
                    a.MergeAttribute("target", navigateTarget.ToString());

                if (isShowUserCard)
                {
                    a.MergeAttribute("plugin", "tipsyHoverCard");
                    a.MergeAttribute("data-user-card-url", SiteUrls.Instance()._UserCard(user.UserId));
                    a.MergeAttribute("outerclass", "tn-user-card");
                }

                a.InnerHtml = img.ToString(TagRenderMode.SelfClosing);
                div.InnerHtml = a.ToString();
                return new MvcHtmlString(div.ToString());
            }
            else
            {
                div.InnerHtml = img.ToString(TagRenderMode.SelfClosing);
                return new MvcHtmlString(div.ToString());
            }
        }
        #endregion
    }

    /// <summary>
    /// 超级链接Target
    /// </summary>
    public enum HyperLinkTarget
    {
        /// <summary>
        /// 将内容呈现在一个没有框架的新窗口中
        /// </summary>
        _blank,

        /// <summary>
        /// 将内容呈现在含焦点的框架中
        /// </summary>
        _self,

        /// <summary>
        /// 将内容呈现在上一个框架集父级中
        /// </summary>
        _parent,

        /// <summary>
        /// 将内容呈现在没有框架的全窗口中
        /// </summary>
        _top

    }
}
