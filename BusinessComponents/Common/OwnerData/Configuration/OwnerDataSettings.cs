//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 用户数据配置类
    /// </summary>
    public class OwnerDataSettings
    {
        private static ConcurrentDictionary<string, List<string>> tenantDataKeys = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 获取注册的DataKey
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public static IEnumerable<string> GetDataKeys(string tenantTypeId)
        {
            if (tenantDataKeys != null && tenantDataKeys.ContainsKey(tenantTypeId))
            {
                List<string> dataKeys = new List<string>();
                tenantDataKeys.TryGetValue(tenantTypeId, out dataKeys);

                return dataKeys;
            }

            return new List<string>();
        }

        /// <summary>
        /// 注册用户统计内容数的DataKey
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<param name="dataKeys">需要统计的数据DataKey</param>
        public static void RegisterStatisticsDataKeys(string tenantTypeId, params string[] dataKeys)
        {
            RegisterStatisticsDataKeys(new List<string>() { tenantTypeId }, dataKeys);
        }

        /// <summary>
        /// 注册用户统计内容数的DataKey
        /// </summary>
        /// <param name="tenantTypeIds">租户类型Id集合</param>
        ///<param name="dataKeys">需要统计的数据DataKey</param>
        public static void RegisterStatisticsDataKeys(List<string> tenantTypeIds, params string[] dataKeys)
        {
            if (tenantTypeIds != null && dataKeys != null)
            {
                foreach (var tenantTypeId in tenantTypeIds)
                {
                    if (!tenantDataKeys.ContainsKey(tenantTypeId))
                    {
                        tenantDataKeys[tenantTypeId] = new List<string>();
                    }
                    tenantDataKeys[tenantTypeId].AddRange(dataKeys);
                }
            }
        }
    }
}
