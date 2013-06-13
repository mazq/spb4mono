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
    /// InvitationSettings管理器接口
    /// </summary>
    public interface IInvitationSettingsManager
    {
        /// <summary>
        /// 获取InvitationSettings
        /// </summary>
        /// <returns></returns>
        InvitationSettings Get();

        /// <summary>
        /// 保存InvitationSettings
        /// </summary>
        /// <returns></returns>
        void Save(InvitationSettings invitationSettings);
    }
}
