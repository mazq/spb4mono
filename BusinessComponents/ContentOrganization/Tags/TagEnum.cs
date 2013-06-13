//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 标签排序字段
    /// </summary>
    public enum SortBy_Tag
    {
        /// <summary>
        /// 使用数
        /// </summary>
        OwnerCountDesc,

        /// <summary>
        /// 内容数
        /// </summary>
        ItemCountDesc,

        /// <summary>
        /// 每日内容数
        /// </summary>
        PreDayItemCountDesc,

        /// <summary>
        /// 每周内容数
        /// </summary>
        PreWeekItemCountDesc,

        /// <summary>
        /// 发布日期
        /// </summary>
        DateCreated,

        /// <summary>
        /// 发布日期倒序
        /// </summary>
        DateCreatedDesc
    }
}