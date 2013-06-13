//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 重设密码ViewModel
    /// </summary>
    public class ResetPasswordEditModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "帐号")]
        [Required(ErrorMessage = "帐号为必填项")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Remote("ValidatePassword", "Account", ErrorMessage = "请输入合法的密码")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码为必填项")]
        public string Password { get; set; }

        /// <summary>
        /// 验证密码
        /// </summary>
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "与输入的密码不符")]
        public string ComparePassword { get; set; }

        /// <summary>
        /// 更改密码的凭证
        /// </summary>
        public string Token { get; set; }
    }
}
