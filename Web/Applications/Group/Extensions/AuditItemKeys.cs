//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组审核项
    /// </summary>
    public static class AuditItemKeysExtension
    {
        /// <summary>
        /// 群组审核项
        /// </summary>
        public static string Group(this AuditItemKeys auditItemKeys)
        {
            return "Group";
        }
    }
}