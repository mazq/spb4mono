//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using System.IO;
using Tunynet;
using Tunynet.Events;
using Spacebuilder.Common;

namespace Spacebuilder.Group
{
    
    
    /// <summary>
    /// 群组业务逻辑
    /// </summary>
    public class GroupService
    {
        private IGroupRepository groupRepository = null;
        private IGroupMemberRepository groupMemberRepository = null;
        private IGroupMemberApplyRepository groupMemberApplyRepository = null;
        private AuditService auditService = new AuditService();

        /// <summary>
        /// 构造函数
        /// </summary>
        public GroupService()
            : this(new GroupRepository(), new GroupMemberRepository(), new GroupMemberApplyRepository())
        {
        }
        
        
        /// <summary>
        /// 构造函数
        /// </summary>
        ///<param name="groupMemberApplyRepository">群组成员申请仓储</param>
        ///<param name="groupMemberRepository">群组成员仓储</param>
        ///<param name="groupRepository">群组仓储</param>
        /// <param name="GroupRepository"></param>
        public GroupService(IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository, IGroupMemberApplyRepository groupMemberApplyRepository)
        {
            this.groupRepository = groupRepository;
            this.groupMemberRepository = groupMemberRepository;
            this.groupMemberApplyRepository = groupMemberApplyRepository;
        }

        #region 维护群组
        
        

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="userId">当前操作人</param>
        /// <param name="group"><see cref="GroupEntity"/></param>
        /// <param name="logoFile">群组标识图</param>
        /// <returns>创建成功返回true，失败返回false</returns>
        public bool Create(long userId, GroupEntity group)
        {
            //设计要点
            //1、使用AuditService设置审核状态；
            //2、需要触发的事件参见《设计说明书-日志》     
            //3、单独调用标签服务设置标签
            //4、使用 IdGenerator.Next() 生成GroupId
            EventBus<GroupEntity>.Instance().OnBefore(group, new CommonEventArgs(EventOperationType.Instance().Create()));
            //设置审核状态
            auditService.ChangeAuditStatusForCreate(userId, group);
            long id = 0;
            long.TryParse(groupRepository.Insert(group).ToString(), out id);

            if (id > 0)
            {
                EventBus<GroupEntity>.Instance().OnAfter(group, new CommonEventArgs(EventOperationType.Instance().Create()));
                EventBus<GroupEntity, AuditEventArgs>.Instance().OnAfter(group, new AuditEventArgs(null, group.AuditStatus));
                //用户的创建群组数+1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(group.UserId, OwnerDataKeys.Instance().CreatedGroupCount(), 1);
            }
            return id > 0;
        }

        /// <summary>
        /// 更新群组
        /// </summary>
        /// <param name="userId">当前操作人</param>
        /// <param name="group"><see cref="GroupEntity"/></param>
        /// <param name="logoFile">群组标识图</param>
        public void Update(long userId, GroupEntity group)
        {
            EventBus<GroupEntity>.Instance().OnBefore(group, new CommonEventArgs(EventOperationType.Instance().Update()));
            auditService.ChangeAuditStatusForUpdate(userId, group);
            groupRepository.Update(group);
            
            

            EventBus<GroupEntity>.Instance().OnAfter(group, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 更新皮肤
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="isUseCustomStyle"></param>
        /// <param name="themeAppearance"></param>
        public void ChangeThemeAppearance(long groupId, bool isUseCustomStyle, string themeAppearance)
        {
            groupRepository.ChangeThemeAppearance(groupId, isUseCustomStyle, themeAppearance);
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        public void Delete(long groupId)
        {
            //设计要点
            //1、需要删除：群组成员、群组申请、Logo；
            GroupEntity group = groupRepository.Get(groupId);
            if (group == null)
                return;

            CategoryService categoryService = new CategoryService();
            categoryService.ClearCategoriesFromItem(groupId,null,TenantTypeIds.Instance().Group());         
            

            EventBus<GroupEntity>.Instance().OnBefore(group, new CommonEventArgs(EventOperationType.Instance().Delete()));
            int affectCount = groupRepository.Delete(group);
            if (affectCount > 0)
            {
                //删除访客记录
                new VisitService(TenantTypeIds.Instance().Group()).CleanByToObjectId(groupId);
                //用户的创建群组数-1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(group.UserId, OwnerDataKeys.Instance().CreatedGroupCount(), -1);
                //删除Logo             
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Group());
                logoService.DeleteLogo(groupId);
                //删除群组下的成员
                DeleteMembersByGroupId(groupId);
                EventBus<GroupEntity>.Instance().OnAfter(group, new CommonEventArgs(EventOperationType.Instance().Delete()));
                EventBus<GroupEntity, AuditEventArgs>.Instance().OnAfter(group, new AuditEventArgs(group.AuditStatus, null));
            }
        }

        /// <summary>
        /// 删除群组下的成员
        /// </summary>
        /// <param name="groupId"></param>
        public void DeleteMembersByGroupId(long groupId)
        {
            IEnumerable<GroupMember> groupMembers = groupMemberRepository.GetAllMembersOfGroup(groupId);
            foreach (var groupMember in groupMembers)
            {
                int affectCount = groupMemberRepository.Delete(groupMember);
                if (affectCount > 0)
                {
                    EventBus<GroupMember>.Instance().OnAfter(groupMember, new CommonEventArgs(EventOperationType.Instance().Delete()));
                    //用户的参与群组数-1
                    OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                    ownerDataService.Change(groupMember.UserId, OwnerDataKeys.Instance().JoinedGroupCount(), -1);
                }
            }
        }

        
        
        /// <summary>
        /// 发送加入群组邀请
        /// </summary>
        /// <param name="group"><see cref="GroupEntity"/></param>
        /// <param name="sender">发送人</param>
        /// <param name="userIds">邀请接收人</param>
        /// <param name="remark">附言</param>
        public void SendInvitations(GroupEntity group, IUser sender, string remark, IEnumerable<long> userIds)
        {
            //调用InvitationService的发送请求的方法
            InvitationService invitationService = new InvitationService();
            foreach (var userId in userIds)
            {
                if (!IsMember(group.GroupId, userId))
                {
                    Invitation invitation = Invitation.New();
                    invitation.ApplicationId = GroupConfig.Instance().ApplicationId;
                    invitation.InvitationTypeKey = InvitationTypeKeys.Instance().InviteJoinGroup();
                    invitation.UserId = userId;
                    invitation.SenderUserId = sender.UserId;
                    invitation.Sender = sender.DisplayName;
                    invitation.SenderUrl = SiteUrls.Instance().SpaceHome(sender.UserId);
                    invitation.RelativeObjectId = group.GroupId;
                    invitation.RelativeObjectName = group.GroupName;
                    invitation.RelativeObjectUrl = SiteUrls.Instance().GroupHome(group.GroupKey);
                    invitation.Remark = remark;
                    invitationService.Create(invitation);
                }
            }
        }

        /// <summary>
        /// 批准/不批准群组
        /// </summary>
        /// <param name="groupId">待被更新的群组Id</param>
        /// <param name="isApproved">是否通过审核</param>
        public void Approve(long groupId, bool isApproved)
        {
            //设计要点
            //1、审核状态未变化不用进行任何操作；
            //2、需要触发的事件参见《设计说明书-群组》；

            GroupEntity group = groupRepository.Get(groupId);

            AuditStatus newAuditStatus = isApproved ? AuditStatus.Success : AuditStatus.Fail;
            if (group.AuditStatus == newAuditStatus)
                return;

            AuditStatus oldAuditStatus = group.AuditStatus;
            group.AuditStatus = newAuditStatus;
            groupRepository.Update(group);

            string operationType = isApproved ? EventOperationType.Instance().Approved() : EventOperationType.Instance().Disapproved();
            EventBus<GroupEntity>.Instance().OnAfter(group, new CommonEventArgs(operationType));
            EventBus<GroupEntity, AuditEventArgs>.Instance().OnAfter(group, new AuditEventArgs(oldAuditStatus, newAuditStatus));
        }

        /// <summary>
        /// 批量批准/不批准群组
        /// </summary>
        /// <param name="groupIds">待处理的群组Id列表</param>
        /// <param name="isApproved">是否通过审核</param>
        public void Approve(IEnumerable<long> groupIds, bool isApproved)
        {
            //设计要点
            //1、审核状态未变化不用进行任何操作；
            //2、需要触发的事件参见《设计说明书-群组》；

            foreach (var threadId in groupIds)
            {
                Approve(threadId, isApproved);
            }
        }

        /// <summary>
        /// 更新群组公告
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="announcement">公告内容</param>
        public void UpdateAnnouncement(long groupId, string announcement)
        {
            GroupEntity group = groupRepository.Get(groupId);
            if (group == null)
                return;
            group.Announcement = announcement;
            groupRepository.Update(group);
        }


        /// <summary>
        /// 上传Logo
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="stream">Logo文件流</param>
        public void UploadLogo(long groupId, Stream stream)
        {
            //按现在设计应该用LogoService，但是感觉LogoService没什么必要（重构Logo/Image直连后再定）
            if (stream != null)
            {
                GroupEntity group = this.Get(groupId);
                LogoService logoService = new LogoService(TenantTypeIds.Instance().Group());
                group.Logo = logoService.UploadLogo(groupId, stream);
                groupRepository.Update(group);
                EventBus<GroupEntity>.Instance().OnAfter(group, new CommonEventArgs(EventOperationType.Instance().Update()));
            }
        }

        /// <summary>
        /// 删除Logo
        /// </summary>
        /// <param name="recommendId">群组Id</param>
        public void DeleteLogo(long groupId)
        {
            LogoService logoService = new LogoService(TenantTypeIds.Instance().Group());
            logoService.DeleteLogo(groupId);
            GroupEntity group = Get(groupId);
            if (group == null)
                return;
            group.Logo = string.Empty;
            groupRepository.Update(group);
        }

        #endregion


        #region 获取群组

        /// <summary>
        /// 通过GroupKey获取群组
        /// </summary>
        /// <param name="groupKey">群组标识</param>
        /// <returns></returns>
        public GroupEntity Get(string groupKey)
        {
            long groupId = GroupIdToGroupKeyDictionary.GetGroupId(groupKey);
            return this.Get(groupId);
        }

        /// <summary>
        /// 获取群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        public GroupEntity Get(long groupId)
        {
            return groupRepository.Get(groupId);
        }

        /// <summary>
        /// 获取前N个排行群组
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GetTops(int topNumber, string areaCode, long? categoryId, SortBy_Group sortBy)
        {
            //设计要点
            //1、查询areaCode时需要包含后代地区；
            //2、查询categoryId时需要包含后代类别；
            //3、无需维护缓存即时性
            return groupRepository.GetTops(topNumber, areaCode, categoryId, sortBy);
        }

        /// <summary>
        /// 获取匹配的前N个排行群组
        /// </summary>
        /// <param name="topNumber">前多少个</param>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GetMatchTops(int topNumber, string keyword, string areaCode, long? categoryId, SortBy_Group sortBy)
        {
            return groupRepository.GetMatchTops(topNumber, keyword, areaCode, categoryId, sortBy);
        }


        /// <summary>
        /// 分页获取排行数据
        /// </summary>
        /// <param name="areaCode">地区代码</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<GroupEntity> Gets(string areaCode, long? categoryId, SortBy_Group sortBy, int pageSize = 20, int pageIndex = 1)
        {
            //无需维护缓存即时性
            return groupRepository.Gets(areaCode, categoryId, sortBy, pageSize, pageIndex);
        }

        /// <summary>
        /// 根据标签名获取群组分页集合
        /// </summary>
        /// <param name="tagName">标签名</param></param>
        /// <param name="sortBy">排序依据</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>分页列表</returns>
        public PagingDataSet<GroupEntity> GetsByTag(string tagName, SortBy_Group sortBy, int pageSize = 20, int pageIndex = 1)
        {
            //无需维护缓存即时性
            return groupRepository.GetsByTag(tagName, sortBy, pageSize, pageIndex);
        }

        
        

        /// <summary>
        /// 获取用户创建的群组列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GetMyCreatedGroups(long userId, bool ignoreAudit)
        {
            //需维护缓存即时性
            return groupRepository.GetMyCreatedGroups(userId, ignoreAudit);
        }

        /// <summary>
        /// 获取用户加入的群组列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<GroupEntity> GetMyJoinedGroups(long userId, int pageSize = 20, int pageIndex = 1)
        {
            //需维护缓存即时性
            return groupRepository.GetMyJoinedGroups(userId, pageSize, pageIndex);
        }

        /// <summary>
        /// 群组成员也加入的群组
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GroupMemberAlsoJoinedGroups(long groupId, int topNumber)
        {
            //设计要点：
            //1、获取群组成员也加入的其他不重复的群组，按群组成长值倒序；
            //2、无需维护缓存即时性，缓存期限：常用集合
            return groupRepository.GroupMemberAlsoJoinedGroups(groupId, topNumber);
        }


        /// <summary>
        /// 获取我关注的用户加入的群组
        /// </summary>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="topNumber">获取前多少条</param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> FollowedUserAlsoJoinedGroups(long userId, int topNumber)
        {
            return groupRepository.FollowedUserAlsoJoinedGroups(userId, topNumber);
        }

        /// <summary>
        /// 分页获取群组后台管理列表
        /// </summary>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="categoryId">类别Id</param>
        /// <param name="keywords">名称关键字</param>
        /// <param name="ownerUserId">群主</param>
        /// <param name="minDateTime">创建时间下限值</param>
        /// <param name="maxDateTime">创建时间上限值</param>
        /// <param name="minMemberCount">成员数量下限值</param>
        /// <param name="maxMemberCount">成员数量上限值</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<GroupEntity> GetsForAdmin(AuditStatus? auditStatus, long? categoryId, string keywords, long? ownerUserId, DateTime? minDateTime, DateTime? maxDateTime, int? minMemberCount, int? maxMemberCount, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点
            //1、查询categoryId时需要包含后代类别；
            //2、不用缓存
            return groupRepository.GetsForAdmin(auditStatus, categoryId, keywords, ownerUserId, minDateTime, maxDateTime, minMemberCount, maxMemberCount, pageSize, pageIndex);
        }

        /// <summary>
        /// 根据群组ID集合获取群组集合
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public IEnumerable<GroupEntity> GetGroupEntitiesByIds(IEnumerable<long> groupIds)
        {
            return groupRepository.PopulateEntitiesByEntityIds(groupIds);
        }

        /// <summary>
        /// 根据审核状态获取群组数
        /// </summary>
        /// <param name="Pending">待审核</param>
        /// <param name="Again">需再审核</param>
        /// <returns></returns>
        public Dictionary<GroupManageableCountType, int> GetManageableCounts()
        {
            return groupRepository.GetManageableCounts();
        }

        #endregion


        #region 群组申请
        
        

        /// <summary>
        /// 用户是否申请过加入群组，并且申请处于待处理状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool IsApplied(long userId, long groupId)
        {
            var groupIds = groupMemberApplyRepository.GetPendingApplyGroupIdsOfUser(userId);
            return groupIds.Contains(groupId);
        }

        /// <summary>
        /// 申请加入群组
        /// </summary>
        /// <param name="groupMemberApply">群组加入申请</param>
        public void CreateGroupMemberApply(GroupMemberApply groupMemberApply)
        {
            //设计要点：
            //1、用户对同一个群组不允许有多个待处理的加入申请
            
            
            if (groupMemberApply == null)
                return;
            if (IsApplied(groupMemberApply.UserId, groupMemberApply.GroupId))
                return;
            long id = 0;
            long.TryParse(groupMemberApplyRepository.Insert(groupMemberApply).ToString(), out id);

            if (id > 0)
                EventBus<GroupMemberApply>.Instance().OnAfter(groupMemberApply, new CommonEventArgs(EventOperationType.Instance().Create()));
        }

        /// <summary>
        /// 接受/拒绝群组加入申请
        /// </summary>
        /// <param name="groupIds">申请Id列表</param>
        /// <param name="isApproved">是否接受</param>
        public void ApproveGroupMemberApply(IEnumerable<long> applyIds, bool isApproved)
        {
            //设计要点：
            //1、仅允许对待处理状态的申请变更状态；
            //2、通过批准的申请，直接创建GroupMember
            IEnumerable<GroupMemberApply> groupMemberApplies = groupMemberApplyRepository.PopulateEntitiesByEntityIds(applyIds);
            GroupMemberApplyStatus applyStatus = isApproved ? GroupMemberApplyStatus.Approved : GroupMemberApplyStatus.Disapproved;
            string operationType = isApproved ? EventOperationType.Instance().Approved() : EventOperationType.Instance().Disapproved();

            foreach (var apply in groupMemberApplies)
            {
                if (apply.ApplyStatus != GroupMemberApplyStatus.Pending)
                    continue;
                
                

                apply.ApplyStatus = applyStatus;
                groupMemberApplyRepository.Update(apply);
                
                

                EventBus<GroupMemberApply>.Instance().OnAfter(apply, new CommonEventArgs(operationType));

                if (isApproved)
                {
                    GroupMember member = GroupMember.New();
                    member.UserId = apply.UserId;
                    member.GroupId = apply.GroupId;
                    CreateGroupMember(member);
                }
            }
        }

        /// <summary>
        /// 删除群组加入申请
        /// </summary>
        /// <param name="id">申请Id</param>
        public void DeleteGroupMemberApply(long id)
        {
            GroupMemberApply apply = groupMemberApplyRepository.Get(id);
            if (apply == null)
                return;

            int affectCount = groupMemberApplyRepository.Delete(apply);
            if (affectCount > 0)
                EventBus<GroupMemberApply>.Instance().OnAfter(apply, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }
        
        
        /// <summary>
        /// 获取群组的加入申请列表
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="applyStatus">申请状态</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>加入申请分页数据</returns>
        public PagingDataSet<GroupMemberApply> GetGroupMemberApplies(long groupId, GroupMemberApplyStatus? applyStatus, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点：
            //1、排序：申请状态正序、申请时间倒序（Id代替）；            
            return groupMemberApplyRepository.GetGroupMemberApplies(groupId, applyStatus, pageSize, pageIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        public int GetMemberApplyCount(long groupId)
        {
            return groupMemberApplyRepository.GetMemberApplyCount(groupId);
        }

        #endregion


        #region 群组成员

        /// <summary>
        /// 增加群组成员
        /// </summary>
        /// <param name="groupMember"></param>
        public void CreateGroupMember(GroupMember groupMember)
        {
            //设计要点：
            //1、同一个群组不允许用户重复加入
            //2、群主不允许成为群组成员
            if (IsMember(groupMember.GroupId, groupMember.UserId))
                return;
            if (IsOwner(groupMember.GroupId, groupMember.UserId))
                return;
            long id = 0;
            long.TryParse(groupMemberRepository.Insert(groupMember).ToString(), out id);

            if (id > 0)
            {
                EventBus<GroupMember>.Instance().OnAfter(groupMember, new CommonEventArgs(EventOperationType.Instance().Create()));
                //用户的参与群组数+1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(groupMember.UserId, OwnerDataKeys.Instance().JoinedGroupCount(), 1);
            }
        }
        
        

        /// <summary>
        /// 移除群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">用户Id</param>
        public void DeleteGroupMember(long groupId, long userId)
        {
            
            

            GroupMember groupMember = groupMemberRepository.GetMember(groupId, userId);
            if (groupMember == null)
                return;

            int affectCount = groupMemberRepository.Delete(groupMember);
            if (affectCount > 0)
            {
                EventBus<GroupMember>.Instance().OnAfter(groupMember, new CommonEventArgs(EventOperationType.Instance().Delete()));
                //用户的参与群组数-1
                OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
                ownerDataService.Change(userId, OwnerDataKeys.Instance().JoinedGroupCount(), -1);
            }
        }

        /// <summary>
        /// 批量移除群组成员
        /// </summary>
        /// <param name="userIds">待处理的成员用户Id列表</param>
        /// <param name="groupId">群组Id</param>
        public void DeleteGroupMember(long groupId, IEnumerable<long> userIds)
        {

            foreach (var userId in userIds)
            {
                DeleteGroupMember(groupId, userId);
            }
        }


        /// <summary>
        /// 设置/取消 群组管理员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="isManager">是否管理员</param>
        public bool SetManager(long groupId, long userId, bool isManager)
        {
            //设计要点：
            //1、userId必须是groupId成员
            GroupMember member = groupMemberRepository.GetMember(groupId, userId);
            if (member == null)
                return false;
            if (member.IsManager == isManager)
                return false;
            member.IsManager = isManager;
            groupMemberRepository.Update(member);
            if (isManager)
            {
                EventBus<GroupMember>.Instance().OnAfter(member, new CommonEventArgs(EventOperationType.Instance().SetGroupManager()));
            }
            else
            {
                EventBus<GroupMember>.Instance().OnAfter(member, new CommonEventArgs(EventOperationType.Instance().CancelGroupManager()));
            }
            return true;
        }

        /// <summary>
        /// 更换群主
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="newOwnerUserId">新群主UserId</param>
        public void ChangeGroupOwner(long groupId, long newOwnerUserId)
        {
            //更换群主后，原群主转换成群组成员，如果新群主是群组成员则从成员中移除
            GroupEntity group = groupRepository.Get(groupId);
            long oldOwnerUserId = group.UserId;
            group.UserId = newOwnerUserId;
            groupRepository.ChangeGroupOwner(groupId, newOwnerUserId);

            //原群主的群组数-1，加入群组数+1
            OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
            ownerDataService.Change(oldOwnerUserId, OwnerDataKeys.Instance().CreatedGroupCount(), -1);
            ownerDataService.Change(oldOwnerUserId, OwnerDataKeys.Instance().JoinedGroupCount(), 1);

            //原群主转换成群组成员
            GroupMember groupMember = GroupMember.New();
            groupMember.GroupId = groupId;
            groupMember.UserId = oldOwnerUserId;
            groupMemberRepository.Insert(groupMember);

            //新群主的群组数+1,加入群组数-1
            ownerDataService.Change(newOwnerUserId, OwnerDataKeys.Instance().CreatedGroupCount(), 1);

            //如果新群主是群组成员则从成员中移除
            if (IsMember(groupId, newOwnerUserId))
                DeleteGroupMember(groupId, newOwnerUserId);
        }


        /// <summary>
        /// 是否群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>是群组成员返回true，否则返回false</returns>
        public bool IsMember(long groupId, long userId)
        {
            GroupMemberRole role = GetGroupMemberRole(groupId, userId);
            if (role >= GroupMemberRole.Member)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否群组管理员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>是群组管理员返回true，否则返回false</returns>
        public bool IsManager(long groupId, long userId)
        {
            GroupMemberRole role = GetGroupMemberRole(groupId, userId);
            if (role >= GroupMemberRole.Manager)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否群主
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>是群主返回true，否则返回false</returns>
        public bool IsOwner(long groupId, long userId)
        {
            GroupMemberRole role = GetGroupMemberRole(groupId, userId);
            if (role == GroupMemberRole.Owner)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检测用户在群组中属于什么角色
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns><see cref="GroupMemberRole"/></returns>
        private GroupMemberRole GetGroupMemberRole(long groupId, long userId)
        {
            //设计要点：
            //1、需要缓存，并维护缓存的即时性
            GroupMember member = groupMemberRepository.GetMember(groupId, userId);
            if (member == null)
            {
                GroupEntity group = groupRepository.Get(groupId);
                if (group.UserId == userId)
                    return GroupMemberRole.Owner;
                return GroupMemberRole.None;
            }
            if (member.IsManager)
                return GroupMemberRole.Manager;
            else
                return GroupMemberRole.Member;
        }

        /// <summary>
        /// 获取群组所有成员用户Id集合(用于推送动态）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfGroup(long groupId)
        {
            GroupEntity group = groupRepository.Get(groupId);
            if (group == null)
                return new List<long>();
            //不必缓存
            IEnumerable<long> userIds = groupMemberRepository.GetUserIdsOfGroup(groupId);
            var list = userIds.ToList();
            list.Add(group.UserId);
            return list;
        }

        /// <summary>
        /// 获取群组管理员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns>若没有找到，则返回空集合</returns>
        public IEnumerable<User> GetGroupManagers(long groupId)
        {
            //设计要点：
            //1、需要缓存，并维护缓存的即时性
            IEnumerable<long> manageIds = groupMemberRepository.GetGroupManagers(groupId);
            UserService userService = new UserService();
            return userService.GetFullUsers(manageIds);
        }

        /// <summary>
        /// 获取群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="hasManager">是否包含管理员</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>群组成员分页数据</returns>
        public PagingDataSet<GroupMember> GetGroupMembers(long groupId, bool hasManager = true, SortBy_GroupMember sortBy = SortBy_GroupMember.DateCreated_Asc, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点：
            //1、排序：管理员排在前，其余按加入时间正序；
            //2、使用分区列表缓存
            return groupMemberRepository.GetGroupMembers(groupId, hasManager, sortBy, pageSize, pageIndex);
        }

        /// <summary>
        /// 获取我关注的用户中同时加入某个群组的群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        public IEnumerable<GroupMember> GetGroupMembersAlsoIsMyFollowedUser(long groupId, long userId)
        {
            return groupMemberRepository.GetGroupMembersAlsoIsMyFollowedUser(groupId, userId);
        }
        /// <summary>
        /// 在线群组成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<GroupMember> GetOnlineGroupMembers(long groupId)
        {
            return groupMemberRepository.GetOnlineGroupMembers(groupId);
        }

        #endregion

        #region 删除用户数据
        
        

        /// <summary>
        /// 删除用户记录（删除用户时使用）
        /// </summary>
        /// <param name="userId">被删除用户</param>
        /// <param name="takeOverUserName">接管用户名</param>
        /// <param name="takeOverAll">是否接管被删除用户的所有内容</param>
        public void DeleteUser(long userId, string takeOverUserName, bool takeOverAll)
        {
            //设计要点：
            //1.利用sql转移给接管用户、删除群组成员、群组成员申请；
            //2.删除群组成员时，维护群组的成员数；

            //如果没设置由谁接管群组，就把群组转给网站初始管理员
            long takeOverUserId = 0;
            if (string.IsNullOrEmpty(takeOverUserName))
            {
                takeOverUserId = new SystemDataService().GetLong("Founder");
            }
            else
            {
                takeOverUserId = UserIdToUserNameDictionary.GetUserId(takeOverUserName);
            }


            IUserService userService = DIContainer.Resolve<IUserService>();
            User takeOver = userService.GetFullUser(takeOverUserId);
            groupRepository.DeleteUser(userId, takeOver, takeOverAll);
        }

        #endregion

        /// <summary>
        /// 获取群组管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public Dictionary<string, long> GetManageableDatas(string tenantTypeId = null)
        {
            return groupRepository.GetManageableDatas(tenantTypeId);
        }

        /// <summary>
        /// 获取群组统计数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public Dictionary<string, long> GetStatisticDatas(string tenantTypeId = null)
        {
            return groupRepository.GetStatisticDatas();
        }
    }
}
