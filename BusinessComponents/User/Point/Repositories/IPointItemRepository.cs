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
    /// 积分项目数据访问接口
    /// </summary>
    public interface IPointItemRepository : IRepository<PointItem>
    {
        /// <summary>
        /// 获取积分项目集合
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <returns>如果无满足条件的积分项目返回空集合</returns>
        IEnumerable<PointItem> GetPointItems(int? applicationId);
    }
}