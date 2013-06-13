//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 字符处理状态
    /// </summary>
    public enum WordFilterStatus
    {
        /// <summary>
        /// 匹配到需要替代的关键字
        /// </summary>
        [Display(Name = "替换")]
        Replace,

        /// <summary>
        /// 匹配到禁止提交的关键字
        /// </summary>
        [Display(Name = "禁用")]
        Banned
    }
}
