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
using Spacebuilder.Common.Configuration;
using Tunynet.Common.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户设置
    /// </summary>
    public class UserSettingsEditModel
    {
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public UserSettingsEditModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userProfileSettings">用户资料设置</param>
        /// <param name="userSettings">用户设置</param>
        /// <param name="inviteFriendSettings">邀请朋友设置</param>
        public UserSettingsEditModel(UserProfileSettings userProfileSettings, UserSettings userSettings, InviteFriendSettings inviteFriendSettings)
        {
            if (userSettings != null)
            {
                RegistrationMode = userSettings.RegistrationMode;
                AccountActivation = userSettings.AccountActivation;
                EnableNotActivatedUsersToLogin = userSettings.EnableNotActivatedUsersToLogin;
                EnableTrackAnonymous = userSettings.EnableTrackAnonymous;
                UserOnlineTimeWindow = userSettings.UserOnlineTimeWindow;
                UserPasswordFormat = userSettings.UserPasswordFormat;
                AutomaticModerated = userSettings.AutomaticModerated;
                NoModeratedUserPoint = userSettings.NoModeratedUserPoint;
                EnableNickname = userSettings.EnableNickname;
                DisplayNameType = userSettings.DisplayNameType;
                DisallowedUserNames = userSettings.DisallowedUserNames;
                MyHomePageAsSiteEntry = userSettings.MyHomePageAsSiteEntry;
            }
            if (userProfileSettings != null)
            {
                Avatar = userProfileSettings.IntegrityProportions[0];
                Birthday = userProfileSettings.IntegrityProportions[1];
                NowArea = userProfileSettings.IntegrityProportions[2];
                HomeArea = userProfileSettings.IntegrityProportions[3];
                IM = userProfileSettings.IntegrityProportions[4];
                Mobile = userProfileSettings.IntegrityProportions[5];
                EducationExperience = userProfileSettings.IntegrityProportions[6];
                WorkExperience = userProfileSettings.IntegrityProportions[7];
                Introduction = userProfileSettings.IntegrityProportions[8];
                MaxPersonTag = userProfileSettings.MaxPersonTag;
                MinIntegrity = userProfileSettings.MinIntegrity;
            }
            if (inviteFriendSettings != null)
            {
                AllowInvitationCodeUseOnce = inviteFriendSettings.AllowInvitationCodeUseOnce;
                InvitationCodeTimeLiness = inviteFriendSettings.InvitationCodeTimeLiness;
                InvitationCodeUnitPrice = inviteFriendSettings.InvitationCodeUnitPrice;
                DefaultUserInvitationCodeCount = inviteFriendSettings.DefaultUserInvitationCodeCount;
            }
        }

        #region 需持久化属性

        #region 用户资料设置

        /// <summary>
        /// 头像
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? Birthday { get; set; }

        /// <summary>
        /// 所在地
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? NowArea { get; set; }

        /// <summary>
        /// 家乡
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? HomeArea { get; set; }

        /// <summary>
        /// 即时通讯帐号
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? IM { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? Mobile { get; set; }

        /// <summary>
        /// 教育经历
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? EducationExperience { get; set; }

        /// <summary>
        /// 工作经历
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? WorkExperience { get; set; }

        /// <summary>
        /// 自我介绍
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? Introduction { get; set; }

        #endregion

        #region 邀请好友设置

        /// <summary>
        /// 邀请码是否仅允许使用一次(仅在注册选项为邀请注册的时候才允许管理员修改，其他时候。都修改false)
        /// </summary>
        public bool AllowInvitationCodeUseOnce { get; set; }

        /// <summary>
        /// 邀请码有效期（单位：天）
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int InvitationCodeTimeLiness { get; set; }

        /// <summary>
        /// 设置购买邀请码所需的交易积分
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int InvitationCodeUnitPrice { get; set; }

        /// <summary>
        /// 默认用户邀请码配额
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int DefaultUserInvitationCodeCount { get; set; }

        #endregion

        #region 用户设置

        /// <summary>
        /// 我的首页作为站点入口
        /// </summary>
        /// <remarks>
        /// 登录成功后是否跳转到我的首页
        /// </remarks>
        public bool MyHomePageAsSiteEntry { get; set; }

        /// <summary>
        /// 用户注册方式设置
        /// </summary>
        public RegistrationMode RegistrationMode { get; set; }

        /// <summary>
        /// 账户激活方法
        /// </summary>
        public AccountActivation AccountActivation { get; set; }

        /// <summary>
        /// 允许未激活的用户登录
        /// </summary>
        public bool EnableNotActivatedUsersToLogin { get; set; }

        /// <summary>
        /// 是否启用匿名用户跟踪
        /// </summary>
        public bool EnableTrackAnonymous { get; set; }

        /// <summary>
        /// 指定用户在最近一次活动时间之后多长时间视为在线的分钟数
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? UserOnlineTimeWindow { get; set; }

        /// <summary>
        /// 用户密码加密方式
        /// </summary>
        public UserPasswordFormat UserPasswordFormat { get; set; }

        /// <summary>
        /// 新注册用户是否自动处于管制状态
        /// </summary>
        public bool AutomaticModerated { get; set; }

        ///	<summary>
        ///	用户自动接触管制状态所需的积分（用户综合积分）
        ///	</summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? NoModeratedUserPoint { get; set; }

        /// <summary>
        /// 是否启用昵称
        /// </summary>
        public bool EnableNickname { get; set; }

        /// <summary>
        /// 用户对外显示哪个名称（如果未启用昵称，则该选项无需设置）
        /// </summary>
        public DisplayNameType DisplayNameType { get; set; }

        /// <summary>
        /// 不允许使用的用户名
        /// </summary>
        /// <remarks>
        /// 多个用户名之间用逗号分割
        /// </remarks>
        [RegularExpression("^([\\w\\d\u4e00-\u9fa5](,|，)?)+$", ErrorMessage = "各名称之间必须以逗号分隔")]
        public string DisallowedUserNames { get; set; }

        /// <summary>
        /// 最小资料完整度
        /// </summary>
        [Range(-100, 100, ErrorMessage = "请输入-100至100之间的数")]
        public int? MinIntegrity { get; set; }

        /// <summary>
        /// 最多可以贴的标签数
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int? MaxPersonTag { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// 转换为userProfileSettings用于数据库存储
        /// </summary>
        public UserProfileSettings AsUserProfileSettings()
        {
            UserProfileSettings userProfileSettings = DIContainer.Resolve<IUserProfileSettingsManager>().GetUserProfileSettings();
            userProfileSettings.IntegrityProportions[0] = Avatar ?? 20;
            userProfileSettings.IntegrityProportions[1] = Birthday ?? 10;
            userProfileSettings.IntegrityProportions[2] = NowArea ?? 10;
            userProfileSettings.IntegrityProportions[3] = HomeArea ?? 10;
            userProfileSettings.IntegrityProportions[4] = IM ?? 10;
            userProfileSettings.IntegrityProportions[5] = Mobile ?? 0;
            userProfileSettings.IntegrityProportions[6] = EducationExperience ?? 15;
            userProfileSettings.IntegrityProportions[7] = WorkExperience ?? 15;
            userProfileSettings.IntegrityProportions[8] = Introduction ?? 10;
            userProfileSettings.MinIntegrity = MinIntegrity ?? 50;
            userProfileSettings.MaxPersonTag = MaxPersonTag ?? 10;
            return userProfileSettings;
        }

        /// <summary>
        /// 转换为UserSettings用于数据库存储
        /// </summary>
        public UserSettings AsUserSettings()
        {
            UserSettings userSettings = DIContainer.Resolve<IUserSettingsManager>().Get();
            userSettings.RegistrationMode = RegistrationMode;
            userSettings.AccountActivation = AccountActivation;
            userSettings.EnableNotActivatedUsersToLogin = EnableNotActivatedUsersToLogin;
            userSettings.EnableTrackAnonymous = EnableTrackAnonymous;
            userSettings.UserOnlineTimeWindow = UserOnlineTimeWindow ?? 0;
            userSettings.UserPasswordFormat = UserPasswordFormat;
            userSettings.AutomaticModerated = AutomaticModerated;
            userSettings.NoModeratedUserPoint = NoModeratedUserPoint ?? 0;
            userSettings.EnableNickname = EnableNickname;
            userSettings.DisplayNameType = DisplayNameType;
            userSettings.DisallowedUserNames = DisallowedUserNames ?? string.Empty;
            userSettings.MyHomePageAsSiteEntry = MyHomePageAsSiteEntry;
            return userSettings;
        }

        /// <summary>
        /// 转换为InviteFriendSettings用于数据库存储
        /// </summary>
        /// <returns></returns>
        public InviteFriendSettings AsInviteFriendSettings()
        {
            InviteFriendSettings inviteFriendSettings = DIContainer.Resolve<IInviteFriendSettingsManager>().Get();
            inviteFriendSettings.AllowInvitationCodeUseOnce = AllowInvitationCodeUseOnce;
            inviteFriendSettings.InvitationCodeTimeLiness = InvitationCodeTimeLiness;
            inviteFriendSettings.InvitationCodeUnitPrice = InvitationCodeUnitPrice;
            inviteFriendSettings.DefaultUserInvitationCodeCount = DefaultUserInvitationCodeCount;
            return inviteFriendSettings;
        }
    }

}