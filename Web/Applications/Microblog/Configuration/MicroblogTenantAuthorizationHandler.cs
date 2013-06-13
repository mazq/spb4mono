//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 帖吧租户权限验证处理器
    /// </summary>
    public class MicroblogTenantAuthorizationHandler : ITenantAuthorizationHandler
    {

        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().User(); }
        }

        /// <summary>
        /// 判断是否为租户管理者
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="tenantOwnerId">租户拥有者Id</param>
        /// <returns>true-是；false-不是</returns>
        public bool IsTenantManager(IUser currentUser, long tenantOwnerId)
        {
            return tenantOwnerId == currentUser.UserId;
        }

        /// <summary>
        /// 判断是否为租户普通成员
        /// </summary>
        /// <param name="currentUser">当前用户</param>
        /// <param name="tenantOwnerId">租户拥有者Id</param>
        /// <returns>true-是；false-不是</returns>
        public bool IsTenantMember(IUser currentUser, long tenantOwnerId)
        {
            return tenantOwnerId == currentUser.UserId;
        }

    }
}