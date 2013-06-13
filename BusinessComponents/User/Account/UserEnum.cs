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

namespace Tunynet.Common
{
    /// <summary>
    /// 注册方式
    /// </summary>
    public enum RegistrationMode
    {
        /// <summary>
        /// 允许所有途径的注册
        /// </summary>
        [Display(Name = "允许注册")]
        All = 1,

        /// <summary>
        /// 仅允许通过邀请注册
        /// </summary>
        [Display(Name = "仅邀请注册")]
        Invitation = 2,

        /// <summary>
        /// 禁止注册
        /// </summary>
        [Display(Name = "禁止注册")]
        Disabled = 4
    }

    /// <summary>
    /// 帐号激活方式
    /// </summary>
    public enum AccountActivation
    {
        /// <summary>
        /// 用户注册时自动激活
        /// </summary>
        [Display(Name = "自动激活")]
        Automatic = 0,

        /// <summary>
        /// 通过验证Email激活
        /// </summary>
        [Display(Name = "Email激活")]
        Email = 1,

        /// <summary>
        /// 通过手机短信激活
        /// </summary>
        [Display(Name = "短信激活")]
        SMS = 2,

        /// <summary>
        /// 管理员激活
        /// </summary>
        [Display(Name = "管理员激活")]
        Administrator = 9
    }

    /// <summary>
    /// 用户密码存储格式
    /// </summary>
    public enum UserPasswordFormat
    {
        /// <summary>
        /// 密码未加密
        /// </summary>
        [Display(Name = "不加密")]
        Clear = 0,

        /// <summary>
        /// 标准MD5加密
        /// </summary>
        [Display(Name = "MD5加密")]
        MD5 = 1,
    }

    /// <summary>
    /// 用什么名称作为用户的DisplayName对外显示
    /// </summary>
    public enum DisplayNameType
    {
        /// <summary>
        /// 首先采用昵称作为DisplayName，如果昵称不存在则用真实姓名作为DisplayName，如果真实姓名也不存在则用UserName作为DisplayName
        /// </summary>
        NicknameFirst = 1,

        /// <summary>
        /// 首先采用真实姓名作为DisplayName，如果真实姓名不存在则用昵称作为DisplayName，如果昵称也不存在则用UserName作为DisplayName
        /// </summary>
        TrueNameFirst = 2
    }


    /// <summary>    
    /// 用于创建用户账号时的返回值
    /// </summary>
    public enum UserCreateStatus
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownFailure = 0,

        /// <summary>
        /// 创建成功
        /// </summary>
        Created = 1,

        /// <summary>
        /// 用户名重复
        /// </summary>
        DuplicateUsername = 2,

        /// <summary>
        /// Email重复
        /// </summary>
        DuplicateEmailAddress = 3,

        /// <summary>
        /// 手机号重复
        /// </summary>
        DuplicateMobile = 4,


        /// <summary>
        /// 不允许的用户名
        /// </summary>
        DisallowedUsername = 5,

        ///// <summary>
        ///// 更新成功
        ///// </summary>
        //Updated = 6,

        /// <summary>
        /// 不合法的密码提示问题/答案
        /// </summary>
        InvalidQuestionAnswer = 7,

        /// <summary>
        /// 不合法的密码
        /// </summary>
        InvalidPassword = 8
    }


    /// <summary>
    /// 删除用户时的返回状态
    /// </summary>
    public enum UserDeleteStatus
    {
        /// <summary>
        /// 删除成功
        /// </summary>
        Deleted = 1,

        /// <summary>
        /// 接管被删除用户内容的用户名不存在
        /// </summary>
        InvalidTakeOverUsername = 2,

        /// <summary>
        /// 待删除的用户不存在
        /// </summary>
        DeletingUserNotFound = 3,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownFailure = 10
    }


    /// <summary>
    /// 用户登录状态
    /// </summary>
    public enum UserLoginStatus
    {
        /// <summary>
        /// 通过身份验证，登录成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 用户名、密码不匹配
        /// </summary>
        InvalidCredentials = 1,

        /// <summary>
        /// 帐户未激活
        /// </summary>
        NotActivated = 2,

        /// <summary>
        /// 帐户被封禁
        /// </summary>
        Banned = 3,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = 100
    }

}
