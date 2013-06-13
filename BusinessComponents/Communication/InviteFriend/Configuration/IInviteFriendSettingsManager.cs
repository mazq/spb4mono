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
    /// InviteFriendSettings管理器接口
    /// </summary>
    public interface IInviteFriendSettingsManager
    {
        /// <summary>
        /// 获取InviteFriendSettings
        /// </summary>
        /// <returns></returns>
        InviteFriendSettings Get();

        /// <summary>
        /// 保存inviteFriendSettings
        /// </summary>
        /// <param name="inviteFriendSettings"></param>
        void Save(InviteFriendSettings inviteFriendSettings);

    }
}
