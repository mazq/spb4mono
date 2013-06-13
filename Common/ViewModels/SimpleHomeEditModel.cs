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

namespace Spacebuilder.Common
{
    public class SimpleHomeEditModel : LoginEditModel
    {
        /// <summary>
        /// 是否属于模式窗口登录
        /// </summary>
        public bool loginInModal { get; set; }

        /// <summary>
        /// 回跳网页
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "请输入帐号或邮箱")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        /// <summary>
        /// 是否记得密码
        /// </summary>
        public bool RememberPassword { get; set; }

        /// <summary>
        /// 将EditModel转换成User
        /// </summary>
        /// <returns></returns>
        public User AsUser()
        {
            User user = User.New();
            user.UserName = UserName;
            user.Password = Password;
            return user;
        }
    }
}
