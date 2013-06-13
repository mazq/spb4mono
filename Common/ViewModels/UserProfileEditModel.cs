//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;
using Tunynet.Utilities;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户表单呈现的UserProfile实体
    /// </summary>
    public class UserProfileEditModel
    {
        public UserProfileEditModel() { }
        public UserProfileEditModel(UserProfile userProfile, User user)
        {
            if (userProfile != null)
            {
                Birthday = userProfile.Birthday;
                BirthdayType = userProfile.BirthdayType;
                Email = !string.IsNullOrEmpty(userProfile.Email) ? userProfile.Email : user.AccountEmail;
                Gender = userProfile.Gender;
                HomeAreaCode = userProfile.HomeAreaCode;
                Introduction = userProfile.Introduction;
                Mobile = userProfile.Mobile;
                Msn = userProfile.Msn;
                NowAreaCode = userProfile.NowAreaCode;
                QQ = userProfile.QQ;
                UserId = userProfile.UserId;
            }
            if (user != null)
            {
                TrueName = user.TrueName;
                NickName = user.NickName;
                AccountEmail = user.AccountEmail;
                UserName = user.UserName;
                IsEmailVerified = user.IsEmailVerified;
            }
        }

        #region 属性

        #region 隐私设置

        /// <summary>
        /// Email隐私设置
        /// </summary>
        [Display(Name = "Email隐私设置")]
        public PrivacyStatus PrivacyEmail { get; set; }

        /// <summary>
        /// 手机号隐私设置
        /// </summary>
        [Display(Name = "手机号隐私设置")]
        public PrivacyStatus PrivacyMobile { get; set; }

        /// <summary>
        /// 生日隐私设置
        /// </summary>
        [Display(Name = "生日隐私设置")]
        public PrivacyStatus PrivacyBirthday { get; set; }

        /// <summary>
        /// QQ隐私设置
        /// </summary>
        [Display(Name = "QQ隐私设置")]
        public PrivacyStatus PrivacyQQ { get; set; }

        /// <summary>
        /// MSN隐私设置
        /// </summary>
        [Display(Name = "MSN隐私设置")]
        public PrivacyStatus PrivacyMSN { get; set; }

        #endregion

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        /// <summary>
        ///帐号邮箱
        /// </summary>
        [Display(Name = "帐号邮箱")]
        [Required(ErrorMessage = "帐号邮箱为必填选项")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "不合法的帐号邮箱")]
        public string AccountEmail { get; set; }

        /// <summary>
        ///姓名
        /// </summary>
        [Display(Name = "姓名")]
        [RegularExpression(@"^[\w|u4e00-\u9fa5]{1,64}$", ErrorMessage = "不允许使用的姓名")]
        [StringLength(30, ErrorMessage = "姓名最大长度允许30个字符")]
        [DataType(DataType.Text)]
        public string TrueName { get; set; }

        /// <summary>
        ///昵称
        /// </summary>
        [Display(Name = "昵称")]
        [Required(ErrorMessage = "请输入昵称")]
        [RegularExpression(@"^[\w|u4e00-\u9fa5]{1,64}$", ErrorMessage = "只能输入字母、数字、汉字和下划线")]
        [Remote("ValidateNickNameForEditUserProfile", "Account", ErrorMessage = "不合法的昵称")]
        [DataType(DataType.Text)]
        public string NickName { get; set; }

        /// <summary>
        ///性别
        /// </summary>
        [Display(Name = "性别")]
        [Required(ErrorMessage = "必须选择一个性别")]
        public GenderType? Gender { get; set; }

        /// <summary>
        ///生日类型
        /// </summary>
        [Display(Name = "生日类型")]
        public BirthdayType BirthdayType { get; set; }

        /// <summary>
        ///公历生日
        /// </summary>
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        ///农历年
        /// </summary>
        public int LunarYear { get; set; }

        /// <summary>
        ///农历月
        /// </summary>
        public int LunarMonth { get; set; }

        /// <summary>
        /// 农历日
        /// </summary>
        public int LunarDay { get; set; }

        /// <summary>
        ///所在地
        /// </summary>
        [Display(Name = "所在地")]
        public string NowAreaCode { get; set; }

        /// <summary>
        ///家乡
        /// </summary>
        [Display(Name = "家乡")]
        public string HomeAreaCode { get; set; }

        /// <summary>
        ///联系邮箱
        /// </summary>
        [Display(Name = "联系邮箱")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "不合法的联系邮箱")]
        public string Email { get; set; }

        /// <summary>
        ///手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        [RegularExpression("[0-9]{11}", ErrorMessage = "不合法的手机号码")]
        public string Mobile { get; set; }

        /// <summary>
        ///QQ
        /// </summary>
        [Display(Name = "QQ")]
        [StringLength(64, ErrorMessage = "QQ最大长度允许64个字符")]
        [RegularExpression("^[0-9]*[1-9][0-9]*$", ErrorMessage = "不合法的QQ号码")]
        public string QQ { get; set; }

        /// <summary>
        ///MSN
        /// </summary>
        [Display(Name = "MSN")]
        [StringLength(64, ErrorMessage = "MSN最大长度允许64个字符")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "不合法的MSN帐号")]
        public string Msn { get; set; }

        /// <summary>
        ///Skype
        /// </summary>
        [Display(Name = "Skype")]
        public string Skype { get; set; }

        /// <summary>
        ///飞信
        /// </summary>
        [Display(Name = "飞信")]
        public string Fetion { get; set; }

        /// <summary>
        ///阿里旺旺
        /// </summary>
        [Display(Name = "阿里旺旺")]
        public string Aliwangwang { get; set; }

        /// <summary>
        ///证件类型
        /// </summary>
        [Display(Name = "证件类型")]
        public CertificateType CardType { get; set; }

        /// <summary>
        ///证件号码
        /// </summary>
        [Display(Name = "证件号码")]
        public string CardID { get; set; }

        /// <summary>
        ///自我介绍
        /// </summary>
        [Display(Name = "自我介绍")]
        [StringLength(200, ErrorMessage = "自我介绍最大长度允许200个字符")]
        [DataType(DataType.MultilineText)]
        public string Introduction { get; set; }

        /// <summary>
        ///帐号邮箱是否通过验证
        /// </summary>
        public bool IsEmailVerified { get; set; }

        #endregion

        /// <summary>
        /// 转换为UserProfile用于数据库存储
        /// </summary>
        public UserProfile AsUserProfile(long userId)
        {
            UserProfile userProfile;
            if (UserId > 0)
            {
                UserProfileService userProfileService = new UserProfileService();
                userProfile = userProfileService.Get(UserId);
                userProfile.BirthdayType = this.BirthdayType;

                if (this.Gender == GenderType.FeMale)
                    userProfile.Gender = GenderType.FeMale;
                else if (this.Gender == GenderType.Male)
                    userProfile.Gender = GenderType.Male;

                else
                    userProfile.Gender = 0;

                if (this.BirthdayType == BirthdayType.LunarBirthday)
                {
                    ChinaDateTime cdt = new ChinaDateTime(LunarYear, LunarMonth, LunarDay);
                    userProfile.LunarBirthday = new DateTime(LunarYear, LunarMonth, LunarDay);
                    userProfile.Birthday = cdt.ToDateTime();

                }
                else
                {
                    if (Birthday != null)
                    {
                        ChinaDateTime cdt = new ChinaDateTime(Birthday.Value);
                        userProfile.Birthday = Birthday.Value;
                        userProfile.LunarBirthday = new DateTime(cdt.Year, cdt.Month, cdt.DayOfMonth);
                    }
                }

                if (!string.IsNullOrEmpty(Introduction))
                    userProfile.Introduction = this.Introduction;
                else
                    userProfile.Introduction = "";

                if (!string.IsNullOrEmpty(Email))
                    userProfile.Email = this.Email;
                else
                    userProfile.Email = "";

                if (!string.IsNullOrEmpty(Mobile))
                    userProfile.Mobile = this.Mobile;
                else
                    userProfile.Mobile = "";

                if (!string.IsNullOrEmpty(Msn))
                    userProfile.Msn = this.Msn;
                else
                    userProfile.Msn = "";

                if (!string.IsNullOrEmpty(NowAreaCode))
                    userProfile.NowAreaCode = this.NowAreaCode;
                else
                    userProfile.NowAreaCode = "";

                if (!string.IsNullOrEmpty(HomeAreaCode))
                    userProfile.HomeAreaCode = this.HomeAreaCode;
                else
                    userProfile.HomeAreaCode = "";

                if (!string.IsNullOrEmpty(QQ))
                    userProfile.QQ = this.QQ;
                else
                    userProfile.QQ = "";
            }
            else
            {
                userProfile = UserProfile.New();

                userProfile.BirthdayType = this.BirthdayType;

                if (this.Gender == GenderType.FeMale)
                    userProfile.Gender = GenderType.FeMale;
                else if (this.Gender == GenderType.Male)
                    userProfile.Gender = GenderType.FeMale;
                else
                    userProfile.Gender = 0;

                if (!string.IsNullOrEmpty(HomeAreaCode))
                    userProfile.HomeAreaCode = this.HomeAreaCode;
                else
                    userProfile.HomeAreaCode = "";

                if (!string.IsNullOrEmpty(NowAreaCode))
                    userProfile.NowAreaCode = this.NowAreaCode;
                else
                    userProfile.NowAreaCode = "";

                if (!string.IsNullOrEmpty(Email))
                    userProfile.Email = this.Email;
                else
                    userProfile.Email = "";

                if (!string.IsNullOrEmpty(Introduction))
                    userProfile.Introduction = this.Introduction;
                else
                    userProfile.Introduction = "";

                if (!string.IsNullOrEmpty(Mobile))
                    userProfile.Mobile = this.Mobile;
                else
                    userProfile.Mobile = "";

                if (!string.IsNullOrEmpty(Msn))
                    userProfile.Msn = this.Msn;
                else
                    userProfile.Msn = "";

                if (!string.IsNullOrEmpty(QQ))
                    userProfile.QQ = this.QQ;
                else
                    userProfile.QQ = "";

                if (Birthday != null)
                {
                    if (this.BirthdayType == BirthdayType.LunarBirthday)
                    {
                        ChinaDateTime cdt = new ChinaDateTime(LunarYear, LunarMonth, LunarDay);
                        userProfile.LunarBirthday = new DateTime(LunarYear, LunarMonth, LunarDay);
                        userProfile.Birthday = cdt.ToDateTime();

                    }
                    else
                    {
                        ChinaDateTime cdt = new ChinaDateTime(Birthday.Value);
                        userProfile.Birthday = Birthday.Value;
                        userProfile.LunarBirthday = new DateTime(cdt.Year, cdt.Month, cdt.DayOfMonth);
                    }
                }
                else
                {
                    ChinaDateTime cdt = new ChinaDateTime(DateTime.UtcNow);
                    userProfile.Birthday = DateTime.UtcNow;
                    userProfile.LunarBirthday = new DateTime(cdt.Year, cdt.Month, cdt.DayOfMonth);
                }

                userProfile.UserId = userId;

            }
            return userProfile;
        }

        /// <summary>
        /// 转换为User用于数据库存储
        /// </summary>
        public User AsUser(long userId)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();
            User user = userService.GetUser(userId) as User;

            if (!string.IsNullOrEmpty(TrueName))
                user.TrueName = this.TrueName;
            else
                user.TrueName = "";

            if (!string.IsNullOrEmpty(NickName))
                user.NickName = this.NickName;
            else
                user.NickName = "";

            return user;
        }

    }

}