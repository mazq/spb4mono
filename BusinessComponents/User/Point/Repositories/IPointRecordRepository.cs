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
    /// 积分记录数据访问接口
    /// </summary>
    public interface IPointRecordRepository : IRepository<PointRecord>
    {
        /// <summary>
        ///  清理积分记录
        /// </summary>
        /// <param name="beforeDays">清理beforeDays天以前的积分记录</param>
        /// <param name="cleanSystemPointRecords">是否也删除系统积分记录</param>
        void CleanPointRecords(int beforeDays, bool cleanSystemPointRecords);

        /// <summary>
        /// 查询用户积分记录
        /// </summary>
        /// <param name="userId">用户Id<remarks>系统积分的UserId=0</remarks></param>
        /// <param name="isIncome">是不是收入的积分</param>
        /// <param name="pointItemName">积分项目名称</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        PagingDataSet<PointRecord> GetPointRecords(long? userId, bool? isIncome, string pointItemName, System.DateTime? startDate, System.DateTime? endDate,int pageSize, int pageIndex);
    }
}