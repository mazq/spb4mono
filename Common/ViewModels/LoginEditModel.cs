//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Tunynet.Mvc;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于用户登录的view与action中传递参数的实体
    /// </summary>
    public class LoginEditModel
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
        [WaterMark(Content = "密码")]
        [Display(Name = "密码")]
        [Required(ErrorMessage = "请输入密码")]
        [NoFilterWord]
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [WaterMark(Content = "用户名或者Email")]
        [Display(Name = "帐号")]
        [Required(ErrorMessage = "请输入帐号或邮箱")]
        [DataType(DataType.Text)]
        [NoFilterWord]
        public string UserName { get; set; }

        /// <summary>
        /// 是否记得密码
        /// </summary>
        [Display(Name = "下次自动登录")]
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

    /// <summary>
    /// User实体的扩展方法
    /// </summary>
    public static class LogonExtensions
    {
        /// <summary>
        /// 转换为EditModel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static LoginEditModel AsLogonEditModel(this User user)
        {
            return new LoginEditModel()
            {
                UserName = user.UserName
            };
        }
    }
}
