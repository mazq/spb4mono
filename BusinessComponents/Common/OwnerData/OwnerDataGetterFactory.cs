//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 评论URL获取器工厂
    /// </summary>
    public static class OwnerDataGetterFactory
    {
        /// <summary>
        /// 依据tenantTypeId获取OwnerDatalGetterFactory
        /// </summary>
        /// <returns></returns>
        public static IOwnerDataGetter Get(string dataKey)
        {
            return DIContainer.Resolve<IEnumerable<IOwnerDataGetter>>().Where(g => g.DataKey.Equals(dataKey, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
