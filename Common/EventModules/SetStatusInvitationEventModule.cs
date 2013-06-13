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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 更改状态的时候触发事件
    /// </summary>
    public class SetStatusInvitationEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<Invitation>.Instance().After += new CommonEventHandler<Invitation, CommonEventArgs>(SetStatusInvitationEventModule_After);
        }

        void SetStatusInvitationEventModule_After(Invitation sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                InvitationService invitationService = DIContainer.Resolve<InvitationService>();
                Invitation invitation = invitationService.Get(sender.Id);
                if (invitation != null && invitation.InvitationTypeKey == InvitationTypeKeys.Instance().InviteFollow() && invitation.Status == InvitationStatus.Accept)
                {
                    FollowService followService = new FollowService();
                    followService.Follow(invitation.UserId, invitation.SenderUserId);
                }
            }
        }
    }
}
