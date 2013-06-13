//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Tunynet;
using Tunynet.Common;
using Tunynet.Events;
using Tunynet.FileStore;
using Tunynet.Imaging;

namespace Tunynet.Common
{
    /// <summary>
    /// 审核变化自定义事件
    /// </summary>
    public class AuditEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldAuditStatus">变化前的审核状态</param>
        /// <param name="newAuditStatus">变化后的审核状态</param>
        public AuditEventArgs(AuditStatus? oldAuditStatus, AuditStatus? newAuditStatus)
            : base(string.Empty)
        {
            this.oldAuditStatus = oldAuditStatus;
            this.newAuditStatus = newAuditStatus;
        }

        private AuditStatus? oldAuditStatus;

        /// <summary>
        /// 旧审核状态
        /// </summary>
        public AuditStatus? OldAuditStatus
        {
            get { return oldAuditStatus; }
        }
        private AuditStatus? newAuditStatus;

        /// <summary>
        /// 新审核状态
        /// </summary>
        public AuditStatus? NewAuditStatus
        {
            get { return newAuditStatus; }
        }
    }
}