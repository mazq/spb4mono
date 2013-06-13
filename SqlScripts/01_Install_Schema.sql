IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_Subject]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_Subject]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_Body]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_Body]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_IsHyperLink]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_IsHyperLink]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_HyperLinkUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_HyperLinkUrl]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_IsEnabled]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Announcements_PresentAreaKey]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Announcements] DROP CONSTRAINT [DF_spb_Announcements_PresentAreaKey]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Announcements]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Announcements]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_CustomStyles]') AND type in (N'U'))
DROP TABLE [dbo].[spb_CustomStyles]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_EducationExperiences]') AND type in (N'U'))
DROP TABLE [dbo].[spb_EducationExperiences]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Identifications]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Identifications]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_IdentificationTypes]') AND type in (N'U'))
DROP TABLE [dbo].[spb_IdentificationTypes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_ImpeachReports]') AND type in (N'U'))
DROP TABLE [dbo].[spb_ImpeachReports]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Links]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Links]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Profiles_Gender]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Profiles] DROP CONSTRAINT [DF_spb_Profiles_Gender]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Profiles_BirthdayType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Profiles] DROP CONSTRAINT [DF_spb_Profiles_BirthdayType]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Profiles]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Profiles]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_WorkExperiences]') AND type in (N'U'))
DROP TABLE [dbo].[spb_WorkExperiences]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountBindings_Identification]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountBindings] DROP CONSTRAINT [DF_tn_AccountBindings_Identification]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountBindings_OauthTokenSecret]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountBindings] DROP CONSTRAINT [DF_tn_AccountBindings_OauthTokenSecret]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AccountBindings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AccountBindings]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_ThirdAccountGetterClassType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_ThirdAccountGetterClassType]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_AppKey]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_AppKey]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_AppSecret]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_AppSecret]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_IsShareMicroBlog1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_IsShareMicroBlog1]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_IsShareMicroBlog]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_IsShareMicroBlog]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_IsFollowMicroBlog]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_IsFollowMicroBlog]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_OfficialMicroBlogAccount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_OfficialMicroBlogAccount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AccountTypes_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AccountTypes] DROP CONSTRAINT [DF_tn_AccountTypes_IsEnabled]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AccountTypes]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AccountTypes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Activities]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ActivityItems_ItemName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ActivityItems] DROP CONSTRAINT [DF_tn_ActivityItems_ItemName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ActivityItems_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ActivityItems] DROP CONSTRAINT [DF_tn_ActivityItems_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ActivityItems_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ActivityItems] DROP CONSTRAINT [DF_tn_ActivityItems_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ActivityItems_IsUserReceived]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ActivityItems] DROP CONSTRAINT [DF_tn_ActivityItems_IsUserReceived]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ActivityItems_IsSiteReceived]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ActivityItems] DROP CONSTRAINT [DF_tn_ActivityItems_IsSiteReceived]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityItems]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ActivityItems]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityItemUserSettings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ActivityItemUserSettings]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivitySiteInbox]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ActivitySiteInbox]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityUserInbox]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ActivityUserInbox]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AdvertisingPosition_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AdvertisingPosition] DROP CONSTRAINT [DF_tn_AdvertisingPosition_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AdvertisingPosition_IsEnable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AdvertisingPosition] DROP CONSTRAINT [DF_tn_AdvertisingPosition_IsEnable]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AdvertisingPosition]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AdvertisingPosition]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Advertisings_Body]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Advertisings] DROP CONSTRAINT [DF_tn_Advertisings_Body]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Advertisings_IsEnable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Advertisings] DROP CONSTRAINT [DF_tn_Advertisings_IsEnable]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Advertisings_IsBlank]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Advertisings] DROP CONSTRAINT [DF_tn_Advertisings_IsBlank]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Advertisings_UseredPositionCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Advertisings] DROP CONSTRAINT [DF_tn_Advertisings_UseredPositionCount]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Advertisings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Advertisings]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AdvertisingsInPosition]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AdvertisingsInPosition]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationData_LongValue]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationData] DROP CONSTRAINT [DF_tn_ApplicationData_LongValue]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationData_DecimalValue]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationData] DROP CONSTRAINT [DF_tn_ApplicationData_DecimalValue]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationData_StringValue]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationData] DROP CONSTRAINT [DF_tn_ApplicationData_StringValue]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationData]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ApplicationData]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationInPresentAreaInstallations]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ApplicationInPresentAreaInstallations]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationInPresentAreaSettings_IsBuiltIn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationInPresentAreaSettings] DROP CONSTRAINT [DF_tn_ApplicationInPresentAreaSettings_IsBuiltIn]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationInPresentAreaSettings_IsAutoInstall]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationInPresentAreaSettings] DROP CONSTRAINT [DF_tn_ApplicationInPresentAreaSettings_IsAutoInstall]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationInPresentAreaSettings_IsGenerateData]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationInPresentAreaSettings] DROP CONSTRAINT [DF_tn_ApplicationInPresentAreaSettings_IsGenerateData]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationInPresentAreaSettings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ApplicationInPresentAreaSettings]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_AssociatedNavigationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_AssociatedNavigationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_OperationText]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_OperationText]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_ResourceName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_ResourceName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_NavigationUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_NavigationUrl]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_OnlyOwnerVisible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_OnlyOwnerVisible]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_IsLocked]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ApplicationManagementOperations_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ApplicationManagementOperations] DROP CONSTRAINT [DF_tn_ApplicationManagementOperations_IsEnabled]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationManagementOperations]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ApplicationManagementOperations]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Applications_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Applications] DROP CONSTRAINT [DF_tn_Applications_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Applications_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Applications] DROP CONSTRAINT [DF_tn_Applications_IsEnabled]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Applications_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Applications] DROP CONSTRAINT [DF_tn_Applications_IsLocked]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Applications_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Applications] DROP CONSTRAINT [DF_tn_Applications_DisplayOrder]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Applications]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Applications]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Areas_ParentCode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Areas] DROP CONSTRAINT [DF_tn_Areas_ParentCode]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Areas_Name]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Areas] DROP CONSTRAINT [DF_tn_Areas_Name]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Areas_PostCode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Areas] DROP CONSTRAINT [DF_tn_Areas_PostCode]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Areas_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Areas] DROP CONSTRAINT [DF_tn_Areas_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Areas_Depth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Areas] DROP CONSTRAINT [DF_tn_Areas_Depth]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Areas_ChildCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Areas] DROP CONSTRAINT [DF_tn_Areas_ChildCount]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Areas]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Areas]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AttachmentDownloadRecords_UserDisplayName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AttachmentDownloadRecords] DROP CONSTRAINT [DF_tn_AttachmentDownloadRecords_UserDisplayName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AttachmentDownloadRecords_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AttachmentDownloadRecords] DROP CONSTRAINT [DF_tn_AttachmentDownloadRecords_Price]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_AttachmentDownloadRecords_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_AttachmentDownloadRecords] DROP CONSTRAINT [DF_tn_AttachmentDownloadRecords_IP]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AttachmentDownloadRecords]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_FileName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_FileName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_FriendlyFileName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_FriendlyFileName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_MediaType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_MediaType]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_ContentType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_ContentType]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_FileLength]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_FileLength]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_Height]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_Height]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_Width]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_Width]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_Price]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_Password]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_Password]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attachments_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attachments] DROP CONSTRAINT [DF_tn_Attachments_IP]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attachments]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Attachments]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttitudeRecords]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AttitudeRecords]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Attitudes_ObjectId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Attitudes] DROP CONSTRAINT [DF_tn_Attitudes_ObjectId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attitudes]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Attitudes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AtUsers]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AtUsers]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AuditItems]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AuditItems]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AuditItemsInUserRoles]') AND type in (N'U'))
DROP TABLE [dbo].[tn_AuditItemsInUserRoles]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_ParentId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_ParentId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tn_Catego__Descr__2354350C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF__tn_Catego__Descr__2354350C]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_Depth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_Depth]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_ChildCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_ChildCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_ItemCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_ItemCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_PrivacyStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_PrivacyStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_AuditingStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_AuditingStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Categories_FeaturedItemId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Categories] DROP CONSTRAINT [DF_tn_Categories_FeaturedItemId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Categories]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Categories]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Comments_AsAnonymous]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Comments] DROP CONSTRAINT [DF_tn_Comments_AsAnonymous]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Comments]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_CommonOperations_NavigationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_CommonOperations] DROP CONSTRAINT [DF_tn_CommonOperations_NavigationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_CommonOperations_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_CommonOperations] DROP CONSTRAINT [DF_tn_CommonOperations_UserId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_CommonOperations]') AND type in (N'U'))
DROP TABLE [dbo].[tn_CommonOperations]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ContentPrivacySpecifyObjects]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ContentPrivacySpecifyObjects]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_EmailQueue_Priority]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_EmailQueue] DROP CONSTRAINT [DF_tn_EmailQueue_Priority]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_EmailQueue_IsBodyHtml]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_EmailQueue] DROP CONSTRAINT [DF_tn_EmailQueue_IsBodyHtml]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_EmailQueue_Subject]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_EmailQueue] DROP CONSTRAINT [DF_tn_EmailQueue_Subject]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_EmailQueue_NumberOfTries]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_EmailQueue] DROP CONSTRAINT [DF_tn_EmailQueue_NumberOfTries]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_EmailQueue_IsFailed]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_EmailQueue] DROP CONSTRAINT [DF_tn_EmailQueue_IsFailed]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_EmailQueue]') AND type in (N'U'))
DROP TABLE [dbo].[tn_EmailQueue]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_EmotionCategories_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_EmotionCategories] DROP CONSTRAINT [DF_tn_EmotionCategories_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_EmotionCategories_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_EmotionCategories] DROP CONSTRAINT [DF_tn_EmotionCategories_IsEnabled]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_EmotionCategories]') AND type in (N'U'))
DROP TABLE [dbo].[tn_EmotionCategories]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Favorites_TenantTypeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Favorites] DROP CONSTRAINT [DF_tn_Favorites_TenantTypeId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Favorites_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Favorites] DROP CONSTRAINT [DF_tn_Favorites_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Favorites_ObjectId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Favorites] DROP CONSTRAINT [DF_tn_Favorites_ObjectId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Favorites]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Favorites]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_FollowedUsers_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Follows] DROP CONSTRAINT [DF_tn_FollowedUsers_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_FollowedUsers_FollowedUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Follows] DROP CONSTRAINT [DF_tn_FollowedUsers_FollowedUserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_FollowedUsers_NoteName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Follows] DROP CONSTRAINT [DF_tn_FollowedUsers_NoteName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_FollowedUsers_IsQuietly]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Follows] DROP CONSTRAINT [DF_tn_FollowedUsers_IsQuietly]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_FollowedUsers_IsNewFollower]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Follows] DROP CONSTRAINT [DF_tn_FollowedUsers_IsNewFollower]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Follows_IsMutual]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Follows] DROP CONSTRAINT [DF_tn_Follows_IsMutual]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Follows]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Follows]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InitialNavigations_ParentNavigationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InitialNavigations] DROP CONSTRAINT [DF_tn_InitialNavigations_ParentNavigationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InitialNavigations_Depth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InitialNavigations] DROP CONSTRAINT [DF_tn_InitialNavigations_Depth]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InitialNavigations_ApplicationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InitialNavigations] DROP CONSTRAINT [DF_tn_InitialNavigations_ApplicationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InitialNavigations_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InitialNavigations] DROP CONSTRAINT [DF_tn_InitialNavigations_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InitialNavigations_OnlyOwnerVisible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InitialNavigations] DROP CONSTRAINT [DF_tn_InitialNavigations_OnlyOwnerVisible]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InitialNavigations_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InitialNavigations] DROP CONSTRAINT [DF_tn_InitialNavigations_IsLocked]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InitialNavigations_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InitialNavigations] DROP CONSTRAINT [DF_tn_InitialNavigations_IsEnabled]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InitialNavigations]') AND type in (N'U'))
DROP TABLE [dbo].[tn_InitialNavigations]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InvitationCodes]') AND type in (N'U'))
DROP TABLE [dbo].[tn_InvitationCodes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InvitationCodeStatistics]') AND type in (N'U'))
DROP TABLE [dbo].[tn_InvitationCodeStatistics]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Invitations_ApplicationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Invitations] DROP CONSTRAINT [DF_tn_Invitations_ApplicationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Invitations_RelativeObjectName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Invitations] DROP CONSTRAINT [DF_tn_Invitations_RelativeObjectName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Invitations_Status]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Invitations] DROP CONSTRAINT [DF_tn_Invitations_Status]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Invitations]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Invitations]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InviteFriendRecords_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InviteFriendRecords] DROP CONSTRAINT [DF_tn_InviteFriendRecords_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_InviteFriendRecords_InvitedUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_InviteFriendRecords] DROP CONSTRAINT [DF_tn_InviteFriendRecords_InvitedUserId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InviteFriendRecords]') AND type in (N'U'))
DROP TABLE [dbo].[tn_InviteFriendRecords]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInCategories]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ItemsInCategories]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInTags]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ItemsInTags]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Messages_SenderUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Messages] DROP CONSTRAINT [DF_tn_Messages_SenderUserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Messages_Sender]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Messages] DROP CONSTRAINT [DF_tn_Messages_Sender]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Messages_ReceiverUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Messages] DROP CONSTRAINT [DF_tn_Messages_ReceiverUserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Messages_Receiver]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Messages] DROP CONSTRAINT [DF_tn_Messages_Receiver]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Messages_Body]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Messages] DROP CONSTRAINT [DF_tn_Messages_Body]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Messages_IsRead]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Messages] DROP CONSTRAINT [DF_tn_Messages_IsRead]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Messages_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Messages] DROP CONSTRAINT [DF_tn_Messages_IP]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Messages]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Messages]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_MessageSessions_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_MessageSessions] DROP CONSTRAINT [DF_tn_MessageSessions_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_MessageSessions_OtherUserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_MessageSessions] DROP CONSTRAINT [DF_tn_MessageSessions_OtherUserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_MessageSessions_LastMessageId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_MessageSessions] DROP CONSTRAINT [DF_tn_MessageSessions_LastMessageId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_MessageSessions_MessageCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_MessageSessions] DROP CONSTRAINT [DF_tn_MessageSessions_MessageCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_MessageSessions_UnreadItemCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_MessageSessions] DROP CONSTRAINT [DF_tn_MessageSessions_UnreadItemCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_MessageSessions_MessageType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_MessageSessions] DROP CONSTRAINT [DF_tn_MessageSessions_MessageType]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_MessageSessions]') AND type in (N'U'))
DROP TABLE [dbo].[tn_MessageSessions]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_MessagesInSessions_MessageId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_MessagesInSessions] DROP CONSTRAINT [DF_tn_MessagesInSessions_MessageId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_MessagesInSessions]') AND type in (N'U'))
DROP TABLE [dbo].[tn_MessagesInSessions]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Notices_ApplicationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Notices] DROP CONSTRAINT [DF_tn_Notices_ApplicationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Notices_RelativeObjectName1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Notices] DROP CONSTRAINT [DF_tn_Notices_RelativeObjectName1]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Notices_RelativeObjectName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Notices] DROP CONSTRAINT [DF_tn_Notices_RelativeObjectName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Notices_Body]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Notices] DROP CONSTRAINT [DF_tn_Notices_Body]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Notices_Status]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Notices] DROP CONSTRAINT [DF_tn_Notices_Status]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Notices]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Notices]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_OnlineUsers_LastAction]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OnlineUsers] DROP CONSTRAINT [DF_tn_OnlineUsers_LastAction]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_OnlineUsers_Ip]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OnlineUsers] DROP CONSTRAINT [DF_tn_OnlineUsers_Ip]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OnlineUsers]') AND type in (N'U'))
DROP TABLE [dbo].[tn_OnlineUsers]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_OnlineUserStatistics_LoggedUserCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OnlineUserStatistics] DROP CONSTRAINT [DF_tn_OnlineUserStatistics_LoggedUserCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_OnlineUserStatistics_AnonymousCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OnlineUserStatistics] DROP CONSTRAINT [DF_tn_OnlineUserStatistics_AnonymousCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_OnlineUserStatistics_UserCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OnlineUserStatistics] DROP CONSTRAINT [DF_tn_OnlineUserStatistics_UserCount]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OnlineUserStatistics]') AND type in (N'U'))
DROP TABLE [dbo].[tn_OnlineUserStatistics]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_OperationLogs_OperationObjectName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OperationLogs] DROP CONSTRAINT [DF_tn_OperationLogs_OperationObjectName]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OperationLogs]') AND type in (N'U'))
DROP TABLE [dbo].[tn_OperationLogs]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_OwnerData_TenantTypeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OwnerData] DROP CONSTRAINT [DF_tn_OwnerData_TenantTypeId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tn_UserDa__LongV__3B2BBE9D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OwnerData] DROP CONSTRAINT [DF__tn_UserDa__LongV__3B2BBE9D]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tn_UserDa__Strin__3C1FE2D6]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_OwnerData] DROP CONSTRAINT [DF__tn_UserDa__Strin__3C1FE2D6]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OwnerData]') AND type in (N'U'))
DROP TABLE [dbo].[tn_OwnerData]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ParsedMedias_Name]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ParsedMedias] DROP CONSTRAINT [DF_tn_ParsedMedias_Name]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ParsedMedias_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ParsedMedias] DROP CONSTRAINT [DF_tn_ParsedMedias_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ParsedMedias_ThumbnailUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ParsedMedias] DROP CONSTRAINT [DF_tn_ParsedMedias_ThumbnailUrl]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ParsedMedias_PlayerUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ParsedMedias] DROP CONSTRAINT [DF_tn_ParsedMedias_PlayerUrl]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ParsedMedias_SourceFileUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ParsedMedias] DROP CONSTRAINT [DF_tn_ParsedMedias_SourceFileUrl]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ParsedMedias]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ParsedMedias]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItems_ItemName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItems] DROP CONSTRAINT [DF_tn_PermissionItems_ItemName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItems_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItems] DROP CONSTRAINT [DF_tn_PermissionItems_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItems_EnableQuota]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItems] DROP CONSTRAINT [DF_tn_PermissionItems_EnableQuota]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItems_EnableScope]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItems] DROP CONSTRAINT [DF_tn_PermissionItems_EnableScope]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PermissionItems]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PermissionItems]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItemsInUserRoles_PermissionType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItemsInUserRoles] DROP CONSTRAINT [DF_tn_PermissionItemsInUserRoles_PermissionType]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItemsInUserRoles_PermissionQuota]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItemsInUserRoles] DROP CONSTRAINT [DF_tn_PermissionItemsInUserRoles_PermissionQuota]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItemsInUserRoles_PermissionScope]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItemsInUserRoles] DROP CONSTRAINT [DF_tn_PermissionItemsInUserRoles_PermissionScope]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PermissionItemsInUserRoles_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PermissionItemsInUserRoles] DROP CONSTRAINT [DF_tn_PermissionItemsInUserRoles_IsLocked]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PermissionItemsInUserRoles]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PermissionItemsInUserRoles]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointCategories_QuotaPerDay]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointCategories] DROP CONSTRAINT [DF_tn_PointCategories_QuotaPerDay]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointCategories_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointCategories] DROP CONSTRAINT [DF_tn_PointCategories_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointCategories_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointCategories] DROP CONSTRAINT [DF_tn_PointCategories_DisplayOrder]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointCategories]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PointCategories]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_ItemName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_ItemName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_ExperiencePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_ExperiencePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_ReputationPoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_ReputationPoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_TradePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_TradePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_TradePoints2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_TradePoints2]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_TradePoints3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_TradePoints3]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_TradePoints4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_TradePoints4]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointItems_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointItems] DROP CONSTRAINT [DF_tn_PointItems_Description]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointItems]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PointItems]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointRecords_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointRecords] DROP CONSTRAINT [DF_tn_PointRecords_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointRecords_ExperiencePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointRecords] DROP CONSTRAINT [DF_tn_PointRecords_ExperiencePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointRecords_ReputationPoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointRecords] DROP CONSTRAINT [DF_tn_PointRecords_ReputationPoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointRecords_TradePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointRecords] DROP CONSTRAINT [DF_tn_PointRecords_TradePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointRecords_TradePoints2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointRecords] DROP CONSTRAINT [DF_tn_PointRecords_TradePoints2]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointRecords_TradePoints3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointRecords] DROP CONSTRAINT [DF_tn_PointRecords_TradePoints3]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointRecords_TradePoints4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointRecords] DROP CONSTRAINT [DF_tn_PointRecords_TradePoints4]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointRecords]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PointRecords]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PointStatistics_Points]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PointStatistics] DROP CONSTRAINT [DF_tn_PointStatistics_Points]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointStatistics]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PointStatistics]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreaNavigations_ParentNavigationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreaNavigations] DROP CONSTRAINT [DF_tn_PresentAreaNavigations_ParentNavigationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreaNavigations_Depth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreaNavigations] DROP CONSTRAINT [DF_tn_PresentAreaNavigations_Depth]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreaNavigations_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreaNavigations] DROP CONSTRAINT [DF_tn_PresentAreaNavigations_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreaNavigations_OnlyOwnerVisible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreaNavigations] DROP CONSTRAINT [DF_tn_PresentAreaNavigations_OnlyOwnerVisible]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreaNavigations_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreaNavigations] DROP CONSTRAINT [DF_tn_PresentAreaNavigations_IsLocked]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreaNavigations_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreaNavigations] DROP CONSTRAINT [DF_tn_PresentAreaNavigations_IsEnabled]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PresentAreaNavigations]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PresentAreaNavigations]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreas_AllowMultipleInstances]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreas] DROP CONSTRAINT [DF_tn_PresentAreas_AllowMultipleInstances]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PresentAreas_EnableThemes]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PresentAreas] DROP CONSTRAINT [DF_tn_PresentAreas_EnableThemes]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PresentAreas]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PresentAreas]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PrivacyItems_ItemName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PrivacyItems] DROP CONSTRAINT [DF_tn_PrivacyItems_ItemName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PrivacyItems_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PrivacyItems] DROP CONSTRAINT [DF_tn_PrivacyItems_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PrivacyItems_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PrivacyItems] DROP CONSTRAINT [DF_tn_PrivacyItems_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_PrivacyItems_PrivacyStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_PrivacyItems] DROP CONSTRAINT [DF_tn_PrivacyItems_PrivacyStatus]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PrivacyItems]') AND type in (N'U'))
DROP TABLE [dbo].[tn_PrivacyItems]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_RatingGrades_ObjectId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RatingGrades] DROP CONSTRAINT [DF_tn_RatingGrades_ObjectId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_RatingGrades_RateNumber]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RatingGrades] DROP CONSTRAINT [DF_tn_RatingGrades_RateNumber]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RatingGrades]') AND type in (N'U'))
DROP TABLE [dbo].[tn_RatingGrades]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ RatingRecords_ObjectId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RatingRecords] DROP CONSTRAINT [DF_tn_ RatingRecords_ObjectId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ RatingRecords_RateNumber]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RatingRecords] DROP CONSTRAINT [DF_tn_ RatingRecords_RateNumber]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ RatingRecords_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RatingRecords] DROP CONSTRAINT [DF_tn_ RatingRecords_UserId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RatingRecords]') AND type in (N'U'))
DROP TABLE [dbo].[tn_RatingRecords]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Ratings_ObjectId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Ratings] DROP CONSTRAINT [DF_tn_Ratings_ObjectId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Ratings_OwnerId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Ratings] DROP CONSTRAINT [DF_tn_Ratings_OwnerId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Ratings_RateCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Ratings] DROP CONSTRAINT [DF_tn_Ratings_RateCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Ratings_RateSum]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Ratings] DROP CONSTRAINT [DF_tn_Ratings_RateSum]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Ratings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Ratings]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_RecommendItems_FeaturedImage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RecommendItems] DROP CONSTRAINT [DF_spb_RecommendItems_FeaturedImage]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_RecommendItems_IsLink]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RecommendItems] DROP CONSTRAINT [DF_tn_RecommendItems_IsLink]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItems]') AND type in (N'U'))
DROP TABLE [dbo].[tn_RecommendItems]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_RecommendItemTypes_HasFeaturedImage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_RecommendItemTypes] DROP CONSTRAINT [DF_tn_RecommendItemTypes_HasFeaturedImage]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItemTypes]') AND type in (N'U'))
DROP TABLE [dbo].[tn_RecommendItemTypes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RelatedTags]') AND type in (N'U'))
DROP TABLE [dbo].[tn_RelatedTags]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ReminderRecords]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ReminderRecords]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_FriendlyRoleName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_FriendlyRoleName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_IsBuiltIn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_IsBuiltIn]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_ConnectToUser]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_ConnectToUser]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_ApplicationId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_ApplicationId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_IsPublic]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_IsPublic]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_IsEnabled]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Roles_ImageName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Roles] DROP CONSTRAINT [DF_tn_Roles_ImageName]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Roles]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Roles]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Schools_Name]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Schools] DROP CONSTRAINT [DF_tn_Schools_Name]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Schools_PinyinName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Schools] DROP CONSTRAINT [DF_tn_Schools_PinyinName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Schools_ShortPinyinName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Schools] DROP CONSTRAINT [DF_tn_Schools_ShortPinyinName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Schools_SchoolType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Schools] DROP CONSTRAINT [DF_tn_Schools_SchoolType]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Schools_AreaCode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Schools] DROP CONSTRAINT [DF_tn_Schools_AreaCode]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Schools_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Schools] DROP CONSTRAINT [DF_tn_Schools_DisplayOrder]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Schools]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Schools]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_SearchedTerms_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_SearchedTerms] DROP CONSTRAINT [DF_tn_SearchedTerms_DisplayOrder]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SearchedTerms]') AND type in (N'U'))
DROP TABLE [dbo].[tn_SearchedTerms]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_SensitiveWords_Word]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_SensitiveWords] DROP CONSTRAINT [DF_tn_SensitiveWords_Word]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_SensitiveWords_Replacement]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_SensitiveWords] DROP CONSTRAINT [DF_tn_SensitiveWords_Replacement]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_SensitiveWords_TypeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_SensitiveWords] DROP CONSTRAINT [DF_tn_SensitiveWords_TypeId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SensitiveWords]') AND type in (N'U'))
DROP TABLE [dbo].[tn_SensitiveWords]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SensitiveWordTypes]') AND type in (N'U'))
DROP TABLE [dbo].[tn_SensitiveWordTypes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Settings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Settings]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ShortUrls_Url]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ShortUrls] DROP CONSTRAINT [DF_tn_ShortUrls_Url]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ShortUrls_OtherShortUrl]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ShortUrls] DROP CONSTRAINT [DF_tn_ShortUrls_OtherShortUrl]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ShortUrls]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ShortUrls]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_StopedUsers]') AND type in (N'U'))
DROP TABLE [dbo].[tn_StopedUsers]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SystemData]') AND type in (N'U'))
DROP TABLE [dbo].[tn_SystemData]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagGroups]') AND type in (N'U'))
DROP TABLE [dbo].[tn_TagGroups]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Tags_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Tags] DROP CONSTRAINT [DF_tn_Tags_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Tags_FeaturedImage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Tags] DROP CONSTRAINT [DF_tn_Tags_FeaturedImage]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Tags_IsFeatured]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Tags] DROP CONSTRAINT [DF_tn_Tags_IsFeatured]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Tags_ItemCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Tags] DROP CONSTRAINT [DF_tn_Tags_ItemCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Tags_OwnerCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Tags] DROP CONSTRAINT [DF_tn_Tags_OwnerCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Tags_AuditingStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Tags] DROP CONSTRAINT [DF_tn_Tags_AuditingStatus]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Tags]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Tags]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_TagsInGroups_TenantTypeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TagsInGroups] DROP CONSTRAINT [DF_tn_TagsInGroups_TenantTypeId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInGroups]') AND type in (N'U'))
DROP TABLE [dbo].[tn_TagsInGroups]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_TagsInOwners_ItemCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TagsInOwners] DROP CONSTRAINT [DF_tn_TagsInOwners_ItemCount]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInOwners]') AND type in (N'U'))
DROP TABLE [dbo].[tn_TagsInOwners]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_TaskDetails_Name]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TaskDetails] DROP CONSTRAINT [DF_tn_TaskDetails_Name]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_TaskDetails_Rule]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TaskDetails] DROP CONSTRAINT [DF_tn_TaskDetails_Rule]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_TaskDetails_Enabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TaskDetails] DROP CONSTRAINT [DF_tn_TaskDetails_Enabled]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_TaskDetails_IsContinue]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TaskDetails] DROP CONSTRAINT [DF_tn_TaskDetails_IsContinue]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_TaskDetails_IsRunning]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TaskDetails] DROP CONSTRAINT [DF_tn_TaskDetails_IsRunning]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TaskDetails]') AND type in (N'U'))
DROP TABLE [dbo].[tn_TaskDetails]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__tn_Tenant__Class__77FFC2B3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_TenantTypes] DROP CONSTRAINT [DF__tn_Tenant__Class__77FFC2B3]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TenantTypes]') AND type in (N'U'))
DROP TABLE [dbo].[tn_TenantTypes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TenantTypesInServices]') AND type in (N'U'))
DROP TABLE [dbo].[tn_TenantTypesInServices]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_PreviewLargeImage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_PreviewLargeImage]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_LogoFileName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_LogoFileName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_Tags]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_Tags]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_Author]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_Author]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_Copyright]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_Copyright]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_Version]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_Version]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_ForProductVersion]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_ForProductVersion]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_IsEnabled]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_DisplayOrder]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_UserCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_UserCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_Roles]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_Roles]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_ThemeAppearances_RequiredRank]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_ThemeAppearances] DROP CONSTRAINT [DF_tn_ThemeAppearances_RequiredRank]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ThemeAppearances]') AND type in (N'U'))
DROP TABLE [dbo].[tn_ThemeAppearances]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Themes_Parent]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Themes] DROP CONSTRAINT [DF_tn_Themes_Parent]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Themes]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Themes]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserBlockedObjects]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UserBlockedObjects]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserInvitationSettings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UserInvitationSettings]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserNoticeSettings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UserNoticeSettings]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_UserPrivacySettings_PrivacyStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_UserPrivacySettings] DROP CONSTRAINT [DF_tn_UserPrivacySettings_PrivacyStatus]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserPrivacySettings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UserPrivacySettings]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserPrivacySpecifyObjects]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UserPrivacySpecifyObjects]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserRanks]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UserRanks]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_UserReminderSettings_ReminderMode]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_UserReminderSettings] DROP CONSTRAINT [DF_tn_UserReminderSettings_ReminderMode]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_UserReminderSettings_ReminderInfoType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_UserReminderSettings] DROP CONSTRAINT [DF_tn_UserReminderSettings_ReminderInfoType]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserReminderSettings]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UserReminderSettings]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_PasswordFormat]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_PasswordFormat]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_PasswordQuestion]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_PasswordQuestion]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_PasswordAnswer]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_PasswordAnswer]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_AccountEmail]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_AccountEmail]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IsEmailVerified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IsEmailVerified]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_AccountMobile]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_AccountMobile]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IsMobileVerified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IsMobileVerified]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_TrueName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_TrueName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_NickName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_NickName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_ForceLogin]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_ForceLogin]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IsActivated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IsActivated]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IpCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IpCreated]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_UserType]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_UserType]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_LastAction]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_LastAction]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IpLastActivity]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IpLastActivity]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IsBanned]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IsBanned]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IsModerated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IsModerated]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IsForceModerated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IsForceModerated]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_DatabaseQuota]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_DatabaseQuota]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_DatabaseQuotaUsed]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_DatabaseQuotaUsed]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_IsUseCustomStyle]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_IsUseCustomStyle]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_Avatar]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_Avatar]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_FollowedCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_FollowedCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_FollowerCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_FollowerCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_ExperiencePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_ExperiencePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_ReputationPoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_ReputationPoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_TradePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_TradePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_TradePoints2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_TradePoints2]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_TradePoints3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_TradePoints3]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_TradePoints4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_TradePoints4]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_FrozenTradePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_FrozenTradePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_Users_Rank]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Users] DROP CONSTRAINT [DF_tn_Users_Rank]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Users]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UsersInRoles]') AND type in (N'U'))
DROP TABLE [dbo].[tn_UsersInRoles]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_VisitRecords_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Visit] DROP CONSTRAINT [DF_tn_VisitRecords_Id]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_VisitRecords_TenantTypeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Visit] DROP CONSTRAINT [DF_tn_VisitRecords_TenantTypeId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_VisitRecords_VisitorId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Visit] DROP CONSTRAINT [DF_tn_VisitRecords_VisitorId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_VisitRecords_Visitor]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Visit] DROP CONSTRAINT [DF_tn_VisitRecords_Visitor]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_VisitRecords_ToObjectId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Visit] DROP CONSTRAINT [DF_tn_VisitRecords_ToObjectId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_VisitRecords_ToObjectName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tn_Visit] DROP CONSTRAINT [DF_tn_VisitRecords_ToObjectName]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Visit]') AND type in (N'U'))
DROP TABLE [dbo].[tn_Visit]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Visit]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Visit](
	[Id] [bigint] NOT NULL CONSTRAINT [DF_tn_VisitRecords_Id]  DEFAULT ((0)),
	[TenantTypeId] [char](6) NOT NULL CONSTRAINT [DF_tn_VisitRecords_TenantTypeId]  DEFAULT (''),
	[VisitorId] [bigint] NOT NULL CONSTRAINT [DF_tn_VisitRecords_VisitorId]  DEFAULT ((0)),
	[Visitor] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_VisitRecords_Visitor]  DEFAULT (''),
	[ToObjectId] [bigint] NOT NULL CONSTRAINT [DF_tn_VisitRecords_ToObjectId]  DEFAULT ((0)),
	[ToObjectName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_VisitRecords_ToObjectName]  DEFAULT (''),
	[LastVisitTime] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_Visit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Visit]') AND name = N'IX_tn_Visit_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Visit_TenantTypeId] ON [dbo].[tn_Visit] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Visit]') AND name = N'IX_tn_Visit_ToObjectId')
CREATE NONCLUSTERED INDEX [IX_tn_Visit_ToObjectId] ON [dbo].[tn_Visit] 
(
	[ToObjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Visit]') AND name = N'IX_tn_Visit_VisitorId')
CREATE NONCLUSTERED INDEX [IX_tn_Visit_VisitorId] ON [dbo].[tn_Visit] 
(
	[VisitorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Visit', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Visit', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Visit', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Visit', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Visit', N'COLUMN',N'VisitorId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'访客用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Visit', @level2type=N'COLUMN',@level2name=N'VisitorId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Visit', N'COLUMN',N'Visitor'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'访客名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Visit', @level2type=N'COLUMN',@level2name=N'Visitor'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Visit', N'COLUMN',N'ToObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被访问对象id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Visit', @level2type=N'COLUMN',@level2name=N'ToObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Visit', N'COLUMN',N'ToObjectName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被访问对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Visit', @level2type=N'COLUMN',@level2name=N'ToObjectName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Visit', N'COLUMN',N'LastVisitTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后访问时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Visit', @level2type=N'COLUMN',@level2name=N'LastVisitTime'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UsersInRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UsersInRoles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RoleName] [varchar](32) NOT NULL,
 CONSTRAINT [PK_tn_UsersInRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UsersInRoles]') AND name = N'IX_tn_UsersInRoles_RoleName')
CREATE NONCLUSTERED INDEX [IX_tn_UsersInRoles_RoleName] ON [dbo].[tn_UsersInRoles] 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UsersInRoles]') AND name = N'IX_tn_UsersInRoles_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_UsersInRoles_UserId] ON [dbo].[tn_UsersInRoles] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Users](
	[UserId] [bigint] NOT NULL,
	[UserName] [nvarchar](64) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL CONSTRAINT [DF_tn_Users_PasswordFormat]  DEFAULT ((1)),
	[PasswordQuestion] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_PasswordQuestion]  DEFAULT (''),
	[PasswordAnswer] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_PasswordAnswer]  DEFAULT (''),
	[AccountEmail] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_AccountEmail]  DEFAULT (''),
	[IsEmailVerified] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_IsEmailVerified]  DEFAULT ((0)),
	[AccountMobile] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_AccountMobile]  DEFAULT (''),
	[IsMobileVerified] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_IsMobileVerified]  DEFAULT ((0)),
	[TrueName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_TrueName]  DEFAULT (''),
	[NickName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_NickName]  DEFAULT (''),
	[ForceLogin] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_ForceLogin]  DEFAULT ((0)),
	[IsActivated] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_IsActivated]  DEFAULT ((1)),
	[DateCreated] [datetime] NOT NULL,
	[IpCreated] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_IpCreated]  DEFAULT (''),
	[UserType] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_UserType]  DEFAULT ((1)),
	[LastActivityTime] [datetime] NOT NULL,
	[LastAction] [nvarchar](512) NOT NULL CONSTRAINT [DF_tn_Users_LastAction]  DEFAULT (''),
	[IpLastActivity] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Users_IpLastActivity]  DEFAULT (''),
	[IsBanned] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_IsBanned]  DEFAULT ((0)),
	[BanReason] [nvarchar](64) NOT NULL,
	[BanDeadline] [datetime] NOT NULL,
	[IsModerated] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_IsModerated]  DEFAULT ((0)),
	[IsForceModerated] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_IsForceModerated]  DEFAULT ((0)),
	[DatabaseQuota] [int] NOT NULL CONSTRAINT [DF_tn_Users_DatabaseQuota]  DEFAULT ((0)),
	[DatabaseQuotaUsed] [int] NOT NULL CONSTRAINT [DF_tn_Users_DatabaseQuotaUsed]  DEFAULT ((0)),
	[ThemeAppearance] [nvarchar](128) NOT NULL,
	[IsUseCustomStyle] [tinyint] NOT NULL CONSTRAINT [DF_tn_Users_IsUseCustomStyle]  DEFAULT ((0)),
	[Avatar] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_Users_Avatar]  DEFAULT (''),
	[FollowedCount] [int] NOT NULL CONSTRAINT [DF_tn_Users_FollowedCount]  DEFAULT ((0)),
	[FollowerCount] [int] NOT NULL CONSTRAINT [DF_tn_Users_FollowerCount]  DEFAULT ((0)),
	[ExperiencePoints] [int] NOT NULL CONSTRAINT [DF_tn_Users_ExperiencePoints]  DEFAULT ((0)),
	[ReputationPoints] [int] NOT NULL CONSTRAINT [DF_tn_Users_ReputationPoints]  DEFAULT ((0)),
	[TradePoints] [int] NOT NULL CONSTRAINT [DF_tn_Users_TradePoints]  DEFAULT ((0)),
	[TradePoints2] [int] NOT NULL CONSTRAINT [DF_tn_Users_TradePoints2]  DEFAULT ((0)),
	[TradePoints3] [int] NOT NULL CONSTRAINT [DF_tn_Users_TradePoints3]  DEFAULT ((0)),
	[TradePoints4] [int] NOT NULL CONSTRAINT [DF_tn_Users_TradePoints4]  DEFAULT ((0)),
	[FrozenTradePoints] [int] NOT NULL CONSTRAINT [DF_tn_Users_FrozenTradePoints]  DEFAULT ((0)),
	[Rank] [int] NOT NULL CONSTRAINT [DF_tn_Users_Rank]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND name = N'IX_tn_Users_AccountEmail')
CREATE NONCLUSTERED INDEX [IX_tn_Users_AccountEmail] ON [dbo].[tn_Users] 
(
	[AccountEmail] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND name = N'IX_tn_Users_AccountMobile')
CREATE NONCLUSTERED INDEX [IX_tn_Users_AccountMobile] ON [dbo].[tn_Users] 
(
	[AccountMobile] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND name = N'IX_tn_Users_FollowedCount')
CREATE NONCLUSTERED INDEX [IX_tn_Users_FollowedCount] ON [dbo].[tn_Users] 
(
	[FollowedCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND name = N'IX_tn_Users_FollowerCount')
CREATE NONCLUSTERED INDEX [IX_tn_Users_FollowerCount] ON [dbo].[tn_Users] 
(
	[FollowerCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND name = N'IX_tn_Users_Rank')
CREATE NONCLUSTERED INDEX [IX_tn_Users_Rank] ON [dbo].[tn_Users] 
(
	[Rank] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Users]') AND name = N'IX_tn_Users_UserName')
CREATE UNIQUE NONCLUSTERED INDEX [IX_tn_Users_UserName] ON [dbo].[tn_Users] 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'UserName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'UserName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'Password'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'Password'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'PasswordFormat'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=Clear（明文）1=标准MD5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'PasswordFormat'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'PasswordQuestion'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码问题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'PasswordQuestion'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'PasswordAnswer'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码答案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'PasswordAnswer'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'AccountEmail'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帐号邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'AccountEmail'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IsEmailVerified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帐号邮箱是否通过验证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IsEmailVerified'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'AccountMobile'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'AccountMobile'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IsMobileVerified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帐号手机是否通过验证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IsMobileVerified'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'TrueName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'个人姓名 或 企业名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'TrueName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'NickName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昵称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'NickName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'ForceLogin'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否强制用户登录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'ForceLogin'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IsActivated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户是否激活' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IsActivated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IpCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户时的ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IpCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'UserType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'UserType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'LastActivityTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次活动时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'LastActivityTime'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'LastAction'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次操作' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'LastAction'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IpLastActivity'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次活动时的ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IpLastActivity'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IsBanned'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否封禁' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IsBanned'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'BanReason'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'封禁原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'BanReason'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'BanDeadline'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'封禁截止日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'BanDeadline'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IsModerated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户是否被监管' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IsModerated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IsForceModerated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'强制用户管制（不会自动解除）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IsForceModerated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'DatabaseQuota'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'磁盘配额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'DatabaseQuota'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'DatabaseQuotaUsed'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'以用磁盘空间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'DatabaseQuotaUsed'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'ThemeAppearance'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户选择的皮肤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'ThemeAppearance'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'IsUseCustomStyle'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否使用了自定义风格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'IsUseCustomStyle'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'Avatar'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'Avatar'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'FollowedCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关注用户数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'FollowedCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'FollowerCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'粉丝数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'FollowerCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'ExperiencePoints'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'经验积分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'ExperiencePoints'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'ReputationPoints'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'威望积分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'ReputationPoints'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'TradePoints'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易积分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'TradePoints'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'TradePoints2'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易积分值2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'TradePoints2'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'TradePoints3'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易积分值3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'TradePoints3'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'TradePoints4'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易积分值4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'TradePoints4'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'FrozenTradePoints'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'冻结的交易积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'FrozenTradePoints'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Users', N'COLUMN',N'Rank'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Users', @level2type=N'COLUMN',@level2name=N'Rank'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserReminderSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UserReminderSettings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ReminderModeId] [int] NOT NULL CONSTRAINT [DF_tn_UserReminderSettings_ReminderMode]  DEFAULT ((1)),
	[ReminderInfoTypeId] [int] NOT NULL CONSTRAINT [DF_tn_UserReminderSettings_ReminderInfoType]  DEFAULT ((1)),
	[ReminderThreshold] [int] NOT NULL,
	[IsEnabled] [tinyint] NOT NULL,
	[IsRepeated] [tinyint] NOT NULL,
	[RepeatInterval] [int] NOT NULL,
 CONSTRAINT [PK_tn_UserReminderSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserReminderSettings]') AND name = N'IX_tn_UserReminderSettings_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_UserReminderSettings_UserId] ON [dbo].[tn_UserReminderSettings] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserReminderSettings', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserReminderSettings', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserReminderSettings', N'COLUMN',N'ReminderModeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提醒方式(Email=1，手机=2)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserReminderSettings', @level2type=N'COLUMN',@level2name=N'ReminderModeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserReminderSettings', N'COLUMN',N'ReminderInfoTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提醒信息类型（私信=1，通知=2，请求=3）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserReminderSettings', @level2type=N'COLUMN',@level2name=N'ReminderInfoTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserReminderSettings', N'COLUMN',N'ReminderThreshold'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送提醒的时间阀值（单位为分钟），超过此值，发现有未处理的信息将发送提醒' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserReminderSettings', @level2type=N'COLUMN',@level2name=N'ReminderThreshold'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserReminderSettings', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用提醒' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserReminderSettings', @level2type=N'COLUMN',@level2name=N'IsEnabled'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserReminderSettings', N'COLUMN',N'IsRepeated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否重复提醒' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserReminderSettings', @level2type=N'COLUMN',@level2name=N'IsRepeated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserReminderSettings', N'COLUMN',N'RepeatInterval'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重复提醒间隔时间，多长时间（单位：分钟）发送一次提醒' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserReminderSettings', @level2type=N'COLUMN',@level2name=N'RepeatInterval'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserRanks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UserRanks](
	[Rank] [int] NOT NULL,
	[PointLower] [int] NOT NULL,
	[RankName] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_tn_UserRanks] PRIMARY KEY CLUSTERED 
(
	[Rank] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserRanks', N'COLUMN',N'Rank'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'级别（从1开始）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserRanks', @level2type=N'COLUMN',@level2name=N'Rank'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserRanks', N'COLUMN',N'PointLower'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'积分下限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserRanks', @level2type=N'COLUMN',@level2name=N'PointLower'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserRanks', N'COLUMN',N'RankName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserRanks', @level2type=N'COLUMN',@level2name=N'RankName'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserPrivacySpecifyObjects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UserPrivacySpecifyObjects](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserPrivacySettingId] [bigint] NOT NULL,
	[SpecifyObjectTypeId] [int] NOT NULL,
	[SpecifyObjectId] [bigint] NOT NULL,
	[SpecifyObjectName] [nvarchar](64) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_UserPrivacySettingId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserPrivacySpecifyObjects]') AND name = N'IX_SpecifyObjectType')
CREATE NONCLUSTERED INDEX [IX_SpecifyObjectType] ON [dbo].[tn_UserPrivacySpecifyObjects] 
(
	[SpecifyObjectTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserPrivacySpecifyObjects]') AND name = N'IX_tn_UserPrivacySpecifyObjects')
CREATE NONCLUSTERED INDEX [IX_tn_UserPrivacySpecifyObjects] ON [dbo].[tn_UserPrivacySpecifyObjects] 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySpecifyObjects', N'COLUMN',N'UserPrivacySettingId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户隐私设置Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'UserPrivacySettingId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySpecifyObjects', N'COLUMN',N'SpecifyObjectTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被指定对象类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'SpecifyObjectTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySpecifyObjects', N'COLUMN',N'SpecifyObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被指定对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'SpecifyObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySpecifyObjects', N'COLUMN',N'SpecifyObjectName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被指定对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'SpecifyObjectName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySpecifyObjects', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserPrivacySettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UserPrivacySettings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ItemKey] [varchar](32) NOT NULL,
	[PrivacyStatus] [smallint] NOT NULL CONSTRAINT [DF_tn_UserPrivacySettings_PrivacyStatus]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_UserPrivacySettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserPrivacySettings]') AND name = N'IX_UserId')
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[tn_UserPrivacySettings] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySettings', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySettings', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySettings', N'COLUMN',N'ItemKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySettings', @level2type=N'COLUMN',@level2name=N'ItemKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserPrivacySettings', N'COLUMN',N'PrivacyStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserPrivacySettings', @level2type=N'COLUMN',@level2name=N'PrivacyStatus'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserNoticeSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UserNoticeSettings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[TypeId] [int] NOT NULL,
	[IsAllowable] [tinyint] NOT NULL,
 CONSTRAINT [PK_tn_UserNoticeSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserNoticeSettings]') AND name = N'IX_tn_UserNoticeSettings_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_UserNoticeSettings_UserId] ON [dbo].[tn_UserNoticeSettings] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserNoticeSettings', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserNoticeSettings', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserNoticeSettings', N'COLUMN',N'TypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserNoticeSettings', @level2type=N'COLUMN',@level2name=N'TypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserNoticeSettings', N'COLUMN',N'IsAllowable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否允许接受' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserNoticeSettings', @level2type=N'COLUMN',@level2name=N'IsAllowable'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserInvitationSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UserInvitationSettings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[InvitationTypeKey] [nvarchar](64) NOT NULL,
	[IsAllowable] [tinyint] NOT NULL,
 CONSTRAINT [PK_tn_UserInvitationSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserInvitationSettings]') AND name = N'IX_tn_UserInvitationSettings_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_UserInvitationSettings_UserId] ON [dbo].[tn_UserInvitationSettings] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserInvitationSettings', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserInvitationSettings', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserInvitationSettings', N'COLUMN',N'InvitationTypeKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求类型KEY' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserInvitationSettings', @level2type=N'COLUMN',@level2name=N'InvitationTypeKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserInvitationSettings', N'COLUMN',N'IsAllowable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否允许接受' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserInvitationSettings', @level2type=N'COLUMN',@level2name=N'IsAllowable'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserBlockedObjects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_UserBlockedObjects](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ObjectType] [smallint] NOT NULL,
	[ObjectId] [bigint] NOT NULL,
	[ObjectName] [nvarchar](64) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_UserBlockedObjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_UserBlockedObjects]') AND name = N'Index_UserId')
CREATE NONCLUSTERED INDEX [Index_UserId] ON [dbo].[tn_UserBlockedObjects] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserBlockedObjects', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserBlockedObjects', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserBlockedObjects', N'COLUMN',N'ObjectType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被屏蔽对象类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserBlockedObjects', @level2type=N'COLUMN',@level2name=N'ObjectType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserBlockedObjects', N'COLUMN',N'ObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被屏蔽对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserBlockedObjects', @level2type=N'COLUMN',@level2name=N'ObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserBlockedObjects', N'COLUMN',N'ObjectName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被屏蔽对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserBlockedObjects', @level2type=N'COLUMN',@level2name=N'ObjectName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_UserBlockedObjects', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_UserBlockedObjects', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Themes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Themes](
	[Id] [varchar](128) NOT NULL,
	[PresentAreaKey] [varchar](32) NOT NULL,
	[ThemeKey] [varchar](32) NOT NULL,
	[Parent] [varchar](32) NOT NULL CONSTRAINT [DF_tn_Themes_Parent]  DEFAULT (''),
	[Version] [varchar](10) NOT NULL,
 CONSTRAINT [PK_tn_Themes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Themes', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'格式：PresentAreaKey,ThemeKey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Themes', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Themes', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Themes', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Themes', N'COLUMN',N'ThemeKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Theme标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Themes', @level2type=N'COLUMN',@level2name=N'ThemeKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Themes', N'COLUMN',N'Parent'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父主题ThemeKey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Themes', @level2type=N'COLUMN',@level2name=N'Parent'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Themes', N'COLUMN',N'Version'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Themes', @level2type=N'COLUMN',@level2name=N'Version'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ThemeAppearances]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ThemeAppearances](
	[Id] [varchar](128) NOT NULL,
	[PresentAreaKey] [varchar](32) NOT NULL,
	[ThemeKey] [varchar](32) NOT NULL,
	[AppearanceKey] [varchar](32) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[PreviewImage] [nvarchar](255) NOT NULL,
	[PreviewLargeImage] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_PreviewLargeImage]  DEFAULT (''),
	[LogoFileName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_LogoFileName]  DEFAULT (''),
	[Description] [nvarchar](1024) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_Description]  DEFAULT (''),
	[Tags] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_Tags]  DEFAULT (''),
	[Author] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_Author]  DEFAULT (''),
	[Copyright] [nvarchar](512) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_Copyright]  DEFAULT (''),
	[LastModified] [datetime] NOT NULL,
	[Version] [varchar](10) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_Version]  DEFAULT (''),
	[ForProductVersion] [nvarchar](10) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_ForProductVersion]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_IsEnabled]  DEFAULT ((1)),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_DisplayOrder]  DEFAULT ((0)),
	[UserCount] [int] NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_UserCount]  DEFAULT ((0)),
	[Roles] [varchar](255) NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_Roles]  DEFAULT (''),
	[RequiredRank] [int] NOT NULL CONSTRAINT [DF_tn_ThemeAppearances_RequiredRank]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_ThemeAppearances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'格式：PresentAreaKey,ThemeKey,AppearanceKey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Appearance名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'PreviewImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'皮肤预览图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'PreviewImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'PreviewLargeImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'皮肤大预览图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'PreviewLargeImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'LogoFileName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重置的网站Logo图片名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'LogoFileName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'皮肤描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Tags'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签（多个标签用逗号分隔）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Tags'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Author'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'皮肤作者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Author'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Copyright'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版权声明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Copyright'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'皮肤最后更新日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'LastModified'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Version'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Version'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'ForProductVersion'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'适用产品版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'ForProductVersion'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'安装日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用(bool)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'IsEnabled'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排列顺序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'UserCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用者数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'UserCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'Roles'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允许使用的角色名称 多个角色用’,’分隔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'Roles'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ThemeAppearances', N'COLUMN',N'RequiredRank'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允许的最小等级(用户等级或群组等级)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ThemeAppearances', @level2type=N'COLUMN',@level2name=N'RequiredRank'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TenantTypesInServices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_TenantTypesInServices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[ServiceKey] [varchar](32) NULL,
 CONSTRAINT [PK_tn_TenantTypesInServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TenantTypesInServices]') AND name = N'tn_TenantTypesInServices_ServiceKey')
CREATE NONCLUSTERED INDEX [tn_TenantTypesInServices_ServiceKey] ON [dbo].[tn_TenantTypesInServices] 
(
	[ServiceKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TenantTypesInServices]') AND name = N'tn_TenantTypesInServices_TenantTypeId')
CREATE NONCLUSTERED INDEX [tn_TenantTypesInServices_TenantTypeId] ON [dbo].[tn_TenantTypesInServices] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TenantTypesInServices', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TenantTypesInServices', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TenantTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_TenantTypes](
	[TenantTypeId] [char](6) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[ClassType] [varchar](255) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_tn_TenantTypes] PRIMARY KEY CLUSTERED 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TenantTypes]') AND name = N'tn_TenantTypes')
CREATE NONCLUSTERED INDEX [tn_TenantTypes] ON [dbo].[tn_TenantTypes] 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TenantTypes', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TenantTypes', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TenantTypes', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TenantTypes', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TenantTypes', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TenantTypes', @level2type=N'COLUMN',@level2name=N'Name'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TaskDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_TaskDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](64) NOT NULL CONSTRAINT [DF_tn_TaskDetails_Name]  DEFAULT (''),
	[TaskRule] [varchar](64) NOT NULL CONSTRAINT [DF_tn_TaskDetails_Rule]  DEFAULT (''),
	[ClassType] [varchar](255) NOT NULL,
	[Enabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_TaskDetails_Enabled]  DEFAULT ((1)),
	[RunAtRestart] [tinyint] NOT NULL CONSTRAINT [DF_tn_TaskDetails_IsContinue]  DEFAULT ((1)),
	[IsRunning] [tinyint] NOT NULL CONSTRAINT [DF_tn_TaskDetails_IsRunning]  DEFAULT ((0)),
	[LastStart] [datetime] NULL,
	[LastEnd] [datetime] NULL,
	[LastIsSuccess] [tinyint] NULL,
	[NextStart] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[RunAtServer] [tinyint] NULL,
 CONSTRAINT [PK_tn_TaskDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInOwners]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_TagsInOwners](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[TagName] [nvarchar](128) NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[ItemCount] [int] NOT NULL CONSTRAINT [DF_tn_TagsInOwners_ItemCount]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_TagsInOwners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInOwners]') AND name = N'IX_tn_TagsInOwners_OwnerId')
CREATE NONCLUSTERED INDEX [IX_tn_TagsInOwners_OwnerId] ON [dbo].[tn_TagsInOwners] 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInOwners]') AND name = N'IX_tn_TagsInOwners_TagName')
CREATE NONCLUSTERED INDEX [IX_tn_TagsInOwners_TagName] ON [dbo].[tn_TagsInOwners] 
(
	[TagName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInOwners', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInOwners', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInOwners', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInOwners', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInOwners', N'COLUMN',N'TagName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInOwners', @level2type=N'COLUMN',@level2name=N'TagName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInOwners', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInOwners', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInOwners', N'COLUMN',N'ItemCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容项数目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInOwners', @level2type=N'COLUMN',@level2name=N'ItemCount'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInGroups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_TagsInGroups](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[TagName] [nvarchar](64) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL CONSTRAINT [DF_tn_TagsInGroups_TenantTypeId]  DEFAULT (''),
 CONSTRAINT [PK_tn_TagsInGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInGroups]') AND name = N'IX_tn_TagsInGroups_GroupId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_TagsInGroups_GroupId_TenantTypeId] ON [dbo].[tn_TagsInGroups] 
(
	[GroupId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagsInGroups]') AND name = N'IX_tn_TagsInGroups_TagName_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_TagsInGroups_TagName_TenantTypeId] ON [dbo].[tn_TagsInGroups] 
(
	[TagName] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInGroups', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInGroups', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInGroups', N'COLUMN',N'GroupId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInGroups', @level2type=N'COLUMN',@level2name=N'GroupId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagsInGroups', N'COLUMN',N'TagName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分组名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagsInGroups', @level2type=N'COLUMN',@level2name=N'TagName'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Tags](
	[TagId] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[TagName] [nvarchar](64) NOT NULL,
	[DisplayName] [nvarchar](64) NULL,
	[Description] [nvarchar](512) NOT NULL CONSTRAINT [DF_tn_Tags_Description]  DEFAULT (''),
	[FeaturedImage] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_Tags_FeaturedImage]  DEFAULT (''),
	[IsFeatured] [tinyint] NOT NULL CONSTRAINT [DF_tn_Tags_IsFeatured]  DEFAULT ((0)),
	[ItemCount] [int] NOT NULL CONSTRAINT [DF_tn_Tags_ItemCount]  DEFAULT ((0)),
	[OwnerCount] [int] NOT NULL CONSTRAINT [DF_tn_Tags_OwnerCount]  DEFAULT ((0)),
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_tn_Tags_AuditingStatus]  DEFAULT ((40)),
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_tn_Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Tags]') AND name = N'IX_tn_Tags_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_tn_Tags_AuditStatus] ON [dbo].[tn_Tags] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Tags]') AND name = N'IX_tn_Tags_ItemCount')
CREATE NONCLUSTERED INDEX [IX_tn_Tags_ItemCount] ON [dbo].[tn_Tags] 
(
	[ItemCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Tags]') AND name = N'IX_tn_Tags_OwnerCount')
CREATE NONCLUSTERED INDEX [IX_tn_Tags_OwnerCount] ON [dbo].[tn_Tags] 
(
	[OwnerCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Tags]') AND name = N'IX_tn_Tags_TagName')
CREATE NONCLUSTERED INDEX [IX_tn_Tags_TagName] ON [dbo].[tn_Tags] 
(
	[TagName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Tags]') AND name = N'IX_tn_Tags_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Tags_TenantTypeId] ON [dbo].[tn_Tags] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'TagId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'TagId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'TagName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'TagName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'DisplayName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签显示名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'DisplayName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'FeaturedImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签标题图（存储图片文件名）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'FeaturedImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'IsFeatured'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为特色标签' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'IsFeatured'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'ItemCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容项数目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'ItemCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'OwnerCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用者数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'OwnerCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Tags', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Tags', @level2type=N'COLUMN',@level2name=N'PropertyValues'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagGroups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_TagGroups](
	[GroupId] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[GroupName] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_tn_TagGroups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagGroups]') AND name = N'IX_tn_TagGroups_GroupName')
CREATE NONCLUSTERED INDEX [IX_tn_TagGroups_GroupName] ON [dbo].[tn_TagGroups] 
(
	[GroupName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_TagGroups]') AND name = N'IX_tn_TagGroups_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_TagGroups_TenantTypeId] ON [dbo].[tn_TagGroups] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagGroups', N'COLUMN',N'GroupId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagGroups', @level2type=N'COLUMN',@level2name=N'GroupId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagGroups', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagGroups', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_TagGroups', N'COLUMN',N'GroupName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分组名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_TagGroups', @level2type=N'COLUMN',@level2name=N'GroupName'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SystemData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_SystemData](
	[Datakey] [varchar](32) NOT NULL,
	[LongValue] [bigint] NOT NULL,
	[DecimalValue] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_tn_SystemData] PRIMARY KEY CLUSTERED 
(
	[Datakey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_StopedUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_StopedUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ToUserId] [bigint] NOT NULL,
	[ToUserDisplayName] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_tn_StopedUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_StopedUsers]') AND name = N'IX_UserId')
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[tn_StopedUsers] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_StopedUsers', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_StopedUsers', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_StopedUsers', N'COLUMN',N'ToUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被阻止用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_StopedUsers', @level2type=N'COLUMN',@level2name=N'ToUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_StopedUsers', N'COLUMN',N'ToUserDisplayName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被阻止用户名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_StopedUsers', @level2type=N'COLUMN',@level2name=N'ToUserDisplayName'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ShortUrls]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ShortUrls](
	[Alias] [varchar](6) NOT NULL,
	[Url] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ShortUrls_Url]  DEFAULT (''),
	[OtherShortUrl] [varchar](32) NOT NULL CONSTRAINT [DF_tn_ShortUrls_OtherShortUrl]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_ShortUrls] PRIMARY KEY CLUSTERED 
(
	[Alias] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ShortUrls', N'COLUMN',N'Alias'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url别名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ShortUrls', @level2type=N'COLUMN',@level2name=N'Alias'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ShortUrls', N'COLUMN',N'Url'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ShortUrls', @level2type=N'COLUMN',@level2name=N'Url'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ShortUrls', N'COLUMN',N'OtherShortUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方短网址服务提供的短网址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ShortUrls', @level2type=N'COLUMN',@level2name=N'OtherShortUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ShortUrls', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ShortUrls', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Settings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Settings](
	[ClassType] [varchar](128) NOT NULL,
	[Settings] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tn_Settings] PRIMARY KEY CLUSTERED 
(
	[ClassType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Settings', N'COLUMN',N'ClassType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应配置实体类的Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Settings', @level2type=N'COLUMN',@level2name=N'ClassType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Settings', N'COLUMN',N'Settings'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置的xml' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Settings', @level2type=N'COLUMN',@level2name=N'Settings'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SensitiveWordTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_SensitiveWordTypes](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_tn_SensitiveWordTypes] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SensitiveWordTypes', N'COLUMN',N'TypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TypeId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SensitiveWordTypes', @level2type=N'COLUMN',@level2name=N'TypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SensitiveWordTypes', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'敏感词类型名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SensitiveWordTypes', @level2type=N'COLUMN',@level2name=N'Name'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SensitiveWords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_SensitiveWords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Word] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_SensitiveWords_Word]  DEFAULT (''),
	[Replacement] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_SensitiveWords_Replacement]  DEFAULT (''),
	[TypeId] [int] NOT NULL CONSTRAINT [DF_tn_SensitiveWords_TypeId]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_SensitiveWords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SensitiveWords', N'COLUMN',N'Word'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'需要替换的敏感词' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SensitiveWords', @level2type=N'COLUMN',@level2name=N'Word'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SensitiveWords', N'COLUMN',N'Replacement'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'占位字符' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SensitiveWords', @level2type=N'COLUMN',@level2name=N'Replacement'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SensitiveWords', N'COLUMN',N'TypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'敏感词类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SensitiveWords', @level2type=N'COLUMN',@level2name=N'TypeId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SearchedTerms]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_SearchedTerms](
	[Id] [bigint] NOT NULL,
	[Term] [nvarchar](64) NOT NULL,
	[SearchTypeCode] [varchar](32) NOT NULL,
	[IsAddedByAdministrator] [tinyint] NOT NULL,
	[DisplayOrder] [bigint] NOT NULL CONSTRAINT [DF_tn_SearchedTerms_DisplayOrder]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_SearchedTerms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SearchedTerms', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id（使用Id生成器自动生成）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SearchedTerms', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SearchedTerms', N'COLUMN',N'Term'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'搜索词' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SearchedTerms', @level2type=N'COLUMN',@level2name=N'Term'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SearchedTerms', N'COLUMN',N'SearchTypeCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'搜索类型编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SearchedTerms', @level2type=N'COLUMN',@level2name=N'SearchTypeCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SearchedTerms', N'COLUMN',N'IsAddedByAdministrator'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否由管理员添加（人工干预）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SearchedTerms', @level2type=N'COLUMN',@level2name=N'IsAddedByAdministrator'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SearchedTerms', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序字段（默认与Id相同）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SearchedTerms', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SearchedTerms', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SearchedTerms', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_SearchedTerms', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后使用日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_SearchedTerms', @level2type=N'COLUMN',@level2name=N'LastModified'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Schools]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Schools](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_Schools_Name]  DEFAULT (''),
	[PinyinName] [varchar](512) NOT NULL CONSTRAINT [DF_tn_Schools_PinyinName]  DEFAULT (''),
	[ShortPinyinName] [varchar](64) NOT NULL CONSTRAINT [DF_tn_Schools_ShortPinyinName]  DEFAULT (''),
	[SchoolType] [smallint] NOT NULL CONSTRAINT [DF_tn_Schools_SchoolType]  DEFAULT ((0)),
	[AreaCode] [varchar](8) NOT NULL CONSTRAINT [DF_tn_Schools_AreaCode]  DEFAULT (''),
	[DisplayOrder] [bigint] NOT NULL CONSTRAINT [DF_tn_Schools_DisplayOrder]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_Schools] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Schools]') AND name = N'IX_tn_Schools_AreaCode')
CREATE NONCLUSTERED INDEX [IX_tn_Schools_AreaCode] ON [dbo].[tn_Schools] 
(
	[AreaCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Schools]') AND name = N'IX_tn_Schools_DisplayOrder')
CREATE NONCLUSTERED INDEX [IX_tn_Schools_DisplayOrder] ON [dbo].[tn_Schools] 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Schools]') AND name = N'IX_tn_Schools_PinyinName')
CREATE NONCLUSTERED INDEX [IX_tn_Schools_PinyinName] ON [dbo].[tn_Schools] 
(
	[PinyinName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Schools]') AND name = N'IX_tn_Schools_SchoolType')
CREATE NONCLUSTERED INDEX [IX_tn_Schools_SchoolType] ON [dbo].[tn_Schools] 
(
	[SchoolType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Schools]') AND name = N'IX_tn_Schools_ShortPinyinName')
CREATE NONCLUSTERED INDEX [IX_tn_Schools_ShortPinyinName] ON [dbo].[tn_Schools] 
(
	[ShortPinyinName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Schools', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Schools', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Schools', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'院校名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Schools', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Schools', N'COLUMN',N'PinyinName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称的拼音(例如“汉语”：hanyu)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Schools', @level2type=N'COLUMN',@level2name=N'PinyinName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Schools', N'COLUMN',N'ShortPinyinName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称的简写拼音（例如“汉语”的简写拼音：hy）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Schools', @level2type=N'COLUMN',@level2name=N'ShortPinyinName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Schools', N'COLUMN',N'SchoolType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'学校类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Schools', @level2type=N'COLUMN',@level2name=N'SchoolType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Schools', N'COLUMN',N'AreaCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在地区编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Schools', @level2type=N'COLUMN',@level2name=N'AreaCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Schools', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Schools', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Roles](
	[RoleName] [varchar](32) NOT NULL,
	[FriendlyRoleName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Roles_FriendlyRoleName]  DEFAULT (''),
	[IsBuiltIn] [tinyint] NOT NULL CONSTRAINT [DF_tn_Roles_IsBuiltIn]  DEFAULT ((0)),
	[ConnectToUser] [tinyint] NOT NULL CONSTRAINT [DF_tn_Roles_ConnectToUser]  DEFAULT ((0)),
	[ApplicationId] [int] NOT NULL CONSTRAINT [DF_tn_Roles_ApplicationId]  DEFAULT ((0)),
	[IsPublic] [tinyint] NOT NULL CONSTRAINT [DF_tn_Roles_IsPublic]  DEFAULT ((0)),
	[Description] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_Roles_Description]  DEFAULT (''),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_Roles_IsEnabled]  DEFAULT ((1)),
	[RoleImage] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_Roles_ImageName]  DEFAULT (''),
 CONSTRAINT [PK_tn_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'RoleName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称 注：仅允许字母、数字及.-_' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'RoleName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'FriendlyRoleName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色友好名称 用于对外显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'FriendlyRoleName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'IsBuiltIn'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是系统内置的    默认=0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'IsBuiltIn'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'ConnectToUser'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否直接关联到用户（例如：版主、注册用户 无需直接绑定到用户）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'ConnectToUser'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'哪个应用模块' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'IsPublic'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否对外显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'IsPublic'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Roles', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Roles', @level2type=N'COLUMN',@level2name=N'IsEnabled'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ReminderRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ReminderRecords](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ReminderModeId] [int] NOT NULL,
	[ReminderInfoTypeId] [int] NOT NULL,
	[ObjectId] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastReminderTime] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_ReminderRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ReminderRecords]') AND name = N'IX_tn_ReminderRecords_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_ReminderRecords_UserId] ON [dbo].[tn_ReminderRecords] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ReminderRecords', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ReminderRecords', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ReminderRecords', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ReminderRecords', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ReminderRecords', N'COLUMN',N'ReminderModeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提醒方式(Email=1，手机=2)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ReminderRecords', @level2type=N'COLUMN',@level2name=N'ReminderModeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ReminderRecords', N'COLUMN',N'ReminderInfoTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提醒信息类型（Message=1，Notice=2，Invitation=3）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ReminderRecords', @level2type=N'COLUMN',@level2name=N'ReminderInfoTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ReminderRecords', N'COLUMN',N'ObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提醒对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ReminderRecords', @level2type=N'COLUMN',@level2name=N'ObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ReminderRecords', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ReminderRecords', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ReminderRecords', N'COLUMN',N'LastReminderTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后提醒时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ReminderRecords', @level2type=N'COLUMN',@level2name=N'LastReminderTime'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RelatedTags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_RelatedTags](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TagId] [bigint] NOT NULL,
	[RelatedTagId] [bigint] NOT NULL,
 CONSTRAINT [PK_tn_RelatedTags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RelatedTags]') AND name = N'IX_tn_RelatedTags_TagId_RelatedTagId')
CREATE NONCLUSTERED INDEX [IX_tn_RelatedTags_TagId_RelatedTagId] ON [dbo].[tn_RelatedTags] 
(
	[RelatedTagId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RelatedTags', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RelatedTags', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RelatedTags', N'COLUMN',N'TagId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RelatedTags', @level2type=N'COLUMN',@level2name=N'TagId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RelatedTags', N'COLUMN',N'RelatedTagId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关标签Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RelatedTags', @level2type=N'COLUMN',@level2name=N'RelatedTagId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItemTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_RecommendItemTypes](
	[TypeId] [varchar](8) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](512) NOT NULL,
	[HasFeaturedImage] [tinyint] NOT NULL CONSTRAINT [DF_tn_RecommendItemTypes_HasFeaturedImage]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_RecommendItemTypes] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItemTypes]') AND name = N'IX_tn_RecommendItemTypes_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_RecommendItemTypes_TenantTypeId] ON [dbo].[tn_RecommendItemTypes] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItemTypes', N'COLUMN',N'TypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建后不允许修改，建议格式为：6位TenantTypeId +2位顺序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItemTypes', @level2type=N'COLUMN',@level2name=N'TypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItemTypes', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItemTypes', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItemTypes', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐类型名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItemTypes', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItemTypes', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐类型描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItemTypes', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItemTypes', N'COLUMN',N'HasFeaturedImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含标题图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItemTypes', @level2type=N'COLUMN',@level2name=N'HasFeaturedImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItemTypes', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItemTypes', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_RecommendItems](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[TypeId] [varchar](8) NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[ItemName] [nvarchar](255) NOT NULL,
	[FeaturedImage] [nvarchar](512) NOT NULL CONSTRAINT [DF_spb_RecommendItems_FeaturedImage]  DEFAULT (''),
	[ReferrerName] [nvarchar](64) NOT NULL,
	[ReferrerId] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[ExpiredDate] [datetime] NOT NULL,
	[DisplayOrder] [bigint] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
	[IsLink] [tinyint] NOT NULL CONSTRAINT [DF_tn_RecommendItems_IsLink]  DEFAULT ((0)),
 CONSTRAINT [PK_spb_RecommendItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItems]') AND name = N'IK_spb_RecommendItems_ItemId')
CREATE NONCLUSTERED INDEX [IK_spb_RecommendItems_ItemId] ON [dbo].[tn_RecommendItems] 
(
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItems]') AND name = N'IK_spb_RecommendItems_TenantTypeId')
CREATE NONCLUSTERED INDEX [IK_spb_RecommendItems_TenantTypeId] ON [dbo].[tn_RecommendItems] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RecommendItems]') AND name = N'IK_spb_RecommendItems_TypeId')
CREATE NONCLUSTERED INDEX [IK_spb_RecommendItems_TypeId] ON [dbo].[tn_RecommendItems] 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'TypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'TypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'ItemId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容实体Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'ItemId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'ItemName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐标题（默认为内容名称或标题，允许推荐人修改）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'ItemName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'FeaturedImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐标题图(存储图片文件名或完整图片链接地址)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'FeaturedImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'ReferrerName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人DisplayName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'ReferrerName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'ReferrerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐人用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'ReferrerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'ExpiredDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐期限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'ExpiredDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序顺序（默认和Id一致）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RecommendItems', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RecommendItems', @level2type=N'COLUMN',@level2name=N'PropertyValues'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Ratings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Ratings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ObjectId] [bigint] NOT NULL CONSTRAINT [DF_tn_Ratings_ObjectId]  DEFAULT ((0)),
	[TenantTypeId] [char](6) NOT NULL,
	[OwnerId] [bigint] NOT NULL CONSTRAINT [DF_tn_Ratings_OwnerId]  DEFAULT ((0)),
	[RateCount] [int] NOT NULL CONSTRAINT [DF_tn_Ratings_RateCount]  DEFAULT ((0)),
	[Comprehensive] [float] NOT NULL,
	[RateSum] [int] NOT NULL CONSTRAINT [DF_tn_Ratings_RateSum]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_Ratings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Ratings]') AND name = N'IX_tn_Ratings_ObjectId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Ratings_ObjectId_TenantTypeId] ON [dbo].[tn_Ratings] 
(
	[ObjectId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Ratings', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Ratings', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Ratings', N'COLUMN',N'ObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Ratings', @level2type=N'COLUMN',@level2name=N'ObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Ratings', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Ratings', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Ratings', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Ratings', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Ratings', N'COLUMN',N'RateCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评价总数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Ratings', @level2type=N'COLUMN',@level2name=N'RateCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Ratings', N'COLUMN',N'Comprehensive'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评价结果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Ratings', @level2type=N'COLUMN',@level2name=N'Comprehensive'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Ratings', N'COLUMN',N'RateSum'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评价总分值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Ratings', @level2type=N'COLUMN',@level2name=N'RateSum'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RatingRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_RatingRecords](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ObjectId] [bigint] NOT NULL CONSTRAINT [DF_tn_ RatingRecords_ObjectId]  DEFAULT ((0)),
	[TenantTypeId] [char](6) NOT NULL,
	[RateNumber] [tinyint] NOT NULL CONSTRAINT [DF_tn_ RatingRecords_RateNumber]  DEFAULT ((1)),
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_tn_ RatingRecords_UserId]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_ RatingRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RatingRecords]') AND name = N'IX_tn_ RatingRecords_ObjectId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_ RatingRecords_ObjectId_TenantTypeId] ON [dbo].[tn_RatingRecords] 
(
	[ObjectId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RatingRecords]') AND name = N'IX_tn_ RatingRecords_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_ RatingRecords_UserId] ON [dbo].[tn_RatingRecords] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingRecords', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingRecords', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingRecords', N'COLUMN',N'ObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingRecords', @level2type=N'COLUMN',@level2name=N'ObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingRecords', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingRecords', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingRecords', N'COLUMN',N'RateNumber'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'星级评价等级类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingRecords', @level2type=N'COLUMN',@level2name=N'RateNumber'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingRecords', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingRecords', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingRecords', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingRecords', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_RatingGrades]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_RatingGrades](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ObjectId] [bigint] NOT NULL CONSTRAINT [DF_tn_RatingGrades_ObjectId]  DEFAULT ((0)),
	[TenantTypeId] [char](6) NOT NULL,
	[RateNumber] [tinyint] NOT NULL CONSTRAINT [DF_tn_RatingGrades_RateNumber]  DEFAULT ((1)),
	[RateCount] [int] NOT NULL,
 CONSTRAINT [PK_tn_RatingGrades] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_RatingGrades]') AND name = N'IX_tn_RatingGrades_ObjectId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_RatingGrades_ObjectId_TenantTypeId] ON [dbo].[tn_RatingGrades] 
(
	[ObjectId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingGrades', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingGrades', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingGrades', N'COLUMN',N'ObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingGrades', @level2type=N'COLUMN',@level2name=N'ObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingGrades', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingGrades', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingGrades', N'COLUMN',N'RateNumber'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'星级评价等级类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingGrades', @level2type=N'COLUMN',@level2name=N'RateNumber'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_RatingGrades', N'COLUMN',N'RateCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'星级统计总数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_RatingGrades', @level2type=N'COLUMN',@level2name=N'RateCount'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PrivacyItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PrivacyItems](
	[ItemKey] [varchar](32) NOT NULL,
	[ItemGroupId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[ItemName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_PrivacyItems_ItemName]  DEFAULT (''),
	[Description] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_PrivacyItems_Description]  DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_PrivacyItems_DisplayOrder]  DEFAULT ((0)),
	[PrivacyStatus] [smallint] NOT NULL CONSTRAINT [DF_tn_PrivacyItems_PrivacyStatus]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_PrivacyItems] PRIMARY KEY CLUSTERED 
(
	[ItemKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_PrivacyItems]') AND name = N'IX_ItemGroupId')
CREATE NONCLUSTERED INDEX [IX_ItemGroupId] ON [dbo].[tn_PrivacyItems] 
(
	[ItemGroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PrivacyItems', N'COLUMN',N'ItemKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私项目标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PrivacyItems', @level2type=N'COLUMN',@level2name=N'ItemKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PrivacyItems', N'COLUMN',N'ItemGroupId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私项目分组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PrivacyItems', @level2type=N'COLUMN',@level2name=N'ItemGroupId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PrivacyItems', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用程序Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PrivacyItems', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PrivacyItems', N'COLUMN',N'ItemName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私项目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PrivacyItems', @level2type=N'COLUMN',@level2name=N'ItemName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PrivacyItems', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私项目描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PrivacyItems', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PrivacyItems', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PrivacyItems', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PrivacyItems', N'COLUMN',N'PrivacyStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PrivacyItems', @level2type=N'COLUMN',@level2name=N'PrivacyStatus'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PresentAreas]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PresentAreas](
	[PresentAreaKey] [varchar](32) NOT NULL,
	[AllowMultipleInstances] [tinyint] NOT NULL CONSTRAINT [DF_tn_PresentAreas_AllowMultipleInstances]  DEFAULT ((1)),
	[EnableThemes] [tinyint] NOT NULL CONSTRAINT [DF_tn_PresentAreas_EnableThemes]  DEFAULT ((1)),
	[DefaultAppearanceId] [varchar](128) NOT NULL,
	[ThemeLocation] [varchar](255) NOT NULL,
 CONSTRAINT [PK_tn_PresentAreas] PRIMARY KEY CLUSTERED 
(
	[PresentAreaKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreas', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识（与目录名称相同）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreas', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreas', N'COLUMN',N'AllowMultipleInstances'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可有多个实例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreas', @level2type=N'COLUMN',@level2name=N'AllowMultipleInstances'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreas', N'COLUMN',N'EnableThemes'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用皮肤(bool)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreas', @level2type=N'COLUMN',@level2name=N'EnableThemes'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreas', N'COLUMN',N'DefaultAppearanceId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认AppearanceID（格式：PresentAreaKey,ThemeKey,AppearanceKey）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreas', @level2type=N'COLUMN',@level2name=N'DefaultAppearanceId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreas', N'COLUMN',N'ThemeLocation'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'皮肤文件所在位置（以”~/目录”表示）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreas', @level2type=N'COLUMN',@level2name=N'ThemeLocation'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PresentAreaNavigations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PresentAreaNavigations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NavigationId] [int] NOT NULL,
	[ParentNavigationId] [int] NOT NULL CONSTRAINT [DF_tn_PresentAreaNavigations_ParentNavigationId]  DEFAULT ((0)),
	[Depth] [int] NOT NULL CONSTRAINT [DF_tn_PresentAreaNavigations_Depth]  DEFAULT ((0)),
	[PresentAreaKey] [varchar](32) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[NavigationType] [int] NOT NULL,
	[NavigationText] [nvarchar](64) NOT NULL,
	[ResourceName] [nvarchar](64) NOT NULL,
	[NavigationUrl] [nvarchar](255) NOT NULL,
	[UrlRouteName] [varchar](64) NOT NULL,
	[RouteDataName] [nvarchar](255) NULL,
	[IconName] [nvarchar](32) NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[NavigationTarget] [varchar](32) NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_PresentAreaNavigations_DisplayOrder]  DEFAULT ((100)),
	[OnlyOwnerVisible] [tinyint] NOT NULL CONSTRAINT [DF_tn_PresentAreaNavigations_OnlyOwnerVisible]  DEFAULT ((0)),
	[IsLocked] [tinyint] NOT NULL CONSTRAINT [DF_tn_PresentAreaNavigations_IsLocked]  DEFAULT ((0)),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_PresentAreaNavigations_IsEnabled]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_PresentAreaNavigations_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'Depth'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'深度（从上到下以0开始）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'Depth'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域实例OwnerId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'NavigationType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'NavigationType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'NavigationText'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航文字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'NavigationText'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'ResourceName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航文字资源名称（如果同时设置NavigationText则以NavigationText优先）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'ResourceName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'NavigationUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'NavigationUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'UrlRouteName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用导航路由规则名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'UrlRouteName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'IconName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统内置图标名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'IconName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'ImageUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单文字旁边的图标url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'ImageUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'NavigationTarget'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是新开窗口还是在当前窗口（默认:_self）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'NavigationTarget'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'OnlyOwnerVisible'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否仅拥有者可见' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'OnlyOwnerVisible'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'IsLocked'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'IsLocked'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PresentAreaNavigations', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PresentAreaNavigations', @level2type=N'COLUMN',@level2name=N'IsEnabled'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointStatistics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PointStatistics](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[PointCategoryKey] [varchar](32) NOT NULL,
	[Points] [int] NOT NULL CONSTRAINT [DF_tn_PointStatistics_Points]  DEFAULT ((0)),
	[StatisticalYear] [smallint] NOT NULL,
	[StatisticalMonth] [smallint] NOT NULL,
	[StatisticalDay] [smallint] NOT NULL,
 CONSTRAINT [PK_tn_PointStatistics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointStatistics]') AND name = N'IX_tn_PointStatistics_PointCategoryKey')
CREATE NONCLUSTERED INDEX [IX_tn_PointStatistics_PointCategoryKey] ON [dbo].[tn_PointStatistics] 
(
	[PointCategoryKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointStatistics]') AND name = N'IX_tn_PointStatistics_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_PointStatistics_UserId] ON [dbo].[tn_PointStatistics] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PointRecords](
	[RecordId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[PointItemName] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](512) NOT NULL CONSTRAINT [DF_tn_PointRecords_Description]  DEFAULT (''),
	[ExperiencePoints] [int] NOT NULL CONSTRAINT [DF_tn_PointRecords_ExperiencePoints]  DEFAULT ((0)),
	[ReputationPoints] [int] NOT NULL CONSTRAINT [DF_tn_PointRecords_ReputationPoints]  DEFAULT ((0)),
	[TradePoints] [int] NOT NULL CONSTRAINT [DF_tn_PointRecords_TradePoints]  DEFAULT ((0)),
	[TradePoints2] [int] NOT NULL CONSTRAINT [DF_tn_PointRecords_TradePoints2]  DEFAULT ((0)),
	[TradePoints3] [int] NOT NULL CONSTRAINT [DF_tn_PointRecords_TradePoints3]  DEFAULT ((0)),
	[TradePoints4] [int] NOT NULL CONSTRAINT [DF_tn_PointRecords_TradePoints4]  DEFAULT ((0)),
	[IsIncome] [tinyint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_PointRecords] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointRecords]') AND name = N'IX_tn_PointRecords_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_PointRecords_UserId] ON [dbo].[tn_PointRecords] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PointItems](
	[ItemKey] [varchar](32) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[ItemName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_PointItems_ItemName]  DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_PointItems_DisplayOrder]  DEFAULT ((0)),
	[ExperiencePoints] [int] NOT NULL CONSTRAINT [DF_tn_PointItems_ExperiencePoints]  DEFAULT ((0)),
	[ReputationPoints] [int] NOT NULL CONSTRAINT [DF_tn_PointItems_ReputationPoints]  DEFAULT ((0)),
	[TradePoints] [int] NOT NULL CONSTRAINT [DF_tn_PointItems_TradePoints]  DEFAULT ((0)),
	[TradePoints2] [int] NOT NULL CONSTRAINT [DF_tn_PointItems_TradePoints2]  DEFAULT ((0)),
	[TradePoints3] [int] NOT NULL CONSTRAINT [DF_tn_PointItems_TradePoints3]  DEFAULT ((0)),
	[TradePoints4] [int] NOT NULL CONSTRAINT [DF_tn_PointItems_TradePoints4]  DEFAULT ((0)),
	[Description] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_PointItems_Description]  DEFAULT (''),
 CONSTRAINT [PK_tn_PointItems] PRIMARY KEY CLUSTERED 
(
	[ItemKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PointCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PointCategories](
	[CategoryKey] [varchar](32) NOT NULL,
	[CategoryName] [nvarchar](64) NOT NULL,
	[Unit] [nvarchar](8) NOT NULL,
	[QuotaPerDay] [int] NOT NULL CONSTRAINT [DF_tn_PointCategories_QuotaPerDay]  DEFAULT ((0)),
	[Description] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_PointCategories_Description]  DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_PointCategories_DisplayOrder]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_PointCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PermissionItemsInUserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PermissionItemsInUserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](32) NOT NULL,
	[ItemKey] [varchar](32) NOT NULL,
	[PermissionType] [int] NOT NULL CONSTRAINT [DF_tn_PermissionItemsInUserRoles_PermissionType]  DEFAULT ((1)),
	[PermissionQuota] [float] NOT NULL CONSTRAINT [DF_tn_PermissionItemsInUserRoles_PermissionQuota]  DEFAULT ((0)),
	[PermissionScope] [int] NOT NULL CONSTRAINT [DF_tn_PermissionItemsInUserRoles_PermissionScope]  DEFAULT ((4)),
	[IsLocked] [tinyint] NOT NULL CONSTRAINT [DF_tn_PermissionItemsInUserRoles_IsLocked]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_PermissionItemsInUserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_PermissionItemsInUserRoles]') AND name = N'IX_tn_PermissionItemsInUserRoles_RoleName')
CREATE NONCLUSTERED INDEX [IX_tn_PermissionItemsInUserRoles_RoleName] ON [dbo].[tn_PermissionItemsInUserRoles] 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItemsInUserRoles', N'COLUMN',N'RoleName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItemsInUserRoles', @level2type=N'COLUMN',@level2name=N'RoleName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItemsInUserRoles', N'COLUMN',N'ItemKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限项目标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItemsInUserRoles', @level2type=N'COLUMN',@level2name=N'ItemKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItemsInUserRoles', N'COLUMN',N'PermissionType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限设置类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItemsInUserRoles', @level2type=N'COLUMN',@level2name=N'PermissionType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItemsInUserRoles', N'COLUMN',N'PermissionQuota'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允许的权限额度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItemsInUserRoles', @level2type=N'COLUMN',@level2name=N'PermissionQuota'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItemsInUserRoles', N'COLUMN',N'PermissionScope'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允许的权限范围' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItemsInUserRoles', @level2type=N'COLUMN',@level2name=N'PermissionScope'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItemsInUserRoles', N'COLUMN',N'IsLocked'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItemsInUserRoles', @level2type=N'COLUMN',@level2name=N'IsLocked'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_PermissionItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_PermissionItems](
	[ItemKey] [varchar](32) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[ItemName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_PermissionItems_ItemName]  DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_PermissionItems_DisplayOrder]  DEFAULT ((0)),
	[EnableQuota] [tinyint] NOT NULL CONSTRAINT [DF_tn_PermissionItems_EnableQuota]  DEFAULT ((0)),
	[EnableScope] [tinyint] NOT NULL CONSTRAINT [DF_tn_PermissionItems_EnableScope]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_PermissionItems] PRIMARY KEY CLUSTERED 
(
	[ItemKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItems', N'COLUMN',N'ItemKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限项目标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItems', @level2type=N'COLUMN',@level2name=N'ItemKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItems', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用程序id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItems', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItems', N'COLUMN',N'ItemName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限项目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItems', @level2type=N'COLUMN',@level2name=N'ItemName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItems', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItems', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItems', N'COLUMN',N'EnableQuota'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用权限额度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItems', @level2type=N'COLUMN',@level2name=N'EnableQuota'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_PermissionItems', N'COLUMN',N'EnableScope'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用权限范围' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_PermissionItems', @level2type=N'COLUMN',@level2name=N'EnableScope'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ParsedMedias]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ParsedMedias](
	[Alias] [varchar](16) NOT NULL,
	[Url] [nvarchar](255) NOT NULL,
	[MediaType] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL CONSTRAINT [DF_tn_ParsedMedias_Name]  DEFAULT (''),
	[Description] [nvarchar](512) NOT NULL CONSTRAINT [DF_tn_ParsedMedias_Description]  DEFAULT (''),
	[ThumbnailUrl] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ParsedMedias_ThumbnailUrl]  DEFAULT (''),
	[PlayerUrl] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ParsedMedias_PlayerUrl]  DEFAULT (''),
	[SourceFileUrl] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ParsedMedias_SourceFileUrl]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_ParsedMedias] PRIMARY KEY CLUSTERED 
(
	[Alias] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'Alias'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url别名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'Alias'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'Url'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'Url'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'MediaType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'多媒体类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'MediaType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'多媒体名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'ThumbnailUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缩略图地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'ThumbnailUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'PlayerUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'播放器地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'PlayerUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'SourceFileUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'源文件地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'SourceFileUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ParsedMedias', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ParsedMedias', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OwnerData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_OwnerData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL CONSTRAINT [DF_tn_OwnerData_TenantTypeId]  DEFAULT (''),
	[DataKey] [nvarchar](32) NOT NULL,
	[LongValue] [bigint] NOT NULL CONSTRAINT [DF__tn_UserDa__LongV__3B2BBE9D]  DEFAULT ((0)),
	[DecimalValue] [bigint] NOT NULL,
	[StringValue] [nvarchar](255) NOT NULL CONSTRAINT [DF__tn_UserDa__Strin__3C1FE2D6]  DEFAULT (''),
 CONSTRAINT [PK_tn_UserData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_OwnerData]') AND name = N'tn_OwnerData_OwnerId_DatKey')
CREATE NONCLUSTERED INDEX [tn_OwnerData_OwnerId_DatKey] ON [dbo].[tn_OwnerData] 
(
	[OwnerId] ASC,
	[DataKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OwnerData', N'COLUMN',N'DataKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据键值（要求每个用户的Datakey唯一）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OwnerData', @level2type=N'COLUMN',@level2name=N'DataKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OwnerData', N'COLUMN',N'LongValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'long数据值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OwnerData', @level2type=N'COLUMN',@level2name=N'LongValue'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OwnerData', N'COLUMN',N'DecimalValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'decimal数据值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OwnerData', @level2type=N'COLUMN',@level2name=N'DecimalValue'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OperationLogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_OperationLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Source] [nvarchar](64) NOT NULL,
	[OperationType] [nvarchar](64) NOT NULL,
	[OperationObjectName] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_OperationLogs_OperationObjectName]  DEFAULT (''),
	[OperationObjectId] [bigint] NOT NULL,
	[Description] [nvarchar](2000) NOT NULL,
	[OperatorUserId] [bigint] NOT NULL,
	[Operator] [nvarchar](64) NOT NULL,
	[OperatorIP] [nvarchar](64) NOT NULL,
	[AccessUrl] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_OperationLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_OperationLogs]') AND name = N'IX_tn_OperationLogs_ApplicationId')
CREATE NONCLUSTERED INDEX [IX_tn_OperationLogs_ApplicationId] ON [dbo].[tn_OperationLogs] 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_OperationLogs]') AND name = N'IX_tn_OperationLogs_OperationType')
CREATE NONCLUSTERED INDEX [IX_tn_OperationLogs_OperationType] ON [dbo].[tn_OperationLogs] 
(
	[OperationType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'Source'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志来源，一般为应用模块名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'Source'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'OperationType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作类型标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'OperationType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'OperationObjectName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'OperationObjectName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'OperatorUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作者UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'OperatorUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'OperatorIP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作者IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'OperatorIP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'AccessUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作访问的url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'AccessUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OperationLogs', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OperationLogs', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OnlineUserStatistics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_OnlineUserStatistics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoggedUserCount] [int] NOT NULL CONSTRAINT [DF_tn_OnlineUserStatistics_LoggedUserCount]  DEFAULT ((0)),
	[AnonymousCount] [int] NOT NULL CONSTRAINT [DF_tn_OnlineUserStatistics_AnonymousCount]  DEFAULT ((0)),
	[UserCount] [int] NOT NULL CONSTRAINT [DF_tn_OnlineUserStatistics_UserCount]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_OnlineUserStatistics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_OnlineUserStatistics]') AND name = N'IX_tn_OnlineUserStatistics_UserCount')
CREATE NONCLUSTERED INDEX [IX_tn_OnlineUserStatistics_UserCount] ON [dbo].[tn_OnlineUserStatistics] 
(
	[UserCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUserStatistics', N'COLUMN',N'LoggedUserCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在线登录用户数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUserStatistics', @level2type=N'COLUMN',@level2name=N'LoggedUserCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUserStatistics', N'COLUMN',N'AnonymousCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在线匿名用户数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUserStatistics', @level2type=N'COLUMN',@level2name=N'AnonymousCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUserStatistics', N'COLUMN',N'UserCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在线用户数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUserStatistics', @level2type=N'COLUMN',@level2name=N'UserCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUserStatistics', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUserStatistics', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_OnlineUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_OnlineUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UserName] [nvarchar](64) NOT NULL,
	[DisplayName] [nvarchar](64) NOT NULL,
	[LastActivityTime] [datetime] NOT NULL,
	[LastAction] [nvarchar](512) NOT NULL CONSTRAINT [DF_tn_OnlineUsers_LastAction]  DEFAULT (''),
	[Ip] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_OnlineUsers_Ip]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_OnlineUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUsers', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUsers', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUsers', N'COLUMN',N'UserName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUsers', @level2type=N'COLUMN',@level2name=N'UserName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUsers', N'COLUMN',N'DisplayName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对外显示的名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUsers', @level2type=N'COLUMN',@level2name=N'DisplayName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUsers', N'COLUMN',N'LastActivityTime'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次活动时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUsers', @level2type=N'COLUMN',@level2name=N'LastActivityTime'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUsers', N'COLUMN',N'LastAction'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次操作' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUsers', @level2type=N'COLUMN',@level2name=N'LastAction'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUsers', N'COLUMN',N'Ip'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUsers', @level2type=N'COLUMN',@level2name=N'Ip'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_OnlineUsers', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_OnlineUsers', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Notices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Notices](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL CONSTRAINT [DF_tn_Notices_ApplicationId]  DEFAULT ((0)),
	[TypeId] [int] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[TemplateName] [nvarchar](64) NOT NULL,
	[LeadingActorUserId] [bigint] NOT NULL,
	[LeadingActor] [nvarchar](64) NOT NULL,
	[RelativeObjectUrl] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_Notices_RelativeObjectName1]  DEFAULT (''),
	[RelativeObjectName] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_Notices_RelativeObjectName]  DEFAULT (''),
	[RelativeObjectId] [bigint] NOT NULL,
	[Body] [nvarchar](2000) NOT NULL CONSTRAINT [DF_tn_Notices_Body]  DEFAULT (''),
	[Status] [tinyint] NOT NULL CONSTRAINT [DF_tn_Notices_Status]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_tn_Notices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Notices]') AND name = N'IX_tn_Notices_ApplicationId')
CREATE NONCLUSTERED INDEX [IX_tn_Notices_ApplicationId] ON [dbo].[tn_Notices] 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Notices]') AND name = N'IX_tn_Notices_TypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Notices_TypeId] ON [dbo].[tn_Notices] 
(
	[TypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Notices]') AND name = N'IX_tn_Notices_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_Notices_UserId] ON [dbo].[tn_Notices] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'TypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'TypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知接收人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'TemplateName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通知模板名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'TemplateName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'LeadingActorUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主角UserID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'LeadingActorUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'LeadingActor'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主角' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'LeadingActor'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'RelativeObjectUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关项对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'RelativeObjectUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'RelativeObjectName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关项对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'RelativeObjectName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'RelativeObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关项对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'RelativeObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'Status'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理状态  0= Unhandled:未处理;1= Handled 已处理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'Status'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Notices', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Notices', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_MessagesInSessions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_MessagesInSessions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SessionId] [bigint] NOT NULL,
	[MessageId] [bigint] NOT NULL CONSTRAINT [DF_tn_MessagesInSessions_MessageId]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_MessagesInSessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_MessagesInSessions]') AND name = N'IX_tn_MessagesInSessions_SessionId')
CREATE NONCLUSTERED INDEX [IX_tn_MessagesInSessions_SessionId] ON [dbo].[tn_MessagesInSessions] 
(
	[SessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessagesInSessions', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessagesInSessions', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessagesInSessions', N'COLUMN',N'SessionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会话Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessagesInSessions', @level2type=N'COLUMN',@level2name=N'SessionId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessagesInSessions', N'COLUMN',N'MessageId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'私信Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessagesInSessions', @level2type=N'COLUMN',@level2name=N'MessageId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_MessageSessions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_MessageSessions](
	[SessionId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_tn_MessageSessions_UserId]  DEFAULT ((0)),
	[OtherUserId] [bigint] NOT NULL CONSTRAINT [DF_tn_MessageSessions_OtherUserId]  DEFAULT ((0)),
	[LastMessageId] [bigint] NOT NULL CONSTRAINT [DF_tn_MessageSessions_LastMessageId]  DEFAULT ((0)),
	[MessageCount] [int] NOT NULL CONSTRAINT [DF_tn_MessageSessions_MessageCount]  DEFAULT ((0)),
	[UnreadMessageCount] [int] NOT NULL CONSTRAINT [DF_tn_MessageSessions_UnreadItemCount]  DEFAULT ((0)),
	[MessageType] [int] NOT NULL CONSTRAINT [DF_tn_MessageSessions_MessageType]  DEFAULT ((0)),
	[LastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_MessageSessions] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'SessionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SessionId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'SessionId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会话拥有者UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'OtherUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会话参与人UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'OtherUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'LastMessageId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'会话中最新的私信MessageId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'LastMessageId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'MessageCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信息数统计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'MessageCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'UnreadMessageCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未读信息数统计（用来显示未读私信统计数和和标示会话的阅读状态）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'UnreadMessageCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'MessageType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'MessageType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_MessageSessions', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后回复日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_MessageSessions', @level2type=N'COLUMN',@level2name=N'LastModified'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Messages]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Messages](
	[MessageId] [bigint] IDENTITY(1,1) NOT NULL,
	[SenderUserId] [bigint] NOT NULL CONSTRAINT [DF_tn_Messages_SenderUserId]  DEFAULT ((0)),
	[Sender] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Messages_Sender]  DEFAULT (''),
	[ReceiverUserId] [bigint] NOT NULL CONSTRAINT [DF_tn_Messages_ReceiverUserId]  DEFAULT ((0)),
	[Receiver] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Messages_Receiver]  DEFAULT (''),
	[Subject] [nvarchar](255) NULL,
	[Body] [nvarchar](4000) NOT NULL CONSTRAINT [DF_tn_Messages_Body]  DEFAULT (''),
	[IsRead] [tinyint] NOT NULL CONSTRAINT [DF_tn_Messages_IsRead]  DEFAULT ((0)),
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Messages_IP]  DEFAULT (N'000.000.000.000'),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'MessageId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MessageId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'MessageId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'SenderUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发件人UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'SenderUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'Sender'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发件人的DisplayName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'Sender'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'ReceiverUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收件人UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'ReceiverUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'Receiver'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收件人DisplayName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'Receiver'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'Subject'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'私信标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'Subject'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'私信内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'IsRead'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已读' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'IsRead'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'私信来源IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Messages', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Messages', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInTags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ItemsInTags](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](128) NULL,
	[TagInOwnerId] [bigint] NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
 CONSTRAINT [PK_tn_ItemsInTags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInTags]') AND name = N'IX_tn_ItemsInTags_ItemId')
CREATE NONCLUSTERED INDEX [IX_tn_ItemsInTags_ItemId] ON [dbo].[tn_ItemsInTags] 
(
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInTags]') AND name = N'IX_tn_ItemsInTags_TagName')
CREATE NONCLUSTERED INDEX [IX_tn_ItemsInTags_TagName] ON [dbo].[tn_ItemsInTags] 
(
	[TagName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInTags]') AND name = N'IX_tn_ItemsInTags_TagsInOwnersId')
CREATE NONCLUSTERED INDEX [IX_tn_ItemsInTags_TagsInOwnersId] ON [dbo].[tn_ItemsInTags] 
(
	[TagInOwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ItemsInTags', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ItemsInTags', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ItemsInTags', N'COLUMN',N'TagName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ItemsInTags', @level2type=N'COLUMN',@level2name=N'TagName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ItemsInTags', N'COLUMN',N'TagInOwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签与拥有着关联Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ItemsInTags', @level2type=N'COLUMN',@level2name=N'TagInOwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ItemsInTags', N'COLUMN',N'ItemId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容向Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ItemsInTags', @level2type=N'COLUMN',@level2name=N'ItemId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ItemsInCategories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryId] [bigint] NOT NULL,
	[ItemId] [bigint] NOT NULL,
 CONSTRAINT [PK_tn_ItemsInCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInCategories]') AND name = N'IX_tn_ItemsInCategories_CategoryId_ItemId')
CREATE NONCLUSTERED INDEX [IX_tn_ItemsInCategories_CategoryId_ItemId] ON [dbo].[tn_ItemsInCategories] 
(
	[CategoryId] ASC,
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ItemsInCategories]') AND name = N'IX_tn_ItemsInCategories_ItemId')
CREATE NONCLUSTERED INDEX [IX_tn_ItemsInCategories_ItemId] ON [dbo].[tn_ItemsInCategories] 
(
	[ItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ItemsInCategories', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ItemsInCategories', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ItemsInCategories', N'COLUMN',N'CategoryId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ItemsInCategories', @level2type=N'COLUMN',@level2name=N'CategoryId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ItemsInCategories', N'COLUMN',N'ItemId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容项Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ItemsInCategories', @level2type=N'COLUMN',@level2name=N'ItemId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InviteFriendRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_InviteFriendRecords](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_tn_InviteFriendRecords_UserId]  DEFAULT ((0)),
	[InvitedUserId] [bigint] NOT NULL CONSTRAINT [DF_tn_InviteFriendRecords_InvitedUserId]  DEFAULT ((0)),
	[Code] [nvarchar](512) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[InvitingUserHasBeingRewarded] [tinyint] NOT NULL,
 CONSTRAINT [PK_tn_InviteFriendRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InviteFriendRecords', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邀请人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InviteFriendRecords', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InviteFriendRecords', N'COLUMN',N'InvitedUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'受邀人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InviteFriendRecords', @level2type=N'COLUMN',@level2name=N'InvitedUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InviteFriendRecords', N'COLUMN',N'Code'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邀请码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InviteFriendRecords', @level2type=N'COLUMN',@level2name=N'Code'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InviteFriendRecords', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InviteFriendRecords', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Invitations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Invitations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL CONSTRAINT [DF_tn_Invitations_ApplicationId]  DEFAULT ((0)),
	[InvitationTypeKey] [nvarchar](64) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[SenderUserId] [bigint] NOT NULL,
	[Sender] [nvarchar](64) NOT NULL,
	[RelativeObjectName] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_Invitations_RelativeObjectName]  DEFAULT (''),
	[RelativeObjectId] [bigint] NOT NULL,
	[RelativeObjectUrl] [nvarchar](255) NOT NULL,
	[Status] [tinyint] NOT NULL CONSTRAINT [DF_tn_Invitations_Status]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_tn_Invitations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Invitations]') AND name = N'IX_tn_Invitations_ApplicationId')
CREATE NONCLUSTERED INDEX [IX_tn_Invitations_ApplicationId] ON [dbo].[tn_Invitations] 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Invitations]') AND name = N'IX_tn_Invitations_InvitationTypeKey')
CREATE NONCLUSTERED INDEX [IX_tn_Invitations_InvitationTypeKey] ON [dbo].[tn_Invitations] 
(
	[InvitationTypeKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Invitations]') AND name = N'IX_tn_Invitations_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_Invitations_UserId] ON [dbo].[tn_Invitations] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'InvitationTypeKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求类型key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'InvitationTypeKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求接受人用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'SenderUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求发送人用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'SenderUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'Sender'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求发送人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'Sender'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'RelativeObjectName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关项对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'RelativeObjectName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'RelativeObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关项对象id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'RelativeObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'RelativeObjectUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'相关项对象链接地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'RelativeObjectUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'Status'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求状态  0= Unhandled:未处理；1= Accept接受；2=Refuse 拒绝；' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'Status'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Invitations', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Invitations', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InvitationCodeStatistics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_InvitationCodeStatistics](
	[UserId] [bigint] NOT NULL,
	[CodeUnUsedCount] [int] NOT NULL,
	[CodeUsedCount] [int] NOT NULL,
	[CodeBuyedCount] [int] NOT NULL,
 CONSTRAINT [PK_tn_InvitationCodeStatistics] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodeStatistics', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodeStatistics', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodeStatistics', N'COLUMN',N'CodeUnUsedCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未使用的邀请码数量(仅当用户申请过邀请码时,才做记录)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodeStatistics', @level2type=N'COLUMN',@level2name=N'CodeUnUsedCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodeStatistics', N'COLUMN',N'CodeUsedCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用的邀请码数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodeStatistics', @level2type=N'COLUMN',@level2name=N'CodeUsedCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodeStatistics', N'COLUMN',N'CodeBuyedCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'购买的邀请码数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodeStatistics', @level2type=N'COLUMN',@level2name=N'CodeBuyedCount'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InvitationCodes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_InvitationCodes](
	[Code] [varchar](32) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[IsMultiple] [tinyint] NOT NULL,
	[ExpiredDate] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_InvitationCodes] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodes', N'COLUMN',N'Code'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'(使用MD5_16生成)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodes', @level2type=N'COLUMN',@level2name=N'Code'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodes', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodes', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodes', N'COLUMN',N'IsMultiple'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否可以多次使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodes', @level2type=N'COLUMN',@level2name=N'IsMultiple'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodes', N'COLUMN',N'ExpiredDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'过期日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodes', @level2type=N'COLUMN',@level2name=N'ExpiredDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InvitationCodes', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InvitationCodes', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_InitialNavigations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_InitialNavigations](
	[NavigationId] [int] NOT NULL,
	[ParentNavigationId] [int] NOT NULL CONSTRAINT [DF_tn_InitialNavigations_ParentNavigationId]  DEFAULT ((0)),
	[Depth] [int] NOT NULL CONSTRAINT [DF_tn_InitialNavigations_Depth]  DEFAULT ((0)),
	[PresentAreaKey] [varchar](32) NOT NULL,
	[ApplicationId] [int] NOT NULL CONSTRAINT [DF_tn_InitialNavigations_ApplicationId]  DEFAULT ((0)),
	[NavigationType] [int] NOT NULL,
	[NavigationText] [nvarchar](64) NOT NULL,
	[ResourceName] [nvarchar](64) NOT NULL,
	[NavigationUrl] [nvarchar](255) NOT NULL,
	[UrlRouteName] [varchar](64) NOT NULL,
	[RouteDataName] [nvarchar](255) NULL,
	[IconName] [nvarchar](32) NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[NavigationTarget] [varchar](32) NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_InitialNavigations_DisplayOrder]  DEFAULT ((100)),
	[OnlyOwnerVisible] [tinyint] NOT NULL CONSTRAINT [DF_tn_InitialNavigations_OnlyOwnerVisible]  DEFAULT ((0)),
	[IsLocked] [tinyint] NOT NULL CONSTRAINT [DF_tn_InitialNavigations_IsLocked]  DEFAULT ((0)),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_InitialNavigations_IsEnabled]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_InitialNavigations] PRIMARY KEY CLUSTERED 
(
	[NavigationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'Depth'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'深度（从上到下以0开始）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'Depth'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'NavigationType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'NavigationType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'NavigationText'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航文字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'NavigationText'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'ResourceName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航文字资源名称（如果同时设置NavigationText则以NavigationText优先）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'ResourceName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'NavigationUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航url， 如果是来源于应用,并且该字段为空,则根据UrlRouteName获取 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'NavigationUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'UrlRouteName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用导航路由规则名称 将会根据该规则名称获取应用导航地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'UrlRouteName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'IconName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统内置图标名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'IconName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'ImageUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单文字旁边的图标url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'ImageUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'NavigationTarget'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是新开窗口还是在当前窗口（默认:_self）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'NavigationTarget'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'OnlyOwnerVisible'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否仅拥有者可见' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'OnlyOwnerVisible'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'IsLocked'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'IsLocked'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_InitialNavigations', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_InitialNavigations', @level2type=N'COLUMN',@level2name=N'IsEnabled'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Follows]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Follows](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_tn_FollowedUsers_UserId]  DEFAULT ((0)),
	[FollowedUserId] [bigint] NOT NULL CONSTRAINT [DF_tn_FollowedUsers_FollowedUserId]  DEFAULT ((0)),
	[NoteName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_FollowedUsers_NoteName]  DEFAULT (''),
	[IsQuietly] [tinyint] NOT NULL CONSTRAINT [DF_tn_FollowedUsers_IsQuietly]  DEFAULT ((0)),
	[IsNewFollower] [tinyint] NOT NULL CONSTRAINT [DF_tn_FollowedUsers_IsNewFollower]  DEFAULT ((1)),
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
	[IsMutual] [tinyint] NOT NULL CONSTRAINT [DF_tn_Follows_IsMutual]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_FollowedUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Follows]') AND name = N'IX_tn_FollowedUsers_UserId_FollowedUserId')
CREATE NONCLUSTERED INDEX [IX_tn_FollowedUsers_UserId_FollowedUserId] ON [dbo].[tn_Follows] 
(
	[UserId] ASC,
	[FollowedUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Follows]') AND name = N'IX_tn_Follows_FollowedUserId')
CREATE NONCLUSTERED INDEX [IX_tn_Follows_FollowedUserId] ON [dbo].[tn_Follows] 
(
	[FollowedUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Follows', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关注用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Follows', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Follows', N'COLUMN',N'FollowedUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被关注用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Follows', @level2type=N'COLUMN',@level2name=N'FollowedUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Follows', N'COLUMN',N'NoteName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Follows', @level2type=N'COLUMN',@level2name=N'NoteName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Follows', N'COLUMN',N'IsQuietly'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为悄悄关注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Follows', @level2type=N'COLUMN',@level2name=N'IsQuietly'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Follows', N'COLUMN',N'IsNewFollower'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为新增粉丝' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Follows', @level2type=N'COLUMN',@level2name=N'IsNewFollower'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Follows', N'COLUMN',N'IsMutual'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否相互关注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Follows', @level2type=N'COLUMN',@level2name=N'IsMutual'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Favorites]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Favorites](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL CONSTRAINT [DF_tn_Favorites_TenantTypeId]  DEFAULT (''),
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_tn_Favorites_UserId]  DEFAULT ((0)),
	[ObjectId] [bigint] NOT NULL CONSTRAINT [DF_tn_Favorites_ObjectId]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_Favorites] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Favorites]') AND name = N'IX_tn_Favorites_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Favorites_TenantTypeId] ON [dbo].[tn_Favorites] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Favorites]') AND name = N'IX_tn_Favorites_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_Favorites_UserId] ON [dbo].[tn_Favorites] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_EmotionCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_EmotionCategories](
	[DirectoryName] [nvarchar](32) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_EmotionCategories_DisplayOrder]  DEFAULT ((100)),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_EmotionCategories_IsEnabled]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_EmotionCategories] PRIMARY KEY CLUSTERED 
(
	[DirectoryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_EmotionCategories', N'COLUMN',N'DirectoryName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'表情包目录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_EmotionCategories', @level2type=N'COLUMN',@level2name=N'DirectoryName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_EmotionCategories', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_EmotionCategories', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_EmotionCategories', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_EmotionCategories', @level2type=N'COLUMN',@level2name=N'IsEnabled'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_EmailQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_EmailQueue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Priority] [int] NOT NULL CONSTRAINT [DF_tn_EmailQueue_Priority]  DEFAULT ((0)),
	[IsBodyHtml] [tinyint] NOT NULL CONSTRAINT [DF_tn_EmailQueue_IsBodyHtml]  DEFAULT ((1)),
	[MailTo] [nvarchar](max) NOT NULL,
	[MailCc] [nvarchar](max) NULL,
	[MailBcc] [nvarchar](max) NULL,
	[MailFrom] [nvarchar](512) NOT NULL,
	[Subject] [nvarchar](512) NOT NULL CONSTRAINT [DF_tn_EmailQueue_Subject]  DEFAULT (''),
	[Body] [nvarchar](max) NOT NULL,
	[NextTryTime] [datetime] NOT NULL,
	[NumberOfTries] [int] NOT NULL CONSTRAINT [DF_tn_EmailQueue_NumberOfTries]  DEFAULT ((0)),
	[IsFailed] [tinyint] NOT NULL CONSTRAINT [DF_tn_EmailQueue_IsFailed]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_EmailQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ContentPrivacySpecifyObjects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ContentPrivacySpecifyObjects](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[ContentId] [bigint] NOT NULL,
	[SpecifyObjectTypeId] [int] NOT NULL,
	[SpecifyObjectId] [bigint] NOT NULL,
	[SpecifyObjectName] [nvarchar](64) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_ContentPrivacySpecifyObjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ContentPrivacySpecifyObjects]') AND name = N'IX_ContentId')
CREATE NONCLUSTERED INDEX [IX_ContentId] ON [dbo].[tn_ContentPrivacySpecifyObjects] 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ContentPrivacySpecifyObjects]') AND name = N'IX_SpecifyObjectType')
CREATE NONCLUSTERED INDEX [IX_SpecifyObjectType] ON [dbo].[tn_ContentPrivacySpecifyObjects] 
(
	[SpecifyObjectTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ContentPrivacySpecifyObjects]') AND name = N'IX_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_TenantTypeId] ON [dbo].[tn_ContentPrivacySpecifyObjects] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ContentPrivacySpecifyObjects', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容项租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ContentPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ContentPrivacySpecifyObjects', N'COLUMN',N'ContentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容项Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ContentPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'ContentId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ContentPrivacySpecifyObjects', N'COLUMN',N'SpecifyObjectTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被指定对象类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ContentPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'SpecifyObjectTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ContentPrivacySpecifyObjects', N'COLUMN',N'SpecifyObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被指定对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ContentPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'SpecifyObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ContentPrivacySpecifyObjects', N'COLUMN',N'SpecifyObjectName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被指定对象名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ContentPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'SpecifyObjectName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ContentPrivacySpecifyObjects', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ContentPrivacySpecifyObjects', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_CommonOperations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_CommonOperations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NavigationId] [int] NOT NULL CONSTRAINT [DF_tn_CommonOperations_NavigationId]  DEFAULT ((0)),
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_tn_CommonOperations_UserId]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_CommonOperations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_CommonOperations]') AND name = N'IX_tn_NavigationId')
CREATE NONCLUSTERED INDEX [IX_tn_NavigationId] ON [dbo].[tn_CommonOperations] 
(
	[NavigationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_CommonOperations]') AND name = N'IX_tn_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_UserId] ON [dbo].[tn_CommonOperations] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Comments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentId] [bigint] NOT NULL,
	[CommentedObjectId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Author] [nvarchar](64) NOT NULL,
	[ToUserId] [bigint] NOT NULL,
	[ToUserDisplayName] [nvarchar](64) NOT NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[IsPrivate] [tinyint] NOT NULL,
	[AuditStatus] [smallint] NOT NULL,
	[ChildCount] [int] NOT NULL,
	[IsAnonymous] [tinyint] NOT NULL CONSTRAINT [DF_tn_Comments_AsAnonymous]  DEFAULT ((0)),
	[IP] [nvarchar](64) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_tn_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND name = N'IX_tn_Comments_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_tn_Comments_AuditStatus] ON [dbo].[tn_Comments] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND name = N'IX_tn_Comments_CommentedObjectId')
CREATE NONCLUSTERED INDEX [IX_tn_Comments_CommentedObjectId] ON [dbo].[tn_Comments] 
(
	[CommentedObjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND name = N'IX_tn_Comments_OwnerId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Comments_OwnerId_TenantTypeId] ON [dbo].[tn_Comments] 
(
	[OwnerId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND name = N'IX_tn_Comments_ParentId')
CREATE NONCLUSTERED INDEX [IX_tn_Comments_ParentId] ON [dbo].[tn_Comments] 
(
	[ParentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND name = N'IX_tn_Comments_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Comments_TenantTypeId] ON [dbo].[tn_Comments] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Comments]') AND name = N'IX_tn_Comments_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_Comments_UserId] ON [dbo].[tn_Comments] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'ParentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父评论Id（一级ParentId等于Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'ParentId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'CommentedObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被评论对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'CommentedObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id（4位ApplicationId+2位顺序号）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论人UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'Author'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论人名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'Author'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'ToUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被回复UserId（一级ToUserId为0）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'ToUserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'ToUserDisplayName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被回复人名称（一级ToUserDisplayName为空字符串）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'ToUserDisplayName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'Subject'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'Subject'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'IsPrivate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否属于悄悄话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'IsPrivate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'ChildCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子级评论数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'ChildCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'IsAnonymous'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否匿名评论' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'IsAnonymous'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Comments', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Comments', @level2type=N'COLUMN',@level2name=N'PropertyValues'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Categories](
	[CategoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentId] [bigint] NOT NULL CONSTRAINT [DF_tn_Categories_ParentId]  DEFAULT ((0)),
	[OwnerId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[CategoryName] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](255) NOT NULL DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_Categories_DisplayOrder]  DEFAULT ((0)),
	[Depth] [int] NOT NULL CONSTRAINT [DF_tn_Categories_Depth]  DEFAULT ((0)),
	[ChildCount] [int] NOT NULL CONSTRAINT [DF_tn_Categories_ChildCount]  DEFAULT ((0)),
	[ItemCount] [int] NOT NULL CONSTRAINT [DF_tn_Categories_ItemCount]  DEFAULT ((0)),
	[PrivacyStatus] [tinyint] NOT NULL CONSTRAINT [DF_tn_Categories_PrivacyStatus]  DEFAULT ((30)),
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_tn_Categories_AuditingStatus]  DEFAULT ((40)),
	[FeaturedItemId] [bigint] NOT NULL CONSTRAINT [DF_tn_Categories_FeaturedItemId]  DEFAULT ((0)),
	[LastModified] [datetime] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_tn_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Categories]') AND name = N'IX_tn_Categories_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_tn_Categories_AuditStatus] ON [dbo].[tn_Categories] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Categories]') AND name = N'IX_tn_Categories_CategoryName')
CREATE NONCLUSTERED INDEX [IX_tn_Categories_CategoryName] ON [dbo].[tn_Categories] 
(
	[CategoryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Categories]') AND name = N'IX_tn_Categories_DisplayOrder')
CREATE NONCLUSTERED INDEX [IX_tn_Categories_DisplayOrder] ON [dbo].[tn_Categories] 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Categories]') AND name = N'IX_tn_Categories_OwnerId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Categories_OwnerId_TenantTypeId] ON [dbo].[tn_Categories] 
(
	[OwnerId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Categories]') AND name = N'IX_tn_Categories_ParentId')
CREATE NONCLUSTERED INDEX [IX_tn_Categories_ParentId] ON [dbo].[tn_Categories] 
(
	[ParentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'CategoryId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'CategoryId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'ParentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父评论Id（顶级ParentId=0）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'ParentId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'CategoryName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'CategoryName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'Depth'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别深度 顶级类别 Depth=0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'Depth'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'ChildCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子类别数目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'ChildCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'ItemCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容项数目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'ItemCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'PrivacyStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'PrivacyStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'FeaturedItemId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'特征内容项目Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'FeaturedItemId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'LastModified'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Categories', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Categories', @level2type=N'COLUMN',@level2name=N'PropertyValues'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AuditItemsInUserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AuditItemsInUserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](32) NOT NULL,
	[ItemKey] [varchar](32) NOT NULL,
	[StrictDegree] [smallint] NOT NULL,
	[IsLocked] [tinyint] NOT NULL,
 CONSTRAINT [PK_tn_AuditItemsInUserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AuditItemsInUserRoles]') AND name = N'IX_tn_AuditItemsInUserRoles_RoleName')
CREATE NONCLUSTERED INDEX [IX_tn_AuditItemsInUserRoles_RoleName] ON [dbo].[tn_AuditItemsInUserRoles] 
(
	[RoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AuditItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AuditItems](
	[ItemKey] [varchar](32) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[ItemName] [nvarchar](64) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Description] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_tn_AuditItems] PRIMARY KEY CLUSTERED 
(
	[ItemKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AuditItems]') AND name = N'IX_tn_AuditItems_ApplicationId')
CREATE NONCLUSTERED INDEX [IX_tn_AuditItems_ApplicationId] ON [dbo].[tn_AuditItems] 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AuditItems]') AND name = N'IX_tn_AuditItems_DisplayOrder')
CREATE NONCLUSTERED INDEX [IX_tn_AuditItems_DisplayOrder] ON [dbo].[tn_AuditItems] 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AtUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AtUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[AssociateId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_tn_AtUsers_UserId]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_AtUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AtUsers]') AND name = N'IX_tn_AtUsers_AssociateId')
CREATE NONCLUSTERED INDEX [IX_tn_AtUsers_AssociateId] ON [dbo].[tn_AtUsers] 
(
	[AssociateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AtUsers]') AND name = N'IX_tn_AtUsers_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_AtUsers_TenantTypeId] ON [dbo].[tn_AtUsers] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AtUsers', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AtUsers', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AtUsers', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AtUsers', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AtUsers', N'COLUMN',N'AssociateId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联项Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AtUsers', @level2type=N'COLUMN',@level2name=N'AssociateId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attitudes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Attitudes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ObjectId] [bigint] NOT NULL CONSTRAINT [DF_tn_Attitudes_ObjectId]  DEFAULT ((0)),
	[SupportCount] [int] NOT NULL,
	[OpposeCount] [int] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[Comprehensive] [float] NOT NULL,
 CONSTRAINT [PK_tn_Attitudes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attitudes]') AND name = N'IX_tn_Attitudes_ObjectId')
CREATE NONCLUSTERED INDEX [IX_tn_Attitudes_ObjectId] ON [dbo].[tn_Attitudes] 
(
	[ObjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attitudes]') AND name = N'IX_tn_Attitudes_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Attitudes_TenantTypeId] ON [dbo].[tn_Attitudes] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attitudes', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attitudes', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attitudes', N'COLUMN',N'ObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attitudes', @level2type=N'COLUMN',@level2name=N'ObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attitudes', N'COLUMN',N'SupportCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支持数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attitudes', @level2type=N'COLUMN',@level2name=N'SupportCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attitudes', N'COLUMN',N'OpposeCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'反对数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attitudes', @level2type=N'COLUMN',@level2name=N'OpposeCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attitudes', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attitudes', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attitudes', N'COLUMN',N'Comprehensive'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'综合评价值（根据支持反对数加权所得）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attitudes', @level2type=N'COLUMN',@level2name=N'Comprehensive'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttitudeRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AttitudeRecords](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ObjectId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[IsSupport] [tinyint] NOT NULL,
 CONSTRAINT [PK_tn_AttitudeRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttitudeRecords]') AND name = N'IX_tn_AttitudeRecords_ObjectId')
CREATE NONCLUSTERED INDEX [IX_tn_AttitudeRecords_ObjectId] ON [dbo].[tn_AttitudeRecords] 
(
	[ObjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttitudeRecords]') AND name = N'IX_tn_AttitudeRecords_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_AttitudeRecords_TenantTypeId] ON [dbo].[tn_AttitudeRecords] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttitudeRecords', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttitudeRecords', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttitudeRecords', N'COLUMN',N'ObjectId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作对象Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttitudeRecords', @level2type=N'COLUMN',@level2name=N'ObjectId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttitudeRecords', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttitudeRecords', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttitudeRecords', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttitudeRecords', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttitudeRecords', N'COLUMN',N'IsSupport'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户是否支持（true为支持false为反对）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttitudeRecords', @level2type=N'COLUMN',@level2name=N'IsSupport'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attachments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Attachments](
	[AttachmentId] [bigint] IDENTITY(1,1) NOT NULL,
	[AssociateId] [bigint] NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UserDisplayName] [nvarchar](64) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_Attachments_FileName]  DEFAULT (''),
	[FriendlyFileName] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_Attachments_FriendlyFileName]  DEFAULT (''),
	[MediaType] [int] NOT NULL CONSTRAINT [DF_tn_Attachments_MediaType]  DEFAULT ((99)),
	[ContentType] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_Attachments_ContentType]  DEFAULT (''),
	[FileLength] [bigint] NOT NULL CONSTRAINT [DF_tn_Attachments_FileLength]  DEFAULT ((0)),
	[Height] [int] NOT NULL CONSTRAINT [DF_tn_Attachments_Height]  DEFAULT ((0)),
	[Width] [int] NOT NULL CONSTRAINT [DF_tn_Attachments_Width]  DEFAULT ((0)),
	[Price] [int] NOT NULL CONSTRAINT [DF_tn_Attachments_Price]  DEFAULT ((0)),
	[Password] [nvarchar](32) NOT NULL CONSTRAINT [DF_tn_Attachments_Password]  DEFAULT (''),
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Attachments_IP]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_tn_Attachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attachments]') AND name = N'IX_tn_Attachements_AssociateId')
CREATE NONCLUSTERED INDEX [IX_tn_Attachements_AssociateId] ON [dbo].[tn_Attachments] 
(
	[AssociateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attachments]') AND name = N'IX_tn_Attachements_OwnerId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Attachements_OwnerId_TenantTypeId] ON [dbo].[tn_Attachments] 
(
	[OwnerId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attachments]') AND name = N'IX_tn_Attachements_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Attachements_TenantTypeId] ON [dbo].[tn_Attachments] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Attachments]') AND name = N'IX_tn_Attachements_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_Attachements_UserId] ON [dbo].[tn_Attachments] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'AttachmentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'AttachmentId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'AssociateId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件关联Id（例如：博文Id、帖子Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'AssociateId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件上传人UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'UserDisplayName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件上传人名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'UserDisplayName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'FileName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际存储文件名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'FileName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'FriendlyFileName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件显示名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'FriendlyFileName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'MediaType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'媒体类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'MediaType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'ContentType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件MIME类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'ContentType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'FileLength'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'FileLength'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'Height'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片类型附件的高度（单位:px）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'Height'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'Width'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片类型附件的高度（单位:px）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'Width'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'Price'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'售价（积分）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'Price'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'Password'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下载密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'Password'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件上传人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Attachments', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Attachments', @level2type=N'COLUMN',@level2name=N'PropertyValues'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AttachmentDownloadRecords](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AttachmentId] [bigint] NOT NULL,
	[AssociateId] [bigint] NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UserDisplayName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_AttachmentDownloadRecords_UserDisplayName]  DEFAULT (''),
	[Price] [int] NOT NULL CONSTRAINT [DF_tn_AttachmentDownloadRecords_Price]  DEFAULT ((0)),
	[LastDownloadDate] [datetime] NOT NULL,
	[DownloadDate] [datetime] NOT NULL,
	[FromUrl] [nvarchar](512) NULL,
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_AttachmentDownloadRecords_IP]  DEFAULT (''),
 CONSTRAINT [PK_tn_AttachmentDownloadRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND name = N'IX_tn_AttachmentDownloadRecords_AssociateId')
CREATE NONCLUSTERED INDEX [IX_tn_AttachmentDownloadRecords_AssociateId] ON [dbo].[tn_AttachmentDownloadRecords] 
(
	[AssociateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND name = N'IX_tn_AttachmentDownloadRecords_AttachmentId')
CREATE NONCLUSTERED INDEX [IX_tn_AttachmentDownloadRecords_AttachmentId] ON [dbo].[tn_AttachmentDownloadRecords] 
(
	[AttachmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND name = N'IX_tn_AttachmentDownloadRecords_LastDownloadDate')
CREATE NONCLUSTERED INDEX [IX_tn_AttachmentDownloadRecords_LastDownloadDate] ON [dbo].[tn_AttachmentDownloadRecords] 
(
	[LastDownloadDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND name = N'IX_tn_AttachmentDownloadRecords_OwnerId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_AttachmentDownloadRecords_OwnerId_TenantTypeId] ON [dbo].[tn_AttachmentDownloadRecords] 
(
	[OwnerId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND name = N'IX_tn_AttachmentDownloadRecords_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_AttachmentDownloadRecords_TenantTypeId] ON [dbo].[tn_AttachmentDownloadRecords] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AttachmentDownloadRecords]') AND name = N'IX_tn_AttachmentDownloadRecords_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_AttachmentDownloadRecords_UserId] ON [dbo].[tn_AttachmentDownloadRecords] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'AttachmentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'AttachmentId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'AssociateId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件关联Id（例如：博文Id、帖子Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'AssociateId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'Price'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消费的积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'Price'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'LastDownloadDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最近下载日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'LastDownloadDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'DownloadDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下载日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'DownloadDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'FromUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下载附件时页面的URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'FromUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AttachmentDownloadRecords', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'附件下载人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AttachmentDownloadRecords', @level2type=N'COLUMN',@level2name=N'IP'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Areas]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Areas](
	[AreaCode] [varchar](8) NOT NULL,
	[ParentCode] [varchar](8) NOT NULL CONSTRAINT [DF_tn_Areas_ParentCode]  DEFAULT (''),
	[Name] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_Areas_Name]  DEFAULT (''),
	[PostCode] [nvarchar](8) NOT NULL CONSTRAINT [DF_tn_Areas_PostCode]  DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_Areas_DisplayOrder]  DEFAULT ((0)),
	[Depth] [int] NOT NULL CONSTRAINT [DF_tn_Areas_Depth]  DEFAULT ((0)),
	[ChildCount] [int] NOT NULL CONSTRAINT [DF_tn_Areas_ChildCount]  DEFAULT ((0)),
 CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED 
(
	[AreaCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Areas]') AND name = N'IX_Table_DisplayOrder')
CREATE NONCLUSTERED INDEX [IX_Table_DisplayOrder] ON [dbo].[tn_Areas] 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Areas', N'COLUMN',N'AreaCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Areas', @level2type=N'COLUMN',@level2name=N'AreaCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Areas', N'COLUMN',N'ParentCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级地区编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Areas', @level2type=N'COLUMN',@level2name=N'ParentCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Areas', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Areas', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Areas', N'COLUMN',N'PostCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Areas', @level2type=N'COLUMN',@level2name=N'PostCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Areas', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Areas', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Areas', N'COLUMN',N'Depth'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'深度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Areas', @level2type=N'COLUMN',@level2name=N'Depth'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Areas', N'COLUMN',N'ChildCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子地区个数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Areas', @level2type=N'COLUMN',@level2name=N'ChildCount'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Applications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Applications](
	[ApplicationId] [int] NOT NULL,
	[ApplicationKey] [varchar](64) NOT NULL,
	[Description] [varchar](255) NOT NULL CONSTRAINT [DF_tn_Applications_Description]  DEFAULT (''),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_Applications_IsEnabled]  DEFAULT ((1)),
	[IsLocked] [tinyint] NOT NULL CONSTRAINT [DF_tn_Applications_IsLocked]  DEFAULT ((0)),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_Applications_DisplayOrder]  DEFAULT ((1000)),
 CONSTRAINT [PK_tn_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Applications', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用程序Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Applications', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Applications', N'COLUMN',N'ApplicationKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Application英文唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Applications', @level2type=N'COLUMN',@level2name=N'ApplicationKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Applications', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Applications', @level2type=N'COLUMN',@level2name=N'IsEnabled'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Applications', N'COLUMN',N'IsLocked'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Applications', @level2type=N'COLUMN',@level2name=N'IsLocked'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Applications', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Applications', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationManagementOperations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ApplicationManagementOperations](
	[OperationId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[AssociatedNavigationId] [int] NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_AssociatedNavigationId]  DEFAULT ((0)),
	[PresentAreaKey] [varchar](32) NOT NULL,
	[OperationType] [int] NOT NULL,
	[OperationText] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_OperationText]  DEFAULT (''),
	[ResourceName] [nvarchar](64) NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_ResourceName]  DEFAULT (''),
	[NavigationUrl] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_NavigationUrl]  DEFAULT (''),
	[UrlRouteName] [nvarchar](64) NOT NULL,
	[RouteDataName] [nvarchar](255) NULL,
	[IconName] [nvarchar](32) NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[NavigationTarget] [varchar](32) NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_DisplayOrder]  DEFAULT ((100)),
	[OnlyOwnerVisible] [tinyint] NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_OnlyOwnerVisible]  DEFAULT ((1)),
	[IsLocked] [tinyint] NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_IsLocked]  DEFAULT ((0)),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_ApplicationManagementOperations_IsEnabled]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_ApplicationManagementOperations] PRIMARY KEY CLUSTERED 
(
	[OperationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'AssociatedNavigationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联的导航Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'AssociatedNavigationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'OperationType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'管理操作类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'OperationType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'OperationText'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作的文字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'OperationText'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'ResourceName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作文字资源名称（如果同时设置OperationText则以OperationText优先）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'ResourceName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'NavigationUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'NavigationUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'UrlRouteName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航路由规则名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'UrlRouteName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'IconName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统内置图标名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'IconName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'ImageUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单文字旁边的图标url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'ImageUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'NavigationTarget'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是新开窗口还是在当前窗口（默认:_self）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'NavigationTarget'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'IsLocked'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'IsLocked'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationManagementOperations', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationManagementOperations', @level2type=N'COLUMN',@level2name=N'IsEnabled'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationInPresentAreaSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ApplicationInPresentAreaSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[PresentAreaKey] [varchar](32) NOT NULL,
	[IsBuiltIn] [tinyint] NOT NULL CONSTRAINT [DF_tn_ApplicationInPresentAreaSettings_IsBuiltIn]  DEFAULT ((0)),
	[IsAutoInstall] [tinyint] NOT NULL CONSTRAINT [DF_tn_ApplicationInPresentAreaSettings_IsAutoInstall]  DEFAULT ((0)),
	[IsGenerateData] [tinyint] NOT NULL CONSTRAINT [DF_tn_ApplicationInPresentAreaSettings_IsGenerateData]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_ApplicationInPresentAreaSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaSettings', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaSettings', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaSettings', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaSettings', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaSettings', N'COLUMN',N'IsBuiltIn'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为呈现区域内置应用，内置应用默认创建，并且不允许卸载' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaSettings', @level2type=N'COLUMN',@level2name=N'IsBuiltIn'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaSettings', N'COLUMN',N'IsAutoInstall'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否在呈现区域自动安装' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaSettings', @level2type=N'COLUMN',@level2name=N'IsAutoInstall'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaSettings', N'COLUMN',N'IsGenerateData'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用在该呈现区域是否产生数据' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaSettings', @level2type=N'COLUMN',@level2name=N'IsGenerateData'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationInPresentAreaInstallations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ApplicationInPresentAreaInstallations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[PresentAreaKey] [varchar](32) NOT NULL,
 CONSTRAINT [PK_tn_ApplicationInPresentAreaInstallations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaInstallations', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域实例拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaInstallations', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaInstallations', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用程序Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaInstallations', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationInPresentAreaInstallations', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationInPresentAreaInstallations', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ApplicationData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[Datakey] [nvarchar](32) NOT NULL,
	[LongValue] [bigint] NOT NULL CONSTRAINT [DF_tn_ApplicationData_LongValue]  DEFAULT ((0)),
	[DecimalValue] [decimal](18, 4) NOT NULL CONSTRAINT [DF_tn_ApplicationData_DecimalValue]  DEFAULT ((0)),
	[StringValue] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_ApplicationData_StringValue]  DEFAULT (''),
 CONSTRAINT [PK_tn_ApplicationData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ApplicationData]') AND name = N'IX_tn_ApplicationData_ApplicationId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_ApplicationData_ApplicationId_TenantTypeId] ON [dbo].[tn_ApplicationData] 
(
	[ApplicationId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationData', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationData', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationData', N'COLUMN',N'Datakey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据键值（要求Application内唯一）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationData', @level2type=N'COLUMN',@level2name=N'Datakey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationData', N'COLUMN',N'LongValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'long数据值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationData', @level2type=N'COLUMN',@level2name=N'LongValue'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationData', N'COLUMN',N'DecimalValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'decimal数据值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationData', @level2type=N'COLUMN',@level2name=N'DecimalValue'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ApplicationData', N'COLUMN',N'StringValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字符串数据值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ApplicationData', @level2type=N'COLUMN',@level2name=N'StringValue'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AdvertisingsInPosition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AdvertisingsInPosition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AdvertisingId] [bigint] NOT NULL,
	[PositionId] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_tn_ AdvertisingsInPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingsInPosition', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingsInPosition', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingsInPosition', N'COLUMN',N'AdvertisingId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingsInPosition', @level2type=N'COLUMN',@level2name=N'AdvertisingId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingsInPosition', N'COLUMN',N'PositionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告位Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingsInPosition', @level2type=N'COLUMN',@level2name=N'PositionId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Advertisings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Advertisings](
	[AdvertisingId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[AdvertisingType] [smallint] NOT NULL,
	[Body] [nvarchar](max) NOT NULL CONSTRAINT [DF_tn_Advertisings_Body]  DEFAULT (''),
	[AttachmentUrl] [nvarchar](512) NOT NULL,
	[Url] [nvarchar](512) NOT NULL,
	[IsEnable] [tinyint] NOT NULL CONSTRAINT [DF_tn_Advertisings_IsEnable]  DEFAULT ((1)),
	[IsBlank] [tinyint] NOT NULL CONSTRAINT [DF_tn_Advertisings_IsBlank]  DEFAULT ((1)),
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[UseredPositionCount] [int] NOT NULL CONSTRAINT [DF_tn_Advertisings_UseredPositionCount]  DEFAULT ((0)),
	[DisplayOrder] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
	[TextStyle] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_tn_Advertisings] PRIMARY KEY CLUSTERED 
(
	[AdvertisingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'AdvertisingId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'AdvertisingId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'AdvertisingType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'AdvertisingType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'AttachmentUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网络图片地址/上传图片存储地址/flash地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'AttachmentUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'Url'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告链接地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'Url'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'IsEnable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'IsEnable'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'IsBlank'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否新开窗口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'IsBlank'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'StartDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'StartDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'EndDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'EndDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'UseredPositionCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投放数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'UseredPositionCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序顺序（默认和Id一致）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'LastModified'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Advertisings', N'COLUMN',N'TextStyle'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文字样式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Advertisings', @level2type=N'COLUMN',@level2name=N'TextStyle'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AdvertisingPosition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AdvertisingPosition](
	[PositionId] [nvarchar](25) NOT NULL,
	[PresentAreaKey] [varchar](32) NOT NULL,
	[Description] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_AdvertisingPosition_Description]  DEFAULT (''),
	[FeaturedImage] [nvarchar](512) NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[IsEnable] [tinyint] NOT NULL CONSTRAINT [DF_tn_AdvertisingPosition_IsEnable]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_AdvertisingPosition] PRIMARY KEY CLUSTERED 
(
	[PositionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingPosition', N'COLUMN',N'PositionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广告位Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingPosition', @level2type=N'COLUMN',@level2name=N'PositionId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingPosition', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投放区域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingPosition', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingPosition', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingPosition', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingPosition', N'COLUMN',N'FeaturedImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'示意图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingPosition', @level2type=N'COLUMN',@level2name=N'FeaturedImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingPosition', N'COLUMN',N'Width'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'宽度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingPosition', @level2type=N'COLUMN',@level2name=N'Width'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingPosition', N'COLUMN',N'Height'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'高度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingPosition', @level2type=N'COLUMN',@level2name=N'Height'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AdvertisingPosition', N'COLUMN',N'IsEnable'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AdvertisingPosition', @level2type=N'COLUMN',@level2name=N'IsEnable'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityUserInbox]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ActivityUserInbox](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivityId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_tn_ActivityUserInbox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityUserInbox]') AND name = N'IX_tn_ActivityUserInbox_ActivityId')
CREATE NONCLUSTERED INDEX [IX_tn_ActivityUserInbox_ActivityId] ON [dbo].[tn_ActivityUserInbox] 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityUserInbox]') AND name = N'IX_tn_ActivityUserInbox_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_ActivityUserInbox_UserId] ON [dbo].[tn_ActivityUserInbox] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityUserInbox', N'COLUMN',N'ActivityId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动态Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityUserInbox', @level2type=N'COLUMN',@level2name=N'ActivityId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityUserInbox', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityUserInbox', @level2type=N'COLUMN',@level2name=N'UserId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivitySiteInbox]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ActivitySiteInbox](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivityId] [bigint] NOT NULL,
 CONSTRAINT [PK_tn_ActivitySiteInbox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivitySiteInbox]') AND name = N'IX_tn_ActivitySiteInbox_ActivityId')
CREATE NONCLUSTERED INDEX [IX_tn_ActivitySiteInbox_ActivityId] ON [dbo].[tn_ActivitySiteInbox] 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivitySiteInbox', N'COLUMN',N'ActivityId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动态Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivitySiteInbox', @level2type=N'COLUMN',@level2name=N'ActivityId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityItemUserSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ActivityItemUserSettings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ItemKey] [varchar](32) NOT NULL,
	[IsReceived] [tinyint] NOT NULL,
 CONSTRAINT [PK_tn_ActivityItemUserSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityItemUserSettings]') AND name = N'IX_tn_ActivityItemUserSettings_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_ActivityItemUserSettings_UserId] ON [dbo].[tn_ActivityItemUserSettings] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItemUserSettings', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItemUserSettings', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItemUserSettings', N'COLUMN',N'ItemKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动态项目标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItemUserSettings', @level2type=N'COLUMN',@level2name=N'ItemKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItemUserSettings', N'COLUMN',N'IsReceived'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否接收' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItemUserSettings', @level2type=N'COLUMN',@level2name=N'IsReceived'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_ActivityItems](
	[ItemKey] [varchar](32) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[ItemName] [nvarchar](32) NOT NULL CONSTRAINT [DF_tn_ActivityItems_ItemName]  DEFAULT (''),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_tn_ActivityItems_DisplayOrder]  DEFAULT ((0)),
	[Description] [nvarchar](128) NOT NULL CONSTRAINT [DF_tn_ActivityItems_Description]  DEFAULT (''),
	[IsOnlyOnce] [tinyint] NOT NULL,
	[IsUserReceived] [tinyint] NOT NULL CONSTRAINT [DF_tn_ActivityItems_IsUserReceived]  DEFAULT ((1)),
	[IsSiteReceived] [tinyint] NOT NULL CONSTRAINT [DF_tn_ActivityItems_IsSiteReceived]  DEFAULT ((1)),
 CONSTRAINT [PK_tn_ActivityItems] PRIMARY KEY CLUSTERED 
(
	[ItemKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityItems]') AND name = N'IX_tn_ActivityItems_ApplicationId')
CREATE NONCLUSTERED INDEX [IX_tn_ActivityItems_ApplicationId] ON [dbo].[tn_ActivityItems] 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_ActivityItems]') AND name = N'IX_tn_ActivityItems_DisplayOrder')
CREATE NONCLUSTERED INDEX [IX_tn_ActivityItems_DisplayOrder] ON [dbo].[tn_ActivityItems] 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'ItemKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动态项目标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'ItemKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用程序Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'ItemName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'ItemName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'IsOnlyOnce'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每个Owner是否仅生成一个动态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'IsOnlyOnce'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'IsUserReceived'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否推送给用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'IsUserReceived'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_ActivityItems', N'COLUMN',N'IsSiteReceived'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否推送给站点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_ActivityItems', @level2type=N'COLUMN',@level2name=N'IsSiteReceived'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_Activities](
	[ActivityId] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[OwnerType] [smallint] NOT NULL,
	[OwnerName] [nvarchar](64) NOT NULL,
	[ActivityItemKey] [varchar](32) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[SourceId] [bigint] NOT NULL,
	[ReferenceId] [bigint] NOT NULL,
	[ReferenceTenantTypeId] [char](6) NOT NULL,
	[IsPrivate] [tinyint] NOT NULL,
	[IsOriginalThread] [tinyint] NOT NULL,
	[HasVideo] [tinyint] NOT NULL,
	[HasMusic] [tinyint] NOT NULL,
	[HasImage] [tinyint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_tn_Activities] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_ActivityItemKey')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_ActivityItemKey] ON [dbo].[tn_Activities] 
(
	[ActivityItemKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_ApplicationId')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_ApplicationId] ON [dbo].[tn_Activities] 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_LastModified')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_LastModified] ON [dbo].[tn_Activities] 
(
	[LastModified] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_OwnerId_OwnerType')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_OwnerId_OwnerType] ON [dbo].[tn_Activities] 
(
	[OwnerId] ASC,
	[OwnerType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_OwnerType')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_OwnerType] ON [dbo].[tn_Activities] 
(
	[OwnerType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_ReferenceId')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_ReferenceId] ON [dbo].[tn_Activities] 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_SourceId')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_SourceId] ON [dbo].[tn_Activities] 
(
	[SourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_TenantTypeId] ON [dbo].[tn_Activities] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_Activities]') AND name = N'IX_tn_Activities_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_Activities_UserId] ON [dbo].[tn_Activities] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'OwnerType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动态拥有者类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'OwnerType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'OwnerName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'OwnerName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'ActivityItemKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动态项目标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'ActivityItemKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'ApplicationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'ApplicationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'SourceId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动态源内容id（例如：日志动态的日志Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'SourceId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'ReferenceId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'涉及的Id（例如：评论动态的评论对象Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'ReferenceId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'ReferenceTenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'涉及对象的租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'ReferenceTenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'IsPrivate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否私有（仅允许自己查看）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'IsPrivate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'IsOriginalThread'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否原创主题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'IsOriginalThread'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'HasVideo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含视频' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'HasVideo'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'HasMusic'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含音乐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'HasMusic'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'HasImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'HasImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_Activities', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_Activities', @level2type=N'COLUMN',@level2name=N'LastModified'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AccountTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AccountTypes](
	[AccountTypeKey] [varchar](64) NOT NULL,
	[ThirdAccountGetterClassType] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_AccountTypes_ThirdAccountGetterClassType]  DEFAULT (''),
	[AppKey] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_AccountTypes_AppKey]  DEFAULT (''),
	[AppSecret] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_AccountTypes_AppSecret]  DEFAULT (''),
	[IsSync] [tinyint] NOT NULL CONSTRAINT [DF_tn_AccountTypes_IsShareMicroBlog1]  DEFAULT ((0)),
	[IsShareMicroBlog] [tinyint] NOT NULL CONSTRAINT [DF_tn_AccountTypes_IsShareMicroBlog]  DEFAULT ((0)),
	[IsFollowMicroBlog] [tinyint] NOT NULL CONSTRAINT [DF_tn_AccountTypes_IsFollowMicroBlog]  DEFAULT ((0)),
	[OfficialMicroBlogAccount] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_AccountTypes_OfficialMicroBlogAccount]  DEFAULT (''),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_tn_AccountTypes_IsEnabled]  DEFAULT ((0)),
 CONSTRAINT [PK_tn_AccountTypes] PRIMARY KEY CLUSTERED 
(
	[AccountTypeKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'AccountTypeKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方帐号类型标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'AccountTypeKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'ThirdAccountGetterClassType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方帐号获取器实现类Type值(如：Spacebuilder.Common.QQAccountGetter,Spacebuilder.QQAccountGetter)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'ThirdAccountGetterClassType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'AppKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站接入应用标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'AppKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'AppSecret'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'网站接入应用加密串' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'AppSecret'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'IsSync'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'绑定成功时是否分享一条微博' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'IsSync'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'IsShareMicroBlog'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'绑定成功时是否分享一条微博' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'IsShareMicroBlog'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'IsFollowMicroBlog'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否关注指定微博' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'IsFollowMicroBlog'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'OfficialMicroBlogAccount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'官方微博帐号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'OfficialMicroBlogAccount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountTypes', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountTypes', @level2type=N'COLUMN',@level2name=N'IsEnabled'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_AccountBindings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_AccountBindings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[AccountTypeKey] [varchar](64) NOT NULL,
	[Identification] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_AccountBindings_Identification]  DEFAULT (''),
	[AccessToken] [nvarchar](255) NOT NULL CONSTRAINT [DF_tn_AccountBindings_OauthTokenSecret]  DEFAULT (''),
 CONSTRAINT [PK_tn_AccountBindings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AccountBindings]') AND name = N'IX_tn_AccountBindings_AccountTypeKey')
CREATE NONCLUSTERED INDEX [IX_tn_AccountBindings_AccountTypeKey] ON [dbo].[tn_AccountBindings] 
(
	[AccountTypeKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AccountBindings]') AND name = N'IX_tn_AccountBindings_Identification')
CREATE NONCLUSTERED INDEX [IX_tn_AccountBindings_Identification] ON [dbo].[tn_AccountBindings] 
(
	[Identification] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[tn_AccountBindings]') AND name = N'IX_tn_AccountBindings_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_AccountBindings_UserId] ON [dbo].[tn_AccountBindings] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountBindings', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountBindings', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountBindings', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountBindings', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountBindings', N'COLUMN',N'AccountTypeKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方帐号类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountBindings', @level2type=N'COLUMN',@level2name=N'AccountTypeKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountBindings', N'COLUMN',N'Identification'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方帐号标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountBindings', @level2type=N'COLUMN',@level2name=N'Identification'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'tn_AccountBindings', N'COLUMN',N'AccessToken'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'oauth授权凭证加密串' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tn_AccountBindings', @level2type=N'COLUMN',@level2name=N'AccessToken'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_WorkExperiences]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_WorkExperiences](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[CompanyName] [nvarchar](64) NOT NULL,
	[CompanyAreaCode] [varchar](8) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[JobDescription] [nvarchar](128) NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_WorkExperiences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_WorkExperiences]') AND name = N'IX_spb_WorkExperiences_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_WorkExperiences_UserId] ON [dbo].[spb_WorkExperiences] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_WorkExperiences', N'COLUMN',N'CompanyName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_WorkExperiences', @level2type=N'COLUMN',@level2name=N'CompanyName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_WorkExperiences', N'COLUMN',N'CompanyAreaCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在地' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_WorkExperiences', @level2type=N'COLUMN',@level2name=N'CompanyAreaCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_WorkExperiences', N'COLUMN',N'StartDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_WorkExperiences', @level2type=N'COLUMN',@level2name=N'StartDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_WorkExperiences', N'COLUMN',N'EndDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'截止时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_WorkExperiences', @level2type=N'COLUMN',@level2name=N'EndDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_WorkExperiences', N'COLUMN',N'JobDescription'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门/职位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_WorkExperiences', @level2type=N'COLUMN',@level2name=N'JobDescription'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_WorkExperiences', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_WorkExperiences', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_WorkExperiences', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_WorkExperiences', @level2type=N'COLUMN',@level2name=N'PropertyValues'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Profiles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Profiles](
	[UserId] [bigint] NOT NULL,
	[Gender] [smallint] NOT NULL CONSTRAINT [DF_spb_Profiles_Gender]  DEFAULT ((0)),
	[BirthdayType] [smallint] NOT NULL CONSTRAINT [DF_spb_Profiles_BirthdayType]  DEFAULT ((1)),
	[Birthday] [datetime] NOT NULL,
	[LunarBirthday] [datetime] NOT NULL,
	[NowAreaCode] [varchar](8) NOT NULL,
	[HomeAreaCode] [varchar](8) NOT NULL,
	[Email] [nvarchar](64) NOT NULL,
	[Mobile] [nvarchar](64) NOT NULL,
	[QQ] [nvarchar](64) NOT NULL,
	[Msn] [nvarchar](64) NOT NULL,
	[Skype] [nvarchar](64) NOT NULL,
	[Fetion] [nvarchar](64) NOT NULL,
	[Aliwangwang] [nvarchar](64) NOT NULL,
	[CardType] [smallint] NOT NULL,
	[CardID] [nvarchar](64) NOT NULL,
	[Introduction] [nvarchar](255) NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
	[Integrity] [int] NOT NULL,
 CONSTRAINT [PK_spb_Profiles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Gender'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别1=男,2=女,0=未设置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Gender'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'BirthdayType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生日类型1=公历,2=阴历' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'BirthdayType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Birthday'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公历生日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Birthday'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'LunarBirthday'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'阴历生日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'LunarBirthday'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'NowAreaCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在地' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'NowAreaCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'HomeAreaCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'家乡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'HomeAreaCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Email'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Email'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Mobile'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Mobile'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'QQ'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'QQ'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Msn'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MSN' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Msn'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Skype'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Skype' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Skype'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Fetion'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飞信' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Fetion'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Aliwangwang'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'阿里旺旺' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Aliwangwang'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'CardType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'证件类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'CardType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'CardID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'证件号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'CardID'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Introduction'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自我介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Introduction'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'PropertyValues'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Profiles', N'COLUMN',N'Integrity'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资料完整度（0至100）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Profiles', @level2type=N'COLUMN',@level2name=N'Integrity'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Links]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Links](
	[LinkId] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnerType] [smallint] NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[LinkName] [nvarchar](128) NOT NULL,
	[LinkType] [tinyint] NOT NULL,
	[LinkUrl] [nvarchar](512) NOT NULL,
	[ImageUrl] [nvarchar](512) NOT NULL,
	[Description] [nvarchar](512) NOT NULL,
	[IsEnabled] [tinyint] NOT NULL,
	[DisplayOrder] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_Links] PRIMARY KEY CLUSTERED 
(
	[LinkId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'LinkId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'友情链接ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'LinkId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'OwnerType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'友情链接拥有者类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'OwnerType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接拥有者Id（如用户Id/群组Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'LinkName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'LinkName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'LinkType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接类型
（0-	文字链接）
（1-	图像链接）
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'LinkType'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'LinkUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'LinkUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'ImageUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Logo地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'ImageUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'IsEnabled'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序，默认与主键相同' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Links', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Links', @level2type=N'COLUMN',@level2name=N'LastModified'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_ImpeachReports]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_ImpeachReports](
	[ReportId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Reporter] [nchar](64) NOT NULL,
	[ReportedUserId] [bigint] NOT NULL,
	[Email] [nchar](64) NOT NULL,
	[Title] [nchar](255) NOT NULL,
	[Telephone] [nchar](64) NOT NULL,
	[Reason] [smallint] NOT NULL,
	[Description] [nchar](255) NOT NULL,
	[URL] [nchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[DisposerId] [bigint] NOT NULL,
 CONSTRAINT [PK_spb_ImpeachReports] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_ImpeachReports]') AND name = N'IX_spb_ImpeachReports_Reason')
CREATE NONCLUSTERED INDEX [IX_spb_ImpeachReports_Reason] ON [dbo].[spb_ImpeachReports] 
(
	[Reason] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_ImpeachReports]') AND name = N'IX_spb_ImpeachReports_Status')
CREATE NONCLUSTERED INDEX [IX_spb_ImpeachReports_Status] ON [dbo].[spb_ImpeachReports] 
(
	[Status] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_IdentificationTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_IdentificationTypes](
	[IdentificationTypeId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Enabled] [tinyint] NOT NULL,
	[CreaterId] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[IdentificationTypeLogo] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_spb_AuthenticationType] PRIMARY KEY CLUSTERED 
(
	[IdentificationTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_IdentificationTypes', N'COLUMN',N'IdentificationTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'认证标识Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_IdentificationTypes', @level2type=N'COLUMN',@level2name=N'IdentificationTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_IdentificationTypes', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_IdentificationTypes', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_IdentificationTypes', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_IdentificationTypes', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_IdentificationTypes', N'COLUMN',N'Enabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用(0=disabled,1=enabled)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_IdentificationTypes', @level2type=N'COLUMN',@level2name=N'Enabled'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_IdentificationTypes', N'COLUMN',N'CreaterId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_IdentificationTypes', @level2type=N'COLUMN',@level2name=N'CreaterId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_IdentificationTypes', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_IdentificationTypes', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_IdentificationTypes', N'COLUMN',N'IdentificationTypeLogo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'认证标识图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_IdentificationTypes', @level2type=N'COLUMN',@level2name=N'IdentificationTypeLogo'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Identifications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Identifications](
	[IdentificationId] [bigint] IDENTITY(1,1) NOT NULL,
	[IdentificationTypeId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[TrueName] [nvarchar](64) NOT NULL,
	[IdNumber] [nvarchar](32) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[Email] [nvarchar](64) NOT NULL,
	[Mobile] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DisposerId] [bigint] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[IdentificationLogo] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_spb_IdentityAuthentication] PRIMARY KEY CLUSTERED 
(
	[IdentificationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Identifications]') AND name = N'IX_spb_Identification_TrueName')
CREATE NONCLUSTERED INDEX [IX_spb_Identification_TrueName] ON [dbo].[spb_Identifications] 
(
	[IdentificationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'IdentificationId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'认证申请Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'IdentificationId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'IdentificationTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'认证标识Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'IdentificationTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'TrueName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人真实姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'TrueName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'IdNumber'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人身份证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'IdNumber'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'Status'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'认证状态(0=fail,1=success,2=pending)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'Status'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'Email'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人电子邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'Email'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'Mobile'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请人手机' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'Mobile'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'认证说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'DisposerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'DisposerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'处理时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'LastModified'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Identifications', N'COLUMN',N'IdentificationLogo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扫描证件图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Identifications', @level2type=N'COLUMN',@level2name=N'IdentificationLogo'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_EducationExperiences]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_EducationExperiences](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Degree] [smallint] NOT NULL,
	[School] [nvarchar](128) NOT NULL,
	[StartYear] [int] NOT NULL,
	[Department] [nvarchar](128) NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_EducationExperiences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_EducationExperiences]') AND name = N'IX_spb_EducationExperiences_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_EducationExperiences_UserId] ON [dbo].[spb_EducationExperiences] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_EducationExperiences', N'COLUMN',N'Degree'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'学历' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_EducationExperiences', @level2type=N'COLUMN',@level2name=N'Degree'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_EducationExperiences', N'COLUMN',N'School'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'学校名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_EducationExperiences', @level2type=N'COLUMN',@level2name=N'School'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_EducationExperiences', N'COLUMN',N'StartYear'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'入学年份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_EducationExperiences', @level2type=N'COLUMN',@level2name=N'StartYear'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_EducationExperiences', N'COLUMN',N'Department'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'院系/班级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_EducationExperiences', @level2type=N'COLUMN',@level2name=N'Department'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_EducationExperiences', N'COLUMN',N'PropertyNames'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_EducationExperiences', @level2type=N'COLUMN',@level2name=N'PropertyNames'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_EducationExperiences', N'COLUMN',N'PropertyValues'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可序列化属性内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_EducationExperiences', @level2type=N'COLUMN',@level2name=N'PropertyValues'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_CustomStyles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_CustomStyles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PresentAreaKey] [varchar](32) NOT NULL,
	[OwnerId] [bigint] NOT NULL,
	[SerializedCustomStyle] [nvarchar](max) NOT NULL,
	[BackgroundImage] [nvarchar](128) NOT NULL,
	[LastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_spb_CustomStyles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_CustomStyles]') AND name = N'IX_spb_CustomStyles_OwnerId')
CREATE NONCLUSTERED INDEX [IX_spb_CustomStyles_OwnerId] ON [dbo].[spb_CustomStyles] 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_CustomStyles', N'COLUMN',N'PresentAreaKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'呈现区域标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_CustomStyles', @level2type=N'COLUMN',@level2name=N'PresentAreaKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_CustomStyles', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_CustomStyles', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_CustomStyles', N'COLUMN',N'SerializedCustomStyle'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'定制样式序列化' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_CustomStyles', @level2type=N'COLUMN',@level2name=N'SerializedCustomStyle'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_CustomStyles', N'COLUMN',N'BackgroundImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'背景图片名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_CustomStyles', @level2type=N'COLUMN',@level2name=N'BackgroundImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_CustomStyles', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_CustomStyles', @level2type=N'COLUMN',@level2name=N'LastModified'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Announcements]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Announcements](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](255) NOT NULL CONSTRAINT [DF_spb_Announcements_Subject]  DEFAULT (''),
	[SubjectStyle] [nvarchar](512) NOT NULL,
	[Body] [nvarchar](max) NOT NULL CONSTRAINT [DF_spb_Announcements_Body]  DEFAULT (''),
	[IsHyperLink] [tinyint] NOT NULL CONSTRAINT [DF_spb_Announcements_IsHyperLink]  DEFAULT ((0)),
	[HyperLinkUrl] [nvarchar](512) NOT NULL CONSTRAINT [DF_spb_Announcements_HyperLinkUrl]  DEFAULT (''),
	[EnabledDescription] [tinyint] NOT NULL CONSTRAINT [DF_spb_Announcements_IsEnabled]  DEFAULT ((0)),
	[ReleaseDate] [datetime] NOT NULL,
	[ExpiredDate] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[CreatDate] [datetime] NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_spb_Announcements_UserId]  DEFAULT ((0)),
	[DisplayOrder] [bigint] NOT NULL CONSTRAINT [DF_spb_Announcements_DisplayOrder]  DEFAULT ((100)),
	[DisplayArea] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_Announcements_PresentAreaKey]  DEFAULT (''),
 CONSTRAINT [PK_spb_Announcements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Announcements]') AND name = N'IX_spb_Announcements_DisplayArea')
CREATE NONCLUSTERED INDEX [IX_spb_Announcements_DisplayArea] ON [dbo].[spb_Announcements] 
(
	[DisplayArea] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Announcements]') AND name = N'IX_spb_Announcements_DisplayOrder')
CREATE NONCLUSTERED INDEX [IX_spb_Announcements_DisplayOrder] ON [dbo].[spb_Announcements] 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'Id'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'Id'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'Subject'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公告主题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'Subject'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'SubjectStyle'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主题字体风格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'SubjectStyle'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公告内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'IsHyperLink'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是连接' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'IsHyperLink'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'HyperLinkUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'HyperLinkUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'EnabledDescription'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'EnabledDescription'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'ReleaseDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'ReleaseDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'ExpiredDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'过期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'ExpiredDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'LastModified'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'CreatDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'CreatDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示顺序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Announcements', N'COLUMN',N'DisplayArea'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'展示区域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Announcements', @level2type=N'COLUMN',@level2name=N'DisplayArea'


/****** Object:  Table [dbo].[tn_SmtpSettings]    Script Date: 04/16/2013 16:37:12 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tn_SmtpSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tn_SmtpSettings](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[Port] [int] NOT NULL,
	[EnableSsl] [tinyint] NOT NULL,
	[RequireCredentials] [tinyint] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserEmailAddress] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[ForceSmtpUserAsFromAddress] [tinyint] NOT NULL,
	[DailyLimit] [int] NOT NULL,
 CONSTRAINT [PK_tn_SmtpSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

--贴吧
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarPosts_SectionId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarPosts] DROP CONSTRAINT [DF_spb_BarPosts_SectionId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarPosts_OwnerId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarPosts] DROP CONSTRAINT [DF_spb_BarPosts_OwnerId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarPosts_ParentId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarPosts] DROP CONSTRAINT [DF_spb_BarPosts_ParentId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarPosts_AuditStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarPosts] DROP CONSTRAINT [DF_spb_BarPosts_AuditStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarPosts_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarPosts] DROP CONSTRAINT [DF_spb_BarPosts_IP]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarPosts_ChildPostCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarPosts] DROP CONSTRAINT [DF_spb_BarPosts_ChildPostCount]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND type in (N'U'))
DROP TABLE [dbo].[spb_BarPosts]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarRatings_TradePoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarRatings] DROP CONSTRAINT [DF_spb_BarRatings_TradePoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarRatings_ReputationPoints]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarRatings] DROP CONSTRAINT [DF_spb_BarRatings_ReputationPoints]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarRatings_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarRatings] DROP CONSTRAINT [DF_spb_BarRatings_IP]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarRatings]') AND type in (N'U'))
DROP TABLE [dbo].[spb_BarRatings]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tn_BarSectionManagers_SectionId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSectionManagers] DROP CONSTRAINT [DF_tn_BarSectionManagers_SectionId]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSectionManagers]') AND type in (N'U'))
DROP TABLE [dbo].[spb_BarSectionManagers]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarSections_OwnerId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSections] DROP CONSTRAINT [DF_spb_BarSections_OwnerId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarSections_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSections] DROP CONSTRAINT [DF_spb_BarSections_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarSections_LogoImage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSections] DROP CONSTRAINT [DF_spb_BarSections_LogoImage]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarSections_IsEnabled]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSections] DROP CONSTRAINT [DF_spb_BarSections_IsEnabled]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarSections_EnableRss]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSections] DROP CONSTRAINT [DF_spb_BarSections_EnableRss]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarSections_ThreadCategoryStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSections] DROP CONSTRAINT [DF_spb_BarSections_ThreadCategoryStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarSections_AuditStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarSections] DROP CONSTRAINT [DF_spb_BarSections_AuditStatus]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSections]') AND type in (N'U'))
DROP TABLE [dbo].[spb_BarSections]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_SectionId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_SectionId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_OwnerId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_OwnerId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_IsLocked]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_IsEssential]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_IsEssential]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_IsSticky]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_IsSticky]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_IsHidden]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_IsHidden]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_AuditStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_AuditStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_PostCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_PostCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BarThreads_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BarThreads] DROP CONSTRAINT [DF_spb_BarThreads_IP]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarThreads]') AND type in (N'U'))
DROP TABLE [dbo].[spb_BarThreads]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarThreads]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_BarThreads](
	[ThreadId] [bigint] IDENTITY(1,1) NOT NULL,
	[SectionId] [bigint] NOT NULL CONSTRAINT [DF_spb_BarThreads_SectionId]  DEFAULT ((0)),
	[TenantTypeId] [char](6) NOT NULL,
	[OwnerId] [bigint] NOT NULL CONSTRAINT [DF_spb_BarThreads_OwnerId]  DEFAULT ((0)),
	[UserId] [bigint] NOT NULL,
	[Author] [nvarchar](64) NOT NULL,
	[Subject] [nvarchar](128) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[IsLocked] [tinyint] NOT NULL CONSTRAINT [DF_spb_BarThreads_IsLocked]  DEFAULT ((0)),
	[IsEssential] [tinyint] NOT NULL CONSTRAINT [DF_spb_BarThreads_IsEssential]  DEFAULT ((0)),
	[IsSticky] [tinyint] NOT NULL CONSTRAINT [DF_spb_BarThreads_IsSticky]  DEFAULT ((0)),
	[StickyDate] [datetime] NOT NULL,
	[IsHidden] [tinyint] NOT NULL CONSTRAINT [DF_spb_BarThreads_IsHidden]  DEFAULT ((0)),
	[HighlightStyle] [nvarchar](512) NOT NULL,
	[HighlightDate] [datetime] NOT NULL,
	[Price] [int] NOT NULL,
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_spb_BarThreads_AuditStatus]  DEFAULT ((40)),
	[PostCount] [int] NOT NULL CONSTRAINT [DF_spb_BarThreads_PostCount]  DEFAULT ((0)),
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_BarThreads_IP]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_BarThreads] PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarThreads]') AND name = N'IX_spb_BarThreads_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_spb_BarThreads_AuditStatus] ON [dbo].[spb_BarThreads] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarThreads]') AND name = N'IX_spb_BarThreads_OwnerId')
CREATE NONCLUSTERED INDEX [IX_spb_BarThreads_OwnerId] ON [dbo].[spb_BarThreads] 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarThreads]') AND name = N'IX_spb_BarThreads_SectionId')
CREATE NONCLUSTERED INDEX [IX_spb_BarThreads_SectionId] ON [dbo].[spb_BarThreads] 
(
	[SectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarThreads]') AND name = N'IX_spb_BarThreads_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_spb_BarThreads_TenantTypeId] ON [dbo].[spb_BarThreads] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarThreads]') AND name = N'IX_spb_BarThreads_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_BarThreads_UserId] ON [dbo].[spb_BarThreads] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'SectionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖吧Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'SectionId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖吧租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖吧拥有者Id（例如：群组Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主题作者用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'Author'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主题作者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'Author'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'Subject'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖子标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'Subject'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖子内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'IsLocked'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'IsLocked'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'IsEssential'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否精华' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'IsEssential'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'IsSticky'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否置顶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'IsSticky'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'StickyDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'置顶期限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'StickyDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'IsHidden'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否仅回复可见' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'IsHidden'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'HighlightStyle'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'高亮显示的样式代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'HighlightStyle'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'HighlightDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'高亮显示期限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'HighlightDate'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'Price'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'售价（交易积分）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'Price'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'PostCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'PostCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发帖人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarThreads', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新日期（被回复时也需要更新时间）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarThreads', @level2type=N'COLUMN',@level2name=N'LastModified'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSections]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_BarSections](
	[SectionId] [bigint] NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[OwnerId] [bigint] NOT NULL CONSTRAINT [DF_spb_BarSections_OwnerId]  DEFAULT ((0)),
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_spb_BarSections_UserId]  DEFAULT ((0)),
	[Name] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[LogoImage] [nvarchar](255) NOT NULL CONSTRAINT [DF_spb_BarSections_LogoImage]  DEFAULT (''),
	[IsEnabled] [tinyint] NOT NULL CONSTRAINT [DF_spb_BarSections_IsEnabled]  DEFAULT ((1)),
	[EnableRss] [tinyint] NOT NULL CONSTRAINT [DF_spb_BarSections_EnableRss]  DEFAULT ((1)),
	[ThreadCategoryStatus] [smallint] NOT NULL CONSTRAINT [DF_spb_BarSections_ThreadCategoryStatus]  DEFAULT ((1)),
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_spb_BarSections_AuditStatus]  DEFAULT ((40)),
	[DisplayOrder] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
 CONSTRAINT [PK_spb_BarSections] PRIMARY KEY CLUSTERED 
(
	[SectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSections]') AND name = N'IX_spb_BarSections_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_spb_BarSections_AuditStatus] ON [dbo].[spb_BarSections] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSections]') AND name = N'IX_spb_BarSections_DisplayOrder')
CREATE NONCLUSTERED INDEX [IX_spb_BarSections_DisplayOrder] ON [dbo].[spb_BarSections] 
(
	[DisplayOrder] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSections]') AND name = N'IX_spb_BarSections_OwnerId')
CREATE NONCLUSTERED INDEX [IX_spb_BarSections_OwnerId] ON [dbo].[spb_BarSections] 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSections]') AND name = N'IX_spb_BarSections_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_spb_BarSections_TenantTypeId] ON [dbo].[spb_BarSections] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSections]') AND name = N'IX_spb_BarSections_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_BarSections_UserId] ON [dbo].[spb_BarSections] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖吧租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖吧拥有者Id（例如：活动Id、群组Id），若是帖吧应用，则OwnerId为0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'吧主用户Id（若是活动/群组，则对应活动/群组创建者Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖吧名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'Name'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖吧描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'LogoImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Logo存储图片名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'LogoImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'IsEnabled'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'IsEnabled'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'EnableRss'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用RSS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'EnableRss'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'ThreadCategoryStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主题分类状态 0=禁用；1=启用（不强制）；2=启用（强制）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'ThreadCategoryStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'DisplayOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'DisplayOrder'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSections', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSections', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSectionManagers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_BarSectionManagers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SectionId] [bigint] NOT NULL CONSTRAINT [DF_tn_BarSectionManagers_SectionId]  DEFAULT ((0)),
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_tn_BarSectionManagers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSectionManagers]') AND name = N'IX_tn_BarSectionManagers_SectionId')
CREATE NONCLUSTERED INDEX [IX_tn_BarSectionManagers_SectionId] ON [dbo].[spb_BarSectionManagers] 
(
	[SectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarSectionManagers]') AND name = N'IX_tn_BarSectionManagers_UserId')
CREATE NONCLUSTERED INDEX [IX_tn_BarSectionManagers_UserId] ON [dbo].[spb_BarSectionManagers] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSectionManagers', N'COLUMN',N'SectionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖吧Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSectionManagers', @level2type=N'COLUMN',@level2name=N'SectionId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarSectionManagers', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarSectionManagers', @level2type=N'COLUMN',@level2name=N'UserId'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarRatings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_BarRatings](
	[RatingId] [bigint] IDENTITY(1,1) NOT NULL,
	[ThreadId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UserDisplayName] [nvarchar](64) NOT NULL,
	[TradePoints] [int] NOT NULL CONSTRAINT [DF_spb_BarRatings_TradePoints]  DEFAULT ((0)),
	[ReputationPoints] [int] NOT NULL CONSTRAINT [DF_spb_BarRatings_ReputationPoints]  DEFAULT ((0)),
	[Reason] [nvarchar](255) NOT NULL,
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_BarRatings_IP]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_spb_BarRatings] PRIMARY KEY CLUSTERED 
(
	[RatingId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarRatings]') AND name = N'IX_spb_BarRatings_ThreadId')
CREATE NONCLUSTERED INDEX [IX_spb_BarRatings_ThreadId] ON [dbo].[spb_BarRatings] 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarRatings]') AND name = N'IX_spb_BarRatings_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_BarRatings_UserId] ON [dbo].[spb_BarRatings] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'ThreadId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖子Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'ThreadId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评分用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'UserDisplayName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评分用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'UserDisplayName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'TradePoints'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评的金币值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'TradePoints'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'ReputationPoints'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评的威望值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'ReputationPoints'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'Reason'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'理由' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'Reason'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发帖人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarRatings', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarRatings', @level2type=N'COLUMN',@level2name=N'DateCreated'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_BarPosts](
	[PostId] [bigint] IDENTITY(1,1) NOT NULL,
	[SectionId] [bigint] NOT NULL CONSTRAINT [DF_spb_BarPosts_SectionId]  DEFAULT ((0)),
	[TenantTypeId] [char](6) NOT NULL,
	[OwnerId] [bigint] NOT NULL CONSTRAINT [DF_spb_BarPosts_OwnerId]  DEFAULT ((0)),
	[ThreadId] [bigint] NOT NULL,
	[ParentId] [bigint] NOT NULL CONSTRAINT [DF_spb_BarPosts_ParentId]  DEFAULT ((0)),
	[UserId] [bigint] NOT NULL,
	[Author] [nvarchar](64) NOT NULL,
	[Subject] [nvarchar](128) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_spb_BarPosts_AuditStatus]  DEFAULT ((40)),
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_BarPosts_IP]  DEFAULT (''),
	[ChildPostCount] [int] NOT NULL CONSTRAINT [DF_spb_BarPosts_ChildPostCount]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_spb_BarPosts] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND name = N'IX_spb_BarPosts_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_spb_BarPosts_AuditStatus] ON [dbo].[spb_BarPosts] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND name = N'IX_spb_BarPosts_OwnerId')
CREATE NONCLUSTERED INDEX [IX_spb_BarPosts_OwnerId] ON [dbo].[spb_BarPosts] 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND name = N'IX_spb_BarPosts_ParentId')
CREATE NONCLUSTERED INDEX [IX_spb_BarPosts_ParentId] ON [dbo].[spb_BarPosts] 
(
	[ParentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND name = N'IX_spb_BarPosts_SectionId')
CREATE NONCLUSTERED INDEX [IX_spb_BarPosts_SectionId] ON [dbo].[spb_BarPosts] 
(
	[SectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND name = N'IX_spb_BarPosts_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_spb_BarPosts_TenantTypeId] ON [dbo].[spb_BarPosts] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND name = N'IX_spb_BarPosts_ThreadId')
CREATE NONCLUSTERED INDEX [IX_spb_BarPosts_ThreadId] ON [dbo].[spb_BarPosts] 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BarPosts]') AND name = N'IX_spb_BarPosts_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_BarPosts_UserId] ON [dbo].[spb_BarPosts] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'SectionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖吧Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'SectionId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖吧租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖吧拥有者Id（例如：群组Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'ThreadId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属帖子Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'ThreadId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'ParentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父回帖Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'ParentId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主题作者用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'Author'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主题作者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'Author'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'Subject'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖子标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'Subject'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帖子内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发帖人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'ChildPostCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子回复数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'ChildPostCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BarPosts', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新日期（被回复时也需要更新时间）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BarPosts', @level2type=N'COLUMN',@level2name=N'LastModified'

--日志
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_OwnerId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_OwnerId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_Summary]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_Summary]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_IsDraft]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_IsDraft]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_IsLocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_IsLocked]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_IsEssential]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_IsEssential]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_IsSticky]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_IsSticky]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_AuditStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_AuditStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_IsReproduced]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_IsReproduced]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_IP]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__spb_BlogT__Keywo__2136E270]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF__spb_BlogT__Keywo__2136E270]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_FeaturedImageAttachmentId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_FeaturedImageAttachmentId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_BlogThreads_FeaturedImage]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_BlogThreads] DROP CONSTRAINT [DF_spb_BlogThreads_FeaturedImage]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND type in (N'U'))
DROP TABLE [dbo].[spb_BlogThreads]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_BlogThreads](
	[ThreadId] [bigint] IDENTITY(1,1) NOT NULL,
	[TenantTypeId] [char](6) NOT NULL,
	[OwnerId] [bigint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_OwnerId]  DEFAULT ((0)),
	[UserId] [bigint] NOT NULL,
	[Author] [nvarchar](64) NOT NULL,
	[Subject] [nvarchar](128) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[Summary] [nvarchar](255) NOT NULL CONSTRAINT [DF_spb_BlogThreads_Summary]  DEFAULT (''),
	[IsDraft] [tinyint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_IsDraft]  DEFAULT ((0)),
	[IsLocked] [tinyint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_IsLocked]  DEFAULT ((0)),
	[IsEssential] [tinyint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_IsEssential]  DEFAULT ((0)),
	[IsSticky] [tinyint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_IsSticky]  DEFAULT ((0)),
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_AuditStatus]  DEFAULT ((40)),
	[PrivacyStatus] [smallint] NOT NULL,
	[IsReproduced] [tinyint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_IsReproduced]  DEFAULT ((0)),
	[OriginalAuthorId] [bigint] NOT NULL,
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_BlogThreads_IP]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
	[Keywords] [nvarchar](128) NOT NULL CONSTRAINT [DF__spb_BlogT__Keywo__2136E270]  DEFAULT (''),
	[FeaturedImageAttachmentId] [bigint] NOT NULL CONSTRAINT [DF_spb_BlogThreads_FeaturedImageAttachmentId]  DEFAULT ((0)),
	[FeaturedImage] [nvarchar](255) NOT NULL CONSTRAINT [DF_spb_BlogThreads_FeaturedImage]  DEFAULT (''),
	[LastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_spb_BlogThreads] PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND name = N'IX_spb_BlogThreads_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_spb_BlogThreads_AuditStatus] ON [dbo].[spb_BlogThreads] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND name = N'IX_spb_BlogThreads_IsEssential')
CREATE NONCLUSTERED INDEX [IX_spb_BlogThreads_IsEssential] ON [dbo].[spb_BlogThreads] 
(
	[IsEssential] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND name = N'IX_spb_BlogThreads_OwnerId_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_spb_BlogThreads_OwnerId_TenantTypeId] ON [dbo].[spb_BlogThreads] 
(
	[OwnerId] ASC,
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND name = N'IX_spb_BlogThreads_PrivacyStatus')
CREATE NONCLUSTERED INDEX [IX_spb_BlogThreads_PrivacyStatus] ON [dbo].[spb_BlogThreads] 
(
	[PrivacyStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND name = N'IX_spb_BlogThreads_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_spb_BlogThreads_TenantTypeId] ON [dbo].[spb_BlogThreads] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_BlogThreads]') AND name = N'IX_spb_BlogThreads_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_BlogThreads_UserId] ON [dbo].[spb_BlogThreads] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拥有者Id（例如：用户Id、群组Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志作者UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'Author'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'Author'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'Subject'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'Subject'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'Summary'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'摘要' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'Summary'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'IsDraft'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否草稿' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'IsDraft'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'IsLocked'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定（锁定的日志不允许评论）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'IsLocked'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'IsEssential'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否精华' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'IsEssential'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'IsSticky'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否置顶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'IsSticky'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'PrivacyStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隐私状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'PrivacyStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'IsReproduced'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否转载' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'IsReproduced'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'OriginalAuthorId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被转载用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'OriginalAuthorId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布人IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'FeaturedImageAttachmentId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题图对应的附件Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'FeaturedImageAttachmentId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'FeaturedImage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题图文件（带部分路径）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'FeaturedImage'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_BlogThreads', N'COLUMN',N'LastModified'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_BlogThreads', @level2type=N'COLUMN',@level2name=N'LastModified'



--群组
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_GroupMemberApplies_ApplyReason]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_GroupMemberApplies] DROP CONSTRAINT [DF_spb_GroupMemberApplies_ApplyReason]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMemberApplies]') AND type in (N'U'))
DROP TABLE [dbo].[spb_GroupMemberApplies]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_GroupMembers_IsManager]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_GroupMembers] DROP CONSTRAINT [DF_spb_GroupMembers_IsManager]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMembers]') AND type in (N'U'))
DROP TABLE [dbo].[spb_GroupMembers]
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_GroupName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_GroupName]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_GroupKey]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_GroupKey]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_Description]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_Description]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_Logo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_Logo]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_IsPublic]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_IsPublic]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_EnableMemberInvite]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_EnableMemberInvite]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_AuditStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_AuditStatus]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_MemberCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_MemberCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_GrowthValue]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_GrowthValue]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_ThemeAppearance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_ThemeAppearance]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_IP]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_Announcement]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_Announcement]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Groups_IsUseCustomStyle]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Groups] DROP CONSTRAINT [DF_spb_Groups_IsUseCustomStyle]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Groups]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Groups]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Groups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Groups](
	[GroupId] [bigint] NOT NULL,
	[GroupName] [nvarchar](255) NOT NULL CONSTRAINT [DF_spb_Groups_GroupName]  DEFAULT (''),
	[GroupKey] [nvarchar](16) NOT NULL CONSTRAINT [DF_spb_Groups_GroupKey]  DEFAULT (''),
	[Description] [nvarchar](512) NOT NULL CONSTRAINT [DF_spb_Groups_Description]  DEFAULT (''),
	[AreaCode] [varchar](8) NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_spb_Groups_UserId]  DEFAULT ((0)),
	[Logo] [nvarchar](128) NOT NULL CONSTRAINT [DF_spb_Groups_Logo]  DEFAULT (''),
	[IsPublic] [tinyint] NOT NULL CONSTRAINT [DF_spb_Groups_IsPublic]  DEFAULT ((1)),
	[JoinWay] [smallint] NOT NULL,
	[EnableMemberInvite] [tinyint] NOT NULL CONSTRAINT [DF_spb_Groups_EnableMemberInvite]  DEFAULT ((1)),
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_spb_Groups_AuditStatus]  DEFAULT ((40)),
	[MemberCount] [int] NOT NULL CONSTRAINT [DF_spb_Groups_MemberCount]  DEFAULT ((0)),
	[GrowthValue] [int] NOT NULL CONSTRAINT [DF_spb_Groups_GrowthValue]  DEFAULT ((0)),
	[ThemeAppearance] [nvarchar](128) NOT NULL CONSTRAINT [DF_spb_Groups_ThemeAppearance]  DEFAULT (''),
	[DateCreated] [datetime] NOT NULL,
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_Groups_IP]  DEFAULT (''),
	[Announcement] [nvarchar](512) NOT NULL CONSTRAINT [DF_spb_Groups_Announcement]  DEFAULT (''),
	[PropertyNames] [nvarchar](max) NULL,
	[PropertyValues] [nvarchar](max) NULL,
	[IsUseCustomStyle] [tinyint] NOT NULL CONSTRAINT [DF_spb_Groups_IsUseCustomStyle]  DEFAULT ((0)),
 CONSTRAINT [PK_spb_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Groups]') AND name = N'IX_spb_Groups_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_spb_Groups_AuditStatus] ON [dbo].[spb_Groups] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Groups]') AND name = N'IX_spb_Groups_GrowthValue')
CREATE NONCLUSTERED INDEX [IX_spb_Groups_GrowthValue] ON [dbo].[spb_Groups] 
(
	[GrowthValue] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Groups]') AND name = N'IX_spb_Groups_MemberCount')
CREATE NONCLUSTERED INDEX [IX_spb_Groups_MemberCount] ON [dbo].[spb_Groups] 
(
	[MemberCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Groups]') AND name = N'IX_spb_Groups_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_Groups_UserId] ON [dbo].[spb_Groups] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'GroupName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群组名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'GroupName'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'GroupKey'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群组标识（个性网址的关键组成部分）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'GroupKey'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群组介绍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'Description'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'AreaCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在地区' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'AreaCode'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群主' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'Logo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'logo名称（带部分路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'Logo'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'IsPublic'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否公开' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'IsPublic'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'JoinWay'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加入方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'JoinWay'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'EnableMemberInvite'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否允许成员邀请（一直允许群管理员邀请）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'EnableMemberInvite'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'MemberCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成员数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'MemberCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'GrowthValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成长值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'GrowthValue'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'ThemeAppearance'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设置的皮肤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'ThemeAppearance'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'DateCreated'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'Announcement'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公告' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'Announcement'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Groups', N'COLUMN',N'IsUseCustomStyle'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否使用了自定义风格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Groups', @level2type=N'COLUMN',@level2name=N'IsUseCustomStyle'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMembers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_GroupMembers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[IsManager] [tinyint] NOT NULL CONSTRAINT [DF_spb_GroupMembers_IsManager]  DEFAULT ((0)),
	[JoinDate] [datetime] NOT NULL,
 CONSTRAINT [PK_spb_GroupMembers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMembers]') AND name = N'IK_spb_GroupMembers_GroupId')
CREATE NONCLUSTERED INDEX [IK_spb_GroupMembers_GroupId] ON [dbo].[spb_GroupMembers] 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMembers]') AND name = N'IK_spb_GroupMembers_UserId')
CREATE NONCLUSTERED INDEX [IK_spb_GroupMembers_UserId] ON [dbo].[spb_GroupMembers] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMembers', N'COLUMN',N'GroupId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMembers', @level2type=N'COLUMN',@level2name=N'GroupId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMembers', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMembers', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMembers', N'COLUMN',N'IsManager'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否群管理员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMembers', @level2type=N'COLUMN',@level2name=N'IsManager'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMembers', N'COLUMN',N'JoinDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加入日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMembers', @level2type=N'COLUMN',@level2name=N'JoinDate'
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMemberApplies]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_GroupMemberApplies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ApplyReason] [nvarchar](255) NOT NULL CONSTRAINT [DF_spb_GroupMemberApplies_ApplyReason]  DEFAULT (''),
	[ApplyStatus] [smallint] NOT NULL,
	[ApplyDate] [datetime] NOT NULL,
 CONSTRAINT [PK_spb_GroupMemberApplies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMemberApplies]') AND name = N'IK_spb_GroupMemberApplies_ApplyStatus')
CREATE NONCLUSTERED INDEX [IK_spb_GroupMemberApplies_ApplyStatus] ON [dbo].[spb_GroupMemberApplies] 
(
	[ApplyStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMemberApplies]') AND name = N'IK_spb_GroupMemberApplies_GroupId_ApplyStatus')
CREATE NONCLUSTERED INDEX [IK_spb_GroupMemberApplies_GroupId_ApplyStatus] ON [dbo].[spb_GroupMemberApplies] 
(
	[GroupId] ASC,
	[ApplyStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_GroupMemberApplies]') AND name = N'IK_spb_GroupMemberApplies_UserId')
CREATE NONCLUSTERED INDEX [IK_spb_GroupMemberApplies_UserId] ON [dbo].[spb_GroupMemberApplies] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMemberApplies', N'COLUMN',N'GroupId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'群组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMemberApplies', @level2type=N'COLUMN',@level2name=N'GroupId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMemberApplies', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMemberApplies', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMemberApplies', N'COLUMN',N'ApplyReason'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请理由' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMemberApplies', @level2type=N'COLUMN',@level2name=N'ApplyReason'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMemberApplies', N'COLUMN',N'ApplyStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMemberApplies', @level2type=N'COLUMN',@level2name=N'ApplyStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_GroupMemberApplies', N'COLUMN',N'ApplyDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_GroupMemberApplies', @level2type=N'COLUMN',@level2name=N'ApplyDate'

--微博
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_UserId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_UserId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_Author]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_Author]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_TenantTypeId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_TenantTypeId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_OwnerId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_OwnerId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_OriginalMicroblogId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_OriginalMicroblogId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_ForwardedMicroblogId]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_ForwardedMicroblogId]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_ReplyCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_ReplyCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_ForwardedCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_ForwardedCount]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_HasPhoto]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_HasPhoto]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_HasVideo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_HasVideo]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_HasMusic]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_HasMusic]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_PostWay]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_PostWay]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_IP]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_IP]
END
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_spb_Microblogs_AuditStatus]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[spb_Microblogs] DROP CONSTRAINT [DF_spb_Microblogs_AuditStatus]
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND type in (N'U'))
DROP TABLE [dbo].[spb_Microblogs]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[spb_Microblogs](
	[MicroblogId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL CONSTRAINT [DF_spb_Microblogs_UserId]  DEFAULT ((0)),
	[Author] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_Microblogs_Author]  DEFAULT (''),
	[TenantTypeId] [char](6) NOT NULL CONSTRAINT [DF_spb_Microblogs_TenantTypeId]  DEFAULT ((101)),
	[OwnerId] [bigint] NOT NULL CONSTRAINT [DF_spb_Microblogs_OwnerId]  DEFAULT ((0)),
	[OriginalMicroblogId] [bigint] NOT NULL CONSTRAINT [DF_spb_Microblogs_OriginalMicroblogId]  DEFAULT ((0)),
	[ForwardedMicroblogId] [bigint] NOT NULL CONSTRAINT [DF_spb_Microblogs_ForwardedMicroblogId]  DEFAULT ((0)),
	[Body] [nvarchar](1500) NOT NULL,
	[ReplyCount] [int] NOT NULL CONSTRAINT [DF_spb_Microblogs_ReplyCount]  DEFAULT ((0)),
	[ForwardedCount] [int] NOT NULL CONSTRAINT [DF_spb_Microblogs_ForwardedCount]  DEFAULT ((0)),
	[HasPhoto] [tinyint] NOT NULL CONSTRAINT [DF_spb_Microblogs_HasPhoto]  DEFAULT ((0)),
	[HasVideo] [tinyint] NOT NULL CONSTRAINT [DF_spb_Microblogs_HasVideo]  DEFAULT ((0)),
	[HasMusic] [tinyint] NOT NULL CONSTRAINT [DF_spb_Microblogs_HasMusic]  DEFAULT ((0)),
	[PostWay] [smallint] NULL CONSTRAINT [DF_spb_Microblogs_PostWay]  DEFAULT ((0)),
	[Source] [nvarchar](64) NULL,
	[SourceUrl] [nvarchar](128) NULL,
	[IP] [nvarchar](64) NOT NULL CONSTRAINT [DF_spb_Microblogs_IP]  DEFAULT (''),
	[AuditStatus] [smallint] NOT NULL CONSTRAINT [DF_spb_Microblogs_AuditStatus]  DEFAULT ((40)),
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_spb_Microblogs] PRIMARY KEY CLUSTERED 
(
	[MicroblogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND name = N'IX_spb_Microblogs_AuditStatus')
CREATE NONCLUSTERED INDEX [IX_spb_Microblogs_AuditStatus] ON [dbo].[spb_Microblogs] 
(
	[AuditStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND name = N'IX_spb_Microblogs_ForwardedCount')
CREATE NONCLUSTERED INDEX [IX_spb_Microblogs_ForwardedCount] ON [dbo].[spb_Microblogs] 
(
	[ForwardedCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND name = N'IX_spb_Microblogs_OriginalMicroblogId')
CREATE NONCLUSTERED INDEX [IX_spb_Microblogs_OriginalMicroblogId] ON [dbo].[spb_Microblogs] 
(
	[OriginalMicroblogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND name = N'IX_spb_Microblogs_OwnerId')
CREATE NONCLUSTERED INDEX [IX_spb_Microblogs_OwnerId] ON [dbo].[spb_Microblogs] 
(
	[OwnerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND name = N'IX_spb_Microblogs_ReplyCount')
CREATE NONCLUSTERED INDEX [IX_spb_Microblogs_ReplyCount] ON [dbo].[spb_Microblogs] 
(
	[ReplyCount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND name = N'IX_spb_Microblogs_TenantTypeId')
CREATE NONCLUSTERED INDEX [IX_spb_Microblogs_TenantTypeId] ON [dbo].[spb_Microblogs] 
(
	[TenantTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[spb_Microblogs]') AND name = N'IX_spb_Microblogs_UserId')
CREATE NONCLUSTERED INDEX [IX_spb_Microblogs_UserId] ON [dbo].[spb_Microblogs] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'MicroblogId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标识列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'MicroblogId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'UserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博作者UserId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'UserId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'Author'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博作者DisplayName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'Author'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'TenantTypeId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'租户类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'TenantTypeId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'OwnerId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博拥有者Id（例如：群组Id）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'OwnerId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'OriginalMicroblogId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原文微博Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'OriginalMicroblogId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'ForwardedMicroblogId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'转发微博Id（在转发非原文微博时使用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'ForwardedMicroblogId'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'Body'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'Body'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'ReplyCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回复数统计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'ReplyCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'ForwardedCount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'被转发数统计' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'ForwardedCount'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'HasPhoto'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'HasPhoto'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'HasVideo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含视频' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'HasVideo'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'HasMusic'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否包含音乐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'HasMusic'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'PostWay'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'PostWay'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'Source'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'Source'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'SourceUrl'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博来源的访问地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'SourceUrl'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'IP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'IP'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'AuditStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审核状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'AuditStatus'
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'spb_Microblogs', N'COLUMN',N'DateCreated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'spb_Microblogs', @level2type=N'COLUMN',@level2name=N'DateCreated'
