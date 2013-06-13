//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using System.Collections.Generic;

namespace Tunynet.Common
{
    /// <summary>
    /// AtUser事件参数
    /// </summary>
    public class AtUserEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="associateSubject">关联项标题</param>
        public AtUserEventArgs(long userId, string tenantTypeId = "", long associateId = 0)
            : base(string.Empty)
        {
            this.userId = userId;
            this.tenantTypeId = tenantTypeId;
            this.associateId = associateId;
        }

        private long userId;
        /// <summary>
        /// 用户密码
        /// </summary>
        public long UserId
        {
            get { return userId; }
        }

        private string tenantTypeId;
        /// <summary>
        /// 租户类型
        /// </summary>
        public string TenantTypeId
        {
            get { return tenantTypeId; }
        }

        private long associateId;
        /// <summary>
        /// 关联项Id
        /// </summary>
        public long AssociateId
        {
            get { return associateId; }
        }
    }
}
