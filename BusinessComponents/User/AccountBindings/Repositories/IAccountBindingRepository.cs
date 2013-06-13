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

namespace Tunynet.Common.Repositories
{
    public interface IAccountBindingRepository : IRepository<AccountBinding>
    {
         /// <summary>
        /// 创建第三方账号绑定
        /// </summary>
        /// <param name="account"></param>     
        void CreateAccountBinding(AccountBinding account);
        /// <summary>
        /// 删除第三方账号绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        void DeleteAccountBinding(long userId, string accountTypeKey);

        /// <summary>
        /// 获取单个第三方账号绑定
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        /// <returns></returns>
        AccountBinding GetAccountBinding(long userId, string accountTypeKey);

        /// <summary>
        /// 获取某用户的所有第三方账号绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>若没有，则返回空集合</returns>
        IEnumerable<AccountBinding> GetAccountBindings(long userId);
        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="accountTypeKey">第三方账号类型Key</param>
        /// <param name="Identification">第三方账号标识</param>
        /// <returns>用户Id</returns>
        long GetUserId(string accountTypeKey, string Identification);
        
        /// <summary>
        /// 更新授权凭据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountTypeKey"></param>
        /// <param name="identification"></param>
        /// <param name="accessToken"></param>
        void UpdateAccessToken(long userId, string accountTypeKey, string identification, string accessToken);
    }
}
