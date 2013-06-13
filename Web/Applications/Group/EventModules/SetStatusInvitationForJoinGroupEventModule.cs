//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common;
using Tunynet;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 更改状态的时候触发事件
    /// </summary>
    public class SetStatusInvitationForJoinGroupEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<Invitation>.Instance().After += new CommonEventHandler<Invitation, CommonEventArgs>(SetStatusInvitationnForJoinGroupEventModule_After);
        }

        void SetStatusInvitationnForJoinGroupEventModule_After(Invitation sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                InvitationService invitationService = DIContainer.Resolve<InvitationService>();
                Invitation invitation = invitationService.Get(sender.Id);
                if (invitation != null && invitation.InvitationTypeKey == InvitationTypeKeys.Instance().InviteJoinGroup() && invitation.Status == InvitationStatus.Accept)
                {
                    GroupService groupService = new GroupService();
                    GroupMember member=GroupMember.New();
                    member.GroupId=sender.RelativeObjectId;
                    member.UserId = sender.UserId;
                    member.IsManager = false;
                    groupService.CreateGroupMember(member);
                }
                else if (invitation != null && invitation.InvitationTypeKey == InvitationTypeKeys.Instance().ApplyJoinGroup() && invitation.Status == InvitationStatus.Accept)
                {
                    GroupService groupService = new GroupService();
                    GroupMember member = GroupMember.New();
                    member.GroupId = sender.RelativeObjectId;
                    member.UserId = sender.SenderUserId;
                    member.IsManager = false;
                    groupService.CreateGroupMember(member);
                }
            }
        }
    }
}
