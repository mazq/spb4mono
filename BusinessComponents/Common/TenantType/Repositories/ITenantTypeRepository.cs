//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///TenantType数据访问接口
    /// </summary>
    public interface ITenantTypeRepository : IRepository<TenantType>
    {
        /// <summary>
        /// 依据服务或应用获取租户类型
        /// </summary>
        /// <param name="serviceKey">服务标识</param>
        /// <param name="applicationId">应用Id</param>
        /// <returns>如未满足条件的TenantType则返回空集合</returns>
        IEnumerable<TenantType> Gets(string serviceKey, int? applicationId = null);
    }
}