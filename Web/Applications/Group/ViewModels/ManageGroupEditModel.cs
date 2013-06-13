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
using Spacebuilder.Group;

namespace Spacebuilder.Group
{

    /// <summary>
    /// 管理群组
    /// </summary>
    public class ManageGroupEditModel
    {
        /// <summary>
        /// 群组Id
        /// </summary>
        [Display(Name = "群组Id")]
        public long GroupId { get; set; }
        /// <summary>
        /// 群主id
        /// </summary>
        [Display(Name = "群主")]
        public string UserId { get; set; }

        /// <summary>
        /// 名称关键字
        /// </summary>
        [Display(Name = "名称关键字")]
        public string GroupNameKeyword { get; set; }

        /// <summary>
        /// 类别Id
        /// </summary>
        [Display(Name = "类别")]
        public long? CategoryId { get; set; }

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
        /// 成员数下限
        /// </summary>
        [Display(Name = "成员数下限")]
        public int? minMemberCount { get; set; }

        /// <summary>
        /// 成员数上限
        /// </summary>
        [Display(Name = "成员数上限")]
        public int? maxMemberCount { get; set; }

        /// <summary>
        /// 显示条数
        /// </summary>
        [Display(Name = "显示条数")]
        public int? PageSize { get; set; }

        /// <summary>
        /// 获取Query
        /// </summary>
        /// <returns></returns>
        public GroupEntityQuery GetGroupQuery()
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
                this.EndDate = this.StartDate.Value;
                this.StartDate = temp;
            }


            return new GroupEntityQuery
            {
                GroupId=this.GroupId,
                UserId = userIdLong,
                GroupNameKeyword = this.GroupNameKeyword,
                CategoryId = this.CategoryId,
                AuditStatus = this.AuditStatus,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                minMemberCount=this.minMemberCount,
                maxMemberCount=this.maxMemberCount
            };
        }
    }
}