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
    /// 动态项目标识
    /// </summary>
    public class ActivityItemKeys
    {
        #region Instance

        private static volatile ActivityItemKeys _instance = null;
        private static readonly object lockObject = new object();

        public static ActivityItemKeys Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ActivityItemKeys();
                    }
                }
            }
            return _instance;
        }

        private ActivityItemKeys()
        { }

        #endregion Instance

    }
}
