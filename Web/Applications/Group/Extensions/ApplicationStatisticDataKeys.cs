//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Mvc;
using System.Web.Routing;
using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 用户计数类型扩展类
    /// </summary>
    public static class ApplicationStatisticDataKeysExtension
    {
        /// <summary>
        /// 群组待审核数
        /// </summary>
        public static string GroupPendingCount(this ApplicationStatisticDataKeys applicationStatisticDataKeys)
        {
            return "GroupPendingCount";
        }

        /// <summary>
        /// 群组需再审核数
        /// </summary>
        public static string GroupAgainCount(this ApplicationStatisticDataKeys applicationStatisticDataKeys)
        {
            return "GroupAgainCount";
        }

    }
}