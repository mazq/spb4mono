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
    /// 用户角色
    /// </summary>
    [TableName("tn_Roles")]
    [PrimaryKey("RoleName", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class Role : IEntity
    {
        #region 需持久化属性

        private string roleName = string.Empty;
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private string friendlyRoleName = string.Empty;
        /// <summary>
        /// 角色友好名称（用于对外显示）
        /// </summary>
        public string FriendlyRoleName
        {
            get { return friendlyRoleName; }
            set { friendlyRoleName = value; }
        }

        private bool isBuiltIn;
        /// <summary>
        /// 是否是系统内置的
        /// </summary>
        public bool IsBuiltIn
        {
            get { return isBuiltIn; }
            set { isBuiltIn = value; }
        }

        private bool connectToUser;
        /// <summary>
        /// 是否直接关联到用户（例如：版主、注册用户 无需直接赋给用户）
        /// </summary>
        public bool ConnectToUser
        {
            get { return connectToUser; }
            set { connectToUser = value; }
        }

        private int applicationId;
        /// <summary>
        /// 哪个应用模块
        /// </summary>
        public int ApplicationId
        {
            get { return applicationId; }
            set { applicationId = value; }
        }

        private bool isPublic;
        /// <summary>
        /// 是否对外显示
        /// </summary>
        public bool IsPublic
        {
            get { return isPublic; }
            set { isPublic = value; }
        }

        private string description = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string roleImage = string.Empty;
        /// <summary>
        /// 角色标识图片名称
        /// </summary>
        public string RoleImage
        {
            get { return roleImage; }
            set { roleImage = value; }
        }

        private bool isEnabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        #endregion
        
        #region IEntity 成员

        object IEntity.EntityId { get { return this.RoleName; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
