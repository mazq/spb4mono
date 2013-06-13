//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet.UI;
using Tunynet;

namespace Spacebuilder.UI
{
    /// <summary>
    /// 当前皮肤获取器接口
    /// </summary>
    public class UserSpaceThemeResolver : IThemeResolver
    {

        #region IThemeSelector 成员

        /// <summary>
        /// 获取请求页面使用的皮肤
        /// </summary>
        /// <param name="controllerContext"><see cref="RequestContext"/></param>
        ThemeAppearance IThemeResolver.GetRequestTheme(RequestContext controllerContext)
        {
            string spaceKey = controllerContext.GetParameterFromRouteDataOrQueryString("spaceKey");
            IUserService userService = DIContainer.Resolve<IUserService>();
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                throw new ExceptionFacade(new ResourceExceptionDescriptor().WithUserNotFound(spaceKey, 0));

            string themeKey = null;
            string appearanceKey = null;
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.UserSpace);
            if (pa == null)
                throw new ExceptionFacade("找不到用户空间呈现区域");

            if (pa.EnableThemes)
            {
                if (user.IsUseCustomStyle)
                {
                    themeKey = "Default";
                    appearanceKey = "Default";
                }
                else
                {
                    string[] themeAppearanceArray = user.ThemeAppearance.Split(',');
                    var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.UserSpace, user.ThemeAppearance);

                    if (appearance != null && themeAppearanceArray.Count() == 2)
                    {
                        themeKey = themeAppearanceArray[0];
                        appearanceKey = themeAppearanceArray[1];
                    }
                }
            }

            if (themeKey == null || appearanceKey == null)
            {
                themeKey = pa.DefaultThemeKey;
                appearanceKey = pa.DefaultAppearanceKey;
            }

            return new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.UserSpace, themeKey, appearanceKey);
        }

        /// <summary>
        /// 加载皮肤的css
        /// </summary>
        /// <param name="controllerContext"><see cref="RequestContext"/></param>
        void IThemeResolver.IncludeCss(RequestContext controllerContext)
        {
            string spaceKey = controllerContext.GetParameterFromRouteDataOrQueryString("SpaceKey");
            IUserService userService = DIContainer.Resolve<IUserService>();
            User user = userService.GetFullUser(spaceKey);
            if (user == null)
                throw new ExceptionFacade(new ResourceExceptionDescriptor().WithUserNotFound(spaceKey, 0));

            PresentArea presentArea = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.UserSpace);
            if (presentArea == null)
                return;

            string themeKey = null;
            string appearanceKey = null;

            IPageResourceManager resourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            if (!presentArea.EnableThemes)
            {
                string themeCssPath = string.Format("{0}/{1}/theme.css", presentArea.ThemeLocation, presentArea.DefaultThemeKey);
                string appearanceCssPath = string.Format("{0}/{1}/Appearances/{2}/appearance.css", presentArea.ThemeLocation, presentArea.DefaultThemeKey, presentArea.DefaultAppearanceKey);

                resourceManager.IncludeCss(themeCssPath, renderPriority: ResourceRenderPriority.Last);
                resourceManager.IncludeCss(appearanceCssPath, renderPriority: ResourceRenderPriority.Last);
                return;
            }

            if (user.IsUseCustomStyle)
            {
                var customStyleEntity = new CustomStyleService().Get(presentArea.PresentAreaKey, user.UserId);
                if (customStyleEntity == null)
                    return;
                CustomStyle customStyle = customStyleEntity.CustomStyle;
                if (customStyle == null)
                    return;
                string themeCssPath = string.Format("{0}/Custom/theme{1}.css", presentArea.ThemeLocation, customStyle.IsDark ? "-deep" : "");
                string appearanceCssPath = SiteUrls.Instance().CustomStyle(presentArea.PresentAreaKey, user.UserId);
                resourceManager.IncludeCss(themeCssPath, renderPriority: ResourceRenderPriority.Last);
                resourceManager.IncludeCss(appearanceCssPath, renderPriority: ResourceRenderPriority.Last);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(".tn-page-bg{");
                if (customStyle.IsUseBackgroundImage)
                {
                    builder.AppendLine("background-image:url('" + customStyle.BackgroundImageStyle.Url + @"');");
                    builder.AppendFormat("background-repeat:{0};\n", customStyle.BackgroundImageStyle.IsRepeat ? "repeat" : "no-repeat");
                    builder.AppendFormat("background-attachment:{0};\n", customStyle.BackgroundImageStyle.IsFix ? "fixed" : "scroll");
                    string position = "center";
                    switch (customStyle.BackgroundImageStyle.BackgroundPosition)
                    {
                        case BackgroundPosition.Left:
                            position = "left";
                            break;
                        case BackgroundPosition.Center:
                            position = "center";
                            break;
                        case BackgroundPosition.Right:
                            position = "right";
                            break;
                        default:
                            position = "center";
                            break;
                    }
                    builder.AppendFormat("background-position:{0} top;\n", position);
                }

                builder.AppendLine("}");
                builder.AppendLine("#tn-content{");
                builder.AppendLine("margin-top:" + customStyle.HeaderHeight.ToString() + "px;");
                builder.AppendLine("}");
                resourceManager.RegisterCssBlockAtHead(builder.ToString());
            }
            else
            {
                string[] themeAppearanceArray = user.ThemeAppearance.Split(',');
                var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.UserSpace, user.ThemeAppearance);

                if (appearance != null && themeAppearanceArray.Count() == 2)
                {
                    themeKey = themeAppearanceArray[0];
                    appearanceKey = themeAppearanceArray[1];
                }
                else
                {
                    themeKey = presentArea.DefaultThemeKey;
                    appearanceKey = presentArea.DefaultAppearanceKey;
                }

                string themeCssPath = string.Format("{0}/{1}/theme.css", presentArea.ThemeLocation, themeKey);
                string appearanceCssPath = string.Format("{0}/{1}/Appearances/{2}/appearance.css", presentArea.ThemeLocation, themeKey, appearanceKey);

                resourceManager.IncludeCss(themeCssPath, renderPriority: ResourceRenderPriority.Last);
                resourceManager.IncludeCss(appearanceCssPath, renderPriority: ResourceRenderPriority.Last);
            }
        }

        #endregion

        /// <summary>
        /// 获取用户当前选中的皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <returns></returns>
        public string GetThemeAppearance(long ownerId)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();
            User user = userService.GetFullUser(ownerId);
            if (user == null)
                throw new ExceptionFacade("找不到用户");
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.UserSpace);
            if (pa != null && !pa.EnableThemes)
            {
                return pa.DefaultThemeKey + "," + pa.DefaultAppearanceKey;
            }

            if (user.IsUseCustomStyle)
            {
                return "Default,Default";
            }
            else if (!string.IsNullOrEmpty(user.ThemeAppearance))
            {
                var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.UserSpace, user.ThemeAppearance);
                if (appearance != null)
                    return user.ThemeAppearance;
            }

            if (pa != null)
            {
                return pa.DefaultThemeKey + "," + pa.DefaultAppearanceKey;
            }

            return string.Empty;
        }


        /// <summary>
        /// 验证当前用户是否修改皮肤的权限
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public bool Validate(long ownerId)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.UserSpace);
            if (!pa.EnableThemes)
                return false;
            if (currentUser.UserId == ownerId || currentUser.IsInRoles(RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator()))
                return true;
            return false;
        }

        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">themeKey与appearanceKey用逗号关联</param>
        public void ChangeThemeAppearance(long ownerId, bool isUseCustomStyle, string themeAppearance)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();
            User user = userService.GetFullUser(ownerId);
            if (user == null)
                throw new ExceptionFacade("找不到用户");
            new ThemeService().ChangeThemeAppearanceUserCount(PresentAreaKeysOfBuiltIn.UserSpace, user.IsUseCustomStyle ? string.Empty : user.ThemeAppearance, isUseCustomStyle ? string.Empty : themeAppearance);
            userService.ChangeThemeAppearance(user.UserId, isUseCustomStyle, themeAppearance);
        }


    }
}
