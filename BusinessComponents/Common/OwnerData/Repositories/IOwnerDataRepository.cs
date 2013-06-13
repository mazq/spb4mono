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
    /// OwnerData数据访问接口
    /// </summary>
    public interface IOwnerDataRepository : IRepository<OwnerData>
    {
        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="ownerId">ownerId</param>
        /// <param name="tenantTypeId">租户类型</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        void Change(long ownerId, string tenantTypeId, string dataKey, long value);

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="ownerId">ownerId</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的数值</param>
        void Change(long ownerId, string tenantTypeId, string dataKey, decimal value);

        /// <summary>
        /// 变更用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="dataKey">数据标识</param>
        /// <param name="value">待变更的值</param>
        void Change(long ownerId, string tenantTypeId, string dataKey, string value);

        /// <summary>
        /// 获取OwnerData
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="dataKey">数据标识</param>
        OwnerData Get(long ownerId, string tenantTypeId, string dataKey);

        /// <summary>
        /// 获取DataKey对应的值
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        IEnumerable<OwnerData> GetAll(long ownerId, string tenantTypeId);

        /// <summary>
        /// 获取用户Id分页数据
        /// </summary>
        /// <param name="dataKey">dataKey</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        PagingDataSet<long> GetPagingOwnerIds(string dataKey, string tenantTypeId, int pageIndex, OwnerData_SortBy? sortBy = null);


        /// <summary>
        /// 获取前N个用户Id数据
        /// </summary>
        /// <param name="dataKey">dataKey</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">获取记录的个数</param>
        /// <param name="sortBy">排序字段</param>
        /// <returns></returns>
        IEnumerable<long> GetTopOwnerIds(string dataKey, string tenantTypeId, int topNumber, OwnerData_SortBy? sortBy = null);

        /// <summary>
        /// 清除指定用户的用户数据
        /// </summary>
        /// <param name="ownerId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        void ClearOwnerData(long ownerId, string tenantTypeId);
    }
}