//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    public interface IOnlineUserStatisticRepository:IRepository<OnlineUserStatistic>
    {
         /// <summary>
        /// 获取历史最高在线记录
        /// </summary>
        /// <returns></returns>
        OnlineUserStatistic GetHighest();
        /// <summary>
        /// 获取在线用户统计记录
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns></returns>
        PagingDataSet<OnlineUserStatistic> GetOnlineUserStatistics(DateTime? startDate, DateTime? endDate);
    }
}
