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
