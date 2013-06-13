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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 设置状态
    /// </summary>
    public class BatchSetStatusEditModel
    {
        /// <summary>
        /// 请求的id字符串
        /// </summary>
        public string invitations { get; set; }

        /// <summary>
        /// 请求的状态
        /// </summary>
        public InvitationStatus invitationStatus { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; }
    }
}
