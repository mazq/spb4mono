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

namespace Spacebuilder.Group
{
    /// <summary>
    /// 封装管理群组时用于查询帖子的条件
    /// </summary>
    public class GroupEntityQuery
    {
        /// <summary>
        /// 群组Id
        /// </summary>
        public long? GroupId { get; set; }
        /// <summary>
        /// 群主id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 名称关键字
        /// </summary>
        public string GroupNameKeyword { get; set; }

        /// <summary>
        /// 类别Id
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatus? AuditStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 成员数下限
        /// </summary>
        public int? minMemberCount { get; set; }

        /// <summary>
        /// 成员数上限
        /// </summary>
        public int? maxMemberCount { get; set; }
    }
}