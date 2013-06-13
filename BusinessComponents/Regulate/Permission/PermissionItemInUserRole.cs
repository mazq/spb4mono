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
namespace Tunynet.Common
{
    /// <summary>
    /// 权限项目与角色关联
    /// </summary>
    [TableName("tn_PermissionItemsInUserRoles")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "RoleName")]
    [Serializable]
    public class PermissionItemInUserRole : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static PermissionItemInUserRole New()
        {
            PermissionItemInUserRole permissionItemsInUserRole = new PermissionItemInUserRole()
            {
                ItemKey = string.Empty,
                RoleName = string.Empty
            };
            return permissionItemsInUserRole;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        ///权限项目标识
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///权限设置类型
        /// </summary>
        public PermissionType PermissionType { get; set; }

        /// <summary>
        ///允许的权限额度
        /// </summary>
        public float PermissionQuota { get; set; }

        /// <summary>
        ///允许的权限范围
        /// </summary>
        public PermissionScope PermissionScope { get; set; }

        /// <summary>
        ///是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
