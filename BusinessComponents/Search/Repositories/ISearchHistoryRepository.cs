//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Search.Repositories
{
    /// <summary>
    /// 搜索历史仓储接口
    /// </summary>
    public interface ISearchHistoryRepository
    {
        /// <summary>
        /// 添加搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词</param>
        void Insert(long userId, string searchTypeCode, string term);


        /// <summary>
        /// 清除用户的搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>        
        void Clear(long userId, string searchTypeCode);

        /// <summary>
        /// 获取用户的搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>按使用时间倒序排列的搜索词</returns>
        IEnumerable<string> Gets(long userId, string searchTypeCode);


    }
}
