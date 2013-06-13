//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 为IUser扩展与角色相关的功能
    /// </summary>
    public static class UserExtensionByRole
    {
        /// <summary>
        /// 判断用户是否至少含有requiredRoleNames的一个用户角色
        /// </summary>
        /// <param name="user"><see cref="IUser"/></param>
        /// <param name="requiredRoleNames">待检测用户角色集合</param>
        /// <returns></returns>
        public static bool IsInRoles(this IUser user, params string[] requiredRoleNames)
        {
            if (user == null)
                return false;

            for (int i = 0; i < requiredRoleNames.Length; i++)
            {
                requiredRoleNames[i] = requiredRoleNames[i].ToLower();
            }

            RoleService roleService = DIContainer.Resolve<RoleService>();
            IEnumerable<string> userRoleNames = roleService.GetRolesOfUser(user.UserId).Select(r => r.RoleName.ToLower());
            if (roleService == null)
                roleService = new RoleService();

            foreach (var roleName in userRoleNames)
            {
                if (requiredRoleNames.Contains(roleName))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断用户是否可以进入后台
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAllowEntryControlPannel(this IUser user)
        {
            if (user.IsInRoles(RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator()))
                return true;

            return user.IsInRoles(ApplicationAdministratorRoleNames.GetAll().ToArray());
        }
    }
}