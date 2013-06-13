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
    public enum MediaType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        Image = 1,

        /// <summary>
        /// 视频
        /// </summary>
        [Display(Name = "视频")]
        Video = 2,

        /// <summary>
        /// Flash
        /// </summary>
        [Display(Name = "Flash")]
        Flash = 3,

        /// <summary>
        /// 音乐
        /// </summary>
        [Display(Name = "音乐")]
        Audio = 4,

        /// <summary>
        /// 其他类型
        /// </summary>
        [Display(Name = "其他类型")]
        Other = 99

    }
}
