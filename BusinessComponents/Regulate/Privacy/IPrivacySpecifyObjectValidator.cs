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
    /// 隐私指定对象验证器
    /// </summary>
    public interface IPrivacySpecifyObjectValidator
    {
        /// <summary>
        /// 验证指定对象针对toUserId是否具有隐私权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被验证用户</param>
        /// <param name="specifyObjectId">指定对象Id</param>
        /// <returns>true-成功，false-失败</returns>
        bool Validate(long userId, long toUserId, long specifyObjectId);
    }
}