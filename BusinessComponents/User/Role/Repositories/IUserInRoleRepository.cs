//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// UserInRole数据访问接口
    /// </summary>
    public interface IUserInRoleRepository : IRepository<UserInRole>
    {

        /// <summary>
        /// 把用户加入到一组角色中
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleNames">赋予用户的用户角色</param>
        void AddUserToRoles(long userId, List<string> roleNames);


        /// <summary>
        /// 获取用户的角色名称
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回用户的所有角色，如果该用户没有用户角色返回空集合</returns>
        IEnumerable<string> GetRoleNamesOfUser(long userId);

        /// <summary>
        /// 移除用户的所有角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        void RemoveUserRoles(long userId);

        /// <summary>
        /// 删除用户的一个角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名</param>
        void Delete(long userId, string roleName);

    }
}
