//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Configuration;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.Mvc;

namespace Spacebuilder.Bar
{
    public class UrlRoutingRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Bar"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            #region Channel
            context.MapRoute(
              "Channel_Bar_Home", // Route name
              "Bar" + extensionForOldIIS, // URL with parameters
              new { controller = "Bar", action = "Home" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Bar_SectionDetail", // Route name
                "Bar/s-{sectionId}" + extensionForOldIIS, // URL with parameters
                new { controller = "Bar", action = "SectionDetail" }, // Parameter defaults
                new { sectionId = @"(\d+)|(\{\d+\})" }
            );

            context.MapRoute(
                "Channel_Bar_ThreadDetail", // Route name
                "Bar/t-{threadId}" + extensionForOldIIS, // URL with parameters
                new { controller = "Bar", action = "ThreadDetail" }, // Parameter defaults
                new { threadId = @"(\d+)|(\{\d+\})" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Bar_UserBar", // Route name
                "Bar/u-{spaceKey}" + extensionForOldIIS, // URL with parameters
                new { controller = "Bar", action = "UserThreads" }
            );

            context.MapRoute(
                "Channel_Bar_Tag", // Route name
                "Bar/Tag/{tagName}" + extensionForOldIIS, // URL with parameters
                new { controller = "Bar", action = "ListsByTag" } // Parameter defaults
            );
            context.MapRoute(
                "Channel_Bar_Common", // Route name
                "Bar/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Bar", action = "Home" } // Parameter defaults
            );

            #endregion

            #region GroupSpace

            context.MapRoute(
                "Group_Bar_SectionDetail", // Route name
                "g/{spaceKey}/bar/home" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceBar", action = "SectionDetail", CurrentNavigationId = "13101201" } // Parameter defaults
            );

            context.MapRoute(
                "Group_Bar_ThreadDetail", // Route name
                "g/{spaceKey}/bar/t-{threadId}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceBar", action = "Detail" }, // Parameter defaults
                new { threadId = @"(\d+)|(\{\d+\})" } // Parameter defaults
            );

            context.MapRoute(
                "Group_Bar_UserBar", // Route name
                "g/{spaceKey}/bar/MyPosts" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceBar", action = "UserThreads", CurrentNavigationId = "13101201" }
            );

            context.MapRoute(
                "Group_Bar_Edit", // Route name
                "g/{spaceKey}/bar/Edit" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceBar", action = "Edit" }
            );

            context.MapRoute(
                "Group_Bar_ManageThreads", // Route name
                "g/{spaceKey}/bar/ManageThreads" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceBar", action = "ManageThreads", CurrentNavigationId = "13101205" } // Parameter defaults
            );

            context.MapRoute(
                "Group_Bar_Tag", // Route name
                "g/{spaceKey}/bar/Tag/{tagName}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceBar", action = "ListByTag" } // Parameter defaults
            );
            context.MapRoute(
                "Group_Bar_Common", // Route name
                "g/{spaceKey}/bar/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceBar", action = "SectionDetail", CurrentNavigationId = "13101201" } // Parameter defaults
            );

            #endregion

            #region ControlPanel

            context.MapRoute(
                "ControlPanel_Bar_Home", // Route name
                "ControlPanelBar" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelBar", action = "ManageThreads", CurrentNavigationId = "20101201" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Bar_Common", // Route name
                "ControlPanelBar/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelBar", CurrentNavigationId = "20000010" } // Parameter defaults
            );

            //群组贴吧管理帖子
            context.MapRoute(
              "ControlPanel_GroupBar_ManageThreads", // Route name
              "ControlPanelGroupBar/ManageThreads" + extensionForOldIIS, // URL with parameters
              new { controller = "ControlPanelBar", action = "ManageThreads", CurrentNavigationId = "20101101", tenantTypeId = TenantTypeIds.Instance().Group() } // Parameter defaults
            );

            //群组贴吧管理回帖
            context.MapRoute(
              "ControlPanel_GroupBar_ManagePosts", // Route name
              "ControlPanelGroupBar/ManagePosts" + extensionForOldIIS, // URL with parameters
              new { controller = "ControlPanelBar", action = "ManagePosts", CurrentNavigationId = "20101101", tenantTypeId = TenantTypeIds.Instance().Group() } // Parameter defaults
            );

            #endregion

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateBarThread", TenantTypeIds.Instance().BarThread()), // Route name
                "BarActivity/CreateThread/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "BarActivity", action = "_CreateBarThread" } // Parameter defaults
            );

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateBarPost", TenantTypeIds.Instance().BarPost()), // Route name
                "BarActivity/CreatePost/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "BarActivity", action = "_CreateBarPost" } // Parameter defaults
            );

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateBarRating", TenantTypeIds.Instance().BarRating()), // Route name
                "BarActivity/CreateRating/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "BarActivity", action = "_CreateBarRating" } // Parameter defaults
            );

            #region Handler
            context.Routes.MapHttpHandler<BarUrlHandler>("BarUrl", "Service/Bar/BarUrl.ashx");
            #endregion

        }
    }
}