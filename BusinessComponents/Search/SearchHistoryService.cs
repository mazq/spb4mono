//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Search.Repositories;

namespace Tunynet.Search
{
    /// <summary>
    /// 搜索历史业务逻辑类
    /// </summary>
    public class SearchHistoryService
    {

        #region 构造器
        private ISearchHistoryRepository searchHistoryRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public SearchHistoryService()
            : this(new SearchHistoryRepository(System.Web.HttpContext.Current))
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="searchHistoryRepository">SearchHistory仓储</param>        
        public SearchHistoryService(ISearchHistoryRepository searchHistoryRepository)
        {
            this.searchHistoryRepository = searchHistoryRepository;
        }
        #endregion


        /// <summary>
        /// 记录到搜索记录
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词</param>
        public void SearchTerm(long userId, string searchTypeCode, string term)
        {
            searchHistoryRepository.Insert(userId, searchTypeCode, term);
        }

        /// <summary>
        /// 清除用户的搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        public void Clear(long userId, string searchTypeCode)
        {
            searchHistoryRepository.Clear(userId, searchTypeCode);
        }

        /// <summary>
        /// 获取用户最近搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>        
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>按使用时间倒序排列的搜索词</returns>
        public IEnumerable<string> Gets(long userId, string searchTypeCode)
        {
            return searchHistoryRepository.Gets(userId, searchTypeCode);
        }

    }
}
