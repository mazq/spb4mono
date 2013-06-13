using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 授权服务接口
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// 当前用户是不是超级管理员
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <returns>是超级管理员返回true，否则返回false</returns>
        bool IsSuperAdministrator(IUser currentUser);

        /// <summary>
        /// 是不是拥有者
        /// </summary>
        /// <remarks>
        /// 拥有者一般对自己的内容有管理权限
        /// </remarks>
        /// <param name="currentUser">当前用户</param>
        /// <param name="userIds">可能作为拥着有的多个用户Id</param>
        /// <returns>是拥有者返回true，否则返回false</returns>
        bool IsOwner(IUser currentUser, params long[] userIds);

        /// <summary>
        /// 是不是租户管理者
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="tenantOwnerId">租户的OwnerId</param>
        /// <returns>是租户拥有者返回true，否则返回false</returns>
        bool IsTenantManager(IUser currentUser, string tenantTypeId, long tenantOwnerId);

        /// <summary>
        /// 是不是租户普通成员
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="tenantOwnerId">租户的OwnerId</param>
        /// <returns>是租户拥有者返回true，否则返回false</returns>
        bool IsTenantMember(IUser currentUser, string tenantTypeId, long tenantOwnerId);

        /// <summary>
        /// 是不是应用管理员
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="applicationId">应用Id</param>
        /// <returns>是应用管理员返回true，否则返回false</returns>
        bool IsApplicationManager(IUser currentUser, int applicationId);
        
        /// <summary>
        /// 检查用户是否有权限进行某项操作
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="permissionItemKey">权限项目标识</param>
        /// <returns>有权限操作返回true，否则返回false</returns>
        bool Check(IUser currentUser, string permissionItemKey);
    }
}