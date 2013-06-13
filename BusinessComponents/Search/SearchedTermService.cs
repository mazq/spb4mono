//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using Tunynet.Search.Repositories;

namespace Tunynet.Search
{
    /// <summary>
    /// 搜索热词业务逻辑类
    /// </summary>
    public class SearchedTermService
    {

        private ISearchedTermRepository searchedTermRepository;
   

        #region 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        public SearchedTermService()
            : this(new SearchedTermRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="searchedTermRepository">SearchedTermRepository仓储</param>
        public SearchedTermService(ISearchedTermRepository searchedTermRepository)
        {
            this.searchedTermRepository = searchedTermRepository;
      
        } 
        #endregion

        /// <summary>
        /// 搜索词记录及计数
        /// </summary>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词</param>
        public void SearchTerm(string searchTypeCode, string term)
        {
            long id = searchedTermRepository.InsertOrUpdate(searchTypeCode, term, false);
            CountService countService = new CountService(TenantTypeIds.Instance().Search());
            countService.ChangeCount(CountTypes.Instance().SearchCount(), id, 0, 1, false);
        }

        /// <summary>
        /// 管理员添加搜索词
        /// </summary>
        /// <param name="searchTypeCode"></param>
        /// <param name="term"></param>
        public void CreateByAdministrator(string searchTypeCode, string term)
        {
            searchedTermRepository.InsertOrUpdate(searchTypeCode, term, true);
        }

        /// <summary>
        /// 更新管理员添加的搜索词
        /// </summary>
        /// <param name="id">搜索词Id</param>
        /// <param name="term">搜索词</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        public void Update(long id, string term, string searchTypeCode)
        {
            //仅允许对管理员添加的搜索词进行更新
            searchedTermRepository.Update(id, term, searchTypeCode);
        }


        /// <summary>
        /// 变更搜索词的排列顺序
        /// </summary>
        /// <param name="id">待调整的Id</param>
        /// <param name="referenceId">参照Id</param>        
        public void ChangeDisplayOrder(long id, long referenceId)
        {
            searchedTermRepository.ChangeDisplayOrder(id, referenceId);
        }

        /// <summary>
        /// 删除搜索词
        /// </summary>
        /// <param name="id">搜索词Id</param>
        public void Delete(long id)
        {
            searchedTermRepository.DeleteByEntityId(id);
            CountService countService = new CountService(TenantTypeIds.Instance().Search());
            countService.Delete(id);
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
            searchedTermRepository.JudgeDisplayOrder(id, out isFirst, out isLast, out decreaseReferenceId, out increaseReferenceId);
        }

        /// <summary>
        /// 获取人工干预的搜索词
        /// </summary>
        /// <remarks>按DisplayOrder正序排列</remarks>
        /// <returns></returns>
        public IEnumerable<SearchedTerm> GetManuals(string searchTypeCode)
        {
            return searchedTermRepository.GetManuals(searchTypeCode);
        }

        /// <summary>
        /// 获取匹配的人工干预的搜索词
        /// </summary>
        /// <remarks>按DisplayOrder正序排列</remarks>
        /// <returns></returns>
        public IEnumerable<SearchedTerm> GetManuals(string keyword,string searchTypeCode)
        {
            return searchedTermRepository.GetManuals(keyword, searchTypeCode);
        }

        /// <summary>
        /// 获取前N条最热的搜索词（仅非人工干预）
        /// </summary>
        /// <remarks>按DisplayOrder正序排列</remarks>
        /// <param name="topNumber">获取的数据条数</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns></returns>
        public IEnumerable<SearchedTerm> GetTops(int topNumber, string searchTypeCode)
        {
            return searchedTermRepository.GetTops(topNumber, searchTypeCode);
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
            return searchedTermRepository.GetTops(keyword,topNumber, searchTypeCode);
        }

        /// <summary>
        /// 分页获取搜索词（仅非人工干预）
        /// </summary>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词关键字（支持右半模糊搜索）</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <param name="isRealtime">是否需要即时缓存</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<SearchedTerm> Gets(string searchTypeCode, string term, DateTime? startDate, DateTime? endDate, bool isRealtime,int pageSize, int pageIndex)
        {
            return searchedTermRepository.Gets(searchTypeCode, term, startDate, endDate, isRealtime,pageSize, pageIndex);      
        }

    }
}
