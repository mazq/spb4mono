//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 扩展InvitationTypeKeys
    /// </summary>
    public static class InvitationTypeKeysExtension
    {
        /// <summary>
        /// 求关注
        /// </summary>
        /// <param name="invitationTypeKeys">questionId</param>
        public static string InviteFollow(this InvitationTypeKeys invitationTypeKeys)
        {
            return "InviteFollow";
        }
    }

}
