-----微博标签（话题）
delete from tn_Tags where TenantTypeId = N'100101'
insert tn_Tags ([TenantTypeId],[TagName],[IsFeatured],Description,FeaturedImage,
				[ItemCount],OwnerCount,[AuditStatus],[DateCreated]) 
select N'100101',Body,0,'','',ItemCount,0,40,DateCreated
from old_spb_MicroBlogTopics

-----微博评论
delete from tn_Comments where TenantTypeId = N'100101'
insert tn_Comments ([ParentId],[CommentedObjectId],[TenantTypeId],[OwnerId],[UserId]
					,[Author],[ToUserId],[ToUserDisplayName],[Subject],[Body],[IsPrivate]
					,[AuditStatus],[ChildCount],[IsAnonymous],[IP],[DateCreated]) 
select 0,ThreadID,N'100101',OwnerUserID,UserID,Author,0,N'',N'',Body,0,40,0,0,N'',DateCreated
from old_spb_MicroBlogComments

-----微博附件
delete from tn_Attachments where TenantTypeId = N'100101'
insert tn_Attachments ([AssociateId],[OwnerId],[TenantTypeId],[UserId]
					   ,[UserDisplayName],[FileName],[FriendlyFileName],[MediaType]
					   ,[ContentType],[FileLength],[Height],[Width],[Price],[Password]
					   ,[IP],[DateCreated]) 
select ThreadID,UserID,N'100101',UserID
	   ,isnull((select top 1 Author from old_spb_MicroBlogThreads where OwnerUserID = UserID and Author <> N''),N'')
	   ,FileName,FriendlyFileName,1,ContentType,ContentSize,Height,Width,0,N'',N'',DateCreated
from old_spb_MicroBlogAttachments

-----微博
delete from spb_Microblogs
set identity_insert spb_Microblogs ON
insert spb_Microblogs ([MicroblogId],[UserId],[Author],[TenantTypeId],[OwnerId]
,[OriginalMicroblogId],[ForwardedMicroblogId],[Body],[ReplyCount],[ForwardedCount]
,[HasPhoto],[HasVideo],[HasMusic],[PostWay],[Source],[SourceUrl],[IP],[AuditStatus]
,[DateCreated]) 
 select ThreadID,OwnerUserID,Author,'000011',OwnerUserID
 ,OriginalThreadID,ForwardedThreadID,Body,ReplyCount,ForwardedCount
 ,HasPhoto,HasVideo,HasMusic,PostMode,PostSource,'',N'127.0.0.1',AuditingStatus
 ,Datecreated from old_spb_MicroBlogThreads
 set identity_insert spb_Microblogs OFF

 --微博回复计数
update spb_Microblogs set ReplyCount = (select count(*) from tn_Comments where tn_Comments.CommentedObjectId =spb_Microblogs.MicroblogId )
