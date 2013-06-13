//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;
using System;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///广告数据访问接口
    /// </summary>
    public interface IAdvertisingRepository : IRepository<Advertising>
    {
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="presentAreaKey">投放区域</param>
        /// <param name="positionId">广告位</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isExpired">是否过期</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        PagingDataSet<Advertising> GetAdvertisingsForAdmin(string presentAreaKey, string positionId, DateTime? startDate, DateTime? endDate, bool? isExpired, bool? isEnable,int pageSize,int pageIndex);

        /// <summary>
        /// 根据广告Id取所有的广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        IEnumerable<AdvertisingPosition> GetPositionsByAdvertisingId(long advertisingId);

        /// <summary>
        /// 获取广告统计数据
        /// </summary>
        /// <returns></returns>
        long GetAdvertisingCount();

         /// <summary>
        /// 清除广告的所有广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        void ClearPositionsFromAdvertising(long advertisingId);

        /// <summary>
        /// 为广告批量设置广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <param name="positionIds">广告位Id集合</param>
        void AddPositionsToAdvertising(long advertisingId, IEnumerable<string> positionIds);
    }
}
