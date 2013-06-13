//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 租户权限验证处理器
    /// </summary>
    public interface ITenantAuthorizationHandler
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        string TenantTypeId { get; }

        /// <summary>
        /// 判断是否为租户管理者
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="tenantOwnerId">租户拥有者Id</param>
        /// <returns>true-是；false-不是</returns>
        bool IsTenantManager(IUser currentUser, long tenantOwnerId);

        /// <summary>
        /// 判断是否为租户普通成员
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="tenantOwnerId">租户拥有者Id</param>
        /// <returns>true-是；false-不是</returns>
        bool IsTenantMember(IUser currentUser, long tenantOwnerId);
    }
}
