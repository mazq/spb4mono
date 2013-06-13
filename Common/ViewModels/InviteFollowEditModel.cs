//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Mvc;
using System.ComponentModel.DataAnnotations;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 请关注编辑实体
    /// </summary>
    public class InviteFollowEditModel
    {
        /// <summary>
        /// 用户对外显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 用户输入的提示
        /// </summary>
        [WaterMark(Content = "请求内容")]
        [StringLength(100, ErrorMessage = "最多可以输入一百字")]
        [DataType(DataType.MultilineText)]
        public string remark { get; set; }

    }
}
