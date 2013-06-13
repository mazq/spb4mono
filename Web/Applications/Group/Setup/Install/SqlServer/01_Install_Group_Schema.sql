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
