//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 标签自定义事件
    /// </summary>
    public class TagEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventOperationType">时间操作类型</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public TagEventArgs(string eventOperationType, string tenantTypeId)
            : base(string.Empty)
        {
            _tenantTypeId = tenantTypeId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventOperationType">时间操作类型</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="itemId">标签成员Id</param>
        public TagEventArgs(string eventOperationType, string tenantTypeId, long itemId)
            : base(eventOperationType)
        {
            _tenantTypeId = tenantTypeId;
            _itemId = itemId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventOperationType">时间操作类型</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="tagName">标签名</param>
        public TagEventArgs(string eventOperationType, string tenantTypeId, string tagName)
            : base(eventOperationType)
        {
            _tenantTypeId = tenantTypeId;
            _tagName = tagName;
        }


        private string _tenantTypeId;
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return _tenantTypeId; }
        }

        private long _itemId;
        /// <summary>
        /// 标签成员Id
        /// </summary>
        public long ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        private string _tagName;
        /// <summary>
        /// 标签名
        /// </summary>
        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
        }

    }
}