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
using Tunynet.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 创建邀请记录的时候事件
    /// </summary>
    public class CreateInviteFriendRecordEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册创建邀请记录的方法
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<InviteFriendRecord>.Instance().After += new CommonEventHandler<InviteFriendRecord, CommonEventArgs>(CreateInviteFriendRecordEventModule_After);
        }

        /// <summary>
        /// 创建邀请记录之后的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void CreateInviteFriendRecordEventModule_After(InviteFriendRecord sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                PointService pointService = new PointService();
                string userName = UserIdToUserNameDictionary.GetUserName(sender.UserId);
                string invitedName = UserIdToUserNameDictionary.GetUserName(sender.InvitedUserId);
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_CreateInviteFriendRecord"), userName, invitedName);
                pointService.GenerateByRole(sender.UserId, PointItemKeys.Instance().InviteUserRegister(), description);
            }
        }
    }
}
