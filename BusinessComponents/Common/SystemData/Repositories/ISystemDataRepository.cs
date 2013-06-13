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
    /// SystemData数据访问接口
    /// </summary>
    public interface ISystemDataRepository : IRepository<SystemData>
    {
        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="dataKey">数据标识</param>
        /// <param name="number">待变更的数值</param>
        void Change(string dataKey, long number);

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="dataKey">数据标识</param>
        /// <param name="number">待变更的数值</param>
        void Change(string dataKey, decimal number);              
    }
}