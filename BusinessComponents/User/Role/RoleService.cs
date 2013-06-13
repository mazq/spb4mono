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
using Tunynet.Common.Repositories;
using Tunynet.Events;
using System.IO;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户角色业务逻辑类
    /// </summary>
    public class RoleService
    {

        private IRepository<Role> roleRepository;
        private IUserInRoleRepository userInRoleRepository;
        private LogoService logoService = new LogoService(TenantTypeIds.Instance().Role());

        /// <summary>
        /// 构造器
        /// </summary>
        public RoleService()
            : this(new Repository<Role>(), new UserInRoleRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="roleRepository">Role仓储</param>
        /// <param name="userInRoleRepository"><see cref="IUserInRoleRepository"/></param>
        public RoleService(IRepository<Role> roleRepository, IUserInRoleRepository userInRoleRepository)
        {
            this.roleRepository = roleRepository;
            this.userInRoleRepository = userInRoleRepository;
        }

        #region Role

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"><see cref="Role"/>要添加的角色</param>
        /// <param name="stream">输入流</param>
        public bool Create(Role role,Stream stream)
        {
            if (!roleRepository.Exists(role.RoleName))
            {
                EventBus<Role>.Instance().OnBefore(role, new CommonEventArgs(EventOperationType.Instance().Create()));
                role.RoleImage = logoService.UploadLogo(role.RoleName,stream);
                roleRepository.Insert(role);
                EventBus<Role>.Instance().OnAfter(role, new CommonEventArgs(EventOperationType.Instance().Create()));
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"><see cref="Role"/>要更新的角色</param>
        /// <param name="stream">输入流</param>
        public void Update(Role role,Stream stream)
        {
            if (roleRepository.Exists(role.RoleName))
            {
                EventBus<Role>.Instance().OnBefore(role, new CommonEventArgs(EventOperationType.Instance().Update()));
                role.RoleImage = logoService.UploadLogo(role.RoleName,stream);
                roleRepository.Update(role);
                EventBus<Role>.Instance().OnAfter(role, new CommonEventArgs(EventOperationType.Instance().Update()));
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        public void Delete(string roleName)
        {
            Role role = Get(roleName);
            EventBus<Role>.Instance().OnBefore(role, new CommonEventArgs(EventOperationType.Instance().Delete()));
            roleRepository.DeleteByEntityId(roleName);
            logoService.DeleteLogo(roleName);
            EventBus<Role>.Instance().OnAfter(role, new CommonEventArgs(EventOperationType.Instance().Delete()));

        }

        /// <summary>
        /// 获取Role
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns><see cref="Role"/></returns>
        public Role Get(string roleName)
        {
            return roleRepository.Get(roleName);
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <remarks>
        /// 按是否内置及角色名称排序
        /// </remarks>
        /// <returns>符合查询条件的Role集合</returns>
        public IEnumerable<Role> GetRoles()
        {
            return roleRepository.GetAll("IsBuiltIn desc,RoleName");
        }

        /// <summary>
        /// 根据条件获取Role
        /// </summary>
        /// <param name="connectToUser">是否可关联到用户</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="isEnabled">是否启用</param>
        /// <returns>符合查询条件的Role集合</returns>
        public IEnumerable<Role> GetRoles(bool? connectToUser, int? applicationId, bool? isEnabled)
        {
            return GetRoles().Where(n => (applicationId.HasValue ? n.ApplicationId == applicationId.Value : true)
                && (connectToUser.HasValue ? n.ConnectToUser == connectToUser.Value : true) && (isEnabled.HasValue ? n.IsEnabled == isEnabled.Value : true));
        }

        #endregion


        #region UsersInRoles

        /// <summary>
        /// 把用户加入到一组角色中
        /// </summary>  
        public void AddUserToRoles(long userId, List<string> roleNames)
        {
            if (roleNames == null)
                return;

            IEnumerable<string> oldRoleNames = GetRolesOfUser(userId).Select(n => n.RoleName);
            bool nameIsChange = false;
            //done:zhangp,by zhengw:以下代码有问题，当oldRoleNames.Count()为0时，不会为用户添加角色
            //foreach (var roleName in roleNames)
            //{
            //    if (oldRoleNames.Any(r => !r.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)))
            //        nameIsChange = true;                
            //}
            //以下为nameIsChange赋值的代码为郑伟添加
            nameIsChange = roleNames.Except(oldRoleNames).Count() > 0;

            if (nameIsChange)
            {
                userInRoleRepository.AddUserToRoles(userId, roleNames);
                List<UserInRole> newUsersInRoles = new List<UserInRole>();
                foreach (var r in roleNames)
                {
                    newUsersInRoles.Add(new UserInRole() { UserId = userId, RoleName = r });
                }
                EventBus<UserInRole>.Instance().OnBatchAfter(newUsersInRoles, new CommonEventArgs(EventOperationType.Instance().Update()));
            }
        }

        /// <summary>
        /// 给用户添加角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名称</param>
        public void AddUserToRole(long userId, string roleName)
        {
            Role role = Get(roleName);
            if (role != null)
            {
                if (!role.ConnectToUser)
                    return;

                UserInRole userInRole = new UserInRole()
                {
                    UserId = userId,
                    RoleName = role.RoleName
                };

                userInRoleRepository.Insert(userInRole);

                EventBus<UserInRole>.Instance().OnAfter(userInRole, new CommonEventArgs(EventOperationType.Instance().Create()));
            }
        }

        /// <summary>
        /// 移除用户的一个角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名称</param>
        public void RemoveUserFromRole(long userId, string roleName)
        {
            Role role = Get(roleName);
            if (role != null)
            {
                UserInRole userInRole = new UserInRole()
                {
                    UserId = userId,
                    RoleName = role.RoleName
                };

                userInRoleRepository.Delete(userId,roleName);

                EventBus<UserInRole>.Instance().OnAfter(userInRole, new CommonEventArgs(EventOperationType.Instance().Delete()));
            }
        }

        /// <summary>
        /// 移除用户的所有角色
        /// </summary>
        /// <remarks>
        /// 删除用户时使用
        /// </remarks>
        public void RemoveUserRoles(long userId)
        {
            userInRoleRepository.RemoveUserRoles(userId);
        }

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="onlyPublic">是否仅获取对外公开的角色</param>
        /// <returns>返回用户的所有角色，如果该用户没有用户角色返回空集合</returns>
        public IEnumerable<Role> GetRolesOfUser(long userId, bool onlyPublic = false)
        {
            IEnumerable<string> userRoleNames = userInRoleRepository.GetRoleNamesOfUser(userId);
            IEnumerable<Role> userRoles = roleRepository.PopulateEntitiesByEntityIds(userRoleNames);

            if (onlyPublic)
                return userRoles.Where(u => u.IsPublic);
            else
                return userRoles;
        }

        /// <summary>
        /// 判断UserId是否至少拥有roleNames的一个用户角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleNames">用户角色集合</param>
        /// <returns></returns>
        public bool IsUserInRoles(long userId, params string[] roleNames)
        {
            IEnumerable<string> userRoleNames = GetRolesOfUser(userId).Select(r => r.RoleName);

            if (userRoleNames.Any(r => roleNames.Contains(r)))
                return true;

            return false;
        }


        #endregion


    }
}
