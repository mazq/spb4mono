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
    /// 审核项目与角色实体类
    /// </summary>
    [TableName("tn_AuditItemsInUserRoles")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class AuditItemInUserRole : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static AuditItemInUserRole New()
        {
            AuditItemInUserRole auditItemInUserRole = new AuditItemInUserRole()
            {
                ItemKey = string.Empty,
                RoleName = string.Empty
            };
            return auditItemInUserRole;
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
        ///审核项目标识
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///严格程度
        /// </summary>
        public AuditStrictDegree StrictDegree { get; set; }

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
