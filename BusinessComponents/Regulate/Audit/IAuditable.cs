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
    /// 可审核接口
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        AuditStatus AuditStatus { get; set; }

        /// <summary>
        /// 审核项目标识
        /// </summary>
        /// <remarks>
        /// 具体实现类显性实现
        /// </remarks>
        string AuditItemKey { get; }



    }
}
