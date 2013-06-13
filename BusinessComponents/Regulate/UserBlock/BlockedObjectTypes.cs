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
    /// 被屏蔽对象类型
    /// </summary>
    public class BlockedObjectTypes
    {
        #region Instance
        private static BlockedObjectTypes _instance = new BlockedObjectTypes();
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static BlockedObjectTypes Instance()
        {
            return _instance;
        }

        private BlockedObjectTypes()
        { }

        #endregion


        /// <summary>
        /// 用户
        /// </summary>
        /// <returns>返回用户对应的被屏蔽对象类型</returns>
        public int User()
        {
            return 1;
        }

        /// <summary>
        /// 群组
        /// </summary>
        /// <returns>返回群组对应的被屏蔽对象类型</returns>
        public int Group()
        {
            return 11;
        }

    }
}
