-----论坛（贴吧）标签
delete from tn_Tags where TenantTypeId = N'101202'
insert tn_Tags ([TenantTypeId],[TagName],Description,FeaturedImage
				,[IsFeatured],[ItemCount],[OwnerCount],[AuditStatus],[DateCreated]) 
select N'101202',TagName,'',''
		,0,ItemCount
	   ,(select count(UserID) from old_spb_ForumTags ft where ft.TagName = TagName)
	   ,40,getdate()
from old_spb_ForumTags

delete from tn_TagsInOwners where TenantTypeId = N'101202' 
insert tn_TagsInOwners (TenantTypeId,TagName,OwnerId,ItemCount)
select N'101202',TagName,OwnerID,ItemCount from old_spb_ForumTags

delete from tn_ItemsInTags where TenantTypeId = N'101202' 
insert tn_ItemsInTags (TagName,TagInOwnerId,ItemId,TenantTypeId)
select iift.TagName,Id,ItemID,TenantTypeId
from old_spb_ItemsInForumTags iift
right join tn_TagsInOwners tio
on iift.TagName = tio.TagName
where TenantTypeId = N'101202' and iift.TagName <> N''

-----论坛附件
delete from tn_Attachments where TenantTypeId in (N'101202',N'101203')
insert tn_Attachments ([AssociateId],
						[OwnerId],
						[TenantTypeId],
						[UserId],
						[UserDisplayName],[FileName],[FriendlyFileName],[MediaType]
					   ,[ContentType],[FileLength],[Height],[Width],[Price],[Password]
					   ,[IP],[DateCreated]) 
select case when fp.ParentID = 0 and fp.ThreadID > 0 then fp.ThreadID else fp.PostID end
	   ,fa.UserID
	   ,case when fp.ParentID = 0 and fp.ThreadID > 0 then N'101202' else N'101203' end
	   ,fa.UserID
	   ,isnull((select top 1 Author from old_spb_ForumPosts where fa.UserID = fp.UserID and Author <> N''),N'')
	   ,FileName,FriendlyFileName
	   ,case when charindex('jpg',FileName) > 0 then 1 when charindex('bmp',FileName) > 0 then 1 when charindex('png',FileName) > 0 then 1 when charindex('gif',FileName) > 0 then 1 else 99 end
	   ,ContentType,ContentSize,Height,Width,Price,cast(fp.PostID as nvarchar(100)),N'',fa.DateCreated
from old_spb_ForumPostAttachments fa
inner join old_spb_ForumPosts fp
on fa.PostID = fp.PostID

-----帖吧管理员
delete from spb_BarSectionManagers
insert spb_BarSectionManagers ([SectionId],[UserId]) 
select SectionID,UserID from old_spb_ForumModerators


-----帖吧
delete from spb_BarSections
declare @userId bigint
  set @userId=(select top 1 old_spb_Users.UserID from old_spb_Users order by UserID asc)

Create Table #TempSections(OldSectionId int,SectionId bigint,[TenantTypeId] char(6), OwnerId bigint,UserId bigint)
Insert #TempSections 
select S.SectionID,
	  CASE WHEN G.PresentAreaID= 13 THEN  spb_Groups.GroupId ELSE S.SectionID END,
	  CASE WHEN G.PresentAreaID= 13 THEN '101100' ELSE '101200' END ,
	  CASE WHEN G.PresentAreaID= 13 THEN spb_Groups.GroupId ELSE @userId END,
	  CASE WHEN G.PresentAreaID= 13 THEN spb_Groups.UserId ELSE @userId END
	  from old_spb_ForumSections S 
	  inner join old_spb_ForumSectionGroups G on S.GroupID=G.GroupID
	  left join spb_Groups on spb_Groups.[Announcement] = CAST(G.OwnerID as nvarchar(128)) and G.PresentAreaID=13
	  where G.PresentAreaID != 13 or spb_Groups.GroupId is not null

insert spb_BarSections([SectionId],[TenantTypeId],[OwnerId],[UserId],[Name]
	  ,[Description],[LogoImage],[IsEnabled],[EnableRss],[ThreadCategoryStatus],[AuditStatus]
	  ,[DisplayOrder],[DateCreated])
	  select 
	  T.SectionId,T.TenantTypeId,T.OwnerId,T.UserId,[SectionName]
	  ,S.[Description],[LogoUrl],[IsActive],[EnableRSS],[ThreadCategoryStatus],40
	  ,S.DisplayOrder,S.[DateCreated]
	  from old_spb_ForumSections S 
	  inner join old_spb_ForumSectionGroups G on S.GroupID=G.GroupID
	  inner join #TempSections T on T.OldSectionId = S.SectionID
	  where T.SectionId is not null and OldSectionId in(select MIN(OldSectionId) from #TempSections group by SectionId)

-----帖子
delete from spb_BarThreads
set identity_insert spb_BarThreads ON
insert spb_BarThreads ([ThreadId],[SectionId],[TenantTypeId],[OwnerId],[UserId]
,[Author],[Subject],[Body],[IsLocked],[IsEssential],[IsSticky],[StickyDate]
,[IsHidden],[HighlightStyle],[HighlightDate],[Price],[AuditStatus]
,[PostCount],[IP],[DateCreated],[LastModified])
 select old_spb_ForumThreads.ThreadID,T.SectionID,T.TenantTypeId,T.OwnerId,old_spb_ForumThreads.UserID
 ,old_spb_ForumThreads.Author,old_spb_ForumPosts.[Subject],old_spb_ForumPosts.Body,IsLocked,IsEssential,IsSticky,StickyDate
 ,IsHidden,'',HighlightDate,Price,(select AuditingStatus from old_spb_ForumPosts where ParentID= 0 and ThreadID =old_spb_ForumThreads.ThreadId),ReplyCount,
 old_spb_ForumPosts.UserHostAddress,old_spb_ForumThreads.PostDate,LastRepliedDate
 from old_spb_ForumThreads 
 left join old_spb_ForumPosts
 on old_spb_ForumThreads.ThreadID = old_spb_ForumPosts.ThreadID 	
inner join #TempSections T on T.OldSectionId = old_spb_ForumThreads.SectionID
 where old_spb_ForumPosts.ParentID=0 and old_spb_ForumPosts.[Subject] IS NOT NULL
 and old_spb_ForumPosts.Body IS NOT NULL and old_spb_ForumPosts.UserHostAddress IS NOT NULL
 
 set identity_insert spb_BarThreads OFF

-----回帖
delete from spb_BarPosts
set identity_insert spb_BarPosts ON
insert spb_BarPosts ([PostId],[SectionId],[TenantTypeId],[OwnerId],[ThreadId]
,[ParentId],[UserId],[Author],[Subject],[Body],[AuditStatus],[IP]
,[ChildPostCount],[DateCreated],[LastModified]) 
 select PostID,temp.SectionID,temp.TenantTypeId,temp.OwnerId,p.ThreadID
 ,case when (select ParentID from old_spb_ForumPosts where PostID= p.ParentID)=0 then 0 else isnull(p.ParentID,0) end,p.UserID,p.Author,Subject,Body,AuditingStatus,p.UserHostAddress
 ,0,p.PostDate,p.PostDate
 from old_spb_ForumPosts p 
 left join old_spb_ForumThreads t 
  on p.ThreadID = t.ThreadID
  inner join #TempSections temp on temp.OldSectionId = p.SectionID
 where p.ParentID>0 order by PostID asc
set identity_insert spb_BarPosts OFF

------删除临时表，并清除spb_Groups.Announcement(用此字段临时存储过旧群组Id)
drop Table #TempSections
update spb_Groups set Announcement = N''
 -----帖子正文中的附件地址
Declare @AttachmentID int
Declare @PostID int
Declare @FileName nvarchar(512)
Declare @ContentType nvarchar(64)
Declare @DateCreated datetime

Declare Cur Cursor For 
select AttachmentID,PostID,FileName,ContentType,DateCreated from old_spb_ForumPostAttachments
Open Cur 
Fetch next From Cur Into @AttachmentID,@PostID,@FileName,@ContentType,@DateCreated
While @@fetch_status=0 
Begin 
Declare @oldFilePath nvarchar(100),
		@newFilePath nvarchar(100),
		@ThreadId int,
		@AttachId bigint

set @ThreadId = 0								  
--查询旧回帖表，判断是否为主题帖
select 	@ThreadId = ThreadID from old_spb_ForumPosts where PostID=@PostID and ParentID = 0

set @oldFilePath = 'Services/ForumAttachment.ashx?AttachmentID='+CAST(@AttachmentID as nvarchar(100))
if(@ThreadId>0)
	begin
		if(charindex('image', @ContentType) > 0 )
		  begin
			UPDATE spb_BarThreads SET Body = REPLACE(Body,@oldFilePath+'" target="_blank"><img src="',@oldFilePath+'" rel="fancybox"><img src="') where ThreadId =@ThreadId
			set @newFilePath = 'Uploads/BarThread/'+CAST(YEAR(@DateCreated) as nvarchar(100)) +'/'+right ('0'+CAST(MONTH(@DateCreated) as varchar(100)),2)+'/'
											  +right ('0'+CAST(DAY(@DateCreated) as varchar(100)),2)+'/'+@FileName
		  end
		else
			 begin
				 set @AttachId = 0
				 select @AttachId=attachmentId from tn_Attachments where TenantTypeId=101202 and AssociateId=@ThreadId and FileName=@FileName 
				 set @newFilePath = 'Handlers/AttachmentAuthorize.ashx?tenantTypeId=101202&enableCaching=True&attachmentId='+CAST(@AttachId as nvarchar(100))
			 end

		UPDATE spb_BarThreads SET Body = REPLACE(Body,@oldFilePath,@newFilePath) where ThreadId =@ThreadId
	end
else
	begin
		if(charindex('image', @ContentType) > 0 )
		  begin
			UPDATE spb_BarPosts SET Body = REPLACE(Body,@oldFilePath+'" target="_blank"><img src="',@oldFilePath+'" rel="fancybox"><img src="') where PostId =@PostID
			set @newFilePath = 'Uploads/BarPost/'+CAST(YEAR(@DateCreated) as nvarchar(100)) +'/'+right ('0'+CAST(MONTH(@DateCreated) as varchar(100)),2)+'/'
											  +right ('0'+CAST(DAY(@DateCreated) as varchar(100)),2)+'/'+@FileName
		  end
		else
			 begin
				 set @AttachId = 0
				 select @AttachId=attachmentId from tn_Attachments where TenantTypeId=101203 and AssociateId=@PostID and FileName=@FileName 
				 set @newFilePath = 'Handlers/AttachmentAuthorize.ashx?tenantTypeId=101203&enableCaching=True&attachmentId='+CAST(@AttachId as nvarchar(100))
			 end

		UPDATE spb_BarPosts SET Body = REPLACE(Body,@oldFilePath,@newFilePath) where PostId =@PostID
	end

Fetch Next From Cur Into @AttachmentID,@PostID,@FileName,@ContentType,@DateCreated
End 
Close Cur 
Deallocate Cur