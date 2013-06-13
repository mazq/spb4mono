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
    /// 提醒信息类型Id管理类
    /// </summary>
    public class ReminderInfoTypeIds
    {
        #region Instance
        private static ReminderInfoTypeIds _instance = new ReminderInfoTypeIds();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static ReminderInfoTypeIds Instance()
        {
            return _instance;
        }

        private ReminderInfoTypeIds()
        { }

        #endregion

        /// <summary>
        /// 私信
        /// </summary>
        /// <returns></returns>
        public int Message()
        {
            return 1;
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <returns></returns>
        public int Notice()
        {
            return 2;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <returns></returns>
        public int Invitation()
        {
            return 3;
        }
    }
}