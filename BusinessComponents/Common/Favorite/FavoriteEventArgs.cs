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
    public class FavoriteEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="eventOperationType">事件操作类型</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">收藏用户Id</param>
        public FavoriteEventArgs(string eventOperationType, string tenantTypeId, long userId)
            : base(eventOperationType)
        {
            _tenantTypeId = tenantTypeId;
            _userId = userId;
        }

        private string _tenantTypeId;
        /// <summary>
        ///租户类型Id 
        /// </summary>
        public string TenantTypeId
        {
            get { return _tenantTypeId; }
        }

        private long _userId;
        /// <summary>
        ///收藏用户Id 
        /// </summary>
        public long UserId
        {
            get { return _userId; }
        }
    }
}