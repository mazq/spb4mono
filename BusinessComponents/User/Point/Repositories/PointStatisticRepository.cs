//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Text;
using System.Threading;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 积分统计Repository
    /// </summary>
    public class PointStatisticRepository : Repository<PointStatistic>, IPointStatisticRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();
        private static ReaderWriterLockSlim RWLock = new System.Threading.ReaderWriterLockSlim();

        /// <summary>
        /// 更新积分统计
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pointCategory2PointsDictionary"><remarks>key=PointCategory,value=Points</remarks>积分分类-积分字典</param>
        /// <returns>修订后应获取到的积分值</returns>
        public Dictionary<string, int> UpdateStatistic(long userId, Dictionary<PointCategory, int> pointCategory2PointsDictionary)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            RWLock.EnterWriteLock();

            PetaPocoDatabase dao = CreateDAO();
            try
            {
                dao.OpenSharedConnection();

                //1、检查当日积分统计是否存在，不存在创建 
                //2、检查是否超过当日限额，如果未超过更新当日积分累计
                var sql = Sql.Builder;
                sql.Select("*")
                   .From("tn_PointStatistics")
                   .Where("UserId = @0", userId)
                   .Where("StatisticalYear = @0", DateTime.UtcNow.Year)
                   .Where("StatisticalMonth = @0", DateTime.UtcNow.Month)
                   .Where("StatisticalDay = @0", DateTime.UtcNow.Day);

                IEnumerable<PointStatistic> pointStatistices = dao.Fetch<PointStatistic>(sql);

                //初始化
                foreach (var pair in pointCategory2PointsDictionary)
                {
                    dictionary[pair.Key.CategoryKey] = pair.Value;
                }

                //当日积分统计不存在
                if (pointStatistices == null || pointStatistices.Count() == 0)
                {
                    //创建当日积分统计
                    foreach (var pair in pointCategory2PointsDictionary)
                    {
                        if (pair.Key.QuotaPerDay <= 0)
                            continue;

                        var pointStatistic = PointStatistic.New();
                        pointStatistic.UserId = userId;
                        pointStatistic.PointCategoryKey = pair.Key.CategoryKey;
                        pointStatistic.Points = pair.Value;

                        dao.Insert(pointStatistic);
                    }
                }
                else
                {
                    //检查是积分限额，调整用户最终获取到的积分
                    foreach (var pair in pointCategory2PointsDictionary)
                    {
                        if (pair.Key.QuotaPerDay <= 0)
                            continue;
                        var category = pair.Key;
                        var pointStatistic = pointStatistices.FirstOrDefault(n => n.PointCategoryKey == category.CategoryKey);
                        if (pointStatistic == null)
                            continue;
                        if (pair.Value > 0 && pointStatistic.Points + pair.Value > category.QuotaPerDay)//超过限额
                        {
                            dictionary[pair.Key.CategoryKey] = 0;
                        }
                        else
                        {
                            pointStatistic.Points += pair.Value;
                            dao.Update(pointStatistic);
                        }
                    }
                }
            }
            finally
            {

                dao.CloseSharedConnection();
                RWLock.ExitWriteLock();
            }

            //更新用户分区缓存
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);


            return dictionary;
        }


        /// <summary>
        /// 删除beforeDays天以前的积分统计
        /// </summary>
        /// <param name="beforeDays">天数</param>
        /// <returns>清除的记录数</returns>
        public int Clean(int beforeDays)
        {
        
            var sql = PetaPoco.Sql.Builder;
            DateTime dateTime = DateTime.UtcNow.AddDays(-beforeDays);
            sql.Append("Delete from tn_PointStatistics")
               .Where("right(10000 + StatisticalYear ,4) + right(100+ StatisticalMonth ,2) + right(100+ StatisticalDay ,2)<=@0", dateTime.Year.ToString() + dateTime.Month.ToString() + dateTime.Day.ToString());
            return CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 查询积分统计列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="PointCategoryKey">积分项目Key</param>
        /// <param name="statisticalYear">统计年份</param>
        /// <param name="statisticalMonth">统计月份</param>
        /// <param name="statisticalDay">统计月份的第几天</param>
        /// <returns>积分统计列表</returns>
        public IEnumerable<PointStatistic> Gets(long userId, string PointCategoryKey, int? statisticalYear = null, int? statisticalMonth = null, int? statisticalDay = null)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
            IEnumerable<long> Ids = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());
            if (Ids == null)
            {
                var sql = Sql.Builder;
                sql.Select("Id")
                   .From("tn_PointStatistics")
                   .Where("UserId = @0", userId)
                   .Where("PointCategoryKey = @0", PointCategoryKey);
                if (statisticalYear.HasValue)
                    sql.Where("StatisticalYear = @0", statisticalYear);
                if (statisticalMonth.HasValue)
                    sql.Where("StatisticalMonth = @0", statisticalMonth);
                if (statisticalDay.HasValue)
                    sql.Where("StatisticalDay = @0", statisticalDay);
                Ids = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey.ToString(), Ids, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds<long>(Ids);
        }

    }
}
