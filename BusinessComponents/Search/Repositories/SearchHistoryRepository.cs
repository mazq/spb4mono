//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Tunynet.Utilities;

namespace Tunynet.Search.Repositories
{
    

    /// <summary>
    /// 搜索历史仓储
    /// </summary>
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        
        //搜索历史最多保留多少个词
        int maxHistoryCount = 20;

        HttpContextBase httpContextBase = null;
        HttpContext httpContext = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SearchHistoryRepository(HttpContextBase httpContextBase)
        {
            this.httpContextBase = httpContextBase;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SearchHistoryRepository(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        //done:mazq,by zhengw:没有注释
        //mazq回复：已添加

        /// <summary>
        /// 添加搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <param name="term">搜索词</param>
        public void Insert(long userId, string searchTypeCode, string term)
        {
            HttpCookie cookie = GetCookie(userId, searchTypeCode);
            //done:mazq,by zhengw:cookie.Value可能为null，如果用户没有搜索过
            //mazq回复：已修改

            List<string> terms;
            if (!string.IsNullOrEmpty(cookie.Value))
            {
                terms = WebUtility.UrlDecode(cookie.Value).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (int i = 0; i < terms.Count; i++)
                {
                    if (terms[i].Equals(term, StringComparison.InvariantCultureIgnoreCase))
                    {
                        terms.RemoveAt(i);
                        i--;
                    }
                }
                terms.Insert(0, term);

                if (terms.Count > maxHistoryCount)
                    terms.RemoveRange(maxHistoryCount, terms.Count - maxHistoryCount);
            }
            else
            {
                terms = new List<string>() { term };
            }

            cookie.Value = WebUtility.UrlEncode(string.Join(",", terms));

            WriteCookie(cookie);
        }

        /// <summary>
        /// 清除用户的搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>   
        public void Clear(long userId, string searchTypeCode)
        {
            HttpCookie cookie = GetCookie(userId, searchTypeCode);
            cookie.Value = string.Empty;
            WriteCookie(cookie);
        }

        /// <summary>
        /// 获取用户的搜索历史
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="searchTypeCode">搜索类型编码</param>
        /// <returns>按使用时间倒序排列的搜索词</returns>
        public IEnumerable<string> Gets(long userId, string searchTypeCode)
        {
            HttpCookie cookie = GetCookie(userId, searchTypeCode);
            //done:mazq,by zhengw:cookie.Value可能为null，如果用户没有搜索过
            //mazq回复：已修复
            if (string.IsNullOrEmpty(cookie.Value))
                return Enumerable.Empty<string>();
            else
                return WebUtility.UrlDecode(cookie.Value).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }


        #region Help Methods

        /// <summary>
        /// 获取HttpCookie
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchTypeCode"></param>
        /// <returns></returns>
        private HttpCookie GetCookie(long userId, string searchTypeCode)
        {
            string cookieName = "tn-search-history:" + userId + ":" + searchTypeCode;
            HttpCookie cookie = null;

            if (httpContextBase != null)
                cookie = httpContext.Request.Cookies[cookieName];
            else if (httpContext != null)
                cookie = httpContext.Request.Cookies[cookieName];

            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                cookie.Value = string.Empty;
            }
            return cookie;
        }

        /// <summary>
        /// 写入cookie
        /// </summary>
        /// <remarks>cookie过期时间为1年</remarks>
        private void WriteCookie(HttpCookie cookie)
        {
            cookie.Expires = DateTime.Now.AddMonths(1);

            if (httpContextBase != null)
                httpContextBase.Response.Cookies.Add(cookie);
            else if (httpContext != null)
                httpContext.Response.Cookies.Add(cookie);
        }

        #endregion


    }
}
