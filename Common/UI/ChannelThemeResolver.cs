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
using Tunynet.Common;
using Tunynet.UI;
using Tunynet;
using Spacebuilder.Common;

namespace Spacebuilder.UI
{
    /// <summary>
    /// 当前皮肤获取器接口
    /// </summary>
    public class ChannelThemeResolver : IThemeResolver
    {

        #region IThemeSelector 成员

        public ThemeAppearance GetRequestTheme(RequestContext controllerContext)
        {
            string themeKey = null;
            string appearanceKey = null;
            SiteSettings siteSettings = DIContainer.Resolve<ISiteSettingsManager>().Get();
            if (!string.IsNullOrEmpty(siteSettings.SiteTheme) && !string.IsNullOrEmpty(siteSettings.SiteThemeAppearance))
            {
                themeKey = siteSettings.SiteTheme;
                appearanceKey = siteSettings.SiteThemeAppearance;
            }
            else
            {
                PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.Channel);
                if (pa != null)
                {
                    themeKey = pa.DefaultThemeKey;
                    appearanceKey = pa.DefaultAppearanceKey;
                }
            }
            return new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.Channel, themeKey, appearanceKey);
        }

        public void IncludeCss(RequestContext controllerContext)
        {
            ThemeAppearance themeAppearance = GetRequestTheme(controllerContext);
            if (themeAppearance == null)
                return;

            PresentArea presentArea = new PresentAreaService().Get(themeAppearance.PresentAreaKey);
            if (presentArea == null)
                return;

            string themeCssPath = string.Format("{0}/{1}/theme.css", presentArea.ThemeLocation, themeAppearance.ThemeKey);
            string appearanceCssPath = string.Format("{0}/{1}/Appearances/{2}/appearance.css", presentArea.ThemeLocation, themeAppearance.ThemeKey, themeAppearance.AppearanceKey);

            IPageResourceManager resourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            resourceManager.IncludeCss(themeCssPath, renderPriority: ResourceRenderPriority.Last);
            resourceManager.IncludeCss(appearanceCssPath, renderPriority: ResourceRenderPriority.Last);
        }

        #endregion

        /// <summary>
        /// 获取拥有者当前选中的皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <returns></returns>
        public string GetThemeAppearance(long ownerId)
        {
            SiteSettings siteSettings = DIContainer.Resolve<ISiteSettingsManager>().Get();
            return siteSettings.SiteTheme + "," + siteSettings.SiteThemeAppearance;
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
            if (currentUser.IsInRoles(RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator()))
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
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            string themeKey = null;
            string appearanceKey = null;
            string[] themeAppearanceArray = themeAppearance.Split(',');
            if (themeAppearanceArray.Count() == 2)
            {
                themeKey = themeAppearanceArray[0];
                appearanceKey = themeAppearanceArray[1];
            }
            else
            {
                PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.Channel);
                if (pa != null)
                {
                    themeKey = pa.DefaultThemeKey;
                    appearanceKey = pa.DefaultAppearanceKey;
                }
            }
            siteSettings.SiteTheme = themeKey;
            siteSettings.SiteThemeAppearance = appearanceKey;
            siteSettingsManager.Save(siteSettings);
        }
    }
}