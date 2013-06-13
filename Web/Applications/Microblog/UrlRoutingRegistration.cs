//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Spacebuilder.Common;
using Tunynet.Mvc;
using Tunynet.Common;

namespace Spacebuilder.Microblog
{
    public class UrlRoutingRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Microblog"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //广场首页是否以瀑布流显示
            string isWaterfall = MicroblogConfig.GetConfig(1001).ApplicationElement.Element("microblogSquare").Attribute("isWaterfall").Value;
            string actionName = isWaterfall == "true" ? "Waterfall" : "Microblog";

            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            #region UserSpace

            context.MapRoute(
                "UserSpace_Microblog_Home", // Route name
                "u/{SpaceKey}/microblog/home" + extensionForOldIIS, // URL with parameters
                new { controller = "Microblog", action = "Mine", CurrentNavigationId = "11100102" } // Parameter defaults
            );

            context.MapRoute(
                "ApplicationCount_Microblog", // Route name
                "u/{SpaceKey}/microblog/home" + extensionForOldIIS, // URL with parameters
                new { controller = "Microblog", action = "Mine", CurrentNavigationId = "11100102" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Microblog_AtMe", // Route name
                "u/{SpaceKey}/microblog/atme" + extensionForOldIIS, // URL with parameters
                new { controller = "Microblog", action = "ListReferred", CurrentNavigationId = "11100103" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Microblog_Favorites", // Route name
                "u/{SpaceKey}/microblog/Favorites" + extensionForOldIIS, // URL with parameters
                new { controller = "Microblog", action = "ListFavorites", CurrentNavigationId = "11100104" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Microblog_Detail", // Route name
                "u/{SpaceKey}/microblog/{microblogId}" + extensionForOldIIS, // URL with parameters
                new { controller = "Microblog", action = "Detail" }, // Parameter defaults
                new { microblogId = @"(\d+)|(\{\d+\})" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Microblog_Common", // Route name
                "u/{SpaceKey}/microblog/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Microblog", action = "ListMy" } // Parameter defaults

            );

            #region 动态列表控件路由

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateMicroblog", TenantTypeIds.Instance().Microblog()), // Route name
                "microblog/activity/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "MicroblogActivity", action = "_Microblog_Activity" } // Parameter defaults
                );

            context.MapRoute(
              string.Format("MicroblogActivity_Common", TenantTypeIds.Instance().Microblog()), // Route name
               "microblogactivity/{action}" + extensionForOldIIS, // URL with parameters
               new { controller = "MicroblogActivity" } // Parameter defaults
            );

            #endregion

            context.MapRoute(
                "Microblog_Common", // Route name
                "microblog/common/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Microblog" } // Parameter defaults
            );

            #endregion UserSpace

            #region Channel

            context.MapRoute(
                "Channel_Microblog_Square", // Route name
                "microblog/s-{sortBy}-{tagGroupId}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelMicroblog", action = "Microblog" }, // Parameter defaults
                new { sortBy = @"(\w+)|(\{\w+\})", tagGroupId = @"(\d+)|(\{\d+\})" } // Parameter defaults
            );

            context.MapRoute(
              "Channel_Microblog", // Route name
              "microblog" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelMicroblog", action = actionName } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Microblog_Topic", // Route name
                "microblog/t-{topic}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelMicroblog", action = "Topic" }, // Parameter defaults
                new { topic = @"(\S+)|(\{\S+\})" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Microblog_Common", // Route name
                "microblog/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelMicroblog", action = "Home" } // Parameter defaults
            );

            #endregion

            #region GroupSpace

            context.MapRoute(
                "Group_Microblog_Tag", // Route name
                "g/{SpaceKey}/microblog/t-{tagName}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceMicroblog", action = "TopicDetail" }, // Parameter defaults
                new { tagName = @"(\S+)|(\{\S+\})" } // Parameter defaults
            );

            context.MapRoute(
                "Group_Microblog_Detail", // Route name
                "g/{SpaceKey}/microblog/{microblogId}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceMicroblog", action = "Detail" }, // Parameter defaults
                new { microblogId = @"(\d+)|(\{\d+\})" } // Parameter defaults
            );

            context.MapRoute(
                "Group_Microblog_Common", // Route name
                "g/{SpaceKey}/microblog/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceMicroblog" } // Parameter defaults
            );
            #endregion

            #region ControlPanel

            context.MapRoute(
                "ControlPanel_Microblog_Home", // Route name
                "controlpanelmicroblogs" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelMicroblog", action = "ManageMicroblogs", CurrentNavigationId = "20100101" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Microblog_Common", // Route name
                "controlpanelmicroblog/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelMicroblog", CurrentNavigationId = "20000010" } // Parameter defaults
            );

            //群组微博中管理发言
            context.MapRoute(
               "ControlPanel_GroupMicroblog_Common", // Route name
               "ControlPanelGroupMicroblogs/ManageMicroblogs" + extensionForOldIIS, // URL with parameters
               new { controller = "ControlPanelMicroblog", action = "ManageMicroblogs", CurrentNavigationId = "20101101", tenantTypeId = TenantTypeIds.Instance().Group() } // Parameter defaults
           );
            #endregion


        }
    }
}
