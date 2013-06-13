//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Spacebuilder.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tunynet.Utilities;
using Tunynet.Mvc;
using Tunynet;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 编辑帖子的EditModel
    /// </summary>
    public class BarThreadEditModel
    {
        private static BarSettings barSettings = DIContainer.Resolve<IBarSettingsManager>().Get();
        /// <summary>
        /// 是否精华
        /// </summary>
        [Display(Name = "帖子加精")]
        public bool IsEssential { get; set; }

        /// <summary>
        /// 只恢复可见
        /// </summary>
        [Display(Name = "仅回复可见")]
        public bool IsHidden { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [Display(Name = "帖子置顶")]
        public bool IsSticky { get; set; }

        /// <summary>
        /// 置顶期限
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime StickyDate { get; set; }

        /// <summary>
        /// 帖子标题
        /// </summary>
        [WaterMark(Content = "标题")]
        [Required(ErrorMessage = "请输入标题")]
        public string Subject { get; set; }

        /// <summary>
        /// 帖子Id
        /// </summary>
        public long ThreadId { get; set; }

        /// <summary>
        /// 帖子的内容
        /// </summary>
        [Required(ErrorMessage = "请输入内容")]
        [AllowHtml]
        [DataType(DataType.Html)]
        public string Body { get; set; }

        /// <summary>
        /// 所属帖吧Id
        /// </summary>
        public long SectionId { get; set; }

        /// <summary>
        /// 主题作者作者id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public long? CategoryId { get; set; }

        public User User
        {
            get
            {
                IUserService service = DIContainer.Resolve<IUserService>();
                return service.GetFullUser(this.UserId);
            }
        }

        /// <summary>
        /// 主题帖作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<string> TagNames { get; set; }

        /// <summary>
        /// 主题帖创建的时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// 将ViewModel转换成数据库存储实体
        /// </summary>
        /// <returns></returns>
        public BarThread AsBarThread()
        {
            BarThreadService barThreadService = new BarThreadService();
            BarSectionService barSectionService = new BarSectionService();

            BarThread barThread = null;
            if (this.ThreadId > 0)
            {
                barThread = barThreadService.Get(this.ThreadId);

            }
            else
            {
                barThread = BarThread.New();
                barThread.UserId = UserContext.CurrentUser.UserId;
                barThread.Author = UserContext.CurrentUser.DisplayName;
            }
            if (barThread == null)
                return barThread;
            barThread.SectionId = this.SectionId;
            if (this.StickyDate.CompareTo(DateTime.UtcNow) > 0)
                barThread.StickyDate = this.StickyDate;
            barThread.Subject = this.Subject;
            barThread.Body = this.Body;
            barThread.IsHidden = this.IsHidden;
            barThread.IsEssential = this.IsEssential;
            barThread.IsSticky = this.IsSticky;
            barThread.LastModified = DateTime.UtcNow;
            barThread.TenantTypeId = TenantTypeIds.Instance().Bar();

            BarSection barSection = barSectionService.Get(this.SectionId);
            if (barSection != null)
            {
                barThread.OwnerId = barSection.OwnerId;
                barThread.TenantTypeId = barSection.TenantTypeId;
            }
            return barThread;
        }
    }

    /// <summary>
    /// 帖子的扩展类
    /// </summary>
    public static class BarThreadExtensions
    {
        /// <summary>
        /// 将数据库中的信息转换成EditModel实体
        /// </summary>
        /// <param name="barThread"></param>
        /// <returns></returns>
        public static BarThreadEditModel AsEditModel(this BarThread barThread)
        {
            return new BarThreadEditModel
            {
                IsEssential = barThread.IsEssential,
                Body = barThread.GetBody(),
                IsHidden = barThread.IsHidden,
                IsLocked = barThread.IsLocked,
                IsSticky = barThread.IsSticky,
                SectionId = barThread.SectionId,
                StickyDate = barThread.StickyDate,
                Subject = barThread.Subject,
                ThreadId = barThread.ThreadId,
                Author = barThread.Author,
                UserId = barThread.UserId,
                DateCreated = barThread.DateCreated,
                CategoryId = barThread.CategoryId
            };
        }
    }
}