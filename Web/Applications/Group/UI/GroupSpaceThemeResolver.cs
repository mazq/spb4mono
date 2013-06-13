//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System.Web.Routing;
using Tunynet.UI;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet;
using System.Linq;
using Spacebuilder.UI;
using System.Text;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 当前皮肤获取器接口
    /// </summary>
    public class GroupSpaceThemeResolver : IThemeResolver
    {

        #region IThemeResolver 成员

        ThemeAppearance IThemeResolver.GetRequestTheme(RequestContext controllerContext)
        {
            string spaceKey = controllerContext.GetParameterFromRouteDataOrQueryString("SpaceKey");
            GroupEntity group = new GroupService().Get(spaceKey);
            if (group == null)
                throw new ExceptionFacade("找不到群组");

            string themeKey = null;
            string appearanceKey = null;
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.GroupSpace);
            if (pa == null)
                throw new ExceptionFacade("找不到群组呈现区域");

            if (pa.EnableThemes)
            {
                if (group.IsUseCustomStyle)
                {
                    themeKey = "Default";
                    appearanceKey = "Default";
                }
                else
                {
                    string[] themeAppearanceArray = group.ThemeAppearance.Split(',');
                    var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.GroupSpace, group.ThemeAppearance);

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

            return new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.GroupSpace, themeKey, appearanceKey);
        }

        void IThemeResolver.IncludeCss(RequestContext controllerContext)
        {
            string spaceKey = controllerContext.GetParameterFromRouteDataOrQueryString("SpaceKey");
            GroupEntity group = new GroupService().Get(spaceKey);
            if (group == null)
                return;
            PresentArea presentArea = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.GroupSpace);
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

            if (group.IsUseCustomStyle)
            {
                var customStyleEntity = new CustomStyleService().Get(presentArea.PresentAreaKey, group.GroupId);
                if (customStyleEntity == null)
                    return;
                CustomStyle customStyle = customStyleEntity.CustomStyle;
                if (customStyle == null)
                    return;
                string themeCssPath = string.Format("{0}/Custom/theme{1}.css", presentArea.ThemeLocation, customStyle.IsDark ? "-deep" : "");
                string appearanceCssPath = SiteUrls.Instance().CustomStyle(presentArea.PresentAreaKey, group.GroupId);
                resourceManager.IncludeCss(themeCssPath, renderPriority: ResourceRenderPriority.Last);
                resourceManager.IncludeCss(appearanceCssPath, renderPriority: ResourceRenderPriority.Last);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(".tn-page-bg{");
                if (customStyle.IsUseBackgroundImage && !string.IsNullOrEmpty(customStyle.BackgroundImageStyle.Url))
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

                resourceManager.RegisterCssBlockAtHead(builder.ToString());
            }
            else
            {
                string[] themeAppearanceArray = group.ThemeAppearance.Split(',');
                var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.GroupSpace, group.ThemeAppearance);

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
            var groupService = new GroupService();
            GroupEntity group = groupService.Get(ownerId);
            if (group == null)
                return string.Empty;
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.GroupSpace);
            if (pa != null && !pa.EnableThemes)
            {
                return pa.DefaultThemeKey + "," + pa.DefaultAppearanceKey;
            }

            if (group.IsUseCustomStyle)
            {
                return "Default,Default";
            }
            else if (!string.IsNullOrEmpty(group.ThemeAppearance))
            {
                var appearance = new ThemeService().GetThemeAppearance(PresentAreaKeysOfBuiltIn.GroupSpace, group.ThemeAppearance);
                if (appearance != null)
                    return group.ThemeAppearance;
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
            PresentArea pa = new PresentAreaService().Get(PresentAreaKeysOfBuiltIn.GroupSpace);
            if (!pa.EnableThemes)
                return false;

            if (new Authorizer().Group_Manage(new GroupService().Get(ownerId)))
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
            var groupService = new GroupService();
            GroupEntity group = groupService.Get(ownerId);
            if (group == null)
                throw new ExceptionFacade("找不到群组");

            new ThemeService().ChangeThemeAppearanceUserCount(PresentAreaKeysOfBuiltIn.GroupSpace, group.IsUseCustomStyle ? string.Empty : group.ThemeAppearance, isUseCustomStyle ? string.Empty : themeAppearance);
            new GroupService().ChangeThemeAppearance(ownerId, isUseCustomStyle, themeAppearance);
        }
    }
}
