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
using Tunynet.Caching;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户角色与用户关联Repository
    /// </summary>
    public class UserInRoleRepository : Repository<UserInRole>, IUserInRoleRepository
    {
        //缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 把用户加入到一组角色中
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleNames">赋予用户的用户角色</param>
        public void AddUserToRoles(long userId, List<string> roleNames)
        {
            PetaPocoDatabase dao = CreateDAO();

            dao.OpenSharedConnection();
            RemoveUserRoles(userId);
            var sqlInsert = Sql.Builder;
            UserInRole userInRole = new UserInRole();
            userInRole.UserId = userId;
            foreach (var roleName in roleNames)
            {
                userInRole.RoleName = roleName;
                dao.Insert(userInRole);
            }
            dao.CloseSharedConnection();

            //增加版本
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 删除用户的一个角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleName">角色名</param>
        public void Delete(long userId, string roleName)
        {
            var sql = Sql.Builder;
            sql.Append("delete from tn_UsersInRoles")
                .Where("UserId= @0 and RoleName=@1", userId, roleName);
            CreateDAO().Execute(sql);

            //增加版本
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }


        /// <summary>
        /// 获取用户的角色名称
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户的所有角色，如果该用户没有用户角色返回空集合</returns>
        public IEnumerable<string> GetRoleNamesOfUser(long userId)
        {
            string cacheKeyUserInRole = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId);
            IEnumerable<UserInRole> userInRoles = cacheService.Get<IEnumerable<UserInRole>>(cacheKeyUserInRole);
          
            var sqlRole = Sql.Builder;
            if (userInRoles == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Id")
                    .From("tn_UsersInRoles")
                    .Where("UserId = @0", userId);
                List<long> userInRoleIds = CreateDAO().Fetch<long>(sql);
                userInRoles = PopulateEntitiesByEntityIds(userInRoleIds);
                cacheService.Add(cacheKeyUserInRole, userInRoles, CachingExpirationType.UsualObjectCollection);
            }
            return userInRoles.Select(u => u.RoleName);
        }

        /// <summary>
        /// 移除用户的所有角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void RemoveUserRoles(long userId)
        {
            var sqlDelete = Sql.Builder;
            sqlDelete.Append("Delete from tn_UsersInRoles where UserId = @0", userId);
            CreateDAO().Execute(sqlDelete);
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }
    }
}
