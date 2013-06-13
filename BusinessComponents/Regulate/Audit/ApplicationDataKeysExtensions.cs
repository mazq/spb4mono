//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// ApplicationDataKeys扩展
    /// </summary>
    public static class ApplicationDataKeysExtensions
    {
        /// <summary>
        /// 应用的哪些审核状态可以对外显示
        /// </summary>
        /// <param name="applicationDataKeys"></param>
        public static string PubliclyAuditStatus(this ApplicationDataKeys applicationDataKeys)
        {
            return "PubliclyAuditStatus";
        }

    }
}
