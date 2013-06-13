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
