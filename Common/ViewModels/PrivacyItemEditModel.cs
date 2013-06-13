//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 隐私项目
    /// </summary>
    public class PrivacyItemEditModel
    {
        /// <summary>
        /// 隐私标志
        /// </summary>
        // public string ItemKey { get; set; }

        /// <summary>
        /// 隐私状态
        /// </summary>
        public PrivacyStatus PrivacyStatus { get; set; }
    }
}
