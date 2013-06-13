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
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// Bar管理EditModel
    /// </summary>
    public class ManageBarEditModel
    {
        /// <summary>
        /// 帖吧的类别
        /// </summary>
        [Display(Name = "类别")]
        public long CategoryId { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        public AuditStatus? AuditStatus { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [Display(Name = "名称关键字")]
        public string KeyWord { get; set; }

        /// <summary>
        /// 帖吧吧主
        /// </summary>
        [Display(Name = "吧主")]
        public string UserId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// 显示条数
        /// </summary>
        [Display(Name = "显示条数")]
        public int? PageSize { get; set; }

        /// <summary>
        /// 根据当前实体获取对应的Query
        /// </summary>
        /// <returns></returns>
        public BarSectionQuery GetQuery()
        {
            
            

            return new BarSectionQuery
            {
                AuditStatus = this.AuditStatus,
                CategoryId = this.CategoryId,
                IsEnabled = this.Enabled,
                NameKeyword = this.KeyWord
            };
        }
    }
}