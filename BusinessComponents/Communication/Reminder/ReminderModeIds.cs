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
    /// 提醒方式Id管理类
    /// </summary>
    public class ReminderModeIds
    {
        #region Instance
        private static ReminderModeIds _instance = new ReminderModeIds();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static ReminderModeIds Instance()
        {
            return _instance;
        }

        private ReminderModeIds()
        { }

        #endregion

        /// <summary>
        /// 电子邮件
        /// </summary>
        /// <returns></returns>
        public int Email()
        {
            return 1;
        }

        /// <summary>
        /// 手机短信
        /// </summary>
        /// <returns></returns>
        //public int SMS()
        //{
        //    return 2;
        //}
    }
}