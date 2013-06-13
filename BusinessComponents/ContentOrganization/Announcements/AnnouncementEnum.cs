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
    /// 公告状态
    /// </summary>
    public enum Announcement_Status
    {

        /// <summary>
        /// 未发布
        /// </summary>
        [Display(Name = "未发布")]
        UnPublish = 0,

        /// <summary>
        /// 已发布
        /// </summary>
        [Display(Name = "已发布")]
        Published = 1,

        /// <summary>
        /// 已过期
        /// </summary>
        [Display(Name = "已过期")]
        Expired = 2


    }

    /// <summary>
    /// 公告呈现区域
    /// </summary>
    public enum Announcement_DisplayArea
    {

        /// <summary>
        /// 频道首页
        /// </summary>
        [Display(Name = "频道首页")]
        Home = 0,

        /// <summary>
        /// 用户空间首页
        /// </summary>
        [Display(Name = "用户空间首页")]
        UserSpace = 1

    }
}
