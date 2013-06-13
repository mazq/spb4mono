//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ServiceModel;
using Lucene.Net.Documents;
using Tunynet;
using Tunynet.Search;

namespace Spacebuilder.Search
{
    /// <summary>
    /// SearchEngine的WCF客户端实现
    /// </summary>
    public class SearchEngineServiceClient : ClientBase<ISearchEngineService>, ISearchEngine
    {
        private string indexPath;//索引目录

        /// <summary>
        /// 无参数构造函数，不能直接调用，保留的目的是WCF所需
        /// </summary>
        public SearchEngineServiceClient()
            : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="endpointName">与客户端web.config中client\endpoint的name属性一致</param>
        /// <param name="indexPath">索引目录，在Starter.cs的DI注入时指定</param>
        public SearchEngineServiceClient(string endpointName, string indexPath)
            : base(endpointName)
        {
            this.indexPath = indexPath;
            this.Channel.Initialize(this.indexPath);
        }


        /// <summary>
        /// 获取当前搜索引擎的索引大小，通过远程调用
        /// </summary>
        public double GetIndexSize()
        {
            return this.Channel.GetIndexSize(this.indexPath);
        }

        /// <summary>
        /// 获取当前搜索引擎的索引最后更新时间
        /// </summary>
        public DateTime GetLastModified()
        {
            return this.Channel.GetLastModified(this.indexPath);
        }

        /// <summary>
        /// 删除索引内容
        /// </summary>
        /// <param name="ids">索引内容对应的实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Delete(IEnumerable<string> ids, string fieldNameOfId, bool reopen = true)
        {
            this.Channel.Delete(this.indexPath,ids, fieldNameOfId, reopen);
        }

        /// <summary>
        /// 删除索引内容
        /// </summary>
        /// <param name="id">索引内容对应的实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Delete(string id, string fieldNameOfId, bool reopen = true)
        {
            this.Channel.Delete(this.indexPath, id, fieldNameOfId, reopen);
        }

        /// <summary>
        /// 添加索引内容
        /// </summary>
        /// <param name="indexDocuments">待添加的索引文档</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Insert(IEnumerable<Document> indexDocuments, bool reopen = true)
        {
            this.Channel.Insert(this.indexPath, indexDocuments);
        }

        /// <summary>
        /// 添加索引内容
        /// </summary>
        /// <param name="indexDocument">待添加的索引文档</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Insert(Document indexDocument, bool reopen = true)
        {
            this.Channel.Insert(this.indexPath, indexDocument, reopen);
        }

        /// <summary>
        /// 重建索引
        /// </summary>
        /// <remarks>
        /// 重建索引数据量大，一般分多次进行调用本方法，用isBeginning和isEndding参数；
        /// 重建索引会重置搜索管理器，搜索服务可能在切换索引时短时中断；
        /// 重建索引的目的是为了避免因意外情况导致索引不完整
        /// 重建索引的操作最好是直接在分布式搜索服务的服务器端执行，而不是远程调用
        /// </remarks>
        /// <param name="indexDocuments">待添加的内容</param>
        /// <param name="isBeginning">开始重建</param>
        /// <param name="isEndding">完成重建</param>
        public void RebuildIndex(IEnumerable<Document> indexDocuments, bool isBeginning, bool isEndding)
        {
            this.Channel.RebuildIndex(this.indexPath, indexDocuments, isBeginning, isEndding);
        }

        /// <summary>
        /// 搜索并返回前topNumber条数据
        /// </summary>
        /// <param name="searchQuery"><see cref="Lucene.Net.Search.Query"/></param>
        /// <param name="filter"><see cref="Lucene.Net.Search.Filter"/></param>
        /// <param name="sort"><see cref="Lucene.Net.Search.Sort"/></param>
        /// <param name="topNumber">最多返回多少条数据</param>
        /// <returns></returns>
        public IEnumerable<Document> Search(Lucene.Net.Search.Query searchQuery, Lucene.Net.Search.Filter filter, Lucene.Net.Search.Sort sort, int topNumber)
        {
            return this.Channel.Search(this.indexPath, searchQuery, filter, sort, topNumber);
        }

        /// <summary>
        /// 搜索并返回分页数据
        /// </summary>
        /// <param name="searchQuery"><see cref="Lucene.Net.Search.Query"/></param>
        /// <param name="filter"><see cref="Lucene.Net.Search.Filter"/></param>
        /// <param name="sort"><see cref="Lucene.Net.Search.Sort"/></param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>内容为<see cref="Lucene.Net.Documents.Document"/>的分页数据集合</returns>
        public PagingDataSet<Document> Search(Lucene.Net.Search.Query searchQuery, Lucene.Net.Search.Filter filter, Lucene.Net.Search.Sort sort, int pageIndex, int pageSize)
        {
            return this.Channel.Search(this.indexPath, searchQuery, filter, sort, pageIndex, pageSize);
        }


        /// <summary>
        /// 更新索引内容
        /// </summary>
        /// <param name="indexDocuments">索引文档</param>
        /// <param name="ids">实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Update(IEnumerable<Document> indexDocuments, IEnumerable<string> ids, string fieldNameOfId, bool reopen = true)
        {
            this.Channel.Update(this.indexPath, indexDocuments, ids, fieldNameOfId, reopen);
        }

        /// <summary>
        /// 更新索引内容
        /// </summary>
        /// <param name="indexDocument">索引文档</param>
        /// <param name="id">实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        public void Update(Document indexDocument, string id, string fieldNameOfId, bool reopen = true)
        {
            this.Channel.Update(this.indexPath, indexDocument, id, fieldNameOfId, reopen);
        }
    }
}
