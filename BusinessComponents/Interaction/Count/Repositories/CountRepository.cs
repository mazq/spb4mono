//using Tunynet.Repositories;
//using System.Collections.Generic;
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
using Tunynet.Caching;
using System.Collections.Concurrent;
using System.Configuration;
using PetaPoco;
using System.Threading;

namespace Tunynet.Common
{
    /// <summary>
    /// Count仓储
    /// </summary>
    public class CountRepository : ICountRepository
    {
        private int pageSize = 20;
        private static ConcurrentDictionary<string, Queue<CountQueueItem>> CountQueue = new ConcurrentDictionary<string, Queue<CountQueueItem>>();
        private static ConcurrentDictionary<string, Queue<CountQueueItem>> CountPerDayQueue = new ConcurrentDictionary<string, Queue<CountQueueItem>>();

        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 无参构造器
        /// </summary>
        public CountRepository()
        {

        }

        /// <summary>
        /// 无参构造器
        /// </summary>
        /// <remarks>仅用于单元测试</remarks>
        public CountRepository(ConcurrentDictionary<string, Queue<CountQueueItem>> countQueue, ConcurrentDictionary<string, Queue<CountQueueItem>> countPerDayQueue)
        {
            CountQueue = countQueue;
            CountPerDayQueue = countPerDayQueue;
        }

        /// <summary>
        /// 默认PetaPocoDatabase实例
        /// </summary>
        protected PetaPocoDatabase CreateDAO()
        {
            return PetaPocoDatabase.CreateInstance();
        }

        /// <summary>
        /// 注册计数
        /// </summary>
        public void CheckCountTable(string tenantTypeId)
        {
            //检查数据库表是否已创建,如果不存在则使用SQL脚本创建            
            var builder = new StringBuilder();
            
            #region 创建计数表脚本
            builder.AppendFormat(@"if not exists (select * from sysobjects where [name] = '{0}' and xtype='U')
                begin
                CREATE TABLE [{0}](
                    [CountId] [bigint] IDENTITY(1,1) NOT NULL,
                    [OwnerId] [bigint] NOT NULL,
                    [ObjectId] [bigint] NOT NULL,
                    [CountType] [nvarchar](64) NOT NULL,
                    [StatisticsCount] [int] NOT NULL,
                 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
                (
                    [CountId] ASC
                ) ON [PRIMARY]
                ) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [IX_{0}_CountType] ON [{0}] 
                (
                    [CountType] ASC
                ) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [IX_{0}_ObjectId] ON [{0}] 
                (
                    [ObjectId] ASC
                ) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [IX_{0}_OwnerId] ON [{0}] 
                (
                    [OwnerId] ASC
                ) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [IX_{0}_StatisticsCount] ON [{0}] 
                (
                    [StatisticsCount] ASC
                ) ON [PRIMARY]

                ALTER TABLE [{0}]  ADD  CONSTRAINT [DF_{0}_OwnerId]  DEFAULT ((0)) FOR [OwnerId]
                ALTER TABLE [{0}]  ADD  CONSTRAINT [DF_{0}_StatisticsCount]  DEFAULT ((0)) FOR [StatisticsCount]
                end
  ", GetTableName_Counts(tenantTypeId));

            #endregion

            CreateDAO().Execute(builder.ToString(), null);
        }

        /// <summary>
        /// 注册每日计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public void CheckCountPerDayTable(string tenantTypeId)
        {
            //检查数据库表是否已创建,如果不存在则使用SQL脚本创建
            var builder = new StringBuilder();
            

            #region 创建计数表脚本
            builder.AppendFormat(@"if not exists (select * from sysobjects where [name] = '{0}' and xtype='U')
            begin
            CREATE TABLE [dbo].[{0}](
                [Id] [bigint] IDENTITY(1,1) NOT NULL,
                [OwnerId] [bigint] NOT NULL,
                [ObjectId] [bigint] NOT NULL,
                [ReferenceYear] [int] NOT NULL,
                [ReferenceMonth] [int] NOT NULL,
                [ReferenceDay] [int] NOT NULL,
                [StatisticsCount] [int] NOT NULL,
                [CountType] [nvarchar](64) NOT NULL,
             CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
            (
                [Id] ASC
            ) ON [PRIMARY]
            ) ON [PRIMARY]
            CREATE NONCLUSTERED INDEX [IX_{0}_ObjectId] ON [dbo].[{0}] 
            (
                [ObjectId] ASC
            ) ON [PRIMARY]
            CREATE NONCLUSTERED INDEX [IX_{0}_OwnerId] ON [dbo].[{0}] 
            (
                [OwnerId] ASC
            ) ON [PRIMARY]
            CREATE NONCLUSTERED INDEX [IX_{0}_ReferenceDay] ON [dbo].[{0}] 
            (
                [ReferenceDay] ASC
            ) ON [PRIMARY]
            CREATE NONCLUSTERED INDEX [IX_{0}_ReferenceMonth] ON [dbo].[{0}] 
            (
                [ReferenceMonth] ASC
            ) ON [PRIMARY]
            CREATE NONCLUSTERED INDEX [IX_{0}_ReferenceYear] ON [dbo].[{0}] 
            (
                [ReferenceYear] ASC
            ) ON [PRIMARY]
            ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_OwnerId]  DEFAULT ((0)) FOR [OwnerId]
            ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_StatisticsCount]  DEFAULT ((0)) FOR [StatisticsCount]
            end
  ", GetTableName_CountsPerDay(tenantTypeId));

            #endregion

            CreateDAO().Execute(builder.ToString(), null);
        }

        /// <summary>
        /// 调整计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="objectId">计数对象Id</param>
        /// <param name="ownerId">ownerId</param>
        /// <param name="changeCount">变化数</param>
        /// <param name="stageCountTypes">阶段计数集合</param>
        /// <param name="isRealTime">是否立即更新显示计数</param>
        public void ChangeCount(string tenantTypeId, string countType, long objectId, long ownerId, int changeCount = 1, IList<string> stageCountTypes = null, bool isRealTime = false)
        {


            //1.更新计数队列，使其Count+=changeCount
            if (!CountQueue.ContainsKey(tenantTypeId))
                CountQueue[tenantTypeId] = new Queue<CountQueueItem>();
            Queue<CountQueueItem> countList = CountQueue[tenantTypeId];

            List<string> countTypes = new List<string>() { countType };
            //同时维护阶段计数
            if (stageCountTypes != null)
                countTypes.AddRange(stageCountTypes);
            foreach (var cType in countTypes)
            {
                IEnumerable<CountQueueItem> countQueueItems = countList.Where(n => n.ObjectId == objectId && n.CountType == cType);
                CountQueueItem countQueueItem = null;
                if (countQueueItems != null)
                    countQueueItem = countQueueItems.FirstOrDefault();
                if (countQueueItem == null)
                {
                    countQueueItem = new CountQueueItem(cType, objectId, ownerId, changeCount);
                    countList.Enqueue(countQueueItem);
                }
                else
                    countQueueItem.StatisticsCount += changeCount;

                //2.维护及时性
                if (isRealTime)
                {
                    int count = Get(tenantTypeId, cType, objectId);
                    count += changeCount;
                    string cacheKey = GetCacheKey_Count(tenantTypeId, cType, objectId);

                    cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
                }
            }

            //3.根据tenantTypeId、countType、objectId、ownerId更新每日计数队列CountPerDayQueue，使其Count+=changeCount
            //更新每日计数时，还需要检查当前日期是否已存在,如果记录不存在还需要创建
            if (stageCountTypes != null)
            {
                if (!CountPerDayQueue.ContainsKey(tenantTypeId))
                    CountPerDayQueue[tenantTypeId] = new Queue<CountQueueItem>();
                CountQueueItem countPerDayQueueItem = CountPerDayQueue[tenantTypeId].Where(n => n.ObjectId == objectId && n.CountType == countType).FirstOrDefault();
                if (countPerDayQueueItem == null)
                {
                    countPerDayQueueItem = new CountQueueItem(countType, objectId, ownerId, changeCount);
                    CountPerDayQueue[tenantTypeId].Enqueue(countPerDayQueueItem);
                }
                else
                    countPerDayQueueItem.StatisticsCount += changeCount;
            }
        }

        /// <summary>
        /// 执行队列
        /// </summary>
        public void ExecQueue()
        {
            Database database = CreateDAO();

            try
            {
                database.OpenSharedConnection();

                //将CountQueue中的数据更新至计数表
                foreach (string key in CountQueue.Keys)
                {
                    Queue<CountQueueItem> queue = null;
                    bool removed = CountQueue.TryRemove(key, out queue);
                    if (!removed || queue == null)
                    {
                        continue;
                    }

                    string countTableName = GetTableName_Counts(key);

                    while (queue.Count > 0)
                    {
                        var item = queue.Dequeue();

                        //需要判断数据库中是否存在记录
                        var sql = PetaPoco.Sql.Builder;
                        sql.Select("CountId")
                            .From(countTableName)
                            .Where("ObjectId=@0 and CountType=@1", item.ObjectId, item.CountType);

                        CountEntity countEntity = database.FirstOrDefault<CountEntity>(sql);

                        if (countEntity == null)
                        {
                            database.Insert(countTableName, "CountId", item.AsCountEntity());
                        }
                        else
                        {
                            sql = PetaPoco.Sql.Builder;
                            sql.Append("Update " + countTableName)
                            .Append("Set StatisticsCount = StatisticsCount + @0 ", item.StatisticsCount)
                            .Where("ObjectId=@0", item.ObjectId)
                            .Where("CountType=@0", item.CountType);

                            database.Execute(sql);
                        }
                    }
                }

                //将CountPerDayQueue中的数据更新每日计数表
                foreach (string key in CountPerDayQueue.Keys)
                {
                    Queue<CountQueueItem> queue = null;
                    bool removed = CountPerDayQueue.TryRemove(key, out queue);
                    if (!removed || queue == null)
                    {
                        continue;
                    }

                    string countPerDayTableName = GetTableName_CountsPerDay(key);

                    while (queue.Count > 0)
                    {
                        var item = queue.Dequeue();

                        var sql = PetaPoco.Sql.Builder;
                        sql.Select("Id")
                            .From(countPerDayTableName)
                            .Where("ObjectId=@0 and CountType=@1 and ReferenceYear=@2 and ReferenceMonth=@3 and ReferenceDay=@4", item.ObjectId, item.CountType, DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

                        CountPerDayEntity countPerDayEntity = database.FirstOrDefault<CountPerDayEntity>(sql);

                        if (countPerDayEntity == null)
                        {
                            database.Insert(countPerDayTableName, "Id", item.AsCountPerDayEntity());
                        }
                        else
                        {
                            sql = PetaPoco.Sql.Builder;
                            sql.Append("Update " + countPerDayTableName)
                            .Append("Set StatisticsCount = StatisticsCount + @0 ", item.StatisticsCount)
                            .Where("ObjectId=@0", item.ObjectId)
                            .Where("CountType=@0", item.CountType);

                            database.Execute(sql);
                        }
                    }
                }
            }
            finally
            {
                database.CloseSharedConnection();
            }
        }

        /// <summary>
        /// 批量更新计数表中的阶段计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="countType2Days"> 计数类型 -统计天数字典集合</param>
        public void UpdateStageCountPerDay(string tenantTypeId, string countType, Dictionary<string, int> countType2Days)
        {
            string countTableName = GetTableName_Counts(tenantTypeId);
            string countPerDayTableName = GetTableName_CountsPerDay(tenantTypeId);

            Database database = CreateDAO();
            database.OpenSharedConnection();
            foreach (var countType2Day in countType2Days)
            {
                //阶段计数统计天数的前一天
                DateTime dateTime = DateTime.UtcNow.AddDays(-countType2Day.Value);
                StringBuilder builder = new StringBuilder();

                builder.AppendFormat(@"update dbo.{0} set StatisticsCount = 
	                                    (case when(select SUM(StatisticsCount) from {1} 
				                                    where {1}.ObjectId = {0}.ObjectId
				                                    and right(10000 + {1}.ReferenceYear ,4) + right(100+ {1}.ReferenceMonth ,2) + right(100+ {1}.ReferenceDay ,2) >'{4}' 
				                                    and right(10000 + {1}.ReferenceYear ,4) + right(100+ {1}.ReferenceMonth ,2) + right(100+ {1}.ReferenceDay ,2)<='{5}' 
				                                    and {1}.CountType='{2}')>0
			                                    then(
			                                    select SUM(StatisticsCount) from {1} 
				                                    where {1}.ObjectId = {0}.ObjectId
				                                    and right(10000 + {1}.ReferenceYear ,4) + right(100+ {1}.ReferenceMonth ,2) + right(100+ {1}.ReferenceDay ,2) >'{4}' 
				                                    and right(10000 + {1}.ReferenceYear ,4) + right(100+ {1}.ReferenceMonth ,2) + right(100+ {1}.ReferenceDay ,2)<='{5}' 
				                                    and {1}.CountType='{2}'
			                                    )else 0 end)
	                                    where CountType= '{3}'"
                    , countTableName, countPerDayTableName
                    , countType, countType2Day.Key
                    , dateTime.ToString("yyyyMMdd"), DateTime.UtcNow.ToString("yyyyMMdd"));

                database.Execute(builder.ToString(), null);
            }
            database.CloseSharedConnection();
        }

        /// <summary>
        /// 删除每日计数表中的过期的历史计数记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="maxValue">保留记录的最大天数</param>
        public void DeleteTrashCountPerDays(string tenantTypeId, string countType, int maxValue)
        {
            var sql = PetaPoco.Sql.Builder;
            DateTime dateTime = DateTime.UtcNow.AddDays(-maxValue);
            sql.Append("delete from " + GetTableName_CountsPerDay(tenantTypeId))
            .Where("CountType=@0", countType)
            .Where("CAST(ReferenceYear AS varchar(4)) + right(100 + ReferenceMonth,2) + right(100 + ReferenceDay,2)<=@0", dateTime.Year.ToString() + dateTime.Month.ToString().PadLeft(2, '0') + dateTime.Day.ToString().PadLeft(2, '0'));
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 删除垃圾数据
        /// </summary>
        public void DeleteTrashCount()
        {
            TenantTypeService tenantTypeService = new TenantTypeService();
            IEnumerable<TenantType> tenantTypes = tenantTypeService.Gets(MultiTenantServiceKeys.Instance().Count());
            List<Sql> sqls = new List<Sql>();
            foreach (var tenantType in tenantTypes)
            {
                Type type = Type.GetType(tenantType.ClassType);
                if (type == null)
                    continue;
                var pd = PetaPoco.Database.PocoData.ForType(type);
                sqls.Add(Sql.Builder.Append("delete from " + GetTableName_Counts(tenantType.TenantTypeId))
                .Where("not exists (select 1 from " + pd.TableInfo.TableName + " where ObjectId = " + pd.TableInfo.PrimaryKey + ")"));
            }
            CreateDAO().Execute(sqls);
        }

        /// <summary>
        /// 获取计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="objectId">计数对象Id</param>
        public int Get(string tenantTypeId, string countType, long objectId)
        {
            string cacheKey = GetCacheKey_Count(tenantTypeId, countType, objectId);
            //拼写SQL语句在tn_Counts_@1表中进行查询
            int? count = cacheService.GetFromFirstLevel(cacheKey) as int?;
            if (count == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("StatisticsCount")
                .From(GetTableName_Counts(tenantTypeId))
                .Where("ObjectId=@0 and CountType=@1", objectId, countType);
                count = CreateDAO().FirstOrDefault<int?>(sql);
                count = count ?? 0;
                cacheService.Set(cacheKey, count, CachingExpirationType.SingleObject);
            }
            return count ?? 0;
        }

        /// <summary>
        /// 删除计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="objectId">计数对象Id</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public bool Delete(string tenantTypeId, long objectId)
        {
            Database database = CreateDAO();

            //需要同时删除计数表和每日计数表
            database.OpenSharedConnection();

            var sql = PetaPoco.Sql.Builder;
            sql.Append("delete from " + GetTableName_Counts(tenantTypeId))
            .Where("ObjectId=@0", objectId);
            int count = database.Execute(sql);

            sql = PetaPoco.Sql.Builder;
            sql.Append("delete from " + GetTableName_CountsPerDay(tenantTypeId))
            .Where("ObjectId=@0", objectId);
            int countPerDay = database.Execute(sql);

            database.CloseSharedConnection();

            return count > 0 && countPerDay > 0;
        }

        /// <summary>
        /// 获取计数对象Id的所有每天计数记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <returns>每天计数记录集合</returns>
        public IEnumerable<CountPerDayEntity> GetAllCountPerDays(string tenantTypeId, string countType)
        {
            string cacheKey = string.Format("AllCountPerDays::TenantTypeId-{0}:CountType-{1}", tenantTypeId, countType);
            //拼写SQL语句在tn_Counts_@1表中进行查询
            IEnumerable<CountPerDayEntity> countPerDayEntitys = cacheService.Get<IEnumerable<CountPerDayEntity>>(cacheKey);
            if (countPerDayEntitys == null || countPerDayEntitys.Count() == 0)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("*")
                .From(GetTableName_CountsPerDay(tenantTypeId))
                .Where("CountType=@0", countType);
                countPerDayEntitys = CreateDAO().Fetch<CountPerDayEntity>(sql);

                cacheService.Set(cacheKey, countPerDayEntitys, CachingExpirationType.ObjectCollection);
            }
            return countPerDayEntitys;
        }

        /// <summary>
        /// 获取计数对象Id集合
        /// </summary>
        /// <remarks>
        /// 一次性取出前SecondaryMaxRecords条记录
        /// </remarks>
        /// <param name="topNumber">准备获取的条数</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>计数对象Id集合</returns>
        public IEnumerable<long> GetTops(int topNumber, string tenantTypeId, string countType, long? ownerId = null)
        {
            //从计数表中查询，并按统计数倒序排序
            PagingEntityIdCollection peic = null;
            string cacheKey = GetCacheKey_Counts(tenantTypeId, countType, ownerId);
            peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (peic == null)
            {
                IEnumerable<object> entityIds = CreateDAO().FetchTopPrimaryKeys(SecondaryMaxRecords, "ObjectId", GetsSql(tenantTypeId, countType, ownerId));
                peic = new PagingEntityIdCollection(entityIds);
                cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
            }

            IEnumerable<object> topEntityIds = peic.GetTopEntityIds(topNumber);
            return topEntityIds.Cast<long>();
        }

        /// <summary>
        /// 获取计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="pageIndex">页码数</param>
        public PagingEntityIdCollection Gets(string tenantTypeId, string countType, long? ownerId = null, int pageIndex = 1)
        {
            //从计数表中查询，并按统计数倒序排序
            PagingEntityIdCollection peic = null;
            if (pageIndex < CacheablePageCount)
            {
                string cacheKey = GetCacheKey_Counts(tenantTypeId, countType, ownerId);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = CreateDAO().FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "ObjectId", GetsSql(tenantTypeId, countType, ownerId));
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = CreateDAO().FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "ObjectId", GetsSql(tenantTypeId, countType, ownerId));
            }

            return peic;
        }

        /// <summary>
        /// 获取计数表名
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>计数表名</returns>
        public string GetTableName_Counts(string tenantTypeId)
        {
            return "tn_Counts_" + tenantTypeId;
        }

        /// <summary>
        /// 获取每日计数表名
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>每日计数表名</returns>
        public string GetTableName_CountsPerDay(string tenantTypeId)
        {
            return "tn_CountsPerDay_" + tenantTypeId;
        }


        /// <summary>
        /// 获取计数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="ownerId">拥有者Id</param>
        private PetaPoco.Sql GetsSql(string tenantTypeId, string countType, long? ownerId = null)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("ObjectId")
            .From(GetTableName_Counts(tenantTypeId))
            .Where("CountType=@0", countType);
            if (ownerId.HasValue && ownerId.Value > 0)
                sql.Where("OwnerId=@0", ownerId);

            sql.OrderBy("StatisticsCount  DESC");
            return sql;
        }

        #region Help Methods
        /// <summary>
        /// 获取计数Id集合的CacheKey
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="ownerId">拥有者Id</param>
        private string GetCacheKey_Counts(string tenantTypeId, string countType, long? ownerId)
        {
            return string.Format("Counts::TenantTypeId-{0}:CountType-{1}:OwnerId-{2}", tenantTypeId, countType, ownerId);
        }


        /// <summary>
        /// 获取计数的CacheKey
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="countType">计数类型</param>
        /// <param name="objectId">计数类型id</param>
        /// <returns></returns>
        private string GetCacheKey_Count(string tenantTypeId, string countType, long objectId)
        {
            return string.Format("Count::TenantTypeId:{0}-CountType:{1}-ObjectId:{2}", tenantTypeId, countType, objectId);
        }

        #endregion

        #region 配置属性

        private int cacheablePageCount = 30;
        /// <summary>
        /// 可缓存的列表缓存页数
        /// </summary>
        protected virtual int CacheablePageCount
        {
            get { return cacheablePageCount; }
        }

        private int primaryMaxRecords = 50000;
        /// <summary>
        /// 主流查询最大允许返回记录数
        /// </summary>
        protected virtual int PrimaryMaxRecords
        {
            get { return primaryMaxRecords; }
        }

        private int secondaryMaxRecords = 1000;
        /// <summary>
        /// 非主流查询最大允许返回记录数
        /// </summary>
        /// <remarks>
        /// 例如：排行数据
        /// </remarks>
        protected virtual int SecondaryMaxRecords
        {
            get { return secondaryMaxRecords; }
        }


        #endregion
    }
}
