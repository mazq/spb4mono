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
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于创建和显示用户举报的Model
    /// </summary>
    public class ImpeachReportCreateModel
    {

        #region 属性

        /// <summary>
        /// 被举报人Id
        /// </summary>
        public long ReportedUserId { get; set; }

        /// <summary>
        /// 举报人Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 举报人姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入姓名！")]
        [StringLength(30, ErrorMessage = "不能超过30个字符！")]
        public string Reporter { get; set; }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "不合法的帐号邮箱")]
        [Required(ErrorMessage = "请输入邮箱地址！")]
        [StringLength(64, ErrorMessage = "不能超过64个字符！")]
        public string Email { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        // 电话号码正则表达式（支持手机号码，3-4位区号，7-8位直播号码，1－4位分机号）
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "不合法的联系电话！")]
        [Required(ErrorMessage = "请输入联系电话！")]
        public string Telephone { get; set; }

        /// <summary>
        /// 举报原因
        /// </summary>
        [Display(Name = "举报原因")]
        [Required(ErrorMessage = "请选择举报原因！")]
        public ImpeachReason Reason { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [Required(ErrorMessage = "请输入描述！")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "描述必须在8~255个字符之间！")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /// <summary>
        /// 举报内容标题
        /// </summary>
        [Display(Name = "举报内容标题")]
        public string Title { get; set; }

        /// <summary>
        /// 举报内容链接
        /// </summary>
        public string Url { get; set; }


        #endregion

        /// <summary>
        /// 转化为ImpeachReportEntity
        /// </summary>
        /// <returns>ImpeachReportEntity</returns>
        public ImpeachReportEntity AsReportEntity()
        {
            ImpeachReportEntity reportEntity = ImpeachReportEntity.New();
            User user = new UserService().GetFullUser(this.UserId);
            UserProfile profile = user!=null?user.Profile:null;
            if (profile != null)    //登陆用户
            {
                reportEntity.Reporter = user.DisplayName;
                reportEntity.Email = profile.Email;
                reportEntity.Telephone = profile.Mobile;
            }
            else
            {
                reportEntity.Reporter = this.Reporter;
                reportEntity.Email = this.Email ?? string.Empty;
                reportEntity.Telephone = this.Telephone ?? string.Empty;
            }
            reportEntity.Reason = this.Reason;
            reportEntity.Description = this.Description;
            reportEntity.Url = this.Url ?? string.Empty;
            reportEntity.ReportedUserId = this.ReportedUserId;
            reportEntity.UserId = this.UserId;
            reportEntity.Title = this.Title ?? string.Empty;
            reportEntity.DateCreated = DateTime.UtcNow;
            reportEntity.LastModified = DateTime.UtcNow;
            return reportEntity;
        }

    }
}
