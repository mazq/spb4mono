//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tunynet.Common
{
    /// <summary>
    /// 评论URL获取器工厂
    /// </summary>
    public static class AtUserAssociatedUrlGetterFactory
    {
        /// <summary>
        /// 依据tenantTypeId获取IAtUserAssociatedUrlGetter
        /// </summary>
        /// <returns></returns>
        public static IAtUserAssociatedUrlGetter Get(string tenantTypeId)
        {
            return DIContainer.Resolve<IEnumerable<IAtUserAssociatedUrlGetter>>().Where(g => g.TenantTypeId.Equals(tenantTypeId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
