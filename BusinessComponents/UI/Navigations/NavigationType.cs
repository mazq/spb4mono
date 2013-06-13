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
    /// 导航类型
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// 来源于Application
        /// </summary>
        Application = 0,

        /// <summary>
        /// 呈现区域初始化的导航
        /// </summary>
        PresentAreaInitial = 1,

        /// <summary>
        /// 呈现区域Owner新增的导航
        /// </summary>
        AddedByOwner = 2,

    }
}
