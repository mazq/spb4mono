using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;

namespace Tunynet.Common
{
    public class SupportOpposeEventArgs : CommonEventArgs
    {
        public SupportOpposeEventArgs(string tenantTypeId, long userId,bool firstTime, string eventOperationType)
            : base(eventOperationType)
        {
            this.TenantTypeId = tenantTypeId;
            this.UserId = userId;
            this.FirstTime = firstTime;
        }

        /// <summary>
        /// 租户类型ID
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        /// 操作用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 是否第一次顶或踩的操作，用于区别是否反向操作后又正向操作
        /// </summary>
        public bool FirstTime { get; set; }
    }
}
