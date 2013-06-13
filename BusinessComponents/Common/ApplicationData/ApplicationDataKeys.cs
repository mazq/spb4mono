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
    /// 应用数据标识
    /// </summary>
    public class ApplicationDataKeys
    {

        #region Instance
        private static ApplicationDataKeys _instance = new ApplicationDataKeys();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static ApplicationDataKeys Instance()
        {
            return _instance;
        }

        private ApplicationDataKeys()
        { }
        #endregion


    }
}
