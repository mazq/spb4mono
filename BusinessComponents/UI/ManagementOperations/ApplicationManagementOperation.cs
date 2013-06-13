//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.UI
{
    /// <summary>
    /// 应用的管理操作实体
    /// </summary>
    [TableName("tn_ApplicationManagementOperations")]
    [PrimaryKey("OperationId", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable, PropertyNamesOfArea = "PresentAreaKey")]
    [Serializable]
    public class ApplicationManagementOperation : IEntity
    {
        #region 需持久化属性
        /// <summary>
        ///OperationId
        /// </summary>
        public int OperationId { get; set; }

        /// <summary>
        ///ApplicationId
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 关联导航Id
        /// </summary>
        public int AssociatedNavigationId { get; set; }

        /// <summary>
        ///呈现区域标识
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        ///管理操作类型
        /// </summary>
        public ManagementOperationType OperationType { get; set; }

        /// <summary>
        ///操作的文字
        /// </summary>
        public string OperationText { get; set; }

        /// <summary>
        ///操作文字资源名称（如果同时设置OperationText则以OperationText优先）
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        ///导航url
        /// </summary>
        public string NavigationUrl { get; set; }

        /// <summary>
        ///导航路由规则名称
        /// </summary>
        public string UrlRouteName { get; set; }

        /// <summary>
        /// 路由数据名
        /// </summary>
        public string RouteDataName { get; set; }

        /// <summary>
        /// 内置图标名称
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        ///菜单文字旁边的图标url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///是新开窗口还是在当前窗口（默认:_self）
        /// </summary>
        public string NavigationTarget { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 仅拥有者可见
        /// </summary>
        public bool OnlyOwnerVisible { get; set; }

        /// <summary>
        ///是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.OperationId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
