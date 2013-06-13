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
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 权限项目关联设置的数据访问
    /// </summary>
    public class PermissionItemInUserRoleRepository : IPermissionItemInUserRoleRepository
    {
        /// <summary>
        /// 缓存设置
        /// </summary>
        protected static RealTimeCacheHelper RealTimeCacheHelper { get { return EntityData.ForType(typeof(PermissionItemInUserRole)).RealTimeCacheHelper; } }

        // 缓存服务
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 默认PetaPocoDatabase实例
        /// </summary>
        protected PetaPocoDatabase CreateDAO()
        {
            return PetaPocoDatabase.CreateInstance();
        }

        /// <summary>
        /// 更新权限项目设置
        /// </summary>
        /// <param name="permissionItemInUserRoles">待更新的权限项目规则集合</param>
        public void UpdatePermissionItemInUserRole(IEnumerable<PermissionItemInUserRole> permissionItemInUserRoles)
        {
            if (permissionItemInUserRoles == null)
                return;
            Database database = CreateDAO();
            database.OpenSharedConnection();

            List<Sql> sqls = new List<Sql>();
            foreach (var permissionItemInUserRole in permissionItemInUserRoles)
            {
                PermissionItemInUserRole tempPermissionItemInUserRole = null;
                var sql = Sql.Builder;
                sql.From("tn_PermissionItemsInUserRoles")
                   .Where("RoleName = @0 and ItemKey = @1", permissionItemInUserRole.RoleName, permissionItemInUserRole.ItemKey);
                //获取是否存在记录
                tempPermissionItemInUserRole = database.FirstOrDefault<PermissionItemInUserRole>(sql);

                //检测是否存在、锁定
                if (tempPermissionItemInUserRole != null)
                {
                    if (!tempPermissionItemInUserRole.IsLocked)
                    {
                        sqls.Add(Sql.Builder.Append(" update tn_PermissionItemsInUserRoles ")
                                            .Append(" set PermissionType = @0, PermissionQuota = @1, PermissionScope = @2, IsLocked= @3 ", permissionItemInUserRole.PermissionType, permissionItemInUserRole.PermissionQuota, permissionItemInUserRole.PermissionScope, permissionItemInUserRole.IsLocked)
                                            .Append(" where RoleName = @0 and ItemKey = @1", permissionItemInUserRole.RoleName, permissionItemInUserRole.ItemKey));
                    }
                }
                else
                {
                    sqls.Add(Sql.Builder.Append("INSERT INTO tn_PermissionItemsInUserRoles (RoleName, ItemKey, PermissionType, PermissionQuota, PermissionScope, IsLocked) VALUES (@0,@1,@2,@3,@4,@5)",
                                                 permissionItemInUserRole.RoleName,
                                                 permissionItemInUserRole.ItemKey,
                                                 permissionItemInUserRole.PermissionType,
                                                 permissionItemInUserRole.PermissionQuota,
                                                 permissionItemInUserRole.PermissionScope,
                                                 permissionItemInUserRole.IsLocked));
                }
            }

            database.Execute(sqls);

            database.CloseSharedConnection();

            IEnumerable<string> roleNames = permissionItemInUserRoles.Select(n => n.RoleName).Distinct();
            foreach (var roleName in roleNames)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("RoleName", roleName);
            }

            RealTimeCacheHelper.IncreaseGlobalVersion();
        }

        /// <summary>
        /// 获取用户角色对应的权限设置
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>返回roleName对应的权限设置</returns>
        public IEnumerable<PermissionItemInUserRole> GetPermissionItemsInUserRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return null;

            string cacheKey = GetCacheKey_GetPermissionItemsInUserRole(roleName);
            IEnumerable<PermissionItemInUserRole> permissionItemInUserRoles = cacheService.Get<IEnumerable<PermissionItemInUserRole>>(cacheKey);
            if (permissionItemInUserRoles == null)
            {
                var sql = Sql.Builder;
                sql.Where("RoleName = @0", roleName);
                permissionItemInUserRoles = CreateDAO().Fetch<PermissionItemInUserRole>(sql);
                cacheService.Add(cacheKey, permissionItemInUserRoles, CachingExpirationType.UsualObjectCollection);
            }
            return permissionItemInUserRoles;
        }

        /// <summary>
        /// 获取权限项目与角色关联 的CacheKey
        /// </summary>
        /// <param name="roleName">角色名</param>
        private string GetCacheKey_GetPermissionItemsInUserRole(string roleName)
        {
            string cacheKeyPrefix = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "RoleName", roleName);
            return string.Format("{0}::PermissionItemsInUserRole:RoleName:{1}", cacheKeyPrefix, roleName);
        }
    }
}