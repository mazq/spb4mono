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
    /// 处理群组申请通知的EventMoudle
    /// </summary>
    public class GroupMemberApplyEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册EventHandler
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<GroupMemberApply>.Instance().After += new CommonEventHandler<GroupMemberApply, CommonEventArgs>(GroupMemberApplyNoticeModule_After);
        }

        /// <summary>
        /// 通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void GroupMemberApplyNoticeModule_After(GroupMemberApply sender, CommonEventArgs eventArgs)
        {
            GroupService groupService = new GroupService();
            GroupEntity entity = groupService.Get(sender.GroupId);
            if (entity == null)
                return;

            User senderUser = DIContainer.Resolve<IUserService>().GetFullUser(sender.UserId);
            if (senderUser == null)
                return;
            InvitationService invitationService = new InvitationService();
            Invitation invitation;
            NoticeService noticeService = DIContainer.Resolve<NoticeService>();
            Notice notice;
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                if (sender.ApplyStatus == GroupMemberApplyStatus.Pending)
                {
                    List<long> toUserIds = new List<long>();
                    toUserIds.Add(entity.UserId);
                    toUserIds.AddRange(entity.GroupManagers.Select(n => n.UserId));
                    foreach (var toUserId in toUserIds)
                    {

                        //申请加入群组的请求
                        if (!groupService.IsMember(sender.GroupId, sender.UserId))
                        {
                            invitation = Invitation.New();
                            invitation.ApplicationId = GroupConfig.Instance().ApplicationId;
                            invitation.InvitationTypeKey = InvitationTypeKeys.Instance().ApplyJoinGroup();
                            invitation.UserId = toUserId;
                            invitation.SenderUserId = sender.UserId;
                            invitation.Sender = senderUser.DisplayName;
                            invitation.SenderUrl = SiteUrls.Instance().SpaceHome(sender.UserId);
                            invitation.RelativeObjectId = sender.GroupId;
                            invitation.RelativeObjectName = entity.GroupName;
                            invitation.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().GroupHome(entity.GroupKey));
                            invitation.Remark = string.Empty;
                            invitationService.Create(invitation);
                        }
                    }
                }
                return;
            }

            string noticeTemplateName = string.Empty;
            if (eventArgs.EventOperationType == EventOperationType.Instance().Approved())
            {
                if (sender.ApplyStatus == GroupMemberApplyStatus.Approved)
                {
                    noticeTemplateName = NoticeTemplateNames.Instance().MemberApplyApproved();
                }
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Disapproved())
            {
                if (sender.ApplyStatus == GroupMemberApplyStatus.Disapproved)
                {
                    noticeTemplateName = NoticeTemplateNames.Instance().MemberApplyDisapproved();
                }
            }

            if (string.IsNullOrEmpty(noticeTemplateName))
                return;

            notice = Notice.New();

            notice.UserId = sender.UserId;
            notice.ApplicationId = GroupConfig.Instance().ApplicationId;
            notice.TypeId = NoticeTypeIds.Instance().Hint();
            //notice.LeadingActorUserId = UserContext.CurrentUser.UserId;
            //notice.LeadingActor = UserContext.CurrentUser.DisplayName;
            //notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(UserContext.CurrentUser.UserId));
            notice.RelativeObjectId = sender.GroupId;
            notice.RelativeObjectName = StringUtility.Trim(entity.GroupName, 64);
            notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().GroupHome(entity.GroupKey));
            notice.TemplateName = noticeTemplateName;
            noticeService.Create(notice);
        }
    }
}