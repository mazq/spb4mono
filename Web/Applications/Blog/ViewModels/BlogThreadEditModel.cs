//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Spacebuilder.Common;
using Tunynet.Common;
using Tunynet.Mvc;
using System.Collections;
using System.Collections.Generic;
using Spacebuilder.Search;
using System.Text;
using Tunynet.Utilities;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 编辑日志的EditModel
    /// </summary>
    public class BlogThreadEditModel
    {
        AttachmentService<Attachment> attachmentService = new AttachmentService<Attachment>(TenantTypeIds.Instance().BlogThread());

        /// <summary>
        /// Id
        /// </summary>
        public long ThreadId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [WaterMark(Content = "在此输入日志标题")]
        [Required(ErrorMessage = "请输入日志标题")]
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, MinimumLength = TextLengthSettings.TEXT_SUBJECT_MINLENGTH, ErrorMessage = "最多可以输入{1}个字")]
        public string Subject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [Required(ErrorMessage = "请输入日志内容")]
        [StringLength(TextLengthSettings.TEXT_BODY_MAXLENGTH, MinimumLength = TextLengthSettings.TEXT_BODY_MINLENGTH, ErrorMessage = "最多可以输入{1}个字")]
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Body { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [Display(Name = "置顶")]
        public bool IsSticky { get; set; }

        /// <summary>
        /// 是否禁止评论
        /// </summary>
        [Display(Name = "禁止评论")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 是否草稿
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "关键字")]
        [WaterMark(Content = "关键字用于设置日志页面的Meta标签，利于SEO")]
        [StringLength(64, ErrorMessage = "最多可以输入64个字")]
        public string Keywords { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Display(Name = "摘要")]
        [StringLength(TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH, ErrorMessage = "最多可以输入{1}个字")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        /// <summary>
        /// 所有者id
        /// </summary>
        public long? OwnerId { get; set; }

        /// <summary>
        /// 用户日志分类
        /// </summary>
        public string OwnerCategoryIds { get; set; }

        /// <summary>
        /// 站点日志分类
        /// </summary>
        public long? SiteCategoryId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<string> TagNames { get; set; }

        /// <summary>
        ///隐私状态
        /// </summary>
        public PrivacyStatus PrivacyStatus { get; set; }

        /// <summary>
        /// 隐私设置用户列表
        /// </summary>
        public string PrivacyStatus1 { get; set; }

        /// <summary>
        /// 隐私设置分组列表
        /// </summary>
        public string PrivacyStatus2 { get; set; }

        private long featuredImageAttachmentId = 0;
        /// <summary>
        /// 标题图对应的附件Id
        /// </summary>
        public long FeaturedImageAttachmentId
        {
            get
            {
                return featuredImageAttachmentId;
            }
            set
            {
                featuredImageAttachmentId = value;
            }
        }

        /// <summary>
        /// 转换成BlogThread类型
        /// </summary>
        /// <returns>日志实体</returns>
        public BlogThread AsBlogThread()
        {
            BlogThread blogThread = null;

            //写日志
            if (this.ThreadId == 0)
            {
                blogThread = BlogThread.New();
                blogThread.UserId = UserContext.CurrentUser.UserId;
                blogThread.Author = UserContext.CurrentUser.DisplayName;
                if (this.OwnerId.HasValue)
                {
                    blogThread.OwnerId = this.OwnerId.Value;
                    blogThread.TenantTypeId = TenantTypeIds.Instance().Group();
                }
                else
                {
                    blogThread.OwnerId = UserContext.CurrentUser.UserId;
                    blogThread.TenantTypeId = TenantTypeIds.Instance().User();
                }
                blogThread.OriginalAuthorId = UserContext.CurrentUser.UserId;
            }
            //编辑日志
            else
            {
                BlogService blogService = new BlogService();
                blogThread = blogService.Get(this.ThreadId).Clone();
                blogThread.LastModified = DateTime.UtcNow;
            }

            blogThread.Subject = this.Subject;
            blogThread.Body = this.Body;
            blogThread.IsDraft = this.IsDraft;
            blogThread.IsSticky = this.IsSticky;
            blogThread.IsLocked = this.IsLocked;
            blogThread.PrivacyStatus = this.PrivacyStatus;
            blogThread.Keywords = this.Keywords;
            if (string.IsNullOrEmpty(blogThread.Keywords))
            {
                string[] keywords = ClauseScrubber.TitleToKeywords(this.Subject);
                blogThread.Keywords = string.Join(" ", keywords);
            }
            blogThread.Summary = this.Summary;
            //if (string.IsNullOrEmpty(blogThread.Summary))
            //{
                blogThread.Summary = HtmlUtility.TrimHtml(this.Body, this.Body.Length).Substring(0, HtmlUtility.TrimHtml(this.Body, this.Body.Length).Length >= TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH ? TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH : HtmlUtility.TrimHtml(this.Body, this.Body.Length).Length);
            //}

            blogThread.FeaturedImageAttachmentId = this.FeaturedImageAttachmentId;
            if (blogThread.FeaturedImageAttachmentId > 0)
            {
                Attachment attachment = attachmentService.Get(blogThread.FeaturedImageAttachmentId);
                if (attachment != null)
                {
                    blogThread.FeaturedImage = attachment.GetRelativePath() + "\\" + attachment.FileName;
                }
                else
                {
                    blogThread.FeaturedImageAttachmentId = 0;
                }
            }
            else
            {
                blogThread.FeaturedImage = string.Empty;
            }

            return blogThread;

        }
    }

    /// <summary>
    /// 日志实体的扩展类
    /// </summary>
    public static class BlogThreadExtensions
    {
        /// <summary>
        /// 将数据库中的信息转换成EditModel实体
        /// </summary>
        /// <param name="blogThread">日志实体</param>
        /// <returns>编辑日志的EditModel</returns>
        public static BlogThreadEditModel AsEditModel(this BlogThread blogThread)
        {
            return new BlogThreadEditModel
            {
                ThreadId = blogThread.ThreadId,
                Subject = blogThread.Subject,
                Body = blogThread.GetBody(),
                IsSticky = blogThread.IsSticky,
                IsLocked = blogThread.IsLocked,
                PrivacyStatus = blogThread.PrivacyStatus,
                Keywords = blogThread.Keywords,
                Summary = blogThread.Summary,
                IsDraft = blogThread.IsDraft,
                FeaturedImageAttachmentId = blogThread.FeaturedImageAttachmentId
            };
        }
    }
}