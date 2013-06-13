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
    /// 附件媒体类型
    /// </summary>
    public enum AdvertisingType
    {
        /// <summary>
        /// 代码
        /// </summary>
        [Display(Name = "代码")]
        Script = 0,

        /// <summary>
        /// 文字
        /// </summary>
        [Display(Name = "文字")]
        Text = 1,

        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        Image = 2,

        /// <summary>
        /// Flash
        /// </summary>
        [Display(Name = "Flash")]
        Flash = 3

    }
}
