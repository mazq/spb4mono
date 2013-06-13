//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 创建公告的类
    /// </summary>
    [Serializable]
    public class AnnouncementEditModel
    {
        /// <summary>
        ///Primary key
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        ///公告主题
        /// </summary>
        [Required(ErrorMessage = "请输入公告主题")]
        [StringLength(TextLengthSettings.TEXT_SUBJECT_MAXLENGTH, ErrorMessage = "主题名过长")]
        public string Subject { get; set; }

        /// <summary>
        ///主题字体风格
        /// </summary>
        public string SubjectStyle { get; set; }

        /// <summary>
        ///公告内容
        /// </summary>
        [AllowHtml]
        [DataType(DataType.Html)]        
        [StringLength(TextLengthSettings.TEXT_BODY_MAXLENGTH, ErrorMessage = "内容过长")]
        public string Body { get; set; }

        /// <summary>
        ///是否是连接
        /// </summary>
        public bool IsHyperLink { get; set; }


        private string _hyperLinkUrl = "http://";

        /// <summary>
        ///链接地址
        /// </summary>
        [Required(ErrorMessage = "请输入链接地址")]        
        public string HyperLinkUrl
        {
            get { return _hyperLinkUrl; }
            set { _hyperLinkUrl = value; }
        }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool EnabledDescription { get; set; }

        /// <summary>
        ///发布时间
        /// </summary>
        [Required(ErrorMessage = "请输入发布时间")]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        ///过期时间
        /// </summary>
        /// ExpiredDate != DateTime.MinValue
        [Required(ErrorMessage = "请输入过期时间")]       
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        ///创建人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///展示区域
        /// </summary>
        public string DisplayArea { get; set; }

        /// <summary>
        /// 转化为Announcement用于数据库存储
        /// </summary>
        /// <returns>Announcement</returns>
        public Announcement AsAnnouncement()
        {
            Announcement announcement = null;

            if (this.Id.HasValue && this.Id > 0)
                announcement = new AnnouncementService().Get(this.Id.Value);
            else
                announcement = Announcement.New();

            announcement.Subject = this.Subject;
            if (this.SubjectStyle != null)
            {
                announcement.SubjectStyle = this.SubjectStyle;
            }
            
            announcement.Body = this.Body??string.Empty;
            
            announcement.IsHyperLink = this.IsHyperLink;
            if (this.HyperLinkUrl != null)
            {
                announcement.HyperLinkUrl = this.HyperLinkUrl;
            }
            announcement.EnabledDescription = this.EnabledDescription;
            announcement.ReleaseDate = this.ReleaseDate;
            announcement.ExpiredDate = this.ExpiredDate;
            announcement.UserId = this.UserId;
            announcement.DisplayArea = this.DisplayArea;

            return announcement;
        }
    }

    /// <summary>
    /// 公告的扩展方法
    /// </summary>
    public static class AnnouncementExtensions
    {
        /// <summary>
        /// 转换成AsEditModel
        /// </summary>
        public static AnnouncementEditModel AsEditModel(this Announcement announcement)
        {
            return new AnnouncementEditModel
            {
                Id = announcement.Id,
                Subject = announcement.Subject,
                SubjectStyle = announcement.SubjectStyle,
                Body = announcement.Body??string.Empty,
                IsHyperLink = announcement.IsHyperLink,
                HyperLinkUrl = announcement.HyperLinkUrl,
                EnabledDescription = announcement.EnabledDescription,
                ReleaseDate = announcement.ReleaseDate,
                ExpiredDate = announcement.ExpiredDate,
                UserId = announcement.UserId,
                DisplayArea = announcement.DisplayArea
            };
        }
    }
}
