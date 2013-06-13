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
    /// 评论URL获取器工厂
    /// </summary>
    public static class CommentUrlGetterFactory
    {
        /// <summary>
        /// 依据tenantTypeId获取ICommentUrlGetter
        /// </summary>
        /// <returns></returns>
        public static ICommentUrlGetter Get(string tenantTypeId)
        {
            return DIContainer.Resolve<IEnumerable<ICommentUrlGetter>>().Where(g => g.TenantTypeId.Equals(tenantTypeId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
