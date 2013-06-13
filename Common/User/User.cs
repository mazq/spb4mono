//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Linq;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using System.Collections.Generic;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户帐号
    /// </summary>
    [TableName("tn_Users")]
    [PrimaryKey("UserId", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class User : IUser, IEntity
    {
        #region 构造器

        /// <summary>
        /// 构造器
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="username">用户名称</param>
        /// <param name="accountEmail">账户Email</param>
        /// <param name="accountMobile">账户手机号</param>
        public static User New(long userId, string username, string accountEmail, string accountMobile)
        {
            User user = New();
            user.UserId = userId;
            user.UserName = username;
            user.AccountEmail = accountEmail;
            user.AccountMobile = accountMobile;
            user.Avatar = "avatar_default";
            user.Rank = 1;
            return user;
        }

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static User New()
        {
            User user = new User();
            user.UserName = string.Empty;
            user.Password = string.Empty;
            user.PasswordQuestion = string.Empty;
            user.PasswordAnswer = string.Empty;
            user.AccountEmail = string.Empty;
            user.AccountMobile = string.Empty;
            user.TrueName = string.Empty;
            user.NickName = string.Empty;
            user.DateCreated = DateTime.UtcNow;
            user.IpCreated = string.Empty;
            user.LastActivityTime = DateTime.UtcNow;
            user.LastAction = string.Empty;
            user.IpLastActivity = string.Empty;
            user.BanReason = string.Empty;
            user.BanDeadline = DateTime.UtcNow;
            user.ThemeAppearance = string.Empty;
            user.FollowedCount = 0;
            user.FollowerCount = 0;
            user.Avatar = "avatar_default";
            user.Rank = 1;
            return user;
        }

        #endregion 构造器

        #region 需持久化属性

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///0=Clear（明文）1=标准MD5
        /// </summary>
        public int PasswordFormat { get; set; }

        /// <summary>
        ///密码问题
        /// </summary>
        public string PasswordQuestion { get; set; }

        /// <summary>
        ///密码答案
        /// </summary>
        public string PasswordAnswer { get; set; }

        /// <summary>
        ///帐号邮箱
        /// </summary>
        public string AccountEmail { get; set; }

        /// <summary>
        ///帐号邮箱是否通过验证
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        ///手机号码
        /// </summary>
        public string AccountMobile { get; set; }

        /// <summary>
        ///帐号手机是否通过验证
        /// </summary>
        public bool IsMobileVerified { get; set; }

        /// <summary>
        ///个人姓名 或 企业名称
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        ///昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        ///是否强制用户登录
        /// </summary>
        public bool ForceLogin { get; set; }

        /// <summary>
        ///账户是否激活
        /// </summary>
        public bool IsActivated { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///创建用户时的ip
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public string IpCreated { get; set; }

        /// <summary>
        ///用户类别
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        ///上次活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }

        /// <summary>
        ///上次操作
        /// </summary>
        public string LastAction { get; set; }

        /// <summary>
        ///上次活动时的ip
        /// </summary>
        public string IpLastActivity { get; set; }

        /// <summary>
        ///是否封禁
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        ///封禁原因
        /// </summary>
        public string BanReason { get; set; }

        /// <summary>
        ///封禁截止日期
        /// </summary>
        public DateTime BanDeadline { get; set; }

        /// <summary>
        ///用户是否被监管
        /// </summary>
        public bool IsModerated { get; set; }

        /// <summary>
        ///强制用户管制（不会自动解除）
        /// </summary>
        public bool IsForceModerated { get; set; }

        /// <summary>
        /// 头像(存储相对路径)
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public string Avatar { get; set; }

        /// <summary>
        ///磁盘配额
        /// </summary>
        public int DatabaseQuota { get; set; }

        /// <summary>
        ///已用磁盘空间
        /// </summary>
        public int DatabaseQuotaUsed { get; set; }

        /// <summary>
        ///用户选择的皮肤
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public string ThemeAppearance { get; set; }

        /// <summary>
        /// 是否使用了自定义风格
        /// </summary>
        public bool IsUseCustomStyle { get; set; }

        /// <summary>
        /// 关注用户数
        /// </summary>
        public int FollowedCount { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FollowerCount { get; set; }

        /// <summary>
        /// 经验积分值
        /// </summary>
        public int ExperiencePoints { get; set; }

        /// <summary>
        /// 威望积分值
        /// </summary>
        public int ReputationPoints { get; set; }

        /// <summary>
        /// 交易积分值
        /// </summary>
        public int TradePoints { get; set; }

        /// <summary>
        /// 交易积分值2
        /// </summary>
        public int TradePoints2 { get; set; }

        /// <summary>
        /// 交易积分值3
        /// </summary>
        public int TradePoints3 { get; set; }

        /// <summary>
        /// 交易积分值4
        /// </summary>
        public int TradePoints4 { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public int Rank { get; set; }

        /// <summary>
        /// 冻结的交易积分
        /// </summary>
        public int FrozenTradePoints { get; set; }

        #endregion


        #region 显示实现接口

        long IUser.UserId
        {
            get { return UserId; }
        }

        string IUser.UserName
        {
            get { return UserName; }
        }

        int IUser.UserType
        {
            get { return UserType; }
        }

        string IUser.AccountEmail
        {
            get { return AccountEmail; }
        }

        bool IUser.IsEmailVerified
        {
            get { return IsEmailVerified; }
        }

        string IUser.AccountMobile
        {
            get { return AccountMobile; }
        }

        bool IUser.IsMobileVerified
        {
            get { return IsMobileVerified; }
        }

        string IUser.TrueName
        {
            get { return TrueName; }
        }

        string IUser.NickName
        {
            get { return NickName; }
        }

        string IUser.DisplayName
        {
            get { return DisplayName; }
        }

        bool IUser.ForceLogin
        {
            get { return ForceLogin; }
        }

        bool IUser.IsActivated
        {
            get { return IsActivated; }
        }

        DateTime IUser.DateCreated
        {
            get { return DateCreated; }
        }

        DateTime IUser.LastActivityTime
        {
            get { return LastActivityTime; }
        }

        string IUser.LastAction
        {
            get { return LastAction; }
        }

        string IUser.IpCreated
        {
            get { return IpCreated; }
        }

        string IUser.IpLastActivity
        {
            get { return IpLastActivity; }
        }

        bool IUser.IsBanned
        {
            get { return IsBanned; }
        }

        bool IUser.IsModerated
        {
            get { return IsModerated; }
        }

        string IUser.Avatar
        {
            get { return Avatar; }
        }

        bool IUser.HasAvatar
        {
            get { return HasAvatar; }
        }

        /// <summary>
        /// 经验积分值
        /// </summary>
        int IUser.ExperiencePoints
        {
            get { return this.ExperiencePoints; }
        }

        /// <summary>
        /// 威望积分值
        /// </summary>
        int IUser.ReputationPoints
        {
            get { return this.ReputationPoints; }
        }

        /// <summary>
        /// 交易积分值
        /// </summary>
        int IUser.TradePoints
        {
            get { return this.TradePoints; }
        }

        /// <summary>
        /// 交易积分值2
        /// </summary>
        int IUser.TradePoints2
        {
            get { return this.TradePoints2; }
        }

        /// <summary>
        /// 交易积分值3
        /// </summary>
        int IUser.TradePoints3
        {
            get { return this.TradePoints3; }
        }

        /// <summary>
        /// 交易积分值4
        /// </summary>
        int IUser.TradePoints4
        {
            get { return this.TradePoints4; }
        }

        /// <summary>
        /// 用户等级
        /// </summary>
        int IUser.Rank
        {
            get { return this.Rank; }
        }

        /// <summary>
        /// 冻结的交易积分
        /// </summary>
        int IUser.FrozenTradePoints
        {
            get { return this.FrozenTradePoints; }
        }

        #endregion 显示实现接口

        #region 扩展属性

        /// <summary>
        /// 是否有头像
        /// </summary>
        [Ignore]
        public bool HasAvatar
        {
            get
            {
                if (Avatar.IndexOf(UserId.ToString()) >= 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 对外显示名称
        /// </summary>
        [Ignore]
        public string DisplayName
        {
            get
            {
                IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
                DisplayNameType displayNameType = userSettingsManager.Get().DisplayNameType;
                if (displayNameType == DisplayNameType.NicknameFirst)
                {
                    if (!string.IsNullOrEmpty(this.NickName))
                        return this.NickName;
                    else if (!string.IsNullOrEmpty(this.TrueName))
                        return this.TrueName;
                }
                else if (displayNameType == DisplayNameType.TrueNameFirst)
                {
                    if (!string.IsNullOrEmpty(this.TrueName))
                        return this.TrueName;
                    else if (!string.IsNullOrEmpty(this.NickName))
                        return this.NickName;
                }

                return UserName;
            }
        }


        #endregion

        /// <summary>
        /// 用户资料
        /// </summary>
        [Ignore]
        public UserProfile Profile
        {
            get { return new UserProfileService().Get(this.UserId); }
        }

        /// <summary>
        /// 用户是否允许被关注
        /// </summary>
        [Ignore]
        public bool IsCanbeFollow
        {
            get
            {
                var followStatus = new PrivacyService().GetUserPrivacySettings(this.UserId).Where(n => n.Key == "Follow");
                if (followStatus.Count() == 0)
                    return true;
                if (followStatus.First().Value == PrivacyStatus.Public)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 判断用户是否在线
        /// </summary>
        [Ignore]
        public bool IsOnline
        {
            get
            {
                if (UserContext.CurrentUser != null && UserContext.CurrentUser.UserId == this.UserId)
                    return true;
                else
                    return new OnlineUserService().IsOnline(this.UserName);
            }
        }

        /// <summary>
        /// 总浏览数
        /// </summary>
        [Ignore]
        public int HitTimes
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().User());
                return countService.Get(CountTypes.Instance().HitTimes(), this.UserId);
            }
        }

        /// <summary>
        /// 内容数
        /// </summary>
        [Ignore]
        public long ContentCount
        {
            get
            {
                string tenantTypeId = TenantTypeIds.Instance().User();
                IEnumerable<string> dataKeys = OwnerDataSettings.GetDataKeys(tenantTypeId).Where(n => n != "PointMall-ThreadCount");
                if (dataKeys != null && dataKeys.Count() > 0)
                {
                    return new OwnerDataService(tenantTypeId).GetTotalCount(dataKeys, UserId);
                }

                return 0;
            }
        }

        /// <summary>
        /// 最近七天的威望数
        /// </summary>
        [Ignore]
        public int PreWeekReputationPointsCount
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().User());
                int count = countService.GetStageCount(CountTypes.Instance().ReputationPointsCounts(), 7, this.UserId);
                if (count < 0)
                    return 0;
                return count;
            }
        }

        /// <summary>
        /// 最近七天浏览数
        /// </summary>
        [Ignore]
        public int PreWeekHitTimes
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().User());
                int count = countService.GetStageCount(CountTypes.Instance().HitTimes(), 7, this.UserId);
                if (count < 0)
                    return 0;
                return count;
            }
        }


        [Ignore]
        public List<string> IdentificationTypeLogos
        {
            get
            {
                IdentificationService identificationService = new IdentificationService();
                List<string> identificationTypeLogoList = new List<string>();
                IEnumerable<Identification> Identification = identificationService.GetUserIdentifications(UserId);
                foreach (var item in Identification)
                {
                    var identificationTypes = identificationService.GetIdentificationTypes(true).Where(n => n.IdentificationTypeId == item.IdentificationTypeId);
                    if (identificationTypes != null && identificationTypes.Count() > 0)
                    {
                        identificationTypeLogoList.Add(identificationTypes.FirstOrDefault().IdentificationTypeLogo); ;
                    }
                    else
                    {
                        continue;
                    }
                }
                return identificationTypeLogoList;
            }
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.UserId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion


    }
}