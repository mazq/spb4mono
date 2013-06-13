//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;
using System;
using System.Web.Mvc;
using System.ComponentModel;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 第三方帐号注册实体
    /// </summary>
    public class ThirdRegisterEditModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
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
        /// 邮箱信息
        /// </summary>
        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "请输入邮箱")]
        [StringLength(50, ErrorMessage = "邮箱最多允许输入50个字符")]
        [Remote("ValidateEmail", "Account", ErrorMessage = "不合法的帐号邮箱")]
        public string AccountEmail { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [Display(Name = "密码")]
        [StringLength(18, ErrorMessage = "密码最多允许输入18个字符")]
        [Remote("ValidatePassword", "Account", ErrorMessage = "不合法的密码")]
        public string PassWord { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "请输入确认密码")]
        [Display(Name = "确认密码")]
        [Compare("PassWord", ErrorMessage = "确认密码与密码不符")]//设置确认密码是否与密码相同
        public string ConfirmPassword { get; set; }


        /// <summary>
        /// 是否关注官方账户
        /// </summary>
        public bool FollowOfficial { get; set; }

        /// <summary>
        /// 分享给朋友
        /// </summary>
        public bool ShareToFirend { get; set; }

        /// <summary>
        /// 清除掉实体中没有必要的内容。
        /// </summary>
        /// <returns></returns>
        internal ThirdRegisterEditModel CleanUp()
        {
            this.PassWord = string.Empty;
            this.ConfirmPassword = string.Empty;
            return this;
        }

        /// <summary>
        /// 转换成数据保存的User对象
        /// </summary>
        /// <returns></returns>
        internal User AsUser()
        {
            User user = User.New();

            user.UserId = IdGenerator.Next();
            user.UserName = this.UserName;
            user.Password = this.PassWord;
            user.AccountEmail = this.AccountEmail;

            return user;
        }
    }
}