//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微薄中获取连接的接口
    /// </summary>
    public static class MicroblogUrlGetterFactory
    {
        /// <summary>
        /// 获取连接的方法
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <returns>获取连接的实例</returns>
        public static IMicroblogUrlGetter Get(string tenantTypeId)
        {
            return DIContainer.Resolve<IEnumerable<IMicroblogUrlGetter>>().Where(n => n.TenantTypeId.Equals(tenantTypeId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}