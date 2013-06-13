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
    /// 用于身份认证的接口
    /// </summary>
    /// <remarks>实例的生命周期为每HttpRequest</remarks>
    public interface IAuthenticationService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录的用户</param>
        /// <param name="rememberPassword">是否记住密码</param>
        void SignIn(IUser user, bool rememberPassword);

        /// <summary>
        /// 注销
        /// </summary>
        void SignOut();

        /// <summary>
        /// 获取当前登录的用户
        /// </summary>
        /// <returns>
        /// 当前用户未通过认证则返回null
        /// </returns>
        IUser GetAuthenticatedUser();


    }
}
