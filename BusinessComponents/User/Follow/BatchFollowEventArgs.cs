//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 收藏自定义事件
    /// </summary>
    public class BatchFollowEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="eventOperationType">事件操作类型</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">收藏用户Id</param>
        public BatchFollowEventArgs(string eventOperationType, long userId)
            : base(eventOperationType)
        {           
            _userId = userId;
        }

        private long _userId;
        /// <summary>
        ///用户Id 
        /// </summary>
        public long UserId
        {
            get { return _userId; }
        }
    }
}