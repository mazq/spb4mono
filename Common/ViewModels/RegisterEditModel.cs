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
using Tunynet.Utilities;
using System.ComponentModel;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 注册时候的信息载体类
    /// </summary>
    public class RegisterEditModel
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public RegisterEditModel()
        {
            AcceptableProvision = true;
        }

        /// <summary>
        /// 回跳网页
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 邀请凭证
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [Remote("ValidateUserName", "Account", ErrorMessage = "不合法的用户名")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Display(Name = "昵称")]
        [Required(ErrorMessage = "请输入昵称")]
        [Remote("ValidateNickName", "Account", ErrorMessage = "不合法的昵称")]
        [DataType(DataType.Text)]
        public string NickName { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        [Display(Name = "密码")]
        [StringLength(18, ErrorMessage = "密码最多允许输入18个字符")]
        [Remote("ValidatePassword", "Account", ErrorMessage = "不合法的密码")]
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "确认密码与密码不符")]//设置确认密码是否与密码相同
        public string ConfirmPassword { get; set; }


        /// <summary>
        ///帐号邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "请输入邮箱")]
        [StringLength(50, ErrorMessage = "邮箱最多允许输入50个字符")]
        [Remote("ValidateEmail", "Account", ErrorMessage = "不合法的帐号邮箱")]
        public string AccountEmail { get; set; }


        /// <summary>
        /// 接受条款
        /// </summary>
        [Display(Name = "我已看过并完全同意")]
        [IsTrue(ErrorMessage = "请同意注册条款")]
        public bool AcceptableProvision { get; set; }

        /// <summary>
        /// 转换成User
        /// </summary>
        /// <returns></returns>
        public User AsUser()
        {
            User user = User.New();
            user.UserName = UserName;
            user.Password = Password;
            user.AccountEmail = AccountEmail;

            user.NickName = NickName;

            user.Rank = 1;
            return user;
        }
    }

    /// <summary>
    /// User的扩展类
    /// </summary>
    public static class RegisterUserExtensions
    {
        /// <summary>
        /// 将User转换成RegisterUserEditModel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static RegisterEditModel AsRegisterUserEditModel(this User user)
        {
            return new RegisterEditModel
            {
                AccountEmail = user.AccountEmail,
                Password = user.Password,
                UserName = user.UserName,
                NickName = user.NickName
            };
        }
    }

    ///// <summary>
    ///// 验证用户名的专用类
    ///// </summary>
    //public class RegisterValidation
    //{
    //    /// <summary>
    //    /// 验证用户名
    //    /// </summary>
    //    /// <param name="userName">用户名</param>
    //    /// <returns>是否通过验证</returns>
    //    public static ValidationResult ValidateUserName(string userName)
    //    {
    //        string errorMessage;
    //        bool isValid = Utility.ValidateUserName(userName, out errorMessage);

    //        if (isValid)
    //        {
    //            return ValidationResult.Success;
    //        }
    //        else
    //        {
    //            return new ValidationResult(errorMessage);
    //        }
    //    }

    //    /// <summary>
    //    /// 验证密码
    //    /// </summary>
    //    /// <param name="password">密码</param>
    //    /// <returns>是否通过验证</returns>
    //    public static ValidationResult ValidatePassword(string password)
    //    {
    //        string errorMessage;
    //        bool isValid = Utility.ValidatePassword(password, out errorMessage);

    //        if (isValid)
    //        {
    //            return ValidationResult.Success;
    //        }
    //        else
    //        {
    //            return new ValidationResult(errorMessage);
    //        }
    //    }

    //    /// <summary>
    //    /// 雁阵邮箱
    //    /// </summary>
    //    /// <param name="accountEmail">邮箱</param>
    //    /// <returns>是否通过验证</returns>
    //    public static ValidationResult ValidateEmail(string accountEmail)
    //    {
    //        string errorMessage;
    //        bool isValid = Utility.ValidateEmail(accountEmail, out errorMessage);

    //        if (isValid)
    //        {
    //            return ValidationResult.Success;
    //        }
    //        else
    //        {
    //            return new ValidationResult(WebUtility.HtmlDecode(errorMessage));
    //        }
    //    }
    //}

}
