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
    ///推荐类别数据访问接口
    /// </summary>
    public interface IRecommendItemTypeRepository : IRepository<RecommendItemType>
    {
        /// <summary>
        /// 获取推荐类别列表
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        IEnumerable<RecommendItemType> GetRecommendTypes(string tenantTypeId);
    }
}