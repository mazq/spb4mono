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
