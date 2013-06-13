//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Configuration;
using Tunynet;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    
    /// <summary>
    /// 对于用户的测试页面
    /// </summary>
    public class TestForUserEditModel
    {
        public TestForUserEditModel GetUserSettings()
        {
            //根据站点配置设置用户属性
            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();
            IInviteFriendSettingsManager inviteFriendSettingsManager = DIContainer.Resolve<IInviteFriendSettingsManager>();
            InviteFriendSettings inviteFriendSettings = inviteFriendSettingsManager.Get();
            return new TestForUserEditModel
            {
                registrationMode = userSettings.RegistrationMode,
                EnableNotActivatedUsersToLogin = userSettings.EnableNotActivatedUsersToLogin,
                MyHomePageAsSiteEntry = userSettings.MyHomePageAsSiteEntry,
                AllowInvitationCodeUseOnce = inviteFriendSettings.AllowInvitationCodeUseOnce,
                UserPasswordFormat = userSettings.UserPasswordFormat
            };
        }

        /// <summary>
        /// 密码加密方式
        /// </summary>
        public UserPasswordFormat UserPasswordFormat { get; set; }

        /// <summary>
        /// 注册方式
        /// </summary>
        public RegistrationMode registrationMode { get; set; }

        /// <summary>
        /// 系统的默认激活方式
        /// </summary>
        public AccountActivation accountActivation { get; set; }

        /// <summary>
        /// 是否允许未激活帐号登录
        /// </summary>
        public bool EnableNotActivatedUsersToLogin { get; set; }

        /// <summary>
        /// 是否以个人主页作为站点起始页
        /// </summary>
        public bool MyHomePageAsSiteEntry { get; set; }

        /// <summary>
        /// 邀请码是否允许只使用一次
        /// </summary>
        public bool AllowInvitationCodeUseOnce { get; set; }
    }
}
