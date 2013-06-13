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

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组搜索器
    /// </summary>
    public class GroupSearcher : ISearcher
    {
        private GroupService groupService = new GroupService();
        private TagService tagService = new TagService(TenantTypeIds.Instance().Group());
        private CategoryService categoryService = new CategoryService();
        private AuditService auditService = new AuditService();
        private ISearchEngine searchEngine;
        private PubliclyAuditStatus publiclyAuditStatus;
        public static string CODE = "GroupSearcher";
        public static string WATERMARK = "搜索群组";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public GroupSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.AsQuickSearch = asQuickSearch;
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
            publiclyAuditStatus = auditService.GetPubliclyAuditStatus(GroupConfig.Instance().ApplicationId);
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
            return SiteUrls.Instance().GroupGlobalSearch() + "?keyword=" + keyword;
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
            return SiteUrls.Instance().GroupQuickSearch() + "?keyword=" + keyword;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return SiteUrls.Instance().GroupPageSearch(keyword);
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
                //分页获取群组列表
                PagingDataSet<GroupEntity> groups = groupService.GetsForAdmin(null, null, null, null, null, null, null, null, pageSize, pageIndex);
                totalRecords = groups.TotalRecords;

                isEndding = (pageSize * pageIndex < totalRecords) ? false : true;

                //重建索引
                List<GroupEntity> groupList = groups.ToList<GroupEntity>();

                IEnumerable<Document> docs = GroupIndexDocument.Convert(groupList);

                searchEngine.RebuildIndex(docs, isBeginning, isEndding);

                isBeginning = false;
                pageIndex++;
            }
            while (!isEndding);
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="GroupEntity">待添加的群组</param>
        public void Insert(GroupEntity group)
        {
            Insert(new GroupEntity[] { group });
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="GroupEntitys">待添加的群组</param>
        public void Insert(IEnumerable<GroupEntity> groups)
        {
            IEnumerable<Document> docs = GroupIndexDocument.Convert(groups);
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="GroupEntityId">待删除的群组Id</param>
        public void Delete(long groupId)
        {
            searchEngine.Delete(groupId.ToString(), GroupIndexDocument.GroupId);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="GroupEntityIds">待删除的群组Id列表</param>
        public void Delete(IEnumerable<long> groupIds)
        {
            foreach (var groupId in groupIds)
            {
                searchEngine.Delete(groupId.ToString(), GroupIndexDocument.GroupId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="GroupEntity">待更新的群组</param>
        public void Update(GroupEntity group)
        {
            Document doc = GroupIndexDocument.Convert(group);
            searchEngine.Update(doc, group.GroupId.ToString(), GroupIndexDocument.GroupId);
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="GroupEntitys">待更新的群组集合</param>
        public void Update(IEnumerable<GroupEntity> groups)
        {
            IEnumerable<Document> docs = GroupIndexDocument.Convert(groups);
            IEnumerable<string> groupIds = groups.Select(n => n.GroupId.ToString());
            searchEngine.Update(docs, groupIds, GroupIndexDocument.GroupId);
        }

        #endregion

        #region 搜索

        
        
        //fixed by wanf
        /// <summary>
        /// 群组分页搜索
        /// </summary>
        /// <param name="groupQuery">搜索条件</param>
        /// <param name="interestGroup">是否是查询可能感兴趣的群组</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public PagingDataSet<GroupEntity> Search(GroupFullTextQuery groupQuery, bool interestGroup = false)
        {
            if (!interestGroup)
            {
                if (string.IsNullOrWhiteSpace(groupQuery.Keyword) && !groupQuery.KeywordIsNull)
                {
                    return new PagingDataSet<GroupEntity>(new List<GroupEntity>());
                }
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(groupQuery, interestGroup);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchService.Search(),执行搜索
            PagingDataSet<Document> searchResult = searchEngine.Search(query, filter, sort, groupQuery.PageIndex, groupQuery.PageSize);
            IEnumerable<Document> docs = searchResult.ToList<Document>();

            //解析出搜索结果中的群组ID
            List<long> groupIds = new List<long>();
            //获取索引中群组的标签
            Dictionary<long, IEnumerable<string>> groupTags = new Dictionary<long, IEnumerable<string>>();
            //获取索引中群组的分类名
            Dictionary<long, string> categoryNames = new Dictionary<long, string>();

            foreach (Document doc in docs)
            {
                long groupId = long.Parse(doc.Get(GroupIndexDocument.GroupId));
                groupIds.Add(groupId);
                groupTags[groupId]=doc.GetValues(GroupIndexDocument.Tag).ToList<string>();
                categoryNames[groupId]=doc.Get(GroupIndexDocument.CategoryName);
            }

            //根据群组ID列表批量查询群组实例
            IEnumerable<GroupEntity> groupList = groupService.GetGroupEntitiesByIds(groupIds);

            foreach (var group in groupList)
            {
                if (groupTags.ContainsKey(group.GroupId))
                {
                    group.TagNames = groupTags[group.GroupId];
                }
                if (categoryNames.ContainsKey(group.GroupId))
                {
                    group.CategoryName = categoryNames[group.GroupId];
                }
            }

            //组装分页对象
            PagingDataSet<GroupEntity> GroupEntitys = new PagingDataSet<GroupEntity>(groupList)
            {
                TotalRecords = searchResult.TotalRecords,
                PageSize = searchResult.PageSize,
                PageIndex = searchResult.PageIndex,
                QueryDuration = searchResult.QueryDuration
            };

            return GroupEntitys;
        }

        /// <summary>
        /// 获取匹配的前几条热门群组
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="topNumber">前N条</param>
        /// <returns>符合搜索条件的分页集合</returns>
        public IEnumerable<string> AutoCompleteSearch(string keyword, int topNumber)
        {
            IEnumerable<GroupEntity> hotGroups = groupService.GetMatchTops(topNumber, keyword, null, null, SortBy_Group.GrowthValue_Desc);
            if (hotGroups.Count() > topNumber)
            {
                hotGroups.Take(topNumber);
            }
            return hotGroups.Select(n => n.GroupName);
        }

        /// <summary>
        /// 根据帖吧搜索查询条件构建Lucene查询条件
        /// </summary>
        /// <param name="Query">搜索条件</param>
        /// <param name="interestGroup">是否是查询可能感兴趣的群组</param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(GroupFullTextQuery groupQuery, bool interestGroup = false)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            Dictionary<string, BoostLevel> fieldNameAndBoosts = new Dictionary<string, BoostLevel>();
            //关键字为空就是在搜地区时关键字为空
            if (groupQuery.KeywordIsNull)
            {
                if (!string.IsNullOrEmpty(groupQuery.NowAreaCode))
                    searchBuilder.WithField(GroupIndexDocument.AreaCode, groupQuery.NowAreaCode.TrimEnd('0'), false, BoostLevel.Hight, false);
                else
                    searchBuilder.WithFields(GroupIndexDocument.AreaCode, new string[] { "1", "2", "3" }, false, BoostLevel.Hight, false);
            }

            if (!string.IsNullOrEmpty(groupQuery.Keyword))
            {
                //范围
                switch (groupQuery.Range)
                {
                    case GroupSearchRange.GROUPNAME:
                        searchBuilder.WithPhrase(GroupIndexDocument.GroupName, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case GroupSearchRange.DESCRIPTION:
                        searchBuilder.WithPhrase(GroupIndexDocument.Description, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case GroupSearchRange.TAG:
                        searchBuilder.WithPhrase(GroupIndexDocument.Tag, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    case GroupSearchRange.CATEGORYNAME:
                        searchBuilder.WithPhrase(GroupIndexDocument.CategoryName, groupQuery.Keyword, BoostLevel.Hight, false);
                        break;
                    default:
                            fieldNameAndBoosts.Add(GroupIndexDocument.GroupName, BoostLevel.Hight);
                            fieldNameAndBoosts.Add(GroupIndexDocument.Description, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(GroupIndexDocument.Tag, BoostLevel.Medium);
                            fieldNameAndBoosts.Add(GroupIndexDocument.CategoryName, BoostLevel.Medium);
                            searchBuilder.WithPhrases(fieldNameAndBoosts, groupQuery.Keyword, BooleanClause.Occur.SHOULD, false);   
                        break;
                }
            }

            //根据标签搜索可能感兴趣的群组
            if (interestGroup)
            {
                searchBuilder.WithPhrases(GroupIndexDocument.Tag, groupQuery.Tags, BoostLevel.Hight, false);
                searchBuilder.NotWithFields(GroupIndexDocument.GroupId, groupQuery.GroupIds);
            }

            //筛选
            //某地区
            if (!string.IsNullOrEmpty(groupQuery.NowAreaCode))
            {
                searchBuilder.WithField(GroupIndexDocument.AreaCode, groupQuery.NowAreaCode.TrimEnd('0'), false, BoostLevel.Hight, true);
            }

            //某分类
            if (groupQuery.CategoryId != 0)
            {
                
                //fixed by wanf:发现群组已经不再用全文检索了

                CategoryService categoryService = new CategoryService();
                IEnumerable<Category> categories = categoryService.GetDescendants(groupQuery.CategoryId);
                List<string> categoryIds = new List<string> { groupQuery.CategoryId.ToString() };
                if (categories != null && categories.Count() > 0)
                {
                    categoryIds.AddRange(categories.Select(n => n.CategoryId.ToString()));
                }

                searchBuilder.WithFields(GroupIndexDocument.CategoryId, categoryIds, true, BoostLevel.Hight, true);
            }

            //公开的群组
            searchBuilder.WithField(GroupIndexDocument.IsPublic, "1", true, BoostLevel.Hight, true);

            //过滤可以显示的群组
            switch (publiclyAuditStatus)
            {
                case PubliclyAuditStatus.Again:
                case PubliclyAuditStatus.Fail:
                case PubliclyAuditStatus.Pending:
                case PubliclyAuditStatus.Success:
                    searchBuilder.WithField(GroupIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), true, BoostLevel.Hight, true);
                    break;
                case PubliclyAuditStatus.Again_GreaterThanOrEqual:
                case PubliclyAuditStatus.Pending_GreaterThanOrEqual:
                    searchBuilder.WithinRange(GroupIndexDocument.AuditStatus, ((int)publiclyAuditStatus).ToString(), ((int)PubliclyAuditStatus.Success).ToString(), true);
                    break;
            }

            if (groupQuery.sortBy.HasValue)
            {
                switch (groupQuery.sortBy.Value)
                {
                    case SortBy_Group.DateCreated_Desc:
                        searchBuilder.SortByString(GroupIndexDocument.DateCreated, true);
                        break;
                    case SortBy_Group.MemberCount_Desc:
                        searchBuilder.SortByString(GroupIndexDocument.MemberCount, true);
                        break;
                    case SortBy_Group.GrowthValue_Desc:
                        searchBuilder.SortByString(GroupIndexDocument.GrowthValue, true);
                        break;
                }
            }
            else
            {
                //时间倒序排序
                searchBuilder.SortByString(GroupIndexDocument.DateCreated, true);
            }

            return searchBuilder;
        }
        #endregion
    }
}