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
using PetaPoco;
using Tunynet.Caching;


namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 在线用户统计数据访问
    /// </summary>
    public class OnlineUserStatisticRepository : Repository<OnlineUserStatistic>, IOnlineUserStatisticRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        private int pageIndex = 1;
        private int pageSize = 20;
        /// <summary>
        /// 获取历史最高在线记录
        /// </summary>
        /// <returns></returns>
        public OnlineUserStatistic GetHighest()
        {
            //设计要点：
            //1、缓存期限：常用，无需即时，使用一级缓存
            //获取UserCount最高的记录
            string cacheKey = GetCacheKey_Highest();

            
            OnlineUserStatistic highest = cacheService.GetFromFirstLevel<OnlineUserStatistic>(cacheKey);
            if (highest == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("tn_OnlineUserStatistics")
                    .OrderBy("UserCount desc");
                

                highest =(OnlineUserStatistic)CreateDAO().FetchTopPrimaryKeys<OnlineUserStatistic>(1,sql);
                cacheService.Set(cacheKey, highest, CachingExpirationType.UsualSingleObject);
            }
            return highest;
        }
        /// <summary>
        /// 获取在线用户统计记录
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns></returns>
        public PagingDataSet<OnlineUserStatistic> GetOnlineUserStatistics(DateTime? startDate, DateTime? endDate)
        {
            //设计说明: 
            //缓存期限：常用，无需即时，使用一级缓存
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    string cacheKey = GetCacheKey_OnlineUserStatistics(startDate, endDate);
                    return cacheKey;
                }, () =>
                {
                    Sql sql = Sql.Builder;
                    sql.Select("*")
                        .From("tn_OnlineUserStatistics");
                    if (startDate.HasValue)
                        sql.Where("DateCreated >= @0", startDate.Value);
                    if (endDate.HasValue)
                        sql.Where("DateCreated < @0", endDate.Value.AddDays(1));
                    sql.OrderBy("DateCreated desc");
                    return sql;
                }
                );
        }
        /// <summary>
        /// 获取历史最高在线记录CachKey
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey_Highest()
        {
            return "OnlineUser_Highest";
        }
        /// <summary>
        /// 获取在线用户统计记录CacheKey
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private string GetCacheKey_OnlineUserStatistics(DateTime? startDate, DateTime? endDate)
        {
            return string.Format("OnlineUserStatistics::startDate-{0}-endDate-{1}", startDate, endDate);
        }
    }
}
