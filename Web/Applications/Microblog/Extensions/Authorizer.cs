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
using Spacebuilder.Common;
using Tunynet;
using Spacebuilder.Group;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// Microblog的权限验证
    /// </summary>
    public static class AuthorizerExtension
    {

        /// <summary>
        /// 是否具有创建Microblog的权限
        /// </summary>
        /// <param name="authorizer">被扩展对象</param>
        /// <param name="tenantTypeId">微博实体</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns></returns>
        public static bool Microblog_Create(this Authorizer authorizer, string tenantTypeId = null, long ownerId = 0)
        {
            string errorMessage = string.Empty;
            return authorizer.Microblog_Create(out errorMessage, tenantTypeId, ownerId);
        }

        /// <summary>
        /// 是否具有创建Microblog的权限
        /// </summary>
        /// <param name="authorizer">被扩展对象</param>
        /// <param name="tenantTypeId">微博实体</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns></returns>
        public static bool Microblog_Create(this Authorizer authorizer, out string errorMessage, string tenantTypeId = null, long ownerId = 0)
        {
            IUser currentUser = UserContext.CurrentUser;
            errorMessage = "没有权限发微博";
            if (currentUser == null)
            {
                errorMessage = "您必须先登录，才能发微博";
                return false;
            }
            if (authorizer.AuthorizationService.Check(currentUser, PermissionItemKeys.Instance().Microblog_Create()))
            {
                if (tenantTypeId == TenantTypeIds.Instance().User())
                    return true;

                if (authorizer.AuthorizationService.IsTenantMember(currentUser, tenantTypeId, ownerId))
                {
                    return true;
                }
                else
                    errorMessage = "您必须先加入，才能发微博";
            }
            if (currentUser.IsModerated)
                errorMessage = Resources.Resource.Description_ModeratedUser_CreateMicroblogDenied;
            return false;
        }

        /// <summary>
        /// 是否具有删除Microblog的权限
        /// </summary>        
        /// <param name="authorizer">被扩展对象</param>
        /// <param name="microblog">微博实体</param>
        /// <returns></returns>
        public static bool Microblog_Delete(this Authorizer authorizer, MicroblogEntity microblog)
        {
            if (microblog == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (microblog.UserId == currentUser.UserId
                || authorizer.IsAdministrator(MicroblogConfig.Instance().ApplicationId))
                return true;

            return false;
        }

    }
}