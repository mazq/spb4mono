//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 短网址数据访问接口
    /// </summary>
    public interface IShortUrlRepository : IRepository<ShortUrlEntity>
    {
        /// <summary>
        /// 获取未使用的Url别名
        /// </summary>
        /// <param name="aliases">Url别名集合</param>
        /// <param name="url">待处理的Url</param>
        /// <param name="urlExists">带处理Url是否已存在</param>
        string GetUnusedAlias(string[] aliases, string url, out bool urlExists);
    }
}