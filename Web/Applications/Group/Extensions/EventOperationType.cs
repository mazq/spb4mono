//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Events;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 积分项
    /// </summary>
    public static class EventOperationTypeExtension
    {
        /// <summary>
        /// 设置管理员
        /// </summary>
        public static string SetGroupManager(this EventOperationType EventOperationType)
        {
            return "SetGroupManager";
        }

        /// <summary>
        /// 取消管理员
        /// </summary>
        public static string CancelGroupManager(this EventOperationType EventOperationType)
        {
            return "CancelGroupManager";
        }

    }
}
