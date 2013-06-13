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

namespace Tunynet.UI
{
    /// <summary>
    /// 皮肤解析程序，用于获取当前皮肤及加载皮肤的css
    /// </summary>
    public interface IThemeResolver
    {
        /// <summary>
        /// 获取请求页面使用的皮肤
        /// </summary>
        /// <param name="controllerContext"><see cref="RequestContext"/></param>
        ThemeAppearance GetRequestTheme(RequestContext controllerContext);

        /// <summary>
        /// 加载皮肤的css
        /// </summary>
        /// <param name="controllerContext"><see cref="RequestContext"/></param>
        void IncludeCss(RequestContext controllerContext);

        /// <summary>
        /// 验证当前用户是否修改皮肤的权限
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        bool Validate(long ownerId);

        /// <summary>
        /// 获取拥有者当前选中的皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <returns></returns>
        string GetThemeAppearance(long ownerId);

        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="ownerId">拥有者Id（如：用户Id、群组Id）</param>
        /// <param name="isUseCustomStyle">是否使用自定义皮肤</param>
        /// <param name="themeAppearance">themeKey与appearanceKey用逗号关联</param>
        void ChangeThemeAppearance(long ownerId, bool isUseCustomStyle, string themeAppearance);

    }
}
