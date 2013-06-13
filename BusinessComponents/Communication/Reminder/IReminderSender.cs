//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 发送提醒接口
    /// </summary>
    public interface IReminderSender
    {
        /// <summary>
        /// 提醒方式Id
        /// </summary>
        int ReminderModeId { get; }

        /// <summary>
        /// 发送提醒
        /// </summary>
        /// <param name="userReminderInfos">用户提醒信息集合</param>
        void SendReminder(IList<UserReminderInfo> userReminderInfos);
    }
}
