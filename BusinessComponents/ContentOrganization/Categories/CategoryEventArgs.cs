//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 分类自定义事件
    /// </summary>
    public class CategoryEventArgs : CommonEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventOperationType">时间操作类型</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public CategoryEventArgs(string eventOperationType, string tenantTypeId)
            : base(string.Empty)
        {
            _tenantTypeId = tenantTypeId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventOperationType">时间操作类型</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="itemId">分类成员Id</param>
        public CategoryEventArgs(string eventOperationType, string tenantTypeId, long itemId)
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
        /// <param name="tagName">分类名</param>
        public CategoryEventArgs(string eventOperationType, string tenantTypeId, string categoryName)
            : base(eventOperationType)
        {
            _tenantTypeId = tenantTypeId;
            _categoryName = categoryName;
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
        /// 分类成员Id
        /// </summary>
        public long ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        private string _categoryName;
        /// <summary>
        /// 分类名
        /// </summary>
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

    }
}