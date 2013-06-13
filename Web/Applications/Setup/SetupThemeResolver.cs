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

namespace Spacebuilder.Setup
{
    /// <summary>
    /// 当前皮肤获取器接口
    /// </summary>
    public class SetupThemeResolver : IThemeResolver
    {

        #region IThemeSelector 成员

        public ThemeAppearance GetRequestTheme(RequestContext controllerContext)
        {
            return ThemeAppearance.New("Channel", "Default", "Default", string.Empty, string.Empty);
        }

        public void IncludeCss(RequestContext controllerContext)
        {
            ThemeAppearance themeAppearance = GetRequestTheme(controllerContext);
            if (themeAppearance == null)
                return;

            string themeCssPath = "~/Themes/Channel/Default/theme.css";
            string appearanceCssPath = "~/Themes/Channel/Default/Appearances/Default/appearance.css";

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
            return "Default,Default";
        }

        /// <summary>
        /// 验证当前用户是否修改皮肤的权限
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public bool Validate(long ownerId)
        {
            return true;
        }

        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">themeKey与appearanceKey用逗号关联</param>
        public void ChangeThemeAppearance(long ownerId, bool isUseCustomStyle, string themeAppearance)
        {
            
        }
    }
}