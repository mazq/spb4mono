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
    /// 管理用户EditModel
    /// </summary>
    public class ManageUsersEditModel
    {
        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///用户名
        /// </summary>
        [StringLength(64, ErrorMessage = "用户名最大长度为64个字符!")]
        public string UserName { get; set; }

        /// <summary>
        ///昵称
        /// </summary>
        [Display(Name = "昵称")]
        [Required(ErrorMessage = "请输入昵称")]
        [Remote("ValidateNickName", "ControlPanelUser", AdditionalFields = "UserId", ErrorMessage = "不合法的昵称")]
        [DataType(DataType.Text)]
        public string NickName { get; set; }

        /// <summary>
        ///个人姓名 或 企业名称
        /// </summary>
        [Display(Name = "姓名")]
        [StringLength(30, ErrorMessage = "姓名最大长度为30个字符！")]
        [DataType(DataType.Text)]
        public string TrueName { get; set; }

        /// <summary>
        ///帐号邮箱
        /// </summary>
        [Display(Name = "帐号邮箱")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "邮箱为必填选项")]
        [StringLength(64, ErrorMessage = "邮箱最大长度为64个字符！")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "不合法的帐号邮箱")]
        public string AccountEmail { get; set; }

        /// <summary>
        ///手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[0-9]{11}", ErrorMessage = "不合法的手机号码")]
        public string AccountMobile { get; set; }

        /// <summary>
        ///角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        ///账户是否激活
        /// </summary>
        [Display(Name = "是否激活")]
        public bool IsActivated { get; set; }

        /// <summary>
        ///搜索注册起止时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///搜索注册截止时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///用户是否被监管
        /// </summary>
        [Display(Name = "是否管制")]
        public bool IsModerated { get; set; }

        /// <summary>
        ///强制用户管制（不会自动解除）
        /// </summary>
        [Display(Name = "永久管制")]
        public bool IsForceModerated { get; set; }

        /// <summary>
        /// 管制（0:否，1：管制，2：强制管制)
        /// </summary>
        [Display(Name = "是否管制")]
        public int Moderated { get; set; }

        /// <summary>
        ///是否封禁
        /// </summary>
        [Display(Name = "是否封禁")]
        public bool IsBanned { get; set; }

        /// <summary>
        /// 搜索用户等级起始
        /// </summary>
        public int RankStart { get; set; }

        /// <summary>
        /// 搜索用户等级截止
        /// </summary>
        public int RankEnd { get; set; }

        /// <summary>
        ///帐号邮箱是否通过验证
        /// </summary>
        [Display(Name = "通过邮箱验证")]
        public bool IsEmailVerified { get; set; }

        /// <summary>
        ///是否强制用户登录
        /// </summary>
        [Display(Name = "强制重新登录")]
        public bool ForceLogin { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        ///封禁原因
        /// </summary>
        [Display(Name = "封禁原因")]
        [Required(ErrorMessage = "请输入封禁原因")]
        public string BanReason { get; set; }

        /// <summary>
        ///封禁截止日期
        /// </summary>
        [Display(Name = "封禁天数")]
        public long BanDays { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        [Display(Name = "截止日期")]
        [Required(ErrorMessage = "请选择截止日期")]
        public DateTime BanDeadline { get; set; }

        [Display(Name = "手机号码")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[0-9]{11}", ErrorMessage = "不合法的手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        /// 转换为UserQuery用于查询
        /// </summary>
        public UserQuery AsUserQuery()
        {
            UserQuery userQuery = new UserQuery();

            userQuery.AccountEmailFilter = AccountEmail;
            userQuery.IsActivated = IsActivated;
            userQuery.IsBanned = IsBanned;
            userQuery.IsModerated = IsModerated;
            userQuery.Keyword = UserName;
            userQuery.RoleName = RoleName;

            return userQuery;
        }

        /// <summary>
        /// 转换为User用于数据库存储
        /// </summary>
        public User AsUser()
        {
            User user = User.New();
            user.AccountEmail = this.AccountEmail;
            user.UserName = this.UserName;

            return user;
        }

        /// <summary>
        /// 转换为User用于数据库存储
        /// </summary>
        public User AsUserForEditUser()
        {
            UserService userService = new UserService();
            User user = userService.GetFullUser(UserId) as User;
            user.AccountEmail = this.AccountEmail;
            if (!string.IsNullOrEmpty(AccountMobile))
                user.AccountMobile = this.AccountMobile;
            else
                user.AccountMobile = "";
            if (!string.IsNullOrEmpty(NickName))
                user.NickName = this.NickName;
            else
                user.NickName = "";
            if (!string.IsNullOrEmpty(TrueName))
                user.TrueName = this.TrueName;
            else
                user.TrueName = "";
            user.ForceLogin = this.ForceLogin;
            user.IsActivated = this.IsActivated;
            user.IsModerated = this.Moderated == 0 ? false : true;
            user.IsForceModerated = this.Moderated == 2 ? true : false;
            user.IsBanned = this.IsBanned;
            user.IsEmailVerified = this.IsEmailVerified;
            if (this.IsBanned)
            {
                user.BanReason = this.BanReason ?? string.Empty;
                user.BanDeadline = this.BanDeadline;
            }
            else
            {
                user.BanReason = "";
                user.BanDeadline = DateTime.UtcNow;
            }
            return user;
        }
    }

    /// <summary>
    /// User扩展
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// 转换成ManageUsersEditModel
        /// </summary>
        /// <param name="user"></param>
        public static ManageUsersEditModel AsEditModel(this User user)
        {
            int moderated = 0;
            if (user.IsForceModerated)
            {
                moderated = 2;
            }
            else if (user.IsModerated)
            {
                moderated = 1;
            }
            else
            {
                moderated = 0;
            }
            return new ManageUsersEditModel
            {
                UserId = user.UserId,

                UserName = user.UserName,
                NickName = user.NickName,
                AccountEmail = user.AccountEmail,
                AccountMobile = user.AccountMobile,
                Mobile = user.Profile.Mobile,
                TrueName = user.TrueName,
                IsActivated = user.IsActivated,
                Moderated = moderated,
                IsBanned = user.IsBanned,
                ForceLogin = user.ForceLogin,
                IsEmailVerified = user.IsEmailVerified,
                BanDays = user.IsBanned ? (user.BanDeadline - DateTime.Now).Days + 1 : 0,
                BanReason = user.IsBanned ? user.BanReason : "",
                BanDeadline = user.BanDeadline
            };
        }
    }
}
