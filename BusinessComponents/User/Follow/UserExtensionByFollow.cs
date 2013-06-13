//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 为IUser扩展与关注用户相关的功能
    /// </summary>
    public static class UserExtensionByFollow
    {
        /// <summary>
        /// 判断用户是否关注了某个用户
        /// </summary>
        /// <param name="user"><see cref="IUser"/></param>
        /// <param name="toUserId">待检测用户Id</param>
        /// <returns></returns>
        public static bool IsFollowed(this IUser user, long toUserId)
        {
            if (user == null)
                return false;

            FollowService followService = new FollowService();
            return followService.IsFollowed(user.UserId, toUserId);
        }        
    }
}