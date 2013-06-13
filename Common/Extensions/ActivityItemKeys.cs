//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 微博动态项
    /// </summary>
    public static class ActivityItemKeysExtension
    {
        /// <summary>
        /// 关注用户动态项
        /// </summary>
        public static string FollowUser(this ActivityItemKeys activityItemKeys)
        {
            return "FollowUser";
        }
    }

}
