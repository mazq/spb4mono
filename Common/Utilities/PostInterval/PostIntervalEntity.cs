//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 发帖时间间隔实体
    /// </summary>
    public class PostIntervalEntity
    {
        /// <summary>
        /// 连续发布不同类型内容统计
        /// </summary>
        public Dictionary<PostIntervalType, int> PostContentCounts { get; set; }

        /// <summary>
        /// 不同类型内容最后更新时间
        /// </summary>
        public Dictionary<PostIntervalType, DateTime> LastPostDates { get; set; }

    }
}
