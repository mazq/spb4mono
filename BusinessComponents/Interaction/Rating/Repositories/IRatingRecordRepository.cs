//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common
{  /// <summary>
    /// 星级评价记录数据访问接口
    /// </summary>
    public interface IRatingRecordRepository : IRepository<RatingRecord>
    {
        /// <summary>
        /// 删除用户的记录
        /// </summary>
        /// <param name="userId"></param>
        void ClearByUser(long userId);

        /// <summary>
        /// 删除N天前的评价记录
        /// </summary>
        /// <param name="beforeDays">间隔天数</param>
        void Clean(int? beforeDays);

        /// <summary>
        ///获取前N条用户的星级评价记录信息
        /// </summary>
        /// <param name="objectId"> 操作Id</param>
        /// <param name="tentanTypeId">操作类型Id</param>
        /// <param name="rateNumber">等级类型</param>
        /// <param name="topNumber">前N条</param>
        IEnumerable<RatingRecord> GetTopRatingRecords(long objectId, string tentanTypeId, int? rateNumber, int topNumber);

        /// <summary>
        /// 清空相关联评价记录
        /// </summary>
        /// <param name="objectId">操作Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        void ClearRatingRecordsOfObjectId(long objectId, string tenantTypeId);
    }
}