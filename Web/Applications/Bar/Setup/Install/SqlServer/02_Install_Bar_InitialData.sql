-----应用
DELETE FROM [dbo].[tn_Applications] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_Applications] ([ApplicationId], [ApplicationKey], [Description], [IsEnabled], [IsLocked], [DisplayOrder]) VALUES (1012, N'Bar', N'帖吧应用', 1, 0, 1012)

-----快捷操作
DELETE FROM [dbo].[tn_ApplicationManagementOperations]  WHERE [ApplicationId] = 1012
DELETE FROM [dbo].[tn_ApplicationManagementOperations]  WHERE [ApplicationId] = 1011
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (13101201, 1012, 13101201, N'GroupSpace', 1, N'发帖', N'', N'', N'Group_Bar_Edit', NULL, NULL, NULL, N'_self', 13101201, 0, 1, 1)
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20101102, 1011, 0, N'ControlPanel', 1, N'帖子管理', N'', N'', N'ControlPanel_GroupBar_ManageThreads', NULL, NULL, NULL, N'_self', 20101102, 1, 0, 1)
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20101103, 1011, 0, N'ControlPanel', 1, N'回帖管理', N'', N'', N'ControlPanel_GroupBar_ManagePosts', NULL, NULL, NULL, N'_self', 20101104, 1, 0, 1)
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20101104, 1011, 0, N'ControlPanel', 1, N'发言管理', N'', N'', N'ControlPanel_GroupMicroblog_Common', NULL, NULL, NULL, N'_self', 20101103, 1, 0, 1)

-----应用在呈现区域的设置
DELETE FROM [dbo].[tn_ApplicationInPresentAreaSettings] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1012, N'Channel', 0, 1, 1)
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1012, N'UserSpace', 0, 1, 0)
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1012, N'GroupSpace', 0, 1, 1)

-----默认安装记录
DELETE FROM [dbo].[tn_ApplicationInPresentAreaInstallations] WHERE [ApplicationId] = 1012  and OwnerId = 0
INSERT [dbo].[tn_ApplicationInPresentAreaInstallations] ([OwnerId], [ApplicationId], [PresentAreaKey]) VALUES (0, 1012, 'Channel')

-----导航
DELETE FROM [dbo].[tn_InitialNavigations] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10101201, 0, 0, N'Channel', 1012, 0, N'帖吧', N'', N'', N'Channel_Bar_Home', NULL, N'Bar', NULL, N'_self', 10101201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11101201, 0, 0, N'UserSpace', 1012, 0, N'帖吧', N' ', N' ', N'Channel_Bar_UserBar', N'', N'Bar', NULL, N'_blank', 11101201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20101201, 20000011, 2, N'ControlPanel', 1012, 0, N'帖吧', N' ', N' ', N'ControlPanel_Bar_Home', NULL, NULL, NULL, N'_self', 20101201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (13101201, 0, 0, N'GroupSpace', 1012, 0, N'讨论', N'', N'', N'Group_Bar_SectionDetail', NULL, NULL, NULL, N'_self', 13101201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (13101205, 13900190, 1, N'GroupSpace', 1012, 0, N'讨论管理', N' ', N' ', N'Group_Bar_ManageThreads', NULL, NULL, NULL, N'_self', 13101205, 0, 0, 1)

-----用户角色
DELETE FROM [dbo].[tn_Roles] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_Roles] ([RoleName], [FriendlyRoleName], [IsBuiltIn], [ConnectToUser], [ApplicationId], [IsPublic], [Description], [IsEnabled], [RoleImage]) VALUES (N'BarAdministrator', N'帖吧管理员', 1, 1, 1012, 1, N'管理帖吧应用下的内容', 1, N'')

-----权限项
DELETE FROM [dbo].[tn_PermissionItems] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_PermissionItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [EnableQuota], [EnableScope]) VALUES (N'Bar_CreateThread', 1012, N'创建帖子', 11, 0, 0)
INSERT [dbo].[tn_PermissionItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [EnableQuota], [EnableScope]) VALUES (N'Bar_CreatePost', 1012, N'创建回帖', 12, 0, 0)
-----角色针对权限的设置
DELETE FROM [dbo].[tn_PermissionItemsInUserRoles] WHERE [ItemKey] = N'Bar_CreateThread' and [RoleName] = N'RegisteredUsers'
DELETE FROM [dbo].[tn_PermissionItemsInUserRoles] WHERE [ItemKey] = N'Bar_CreatePost' and [RoleName] = N'RegisteredUsers'
INSERT [dbo].[tn_PermissionItemsInUserRoles] ([RoleName], [ItemKey], [PermissionType], [PermissionQuota], [PermissionScope], [IsLocked]) VALUES ( N'RegisteredUsers', N'Bar_CreateThread', 1, 0, 0, 0)
INSERT [dbo].[tn_PermissionItemsInUserRoles] ([RoleName], [ItemKey], [PermissionType], [PermissionQuota], [PermissionScope], [IsLocked]) VALUES ( N'RegisteredUsers', N'Bar_CreatePost', 1, 0, 0, 0)


-----动态初始化数据
DELETE FROM  [dbo].[tn_ActivityItems] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateBarPost', 1012, N'发布回帖', 2, N'', 0, 1, 0)
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateBarRating', 1012, N'帖子评分', 3, N'有人对帖子进行评分时，会产生此动态', 0, 1, 0)
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateBarThread', 1012, N'发布帖子', 1, N'当被关注的帖吧或用户有新帖子发布时，会收到此动态', 0, 1, 1)

-----审核
DELETE FROM [dbo].[tn_AuditItems] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'Bar_Post', 1012, N'回帖', 8, N'')
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'Bar_Section', 1012, N'创建帖吧', 6, N'')
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'Bar_Thread', 1012, N'发帖', 7, N'')

--审核规则
INSERT [dbo].[tn_AuditItemsInUserRoles]([RoleName],[ItemKey] ,[StrictDegree],[IsLocked])VALUES(N'RegisteredUsers',N'Bar_Section',2 ,0)
INSERT [dbo].[tn_AuditItemsInUserRoles]([RoleName],[ItemKey] ,[StrictDegree],[IsLocked])VALUES(N'ModeratedUser',N'Bar_Section',2 ,0)

-----积分
DELETE FROM [dbo].[tn_PointItems] WHERE [ApplicationId]=1012
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description]) VALUES (N'Bar_CreateThread', 1012, N'创建帖子', 120, 5, 1, 5, 0, 0, 0, N'')
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description]) VALUES (N'Bar_DeleteThread', 1012, N'删除帖子', 121, -5, -1, -5, 0, 0, 0, N'')

-----租户类型
DELETE FROM [dbo].[tn_TenantTypes] WHERE [ApplicationId] = 1012
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'101200', 1012, N'帖吧', N'')
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'101201', 1012, N'帖吧', N'Spacebuilder.Bar.BarSection,Spacebuilder.Bar')
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'101202', 1012, N'帖子', N'Spacebuilder.Bar.BarThread,Spacebuilder.Bar')

-----租户使用到的服务
DELETE FROM [dbo].[tn_TenantTypesInServices] WHERE [TenantTypeId]in ('101201','101202','101203')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101201', N'SiteCategory')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101201', N'Subscribe')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101201', N'Recommend')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101201', N'Count')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101202', N'Attachment')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101202', N'AtUser')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101202', N'OwnerCategory')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101202', N'Tag')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101202', N'Comment')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101202', N'Count')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101202', N'Recommend')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101203', N'Attachment')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'101203', N'AtUser')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES ( N'101202', N'UserCategory')

-----自运行任务
DELETE FROM [dbo].[tn_TaskDetails] WHERE [ClassType] = N'Spacebuilder.Bar.ExpireStickyThreadsTask,Spacebuilder.Bar'
INSERT [dbo].[tn_TaskDetails] ([Name], [TaskRule], [ClassType], [Enabled], [RunAtRestart], [IsRunning], [LastStart], [LastEnd], [LastIsSuccess], [NextStart], [StartDate], [EndDate], [RunAtServer]) VALUES (N'更新置顶时间到期的帖子', N'0 0 0/12 * * ?', N'Spacebuilder.Bar.ExpireStickyThreadsTask,Spacebuilder.Bar', 1, 0, 0, N'', N'', 1, N'', N'', NULL, 0)

-----推荐类别
DELETE FROM [dbo].[tn_RecommendItemTypes] WHERE [TenantTypeId] in ('101201','101202')
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'10120101', N'101201', N'推荐帖吧', N'', 0, CAST(0x0000A10400000000 AS DateTime))
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'10120201', N'101202', N'推荐帖子幻灯片', N'', 1, CAST(0x0000A10400000000 AS DateTime))

-----类别
DELETE FROM [dbo].[tn_Categories] WHERE [TenantTypeId] = '101201'
SET IDENTITY_INSERT [tn_Categories] ON
INSERT [tn_Categories] ([CategoryId], [ParentId], [OwnerId], [TenantTypeId], [CategoryName], [Description], [DisplayOrder], [Depth], [ChildCount], [ItemCount], [PrivacyStatus], [AuditStatus], [FeaturedItemId], [LastModified], [DateCreated], [PropertyNames], [PropertyValues]) VALUES (1, 0, 0, N'101201', N'默认类别', N'', 1, 0, 0, 0, 2, 40, 0, CAST(0x0000A187002DE2B1 AS DateTime), CAST(0x0000A187002DE2B1 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [tn_Categories] OFF

-----广告位
DELETE FROM [dbo].[tn_AdvertisingPosition] WHERE [PositionId] like '101012%'
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10101200001', N'Channel', N'贴吧频道中部广告位(710x100)', N'AdvertisingPosition\00001\01012\00001\10101200001.jpg', 710, 100, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10101200002', N'Channel', N'贴吧详细显示页中部广告位(710x70)', N'AdvertisingPosition\00001\01012\00002\10101200002.jpg', 710, 70, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10101200003', N'Channel', N'贴吧详细显示页中部广告位(230x260)', N'AdvertisingPosition\00001\01012\00003\10101200003.jpg', 230, 260, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10101200004', N'Channel', N'帖子详细显示页左中部广告位(230x60)', N'AdvertisingPosition\00001\01012\00004\10101200004.jpg', 230, 60, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10101200005', N'Channel', N'帖子详细显示页左下部广告位(230x260)', N'AdvertisingPosition\00001\01012\00005\10101200005.jpg', 230, 260, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10101200006', N'Channel', N'帖子详细显示页中部广告位(710x70)', N'AdvertisingPosition\00001\01012\00006\10101200006.jpg', 710, 70, 1)