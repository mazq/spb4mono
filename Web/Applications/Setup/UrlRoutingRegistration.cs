//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;

namespace Spacebuilder.Setup
{
    /// <summary>
    /// 日志路由设置
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {

        public override string AreaName
        {
            get { return "Setup"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;


            //RouteTable.Routes.MapRoute(
            //    "Channel_Home", // Route name
            //    string.IsNullOrEmpty(extensionForOldIIS) ? "Home" : "Home" + extensionForOldIIS,
            //    new { controller = "Install", action = "Start" }
            //);

            context.MapRoute(
                 "Install_Home", // Route name
                 "Install" + extensionForOldIIS,
                 new { controller = "Install", action = "Start" }
             );

            context.MapRoute(
                "Install_Common",
                "Install/{action}" + extensionForOldIIS,
                new { controller = "Install", action = "Start" });

            context.MapRoute(
                 "Upgrade_Home", // Route name
                 "Upgrade" + extensionForOldIIS,
                 new { controller = "Upgrade", action = "Ready" }
             );

            context.MapRoute(
                "Upgrade_Common",
                "Upgrade/{action}" + extensionForOldIIS,
                new { controller = "Upgrade", action = "Ready" });
        }
    }
}