//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 管理回帖
    /// </summary>
    public class ManagePostsEditModel
    {

        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        public AuditStatus? AuditStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 帖吧id
        /// </summary>
        [Display(Name = "帖吧")]
        public long? SectionId { get; set; }

        /// <summary>
        /// 标题关键字
        /// </summary>
        [Display(Name = "回帖关键字")]
        public string PostKeyword { get; set; }

        /// <summary>
        /// 吧主id
        /// </summary>
        [Display(Name = "作者")]
        public string UserId { get; set; }

        /// <summary>
        /// 显示条数
        /// </summary>
        [Display(Name = "显示条数")]
        public int? PageSize { get; set; }

        /// <summary>
        /// 将Edit转换成对应的Query
        /// </summary>
        /// <returns></returns>
        public BarPostQuery AsBarPostQuery()
        {
            long? userIdLong = null;
            if (!string.IsNullOrEmpty(UserId))
            {
                long id;
                long.TryParse(UserId.Replace(",", ""), out id);
                userIdLong = id;
            }

            if (this.StartDate.HasValue && this.EndDate.HasValue && this.StartDate.Value > this.EndDate.Value)
            {
                DateTime temp = this.StartDate.Value;
                this.StartDate = this.EndDate;
                this.EndDate = temp;
            }

            return new BarPostQuery
            {
                AuditStatus = this.AuditStatus,
                EndDate = this.EndDate,
                SectionId = this.SectionId,
                StartDate = this.StartDate,
                PostKeyword = this.PostKeyword,
                UserId = userIdLong
            };
        }
    }
}