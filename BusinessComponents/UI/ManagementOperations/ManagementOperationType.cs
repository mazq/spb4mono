//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Tunynet.UI
{
    /// <summary>
    /// 管理操作类型
    /// </summary>
    public enum ManagementOperationType
    {
        /// <summary>
        /// 快捷操作
        /// </summary>
        [Display(Name = "快捷操作")]
        Shortcut = 1,

        /// <summary>
        /// 管理菜单
        /// </summary>
        [Display(Name = "管理菜单")]
        ManagementMenu =2
    }
}
