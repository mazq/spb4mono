using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spacebuilder.Environments;
using Tunynet.Logging;
using Tunynet.Utilities;
using Autofac;
using Tunynet.Caching;
using System.Reflection;
using Autofac.Integration.Mvc;
using Tunynet;
using Tunynet.Common;
using Tunynet.FileStore;
using Tunynet.UI;
using Spacebuilder.UI;
using Fasterflect;
using Spacebuilder.Common;
using Tunynet.Common.Configuration;

namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 应用程序启动时执行的事件
        /// </summary>
        protected void Application_Start()
        {
            if (!CheckInstallStatus())
            {
                InitializeSetup();
                return;
            }

            Starter.Start();

            //记录Windows事件日志
            //if (!EventLog.SourceExists(".NET Runtime"))
            //{
            //    EventLog.CreateEventSource(".NET Runtime", "Application");
            //}

            //EventLog log = new EventLog();
            //log.Source = ".NET Runtime";
            //log.WriteEntry("Spacebuilder应用程序已启动", EventLogEntryType.Information);
        }

        /// <summary>
        /// 应用程序终止时执行的事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Application_End(Object source, EventArgs e)
        {
            if (!CheckInstallStatus())
            {
                return;
            }

            //记录Windows事件日志
            //HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime",
            //                                                                            BindingFlags.NonPublic
            //                                                                            | BindingFlags.Static
            //                                                                            | BindingFlags.GetField,
            //                                                                            null,
            //                                                                            null,
            //                                                                            null);
            //if (runtime == null)
            //{
            //    return;
            //}

            //string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
            //                                                                 BindingFlags.NonPublic
            //                                                                 | BindingFlags.Instance
            //                                                                 | BindingFlags.GetField,
            //                                                                 null,
            //                                                                 runtime,
            //                                                                 null);

            //if (!EventLog.SourceExists(".NET Runtime"))
            //{
            //    EventLog.CreateEventSource(".NET Runtime", "Application");
            //}

            //EventLog log = new EventLog();
            //log.Source = ".NET Runtime";
            //log.WriteEntry(String.Format("Spacebuilder应用程序已关闭：\r\n{0}",
            //                             shutDownMessage),
            //                             EventLogEntryType.Warning);

            Starter.Stop();
        }

        public override void Init()
        {
            base.Init();
            this.PostAuthorizeRequest += new EventHandler(MvcApplication_PostAuthorizeRequest);
            DefaultModelBinder.ResourceClassKey = "Resource";
        }

        void MvcApplication_PostAuthorizeRequest(object sender, EventArgs e)
        {
            #region 设置当前请求的uiCulture

            //首先获取用户的culture设置，如果用户没有设置culture(或者设置的不合规范)则使用站点默认的culture
            string userCultureName = "zh-CN";

            System.Globalization.CultureInfo currentCulture = null;
            if (!string.IsNullOrEmpty(userCultureName))
            {
                try
                {
                    currentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(userCultureName);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = currentCulture;
                }
                catch { }
            }

            #endregion

        }

        #region 程序安装步骤初始化

        bool CheckInstallStatus()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;

            string ddd = AppDomain.CurrentDomain.BaseDirectory;

            System.IO.FileInfo FileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Applications\\Setup\\Views\\Install\\Start.cshtml");
            if (!FileInfo.Exists)
                return true;

            SqlConnection sqlconnection = new System.Data.SqlClient.SqlConnection();
            try
            {
                sqlconnection = new SqlConnection(connectionString);
                if (sqlconnection != null)
                {
                    sqlconnection.Open();

                    SqlCommand command = new SqlCommand("select count(*) from sysobjects where (xtype = 'u')", sqlconnection);
                    int count = 0;

                    int.TryParse(command.ExecuteScalar().ToString(), out count);
                    if (count <= 1)
                    {
                        return false;
                    }

                    command.CommandText = "select DecimalValue from tn_SystemData Where Datakey = 'SPBVersion'";

                    float version = 0;
                    float.TryParse(command.ExecuteScalar().ToString(), out version);

                    sqlconnection.Close();

                    return version > 0;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 初始化程序安装步骤
        /// </summary>
        /// <returns></returns>
        void InitializeSetup()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => new DefaultCacheService(new RuntimeMemoryCache(), 1.0F)).As<ICacheService>().SingleInstance();
            containerBuilder.RegisterAssemblyTypes(Assembly.Load("Spacebuilder.Common")).Where(t => t.Name.EndsWith("SettingsManager")).AsImplementedInterfaces().SingleInstance();
            containerBuilder.Register(c => new DefaultStoreProvider(@"~/Uploads")).As<IStoreProvider>().Named<IStoreProvider>("CommonStorageProvider").SingleInstance();
            //注册PageResourceManager
            bool pageResourceDebugEnabled = false;
            if (ConfigurationManager.AppSettings["PageResource:DebugEnabled"] != null)
            {
                if (!bool.TryParse(ConfigurationManager.AppSettings["PageResource:DebugEnabled"], out pageResourceDebugEnabled))
                    pageResourceDebugEnabled = false;
            }

            //注册标题图的配置
            TenantLogoSettings.RegisterSettings(LogoConfigManager.Instance().GetAllLogoConfigs());

            string resourceSite = null;
            if (ConfigurationManager.AppSettings["PageResource:Site"] != null)
                resourceSite = ConfigurationManager.AppSettings["PageResource:Site"];
            //用户业务逻辑
            containerBuilder.Register(c => new UserService()).As<IUserService>().SingleInstance();
            containerBuilder.Register(c => new PageResourceManager("Spacebuilder v4.0正式版") { DebugEnabled = pageResourceDebugEnabled, ResourceSite = resourceSite }).As<IPageResourceManager>().InstancePerHttpRequest();
            IContainer container = containerBuilder.Build();

            //将Autofac容器中的实例注册到mvc自带DI容器中（这样才获取到每请求缓存的实例）
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            DIContainer.RegisterContainer(container);

            //注册皮肤选择器
            Type themeResolverType = Type.GetType("Spacebuilder.Setup.SetupThemeResolver,Spacebuilder.Setup");
            ConstructorInvoker themeResolverConstructor = themeResolverType.DelegateForCreateInstance();
            IThemeResolver themeResolver = themeResolverConstructor() as IThemeResolver;
            ThemeService.RegisterThemeResolver("Channel", themeResolver);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemedViewEngine());

            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            RouteTable.Routes.MapRoute(
                "Channel_Home", // Route name
                string.IsNullOrEmpty(extensionForOldIIS) ? "" : "Home" + extensionForOldIIS,
                new { controller = "Install", action = "Start", area = "Setup" }
            );

            RouteTable.Routes.MapRoute(
                 "Install_Home", // Route name
                 string.IsNullOrEmpty(extensionForOldIIS) ? "Install" : "Install" + extensionForOldIIS,
                 new { controller = "Install", action = "Start", area = "Setup" }
             );

            RouteTable.Routes.MapRoute(
                "Install_Common",
                "Install/{action}" + extensionForOldIIS,
                new { controller = "Install", action = "Start", area = "Setup" });

            RouteTable.Routes.MapRoute(
                 "Upgrade_Home", // Route name
                 "Upgrade" + extensionForOldIIS,
                 new { controller = "Upgrade", action = "Ready", area = "Setup" }
             );

            RouteTable.Routes.MapRoute(
                "Upgrade_Common",
                "Upgrade/{action}" + extensionForOldIIS,
                new { controller = "Upgrade", action = "Ready", area = "Setup" });
        }

        #endregion
    }
}