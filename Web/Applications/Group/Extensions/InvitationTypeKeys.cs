//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 扩展InvitationTypeKeys
    /// </summary>
    public static class InvitationTypeKeysExtension
    {
        /// <summary>
        /// 邀请加入群组
        /// </summary>
        /// <param name="invitationTypeKeys">invitationTypeKeys</param>
        public static string InviteJoinGroup(this InvitationTypeKeys invitationTypeKeys)
        {
            return "InviteJoinGroup";
        }

        /// <summary>
        /// 申请加入群组
        /// </summary>
        /// <param name="invitationTypeKeys">invitationTypeKeys</param>
        public static string ApplyJoinGroup(this InvitationTypeKeys invitationTypeKeys)
        {
            return "ApplyJoinGroup";
        }
    }
}