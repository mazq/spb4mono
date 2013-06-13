//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Email;
using Tunynet.Events;

namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// User邮件处理
    /// </summary>
    public class UserEmailEventMoudle : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            //EventBus<User, CreateUserEventArgs>.Instance().After += new CommonEventHandler<User, CreateUserEventArgs>(UserEmailEventMoudle_After);
        }

        void UserEmailEventMoudle_After(User sender, CreateUserEventArgs eventArgs)
        {
            EmailService emailService = new EmailService();
            //发送注册成功邮件
            emailService.SendAsyn(EmailBuilder.Instance().RegisterSuccess(eventArgs.Password, sender));
        }
    }
}