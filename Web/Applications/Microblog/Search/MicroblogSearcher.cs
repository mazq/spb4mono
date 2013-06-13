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
using Tunynet;
using Tunynet.Common;
using Tunynet.Search;
using Spacebuilder.Search;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博搜索器
    /// </summary>
    public class MicroblogSearcher : ISearcher
    {
        private MicroblogService microBlogService = new MicroblogService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Microblog());
        private ISearchEngine searchEngine;
        public static string CODE = "MicroblogSearcher";
        public static string WATERMARK = "搜索微博、话题";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public MicroblogSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
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
            return SiteUrls.Instance().MicroblogGlobalSearch() + "?keyword=" + keyword;
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
            return SiteUrls.Instance().MicroblogQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().MicroblogSearch(keyword);
        }

        #endregion

        #region 索引内容维护

        /// <summary>
        /// 重建索引
        /// </summary>
        public void RebuildIndex()
        {   //pageSize参数决定了每次批量取多少条数据进行索引。要注意的是，如果是分布式搜索，客户端会将这部分数据通过WCF传递给服务器端，而WCF默认的最大传输数据量是65535B，pageSize较大时这个设置显然是不够用的，WCF会报400错误；系统现在将最大传输量放宽了，但仍要注意一次不要传输过多，如遇异常，可适当调小pageSize的值
            int pageSize = 1000;
            int pageIndex = 1;
            long totalRecords = 0;
            bool isBeginning = true;
            bool isEndding = false;
            MicroblogQuery query = new MicroblogQuery();

            do
            {
                //分页获取微博列表
                PagingDataSet<MicroblogEntity> microblogs = microBlogService.GetMicroblogs(query, pageSize, pageIndex);
                totalRecords = microblogs.TotalRecords;

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;

                //重建索引
                List<MicroblogEntity> microblogsList = microblogs.ToList<MicroblogEntity>();

                IEnumerable<Document> docs = MicroblogIndexDocument.Convert(microblogsList);

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);

        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="microblog">待添加的微博</param>
        public void Insert(MicroblogEntity microblog)
        {
            Insert(new MicroblogEntity[] { microblog });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="microblogs">待添加的微博</param>
        public void Insert(IEnumerable<MicroblogEntity> microblogs)
        {
            List<Document> docs = new List<Document>();
            foreach (var microblog in microblogs)
            {
                Document doc = MicroblogIndexDocument.Convert(microblog);
                if (doc != null)
                    docs.Add(doc);
            }
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="microblogId">待删除的微博Id</param>
        public void Delete(long microblogId)
        {
            searchEngine.Delete(microblogId.ToString(), MicroblogIndexDocument.MicroblogId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="microblogIds">待删除的微博Id列表</param>
        public void Delete(IEnumerable<long> microblogIds)
        {
            foreach (var microblogId in microblogIds)
            {
                searchEngine.Delete(microblogId.ToString(), MicroblogIndexDocument.MicroblogId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="microblog">待更新的微博</param>
        public void Update(MicroblogEntity microblog)
        {
            Document doc = MicroblogIndexDocument.Convert(microblog);
            searchEngine.Update(doc, microblog.MicroblogId.ToString(), MicroblogIndexDocument.MicroblogId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="microblogs">待更新的微博集合</param>
        public void Update(IEnumerable<MicroblogEntity> microblogs)
        {
            IEnumerable<Document> docs = MicroblogIndexDocument.Convert(microblogs);
            List<string> microblogIds = new List<string>();
            foreach (MicroblogEntity microblog in microblogs)
            {
                microblogIds.Add(microblog.MicroblogId.ToString());
            }
            searchEngine.Update(docs, microblogIds, MicroblogIndexDocument.MicroblogId);
        }

        #endregion

        /// <summary>
        /// 微博分页搜索
        /// </summary>
        /// <param name="query">搜索条件</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public PagingDataSet<MicroblogEntity> Search(MicroblogFullTextQuery microBlogQuery)
        {
            if (microBlogQuery.Keywords == null && string.IsNullOrWhiteSpace(microBlogQuery.Keyword) && microBlogQuery.SearchTerm == MicroblogSearchTerm.ALL)
            {
                return new PagingDataSet<MicroblogEntity>(new List<MicroblogEntity>());
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(microBlogQuery);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, microBlogQuery.PageIndex, microBlogQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的微博ID
            List<long> microblogIds = new List<long>();
            foreach (Document doc in docs)
            {
                long microblogId = long.Parse(doc.Get(MicroblogIndexDocument.MicroblogId));
                microblogIds.Add(microblogId);
            }

            //根据微博ID列表批量查询微博实例
            IEnumerable<MicroblogEntity> microblogList = microBlogService.GetMicroblogs(microblogIds);

            //组装分页对象
            PagingDataSet<MicroblogEntity> microblogs = new PagingDataSet<MicroblogEntity>(microblogList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return microblogs;
        }

        /// <summary>
        /// 获取匹配的前几条话题(标签)
        /// </summary>
        /// <param name="tagName">要匹配的话题(标签)</param>
        /// <param name="topNumber">前N条</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public IEnumerable<string> AutoCompleteSearch(string tagName, int topNumber)
        {
            return tagService.GetTopTagNames(tagName, topNumber);
        }

        /// <summary>
        /// 根据微博搜索查询条件构建Lucene查询条件
        /// </summary>
        /// <param name="userQuery"></param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(MicroblogFullTextQuery microblogQuery)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            //微博搜索词匹配范围
            string fieldName = null;
            switch (microblogQuery.SearchRange)
            {
                case MicroblogSearchRange.TOPIC:
                    fieldName = MicroblogIndexDocument.Topic;
                    break;
                default:
                    fieldName = MicroblogIndexDocument.Body;
                    break;
            }
            if (microblogQuery.Keywords != null && microblogQuery.Keywords.Count() != 0)
            {
                //是否模糊查询
                if (microblogQuery.IsFuzzy)
                {
                    searchBuilder.WithPhrases(fieldName, microblogQuery.Keywords, BoostLevel.Hight, false);
                }
                else
                {
                    searchBuilder.WithFields(fieldName, microblogQuery.Keywords, true, BoostLevel.Hight, false);
                }
            }
            else
            {
                searchBuilder.WithPhrase(fieldName, microblogQuery.Keyword, BoostLevel.Hight);

            }

            //微博搜索条件过滤
            switch (microblogQuery.SearchTerm)
            {
                case MicroblogSearchTerm.ISORIGINALITY:
                    searchBuilder.WithField(MicroblogIndexDocument.IsOriginality, 1, true, BoostLevel.Hight, true);
                    break;
                case MicroblogSearchTerm.HASPHOTO:
                    searchBuilder.WithField(MicroblogIndexDocument.HasPhoto, 1, true, BoostLevel.Hight, true);
                    break;
                case MicroblogSearchTerm.HASMUSIC:
                    searchBuilder.WithField(MicroblogIndexDocument.HasMusic, 1, true, BoostLevel.Hight, true);
                    break;
                case MicroblogSearchTerm.HASVIDEO:
                    searchBuilder.WithField(MicroblogIndexDocument.HasVideo, 1, true, BoostLevel.Hight, true);
                    break;
                default:
                    break;
            }

            ////筛选租户类型
            //if (!string.IsNullOrEmpty(microblogQuery.TenantTypeId))
            //{
            //    searchBuilder.WithField(MicroblogIndexDocument.TenantTypeId, microblogQuery.TenantTypeId, true, BoostLevel.Hight, true);
            //}
            if (!microblogQuery.IsGroup)
            {
                searchBuilder.NotWithField(MicroblogIndexDocument.TenantTypeId, TenantTypeIds.Instance().Group());
            }

            //微博排序
            searchBuilder.SortByString(MicroblogIndexDocument.DateCreated, true);

            return searchBuilder;
        }

    }
}