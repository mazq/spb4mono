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

namespace Tunynet.Common
{
    /// <summary>
    /// 审核严格程度
    /// </summary>
    public enum AuditStrictDegree
    {
        /// <summary>
        /// 未设置
        /// </summary>
        [Display(Name = "未设置")]
        NotSet = 0,

        /// <summary>
        /// 不审核
        /// </summary>
        [Display(Name = "不审核")]
        None = 1,

        /// <summary>
        /// 创建时审核
        /// </summary>
        [Display(Name = "创建时审核")]
        Create = 2,

        /// <summary>
        /// 更新时也审核
        /// </summary>
        [Display(Name = "再审核")]
        Update = 3
    }

    /// <summary>
    /// 审核状态
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// 未通过
        /// </summary>
        [Display(Name = "未通过")]
        Fail = 10,

        /// <summary>
        /// 待审核
        /// </summary>
        [Display(Name = "待审核")]
        Pending = 20,

        /// <summary>
        /// 需再次审核
        /// </summary>
        [Display(Name = "需再审核")]
        Again = 30,

        /// <summary>
        /// 通过审核
        /// </summary>
        [Display(Name = "通过审核")]
        Success = 40
    }

    /// <summary>
    /// 用于显示的审核状态
    /// </summary>
    public enum PubliclyAuditStatus
    {
        /// <summary>
        /// 未通过
        /// </summary>
        Fail = 10,

        /// <summary>
        /// 待审核、需再次审核、通过审核
        /// </summary>
        Pending_GreaterThanOrEqual = 19,

        /// <summary>
        /// 待审核
        /// </summary>
        Pending = 20,

        /// <summary>
        /// 需再次审核、通过审核
        /// </summary>
        Again_GreaterThanOrEqual = 29,

        /// <summary>
        /// 需再次审核
        /// </summary>
        Again = 30,

        /// <summary>
        /// 通过审核
        /// </summary>
        Success = 40
    }


}
