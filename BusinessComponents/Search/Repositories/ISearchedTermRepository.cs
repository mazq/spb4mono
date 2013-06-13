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


namespace Tunynet.Search.Repositories
{
    /// <summary>
    /// 搜索热词仓储接口
    /// </summary>
    public interface ISearchedTermRepository : IRepository<SearchedTerm>
    {
        
        /// <summary>
        /// 添加或更新搜索热词
        /// </summary>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词</param>
        long InsertOrUpdate(string searchTypeCode, string term, bool isAddedByAdministrator);

       

        /// <summary>
        /// 获取人工干预的搜索词
        /// </summary>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>符合条件的搜索词集合</returns>
        IEnumerable<SearchedTerm> GetManuals(string searchTypeCode);

        /// <summary>
        /// 获取匹配的人工干预的搜索词
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>符合条件的搜索词集合</returns>
        IEnumerable<SearchedTerm> GetManuals(string keyword, string searchTypeCode);

        /// <summary>
        /// 获取前N条最热的搜索词（仅非人工干预）
        /// </summary>
        /// <param name="topNumber">获取的数据条数</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>用户最多搜索的前topNumber的搜索词</returns>
        IEnumerable<SearchedTerm> GetTops(int topNumber, string searchTypeCode);

        /// <summary>
        /// 获取匹配的前N条最热的搜索词（仅非人工干预）
        /// </summary>
        /// <param name="keyword">要匹配的关键字</param>
        /// <param name="topNumber">获取的数据条数</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>用户最多搜索的前topNumber的搜索词</returns>
        IEnumerable<SearchedTerm> GetTops(string keyword, int topNumber, string searchTypeCode);

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
        PagingDataSet<SearchedTerm> Gets(string searchTypeCode, string term, DateTime? startDate, DateTime? endDate, bool isRealtime,int pageSize, int pageIndex);

        /// <summary>
        /// 判定搜索词的DisplayOrder可以做哪些调整
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="isFirst">在人工干预同类型搜索词中是否处于首位</param>
        /// <param name="isLast">在人工干预同类型搜索词中是否处于末位</param>
        /// <param name="decreaseReferenceId">降低DisplayOrder时参照的Id</param>
        /// <param name="increaseReferenceId">增加DisplayOrder时参照的Id</param>
       void JudgeDisplayOrder(long id, out bool isFirst, out bool isLast, out long decreaseReferenceId, out long increaseReferenceId);


        /// <summary>
        /// 变更搜索词的排列顺序
        /// </summary>
        /// <param name="id">待调整的Id</param>
        /// <param name="referenceId">参照Id</param>        
        void ChangeDisplayOrder(long id, long referenceId);


        /// <summary>
        /// 从数据库删除实体
        /// </summary>
        /// <param name="entityId">主键</param>
        /// <returns>影响的记录数</returns>
        int DeleteByEntityId(object entityId);

        
        /// <summary>
        /// 更新管理员添加的搜索词
        /// </summary>
        /// <param name="id">搜索词Id</param>
        /// <param name="term">搜索词</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        void Update(long id, string term, string searchTypeCode);

    }

    
}
