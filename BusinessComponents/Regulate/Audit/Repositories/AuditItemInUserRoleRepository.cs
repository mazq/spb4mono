//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 审核设置数据访问
    /// </summary>
    public class AuditItemInUserRoleRepository : IAuditItemInUserRoleRepository
    {
        /// <summary>
        /// 缓存设置
        /// </summary>
        protected static RealTimeCacheHelper RealTimeCacheHelper { get { return EntityData.ForType(typeof(AuditItemInUserRole)).RealTimeCacheHelper; } }

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
        /// 获取用户角色对应的审核设置
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>返回roleName对应的审核设置</returns>
        public IEnumerable<AuditItemInUserRole> GetAuditItemsInUserRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return null;

            string cacheKey = GetCacheKey_GetAuditItemsInUserRole(roleName);
            IEnumerable<AuditItemInUserRole> auditItemInUserRoles = cacheService.Get<IEnumerable<AuditItemInUserRole>>(cacheKey);
            if (auditItemInUserRoles == null)
            {
                var sql = Sql.Builder;
                sql.Where("RoleName = @0", roleName);
                auditItemInUserRoles = CreateDAO().Fetch<AuditItemInUserRole>(sql);
                cacheService.Add(cacheKey, auditItemInUserRoles, CachingExpirationType.UsualObjectCollection);
            }
            return auditItemInUserRoles;
        }

        /// <summary>
        /// 更新审核项目设置
        /// </summary>
        /// <param name="auditItemInUserRoles">待更新的审核项目规则集合</param>
        public void UpdateAuditItemInUserRole(IEnumerable<AuditItemInUserRole> auditItemInUserRoles)
        {
            if (auditItemInUserRoles == null)
                return;
            Database database = CreateDAO();
            database.OpenSharedConnection();

            List<Sql> sqls = new List<Sql>();

            foreach (var auditItemInUserRole in auditItemInUserRoles)
            {
                AuditItemInUserRole tempAuditItemInUserRole = null;

                var sql = Sql.Builder;
                sql.From("tn_AuditItemsInUserRoles")
                   .Where("RoleName = @0 and ItemKey = @1", auditItemInUserRole.RoleName, auditItemInUserRole.ItemKey);

                //获取是否存在记录
                tempAuditItemInUserRole = database.FirstOrDefault<AuditItemInUserRole>(sql);

                //检测是否存在、锁定
                if (tempAuditItemInUserRole != null)
                {
                    if (!tempAuditItemInUserRole.IsLocked)
                    {
                        sqls.Add(Sql.Builder.Append(" update tn_AuditItemsInUserRoles ")
                                            .Append(" set StrictDegree = @0, IsLocked= @1 ", auditItemInUserRole.StrictDegree, auditItemInUserRole.IsLocked)
                                            .Append(" where RoleName = @0 and ItemKey = @1", auditItemInUserRole.RoleName, auditItemInUserRole.ItemKey));
                    }
                }
                else
                {
                    sqls.Add(Sql.Builder.Append("INSERT INTO tn_AuditItemsInUserRoles (RoleName, ItemKey, StrictDegree, IsLocked) VALUES (@0,@1,@2,@3)",
                                                 auditItemInUserRole.RoleName,
                                                 auditItemInUserRole.ItemKey,
                                                 auditItemInUserRole.StrictDegree,
                                                 auditItemInUserRole.IsLocked));
                }
            }



            database.Execute(sqls);

            database.CloseSharedConnection();

            IEnumerable<string> roleNames = auditItemInUserRoles.Select(n => n.RoleName).Distinct();
            foreach (var roleName in roleNames)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("RoleName", roleName);
            }

            RealTimeCacheHelper.IncreaseGlobalVersion();

        }

        /// <summary>
        /// 获取审核项目与角色关联 的CacheKey
        /// </summary>
        /// <param name="roleName">角色名</param>
        private string GetCacheKey_GetAuditItemsInUserRole(string roleName)
        {
            string cacheKeyPrefix = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "RoleName", roleName);
            return string.Format("{0}::AuditItemsInUserRole:RoleName:{1}", cacheKeyPrefix, roleName);
        }
    }
}