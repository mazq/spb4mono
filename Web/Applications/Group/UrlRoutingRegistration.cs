
using System.Configuration;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet.Mvc;
using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组路由设置
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Group"; }
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
              "Channel_Group_Home", // Route name
              "Group" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelGroup", action = "Home", CurrentNavigationId = "10101102" } // Parameter defaults
            );

            context.MapRoute(
              "Channel_Group_Create", // Route name
              "Group/Create" + extensionForOldIIS, // URL with parameters
              new { controller = "ChannelGroup", action = "Create" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Group_UserGroups", // Route name
                "Group/u-{spaceKey}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelGroup", action = "UserJoinedGroups", CurrentNavigationId = "10101103" }
            );

            context.MapRoute(
                "Channel_Group_FindGroup", // Route name
                "Group/FindGroup" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelGroup", action = "FindGroup", CurrentNavigationId = "10101104" }
            );

            context.MapRoute(
                "Channel_Group_Created", // Route name
                "Group/u-{spaceKey}/Created" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelGroup", action = "UserCreatedGroups", CurrentNavigationId = "10101103" }
            );

            context.MapRoute(
                "Channel_Group_Tag", // Route name
                "Group/Tag/{tagName}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelGroup", action = "ListByTag" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Group_Common", // Route name
                "Group/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelGroup" } // Parameter defaults
            );

            #endregion

            #region GroupActivity
            context.MapRoute(
                string.Format("ActivityDetail_{0}_CreateGroup", TenantTypeIds.Instance().Group()), // Route name
                "GroupActivity/CreateThread/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "ChannelGroup", action = "_CreateGroup" } // Parameter defaults
            );

            context.MapRoute(
               string.Format("ActivityDetail_{0}_CreateGroupMember", TenantTypeIds.Instance().User()), // Route name
               "GroupActivity/CreateGroupMember/{ActivityId}" + extensionForOldIIS, // URL with parameters
               new { controller = "ChannelGroup", action = "_CreateGroupMember" } // Parameter defaults
           );

            context.MapRoute(
               string.Format("ActivityDetail_{0}_JoinGroup", TenantTypeIds.Instance().User()), // Route name
               "GroupActivity/JoinGroup/{ActivityId}" + extensionForOldIIS, // URL with parameters
               new { controller = "ChannelGroup", action = "_JoinGroup" } // Parameter defaults
           );

            #endregion

            #region GroupSpace

            context.MapRoute(
                "GroupSpace_Member", // Route name
                "g/{SpaceKey}/Members" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpace", action = "Members", CurrentNavigationId = "13900180" } // Parameter defaults
            );

            #endregion

            #region GroupSpaceTheme

            //群组空间首页
            context.MapRoute(
                "GroupSpaceTheme_Home", // Route name
                "g/{SpaceKey}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceTheme", action = "Home", CurrentNavigationId = "13101101" } // Parameter defaults
            );

            context.MapRoute(
                "GroupSpaceTheme_Common", // Route name
                "grouptheme/{SpaceKey}/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceTheme", action = "Home" } // Parameter defaults
            );
            #endregion

            #region GroupSettings

            context.MapRoute(
                "GroupSpace_Settings_Common", // Route name
                "g/{SpaceKey}/settings/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpaceSettings", action = "ManageMembers" } // Parameter defaults
            );

            #endregion

            context.MapRoute(
                "GroupSpace_Common", // Route name
                "g/{SpaceKey}/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "GroupSpace", action = "Home" } // Parameter defaults
            );


            #region ControlPanel

            context.MapRoute(
                "ControlPanel_Group_Home", // Route name
                "ControlPanelGroup/ManageGroups" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelGroup", action = "ManageGroups", CurrentNavigationId = "20101101", tenantTypeId = TenantTypeIds.Instance().Group() } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Group_Common", // Route name
                "ControlPanelGroup/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelGroup", CurrentNavigationId = "20101110" } // Parameter defaults
            );

            #endregion



        }
    }
}