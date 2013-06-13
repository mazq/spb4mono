//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邀请好友设置
    /// </summary>
    public class InviteFriendSettingsManager : IInviteFriendSettingsManager
    {
        private ISettingsRepository<InviteFriendSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public InviteFriendSettingsManager()
        {
            repository = new SettingsRepository<InviteFriendSettings>();
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        public InviteFriendSettings Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="inviteFriendSettings">设置</param>
        public void Save(InviteFriendSettings inviteFriendSettings)
        {
            repository.Save(inviteFriendSettings);
        }
    }
}
