-----应用数据
DELETE FROM [dbo].[tn_Applications] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_Applications] ([ApplicationId], [ApplicationKey], [Description], [IsEnabled], [IsLocked], [DisplayOrder]) VALUES (1001, N'Microblog', N'微博应用', 1, 1, 1001)

-----应用在呈现区域的设置
DELETE FROM [dbo].[tn_ApplicationInPresentAreaSettings] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1001, N'Channel', 1, 1, 0)
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1001, N'UserSpace', 1, 1, 1)
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1001, N'GroupSpace', 1, 1, 1)

-----默认安装记录
DELETE FROM [dbo].[tn_ApplicationInPresentAreaInstallations] WHERE [ApplicationId] = 1001 and OwnerId = 0
INSERT [dbo].[tn_ApplicationInPresentAreaInstallations] ([OwnerId], [ApplicationId], [PresentAreaKey]) VALUES (0, 1001, 'Channel')

-----租户类型与服务的关系
DELETE FROM [dbo].[tn_TenantTypesInServices] WHERE [TenantTypeId] = '100101'
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100101', N'Comment')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100101', N'Recommend')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100101', N'Tag')

-----导航
DELETE FROM [dbo].[tn_InitialNavigations] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10100101, 0, 0, N'Channel', 1001, 0, N'微博', N'', N'', N'Channel_Microblog', NULL, N'Microblog', NULL, N'_self', 10100101, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100101, 0, 0, N'UserSpace', 1001, 0, N'微博', N'', N'', N'UserSpace_Microblog_Home', NULL, N'Microblog', NULL, N'_self', 11100101, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100102, 11100101, 1, N'UserSpace', 1001, 0, N'我的微博', N'', N'', N'UserSpace_Microblog_Home', NULL, NULL, NULL, N'_self', 11100102, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100103, 11100101, 1, N'UserSpace', 1001, 0, N'提到我的', N' ', N' ', N'UserSpace_Microblog_AtMe', NULL, NULL, NULL, N'_self', 11100103, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100104, 11100101, 1, N'UserSpace', 1001, 0, N'我的收藏', N'', N'', N'UserSpace_Microblog_Favorites', NULL, NULL, NULL, N'_self', 11100104, 0, 1, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20100101, 20000011, 2, N'ControlPanel', 1001, 0, N'微博', N' ', N' ', N'ControlPanel_Microblog_Home', NULL, NULL, NULL, N'_self', 20100101, 0, 1, 1)

-----用户角色
DELETE FROM [dbo].[tn_Roles] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_Roles] ([RoleName], [FriendlyRoleName], [IsBuiltIn], [ConnectToUser], [ApplicationId], [IsPublic], [Description], [IsEnabled], [RoleImage]) VALUES (N'MicroblogAdministrator', N'微博管理员', 1, 1, 1001, 1, N'管理微博应用下的内容', 1, N'')

-----权限项
DELETE FROM [dbo].[tn_PermissionItems] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_PermissionItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [EnableQuota], [EnableScope]) VALUES (N'Microblog_Create', 1001, N'发布微博', 1, 0, 0)
-----角色针对权限的设置
DELETE FROM [dbo].[tn_PermissionItemsInUserRoles] WHERE [ItemKey] = N'Microblog_Create' and [RoleName] = N'RegisteredUsers'
INSERT [dbo].[tn_PermissionItemsInUserRoles] ([RoleName], [ItemKey], [PermissionType], [PermissionQuota], [PermissionScope], [IsLocked]) VALUES ( N'RegisteredUsers', N'Microblog_Create', 1, 0, 0, 0)

-----积分
DELETE FROM [dbo].[tn_PointItems] WHERE [ApplicationId]=1001
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description]) VALUES (N'Microblog_CreateMicroblog', 1001, N'创建微博', 150, 2, 0, 2, 0, 0, 0, N'')
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description]) VALUES (N'Microblog_DeleteMicroblog', 1001, N'删除微博', 151, -2, 0, -2, 0, 0, 0, N'')

-----动态
DELETE FROM  [dbo].[tn_ActivityItems] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateMicroblog', 1001, N'发布微博', 1, N'', 0, 1, 1)

-----审核
DELETE FROM [dbo].[tn_AuditItems] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'Microblog_Comment', 1001, N'评论微博', 3, N' ')
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'Microblog_Create', 1001, N'创建微博', 4, N' ')

-----推荐类别
DELETE FROM [dbo].[tn_RecommendItemTypes] WHERE [TenantTypeId] = '100101'
INSERT INTO [dbo].[tn_RecommendItemTypes]([TypeId],[TenantTypeId],[Name],[Description],[HasFeaturedImage],[DateCreated])VALUES('10010101','100101','推荐话题','推荐话题',0,N'')

-----租户类型
DELETE FROM [dbo].[tn_TenantTypes] WHERE [ApplicationId] = 1001
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'100101', 1001, N'微博', N'Spacebuilder.Microblog.MicroblogEntity,Spacebuilder.Microblog')
