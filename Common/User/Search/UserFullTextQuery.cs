//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Mvc;


namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户全文检索条件
    /// </summary>
    public class UserFullTextQuery
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 关键字列表
        /// </summary>
        public IEnumerable<string> Keywords { get; set; }

        /// <summary>
        /// 搜索范围：全部 姓名 标签 学校 公司 
        /// </summary>
        public UserSearchRange SearchRange { get; set; }

        /// <summary>
        /// 所在地区
        /// </summary>
        public string NowAreaCode { get; set; }

        /// <summary>
        /// 家乡
        /// </summary>
        public string HomeAreaCode { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// 年龄下限
        /// </summary>
        public int AgeMin { get; set; }

        /// <summary>
        /// 年龄上限
        /// </summary>
        public int AgeMax { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public UserSearchSortBy SortBy = UserSearchSortBy.Relevance;

        /// <summary>
        /// 当前显示页面页码
        /// </summary>
        private int pageIndex = 1;
        public int PageIndex
        {
            get
            {
                if (pageIndex < 1)
                    return 1;
                else
                    return pageIndex;
            }
            set { pageIndex = value; }
        }

        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize = 10;

    }

    /// <summary>
    /// 用户搜索排序
    /// </summary>
    public enum UserSearchSortBy
    {
        /// <summary>
        /// 按相关度倒序
        /// </summary>
        Relevance,

        /// <summary>
        /// 按注册时间正序
        /// </summary>
        DateCreated_Asc,

        /// <summary>
        /// 按注册时间倒序
        /// </summary>
        DateCreated_Desc,

        /// <summary>
        /// 按上次活动时间正序
        /// </summary>
        LastActivityTime_Asc,

        /// <summary>
        /// 按上次活动时间倒序
        /// </summary>
        LastActivityTime_Desc
    }

    /// <summary>
    /// 用户搜索范围
    /// </summary>
    public enum UserSearchRange
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL = 0,

        /// <summary>
        /// 姓名
        /// </summary>
        NAME = 1,

        /// <summary>
        /// 标签
        /// </summary>
        TAG = 2,

        /// <summary>
        /// 学校
        /// </summary>
        SCHOOL = 3,

        /// <summary>
        /// 公司
        /// </summary>
        COMPANY = 4,
        
        /// <summary>
        /// 所在地
        /// </summary>
        NOWAREACODE = 5,

        /// <summary>
        /// 家乡
        /// </summary>
        HOMEAREACODE = 6,

        /// <summary>
        /// 性别
        /// </summary>
        Gender=7,

        /// <summary>
        /// 年龄
        /// </summary>
        Age=8
    }
}
