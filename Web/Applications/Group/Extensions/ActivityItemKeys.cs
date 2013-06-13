//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Group
{

    /// <summary>
    /// 群组动态项
    /// </summary>
    public static class ActivityItemKeysExtension
    {
        /// <summary>
        /// 创建群组动态项
        /// </summary>
        public static string CreateGroup(this ActivityItemKeys activityItemKeys)
        {
            return "CreateGroup";
        }

        /// <summary>
        /// 新成员加入动态项
        /// </summary>
        public static string CreateGroupMember(this ActivityItemKeys activityItemKeys)
        {
            return "CreateGroupMember";
        }
        /// <summary>
        /// 加入群组动态项
        /// </summary>
        public static string JoinGroup(this ActivityItemKeys activityItemKeys)
        {
            return "JoinGroup";
        }
    }
}