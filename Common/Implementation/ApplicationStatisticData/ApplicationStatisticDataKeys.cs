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
    /// 应用统计数据标识(要求应用内唯一）
    /// </summary>
    public class ApplicationStatisticDataKeys
    {
        #region Instance

        private static ApplicationStatisticDataKeys _instance = new ApplicationStatisticDataKeys();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static ApplicationStatisticDataKeys Instance()
        {
            return _instance;
        }

        private ApplicationStatisticDataKeys()
        { }
        #endregion


        /// <summary>
        /// 总数
        /// </summary>
        /// <returns></returns>
        public string TotalCount()
        {
            return "TotalCount";
        }

        /// <summary>
        /// 24小时新增数
        /// </summary>
        /// <returns></returns>
        public string Last24HCount()
        {
            return "Last24HCount";
        }

        /// <summary>
        /// 待审核数
        /// </summary>
        /// <returns></returns>
        public string PendingCount()
        {
            return "PendingCount";
        }

        /// <summary>
        /// 需再审核数
        /// </summary>
        /// <returns></returns>
        public string AgainCount()
        {
            return "AgainCount";
        }

    }
}
