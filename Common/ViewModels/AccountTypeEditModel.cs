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
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 帐号类型
    /// </summary>
    public class AccountTypeEditModel
    {

        #region 需持久化属性
        /// <summary>
        /// 第三方帐号类型标识
        /// </summary>
        public string AccountTypeKey { set; get; }

        /// <summary>
        /// 网站接入应用标识
        /// </summary>
        [Display(Name = "AppKey")]
        [StringLength(50, ErrorMessage = "最多输入50个字符")]
        public string AppKey { set; get; }

        /// <summary>
        /// 网站接入应用加密串
        /// </summary>
        [Display(Name = "AppSecret")]
        [StringLength(100, ErrorMessage = "最多输入100个字符")]
        public string AppSecret { set; get; }

        /// <summary>
        /// 是否关注指定微博
        /// </summary>
        [Display(Name = "同步微博")]
        public bool IsSync { set; get; }


        /// <summary>
        /// 绑定成功时是否分享一条微博
        /// </summary>
        [Display(Name = "分享微博")]
        public bool IsShareMicroBlog { set; get; }

        /// <summary>
        /// 是否关注指定微博
        /// </summary>
        [Display(Name = "关注指定微博")]
        public bool IsFollowMicroBlog { set; get; }

        /// <summary>
        /// 指定微博帐号
        /// </summary>
        [Required(ErrorMessage = "请输入微博帐号")]
        [StringLength(64, ErrorMessage = "最多输入64个字符")]
        public string OfficialMicroBlogAccount { set; get; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool IsEnabled { set; get; }

        #endregion

        /// <summary>
        /// 将EditModel转为数据库实体
        /// </summary>
        /// <returns></returns>
        public AccountType AsAccountType()
        {

            AccountBindingService service = new AccountBindingService();
            AccountType accountType = service.GetAccountType(AccountTypeKey);

            accountType.AppKey = AppKey ?? string.Empty;
            accountType.AppSecret = AppSecret ?? string.Empty;
            accountType.IsSync = IsSync;
            accountType.IsShareMicroBlog = IsShareMicroBlog;
            accountType.IsFollowMicroBlog = IsFollowMicroBlog;
            accountType.OfficialMicroBlogAccount = OfficialMicroBlogAccount ?? string.Empty;
            accountType.IsEnabled = IsEnabled;

            return accountType;
        }
    }

    /// <summary>
    /// AccountType扩展
    /// </summary>
    public static class AccountTypeExtensions
    {
        /// <summary>
        /// 将数据库实体转换为EditModel
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public static AccountTypeEditModel AsEditModel(this AccountType accountType)
        {
            return new AccountTypeEditModel
            {
                AccountTypeKey = accountType.AccountTypeKey,
                AppKey = accountType.AppKey,
                AppSecret = accountType.AppSecret,
                IsSync = accountType.IsSync,
                IsShareMicroBlog = accountType.IsShareMicroBlog,
                IsFollowMicroBlog = accountType.IsFollowMicroBlog,
                OfficialMicroBlogAccount = accountType.OfficialMicroBlogAccount,
                IsEnabled = accountType.IsEnabled
            };
        }
    }

}
