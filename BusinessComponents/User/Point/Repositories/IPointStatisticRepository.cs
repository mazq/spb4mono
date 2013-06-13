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
    public interface IPointStatisticRepository
    {
        /// <summary>
        /// 更新用户积分统计
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pointCategory2PointsDictionary"><remarks>key=PointCategory,value=Points</remarks>积分分类-积分字典</param>
        /// <returns>修订后应获取到的积分值</returns>
        Dictionary<string, int> UpdateStatistic(long userId, Dictionary<PointCategory, int> pointCategory2PointsDictionary);

        /// <summary>
        /// 删除beforeDays天以前的积分统计
        /// </summary>
        /// <param name="beforeDays">天数</param>
        /// <returns>清除的记录数</returns>
        int Clean(int beforeDays);

        /// <summary>
        /// 查询积分统计列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pointCategoryKey">积分分类Key</param>
        /// <param name="statisticalYear">统计年份</param>
        /// <param name="statisticalMonth">统计月份</param>
        /// <param name="statisticalDay">统计月份的第几天</param>
        /// <returns>积分统计列表</returns>
        IEnumerable<PointStatistic> Gets(long userId, string pointCategoryKey, int? statisticalYear = null, int? statisticalMonth = null, int? statisticalDay = null);

    }
}