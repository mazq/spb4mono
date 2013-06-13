//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Tunynet.Search;

namespace Spacebuilder.Search
{
    /// <summary>
    /// 搜索引擎的WCF服务器实现
    /// </summary>
    public class SearchEngineService : ISearchEngineService
    {
        //以indexPath为key，存储SearchEngine实例
        public static ConcurrentDictionary<string, SearchEngine> searchEngines = new ConcurrentDictionary<string, SearchEngine>();

        /// <summary>
        /// 初始化搜索引擎
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        public void Initialize(string indexPath)
        {
            if (!searchEngines.ContainsKey(indexPath))
            {
                searchEngines.TryAdd(indexPath, new SearchEngine(indexPath));
            }
        }

        /// <summary>
        /// 根据索引路径获取一个搜索引擎实例
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <returns>搜索引擎实例</returns>
        public static SearchEngine GetSearchEngine(string indexPath)
        {
            SearchEngine searchEngine = null;
            if (!searchEngines.TryGetValue(indexPath, out searchEngine))
            {
                searchEngine = new SearchEngine(indexPath);
                searchEngines.TryAdd(indexPath, searchEngine);
            }
            return searchEngine;
        }

        /// <summary>
        /// 删除索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="id">索引内容对应的实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Delete(string indexPath, IEnumerable<string> ids, string fieldNameOfId, bool reopen = true)
        {
            GetSearchEngine(indexPath).Delete(ids, fieldNameOfId, reopen);
        }

        /// <summary>
        /// 删除索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="ids">索引内容对应的实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Delete(string indexPath, string id, string fieldNameOfId, bool reopen = true)
        {
            GetSearchEngine(indexPath).Delete(id, fieldNameOfId, reopen);
        }

        /// <summary>
        /// 获取当前搜索引擎的索引大小，单位为字节(Byte)
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        public double GetIndexSize(string indexPath)
        {
            return GetSearchEngine(indexPath).GetIndexSize();
        }

        /// <summary>
        /// 获取当前搜索引擎的索引最后更新时间
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        public DateTime GetLastModified(string indexPath)
        {
            return GetSearchEngine(indexPath).GetLastModified();
        }

        /// <summary>
        /// 添加索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocument">待添加的索引文档</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Insert(string indexPath, IEnumerable<Lucene.Net.Documents.Document> indexDocuments, bool reopen = true)
        {
            GetSearchEngine(indexPath).Insert(indexDocuments, reopen);
        }

        /// <summary>
        /// 添加索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocuments">待添加的索引文档</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Insert(string indexPath, Lucene.Net.Documents.Document indexDocument, bool reopen = true)
        {
            GetSearchEngine(indexPath).Insert(indexDocument, reopen);
        }

        /// <summary>
        /// 重建索引
        /// </summary>
        /// <remarks>
        /// 重建索引数据量大，一般分多次进行调用本方法，用isBeginning和isEndding参数；
        /// 重建索引会重置搜索管理器，搜索服务可能在切换索引时短时中断；
        /// 重建索引的目的是为了避免因意外情况导致索引不完整
        /// </remarks>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocuments">待添加的内容</param>
        /// <param name="isBeginning">开始重建</param>
        /// <param name="isEndding">完成重建</param>
        public void RebuildIndex(string indexPath, IEnumerable<Lucene.Net.Documents.Document> indexDocuments, bool isBeginning, bool isEndding)
        {
            GetSearchEngine(indexPath).RebuildIndex(indexDocuments, isBeginning, isEndding);
        }

        /// <summary>
        /// 搜索并返回前topNumber条数据
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="searchQuery"><see cref="Lucene.Net.Search.Query"/></param>
        /// <param name="filter"><see cref="Lucene.Net.Search.Filter"/></param>
        /// <param name="sort"><see cref="Lucene.Net.Search.Sort"/></param>
        /// <param name="topNumber">最多返回多少条数据</param>
        /// <returns></returns>
        public IEnumerable<Lucene.Net.Documents.Document> Search(string indexPath, Lucene.Net.Search.Query searchQuery, Lucene.Net.Search.Filter filter, Lucene.Net.Search.Sort sort, int topNumber)
        {
            return GetSearchEngine(indexPath).Search(searchQuery, filter, sort, topNumber);
        }

        /// <summary>
        /// 搜索并返回分页数据
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="searchQuery"><see cref="Lucene.Net.Search.Query"/></param>
        /// <param name="filter"><see cref="Lucene.Net.Search.Filter"/></param>
        /// <param name="sort"><see cref="Lucene.Net.Search.Sort"/></param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>内容为<see cref="Lucene.Net.Documents.Document"/>的分页数据集合</returns>
        public Tunynet.PagingDataSet<Lucene.Net.Documents.Document> Search(string indexPath, Lucene.Net.Search.Query searchQuery, Lucene.Net.Search.Filter filter, Lucene.Net.Search.Sort sort, int pageIndex, int pageSize)
        {
            return GetSearchEngine(indexPath).Search(searchQuery, filter, sort, pageIndex, pageSize);
        }

        /// <summary>
        /// 更新索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocuments">索引文档</param>
        /// <param name="ids">实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Update(string indexPath, IEnumerable<Lucene.Net.Documents.Document> indexDocuments, IEnumerable<string> ids, string fieldNameOfId, bool reopen = true)
        {
            GetSearchEngine(indexPath).Update(indexDocuments, ids, fieldNameOfId, reopen);
        }

        /// <summary>
        /// 更新索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocument">索引文档</param>
        /// <param name="id">实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Update(string indexPath, Lucene.Net.Documents.Document indexDocument, string id, string fieldNameOfId, bool reopen = true)
        {
            GetSearchEngine(indexPath).Update(indexDocument, id, fieldNameOfId, reopen);
        }
    }
}