//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Common.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 权限管理服务类
    /// </summary>
    public class PermissionService
    {
        #region 构造器
        private IRepository<PermissionItem> permissionItemRepository;
        private IPermissionItemInUserRoleRepository permissionItemInUserRoleRepository;


        /// <summary>
        /// 构造器
        /// </summary>
        public PermissionService()
            : this(new Repository<PermissionItem>(), new PermissionItemInUserRoleRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="permissionItemRepository">PermissionItem仓储</param>
        /// <param name="permissionItemInUserRoleRepository"><see cref="IPermissionItemInUserRoleRepository"/></param>
        public PermissionService(IRepository<PermissionItem> permissionItemRepository, IPermissionItemInUserRoleRepository permissionItemInUserRoleRepository)
        {
            this.permissionItemRepository = permissionItemRepository;
            this.permissionItemInUserRoleRepository = permissionItemInUserRoleRepository;
        }
        #endregion


        #region 权限项目

        /// <summary>
        /// 获取权限项集合
        /// </summary>
        /// <param name="applicationId">应用程序ID</param>
        /// <returns>权限项集合</returns>
        public IEnumerable<PermissionItem> GetPermissionItems(int? applicationId)
        {
            IEnumerable<PermissionItem> allPermissionItems = permissionItemRepository.GetAll("DisplayOrder");
            if (applicationId.HasValue && applicationId.Value > 0)
                allPermissionItems = allPermissionItems.Where(n => n.ApplicationId == applicationId.Value);
            return allPermissionItems;
        }

        /// <summary>
        /// 获取PermissionItem
        /// </summary>
        /// <param name="itemKey">权限项标识</param>
        /// <returns></returns>
        public PermissionItem GetPermissionItem(string itemKey)
        {
            return permissionItemRepository.Get(itemKey);
        }

        /// <summary>
        /// 获取用户角色对应的权限设置
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>返回roleName对应的权限设置</returns>
        public IEnumerable<PermissionItemInUserRole> GetPermissionItemsInUserRole(string roleName)
        {
            return permissionItemInUserRoleRepository.GetPermissionItemsInUserRole(roleName);
        }

        /// <summary>
        /// 更新权限规则
        /// </summary>
        /// <param name="permissionItemInUserRoles">待更新的权限项目规则集合</param>
        public void UpdatePermissionItemInUserRole(IEnumerable<PermissionItemInUserRole> permissionItemInUserRoles)
        {
            permissionItemInUserRoleRepository.UpdatePermissionItemInUserRole(permissionItemInUserRoles);
            EventBus<PermissionItemInUserRole, CommonEventArgs>.Instance().OnBatchAfter(permissionItemInUserRoles, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        #endregion

        #region 获取用户权限

        /// <summary>
        /// 解析用户的权限规则用于权限验证
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public ResolvedUserPermission ResolveUserPermission(long userId)
        {
            string cacheKey = "ResolvedUserPermission:" + userId;

            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            ResolvedUserPermission resolvedUserPermission = cacheService.Get<ResolvedUserPermission>(cacheKey);

            if (resolvedUserPermission == null)
            {
                resolvedUserPermission = new ResolvedUserPermission();
                var user = DIContainer.Resolve<IUserService>().GetUser(userId);
                //匿名用户
                if (user == null)
                    return resolvedUserPermission;

                RoleService roleService = DIContainer.Resolve<RoleService>();
                IEnumerable<Role> userRoles = roleService.GetRolesOfUser(userId);
                IList<string> roleNamesOfUser = userRoles.Select(n => n.RoleName).ToList();
                roleNamesOfUser.Add(RoleNames.Instance().RegisteredUsers());
                if (user.IsModerated)
                    roleNamesOfUser.Add(RoleNames.Instance().ModeratedUser());

                foreach (var roleName in roleNamesOfUser)
                {
                    IEnumerable<PermissionItemInUserRole> permissionItemsInUserRole = GetPermissionItemsInUserRole(roleName);
                    foreach (var permissionItemInUserRole in permissionItemsInUserRole)
                    {
                        PermissionItem permissionItem = GetPermissionItem(permissionItemInUserRole.ItemKey);
                        if (permissionItem == null)
                            continue;
                        resolvedUserPermission.Merge(permissionItem, permissionItemInUserRole.PermissionType, permissionItemInUserRole.PermissionScope, permissionItemInUserRole.PermissionQuota);
                    }
                }
                cacheService.Add(cacheKey, resolvedUserPermission, CachingExpirationType.UsualObjectCollection);
            }
            return resolvedUserPermission;
        }

        #endregion

    }
}
