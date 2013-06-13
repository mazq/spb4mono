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
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户角色Repository
    /// </summary>
    public class RoleRepository : Repository<Role>
    {
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="role">待删除的用户角色</param>
        /// <returns>删除成功返回1，否则返回0</returns>
        public override int Delete(Role role)
        {
            PetaPocoDatabase dao = CreateDAO();

            if (role == null)
                return 0;

            var sql = Sql.Builder;
            sql.Select("Id")
                .From("tn_UsersInRoles")
                .Where("RoleName = @0", role.RoleName);
            IEnumerable<long> userInRoleIds = dao.FetchFirstColumn(sql).Cast<long>();

            var sqlUserInRolesDelete = Sql.Builder;
            var sqlRolesDelete = Sql.Builder;
            int count;
            sqlUserInRolesDelete.Append("Delete from tn_UsersInRoles where RoleName = @0", role.RoleName);
            sqlRolesDelete.Append("Delete from tn_Roles where RoleName =@0", role.RoleName);
            base.OnDeleted(Get(role.RoleName));
            using (var scope = dao.GetTransaction())
            {
                dao.Execute(sqlUserInRolesDelete);
                count = dao.Execute(sqlRolesDelete);
                scope.Complete();
            }
            OnDeleted(role);
            
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            Caching.RealTimeCacheHelper userInRoleCacheHelper = EntityData.ForType(typeof(UserInRole)).RealTimeCacheHelper;
            foreach (var userInRoleId in userInRoleIds)
            {
                UserInRole userInRole = cacheService.Get<UserInRole>(userInRoleCacheHelper.GetCacheKeyOfEntity(userInRoleId));
                if (userInRole != null)
                    userInRoleCacheHelper.MarkDeletion(userInRole);
            }            

            return count;
        }

        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="role">待更新的用户角色</param>
        public override void Update(Role role)
        {
            var sql = Sql.Builder;
            sql.Append("Update tn_Roles set FriendlyRoleName = @0,IsPublic = @1,Description = @2, ImageName = @3, IsEnabled = @4  where RoleName = @5", role.FriendlyRoleName, role.IsPublic, role.Description, role.RoleImage, role.IsEnabled, role.RoleName);
            CreateDAO().Execute(sql);

            role = Get(role.RoleName);
            base.OnUpdated(role);
        }

    }
}
