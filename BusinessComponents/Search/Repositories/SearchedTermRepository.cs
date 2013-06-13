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
using Tunynet.Common;

namespace Tunynet.Search.Repositories
{
    /// <summary>
    /// 搜索热词仓储
    /// </summary>
    public class SearchedTermRepository : Repository<SearchedTerm>, ISearchedTermRepository
    {

        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 添加或更新搜索热词
        /// </summary>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词</param>
        /// <param name="isAddedByAdministrator">是否由管理员添加</param>
        public long InsertOrUpdate(string searchTypeCode, string term, bool isAddedByAdministrator)
        {
            SearchedTerm searchedTerm = SearchedTerm.New();
            var sql = Sql.Builder;

            sql.Select("*")
               .From("tn_SearchedTerms")
               .Where("SearchTypeCode = @0", searchTypeCode)
               .Where("Term = @0", term);
            List<SearchedTerm> searchedTerms = CreateDAO().Fetch<SearchedTerm>(sql);

            if (searchedTerms.Count() > 0)
            {
                searchedTerm = searchedTerms.First();
                if (isAddedByAdministrator)
                    searchedTerm.IsAddedByAdministrator = isAddedByAdministrator;
                searchedTerm.LastModified = DateTime.UtcNow;
                base.Update(searchedTerm);
            }
            else
            {
                long id = IdGenerator.Next();
                searchedTerm.Id = id;

                searchedTerm.DisplayOrder = id;
                searchedTerm.SearchTypeCode = searchTypeCode;
                searchedTerm.Term = term;
                searchedTerm.IsAddedByAdministrator = isAddedByAdministrator;
                base.Insert(searchedTerm);
            }

            if (isAddedByAdministrator)
            {
                RealTimeCacheHelper.IncreaseAreaVersion("SearchTypeCode", string.Empty);
                RealTimeCacheHelper.IncreaseAreaVersion("SearchTypeCode", searchedTerm.SearchTypeCode);
            }
            return searchedTerm.Id;
        }


        /// <summary>
        /// 更新管理员添加的搜索词
        /// </summary>
        /// <param name="id">搜索词Id</param>
        /// <param name="term">搜索词</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        public void Update(long id, string term, string searchTypeCode)
        {
            SearchedTerm searchedTerm = SearchedTerm.New();

            var sql = Sql.Builder;
            sql.Select("*")
               .From("tn_SearchedTerms")
               .Where("SearchTypeCode = @0", searchTypeCode)
               .Where("Term = @0", term);
            List<SearchedTerm> searchedTerms = CreateDAO().Fetch<SearchedTerm>(sql);
            if (searchedTerms.Count() > 0)
            {
                searchedTerm = searchedTerms.First();
                if (searchedTerm.Id != id)
                {
                    searchedTerm.IsAddedByAdministrator = true;
                    base.Update(searchedTerm);
                    base.Delete(Get(id));

                }
                else
                    base.Update(searchedTerm);
            }

            else
            {
                searchedTerm = Get(id);
                if (searchedTerm != null)
                {
                    searchedTerm.Term = term;
                    searchedTerm.SearchTypeCode = searchTypeCode;
                    base.Update(searchedTerm);
                }
            }
            RealTimeCacheHelper.IncreaseAreaVersion("SearchTypeCode", string.Empty);
            RealTimeCacheHelper.IncreaseAreaVersion("SearchTypeCode", searchTypeCode);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityId">主键</param>
        /// <returns>影响的记录数</returns>
        public override int DeleteByEntityId(object entityId)
        {
            SearchedTerm searchedTerm = Get(entityId);
            if (searchedTerm == null)
                return 0;

            return Delete(searchedTerm);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响的记录数</returns>
        public override int Delete(SearchedTerm entity)
        {
            int record = base.Delete(entity);
            RealTimeCacheHelper.IncreaseAreaVersion("SearchTypeCode", entity.SearchTypeCode);
            return record;
        }


        /// <summary>
        /// 获取人工干预的搜索词
        /// </summary>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>符合条件的搜索词集合</returns>
        public IEnumerable<SearchedTerm> GetManuals(string searchTypeCode)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "SearchTypeCode", searchTypeCode));
            IEnumerable<long> ids = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());

            if (ids == null)
            {
                var sql = Sql.Builder;
                sql.Select("id")
                   .From("tn_SearchedTerms");
                if (!String.IsNullOrEmpty(searchTypeCode))
                    sql.Where("SearchTypeCode = @0", searchTypeCode);
                sql.Where("IsAddedByAdministrator = @0", 1)
                   .OrderBy("DisplayOrder");
                ids = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey.ToString(), ids, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds<long>(ids);

        }

        /// <summary>
        /// 获取匹配的人工干预的搜索词
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>符合条件的搜索词集合</returns>
        public IEnumerable<SearchedTerm> GetManuals(string keyword, string searchTypeCode)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return null;
            }
            var sql = Sql.Builder;
            sql.Select("id")
               .From("tn_SearchedTerms")
               .Where("Term like @0", keyword + "%");
            if (!String.IsNullOrEmpty(searchTypeCode))
                sql.Where("SearchTypeCode = @0", searchTypeCode);
            sql.Where("IsAddedByAdministrator = @0", 1)
               .OrderBy("DisplayOrder");
            return PopulateEntitiesByEntityIds<long>(CreateDAO().Fetch<long>(sql));
        }

        /// <summary>
        /// 获取前N条最热的搜索词（仅非人工干预）
        /// </summary>
        /// <param name="topNumber">获取的数据条数</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>用户最多搜索的前topNumber的搜索词</returns>
        public IEnumerable<SearchedTerm> GetTops(int topNumber, string searchTypeCode)
        {
            CountService countService = new CountService(TenantTypeIds.Instance().Search());
            StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Search());
            int stageCountDays = stageCountTypeManager.GetMaxDayCount(CountTypes.Instance().SearchCount());
            string countType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().SearchCount(), stageCountDays);
            string countTableName = countService.GetTableName_Counts();

            return GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.None));
                    cacheKey.AppendFormat("SearchTypeCode-{0}::CountType-{1}", searchTypeCode, countType);
                    return cacheKey.ToString();
                },
                () =>
                {
                    var sql = Sql.Builder;
                    sql.Select("*")
                       .From("tn_SearchedTerms")
                       .InnerJoin(countTableName)
                       .On("tn_SearchedTerms.Id = " + countTableName + ".ObjectId");
                    if (!String.IsNullOrEmpty(searchTypeCode))
                        sql.Where("SearchTypeCode = @0", searchTypeCode);
                    sql.Where("IsAddedByAdministrator = @0", 0)
                       .Where("CountType = @0", countType)
                       .Where("OwnerId = @0", 0)
                       .OrderBy("StatisticsCount desc");
                    return sql;

                });
        }

        /// <summary>
        /// 获取匹配的前N条最热的搜索词（仅非人工干预）
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="topNumber">获取的数据条数</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>用户最多搜索的前topNumber的搜索词</returns>
        public IEnumerable<SearchedTerm> GetTops(string keyword, int topNumber, string searchTypeCode)
        {
            CountService countService = new CountService(TenantTypeIds.Instance().Search());
            StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Search());
            int stageCountDays = stageCountTypeManager.GetMaxDayCount(CountTypes.Instance().SearchCount());
            string countType = stageCountTypeManager.GetStageCountType(CountTypes.Instance().SearchCount(), stageCountDays);
            string countTableName = countService.GetTableName_Counts();

            return GetTopEntities(topNumber, CachingExpirationType.ObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.None));
                    cacheKey.AppendFormat("SearchTypeCode-{0}::CountType-{1}::Keyword-{2}", searchTypeCode, countType, keyword);
                    return cacheKey.ToString();
                },
                () =>
                {
                    var sql = Sql.Builder;
                    sql.Select("*")
                       .From("tn_SearchedTerms")
                       .InnerJoin(countTableName)
                       .On("tn_SearchedTerms.Id = " + countTableName + ".ObjectId")
                       .Where("tn_SearchedTerms.Term like @0", keyword + "%");
                    if (!String.IsNullOrEmpty(searchTypeCode))
                        sql.Where("SearchTypeCode = @0", searchTypeCode);
                    sql.Where("IsAddedByAdministrator = @0", 0)
                       .Where("CountType = @0", countType)
                       .Where("OwnerId = @0", 0)
                       .OrderBy("StatisticsCount desc");
                    return sql;

                });
        }

        /// <summary>
        /// 分页获取搜索词（仅非人工干预）
        /// </summary>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isRealtime">是否实时缓存</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>按条件检索的热词</returns>
        public PagingDataSet<SearchedTerm> Gets(string searchTypeCode, string term, DateTime? startDate, DateTime? endDate, bool isRealtime,int pageSize, int pageIndex)
        {
            CountService countService = new CountService(TenantTypeIds.Instance().Search());
            string countTableName = countService.GetTableName_Counts();
            string countType = CountTypes.Instance().SearchCount();

            StageCountTypeManager stageCountTypeManager = StageCountTypeManager.Instance(TenantTypeIds.Instance().Search());
            int stageCountDays = stageCountTypeManager.GetMaxDayCount(CountTypes.Instance().SearchCount());

            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
                () =>
                {
                    //done:zhangp,by mazq cacheKey应该与查询条件关联
                    StringBuilder cacheKey;
                    if (isRealtime)
                        cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.GlobalVersion));
                    else
                        cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "SearchTypeCode", searchTypeCode));

                    cacheKey.AppendFormat("SearchTypeCode-{0}::Term-{1}::StartData-{2}::EndData-{3}", searchTypeCode, term, startDate, endDate);
                    return cacheKey.ToString();
                },
                () =>
                {
                    var sql = Sql.Builder;
                    sql.Select("*")
                       .From("tn_SearchedTerms")
                       .InnerJoin(countTableName)
                       .On("tn_SearchedTerms.Id = " + countTableName + ".ObjectId")
                       .Where("IsAddedByAdministrator = @0", 0)
                       .Where("CountType = @0", countType)
                       .Where("OwnerId = @0", 0);

                    if (!String.IsNullOrEmpty(searchTypeCode))
                        sql.Where("SearchTypeCode = @0", searchTypeCode);

                    if (!String.IsNullOrEmpty(term))
                        sql.Where("Term like @0", term + "%");

                    if (startDate.HasValue)
                        sql.Where("DateCreated >= @0", startDate);
                    if (endDate.HasValue)
                        sql.Where("DateCreated <= @0", endDate);
                    sql.OrderBy("StatisticsCount desc");
                    return sql;
                }
            );
        }

        /// <summary>
        /// 判定搜索词的DisplayOrder可以做哪些调整
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="isFirst">在人工干预同类型搜索词中是否处于首位</param>
        /// <param name="isLast">在人工干预同类型搜索词中是否处于末位</param>
        /// <param name="decreaseReferenceId">降低DisplayOrder时参照的Id</param>
        /// <param name="increaseReferenceId">增加DisplayOrder时参照的Id</param>
        public void JudgeDisplayOrder(long id, out bool isFirst, out bool isLast, out long decreaseReferenceId, out long increaseReferenceId)
        {
            CountService countService = new CountService(TenantTypeIds.Instance().Search());
            string countTableName = countService.GetTableName_Counts();
            int index;
            isFirst = false;
            isLast = false;
            decreaseReferenceId = 0;
            increaseReferenceId = 0;

            SearchedTerm searchedTerm = Get(id);
            if (searchedTerm == null)
                return;

            IEnumerable<SearchedTerm> isearchedTerms = this.GetManuals(searchedTerm.SearchTypeCode);
            if (isearchedTerms == null || isearchedTerms.Count() == 0)
                return;

            List<SearchedTerm> searchedTerms = isearchedTerms.ToList();
            searchedTerm = searchedTerms.FirstOrDefault(n => n.Id == id);
            index = searchedTerms.IndexOf(searchedTerm);


            if (searchedTerms.Count() == 1)
            {
                isFirst = true;
                isLast = true;
            }

            else if (index == 0)
            {
                isFirst = true;
                increaseReferenceId = searchedTerms.ElementAt(index + 1).Id;
            }
            else if (index == searchedTerms.Count() - 1)
            {
                isLast = true;
                decreaseReferenceId = searchedTerms.ElementAt(index - 1).Id;
            }
            else
            {
                increaseReferenceId = searchedTerms.ElementAt(index + 1).Id;
                decreaseReferenceId = searchedTerms.ElementAt(index - 1).Id;
            }
        }



        /// <summary>
        /// 变更搜索词的排列顺序
        /// </summary>
        /// <param name="id">待调整的Id</param>
        /// <param name="referenceId">参照Id</param>        
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            SearchedTerm searchedTermFir = Get(id);
            SearchedTerm searchedTermSec = Get(referenceId);

            if (searchedTermFir.IsAddedByAdministrator && searchedTermSec.IsAddedByAdministrator && searchedTermFir.SearchTypeCode == searchedTermSec.SearchTypeCode)
            {
                long idMid = searchedTermSec.Id;

                //done:zhangp,by mazq 为什么要修改Id？

                long displayOrder_Sec = searchedTermSec.DisplayOrder;
                searchedTermSec.DisplayOrder = searchedTermFir.DisplayOrder;
                searchedTermFir.DisplayOrder = displayOrder_Sec;

                base.Update(searchedTermFir);
                base.Update(searchedTermSec);
                
                RealTimeCacheHelper.IncreaseAreaVersion("SearchTypeCode", string.Empty);
                RealTimeCacheHelper.IncreaseAreaVersion("SearchTypeCode", searchedTermFir.SearchTypeCode);
            }

        }
    }
}
