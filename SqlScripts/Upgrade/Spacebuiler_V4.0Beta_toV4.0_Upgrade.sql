--升级脚本
----- 2013-4-15-1 Start
delete from tn_InitialNavigations  where NavigationId = 20000075
delete from tn_InitialNavigations  where NavigationId = 20000076
delete from tn_InitialNavigations  where NavigationId = 20000077
delete from tn_InitialNavigations  where NavigationId = 20000063
delete from tn_InitialNavigations  where NavigationId = 20000063
delete from tn_InitialNavigations  where NavigationId = 20000048
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20000075, 20000031, 2, N'ControlPanel', 0, 1, N'客服消息', N'', N'', N'ControlPanel_Operation_ManageCustomMessage', NULL, NULL, NULL, N'_self', 20000075, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20000076, 20000031, 2, N'ControlPanel', 0, 1, N'群发消息', N'', N'', N'ControlPanel_Operation_MassMessages', NULL, NULL, NULL, N'_self', 20000076, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20000077, 20000031, 2, N'ControlPanel', 0, 1, N'站点统计', N'', N'', N'ControlPanel_Operation_Statistics', NULL, NULL, NULL, N'_blank', 20000077, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20000063, 20000056, 2, N'ControlPanel', 0, 1, N'暂停站点', N'', N'', N'ControlPanel_Settings_PauseSiteSettings', NULL, NULL, NULL, N'_self', 20000063, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20000048, 20000043, 2, N'ControlPanel', 0, 1, N'重建缩略图', N'', N'', N'ControlPanel_Tool_RebuildingThumbnails', NULL, NULL, NULL, N'_self', 20000047, 0, 1, 1)
----- 2013-4-15-1 end

----- 2013-4-18-1 Start
delete from tn_InitialNavigations where NavigationId = 10100203
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10100203, 10100201, 1, N'Channel', 1002, 0, N'我的日志', N'', N'', N'UserSpace_Blog_Blog', N'spaceKey', NULL, NULL, N'_self', 10100203, 0, 0, 1)
----- 2013-4-18-1 end

----- 2013-4-20-1 Start
delete from tn_TaskDetails where ClassType= 'Spacebuilder.Common. NoModeratedUsersTask,Spacebuilder.Common'
delete from tn_TaskDetails where ClassType= 'Spacebuilder.Bar.ExpireStickyThreadsTask,Spacebuilder.Bar'
delete from tn_TaskDetails where ClassType= 'Spacebuilder.Group.CalculateGrowthValuesTask,Spacebuilder.Group'
INSERT [dbo].[tn_TaskDetails] ([Name], [TaskRule], [ClassType], [Enabled], [RunAtRestart], [IsRunning], [LastStart], [LastEnd], [LastIsSuccess], [NextStart], [StartDate], [EndDate], [RunAtServer]) VALUES (N'更新置顶时间到期的帖子', N'0 0 0/12 * * ?', N'Spacebuilder.Bar.ExpireStickyThreadsTask,Spacebuilder.Bar', 1, 0, 0,  N'', N'', 1, N'', N'', NULL, 1)
INSERT [dbo].[tn_TaskDetails] ([Name], [TaskRule], [ClassType], [Enabled], [RunAtRestart], [IsRunning], [LastStart], [LastEnd], [LastIsSuccess], [NextStart], [StartDate], [EndDate], [RunAtServer]) VALUES (N'更新群组的成长值', N'0/40 0 * * * ?', N'Spacebuilder.Group.CalculateGrowthValuesTask,Spacebuilder.Group', 1, 0, 0,  N'', N'', 1, N'', N'', NULL, 1)
----- 2013-4-20-1 end

----- 2013-4-22-1 Start
delete from tn_SystemData where Datakey = 'CNZZStatisticsEnable'
delete from tn_SystemData where Datakey = 'CNZZStatisticsPassword'
delete from tn_SystemData where Datakey = 'CNZZStatisticsSiteId'
INSERT [dbo].[tn_SystemData] (Datakey, LongValue, DecimalValue) VALUES ('CNZZStatisticsEnable', 0, 0)
INSERT [dbo].[tn_SystemData] (Datakey, LongValue, DecimalValue) VALUES ('CNZZStatisticsPassword', 0, 0)
INSERT [dbo].[tn_SystemData] (Datakey, LongValue, DecimalValue) VALUES ('CNZZStatisticsSiteId', 0, 0)
----- 2013-4-20-1 end

--删除版主，组织管理员，组织成员
delete from [dbo].[tn_Roles] where [RoleName] = 'Moderator'
delete from [dbo].[tn_Roles] where [RoleName] = 'OrganizationManager'
delete from [dbo].[tn_Roles] where [RoleName] = 'OrganizationMember'



----- 2013-4-25-1 start
update tn_PresentAreas set EnableThemes = 1 where PresentAreaKey = 'GroupSpace'
update tn_PresentAreas set EnableThemes = 1 where PresentAreaKey = 'UserSpace'
----- 2013-4-25-1 end

----- 2013-4-8-1 start
if not exists(select * from syscolumns where id=object_id('tn_InviteFriendRecords') and name='InvitingUserHasBeingRewarded') 
begin 
  alter table tn_InviteFriendRecords add InvitingUserHasBeingRewarded tinyint
end
go
if exists(select * from syscolumns where id=object_id('tn_InviteFriendRecords') and name='InvitingUserHasBeingRewarded')  
begin
update tn_InviteFriendRecords set InvitingUserHasBeingRewarded = 0
update tn_InviteFriendRecords set InvitingUserHasBeingRewarded = 1 
where  UserId in (select tn_InviteFriendRecords.UserId from tn_InviteFriendRecords
inner join tn_Users
on tn_InviteFriendRecords.InvitedUserId = tn_Users.UserId
where tn_Users.IsModerated=1)
end
----- 2013-4-8-1 end


----- 2013-4-24-1 Start
if not exists(select * from syscolumns where id=object_id('tn_AtUsers') and name='UserId')
begin
	ALTER TABLE tn_AtUsers ADD UserId bigint not null default 0
end
GO

IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('tn_AtUsers') and name='UserName')
begin 
	update tn_AtUsers
	set UserId=U.UserId 
	from tn_Users U 
	where tn_AtUsers.UserName = U.UserName
	IF EXISTS (SELECT indid FROM sysindexes WHERE id = OBJECT_ID('tn_AtUsers') AND name = 'IX_tn_AtUsers_UserName')
	DROP INDEX IX_tn_AtUsers_UserName on tn_AtUsers
	ALTER TABLE tn_AtUsers DROP COLUMN UserName
end
----- 2013-4-24-1 end

----- 增加表tn_SmtpSettings start
GO

/****** Object:  Table [dbo].[tn_SmtpSettings]    Script Date: 04/16/2013 16:37:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
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
----- 增加表tn_SmtpSettings end

update tn_Users
set NickName = UserName
where NickName = '' or NickName = NULL

update tn_Users
set NickName = UserName
where NickName in (select NickName from tn_Users group by NickName having count(NickName) > 1)

--加入真实姓名的隐私项
delete from [tn_PrivacyItems] where ItemKey='TrueName'
INSERT [tn_PrivacyItems] ([ItemKey], [ItemGroupId], [ApplicationId], [ItemName], [Description], [DisplayOrder], [PrivacyStatus]) VALUES (N'TrueName', 1, 0, N'TrueName', N'', 8, 1)
