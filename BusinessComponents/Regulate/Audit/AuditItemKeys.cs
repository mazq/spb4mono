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
    /// 审核项目标识
    /// </summary>
    public class AuditItemKeys
    {
        #region Instance
        private static AuditItemKeys _instance = new AuditItemKeys();

        /// <summary>
        /// 获取该类的单例
        /// </summary>
        /// <returns></returns>
        public static AuditItemKeys Instance()
        {
            return _instance;
        }

        private AuditItemKeys()
        { }

        /// <summary>
        /// 评论
        /// </summary>
        public string Comment()
        {
            return "Comment";
        }

        #endregion
    }
}
