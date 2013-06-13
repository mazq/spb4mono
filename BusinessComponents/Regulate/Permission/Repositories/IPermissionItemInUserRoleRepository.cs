//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// PermissionItemInUserRole仓储接口
    /// </summary>
    public interface IPermissionItemInUserRoleRepository
    {
        /// <summary>
        /// 更新权限项目设置
        /// </summary>
        /// <param name="permissionItemInUserRoles">待更新的权限项目规则集合</param>
        void UpdatePermissionItemInUserRole(IEnumerable<PermissionItemInUserRole> permissionItemInUserRoles);

        /// <summary>
        /// 获取用户角色对应的权限设置
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>返回roleName对应的权限设置</returns>
        IEnumerable<PermissionItemInUserRole> GetPermissionItemsInUserRole(string roleName);
        
    }
}
