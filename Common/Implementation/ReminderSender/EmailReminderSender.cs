//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Tunynet;
using Tunynet.Common;
using Tunynet.Email;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 使用Email发送提醒
    /// </summary>
    public class EmailReminderSender : IReminderSender
    {
        /// <summary>
        /// 提醒方式Id
        /// </summary>
        public int ReminderModeId
        {
            get { return ReminderModeIds.Instance().Email(); }
        }

        /// <summary>
        /// 发送提醒
        /// </summary>
        /// <param name="userReminderInfos">用户提醒信息</param>
        public void SendReminder(IList<UserReminderInfo> userReminderInfos)
        {
            if (userReminderInfos == null || userReminderInfos.Count == 0)
                return;
            UserReminderInfo userReminderInfo = userReminderInfos.First();
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.GetUser(userReminderInfo.UserId);
            if (user != null)
            {
                EmailService emailService = new EmailService();
                emailService.Enqueue(EmailBuilder.Instance().Reminder(userReminderInfos, user));
            }
        }
    }
}
