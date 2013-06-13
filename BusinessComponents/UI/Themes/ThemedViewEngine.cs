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
using Tunynet.Caching;

using System.Web.Routing;

namespace Tunynet.UI
{
    /// <summary>
    /// 支持皮肤机制的视图引擎
    /// </summary>
    public class ThemedViewEngine : BuildManagerViewEngine
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ThemedViewEngine()
            : this(null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="viewPageActivator"><see cref="IViewPageActivator"/></param>
        public ThemedViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {

            FileExtensions = new[] { "cshtml" };
        }

        ///// <summary>
        ///// 呈现区域
        ///// </summary>
        //protected string PresentAreaKey { get; set; }

        ///// <summary>
        ///// 是应用模块
        ///// </summary>
        //protected bool IsApplication { get; set; }

        ///// <summary>
        ///// 应用模块标识
        ///// </summary>
        //protected bool ApplicationKey { get; set; }


        private ThemeService themeService = null;
        /// <summary>
        /// ThemeService
        /// </summary>
        protected virtual ThemeService ThemeService
        {
            get
            {
                if (themeService == null)
                    themeService = new ThemeService();

                return themeService;
            }

            set
            {
                themeService = value;
            }
        }


        #region 重写FindPartialView/FindView

        /// <summary>
        /// 重写FindPartialView
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="partialViewName"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (String.IsNullOrEmpty(partialViewName))
                throw new ArgumentException("viewName is required.", "viewName");

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string presentAreaKey;
            bool isApplication;
            GetPresentAreaKeyAndIsApplicationFromThemedAttribute(controllerContext, out presentAreaKey, out isApplication);
            string partialPath = null;
            if (isApplication)
            {
                string areaName = GetAreaName(controllerContext.RouteData);
                partialPath = GetViewPathOfApplication(partialViewName, controllerName, areaName);
            }
            else
            {
                ThemeAppearance themeAppearance = ThemeService.GetRequestTheme(presentAreaKey, controllerContext.RequestContext);
                if (themeAppearance == null)
                    throw new ApplicationException("请求的 ThemeAppearance 未找到.");

                partialPath = GetViewPathOfTheme(themeAppearance, partialViewName, controllerName);
            }

            #region CreatePartialView
            string applicationKey = null;
            if (isApplication)
                applicationKey = GetAreaName(controllerContext.RouteData);

            ThemedRazorView themedRazorView = new ThemedRazorView(controllerContext, partialPath, layoutPath: null, isPartialView: true, applicationKey: applicationKey,
                 findLayoutPathOfThemeDelegate: null, themeAppearance: null, viewPageActivator: ViewPageActivator);
            #endregion

            return new ViewEngineResult(themedRazorView, this);
        }

        /// <summary>
        /// 重写FindView
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentException("viewName is required.", "viewName");

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string presentAreaKey;
            bool isApplication;
            GetPresentAreaKeyAndIsApplicationFromThemedAttribute(controllerContext, out presentAreaKey, out isApplication);

            ThemeAppearance themeAppearance = ThemeService.GetRequestTheme(presentAreaKey, controllerContext.RequestContext);
            if (themeAppearance == null)
                throw new ApplicationException("请求的 ThemeAppearance 未找到.");

            string viewPath = null;
            string masterPath;
            if (isApplication)
            {
                string areaName = GetAreaName(controllerContext.RouteData);
                viewPath = GetViewPathOfApplication(viewName, controllerName, areaName);
                masterPath = GetLayoutPathOfTheme(themeAppearance, masterName, areaName);
            }
            else
            {
                viewPath = GetViewPathOfTheme(themeAppearance, viewName, controllerName);
                masterPath = GetLayoutPathOfTheme(themeAppearance, masterName, null);
            }

            #region CreateView
            string applicationKey = null;
            if (isApplication)
                applicationKey = GetAreaName(controllerContext.RouteData);

            ThemedRazorView themedRazorView = new ThemedRazorView(controllerContext, viewPath, layoutPath: masterPath, isPartialView: false, applicationKey: applicationKey,
                 findLayoutPathOfThemeDelegate: GetLayoutPathOfTheme, themeAppearance: themeAppearance, viewPageActivator: ViewPageActivator);
            #endregion

            return new ViewEngineResult(themedRazorView, this);
        }

        /// <summary>
        /// 重写CreatePartialView
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="partialPath"></param>
        /// <returns></returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return null;
        }

        /// <summary>
        /// 重写CreateView
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewPath"></param>
        /// <param name="masterPath"></param>
        /// <returns></returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return null;
        }


        /// <summary>
        /// 从ThemedAttribute获取PresentAreaKey及IsApplication
        /// </summary>
        /// <param name="controllerContext"><see cref="ControllerContext"/></param>
        private void GetPresentAreaKeyAndIsApplicationFromThemedAttribute(ControllerContext controllerContext, out string presentAreaKey, out bool isApplication)
        {
            string cacheKey = "PresentAreaKey2IsApplication" + controllerContext.Controller.GetType().FullName;
            KeyValuePair<string, bool> presentAreaKey2IsApplication = new KeyValuePair<string, bool>("", false);

            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            object presentAreaKey2IsApplicationObject = cacheService.Get(cacheKey);
            if (presentAreaKey2IsApplicationObject != null)
                presentAreaKey2IsApplication = (KeyValuePair<string, bool>)presentAreaKey2IsApplicationObject;

            if (string.IsNullOrEmpty(presentAreaKey2IsApplication.Key))
            {
                ThemedAttribute themedAttribute = controllerContext.Controller.GetType().GetCustomAttributes(typeof(ThemedAttribute), true).FirstOrDefault() as ThemedAttribute;
                if (themedAttribute != null)
                    presentAreaKey2IsApplication = new KeyValuePair<string, bool>(themedAttribute.PresentAreaKey, themedAttribute.IsApplication);
            }

            presentAreaKey = presentAreaKey2IsApplication.Key;
            isApplication = presentAreaKey2IsApplication.Value;
        }

        #endregion


        #region View/Layout定位

        /// <summary>
        /// 获取应用模块中的ViewPath
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="controllerName"></param>
        /// <param name="areaName">与ApplicationKey相同</param>
        /// <returns></returns>
        private string GetViewPathOfApplication(string viewName, string controllerName, string areaName)
        {
            string cacheKey = "viewPath:" + areaName + "," + controllerName + "," + viewName;
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            string viewPath = cacheService.Get<string>(cacheKey);

            if (viewPath == null)
            {
                if (IsSpecificPath(viewName))
                {
                    viewPath = GetPathFromSpecificName(viewName);
                }
                else
                {
                    string[] applicationViewLocationFormats = new string[]
                    {
                        //{2}:AreaName(ApplicationKey)，{1}:ControllerName，{0}:ViewName
                        "~/Applications/{2}/Views/{1}/{0}.cshtml",  
                        "~/Applications/{2}/Views/{0}.cshtml",
                        "~/Themes/Shared/Views/{0}.cshtml"
                    };

                    foreach (var viewLocationFormat in applicationViewLocationFormats)
                    {
                        string virtualPath = string.Format(viewLocationFormat, viewName, controllerName, areaName);
                        if (VirtualPathProvider.FileExists(virtualPath))
                        {
                            viewPath = virtualPath;
                            break;
                        }
                    }
                }

                if (viewPath != null)
                    cacheService.Add(cacheKey, viewPath, CachingExpirationType.Stable);
            }

            if (viewPath == null)
            {
                throw new ExceptionFacade(new ResourceExceptionDescriptor("The ViewName {0} Of {1} could not be found!", viewName, areaName));
            }

            return viewPath;
        }


        /// <summary>
        /// 获取应用模块中的ViewPath
        /// </summary>
        /// <remarks>
        /// 应用模块以外的功能View定位忽略Area
        /// </remarks>
        /// <param name="themeAppearance"></param>
        /// <param name="viewName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        private string GetViewPathOfTheme(ThemeAppearance themeAppearance, string viewName, string controllerName)
        {
            StringBuilder cacheKeyBuilder = new StringBuilder("viewPath:");
            cacheKeyBuilder.Append(themeAppearance.PresentAreaKey);
            cacheKeyBuilder.AppendFormat(",{0}", themeAppearance.ThemeKey);
            cacheKeyBuilder.AppendFormat(",{0}", controllerName);
            cacheKeyBuilder.AppendFormat(",{0}", viewName);

            string cacheKey = cacheKeyBuilder.ToString();


            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            string viewPath = cacheService.Get<string>(cacheKey);

            if (viewPath == null)
            {
                if (IsSpecificPath(viewName))
                {
                    viewPath = GetPathFromSpecificName(viewName);
                }
                else
                {
                    //{3}:PresentAreaKey，{2}:ThemeKey，{1}:ControllerName，{0}:ViewName

                    List<string> viewLocations = new List<string>();
                    viewLocations.Add(string.Format("~/Themes/{3}/{2}/Views/{1}/{0}.cshtml", viewName, controllerName, themeAppearance.ThemeKey, themeAppearance.PresentAreaKey));

                    //如果Theme有Parent，则把Parent的路径也作为查找路径
                    Theme theme = ThemeService.GetTheme(themeAppearance.PresentAreaKey, themeAppearance.ThemeKey);
                    if (!string.IsNullOrEmpty(theme.Parent))
                        viewLocations.Add(string.Format("~/Themes/{3}/{2}/Views/{1}/{0}.cshtml", viewName, controllerName, theme.Parent, themeAppearance.PresentAreaKey));

                    //最后在~/Themes/Shared/Views/中查找
                    viewLocations.Add(string.Format("~/Themes/Shared/Views/{0}.cshtml", viewName));

                    foreach (var viewLocation in viewLocations)
                    {
                        if (VirtualPathProvider.FileExists(viewLocation))
                        {
                            viewPath = viewLocation;
                            break;
                        }
                    }
                }

                if (viewPath != null)
                    cacheService.Add(cacheKey, viewPath, CachingExpirationType.Stable);
            }

            if (viewPath == null)
            {
                throw new ExceptionFacade(new ResourceExceptionDescriptor("The ViewName {0} Of the controllerName {0} could not be found!", viewName, controllerName));
            }

            return viewPath;
        }

        /// <summary>
        /// 获取Layout具体地址
        /// </summary>
        /// <param name="themeAppearance"></param>
        /// <param name="layoutName">布局文件名称不要带.cshtml</param>
        /// <param name="applicationKey">应用模块标识</param>
        private string GetLayoutPathOfTheme(ThemeAppearance themeAppearance, string layoutName, string applicationKey)
        {
            if (string.IsNullOrEmpty(layoutName))
                return String.Empty;

            bool isFromApplication = !string.IsNullOrEmpty(applicationKey);

            StringBuilder cacheKeyBuilder = new StringBuilder("layoutPath:");
            cacheKeyBuilder.Append(themeAppearance.PresentAreaKey);
            cacheKeyBuilder.AppendFormat(",{0}", themeAppearance.ThemeKey);
            cacheKeyBuilder.AppendFormat(",{0}", themeAppearance.PresentAreaKey);
            cacheKeyBuilder.AppendFormat(",{0}", layoutName);
            if (isFromApplication)
                cacheKeyBuilder.AppendFormat(",{0}", applicationKey);

            string cacheKey = cacheKeyBuilder.ToString();

            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            string layoutPath = cacheService.Get<string>(cacheKey);

            if (layoutPath == null)
            {
                if (IsSpecificPath(layoutName))
                {
                    layoutPath = GetPathFromSpecificName(layoutName);
                }
                else
                {
                    List<string> layoutLocations = new List<string>();

                    if (isFromApplication)
                        layoutLocations.Add(string.Format("~/Applications/{1}/Layouts/{0}.cshtml", layoutName, applicationKey));

                    //{2}:PresentAreaKey，{1}:ThemeKey，{0}:LayoutName
                    layoutLocations.Add(string.Format("~/Themes/{2}/{1}/Layouts/{0}.cshtml", layoutName, themeAppearance.ThemeKey, themeAppearance.PresentAreaKey));

                    //如果Theme有Parent，则把Parent的路径也作为查找路径
                    Theme theme = ThemeService.GetTheme(themeAppearance.PresentAreaKey, themeAppearance.ThemeKey);
                    if (!string.IsNullOrEmpty(theme.Parent))
                        layoutLocations.Add(string.Format("~/Themes/{2}/{1}/Layouts/{0}.cshtml", layoutName, theme.Parent, themeAppearance.PresentAreaKey));

                    //在呈现区域的公共Layouts路径查找
                    layoutLocations.Add(string.Format("~/Themes/{1}/Layouts/{0}.cshtml", layoutName, themeAppearance.PresentAreaKey));

                    //最后在~/Themes/Shared/Views/中查找
                    layoutLocations.Add(string.Format("~/Themes/Shared/Views/{0}.cshtml", layoutName));

                    foreach (var viewLocation in layoutLocations)
                    {
                        if (VirtualPathProvider.FileExists(viewLocation))
                        {
                            layoutPath = viewLocation;
                            break;
                        }
                    }
                }

                if (layoutPath != null)
                    cacheService.Add(cacheKey, layoutPath, CachingExpirationType.Stable);
            }

            if (layoutPath == null)
            {
                throw new ExceptionFacade(new ResourceExceptionDescriptor("The LayoutName {0} could not be found!", layoutName));
            }

            return layoutPath;
        }


        /// <summary>
        /// 检测绝对虚拟路径的ViewName是否存在
        /// </summary>
        private string GetPathFromSpecificName(string viewName)
        {
            string viewPath = viewName;
            if (VirtualPathProvider.FileExists(viewPath))
                return viewPath;
            else
                return null;
        }

        /// <summary>
        /// 是否属于绝对虚拟路径（以'~'或'/'开头）
        /// </summary>
        private static bool IsSpecificPath(string name)
        {
            char c = name[0];
            return (c == '~' || c == '/');
        }

        /// <summary>
        /// 从路由数据获取AreaName
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static string GetAreaName(RouteData routeData)
        {
            object area;
            if (routeData.DataTokens.TryGetValue("area", out area))
            {
                return area as string;
            }

            area = GetAreaName(routeData.Route);
            if (area == null)
            {
                routeData.Values.TryGetValue("area", out area);
            }
            return area as string;
        }

        /// <summary>
        /// 从路由数据获取AreaName
        /// </summary>
        /// <param name="route"><see cref="RouteBase"/></param>
        /// <returns>返回路由中的AreaName，如果无AreaName则返回null</returns>
        public static string GetAreaName(RouteBase route)
        {
            IRouteWithArea routeWithArea = route as IRouteWithArea;
            if (routeWithArea != null)
                return routeWithArea.Area;

            Route castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
                return castRoute.DataTokens["area"] as string;

            return null;
        }


        #endregion

    }
}
