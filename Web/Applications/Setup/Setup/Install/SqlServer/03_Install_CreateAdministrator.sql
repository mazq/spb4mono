DELETE FROM [tn_Users]
DELETE FROM [spb_Profiles]
DELETE FROM [tn_ApplicationInPresentAreaInstallations] WHERE OwnerId = 207677921948
DELETE FROM [tn_PresentAreaNavigations]
DELETE FROM [tn_UsersInRoles]
DELETE FROM [tn_SystemData]

INSERT [dbo].[tn_Users]([UserId],[UserName],[Password],[PasswordFormat],[AccountEmail],[IsEmailVerified],[IsMobileVerified],[NickName],[ForceLogin],[IsActivated],[UserType],[IsBanned],[IsModerated],[IsForceModerated],[ThemeAppearance],[Avatar],[DateCreated],[LastActivityTime],[BanReason],[BanDeadline])VALUES (207677921948,N'admin',N'7fef6171469e80d32c0559f88b377245',1,N'admin@admin.com',0,0,N'站点管理员',0,1,0,0,0,0,N'Default,Default',N'avatar_default',getdate(),getdate(),N'',getdate())
INSERT [dbo].[spb_Profiles] ([UserId],[Integrity],[Birthday],[LunarBirthday],[NowAreaCode],[HomeAreaCode],[Email],[Mobile],[QQ],[Msn],[Skype],[Fetion],[Aliwangwang],[CardType],[CardID],[Introduction]) VALUES (207677921948,0,getdate(),getdate(),N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'')
INSERT INTO [dbo].[tn_UsersInRoles] ([UserId],[RoleName]) VALUES(207677921948, 'SuperAdministrator')
INSERT [dbo].[tn_ApplicationInPresentAreaInstallations] (OwnerId,ApplicationId,PresentAreaKey)
SELECT 207677921948,ApplicationId,PresentAreaKey from [dbo].[tn_ApplicationInPresentAreaSettings] where PresentAreaKey = 'UserSpace' and IsAutoInstall = 1

INSERT [dbo].[tn_PresentAreaNavigations]([NavigationId]
								  ,[ParentNavigationId]
								  ,[Depth]
								  ,[PresentAreaKey]
								  ,[ApplicationId]
								  ,[OwnerId]
								  ,[NavigationType]
								  ,[NavigationText]
								  ,[ResourceName]
								  ,[NavigationUrl]
								  ,[UrlRouteName]
								  ,[RouteDataName]
								  ,[IconName]
								  ,[ImageUrl]
								  ,[NavigationTarget]
								  ,[DisplayOrder]
								  ,[OnlyOwnerVisible]
								  ,[IsLocked]
								  ,[IsEnabled])
SELECT [NavigationId]
		,[ParentNavigationId]
		,[Depth]
		,[PresentAreaKey]
		,[ApplicationId]
		,207677921948
		,[NavigationType]
		,[NavigationText]
		,[ResourceName]
		,[NavigationUrl]
		,[UrlRouteName]
		,[RouteDataName]
		,[IconName]
		,[ImageUrl]
		,[NavigationTarget]
		,[DisplayOrder]
		,[OnlyOwnerVisible]
		,[IsLocked]
		,[IsEnabled]
from  [dbo].[tn_InitialNavigations] where PresentAreaKey = 'UserSpace'

INSERT [dbo].[tn_SystemData] (Datakey, LongValue, DecimalValue) VALUES ('Founder', 207677921948, 0)
INSERT [dbo].[tn_SystemData] (Datakey, LongValue, DecimalValue) VALUES ('CNZZStatisticsEnable', 0, 0)
INSERT [dbo].[tn_SystemData] (Datakey, LongValue, DecimalValue) VALUES ('CNZZStatisticsPassword', 0, 0)
INSERT [dbo].[tn_SystemData] (Datakey, LongValue, DecimalValue) VALUES ('CNZZStatisticsSiteId', 0, 0)
