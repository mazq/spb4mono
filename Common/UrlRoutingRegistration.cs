//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Configuration;
using System.Web.Mvc;
using Tunynet.Mvc;
using Tunynet.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 频道注册路由
    /// </summary>
    public class UrlRoutingRegistration : AreaRegistration
    {
        /// <summary>
        /// 分区名称
        /// </summary>
        public override string AreaName
        {
            get { return "Common"; }
        }

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            //对于IIS6.0默认配置不支持无扩展名的url
            string extensionForOldIIS = ".aspx";
            int iisVersion = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["IISVersion"], out iisVersion))
                iisVersion = 7;
            if (iisVersion >= 7)
                extensionForOldIIS = string.Empty;

            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();

            #region Channel

            context.MapRoute(
                "Channel_SiteHome", // Route name
                "", // URL with parameters
                new { controller = "Channel", action = siteSettings.EnableSimpleHome ? "SimpleHome" : "Home" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Home", // Route name
                "Home" + extensionForOldIIS, // URL with parameters
                new { controller = "Channel", action = "Home", CurrentNavigationId = "1000001" } // Parameter defaults
            );


            context.MapRoute(
                "Channel_SimpleHome", // Route name
                "SimpleHome" + extensionForOldIIS, // URL with parameters
                new { controller = "Channel", action = "SimpleHome" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_ShortUrl",
                "{alias}",
                new { controller = "Channel", action = "RedirectUrl" },
                new { alias = @"([0-9a-zA-Z]{6})|(\{\d+\})" }
            );

            context.MapRoute(
                 "Channel_AnnouncementDetail", // Route name
                 "Announcement/s-{announcementId}" + extensionForOldIIS, // URL with parameters
                 new { controller = "Channel", action = "AnnouncementDetail" }, // Parameter defaults
                 new { announcementId = @"(\d+)|(\{\d+\})" }
             );

            context.MapRoute(
                "Channel_Account_Comon", // Route name
                "Account/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Account", action = "Login" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_Common", // Route name
                "Channel/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Channel" } // Parameter defaults
            );

            context.MapRoute(
                "Channel_FindUser_Ranking", // Route name
                "FindUser/s-{sortBy}-{pageIndex}" + extensionForOldIIS, // URL with parameters
                new { controller = "FindUser", action = "Ranking" }, // Parameter defaults
                new { sortBy = @"(\w+)|(\{\w+\})", pageIndex = @"(\d+)|(\{\d+\})" }
            );

            context.MapRoute(
                "Channel_FindUser", // Route name
                "FindUser/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "FindUser" } // Parameter defaults
            );
            #endregion Channel

            #region UserSpace

            context.MapRoute(
               string.Format("ActivityDetail_{0}_FollowUser", TenantTypeIds.Instance().User()), // Route name
                "follow/activity/{ActivityId}" + extensionForOldIIS, // URL with parameters
                new { controller = "Channel", action = "_FollowUserActivity" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_SpaceHome", // Route name
                "u/{SpaceKey}" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpace", action = "SpaceHome", CurrentNavigationId = "11000102" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_MyHome", // Route name
                "u/{SpaceKey}/MyHome" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpace", action = "MyHome", CurrentNavigationId = "11000101" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Profile", // Route name
                "u/{spaceKey}/Profile" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpace", action = "PersonalInformation", CurrentNavigationId = "11000103" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_InviteFriend", // Route name
                "u/{spaceKey}/InviteFriend" + extensionForOldIIS, // URL with parameters
                new { controller = "Follow", action = "InviteFriend" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_IdentificationResult", // Route name
                "u/{spaceKey}/MyIdentifications" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpace", action = "IdentificationResult" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_EditIdentification", // Route name
                "u/{spaceKey}/ApplyIdentification" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpace", action = "EditIdentification" } // Parameter defaults
            );


            context.MapRoute(
                "UserSpace_Follow_Common", // Route name
                "u/{spaceKey}/Follow/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Follow" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_MessageCenter_Common", // Route name
                "u/{SpaceKey}/MessageCenter/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "MessageCenter", action = "ListMessageSessions" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Settings_EditUserProfile", // Route name
                "u/{SpaceKey}/Settings/EditUserProfile" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpaceSettings", action = "EditUserProfile" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Settings_ManageApplications", // Route name
                "u/{SpaceKey}/Settings/ManageApplications" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpaceSettings", action = "ManageApplications" } // Parameter defaults
            );



            context.MapRoute(
                "UserSpace_Settings_Common", // Route name
                "u/{SpaceKey}/Settings/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpaceSettings", action = "EditUserProfile" } // Parameter defaults
            );

            context.MapRoute(
             "UserSpace_Honour_PointRecords", // Route name
             "u/{SpaceKey}/Honour/PointRecords" + extensionForOldIIS, // URL with parameters
             new { controller = "Honour", action = "ListPointRecords" } // Parameter defaults
            );

            context.MapRoute(
             "UserSpace_Honour_PointRule", // Route name
             "u/{SpaceKey}/Honour/MyRank" + extensionForOldIIS, // URL with parameters
             new { controller = "Honour", action = "MyRank" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Honour_Common", // Route name
                "u/{SpaceKey}/Honour/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "Honour", action = "MyRank" } // Parameter defaults
            );

            context.MapRoute(
                "UserSpace_Common", // Route name
                "u/common/{spaceKey}/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "UserSpace" } // Parameter defaults
            );

            #endregion UserSpace

            #region ControlPanel

            context.MapRoute(
                "ControlPanel_Home", // Route name
                "ControlPanel/Home" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanel", action = "Home", CurrentNavigationId = "20000000" } // Parameter defaults
            );

            #region 运营

            context.MapRoute(
                "ControlPanel_Operation_Home", // Route name
                "ControlPanel/Operation" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "Home", CurrentNavigationId = "20000030" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageRecommendItems", // Route name
                "ControlPanel/Operation/ManageRecommendItems" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageRecommendItems", CurrentNavigationId = "20000032" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageRecommendUsers", // Route name
                "ControlPanel/Operation/ManageRecommendUsers" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageRecommendUsers", CurrentNavigationId = "20000033" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageLinks", // Route name
                "ControlPanel/Operation/ManageLinks" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageLinks", CurrentNavigationId = "20000034" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageAdvertisings", // Route name
                "ControlPanel/Operation/ManageAdvertisings" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageAdvertisings", CurrentNavigationId = "20000035" } // Parameter defaults
            );



            context.MapRoute(
                "ControlPanel_Operation_ManageAnnouncements", // Route name
                "ControlPanel/Operation/ManageAnnouncements" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageAnnouncements", CurrentNavigationId = "20000036" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageAccountTypes", // Route name
                "ControlPanel/Operation/ManageAccountTypes" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageAccountTypes", CurrentNavigationId = "20000037" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManagePointRecords", // Route name
                "ControlPanel/Operation/ManagePointRecords" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManagePointRecords", CurrentNavigationId = "20000038" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageSearchedTerms", // Route name
                "ControlPanel/Operation/ManageSearchedTerms" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageSearchedTerms", CurrentNavigationId = "20000039" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageImpeachReports", // Route name
                "ControlPanel/Operation/ManageImpeachReports" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageImpeachReports", CurrentNavigationId = "20000040" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageOperationLogs", // Route name
                "ControlPanel/Operation/ManageOperationLogs" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageOperationLogs", CurrentNavigationId = "20000041" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_ManageCustomMessage", // Route name
                "ControlPanel/Operation/ManageCustomMessage" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManageCustomMessage", CurrentNavigationId = "20000075" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_MassMessages", // Route name
                "ControlPanel/Operation/MassMessages" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "MassMessages", CurrentNavigationId = "20000076" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_Statistics", // Route name
                "ControlPanel/Operation/Statistics" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "CNZZStatistics", CurrentNavigationId = "20000077" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Operation_Common", // Route name
                "ControlPanel/Operation/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelOperation", action = "ManagePointRecords" } // Parameter defaults
            );

            #endregion

            #region 内容

            context.MapRoute(
                "ControlPanel_Content_Home", // Route name
                "ControlPanel/Content" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelContent", action = "Home", CurrentNavigationId = "20000010" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Content_ManageTags", // Route name
                "ControlPanel/Content/ManageTags" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelContent", action = "ManageTags", CurrentNavigationId = "20000014" } // Parameter defaults
            );

            context.MapRoute(
               "ControlPanel_Content_ManageSiteCategories", // Route name
               "ControlPanel/Content/ManageSiteCategories" + extensionForOldIIS, // URL with parameters
               new { controller = "ControlPanelContent", action = "ManageSiteCategories", CurrentNavigationId = "20000015" } // Parameter defaults
            );

            context.MapRoute(
               "ControlPanel_Content_ManageUserCategories", // Route name
               "ControlPanel/Content/ManageUserCategories" + extensionForOldIIS, // URL with parameters
               new { controller = "ControlPanelContent", action = "ManageUserCategories", CurrentNavigationId = "20000016" } // Parameter defaults
            );

            context.MapRoute(
               "ControlPanel_Content_ManageComments", // Route name
               "ControlPanel/Content/ManageComments" + extensionForOldIIS, // URL with parameters
               new { controller = "ControlPanelContent", action = "ManageComments", CurrentNavigationId = "20000017" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Content_Common", // Route name
                "ControlPanel/Content/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelContent" } // Parameter defaults
            );

            #endregion

            #region 用户

            context.MapRoute(
                "ControlPanel_User_Home", // Route name
                "ControlPanel/Users" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelUser", action = "Home", CurrentNavigationId = "20000020" } // Parameter defaults
            );


            context.MapRoute(
                "ControlPanel_User_ManageUser", // Route name
                "ControlPanel/User/ManageUser" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelUser", action = "ManageUsers", CurrentNavigationId = "20000022" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_User_ManageUserRoles", // Route name
                "ControlPanel/User/ManageUserRoles" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelUser", action = "ManageUserRoles", CurrentNavigationId = "20000023" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_User_ManageRanks", // Route name
                "ControlPanel/User/ManageRanks" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelUser", action = "ManageRanks", CurrentNavigationId = "20000024" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_User_ManageIdentifications", // Route name
                "ControlPanel/User/ManageIdentifications" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelUser", action = "ManageIdentifications", CurrentNavigationId = "20000025" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_User_Common", // Route name
                "ControlPanel/User/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelUser", action = "ManageUsers" } // Parameter defaults
            );

            #endregion

            #region 工具

            context.MapRoute(
                 "ControlPanel_Tool_Home", // Route name
                 "ControlPanel/Tool" + extensionForOldIIS, // URL with parameters
                 new { controller = "ControlPanelTool", action = "Home", CurrentNavigationId = "20000042" } // Parameter defaults
             );

            context.MapRoute(
                "ControlPanel_Tool_ManageTask", // Route name
                "ControlPanel/Tool/ManageTasks" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTool", action = "ManageTasks", CurrentNavigationId = "20000046" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Tool_UnloadAppDomain", // Route name
                "ControlPanel/Tool/UnloadAppDomain" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTool", action = "UnloadAppDomain", CurrentNavigationId = "20000047" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Tool_ManageIndex", // Route name
                "ControlPanel/Tool/ManageIndex" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTool", action = "ManageIndex", CurrentNavigationId = "20000044" } // Parameter defaults
            );


            context.MapRoute(
                "ControlPanel_Tool_ResetCache", // Route name
                "ControlPanel/Tool/ResetCache" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTool", action = "ResetCache", CurrentNavigationId = "20000045" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Tool_RebuildingThumbnails", // Route name
                "ControlPanel/Tool/RebuildingThumbnails" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTool", action = "RebuildingThumbnails", CurrentNavigationId = "20000048" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Tool_Common", // Route name
                "ControlPanel/Tool/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelTool" } // Parameter defaults
            );

            #endregion

            #region 设置

            context.MapRoute(
                 "ControlPanel_Settings_Home", // Route name
                 "ControlPanel/Settings" + extensionForOldIIS, // URL with parameters
                 new { controller = "ControlPanelSettings", action = "SiteSettings", CurrentNavigationId = "20000050" } // Parameter defaults
             );

            context.MapRoute(
                "ControlPanel_Settings_ManagePointItems", // Route name
                "ControlPanel/Settings/ManagePointItems" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManagePointItems", CurrentNavigationId = "20000052" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManagePermissionItems", // Route name
                "ControlPanel/Settings/ManagePermissionItems" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManagePermissionItems", CurrentNavigationId = "20000053" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManagePrivacyItems", // Route name
                "ControlPanel/Settings/ManagePrivacyItems" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManagePrivacyItems", CurrentNavigationId = "20000054" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageAuditItems", // Route name
                "ControlPanel/Settings/ManageAuditItems" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageAuditItems", CurrentNavigationId = "20000055" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_SiteSettings", // Route name
                "ControlPanel/Settings/SiteSettings" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "SiteSettings", CurrentNavigationId = "20000057" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_UserSettings", // Route name
                "ControlPanel/Settings/UserSettings" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "UserSettings", CurrentNavigationId = "20000058" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_AttachmentSettings", // Route name
                "ControlPanel/Settings/AttachmentSettings" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "AttachmentSettings", CurrentNavigationId = "20000059" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageEmails", // Route name
                "ControlPanel/Settings/ManageEmails" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ListOutbox", CurrentNavigationId = "20000060" } // Parameter defaults
            );

            context.MapRoute(
               "ControlPanel_Settings_ManageEmailOtherSettings", // Route name
               "ControlPanel/Settings/ManageEmailOtherSettings" + extensionForOldIIS, // URL with parameters
               new { controller = "ControlPanelSettings", action = "ManageEmailOtherSettings", CurrentNavigationId = "20000060" } // Parameter defaults
           );

            context.MapRoute(
                "ControlPanel_Settings_MessagesSettings", // Route name
                "ControlPanel/Settings/MessagesSettings" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "MessagesSettings", CurrentNavigationId = "20000061" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ReminderSettings", // Route name
                "ControlPanel/Settings/ReminderSettings" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ReminderSettings", CurrentNavigationId = "20000062" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_PauseSiteSettings", // Route name
                "ControlPanel/Settings/PauseSiteSettings" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "PauseSiteSettings", CurrentNavigationId = "20000063" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageApplications", // Route name
                "ControlPanel/Settings/ManageApplications" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageApplications", CurrentNavigationId = "20000065" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageNavigations", // Route name
                "ControlPanel/Settings/ManageNavigations" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageNavigations", CurrentNavigationId = "20000066" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageThemes", // Route name
                "ControlPanel/Settings/ManageThemes" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageThemes", CurrentNavigationId = "20000067" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageAreas", // Route name
                "ControlPanel/Settings/ManageAreas" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageAreas", CurrentNavigationId = "20000071" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageSchools", // Route name
                "ControlPanel/Settings/ManageSchools" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageSchools", CurrentNavigationId = "20000072" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageEmotionCategories", // Route name
                "ControlPanel/Settings/ManageEmotionCategories" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageEmotionCategories", CurrentNavigationId = "20000073" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_ManageSensitiveWords", // Route name
                "ControlPanel/Settings/ManageSensitiveWords" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManageSensitiveWords", CurrentNavigationId = "20000074" } // Parameter defaults
            );

            context.MapRoute(
                "ControlPanel_Settings_Common", // Route name
                "ControlPanel/Settings/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanelSettings", action = "ManagePointItems", CurrentNavigationId = "20000050" } // Parameter defaults
            );

            #endregion


            context.MapRoute(
                "ControlPanel_Common", // Route name
                "ControlPanel/{action}" + extensionForOldIIS, // URL with parameters
                new { controller = "ControlPanel", action = "Home" } // Parameter defaults
            );

            #endregion ControlPanel

            #region Handler

            context.Routes.MapHttpHandler<CaptchaHttpHandler>("Captcha", "Handlers/CaptchaImage.ashx");
            context.Routes.MapHttpHandler<AttachmentAuthorizeHandler>("AttachmentAuthorize", "Handlers/AttachmentAuthorize.ashx");
            context.Routes.MapHttpHandler<AttachmentDownloadHandler>("Attachment", "Handlers/Attachment.ashx");
            context.Routes.MapHttpHandler<CustomStyleHandler>("CustomStyle", "Handlers/CustomStyle.ashx");
            //context.Routes.MapHttpHandler<UserAvatarHandler>("UserAvatar", "Handlers/UserAvatar.ashx");
            //context.Routes.MapHttpHandler<ImageHandler>("Image", "Handlers/Image.ashx");
            //context.Routes.MapHttpHandler<LogoHandler>("Logo", "Handlers/Logo.ashx");

            #endregion Handler
        }
    }
}