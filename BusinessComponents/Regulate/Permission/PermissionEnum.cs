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
    /// 权限许可类型
    /// </summary>
    public enum PermissionType
    {
        [Display(Name = "未设置")]
        /// <summary>
        /// 未设置
        /// </summary>
        NotSet = 0,
         [Display(Name = "允许")]
        /// <summary>
        /// 允许
        /// </summary>
        Allow = 1,
         [Display(Name = "拒绝")]
        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse = 2
    }

    /// <summary>
    /// 权限许可范围
    /// </summary>
    /// <remarks>
    /// 范围越大对应的整型值越大
    /// </remarks>
    public enum PermissionScope
    {
         [Display(Name = "所有的")]
        /// <summary>
        /// 所有的
        /// </summary>
        All = 4,
         [Display(Name = "所属的")]
        /// <summary>
        /// 所属的
        /// </summary>
        Organization = 3,
         [Display(Name = "所辖的")]
        /// <summary>
        /// 所辖的
        /// </summary>
        Management = 2,
         [Display(Name = "用户的")]
        /// <summary>
        /// 用户的
        /// </summary>
        User = 1
    }

}
