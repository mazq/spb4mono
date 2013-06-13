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
    /// 权限项目标识配置类（便于使用PermissionItemKey）
    /// </summary>
    /// <remarks>
    /// 各Application应该对该配置类的方法进行扩展
    /// </remarks>
    public class PermissionItemKeys
    {
        #region Instance
        private static PermissionItemKeys _instance = new PermissionItemKeys();

        /// <summary>
        /// 获取该类的单例
        /// </summary>
        /// <returns></returns>
        public static PermissionItemKeys Instance()
        {
            return _instance;
        }

        private PermissionItemKeys()
        { }
        #endregion
    }
}
