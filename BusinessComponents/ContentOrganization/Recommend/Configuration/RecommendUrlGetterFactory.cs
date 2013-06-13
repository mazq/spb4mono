//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Tunynet.Common
{
    /// <summary>
    /// �Ƽ���ȡ���ӵĹ���
    /// </summary>
    public static class RecommendUrlGetterFactory
    {
        /// <summary>
        /// ��ȡ���ӵķ���
        /// </summary>
        /// <param name="tenantTypeId">�⻧����id</param>
        /// <returns>��ȡ���ӵ�ʵ��</returns>
        public static IRecommendUrlGetter Get(string tenantTypeId)
        {
            return DIContainer.Resolve<IEnumerable<IRecommendUrlGetter>>().Where(n => n.TenantTypeId.Equals(tenantTypeId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}