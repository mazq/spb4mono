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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 通过邮箱激活帐号
    /// </summary>
    public class ActivateByEmail
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "请输入邮箱")]
        public string AccountEmail { get; set; }

        /// <summary>
        /// 登陆凭证
        /// </summary>
        public string Token { get; set; }
    }
}
