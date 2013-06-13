-----日志附件
delete from tn_Attachments where TenantTypeId = N'100201'
insert tn_Attachments ([AssociateId],[OwnerId],[TenantTypeId],[UserId]
					   ,[UserDisplayName],[FileName],[FriendlyFileName],[MediaType]
					   ,[ContentType],[FileLength],[Height],[Width],[Price],[Password]
					   ,[IP],[DateCreated]) 
select ThreadID,OwnerUserID,N'100201',OwnerUserID
	   ,isnull((select top 1 Author from old_spb_BlogThreads where OwnerUserID = OwnerUserID and Author <> N''),N'')
	   ,FileName,FriendlyFileName
	   ,case when charindex('jpg',FileName) > 0 then 1 when charindex('bmp',FileName) > 0 then 1 when charindex('png',FileName) > 0 then 1 when charindex('gif',FileName) > 0 then 1 else 99 end
	   ,ContentType,ContentSize,Height,Width,0,N'',N'',DateCreated
from old_spb_BlogThreadAttachments
GO

-----日志评论
Create Table #TempComments (Id bigint identity(1,1),oldId bigint)
SET IDENTITY_INSERT #TempComments ON
insert #TempComments(Id) select top 1 Id from tn_Comments order by Id desc
SET IDENTITY_INSERT #TempComments OFF

insert #TempComments(oldId) select PostID from old_spb_BlogComments 
delete from #TempComments where oldId is NULL

SET IDENTITY_INSERT tn_Comments ON
delete from tn_Comments where TenantTypeId = N'100201'
insert tn_Comments ([Id],[ParentId],[CommentedObjectId],[TenantTypeId],[OwnerId],[UserId]
					,[Author],[ToUserId],[ToUserDisplayName],[Subject],[Body],[IsPrivate]
					,[AuditStatus],[ChildCount],[IsAnonymous],[IP],[DateCreated]) 
select (select Id from #TempComments where oldId = PostId)
	   ,isnull((select Id from #TempComments where oldId = ParentID and ParentID > 0),0)
	   ,ThreadID,N'100201',OwnerUserID,UserID,Author,CASE WHEN ParentID = 0 THEN 0 ELSE OwnerUserID END
	   ,isnull((select top 1 Author from old_spb_BlogThreads where UserID = OwnerUserID and Author <> N'' and ParentID > 0),N'')
	   ,Subject,Body,0,40,0,charindex(Author,'匿名用户'),N'',getdate()
from old_spb_BlogComments
SET IDENTITY_INSERT tn_Comments OFF

drop table #TempComments
GO

-----日志站点类别
Create Table #TempCategories(CategoryId int,OldCategoryId int, CategoryName nvarchar(128))
delete from tn_Categories where TenantTypeId = N'100201' and OwnerId = 0
insert tn_Categories ([ParentId],[OwnerId],[TenantTypeId],[CategoryName]
					  ,[Description],[DisplayOrder],[Depth],[ChildCount],[ItemCount]
					  ,[PrivacyStatus],[AuditStatus],[FeaturedItemId],[LastModified]
					  ,[DateCreated]) 
select ParentID,0,N'100201',CategoryName,Description,DisplayOrder
	   ,Depth,ChildCount,ItemCount,2,40,0,getdate(),getdate()
from old_spb_SiteCategories where ApplicationID = 111

insert #TempCategories (CategoryName,OldCategoryId,CategoryId)
select c.CategoryName,oc.CategoryID,c.CategoryId
from tn_Categories c inner join old_spb_SiteCategories oc
on c.CategoryName = oc.CategoryName
where TenantTypeId = N'100201' and OwnerId = 0 and ApplicationID = 111

delete from tn_ItemsInCategories 
where exists (select 1 from #TempCategories tc where tc.CategoryId = CategoryId)
insert tn_ItemsInCategories(CategoryId,ItemId)
select tc.CategoryId,ThreadID
from old_spb_BlogThreads ob left join #TempCategories tc
on ob.SiteCategoryID = tc.OldCategoryId
where ob.SiteCategoryID > 0 

delete from #TempCategories

delete from tn_Categories where TenantTypeId = N'100201' and OwnerId > 0
insert tn_Categories ([ParentId],[OwnerId],[TenantTypeId],[CategoryName]
					  ,[Description],[DisplayOrder],[Depth],[ChildCount],[ItemCount]
					  ,[PrivacyStatus],[AuditStatus],[FeaturedItemId],[LastModified]
					  ,[DateCreated]) 
select 0,UserID,N'100201',CategoryName,Description,DisplayOrder,0,0,ItemCount
	   ,PrivacyStatus,40,0,MostRecentUpdateDate,DateCreated 
from old_spb_BlogThreadUserCategories

insert #TempCategories (CategoryName,OldCategoryId,CategoryId)
select c.CategoryName,obc.CategoryID,c.CategoryId
from  old_spb_BlogThreadUserCategories obc 
left join tn_Categories c 
on obc.CategoryName = c.CategoryName
where TenantTypeId = N'100201' and OwnerId > 0 and UserID = OwnerId

delete from tn_ItemsInCategories
where exists (select 1 from #TempCategories tc where tc.CategoryId = CategoryId)
insert tn_ItemsInCategories(CategoryId,ItemId)
select tc.CategoryId,ThreadID
from old_spb_BlogThreads ob left join #TempCategories tc
on ob.UserCategoryID = tc.OldCategoryId
where ob.UserCategoryID > 0 

Drop Table #TempCategories
GO

-----日志标签
delete from tn_Tags where TenantTypeId = N'100201'
insert tn_Tags ([TenantTypeId],[TagName],Description,FeaturedImage,[IsFeatured],[ItemCount],[OwnerCount],[AuditStatus],[DateCreated]) 
select N'100201',TagName,'','',0,ItemCount,UserCount,40,getdate()
from old_spb_BlogThreadSiteTags

delete from tn_TagsInOwners where TenantTypeId = N'100201' 
insert tn_TagsInOwners (TenantTypeId,TagName,OwnerId,ItemCount)
select TenantTypeId,bt.TagName,UserID,bt.ItemCount
from old_spb_BlogThreadUserTags bt
left join tn_Tags t
on bt.TagName = t.TagName
where TenantTypeId = N'100201' 

delete from tn_ItemsInTags where TenantTypeId = N'100201' 
insert tn_ItemsInTags (TagName,TagInOwnerId,ItemId,TenantTypeId)
select btiu.TagName,Id,ThreadID,TenantTypeId
from tn_TagsInOwners tio
right join old_spb_BlogThreads bt
on tio.OwnerId = bt.OwnerUserID
right join old_spb_BlogThreadsInUserTags btiu 
on btiu.ItemID = bt.ThreadID and tio.TagName = btiu.TagName
where TenantTypeId = N'100201' and btiu.TagName <> N''

GO

-----日志
delete from spb_BlogThreads
GO
set identity_insert spb_BlogThreads ON
insert spb_BlogThreads([ThreadId],[TenantTypeId],[OwnerId],[UserId],[Author],[Subject]
					,[Body],[Summary],[IsDraft],[IsLocked],[IsEssential],[IsSticky],[AuditStatus]
					,[PrivacyStatus],[IsReproduced],[OriginalAuthorId],[IP],[DateCreated],Keywords
					,[FeaturedImageAttachmentId],[FeaturedImage],[LastModified])
  select [ThreadId],'000011', [OwnerUserID],[OwnerUserID],[Author],substring([Subject],1,128)
		,[Body],[Excerpt],0,0,[IsEssential],CASE WHEN StickyDate>GETDATE() THEN 1 ELSE 0 END,[AuditingStatus]
		,CASE WHEN [PrivacyStatus]=20 THEN 1 WHEN [PrivacyStatus]=30 THEN 2 ELSE 0 END,0,[OwnerUserID],[UserHostAddress],[PostDate]
		,''
		,0,'',[PostDate]
  from [old_spb_BlogThreads]
set identity_insert spb_BlogThreads OFF
GO

-----日志正文中的附件地址
Declare @AttachmentID int
Declare @ThreadID int
Declare @FileName nvarchar(512) 
Declare @ContentType nvarchar(64)
Declare @DateCreated datetime
Declare Cur Cursor For 
select AttachmentID,ThreadID,FileName,ContentType,DateCreated from old_spb_BlogThreadAttachments
Open Cur 
Fetch next From Cur Into @AttachmentID,@ThreadID,@FileName,@ContentType,@DateCreated
While @@fetch_status=0 
Begin 
Declare @oldFilePath nvarchar(100),
		@newFilePath nvarchar(100)
set @oldFilePath = 'Services/BlogAttachment.ashx?AttachmentID='+CAST(@AttachmentID as nvarchar(100))
if(charindex('image', @ContentType) > 0 )
	begin
		UPDATE spb_BlogThreads SET Body = REPLACE(Body,@oldFilePath+'" target="_blank"><img src="',@oldFilePath+'" rel="fancybox"><img src="') where ThreadId =@ThreadId
		set @newFilePath = 'Uploads/Blog/'+CAST(YEAR(@DateCreated) as char(4)) +'/'+right ('0'+CAST(MONTH(@DateCreated) as varchar(100)),2)+'/'
								  +right ('0'+CAST(DAY(@DateCreated) as varchar(100)),2) +'/'+@FileName
	end
else
	 begin
		 Declare @AttachId bigint
		 set @AttachId = 0
		 select @AttachId=attachmentId from tn_Attachments where TenantTypeId=100201 and AssociateId=@ThreadID and FileName=@FileName 
		 set @newFilePath = 'Handlers/AttachmentAuthorize.ashx?tenantTypeId=100201&enableCaching=True&attachmentId='+CAST(@AttachId as nvarchar(100))
	 end
UPDATE spb_BlogThreads SET Body = REPLACE(Body,@oldFilePath,@newFilePath) where ThreadId =@ThreadID
Fetch Next From Cur Into @AttachmentID,@ThreadID,@FileName,@ContentType,@DateCreated
End 
Close Cur 
Deallocate Cur