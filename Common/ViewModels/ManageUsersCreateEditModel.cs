//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 后台创建用户EditModel
    /// </summary>
    public class ManageUsersCreateEditModel
    {
        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///用户名
        /// </summary>
        [Display(Name = "用户名")]
        [StringLength(64, ErrorMessage = "用户名最大长度为64个字符")]
        [Required(ErrorMessage = "用户名为必填选项")]
        [Remote("ValidateUserName", "Account", ErrorMessage = "不合法的用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [Required(ErrorMessage = "请输入昵称")]
        [Remote("ValidateNickName", "Account", ErrorMessage = "不合法的昵称")]
        [DataType(DataType.Text)]
        public string NickName { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码为必填选项")]
        [Remote("ValidatePassword", "Account", ErrorMessage = "不合法的密码")]
        public string Password { get; set; }

        /// <summary>
        ///确认密码
        /// </summary>
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "确认密码与密码不符")]
        public string PassAgain { get; set; }

        /// <summary>
        ///帐号邮箱
        /// </summary>
        [Display(Name = "帐号邮箱")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "邮箱为必填选项")]
        [Remote("ValidateEmail", "Account", ErrorMessage = "不合法的帐号邮箱")]
        [StringLength(64, ErrorMessage = "邮箱最大长度为64个字符")]
        public string AccountEmail { get; set; }


        /// <summary>
        /// 转换为User用于数据库存储
        /// </summary>
        public User AsUser()
        {
            User user = User.New();
            user.AccountEmail = this.AccountEmail;
            user.UserName = this.UserName;
            user.NickName = this.NickName;

            return user;
        }

        /// <summary>
        /// 转换为User用于数据库存储
        /// </summary>
        public User AsUserForEditUser()
        {
            UserService userService = new UserService();
            User user = userService.GetUser(UserId) as User;
            user.AccountEmail = this.AccountEmail;
            user.UserName = this.UserName;
            user.NickName = this.NickName;

            return user;
        }
    }

}
