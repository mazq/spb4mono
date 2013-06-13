//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// RoleName配置类（用于强类型获取RoleName）
    /// </summary>
    public class RoleNames
    {
        #region Instance
        private static RoleNames _instance = new RoleNames();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static RoleNames Instance()
        {
            return _instance;
        }

        private RoleNames()
        { }
        #endregion

        /// <summary>
        /// 超级管理员
        /// </summary>
        public string SuperAdministrator()
        {
            return "SuperAdministrator";
        }

        /// <summary>
        /// 内容管理员
        /// </summary>
        public string ContentAdministrator()
        {
            return "ContentAdministrator";
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        public string RegisteredUsers()
        {
            return "RegisteredUsers";
        }

        /// <summary>
        /// 管制用户
        /// </summary>
        public string ModeratedUser()
        {
            return "ModeratedUser";
        }

        /// <summary>
        /// 匿名用户
        /// </summary>
        public string Anonymous()
        {
            return "Anonymous";
        }

        /// <summary>
        /// 拥有者
        /// </summary>
        public string Owner()
        {
            return "Owner";
        }

        /// <summary>
        /// 版主
        /// </summary>
        public string Moderator()
        {
            return "Moderator";
        }

        /// <summary>
        /// 组织成员
        /// </summary>
        public string OrganizationMember()
        {
            return "OrganizationMember";
        }

        /// <summary>
        /// 组织管理人
        /// </summary>
        public string OrganizationManager()
        {
            return "OrganizationManager";
        }

    }
}
