//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 被封禁对象模板
    /// </summary>
    public class StopedUserViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 被封禁对象名
        /// </summary>
        public string ToUserDisplayName { get; set; }

        /// <summary>
        /// 被封禁用户id
        /// </summary>
        public long ToUserId { get; set; }
    }

    /// <summary>
    /// 黑名单对象的扩展类
    /// </summary>
    public static class StopedUserExtensions
    {
        /// <summary>
        /// 转换为viewModel的方法
        /// </summary>
        /// <param name="stopedUser"></param>
        /// <returns></returns>
        public static StopedUserViewModel AsViewModel(this StopedUser stopedUser)
        {
            return new StopedUserViewModel
            {
                Id = stopedUser.Id,
                ToUserDisplayName = stopedUser.ToUserDisplayName,
                ToUserId = stopedUser.ToUserId
            };
        }
    }
}
