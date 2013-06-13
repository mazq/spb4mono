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
    /// 隐私状态
    /// </summary>
    public enum PrivacyStatus
    {   
        /// <summary>
        /// 仅自己可见
        /// </summary>        
        Private = 0,
                
        /// <summary>
        /// 仅部分人可见
        /// </summary>
        Part = 1,
        
        /// <summary>
        /// 所有人可见
        /// </summary>
        Public = 2
    } 

}
