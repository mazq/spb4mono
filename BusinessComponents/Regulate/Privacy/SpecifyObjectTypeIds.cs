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
    /// 隐私设置指定对象类型Id管理类
    /// </summary>
    public class SpecifyObjectTypeIds
    {
        #region Instance
        private static SpecifyObjectTypeIds _instance = new SpecifyObjectTypeIds();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static SpecifyObjectTypeIds Instance()
        {
            return _instance;
        }

        private SpecifyObjectTypeIds()
        { }

        #endregion

        /// <summary>
        /// 指定人
        /// </summary>
        /// <returns></returns>
        public int User()
        {
            return 1;
        }

        /// <summary>
        /// 指定关注分组
        /// </summary>
        /// <returns></returns>
        public int UserGroup()
        {
            return 2;
        }       
    }
}