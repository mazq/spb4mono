-----应用
DELETE FROM [dbo].[tn_Applications] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_Applications] ([ApplicationId], [ApplicationKey], [Description], [IsEnabled], [IsLocked], [DisplayOrder]) VALUES (1002, N'Blog', N'日志应用', 1, 0, 1002)

-----应用在呈现区域的设置
DELETE FROM [dbo].[tn_ApplicationInPresentAreaSettings] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1002, N'Channel', 0, 1, 0)
INSERT [dbo].[tn_ApplicationInPresentAreaSettings] ([ApplicationId], [PresentAreaKey], [IsBuiltIn], [IsAutoInstall], [IsGenerateData]) VALUES (1002, N'UserSpace', 0, 1, 1)

-----默认安装记录
DELETE FROM [dbo].[tn_ApplicationInPresentAreaInstallations] WHERE [ApplicationId] = 1002 and OwnerId = 0
INSERT [dbo].[tn_ApplicationInPresentAreaInstallations] ([OwnerId], [ApplicationId], [PresentAreaKey]) VALUES (0, 1002, 'Channel')

-----审核
DELETE FROM [dbo].[tn_AuditItems] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_AuditItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description]) VALUES (N'Blog_Thread', 1002, N'撰写日志', 5, N'')

-----积分
DELETE FROM [dbo].[tn_PointItems] WHERE [ApplicationId]=1002
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description]) VALUES (N'Blog_CreateThread', 1002, N'创建日志', 110, 5, 1, 5, 0, 0, 0, N'')
INSERT [dbo].[tn_PointItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [ExperiencePoints], [ReputationPoints], [TradePoints], [TradePoints2], [TradePoints3], [TradePoints4], [Description]) VALUES (N'Blog_DeleteThread', 1002, N'删除日志', 111, -5, -1, -5, 0, 0, 0, N'')

-----快捷操作
DELETE FROM [dbo].[tn_ApplicationManagementOperations] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10100201, 1002, 0, N'Channel', 1, N'撰写日志', N'', N'', N'UserSpace_Blog_Create', N'spaceKey', N'Write', NULL, N'_blank', 10100202, 1, 0, 1)
INSERT [dbo].[tn_ApplicationManagementOperations] ([OperationId], [ApplicationId], [AssociatedNavigationId], [PresentAreaKey], [OperationType], [OperationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100201, 1002, 0, N'UserSpace', 1, N'撰写日志', N' ', N' ', N'UserSpace_Blog_Create', NULL, N'Write', N'', N'_self', 11100201, 1, 1, 1)

-----导航
DELETE FROM [dbo].[tn_InitialNavigations] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10100201, 0, 0, N'Channel', 1002, 0, N'日志', N'', N'', N'Channel_Blog_Home', NULL, N'Blog', NULL, N'_self', 10100201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10100202, 10100201, 1, N'Channel', 1002, 0, N'日志首页', N'', N'', N'Channel_Blog_Home', NULL, NULL, NULL, N'_self', 10100202, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (10100203, 10100201, 1, N'Channel', 1002, 0, N'我的日志', N'', N'', N'UserSpace_Blog_Blog', N'spaceKey', NULL, NULL, N'_self', 10100203, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100201, 0, 0, N'UserSpace', 1002, 0, N'日志', N' ', N' ', N'UserSpace_Blog_Home', NULL, N'Blog', NULL, N'_self', 11100201, 0, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100202, 11100201, 1, N'UserSpace', 1002, 0, N'日志首页', N' ', N' ', N'UserSpace_Blog_Home', NULL, NULL, NULL, N'_self', 11100202, 1, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100203, 11100201, 1, N'UserSpace', 1002, 0, N'我的日志', N' ', N' ', N'UserSPace_Blog_Blog', NULL, NULL, NULL, N'_self', 11100203, 1, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (11100204, 11100201, 1, N'UserSpace', 1002, 0, N'我的关注', N' ', N' ', N'UserSpace_Blog_Subscribed', NULL, NULL, NULL, N'_self', 11100204, 1, 0, 1)
INSERT [dbo].[tn_InitialNavigations] ([NavigationId], [ParentNavigationId], [Depth], [PresentAreaKey], [ApplicationId], [NavigationType], [NavigationText], [ResourceName], [NavigationUrl], [UrlRouteName], [RouteDataName], [IconName], [ImageUrl], [NavigationTarget], [DisplayOrder], [OnlyOwnerVisible], [IsLocked], [IsEnabled]) VALUES (20100201, 20000011, 2, N'ControlPanel', 1002, 0, N'日志', N'', N'', N'ControlPanel_Blog_Home', NULL, NULL, NULL, N'_self', 20100201, 0, 0, 1)

-----动态初始化数据
DELETE FROM  [dbo].[tn_ActivityItems] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateBlogComment', 1002, N'日志评论', 2, N'', 0, 1, 0)
INSERT [dbo].[tn_ActivityItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [Description], [IsOnlyOnce], [IsUserReceived], [IsSiteReceived]) VALUES (N'CreateBlogThread', 1002, N'发布日志', 1, N'', 0, 1, 1)


-----用户角色
DELETE FROM [dbo].[tn_Roles] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_Roles] ([RoleName], [FriendlyRoleName], [IsBuiltIn], [ConnectToUser], [ApplicationId], [IsPublic], [Description], [IsEnabled], [RoleImage]) VALUES (N'BlogAdministrator', N'日志管理员', 1, 1, 1002, 1, N'管理日志应用下的内容', 1, N'')

-----权限项
DELETE FROM [dbo].[tn_PermissionItems] WHERE [ApplicationId] = 1002
INSERT [dbo].[tn_PermissionItems] ([ItemKey], [ApplicationId], [ItemName], [DisplayOrder], [EnableQuota], [EnableScope]) VALUES (N'Blog_Create', 1002, N'发布日志', 2, 0, 0)
-----角色针对权限的设置
DELETE FROM [dbo].[tn_PermissionItemsInUserRoles] WHERE [ItemKey] = N'Blog_Create' and [RoleName] = N'RegisteredUsers'
INSERT [dbo].[tn_PermissionItemsInUserRoles] ([RoleName], [ItemKey], [PermissionType], [PermissionQuota], [PermissionScope], [IsLocked]) VALUES ( N'RegisteredUsers', N'Blog_Create', 1, 0, 0, 0)

-----审核初始化数据
DELETE FROM [dbo].[tn_AuditItems] WHERE [ApplicationId] = 1002
INSERT INTO [dbo].[tn_AuditItems] ([ItemKey],[ApplicationId],[ItemName],[DisplayOrder],[Description]) VALUES ('Blog_Thread',1002,'日志',1,'')

-----租户类型
DELETE FROM [dbo].[tn_TenantTypes] WHERE [ApplicationId]=1002
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'100200', 1002, N'日志应用', N'')
INSERT [dbo].[tn_TenantTypes] ([TenantTypeId], [ApplicationId], [Name], [ClassType]) VALUES (N'100201', 1002, N'日志', N'Spacebuilder.Blog.BlogThread,Spacebuilder.Blog')

-----租户使用到的服务
DELETE FROM [dbo].[tn_TenantTypesInServices] WHERE [TenantTypeId]='100201'
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'Attachment')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'Attitude')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'AtUser')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'Comment')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'OwnerCategory')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'SiteCategory')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'UserCategory')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'Subscribe')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'Recommend')
INSERT [dbo].[tn_TenantTypesInServices] ([TenantTypeId], [ServiceKey]) VALUES (N'100201', N'Tag')

-----推荐类别
DELETE FROM [dbo].[tn_RecommendItemTypes] WHERE [TypeId] in ('10020101','10020102','00001102')
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'10020101', N'100201', N'推荐日志', N'标题列表', 0, N'')
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'10020102', N'100201', N'推荐日志幻灯片', N'频道日志首页幻灯片日志', 1, N'')
INSERT [dbo].[tn_RecommendItemTypes] ([TypeId], [TenantTypeId], [Name], [Description], [HasFeaturedImage], [DateCreated]) VALUES (N'00001102', N'000011', N'推荐博主', N'推荐博主', 0, N'')

-----类别
DELETE FROM [dbo].[tn_Categories] WHERE [TenantTypeId] = '100201'
SET IDENTITY_INSERT [tn_Categories] ON
INSERT [tn_Categories] ([CategoryId], [ParentId], [OwnerId], [TenantTypeId], [CategoryName], [Description], [DisplayOrder], [Depth], [ChildCount], [ItemCount], [PrivacyStatus], [AuditStatus], [FeaturedItemId], [LastModified], [DateCreated], [PropertyNames], [PropertyValues]) VALUES (40, 0, 0, N'100201', N'默认类别', N'', 40, 0, 0, 0, 2, 40, 0, CAST(0x0000A187003961A9 AS DateTime), CAST(0x0000A187003961A9 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [tn_Categories] OFF

-----广告位
DELETE FROM [dbo].[tn_AdvertisingPosition] WHERE [PositionId] like '101002%'
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10100200001', N'Channel', N'日志频道首页头部广告位(950x100)', N'AdvertisingPosition\00001\01002\00001\10100200001.jpg', 950, 100, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10100200002', N'Channel', N'日志频道首页右中部广告位(230x260)', N'AdvertisingPosition\00001\01002\00002\10100200002.jpg', 230, 260, 1)
INSERT [dbo].[tn_AdvertisingPosition] ([PositionId], [PresentAreaKey], [Description], [FeaturedImage], [Width], [Height], [IsEnable]) VALUES (N'10100200003', N'Channel', N'日志详细显示中部广告位(740x50)', N'AdvertisingPosition\00001\01002\00003\10100200003.jpg', 740, 50, 1)