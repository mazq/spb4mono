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
using Tunynet;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 获取Msn联系人
    /// </summary>
    public interface IMsnContactAccessor
    {
        /// <summary>
        /// 获取Msn联系人
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="password">密码</param>
        /// <param name="isSuccessLogin">是否成功登录</param>
        /// <returns>Key:联系人Email地址，Value：联系人名称</returns>
        Dictionary<string, string> GetContacts(string userName, string password, out bool isSuccessLogin);
    }
}