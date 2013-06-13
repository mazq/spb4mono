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
    /// 第三方账号类型标识
    /// </summary>
    public class AccountTypeKeys
    {
        #region Instance

        private static volatile AccountTypeKeys _instance = null;
        private static readonly object lockObject = new object();

        public static AccountTypeKeys Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new AccountTypeKeys();
                    }
                }
            }
            return _instance;
        }

        private AccountTypeKeys()
        { }

        #endregion Instance


        /// <summary>
        /// QQ
        /// </summary>
        /// <returns></returns>
        public string QQ()
        {
            return "QQ";
        }

        /// <summary>
        /// 新浪微博
        /// </summary>
        /// <returns></returns>
        public string SinaWeibo()
        {
            return "SinaWeibo";
        }

        /// <summary>
        /// 人人
        /// </summary>
        /// <returns></returns>
        public string Renren()
        {
            return "Renren";
        }


    }
}
