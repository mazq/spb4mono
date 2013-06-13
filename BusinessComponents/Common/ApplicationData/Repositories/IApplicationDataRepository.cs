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
    /// ApplicationData数据访问接口
    /// </summary>
    public interface IApplicationDataRepository : IRepository<ApplicationData>
    {
        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="applicationId">applicationId</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        void Change(int applicationId, string dataKey, long value);

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        void Change(int applicationId, string tenantTypeId, string dataKey, long value);

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="applicationId">applicationId</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        void Change(int applicationId, string dataKey, decimal value);

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        void Change(int applicationId, string tenantTypeId, string dataKey, decimal value);

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的值</param>
        void Change(int applicationId, string dataKey, string value);

        /// <summary>
        /// 变更应用数据
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        void Change(int applicationId, string tenantTypeId, string dataKey, string value);

        /// <summary>
        /// 获取ApplicationData
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="dataKey">数据标识</param>
        ApplicationData Get(int applicationId, string dataKey);

        /// <summary>
        /// 获取ApplicationData
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">数据标识</param> 
        /// <param name="dataKey">数据标识</param>
        ApplicationData Get(int applicationId, string tenantTypeId, string dataKey);

        /// <summary>
        /// 获取DataKey对应的值
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="tenantTypeId">租户类型Id</param> 
        IEnumerable<ApplicationData> GetAll(int applicationId, string tenantTypeId = "");

    }
}