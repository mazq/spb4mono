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

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 日志搜索器
    /// </summary>
    public class BlogSearcher : ISearcher
    {
        private BlogService blogThreadService = new BlogService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().BlogThread());
        private CategoryService categoryService = new CategoryService();
        private AuditService auditService = new AuditService();
        private ISearchEngine searchEngine;
        private PubliclyAuditStatus publiclyAuditStatus;
        private SearchedTermService searchedTermService = new SearchedTermService();
        public static string CODE = "BlogSearcher";
        public static string WATERMARK = "搜索日志";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public BlogSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
            publiclyAuditStatus = auditService.GetPubliclyAuditStatus(BlogConfig.Instance().ApplicationId);
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
            return SiteUrls.Instance().BlogGlobalSearch() + "?keyword=" + keyword;
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
            return SiteUrls.Instance().BlogQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().BlogPageSearch(keyword);
        }

        #endregion

        #region 索引内容维护


        /// <summary>
        /// 重建索引
        /// </summary>  
        public void RebuildIndex()
        {
            //pageSize参数决定了每次批量取多少条数据进行索引。要注意的是，如果是分布式搜索，客户端会将这部分数据通过WCF传递给服务器端，而WCF默认的最大传输数据量是65535B，pageSize较大时这个设置显然是不够用的，WCF会报400错误；系统现在将最大传输量放宽了，但仍要注意一次不要传输过多，如遇异常，可适当调小pageSize的值
            int pageSize = 1000;
            int pageIndex = 1;
            long totalRecords = 0;
            bool isBeginning = true;
            bool isEndding = false;
            do
            {
                //分页获取帖子列表
                PagingDataSet<BlogThread> blogThreads = blogThreadService.GetsForAdmin(null, null, null, null, null, null, pageSize, pageIndex);
                totalRecords = blogThreads.TotalRecords;

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;

                //重建索引
                List<BlogThread> blogThreadList = blogThreads.ToList<BlogThread>();

                IEnumerable<Document> docs = BlogIndexDocument.Convert(blogThreadList);

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="blogThread">待添加的日志</param>
        public void Insert(BlogThread blogThread)
        {
            Insert(new BlogThread[] { blogThread });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="blogThreads">待添加的日志</param>
        public void Insert(IEnumerable<BlogThread> blogThreads)
        {
            IEnumerable<Document> docs = BlogIndexDocument.Convert(blogThreads);
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="blogThreadId">待删除的日志Id</param>
        public void Delete(long blogThreadId)
        {
            searchEngine.Delete(blogThreadId.ToString(), BlogIndexDocument.ThreadId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="blogThreadIds">待删除的日志Id列表</param>
        public void Delete(IEnumerable<long> blogThreadIds)
        {
            foreach (var blogThreadId in blogThreadIds)
            {
                searchEngine.Delete(blogThreadId.ToString(), BlogIndexDocument.ThreadId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="blogThread">待更新的日志</param>
        public void Update(BlogThread blogThread)
        {
            Document doc = BlogIndexDocument.Convert(blogThread);
            searchEngine.Update(doc, blogThread.ThreadId.ToString(), BlogIndexDocument.ThreadId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="blogThreads">待更新的日志集合</param>
        public void Update(IEnumerable<BlogThread> blogThreads)
        {
            IEnumerable<Document> docs = BlogIndexDocument.Convert(blogThreads);
            IEnumerable<string> blogThreadIds = blogThreads.Select(n => n.ThreadId.ToString());
            searchEngine.Update(docs, blogThreadIds, BlogIndexDocument.ThreadId);
        }

        #endregion

        #region 搜索
        /// <summary>
        /// 日志分页搜索
        /// </summary>
        /// <param name="blogQuery">搜索条件</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public PagingDataSet<BlogThread> Search(BlogFullTextQuery blogQuery)
        {
            if (blogQuery.SiteCategoryId == 0 && blogQuery.LoginId == 0 && blogQuery.UserId == 0)
            {
                if (string.IsNullOrWhiteSpace(blogQuery.Keyword) && blogQuery.IsRelationBlog == false)
                {
                    return new PagingDataSet<BlogThread>(new List<BlogThread>());
                }
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(blogQuery);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, blogQuery.PageIndex, blogQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的日志ID
            List<long> blogThreadIds = new List<long>();
            //获取索引中日志的标签
            Dictionary<long, IEnumerable<string>> blogTags = new Dictionary<long, IEnumerable<string>>();
            //获取索引中日志的用户分类名
            Dictionary<long, IEnumerable<string>> blogOwnerCategoryNames = new Dictionary<long, IEnumerable<string>>();

            foreach (Document doc in docs)
            {
                long blogThreadId = long.Parse(doc.Get(BlogIndexDocument.ThreadId));
                blogThreadIds.Add(blogThreadId);
                blogTags[blogThreadId] = doc.GetValues(BlogIndexDocument.Tag).ToList<string>();
                blogOwnerCategoryNames[blogThreadId] = doc.GetValues(BlogIndexDocument.OwnerCategoryName).ToList<string>();
            }

            //根据日志ID列表批量查询日志实例
            IEnumerable<BlogThread> blogThreadList = blogThreadService.GetBlogThreads(blogThreadIds);

            foreach (var blogThread in blogThreadList)
            {
                blogThread.Body = blogThread.GetBody();
                if (blogTags.ContainsKey(blogThread.ThreadId))
                {
                    blogThread.TagNames = blogTags[blogThread.ThreadId];
                }
                if (blogOwnerCategoryNames.ContainsKey(blogThread.ThreadId))
                {
                    blogThread.OwnerCategoryNames = blogOwnerCategoryNames[blogThread.ThreadId];
                }
            }

            //组装分页对象
            PagingDataSet<BlogThread> blogThreads = new PagingDataSet<BlogThread>(blogThreadList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return blogThreads;
        }

        /// <summary>
        /// 获取匹配的前几条日志热词
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
        /// <param name="Query"></param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(BlogFullTextQuery blogQuery)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            //范围
            Dictionary<string, BoostLevel> fieldNameAndBoosts = new Dictionary<string, BoostLevel>();

            //如果查的是相关日志
            if (blogQuery.IsRelationBlog)
            {
                fieldNameAndBoosts.Add(BlogIndexDocument.Subject, BoostLevel.Hight);
                fieldNameAndBoosts.Add(BlogIndexDocument.Tag, BoostLevel.Medium);
                fieldNameAndBoosts.Add(BlogIndexDocument.OwnerCategoryName, BoostLevel.Low);
                searchBuilder.WithPhrases(fieldNameAndBoosts, blogQuery.Keywords, BooleanClause.Occur.SHOULD, false);
                searchBuilder.WithField(BlogIndexDocument.PrivacyStatus, ((int)PrivacyStatus.Public).ToString(), true, BoostLevel.Hight, true);
                searchBuilder.NotWithField(BlogIndexDocument.ThreadId.ToString(), blogQuery.CurrentThreadId.ToString());
            }
            else
            {
                switch (blogQuery.BlogRange)
                {
                    case BlogRange.MYBlOG:
                        searchBuilder.WithField(BlogIndexDocument.UserId, blogQuery.LoginId.ToString(), true, BoostLevel.Hight, false);
                        break;
                    case BlogRange.SOMEONEBLOG:
                        searchBuilder.WithField(BlogIndexDocument.UserId, blogQuery.UserId.ToString(), true, BoostLevel.Hight, false);
                        break;
                    case BlogRange.SITECATEGORY:
                        if (blogQuery.LoginId != 0)
                            searchBuilder.WithField(BlogIndexDocument.UserId, blogQuery.LoginId.ToString(), true, BoostLevel.Hight, false);
                        if (blogQuery.UserId != 0)
                            searchBuilder.WithField(BlogIndexDocument.UserId, blogQuery.UserId.ToString(), true, BoostLevel.Hight, false);
                        if (blogQuery.SiteCategoryId != 0)
                            searchBuilder.WithField(BlogIndexDocument.SiteCategoryId, blogQuery.SiteCategoryId.ToString(), true, BoostLevel.Hight, false);
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(blogQuery.Keyword))
                {
                    switch (blogQuery.Range)
                    {
                        case BlogSearchRange.SUBJECT:
                            searchBuilder.WithPhrase(BlogIndexDocument.Subject, blogQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case BlogSearchRange.BODY:
                            searchBuilder.WithPhrase(BlogIndexDocument.Body, blogQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case BlogSearchRange.AUTHOR:
                            searchBuilder.WithPhrase(BlogIndexDocument.Author, blogQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case BlogSearchRange.TAG:
                            searchBuilder.WithPhrase(BlogIndexDocument.Tag, blogQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        case BlogSearchRange.OWNERCATEGORYNAME:
                            searchBuilder.WithPhrase(BlogIndexDocument.OwnerCategoryName, blogQuery.Keyword, BoostLevel.Hight, false);
                            break;
                        default:
                            fieldNameAndBoosts.Add(BlogIndexDocument.Subject, BoostLevel.Hight);
                            fieldNameAndBoosts.Add(BlogIndexDocument.Keywords, BoostLevel.Hight);
                            fieldNameAndBoosts.Add(BlogIndexDocument.Tag, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(BlogIndexDocument.Summary, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(BlogIndexDocument.Body, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(BlogIndexDocument.OwnerCategoryName, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(BlogIndexDocument.Author, BoostLevel.Low);
                            searchBuilder.WithPhrases(fieldNameAndBoosts, blogQuery.Keyword, BooleanClause.Occur.SHOULD, false);
                            break;
                    }
                }

                //某个站点分类
                if (blogQuery.SiteCategoryId != 0)
                {
                    searchBuilder.WithField(BlogIndexDocument.SiteCategoryId, blogQuery.SiteCategoryId.ToString(), true, BoostLevel.Hight, true);
                }

                if (!(UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == blogQuery.LoginId && blogQuery.AllId != 0))
                {
                    searchBuilder.NotWithField(BlogIndexDocument.PrivacyStatus, ((int)PrivacyStatus.Private).ToString());
                }
            }

            //筛选
            //全部、某人的日志
            if (blogQuery.AllId != 0)
            {
                if (blogQuery.LoginId != 0)
                {
                    searchBuilder.WithField(BlogIndexDocument.UserId, blogQuery.LoginId.ToString(), true, BoostLevel.Hight, true);
                }
                else if (blogQuery.UserId != 0)
                {
                    searchBuilder.WithField(BlogIndexDocument.UserId, blogQuery.UserId.ToString(), true, BoostLevel.Hight, true);
                }
            }

            //过滤可以显示的日志
            switch (publiclyAuditStatus)
            {
                case PubliclyAuditStatus.Again:
                case PubliclyAuditStatus.Fail:
                case PubliclyAuditStatus.Pending:
                case PubliclyAuditStatus.Success:
                    searchBuilder.WithField(BlogIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), true, BoostLevel.Hight, true);
                    break;
                case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                    searchBuilder.WithinRange(BlogIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), ((int)PubliclyAuditStatus.Success).ToString(), true);
                    break;
            }

            return searchBuilder;
        }
        #endregion
    }
}