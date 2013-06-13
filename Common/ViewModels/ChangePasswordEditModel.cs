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
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 修改用户密码的类
    /// </summary>
    public class ChangePasswordEditModel
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Display(Name = "旧密码")]
        [Required(ErrorMessage = "请输入旧密码")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Remote("ValidatePassword", "Account", ErrorMessage = "请输入合法的密码")]
        [Display(Name = "新密码")]
        [Required(ErrorMessage = "请输入新密码")]
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }

        /// <summary>
        ///  确认密码
        /// </summary>
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "输入的密码与新密码不同")]//设置确认密码是否与密码相同
        public string ComparePassword { get; set; }
    }
}
