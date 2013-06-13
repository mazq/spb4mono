 ----群组
delete from spb_Groups
insert spb_Groups ([GroupId],[GroupName],[GroupKey],[Description],[AreaCode]
,[UserId],[Logo],[IsPublic],[JoinWay],[EnableMemberInvite],[AuditStatus]
,[MemberCount],[GrowthValue],[ThemeAppearance],[DateCreated],[IP]
,[Announcement],[IsUseCustomStyle]) 
select CONVERT(char(6),GETDATE(),12) + RIGHT(10000000000 + CONVERT(bigint, ABS(CHECKSUM(NEWID()))),10),ClubName,substring(DomainName,0,15),N'',AreaCode
,ClubOwnerUserID,CASE WHEN old_spb_Clubs.HasLogo=1 THEN 'GroupLogo/'+dbo.spb_GetAvatarPath(old_spb_Clubs.ClubID)+'.jpg' ELSE '' END,IsPublic,CASE WHEN IsPublic=0 THEN 2 WHEN IsMemberNeedAuthorize=1 THEN 1 ELSE 0 END,1,AuditingStatus
,MemberCount,Points,N'',DateCreated,UserHostAddress
,cast(ClubID as nvarchar(128)),0
from old_spb_Clubs

 ----成员申请
delete from spb_GroupMemberApplies
insert spb_GroupMemberApplies ([GroupId],[UserId],[ApplyReason],[ApplyStatus],[ApplyDate]) 
 select spb_Groups.GroupId,old_spb_ClubMembers.UserID,old_spb_ClubMembers.ApplyRemark,CASE WHEN Status=11 OR Status=21 THEN 2 ELSE Status END,GETDATE()
 from old_spb_ClubMembers inner join spb_Groups on spb_Groups.[Announcement] = CAST(old_spb_ClubMembers.ClubID as nvarchar(128)) 
 where old_spb_ClubMembers.Status <> 11 and old_spb_ClubMembers.Status <> 21
  
-----群组成员
delete from spb_GroupMembers
insert spb_GroupMembers ([GroupId],[UserId],[IsManager],[JoinDate]) 
select spb_Groups.GroupId,a.UserID,CASE WHEN a.ClubMemberRole= 2 THEN 1 ELSE 0 END,JoinDate
from old_spb_ClubMembers a inner join spb_Groups  on spb_Groups.[Announcement] = CAST(a.ClubID as nvarchar(128)) 
where a.UserID != spb_Groups.UserId

-----用户加入的群组数
delete from tn_OwnerData where DataKey='GroupJoinedGroupCount'
INSERT INTO tn_OwnerData([OwnerId],[TenantTypeId],[DataKey],[LongValue],[DecimalValue] ,[StringValue])
     select UserId,'000011','GroupJoinedGroupCount',COUNT(GroupId),0,''
	 from spb_GroupMembers
	 group by UserId

-----用户创建的群组数
delete from tn_OwnerData where DataKey='Group-ThreadCount'
INSERT INTO tn_OwnerData([OwnerId],[TenantTypeId],[DataKey],[LongValue],[DecimalValue] ,[StringValue])
     select UserId,'000011','Group-ThreadCount',COUNT(GroupId),0,''
	 from spb_Groups
	 group by UserId

-----群组导航
delete from tn_ApplicationInPresentAreaInstallations where PresentAreaKey = N'GroupSpace'
insert tn_ApplicationInPresentAreaInstallations ([OwnerId],[ApplicationId],[PresentAreaKey])
select spb_Groups.GroupId,ApplicationId,PresentAreaKey
from tn_ApplicationInPresentAreaSettings,spb_Groups 
where PresentAreaKey = N'GroupSpace'

delete from tn_PresentAreaNavigations where PresentAreaKey = N'GroupSpace'
insert [tn_PresentAreaNavigations] ([NavigationId],[ParentNavigationId],[Depth]
	   ,[PresentAreaKey],[ApplicationId],[OwnerId],[NavigationType],[NavigationText]
	   ,[ResourceName],[NavigationUrl],[UrlRouteName],[RouteDataName],[IconName],[ImageUrl]
       ,[NavigationTarget],[DisplayOrder],[OnlyOwnerVisible],[IsLocked],[IsEnabled])
select [NavigationId],[ParentNavigationId],[Depth],[PresentAreaKey],[ApplicationId],spb_Groups.GroupId
       ,[NavigationType],[NavigationText],[ResourceName],[NavigationUrl],[UrlRouteName],[RouteDataName]
       ,[IconName],[ImageUrl],[NavigationTarget],[DisplayOrder],[OnlyOwnerVisible],[IsLocked]
       ,[IsEnabled]
from tn_InitialNavigations,spb_Groups 
where PresentAreaKey = N'GroupSpace'

--修复群组下的群组成员数
update spb_Groups set MemberCount = (select COUNT(*) from spb_GroupMembers where spb_GroupMembers.GroupId = spb_Groups.GroupId)