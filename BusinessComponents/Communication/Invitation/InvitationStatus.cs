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
    /// 请求处理状态
    /// </summary>
    public enum InvitationStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        Unhandled = 0,

        /// <summary>
        /// 接受
        /// </summary>
        Accept = 1,

        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse = 2
    }
}