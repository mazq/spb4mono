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
    /// 动态拥有者类型
    /// </summary>
    public class ActivityOwnerTypes
    {
        #region Instance

        private static volatile ActivityOwnerTypes _instance = null;
        private static readonly object lockObject = new object();

        public static ActivityOwnerTypes Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ActivityOwnerTypes();
                    }
                }
            }
            return _instance;
        }

        private ActivityOwnerTypes()
        { }

        #endregion Instance


        /// <summary>
        /// 用户
        /// </summary>
        public int User()
        {
            return 1;
        }

        /// <summary>
        /// 群组
        /// </summary>
        public int Group()
        {
            return 11;
        }


    }
}
