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
    /// 系统数据标识
    /// </summary>
    public class SystemDataKeys
    {
        #region Instance
        private static SystemDataKeys _instance = new SystemDataKeys();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static SystemDataKeys Instance()
        {
            return _instance;
        }

        private SystemDataKeys()
        { }
        #endregion

        /// <summary>
        /// 交易积分
        /// </summary>
        /// <returns></returns>
        public string TradePoints()
        {
            return "TradePoints";
        }


    }
}
