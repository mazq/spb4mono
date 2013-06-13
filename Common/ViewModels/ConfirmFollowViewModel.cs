//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户是否确定关注用户
    /// </summary>
    public class ConfirmFollowViewModel
    {
        /// <summary>
        /// 用户登录凭证
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string invite { get; set; }

        /// <summary>
        /// 准备关注用户名
        /// </summary>
        public string FollowUserName { get; set; }
    }
}
