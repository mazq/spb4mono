//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using Spacebuilder.Group;
using Tunynet.Globalization;
using Tunynet.Utilities;
using Spacebuilder.Common;
using System.Collections.Generic;
using System.Linq;
using Tunynet;

namespace Spacebuilder.Group.EventModules
{
    /// <summary>
    /// 处理群组成员退出群组通知的EventMoudle
    /// </summary>
    public class GroupMemberEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册EventHandler
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<GroupMember>.Instance().After += new CommonEventHandler<GroupMember, CommonEventArgs>(GroupMemberActivityModule_After);
            EventBus<GroupMember>.Instance().After += new CommonEventHandler<GroupMember, CommonEventArgs>(GroupMemberNoticeModule_After);
            EventBus<GroupMember>.Instance().After += new CommonEventHandler<GroupMember, CommonEventArgs>(SetManagerNoticeEventModule_After);
        }

        /// <summary>
        /// 动态处理程序
        /// </summary>
        /// <param name="groupMember"></param>
        /// <param name="eventArgs"></param>
        private void GroupMemberActivityModule_After(GroupMember groupMember, CommonEventArgs eventArgs)
        {
            ActivityService activityService = new ActivityService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                //生成动态
                if (groupMember == null)
                    return;
                var group = new GroupService().Get(groupMember.GroupId);
                if (group == null)
                    return;
                //生成Owner为群组的动态
                Activity actvityOfGroup = Activity.New();
                actvityOfGroup.ActivityItemKey = ActivityItemKeys.Instance().CreateGroupMember();
                actvityOfGroup.ApplicationId = GroupConfig.Instance().ApplicationId;
                actvityOfGroup.IsOriginalThread = true;
                actvityOfGroup.IsPrivate = !group.IsPublic;
                actvityOfGroup.UserId = groupMember.UserId;
                actvityOfGroup.ReferenceId = group.GroupId;
                actvityOfGroup.ReferenceTenantTypeId = TenantTypeIds.Instance().Group();
                actvityOfGroup.SourceId = groupMember.UserId;
                actvityOfGroup.TenantTypeId = TenantTypeIds.Instance().User();
                actvityOfGroup.OwnerId = group.GroupId;
                actvityOfGroup.OwnerName = group.GroupName;
                actvityOfGroup.OwnerType = ActivityOwnerTypes.Instance().Group();

                activityService.Generate(actvityOfGroup, false);

                //生成Owner为用户的动态
                Activity actvityOfUser = Activity.New();
                actvityOfUser.ActivityItemKey = ActivityItemKeys.Instance().JoinGroup();
                actvityOfUser.ApplicationId = actvityOfGroup.ApplicationId;
                actvityOfUser.HasImage = actvityOfGroup.HasImage;
                actvityOfUser.HasMusic = actvityOfGroup.HasMusic;
                actvityOfUser.HasVideo = actvityOfGroup.HasVideo;
                actvityOfUser.IsOriginalThread = actvityOfGroup.IsOriginalThread;
                actvityOfUser.IsPrivate = actvityOfGroup.IsPrivate;
                actvityOfUser.UserId = actvityOfGroup.UserId;
                actvityOfUser.ReferenceId = actvityOfGroup.ReferenceId;
                actvityOfGroup.ReferenceTenantTypeId = actvityOfGroup.ReferenceTenantTypeId;
                actvityOfUser.SourceId = actvityOfGroup.SourceId;

                actvityOfUser.TenantTypeId = actvityOfGroup.TenantTypeId;
                actvityOfUser.OwnerId = groupMember.UserId;
                actvityOfUser.OwnerName = groupMember.User.DisplayName;
                actvityOfUser.OwnerType = ActivityOwnerTypes.Instance().User();
                activityService.Generate(actvityOfUser, false);
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete()) //删除动态
            {
                activityService.DeleteSource(TenantTypeIds.Instance().User(), groupMember.UserId);
            }
        }


        /// <summary>
        /// 通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void GroupMemberNoticeModule_After(GroupMember sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType != EventOperationType.Instance().Delete() && eventArgs.EventOperationType != EventOperationType.Instance().Create() && sender!=null)
                return;
            GroupService groupService = new GroupService();
            GroupEntity entity = groupService.Get(sender.GroupId);
            if (entity == null)
                return;

            User senderUser = DIContainer.Resolve<IUserService>().GetFullUser(sender.UserId);
            if (senderUser == null)
                return;

            NoticeService noticeService = DIContainer.Resolve<NoticeService>();
            Notice notice;

            List<long> toUserIds = new List<long>();
            toUserIds.Add(entity.UserId);
            toUserIds.AddRange(entity.GroupManagers.Select(n => n.UserId));
            //删除群组成员通知群管理员
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                foreach (var toUserId in toUserIds)
                {
                    if (toUserId == sender.UserId)
                        continue;
                    notice = Notice.New();
                    notice.UserId = toUserId;
                    notice.ApplicationId = GroupConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActorUserId = sender.UserId;
                    notice.LeadingActor = senderUser.DisplayName;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                    notice.RelativeObjectId = sender.GroupId;
                    notice.RelativeObjectName = StringUtility.Trim(entity.GroupName, 64);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().GroupHome(entity.GroupKey));
                    notice.TemplateName = NoticeTemplateNames.Instance().MemberQuit();
                    noticeService.Create(notice);
                }
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Create()) //添加群成员时向群管理员发送通知
            {
                foreach (var toUserId in toUserIds)
                {
                    if (toUserId == sender.UserId)
                        continue;
                    notice = Notice.New();
                    notice.UserId = toUserId;
                    notice.ApplicationId = GroupConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActorUserId = sender.UserId;
                    notice.LeadingActor = senderUser.DisplayName;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                    notice.RelativeObjectId = sender.GroupId;
                    notice.RelativeObjectName = StringUtility.Trim(entity.GroupName, 64);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().GroupHome(entity.GroupKey));
                    notice.TemplateName = NoticeTemplateNames.Instance().MemberJoin();
                    noticeService.Create(notice);
                }
                //向加入者发送通知

                notice = Notice.New();
                notice.UserId = sender.UserId;
                notice.ApplicationId = GroupConfig.Instance().ApplicationId;
                notice.TypeId = NoticeTypeIds.Instance().Hint();
                notice.LeadingActorUserId = sender.UserId;
                notice.LeadingActor = senderUser.DisplayName;
                notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                notice.RelativeObjectId = sender.GroupId;
                notice.RelativeObjectName = StringUtility.Trim(entity.GroupName, 64);
                notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().GroupHome(entity.GroupKey));
                notice.TemplateName = NoticeTemplateNames.Instance().MemberApplyApproved();
                noticeService.Create(notice);
            }
        }

        /// <summary>
        /// 设置/取消管理员通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SetManagerNoticeEventModule_After(GroupMember sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType != EventOperationType.Instance().SetGroupManager() && eventArgs.EventOperationType != EventOperationType.Instance().CancelGroupManager())
                return;

            GroupService groupService = new GroupService();
            GroupEntity entity = groupService.Get(sender.GroupId);
            if (entity == null)
                return;

            User senderUser = DIContainer.Resolve<IUserService>().GetFullUser(sender.UserId);
            if (senderUser == null)
                return;

            NoticeService noticeService = DIContainer.Resolve<NoticeService>();

            Notice notice = Notice.New();
            notice.UserId = sender.UserId;
            notice.ApplicationId = GroupConfig.Instance().ApplicationId;
            notice.TypeId = NoticeTypeIds.Instance().Hint();
            notice.LeadingActorUserId = 0;
            notice.LeadingActor = string.Empty;
            notice.LeadingActorUrl = string.Empty;
            notice.RelativeObjectId = sender.GroupId;
            notice.RelativeObjectName = StringUtility.Trim(entity.GroupName, 64);
            notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().GroupHome(entity.GroupKey));

            if (eventArgs.EventOperationType == EventOperationType.Instance().SetGroupManager())
            {
                notice.TemplateName = NoticeTemplateNames.Instance().SetGroupManager();
            }
            else
            {
                notice.TemplateName = NoticeTemplateNames.Instance().CannelGroupManager();
            }
            noticeService.Create(notice);
        }
    }
}