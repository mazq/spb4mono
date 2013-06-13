//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 标签url获取管理器
    /// </summary>
    public class TagUrlGetterManager
    {
        private static ConcurrentDictionary<string, ITagUrlGetter> registeredTagUrlGetters = new ConcurrentDictionary<string, ITagUrlGetter>();

        /// <summary>
        /// 获取注册的TagUrlGetters
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public static ITagUrlGetter GetRegisteredGetter(string tenantTypeId)
        {
            ITagUrlGetter tagUrlGetter;
            if (registeredTagUrlGetters.TryGetValue(tenantTypeId, out tagUrlGetter))
                return tagUrlGetter;

            return null;
        }

        /// <summary>
        /// 注册UserDataSettings
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<param name="getter">标签Url获取器</param>
        public static void RegisterGetter(string tenantTypeId, ITagUrlGetter getter)
        {
            if (getter != null && !registeredTagUrlGetters.ContainsKey(tenantTypeId))
                registeredTagUrlGetters[tenantTypeId] = getter;
        }
    }
}