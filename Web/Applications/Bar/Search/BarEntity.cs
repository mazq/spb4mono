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
    /// 全文检索帖吧实体类
    /// </summary>
    public class BarEntity
    {
        private long sectionId;
        /// <summary>
        /// 帖吧主键
        /// </summary>
        public long SectionId
        {
            get { return sectionId; }
            set { sectionId = value; }
        }

        private string sectionName;
        /// <summary>
        /// 帖吧名称
        /// </summary>
        public string SectionName
        {
            get { return sectionName; }
            set { sectionName = value; }
        }
        private long threadId;
        /// <summary>
        /// 发帖主键
        /// </summary>
        public long ThreadId
        {
            get { return threadId; }
            set { threadId = value; }
        }
        private long postId;
        /// <summary>
        /// 回帖主键
        /// </summary>
        public long PostId
        {
            get { return postId; }
            set { postId = value; }
        }
        private string subject;
        /// <summary>
        /// 标题
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        private string body;
        /// <summary>
        /// 内容
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        private long userId;
        /// <summary>
        /// 作者用户ID
        /// </summary>
        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private string author;
        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        private List<string> tag;
        /// <summary>
        /// 所有标签
        /// </summary>
        public List<string> Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        private string category;
        /// <summary>
        /// 所有类别
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        private DateTime dateCreated;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }
        private bool isPost;
        /// <summary>
        /// 是否回帖
        /// </summary>
        public bool IsPost
        {
            get { return isPost; }
            set { isPost = value; }
        }
    }
}