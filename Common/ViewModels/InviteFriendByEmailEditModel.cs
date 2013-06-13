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
    /// 邀请好友的时候用于用户名和帐号的提交
    /// </summary>
    public class InviteFriendByEmailEditMode
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名为必填项")]
        public string userName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码为必填项")]
        public string password { get; set; }

        /// <summary>
        /// Email的后缀名
        /// </summary>
        [Display(Name = "邮箱尾缀")]
        [Required(ErrorMessage = "邮箱尾坠不能为空")]
        public string emailDomainName { get; set; }
    }
}
