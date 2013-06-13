//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 用户相关设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class UserSettings : IEntity
    {

        private RegistrationMode registrationMode = RegistrationMode.Invitation;
        /// <summary>
        /// 用户注册方式设置
        /// </summary>
        public RegistrationMode RegistrationMode
        {
            get { return registrationMode; }
            set { registrationMode = value; }
        }

        AccountActivation accountActivation = AccountActivation.Email;
        /// <summary>
        /// 账户激活方法
        /// </summary>
        public AccountActivation AccountActivation
        {
            get { return accountActivation; }
            set { accountActivation = value; }
        }

        private int minUserNameLength = 2;

        /// <summary>
        /// 用户名最短长度
        /// </summary>
        public int MinUserNameLength
        {
            get { return minUserNameLength; }
            set { minUserNameLength = value > 64 ? 64 : value; }
        }

        private int maxUserNameLength = 64;

        /// <summary>
        /// 用户名的最大长度
        /// </summary>
        public int MaxUserNameLength
        {
            get { return maxUserNameLength; }
            set { maxUserNameLength = value > 64 ? 64 : value; }
        }


        string userNameRegex = @"^[\w|\u4e00-\u9fa5]{1,64}$";
        /// <summary>
        /// 用户名验证正则表达式
        /// </summary>
        public string UserNameRegex
        {
            get { return userNameRegex; }
            set { userNameRegex = value; }
        }

        string nickNameRegex = @"^[\w|\u4e00-\u9fa5]{1,64}$";

        /// <summary>
        /// 昵称的正则
        /// </summary>
        public string NickNameRegex
        {
            get { return nickNameRegex; }
            set { nickNameRegex = value; }
        }

        int minPasswordLength = 4;
        /// <summary>
        /// 密码最小长度
        /// </summary>
        public int MinPasswordLength
        {
            get { return minPasswordLength; }
            set { minPasswordLength = value > 4 ? value : 4; }
        }

        private int minRequiredNonAlphanumericCharacters = 0;
        /// <summary>
        /// 密码中包含的最少特殊字符数
        /// </summary>
        public int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
            set
            {
                if (value < 0)
                    minRequiredNonAlphanumericCharacters = 0;
                else
                    minRequiredNonAlphanumericCharacters = value;
            }
        }

        string emailRegex = @"^([a-zA-Z0-9_\.-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$";
        /// <summary>
        /// Email验证正则表达式
        /// </summary>
        public string EmailRegex
        {
            get { return emailRegex; }
            set { emailRegex = value; }
        }

        private bool enableTrackAnonymous = true;
        /// <summary>
        /// 是否启用匿名用户跟踪
        /// </summary>
        public bool EnableTrackAnonymous
        {
            get { return enableTrackAnonymous; }
            set { enableTrackAnonymous = value; }
        }

        private int userOnlineTimeWindow = 20;
        /// <summary>
        /// 指定用户在最近一次活动时间之后多长时间视为在线的分钟数
        /// </summary>
        public int UserOnlineTimeWindow
        {
            get { return userOnlineTimeWindow; }
            set { userOnlineTimeWindow = value; }
        }

        private bool enableNotActivatedUsersToLogin = false;
        /// <summary>
        /// 允许未激活的用户登录
        /// </summary>
        public bool EnableNotActivatedUsersToLogin
        {
            get { return enableNotActivatedUsersToLogin; }
            set { enableNotActivatedUsersToLogin = value; }
        }

        private bool requiresUniqueMobile = true;
        /// <summary>
        /// 用户注册时是否允许手机号重复
        /// </summary>
        public bool RequiresUniqueMobile
        {
            get { return requiresUniqueMobile; }
            set { requiresUniqueMobile = value; }
        }

        private UserPasswordFormat userPasswordFormat = UserPasswordFormat.MD5;
        /// <summary>
        /// 用户密码加密方式
        /// </summary>
        public UserPasswordFormat UserPasswordFormat
        {
            get { return userPasswordFormat; }
            set { userPasswordFormat = value; }
        }

        //private bool enableDefaultRole = true;
        ///// <summary>
        ///// 新建用户时是否设置默认的角色
        ///// </summary>
        //public bool EnableDefaultRole
        //{
        //    get { return this.enableDefaultRole; }
        //    set { this.enableDefaultRole = value; }
        //}

        //private string[] roleNamesForEnterControlPanel = null;
        ///// <summary>
        /////  可以进入后台的用户角色集合
        ///// </summary>
        //public string[] RoleNamesForEnterControlPanel
        //{
        //    get
        //    {
        //        if (roleNamesForEnterControlPanel == null)
        //        {
        //            roleNamesForEnterControlPanel = new string[2];
        //            roleNamesForEnterControlPanel[0] = UserRoleNames.SystemAdministrator;
        //            roleNamesForEnterControlPanel[1] = UserRoleNames.ContentFolderModerator;
        //        }

        //        return roleNamesForEnterControlPanel;
        //    }
        //    set { roleNamesForEnterControlPanel = value; }
        //}

        private bool enableNickname = true;
        /// <summary>
        /// 是否启用昵称
        /// </summary>
        public bool EnableNickname
        {
            get { return enableNickname; }
            set { enableNickname = value; }
        }

        private DisplayNameType displayNameType = DisplayNameType.TrueNameFirst;
        /// <summary>
        /// 用户对外显示哪个名称（如果未启用昵称，则该选项无需设置）
        /// </summary>
        public DisplayNameType DisplayNameType
        {
            get { return displayNameType; }
            set { displayNameType = value; }
        }


        private bool automaticModerated = true;
        /// <summary>
        /// 新注册用户是否自动处于管制状态
        /// </summary>
        public bool AutomaticModerated
        {
            get { return automaticModerated; }
            set { automaticModerated = value; }
        }

        private int noModeratedUserPoint = 500;
        ///	<summary>
        ///	用户自动接触管制状态所需的积分（用户综合积分）
        ///	</summary>
        public int NoModeratedUserPoint
        {
            get { return noModeratedUserPoint; }
            set { noModeratedUserPoint = value; }
        }


        private string disallowedUserNames = "admin";
        /// <summary>
        /// 不允许使用的用户名
        /// </summary>
        /// <remarks>
        /// 多个用户名之间用逗号分割
        /// </remarks>
        public string DisallowedUserNames
        {
            get { return disallowedUserNames; }
            set { disallowedUserNames = value; }
        }

        private bool myHomePageAsSiteEntry = true;
        /// <summary>
        /// 我的首页作为站点入口
        /// </summary>
        /// <remarks>
        /// 登录成功后是否跳转到我的首页
        /// </remarks>
        public bool MyHomePageAsSiteEntry
        {
            get { return myHomePageAsSiteEntry; }
            set { myHomePageAsSiteEntry = value; }
        }


        private string superAdministratorRoleName = "SuperAdministrator";
        /// <summary>
        /// 超级管理员角色名称
        /// </summary>
        public string SuperAdministratorRoleName
        {
            get { return superAdministratorRoleName; }
            set { superAdministratorRoleName = value; }
        }

        private string anonymousRoleName = "Anonymous";
        /// <summary>
        /// 匿名用户角色名称
        /// </summary>
        public string AnonymousRoleName
        {
            get { return anonymousRoleName; }
            set { anonymousRoleName = value; }
        }

        private bool enableAudit = true;
        /// <summary>
        /// 是否启用人工审核
        /// </summary>
        public bool EnableAudit
        {
            get { return enableAudit; }
            set { enableAudit = value; }
        }

        private List<string> noAuditedRoleNames = new List<string> { RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator() };
        /// <summary>
        /// 不需要审核的角色集合
        /// </summary>
        public List<string> NoAuditedRoleNames
        {
            get { return noAuditedRoleNames; }
            set { noAuditedRoleNames = value; }
        }

        private int minNoAuditedUserRank = 8;
        /// <summary>
        /// 最小不需要审核的用户等级
        /// </summary>
        public int MinNoAuditedUserRank
        {
            get { return minNoAuditedUserRank; }
            set { minNoAuditedUserRank = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(UserSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
