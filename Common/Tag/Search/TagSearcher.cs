//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spacebuilder.Search;
using Tunynet.Common;
using Tunynet;
using Tunynet.Search;
using Lucene.Net.Search;
using Lucene.Net.Documents;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 标签搜索器
    /// </summary>
    public class TagSearcher : ISearcher
    {
        public TagService tagService = new TagService(string.Empty);
        public string tenantTypeId;
        private ISearchEngine searchEngine;
        public static string CODE = "TagSearcher";
        public static string WATERMARK = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Searcher名称</param>
        /// <param name="indexPath">索引文件所在路径（支持"~/"及unc路径）</param>
        /// <param name="asQuickSearch">是否作为快捷搜索</param>
        /// <param name="displayOrder">显示顺序</param>
        public TagSearcher(string name, string indexPath, bool asQuickSearch, int displayOrder)
        {
            this.Name = name;
            this.IndexPath = Tunynet.Utilities.WebUtility.GetPhysicalFilePath(indexPath);
            this.DisplayOrder = displayOrder;
            searchEngine = SearcherFactory.GetSearchEngine(indexPath);
        }

        #region 搜索器属性

        public string WaterMark { get { return WATERMARK; } }

        /// <summary>
        /// 搜索器的唯一标识
        /// </summary>
        public string Code { get { return CODE; } }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Lucene索引路径（完整物理路径，支持unc）
        /// </summary>
        public string IndexPath { get; private set; }

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
            return string.Empty;
        }

        /// <summary>
        /// 处理快捷搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string QuickSearchActionUrl(string keyword)
        {
            return string.Empty;
        }

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        public string PageSearchActionUrl(string keyword)
        {
            return string.Empty;
        }

        /// <summary>
        /// 是否作为快捷搜索
        /// </summary>
        public bool AsQuickSearch { get; private set; }


        /// <summary>
        /// 是否基于Lucene实现
        /// </summary>
        public bool IsBaseOnLucene
        {
            get { return true; }
        }

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
        /// 是否前台显示
        /// </summary>
        public bool IsDisplay
        {
            get { return false; }
        }

        #endregion

        #region 索引内容维护

        /// <summary>
        /// 重建索引
        /// </summary>  
        public void RebuildIndex()
        {
            //pageSize参数决定了每次批量取多少条数据进行索引。要注意的是，如果是分布式搜索，客户端会将这部分数据通过WCF传递给服务器端，而WCF默认的最大传输数据量是65535B，pageSize较大时这个设置显然是不够用的，WCF会报400错误；系统现在将最大传输量放宽了，但仍要注意一次不要传输过多，如遇异常，可适当调小pageSize的值
            bool hasData = true;
            int pageSizeTags = 1000;
            int pageIndexTags = 1;
            long totalRecordTags = 0;
            bool isBeginningTags = true;
            bool isEnddingTags = false;
            TagQuery tagQuery = new TagQuery();
            do
            {
                PagingDataSet<Tag> tags = tagService.GetTags(tagQuery, pageIndexTags, pageSizeTags);
                totalRecordTags = tags.TotalRecords;
                if (totalRecordTags > 0)
                {
                    hasData = true;
                }
                isEnddingTags = (pageSizeTags * pageIndexTags < totalRecordTags) ? false : true;

                //重建索引
                List<Tag> tagList = tags.ToList<Tag>();

                IEnumerable<Document> docs = TagIndexDocument.Convert(tagList);

                searchEngine.RebuildIndex(docs, isBeginningTags, false);

                isBeginningTags = false;
                pageIndexTags++;
            }
            while (!isEnddingTags);

            int pageSizeTagInOwner = 1000;
            int pageIndexTagInOwner = 1;
            long totalRecordsTagInOwner = 0;
            bool isEnddingTagInOwner = false;
            do
            {
                //分页获取帖子列表
                PagingDataSet<TagInOwner> tagInOwners = tagService.GetOwnerTags(pageIndexTagInOwner, pageSizeTagInOwner);
                totalRecordsTagInOwner = tagInOwners.TotalRecords;
                if (totalRecordsTagInOwner > 0)
                {
                    hasData = true;
                }
                isEnddingTagInOwner = (pageSizeTagInOwner * pageIndexTagInOwner < totalRecordsTagInOwner) ? false : true;

                //重建索引
                List<TagInOwner> tagInOwnerList = tagInOwners.ToList<TagInOwner>();

                IEnumerable<Document> docs = TagIndexDocument.Convert(tagInOwnerList);

                searchEngine.RebuildIndex(docs, false, false);

                pageIndexTagInOwner++;
            }
            while (!isEnddingTagInOwner);

            if (hasData)
            {
                searchEngine.RebuildIndex(null, false, true);
            }
        }

        /// <summary>
        /// 添加索引
        /// </summary>
        public void InsertTag(Tag tag)
        {
            InsertTag(new Tag[] { tag });
        }

        public void InsertTag(IEnumerable<Tag> tags)
        {
            List<Document> docs = new List<Document>();
            foreach (var tag in tags)
            {
                Document doc = TagIndexDocument.Convert(tag);
                if (doc != null)
                    docs.Add(doc);
            }
            searchEngine.Insert(docs);
        }

        public void InsertTagInOwner(TagInOwner tagInOwner)
        {
            InsertTagInOwner(new TagInOwner[] { tagInOwner });
        }

        public void InsertTagInOwner(IEnumerable<TagInOwner> tagInOwners)
        {
            List<Document> docs = new List<Document>();
            foreach (var tagInOwner in tagInOwners)
            {
                Document doc = TagIndexDocument.Convert(tagInOwner);
                if (doc != null)
                    docs.Add(doc);
            }
            searchEngine.Insert(docs);
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        public void DeleteTag(long tagId)
        {
            searchEngine.Delete(tagId.ToString(), TagIndexDocument.TagId);
        }

        public void DeleteTag(IEnumerable<long> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                searchEngine.Delete(tagId.ToString(), TagIndexDocument.TagId);
            }
        }

        public void DeleteTagInOwner(long tagInOwnerId)
        {
            searchEngine.Delete(tagInOwnerId.ToString(), TagIndexDocument.TagInOwnerId);
        }

        public void DeleteTagInOwner(IEnumerable<long> tagInOwnerIds)
        {
            foreach (var tagInOwnerId in tagInOwnerIds)
            {
                searchEngine.Delete(tagInOwnerId.ToString(), TagIndexDocument.TagInOwnerId);
            }
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        public void UpdateTag(Tag tag)
        {
            Document doc = TagIndexDocument.Convert(tag);
            searchEngine.Update(doc, tag.TagId.ToString(), TagIndexDocument.TagId);
        }

        public void UpdateTag(IEnumerable<Tag> tags)
        {
            IEnumerable<Document> docs = TagIndexDocument.Convert(tags);
            List<string> tagIds = new List<string>();
            foreach (Tag tag in tags)
            {
                tagIds.Add(tag.TagId.ToString());
            }
            searchEngine.Update(docs, tagIds, TagIndexDocument.TagId);
        }

        public void UpdateTagInOwner(TagInOwner tagInOwner)
        {
            Document doc = TagIndexDocument.Convert(tagInOwner);
            searchEngine.Update(doc, tagInOwner.Id.ToString(), TagIndexDocument.TagInOwnerId);
        }

        public void UpdateTagInOwner(IEnumerable<TagInOwner> tagInOwners)
        {
            IEnumerable<Document> docs = TagIndexDocument.Convert(tagInOwners);
            List<string> tagInOwnerIds = new List<string>();
            foreach (TagInOwner tagInOwner in tagInOwners)
            {
                tagInOwnerIds.Add(tagInOwner.Id.ToString());
            }
            searchEngine.Update(docs, tagInOwnerIds, TagIndexDocument.TagInOwnerId);
        }

        #endregion

        #region 搜索
        /// <summary>
        /// 标签搜索
        /// </summary>
        public IEnumerable<string> Search(string tagName, int topNumber, long ownerId = 0)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return new List<string>();
            }

            LuceneSearchBuilder searchBuilder = BuildLuceneSearchBuilder(tagName, ownerId);

            //使用LuceneSearchBuilder构建Lucene需要Query、Filter、Sort
            Query query = null;
            Filter filter = null;
            Sort sort = null;
            searchBuilder.BuildQuery(out query, out filter, out sort);

            //调用SearchEngine.Search(),执行搜索
            IEnumerable<Document> searchResult = searchEngine.Search(query, filter, sort, topNumber);
            return searchResult.Select(d => d.Get(TagIndexDocument.TagName));
        }

        /// <summary>
        /// 根据标签搜索查询条件构建Lucene查询条件
        /// </summary>
        /// <param name="userQuery"></param>
        /// <returns></returns>
        private LuceneSearchBuilder BuildLuceneSearchBuilder(string tagName, long ownerId)
        {
            LuceneSearchBuilder searchBuilder = new LuceneSearchBuilder();
            //搜索词匹配范围
            searchBuilder.WithField(TagIndexDocument.TagName, tagName, false, BoostLevel.Hight, false);
            //searchBuilder.WithPhrase(TagIndexDocument.TagName, tagName, BoostLevel.Hight, false);
            //租户ID过滤
            searchBuilder.WithField(TagIndexDocument.TenantTypeId, this.tenantTypeId, true, BoostLevel.Hight, true);
            //OwnerId过滤
            searchBuilder.WithField(TagIndexDocument.OwnerId, ownerId.ToString(), true, BoostLevel.Hight, true);
            //按内容数倒叙
            searchBuilder.SortByInteger(TagIndexDocument.ItemCount, true);
            return searchBuilder;
        }
        #endregion

    }
}
