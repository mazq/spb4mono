//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.UI
{
    /// <summary>
    /// 资源呈现优先级
    /// </summary>
    public enum ResourceRenderPriority
    {
        /// <summary>
        /// 优先呈现
        /// </summary>
        First,

        /// <summary>
        /// 未指定优先级
        /// </summary>
        Unspecified,

        /// <summary>
        /// 最后呈现
        /// </summary>
        Last
    }
}
