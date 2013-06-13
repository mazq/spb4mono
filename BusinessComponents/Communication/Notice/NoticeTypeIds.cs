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
    /// 通知类型Id管理类
    /// </summary>
    public class NoticeTypeIds
    {
        #region Instance
        private static NoticeTypeIds _instance = new NoticeTypeIds();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static NoticeTypeIds Instance()
        {
            return _instance;
        }

        private NoticeTypeIds()
        { }

        #endregion

        /// <summary>
        /// 回复
        /// </summary>
        /// <returns></returns>
        public int Reply()
        {
            return 1;
        }

        /// <summary>
        /// 管理
        /// </summary>
        /// <returns></returns>
        public int Manage()
        {
            return 2;
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <returns></returns>
        public int Hint()
        {
            return 3;
        }


    }
}