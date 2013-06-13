//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Spacebuilder.Common;
using Spacebuilder.Search;
using Tunynet;
using Tunynet.Common;
using Tunynet.Search;

namespace Spacebuilder.Bar.Search
{
    /// <summary>
    /// 帖吧搜索器
    /// </summary>
    public class BarSearcher : ISearcher
    {
        private BarThreadService barThreadService = new BarThreadService();
        private BarPostService barPostService = new BarPostService();
        private BarSectionService barSectionService = new BarSectionService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().BarThread());
        private CategoryService categoryService = new CategoryService();
        private SearchedTermService searchedTermService = new SearchedTermService();
        private ISearchEngine searchEngine;
        public static string CODE = "BarSearcher";
        public static string WATERMARK = "搜索帖子、回帖";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public BarSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
        }

        #region 搜索器属性

        /// <summary>
        /// 是否作为快捷搜索
        /// </summary>
        public bool AsQuickSearch { get; private set; }

        public string WaterMark { get { return WATERMARK; } }

        /// <summary>
        /// 关联的搜索引擎实例
        /// </summary>
        public ISearchEngine SearchEngine
        {
            get
            {
                return searchEngine;
            }
        }

        /// <summary>
        /// 搜索器的唯一标识
        /// </summary>
        public string Code { get { return CODE; } }

        /// <summary>
        /// 是否前台显示
        /// </summary>
        public bool IsDisplay
        {
            get { return true; }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; private set; }

        /// <summary>
        /// 处理全局搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string GlobalSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().BarGlobalSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// Lucene索引路径（完整物理路径，支持unc）
        /// </summary>
        public string IndexPath { get; private set; }

        /// <summary>
        /// 是否基于Lucene实现
        /// </summary>
        public bool IsBaseOnLucene
        {
            get { return true; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 处理快捷搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string QuickSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().BarQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().BarPageSearch(keyword);
        }

        #endregion

        #region 索引内容维护

        /// <summary>
        /// 重建索引
        /// </summary>  
        public void RebuildIndex()
        {
            bool hasData = false;

            //pageSize参数决定了每次批量取多少条数据进行索引。要注意的是，如果是分布式搜索，客户端会将这部分数据通过WCF传递给服务器端，而WCF默认的最大传输数据量是65535B，pageSize较大时这个设置显然是不够用的，WCF会报400错误；系统现在将最大传输量放宽了，但仍要注意一次不要传输过多，如遇异常，可适当调小pageSize的值
            int pageSizeBarThread = 100;
            int pageIndexBarThread = 1;
            long totalRecordsBarThread = 0;
            bool isBeginningBarThread = true;
            bool isEnddingBarThread = false;
            BarThreadQuery barThreadQuery = new BarThreadQuery();
            do
            {
                //分页获取帖子列表
                PagingDataSet<BarThread> barThreads = barThreadService.Gets(null, barThreadQuery, pageSizeBarThread, pageIndexBarThread);
                totalRecordsBarThread = barThreads.TotalRecords;
                if (totalRecordsBarThread > 0)
                {
                    hasData = true;
                }
                isEnddingBarThread = (pageSizeBarThread * pageIndexBarThread < totalRecordsBarThread) ? false : true;

                //重建索引
                List<BarThread> barThreadList = barThreads.ToList<BarThread>();

                IEnumerable<Document> docs = BarIndexDocument.Convert(barThreadList);

                searchEngine.RebuildIndex(docs, isBeginningBarThread, false);

                isBeginningBarThread = false;
                pageIndexBarThread++;
            }
            while (!isEnddingBarThread);

            int pageSizeBarPost = 100;
            int pageIndexBarPost = 1;
            long totalRecordsBarPost = 0;
            bool isEnddingBarPost = false;
            BarPostQuery barPostQuery = new BarPostQuery();
            do
            {
                //分页获取帖子列表
                PagingDataSet<BarPost> barPosts = barPostService.Gets(null, barPostQuery, pageSizeBarPost, pageIndexBarPost);
                totalRecordsBarPost = barPosts.TotalRecords;
                if (totalRecordsBarPost > 0)
                {
                    hasData = true;
                }
                isEnddingBarPost = (pageSizeBarPost * pageIndexBarPost < totalRecordsBarPost) ? false : true;

                //重建索引
                List<BarPost> barPostList = barPosts.ToList<BarPost>();

                IEnumerable<Document> docs = BarIndexDocument.Convert(barPostList);

                searchEngine.RebuildIndex(docs, false, false);

                pageIndexBarPost++;
            }
            while (!isEnddingBarPost);

            if (hasData)
            {
                searchEngine.RebuildIndex(null, false, true);
            }
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="barThread">待添加的发帖</param>
        public void InsertBarThread(BarThread barThread)
        {
            InsertBarThread(new BarThread[] { barThread });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="barThreads">待添加的发帖</param>
        public void InsertBarThread(IEnumerable<BarThread> barThreads)
        {
            IEnumerable<Document> docs = BarIndexDocument.Convert(barThreads);
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="barPost">待添加的回帖</param>
        public void InsertBarPost(BarPost barPost)
        {
            InsertBarPost(new BarPost[] { barPost });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="barPosts">待添加的回帖</param>
        public void InsertBarPost(IEnumerable<BarPost> barPosts)
        {
            IEnumerable<Document> docs = BarIndexDocument.Convert(barPosts);
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="barThreadId">待删除的发帖Id</param>
        public void DeleteBarThread(long barThreadId)
        {
            searchEngine.Delete(barThreadId.ToString(), BarIndexDocument.ThreadId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="microblogIds">待删除的发帖Id列表</param>
        public void DeleteBarThread(IEnumerable<long> barThreadIds)
        {
            foreach (var barThreadId in barThreadIds)
            {
                searchEngine.Delete(barThreadId.ToString(), BarIndexDocument.ThreadId);
            }
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="barPostId">待删除的回帖Id</param>
        public void DeleteBarPost(long barPostId)
        {
            searchEngine.Delete(barPostId.ToString(), BarIndexDocument.PostId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="microblogIds">待删除的回帖Id列表</param>
        public void DeleteBarPost(IEnumerable<long> barPostIds)
        {
            foreach (var barPostId in barPostIds)
            {
                searchEngine.Delete(barPostId.ToString(), BarIndexDocument.PostId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="barThread">待更新的发帖</param>
        public void UpdateBarThread(BarThread barThread)
        {
            Document doc = BarIndexDocument.Convert(barThread);
            searchEngine.Update(doc, barThread.ThreadId.ToString(), BarIndexDocument.ThreadId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="barThreads">待更新的发帖集合</param>
        public void UpdateBarThread(IEnumerable<BarThread> barThreads)
        {
            IEnumerable<Document> docs = BarIndexDocument.Convert(barThreads);
            IEnumerable<string> barThreadIds = barThreads.Select(n => n.ThreadId.ToString());
            searchEngine.Update(docs, barThreadIds, BarIndexDocument.ThreadId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="barPost">待更新的回帖</param>
        public void UpdateBarPost(BarPost barPost)
        {
            Document doc = BarIndexDocument.Convert(barPost);
            searchEngine.Update(doc, barPost.PostId.ToString(), BarIndexDocument.PostId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="barPosts">待更新的回帖集合</param>
        public void UpdateBarPost(IEnumerable<BarPost> barPosts)
        {
            IEnumerable<Document> docs = BarIndexDocument.Convert(barPosts);
            IEnumerable<string> barPostIds = barPosts.Select(n => n.ThreadId.ToString());
            searchEngine.Update(docs, barPostIds, BarIndexDocument.PostId);
        }

        #endregion

        #region 搜索
        /// <summary>
        /// 微博分页搜索
        /// </summary>
        /// <param name="query">搜索条件</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public PagingDataSet<BarEntity> Search(BarFullTextQuery barQuery)
        {
            if (string.IsNullOrWhiteSpace(barQuery.Keyword))
            {
                return new PagingDataSet<BarEntity>(new List<BarEntity>());
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(barQuery);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, barQuery.PageIndex, barQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的发帖ID
            List<long> barthreadIds = new List<long>();
            //解析出搜索结果中的回帖ID
            List<long> barpostIds = new List<long>();
            //获取索引中发帖的标签
            Dictionary<long, List<string>> barTags = new Dictionary<long, List<string>>();
            //获取帖吧名
            Dictionary<long, string> barSectionNames = new Dictionary<long, string>();
            //获取索引中回帖的标题
            Dictionary<long, string> barPostSubjects = new Dictionary<long, string>();

            foreach (Document doc in docs)
            {
                //是回帖
                if (doc.Get(BarIndexDocument.IsPost) == "1")
                {
                    long postId = long.Parse(doc.Get(BarIndexDocument.PostId));
                    barpostIds.Add(postId);
                    string subject = doc.Get(BarIndexDocument.Subject);
                    barPostSubjects[postId]=subject;
                }
                else
                {
                    long threadId = long.Parse(doc.Get(BarIndexDocument.ThreadId));
                    barthreadIds.Add(threadId);
                    //if (!string.IsNullOrWhiteSpace(doc.Get(BarIndexDocument.Tag)))
                    //{
                    barTags[threadId]=doc.GetValues(BarIndexDocument.Tag).ToList<string>();
                    //}

                }
                long sectionId = long.Parse(doc.Get(BarIndexDocument.SectionId));
                if (!barSectionNames.ContainsKey(sectionId))
                {
                    string sectionName = barSectionService.Get(sectionId).Name;
                    barSectionNames[sectionId]=sectionName;
                }
            }

            //根据发帖ID列表批量查询发帖实例
            IEnumerable<BarThread> barThreadList = barThreadService.GetBarThreads(barthreadIds);
            //根据回帖ID列表批量查询回帖实例
            IEnumerable<BarPost> barPostList = barPostService.GetBarPosts(barpostIds);



            //组装帖子列表
            List<BarEntity> barEntityList = new List<BarEntity>();
            foreach (var barThread in barThreadList)
            {
                BarEntity barEntity = new BarEntity();
                barEntity.SectionId = barThread.SectionId;
                barEntity.ThreadId = barThread.ThreadId;
                barEntity.PostId = 0;
                barEntity.Subject = barThread.Subject;
                barEntity.Body = barThread.GetBody();
                barEntity.UserId = barThread.UserId;
                barEntity.Author = barThread.Author;
                if (barTags.Keys.Contains(barEntity.ThreadId))
                {
                    barEntity.Tag = barTags.Count == 0 ? new List<string>() : barTags[barEntity.ThreadId];
                }
                else {
                    barEntity.Tag = new List<string>();
                }
                if (barSectionNames.Keys.Contains(barEntity.SectionId))
                {
                    barEntity.SectionName = barSectionNames.Count == 0 ? "" : barSectionNames[barEntity.SectionId];   
                }
                else {
                    barEntity.SectionName = "";
                }
                barEntity.DateCreated = barThread.DateCreated;
                barEntity.IsPost = false;
                barEntityList.Add(barEntity);
            }
            foreach (var barPost in barPostList)
            {
                BarEntity barEntity = new BarEntity();
                barEntity.SectionId = barPost.SectionId;
                barEntity.ThreadId = barPost.ThreadId;
                barEntity.PostId = barPost.PostId;
                barEntity.Subject = barPostSubjects[barPost.PostId];
                barEntity.UserId = barPost.UserId;
                barEntity.Body = barPost.GetBody();
                barEntity.Author = barPost.Author;
                barEntity.Tag = null;
                if (barSectionNames.Keys.Contains(barEntity.SectionId))
                {
                    barEntity.SectionName = barSectionNames.Count == 0 ? "" : barSectionNames[barEntity.SectionId];
                }
                else{
                    barEntity.SectionName = "";
                }
                barEntity.DateCreated = barPost.DateCreated;
                barEntity.IsPost = true;
                barEntityList.Add(barEntity);
            }

            //组装分页对象
            PagingDataSet<BarEntity> barEntities = new PagingDataSet<BarEntity>(barEntityList.OrderByDescending(b => b.DateCreated))
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return barEntities;
        }

        /// <summary>
        /// 获取匹配的前几条帖吧热词
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="topNumber">前N条</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public IEnumerable<string> AutoCompleteSearch(string keyword, int topNumber)
        {
            IEnumerable<SearchedTerm> searchedAdminTerms = searchedTermService.GetManuals(keyword, CODE);
            IEnumerable<SearchedTerm> searchedUserTerms = searchedTermService.GetTops(keyword, topNumber, CODE);
            IEnumerable<SearchedTerm> listSearchAdminUserTerms = searchedAdminTerms.Union(searchedUserTerms);
            if (listSearchAdminUserTerms.Count() > topNumber)
            {
                listSearchAdminUserTerms.Take(topNumber);
            }
            return listSearchAdminUserTerms.Select(t => t.Term);
        }

        /// <summary>
        /// 根据帖吧搜索查询条件构建Lucene查询条件
        /// </summary>
        /// <param name="barQuery"></param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(BarFullTextQuery barQuery)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            //微博搜索词匹配范围
            //搜索词匹配范围
            Dictionary<string, BoostLevel> fieldNameAndBoosts = new Dictionary<string, BoostLevel>();
            switch (barQuery.Term)
            {
                case BarSearchRange.SUBJECT:
                    searchBuilder.WithPhrase(BarIndexDocument.Subject, barQuery.Keyword, BoostLevel.Hight, false);
                    break;
                case BarSearchRange.BODY:
                    fieldNameAndBoosts.Add(BarIndexDocument.Body, BoostLevel.Hight);
                    fieldNameAndBoosts.Add(BarIndexDocument.Subject, BoostLevel.Medium);
                    searchBuilder.WithPhrases(fieldNameAndBoosts, barQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    //searchBuilder.WithPhrase(BarIndexDocument.Body, barQuery.Keyword, BoostLevel.Hight, false);
                    break;
                case BarSearchRange.AUTHOR:
                    searchBuilder.WithPhrase(BarIndexDocument.Author, barQuery.Keyword, BoostLevel.Hight, false);
                    break;
                case BarSearchRange.TAG:
                    searchBuilder.WithPhrase(BarIndexDocument.Tag, barQuery.Keyword, BoostLevel.Hight, false);
                    break;
                default:
                    fieldNameAndBoosts.Add(BarIndexDocument.Subject, BoostLevel.Hight);
                    fieldNameAndBoosts.Add(BarIndexDocument.Body, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(BarIndexDocument.Tag, BoostLevel.Medium);
                    fieldNameAndBoosts.Add(BarIndexDocument.Author, BoostLevel.Low);
                    searchBuilder.WithPhrases(fieldNameAndBoosts, barQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                    break;
            }

            //筛选租户类型
            if (!string.IsNullOrEmpty(barQuery.TenantTypeId))
            {
                searchBuilder.WithField(BarIndexDocument.TenantTypeId, barQuery.TenantTypeId, true, BoostLevel.Hight, true);
            }

            //帖吧搜索条件过滤(全吧、吧内)
            if (barQuery.Range != "-1")
            {
                searchBuilder.WithField(BarIndexDocument.SectionId, barQuery.Range, true, BoostLevel.Hight, true);
            }

            //帖吧搜索条件过滤(帖子、回帖)
            if (barQuery.IsPost == "1")
            {
                searchBuilder.WithField(BarIndexDocument.IsPost, "1", true, BoostLevel.Hight, true);
            }
            if (barQuery.IsPost == "0")
            {
                searchBuilder.WithField(BarIndexDocument.IsPost, "0", true, BoostLevel.Hight, true);
            }

            //帖吧排序
            searchBuilder.SortByString(BarIndexDocument.DateCreated, true);

            return searchBuilder;
        }
        #endregion

    }
}