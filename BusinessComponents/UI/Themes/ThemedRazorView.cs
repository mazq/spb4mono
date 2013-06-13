//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using System.IO;
using System.Web.WebPages;

namespace Tunynet.UI
{
    /// <summary>
    /// 重写RazorView用于支持皮肤机制
    /// </summary>
    public class ThemedRazorView : BuildManagerCompiledView
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewPath"></param>
        /// <param name="layoutPath"></param>
        /// <param name="isPartialView"></param>
        /// <param name="applicationKey"></param>
        /// <param name="findLayoutPathOfThemeDelegate"></param>
        /// <param name="themeAppearance"></param>
        /// <param name="viewPageActivator"></param>
        public ThemedRazorView(ControllerContext controllerContext, string viewPath, string layoutPath, bool isPartialView, string applicationKey, Func<ThemeAppearance, string, string, string> findLayoutPathOfThemeDelegate, ThemeAppearance themeAppearance, IViewPageActivator viewPageActivator)
            : base(controllerContext, viewPath, viewPageActivator)
        {
            IsPartialView = isPartialView;
            ApplicationKey = applicationKey;
            if (!IsPartialView)
            {
                if (!string.IsNullOrEmpty(layoutPath))
                    OverridenLayoutPath = layoutPath;
                else
                {
                    OverridenLayoutPath = string.Empty;
                    ThemeAppearance = themeAppearance;
                    FindLayoutPathOfThemeDelegate = findLayoutPathOfThemeDelegate;
                }
            }
        }

        /// <summary>
        /// 从Controller/Action设置的Layout（已经经过视图引擎定位）
        /// </summary>
        public string OverridenLayoutPath { get; private set; }

        /// <summary>
        /// 利用视图引擎定位layout的委托
        /// </summary>
        Func<ThemeAppearance, string, string, string> FindLayoutPathOfThemeDelegate;

        /// <summary>
        /// 当前页面的ThemeAppearance
        /// </summary>
        public ThemeAppearance ThemeAppearance { get; private set; }

        /// <summary>
        /// 是否局部视图
        /// </summary>
        public bool IsPartialView { get; private set; }

        /// <summary>
        /// 应用模块标识
        /// </summary>
        public string ApplicationKey { get; private set; }

        /// <summary>
        /// 呈现View
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="writer"></param>
        /// <param name="instance"></param>
        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            ThemedWebViewPage webViewPage = instance as ThemedWebViewPage;
            if (webViewPage == null)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture, "The view at '{0}' must derive from WebViewPage, or WebViewPage<TModel>.",
                        ViewPath));
            }

            webViewPage.VirtualPath = ViewPath;
            webViewPage.ViewContext = viewContext;
            webViewPage.ViewData = viewContext.ViewData;
            webViewPage.ApplicationKey = ApplicationKey;

            webViewPage.InitHelpers();
            WebPageRenderingBase startPage = null;

            // 在View中设置的Layout也可以依据视图引擎进行定位
            if (!IsPartialView)
            {
                if (!string.IsNullOrEmpty(OverridenLayoutPath))
                    webViewPage.OverridenLayoutPath = OverridenLayoutPath;
                else
                {
                    webViewPage.IsPartialView = false;
                    webViewPage.ThemeAppearance = ThemeAppearance;
                    webViewPage.FindLayoutPathOfThemeDelegate = FindLayoutPathOfThemeDelegate;
                }
            }
            else
            {
                webViewPage.IsPartialView = true;
            }

            //禁用ViewStartPage
            //if (RunViewStartPages)
            //{
            //    startPage = StartPageLookup(webViewPage, RazorViewEngine.ViewStartFileName, ViewStartFileExtensions);
            //}

            webViewPage.ExecutePageHierarchy(new WebPageContext(context: viewContext.HttpContext, page: null, model: null), writer, startPage);
        }
    }
}
