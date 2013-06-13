//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ServiceModel;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Tunynet;

namespace Spacebuilder.Search
{
    /// <summary>
    /// 搜索引擎WCF服务的服务契约
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(QueryWrapperFilter))]
    [ServiceKnownType(typeof(Query))]
    [ServiceKnownType(typeof(BooleanQuery))]
    [ServiceKnownType(typeof(PrefixQuery))]
    [ServiceKnownType(typeof(PhraseQuery))]
    [ServiceKnownType(typeof(TermRangeQuery))]
    [ServiceKnownType(typeof(TermQuery))]
    [ServiceKnownType(typeof(MultiTermQuery))]
    [ServiceKnownType(typeof(MultiTermQuery.AnonymousClassConstantScoreAutoRewrite))]
    [ServiceKnownType(typeof(BooleanClause))]
    [ServiceKnownType(typeof(Term))]
    [ServiceKnownType(typeof(Filter))]
    [ServiceKnownType(typeof(SortField))]
    [ServiceKnownType(typeof(Field))]
    [ServiceKnownType(typeof(Analyzer))]
    [ServiceKnownType(typeof(Document))]
    [ServiceKnownType(typeof(PagingDataSet<Document>))]
    public interface ISearchEngineService
    {
        /// <summary>
        /// 初始化搜索引擎
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        [OperationContract]
        void Initialize(string indexPath);

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
        [OperationContract]
        void RebuildIndex(string indexPath, IEnumerable<Document> indexDocuments, bool isBeginning, bool isEndding);

        /// <summary>
        /// 添加索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocument">待添加的索引文档</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        [OperationContract(Name = "Insert")]
        void Insert(string indexPath, Document indexDocument, bool reopen = true);

        /// <summary>
        /// 添加索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocuments">待添加的索引文档</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        [OperationContract(Name = "BulkInsert")]
        void Insert(string indexPath, IEnumerable<Document> indexDocuments, bool reopen = true);

        /// <summary>
        /// 删除索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="id">索引内容对应的实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        [OperationContract(Name = "Delete")]
        void Delete(string indexPath, string id, string fieldNameOfId, bool reopen = true);

        /// <summary>
        /// 删除索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="ids">索引内容对应的实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        [OperationContract(Name = "BulkDelete")]
        void Delete(string indexPath, IEnumerable<string> ids, string fieldNameOfId, bool reopen = true);

        /// <summary>
        /// 更新索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocument">索引文档</param>
        /// <param name="id">实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        [OperationContract(Name = "Update")]
        void Update(string indexPath, Document indexDocument, string id, string fieldNameOfId, bool reopen = true);

        /// <summary>
        /// 更新索引内容
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="indexDocuments">索引文档</param>
        /// <param name="ids">实体主键</param>
        /// <param name="fieldNameOfId">实体主键对应的索引字段名称</param>
        /// <param name="reopen">是否重新打开NRT查询</param>
        [OperationContract(Name = "BulkUpdate")]
        void Update(string indexPath, IEnumerable<Document> indexDocuments, IEnumerable<string> ids, string fieldNameOfId, bool reopen = true);

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
        [OperationContract(Name = "SearchPaging")]
        PagingDataSet<Document> Search(string indexPath, Query searchQuery, Filter filter, Sort sort, int pageIndex, int pageSize);

        /// <summary>
        /// 搜索并返回前topNumber条数据
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <param name="searchQuery"><see cref="Lucene.Net.Search.Query"/></param>
        /// <param name="filter"><see cref="Lucene.Net.Search.Filter"/></param>
        /// <param name="sort"><see cref="Lucene.Net.Search.Sort"/></param>
        /// <param name="topNumber">最多返回多少条数据</param>
        /// <returns></returns>
        [OperationContract(Name = "SearchTop")]
        IEnumerable<Document> Search(string indexPath, Query searchQuery, Filter filter, Sort sort, int topNumber);

        /// <summary>
        /// 获取当前搜索引擎的索引大小，单位为字节(Byte)
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        [OperationContract(Name = "GetIndexSize")]
        double GetIndexSize(string indexPath);

        /// <summary>
        /// 获取当前搜索引擎的索引最后更新时间
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        [OperationContract(Name = "GetLastModified")]
        DateTime GetLastModified(string indexPath);
    }
}
