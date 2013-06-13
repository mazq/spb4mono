//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace Tunynet.Common
{
    /// <summary>
    /// 请求类型设置实体类
    /// </summary>
    public class InvitationTypeSettings
    {
        /// <summary>
        /// 类型Key
        /// </summary>
        public string TypeKey { get; set; }

        /// <summary>
        /// 是否允许
        /// </summary>
        public bool IsAllow { get; set; }
    }
}
