//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 拥有者数据标识
    /// </summary>
    public class OwnerDataKeys
    {
        private static ConcurrentDictionary<string, OwnerDataKeys> registeredTenantAttachmentSettings = new ConcurrentDictionary<string, OwnerDataKeys>();

        #region Instance
        private static OwnerDataKeys _instance = new OwnerDataKeys();

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static OwnerDataKeys Instance()
        {
            return _instance;
        }

        #endregion
    }
}
