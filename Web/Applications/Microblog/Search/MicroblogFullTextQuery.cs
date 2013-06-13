//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;


namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博全文检索条件
    /// </summary>
    public class MicroblogFullTextQuery
    {

        #region 查询条件

        /// <summary>
        /// 关键词集合
        /// </summary>
        public IEnumerable<string> Keywords { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }

        private bool isFuzzy = true;
        /// <summary>
        /// 是否模糊查询(true查Body，false查Topic)
        /// </summary>
        public bool IsFuzzy { get{return isFuzzy;}set{isFuzzy=value;}}

        /// <summary>
        /// 搜索条件：全部 原创 含图 含音乐 含视频
        /// </summary>
        public MicroblogSearchTerm SearchTerm { get; set; }

        private bool isGroup = false;
        /// <summary>
        /// 不查询群组的
        /// </summary>
        public bool IsGroup { get { return isGroup; } set { isGroup = value; } }

        /// <summary>
        /// 搜索范围:微博内容、话题
        /// </summary>
        public MicroblogSearchRange SearchRange { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public MicroblogSearchSortBy SortBy = MicroblogSearchSortBy.Relevance;

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
        public int PageSize = 20;

        #endregion

    }

    /// <summary>
    /// 微博搜索排序
    /// </summary>
    public enum MicroblogSearchSortBy
    {
        /// <summary>
        /// 按相关度倒序
        /// </summary>
        Relevance,

        /// <summary>
        /// 按回复数倒序
        /// </summary>
        ReplyCount_Desc,

        /// <summary>
        /// 按被转发数倒序
        /// </summary>
        ForwardedCount_Desc,


        /// <summary>
        /// 按创建时间倒序
        /// </summary>
        DateCreated_Desc,

    }

    /// <summary>
    /// 微博搜索条件
    /// </summary>
    public enum MicroblogSearchTerm
    {
        /// <summary>
        /// 全部
        /// </summary>
        ALL=0,

        /// <summary>
        /// 原创
        /// </summary>
        ISORIGINALITY = 1,

        /// <summary>
        /// 含图
        /// </summary>
        HASPHOTO = 2,

        /// <summary>
        /// 含音乐
        /// </summary>
        HASMUSIC = 3,

        /// <summary>
        /// 含视频
        /// </summary>
        HASVIDEO = 4
    }

    /// <summary>
    /// 微博搜索范围
    /// </summary>
    public enum MicroblogSearchRange
    {
        /// <summary>
        /// 微博内容
        /// </summary>
        BODY,
        /// <summary>
        /// 微博话题
        /// </summary>
        TOPIC
    }
}