//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spacebuilder.Bar.Search
{
    /// <summary>
    /// 帖吧全文检索条件
    /// </summary>
    public class BarFullTextQuery
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 租户类型id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        /// 搜索范围：标题 全文 作者 标签 
        /// </summary>
        public BarSearchRange Term { get; set; }

        private string sectionId = "-1";
        /// <summary>
        /// 帖吧ID
        /// </summary>
        public string Range { get { return sectionId; } set { sectionId = value; } }

        private string isPost = "2";
        /// <summary>
        /// 是否回帖(1是 0不是 2全部)
        /// </summary>
        public string IsPost { get { return isPost; } set { isPost = value; } }

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
    /// 帖吧搜索范围
    /// </summary>
    public enum BarSearchRange
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL=0,

        /// <summary>
        /// 标题
        /// </summary>
        SUBJECT = 1,

        /// <summary>
        /// 全文
        /// </summary>
        BODY = 2,

        /// <summary>
        /// 作者
        /// </summary>
        AUTHOR = 3,

        /// <summary>
        /// 标签
        /// </summary>
        TAG = 4,
    }
}