//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tunynet;
using Tunynet.Common;
using Tunynet.Common.Configuration;
using Tunynet.Globalization;
using Tunynet.Utilities;
using Tunynet.Logging;
using System.Web;
using System.Collections.Generic;
using System.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 获取隐私的特别类型的
        /// </summary>
        /// <param name="PrivacySpecifyUsers">隐私的用户的id字符串</param>
        /// <param name="PrivacySpecifyUserGroup">隐私的分组的id字符串</param>
        /// <param name="tenantTypeId">类型id</param>
        /// <param name="contentId">对象id</param>
        /// <returns>隐私特别对象集合</returns>
        public static Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> GetContentPrivacySpecifyObjects(string PrivacySpecifyUsers, string PrivacySpecifyUserGroup, string tenantTypeId, long contentId)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();
            CategoryService categoryService = new CategoryService();
            Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> privacySpecifyObjects = new Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>>();

            if (!string.IsNullOrEmpty(PrivacySpecifyUsers))
            {
                string[] userIds = PrivacySpecifyUsers.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                IEnumerable<User> users = userService.GetFullUsers(userIds.Select(n => long.Parse(n)));
                List<ContentPrivacySpecifyObject> contentPrivacySpecifyObjects = new List<ContentPrivacySpecifyObject>();
                foreach (User user in users)
                {
                    ContentPrivacySpecifyObject contentPrivacySpecifyObject = ContentPrivacySpecifyObject.New();
                    contentPrivacySpecifyObject.TenantTypeId = tenantTypeId;
                    contentPrivacySpecifyObject.ContentId = contentId;
                    contentPrivacySpecifyObject.SpecifyObjectId = user.UserId;
                    contentPrivacySpecifyObject.SpecifyObjectName = user.DisplayName;
                    contentPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().User();

                    contentPrivacySpecifyObjects.Add(contentPrivacySpecifyObject);
                }

                privacySpecifyObjects[SpecifyObjectTypeIds.Instance().User()] = contentPrivacySpecifyObjects;
            }

            if (!string.IsNullOrEmpty(PrivacySpecifyUserGroup))
            {
                List<ContentPrivacySpecifyObject> contentPrivacySpecifyObjects = new List<ContentPrivacySpecifyObject>();
                if (PrivacySpecifyUserGroup.Contains("-1"))
                {
                    ContentPrivacySpecifyObject contentPrivacySpecifyObject = ContentPrivacySpecifyObject.New();
                    contentPrivacySpecifyObject.TenantTypeId = tenantTypeId;
                    contentPrivacySpecifyObject.ContentId = contentId;
                    contentPrivacySpecifyObject.SpecifyObjectId = -1;
                    contentPrivacySpecifyObject.SpecifyObjectName = "我关注的所有人";
                    contentPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().UserGroup();

                    contentPrivacySpecifyObjects.Add(contentPrivacySpecifyObject);
                }
                else
                {
                    string[] userGroupIds = PrivacySpecifyUserGroup.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (string categoryId in userGroupIds)
                    {
                        ContentPrivacySpecifyObject contentPrivacySpecifyObject = ContentPrivacySpecifyObject.New();
                        contentPrivacySpecifyObject.TenantTypeId = tenantTypeId;
                        contentPrivacySpecifyObject.ContentId = contentId;
                        contentPrivacySpecifyObject.SpecifyObjectTypeId = SpecifyObjectTypeIds.Instance().UserGroup();

                        if (categoryId == "-2")
                        {
                            contentPrivacySpecifyObject.SpecifyObjectId = -2;
                            contentPrivacySpecifyObject.SpecifyObjectName = "相互关注";

                            contentPrivacySpecifyObjects.Add(contentPrivacySpecifyObject);
                        }
                        else
                        {
                            Category category = categoryService.Get(long.Parse(categoryId));
                            contentPrivacySpecifyObject.SpecifyObjectId = category.CategoryId;
                            contentPrivacySpecifyObject.SpecifyObjectName = category.CategoryName;

                            contentPrivacySpecifyObjects.Add(contentPrivacySpecifyObject);
                        }
                    }
                }

                privacySpecifyObjects[SpecifyObjectTypeIds.Instance().UserGroup()] = contentPrivacySpecifyObjects;
            }

            return privacySpecifyObjects;
        }

        #region 加密与解密
        /// <summary>
        /// 加密AdiminCookie
        /// </summary>
        /// <param name="timeliness">加密有效期</param>
        /// <param name="userId">要加密的用户Id</param>
        /// <returns>加密令牌</returns>
        public static string EncryptTokenForAdminCookie(string encryptString)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForAdminLogin"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForAdminLogin"];
            return EncryptTokenForCookie(encryptString, key, iv);
        }

        /// <summary>
        /// 解密AdiminCookie
        /// </summary>
        /// <param name="token">要解密的令牌</param>
        /// <param name="isTimeout">输出参数：令牌是否过期</param>
        /// <returns>解密后的用户Id</returns>
        public static string DecryptTokenForAdminCookie(string token)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForAdminLogin"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForAdminLogin"];
            return DecryptTokenForCookie(token, key, iv);
        }


        /// <summary>
        /// 加密上传图片所属用户的Id
        /// </summary>
        /// <param name="timeliness">加密有效期</param>
        /// <param name="userId">要加密的用户Id</param>
        /// <returns>加密令牌</returns>
        public static string EncryptTokenForUploadfile(double timeliness, long userId)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForUploadfile"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForUploadfile"];
            return EncryptToken(timeliness, userId, key, iv);
        }

        /// <summary>
        /// 解密上传附件的用户Id
        /// </summary>
        /// <param name="token">要解密的令牌</param>
        /// <param name="isTimeout">输出参数：令牌是否过期</param>
        /// <returns>解密后的用户Id</returns>
        public static long DecryptTokenForUploadfile(string token, out bool isTimeout)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForUploadfile"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForUploadfile"];
            return DecryptToken(token, out isTimeout, key, iv);
        }

        
        /// <summary>
        /// 用于邀请好友的Token加密方法
        /// </summary>
        /// <param name="timeliness">实效（天）</param>
        /// <param name="userId">用户id</param>
        /// <returns>生成的token</returns>
        public static string EncryptTokenForInviteFriend(double timeliness, long userId)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForInviteFriend"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForInviteFriend"];
            
            return EncryptToken(timeliness, userId, key, iv);
        }

        /// <summary>
        /// 用户邀请好友的Toke解析方法
        /// </summary>
        /// <param name="token">邀请链接</param>
        /// <param name="isTimeout">是否超时</param>
        /// <returns>用户id</returns>
        public static long DecryptTokenForInviteFriend(string token, out bool isTimeout)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForInviteFriend"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForInviteFriend"];
            return DecryptToken(token, out isTimeout, key, iv);
        }

        /// <summary>
        /// 生成忘记密码Token
        /// </summary>
        /// <param name="timeliness">实效（天）</param>
        /// <param name="userId">用户id</param>
        /// <returns>生成的token</returns>
        public static string EncryptTokenFindPassword(double timeliness, long userId)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForFindPassword"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForFindPassword"];
            return EncryptToken(timeliness, userId, key, iv);
        }

        /// <summary>
        /// 解析忘记密码Token
        /// </summary>
        /// <param name="token">链接</param>
        /// <param name="isTimeout">是否超时</param>
        /// <returns>用户id</returns>
        public static long DecryptTokenForFindPassword(string token, out bool isTimeout)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForFindPassword"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForFindPassword"];
            return DecryptToken(token, out isTimeout, key, iv);
        }

        /// <summary>
        /// 生成验证邮箱的token
        /// </summary>
        /// <param name="timeliness">时间限制</param>
        /// <param name="userId">用户id</param>
        /// <returns>生成的token</returns>
        public static string EncryptTokenForValidateEmail(double timeliness, long userId)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForValidateEmail"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForValidateEmail"];
            return EncryptToken(timeliness, userId, key, iv);
        }

        /// <summary>
        /// 解析验证邮箱的token
        /// </summary>
        /// <param name="token">被验证的token</param>
        /// <param name="isTimeout">标识是否过期</param>
        /// <returns>用户的id</returns>
        public static long DecryptTokenForValidateEmail(string token, out bool isTimeout)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForValidateEmail"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForValidateEmail"];
            return DecryptToken(token, out isTimeout, key, iv);
        }

        /// <summary>
        /// 获取加密附件下载的token
        /// </summary>
        /// <param name="timeliness">加密有效期</param>
        /// <param name="ip">要加密的用户ip</param>
        /// <returns>加密令牌</returns>
        public static string EncryptTokenForAttachmentDownload(double timeliness, long attachmentId)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForAttachmentDownload"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForAttachmentDownload"];
            return EncryptToken(timeliness, attachmentId, key, iv);
        }

        /// <summary>
        /// 附件下载的token解密
        /// </summary>
        /// <param name="token">加密串</param>
        /// <param name="isTimeout">是否失效</param>
        /// <returns></returns>
        public static long DecryptTokenForAttachmentDownload(string token, out bool isTimeout)
        {
            string key = System.Configuration.ConfigurationManager.AppSettings["TokenKeyForAttachmentDownload"];
            string iv = System.Configuration.ConfigurationManager.AppSettings["TokenIvForAttachmentDownload"];

            return DecryptToken(token, out isTimeout, key, iv);
        }

        /// <summary>
        /// 加密的操作类
        /// </summary>
        /// <param name="timeliness">时限</param>
        /// <param name="userId">用户id</param>
        /// <param name="key">key</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        private static string EncryptToken(double timeliness, long userId, string key, string iv)
        {
            string tonkenStr = userId + "," + DateTime.Now.AddDays(timeliness).ToString("yyyy/MM/dd HH:mm");
            string token = EncryptionUtility.SymmetricEncrypt(SymmetricEncryptType.DES, tonkenStr, iv, key);
            return WebUtility.UrlEncode(token);
        }

        /// <summary>
        /// 解密操作类
        /// </summary>
        /// <param name="token">网络令牌</param>
        /// <param name="isTimeout">是否失效</param>
        /// <param name="key">key</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        private static long DecryptToken(string token, out bool isTimeout, string key, string iv)
        {
            long userId = 0;
            isTimeout = true;
            try
            {
                token = token.Replace(" ", "+");
                string tokenStr = EncryptionUtility.SymmetricDncrypt(SymmetricEncryptType.DES, token, iv, key);
                int partition = tokenStr.IndexOf(',');
                if (partition > 0)
                {
                    long.TryParse(tokenStr.Substring(0, tokenStr.IndexOf(',')), out userId);
                    DateTime dtTokenTime = new DateTime();
                    DateTime.TryParse(tokenStr.Substring(tokenStr.IndexOf(',')), out dtTokenTime);
                    isTimeout = DateTime.Compare(DateTime.Now, dtTokenTime) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionFacade("解密操作的时候发生错误", ex);
            }
            return userId;
        }

        #endregion

        #region 验证信息
        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <param name="userName">待验证的用户名</param>
        /// <param name="errorMessage">输出出错信息</param>
        /// <returns>是否通过验证</returns>
        public static bool ValidateUserName(string userName, out string errorMessage)
        {
            if (string.IsNullOrEmpty(userName))
            {
                errorMessage = ResourceAccessor.GetString("Validate_UserNameRequired");
                return false;
            }
            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();

            if (userName.Length < userSettings.MinUserNameLength)
            {
                errorMessage = string.Format(ResourceAccessor.GetString("Validate_UserNameTooShort"), userSettings.MinUserNameLength);
                return false;
            }

            if (userName.Length > userSettings.MaxUserNameLength)
            {
                errorMessage = string.Format(ResourceAccessor.GetString("Validate_UserNameTooLong"), userSettings.MaxUserNameLength);
                return false;
            }

            Regex regex = new Regex(userSettings.UserNameRegex);
            if (!regex.IsMatch(userName))
            {
                errorMessage = ResourceAccessor.GetString("Validate_UserNameRegex");
                return false;
            }

            AuthorizationService authorizationService = new AuthorizationService();
            authorizationService.IsSuperAdministrator(UserContext.CurrentUser);
            //验证UserName是否被禁止使用
            if (!authorizationService.IsSuperAdministrator(UserContext.CurrentUser) &&
                userSettings.DisallowedUserNames.Split(',', '，').Any(n => n.Equals(userName, StringComparison.CurrentCultureIgnoreCase)))
            {
                errorMessage = ResourceAccessor.GetString("Validate_UserNameIsDisallowed");
                return false;
            }


            //验证UserName是否已经存在
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.GetUser(userName);
            if (user != null)
            {
                errorMessage = ResourceAccessor.GetString("Validate_UserNameIsExisting");
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email">待验证的邮箱</param>
        /// <param name="errorMessage">输出出错信息</param>
        /// <returns>是否通过验证</returns>
        public static bool ValidateEmail(string email, out string errorMessage)
        {
            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();

            if (string.IsNullOrEmpty(email))
            {
                errorMessage = ResourceAccessor.GetString("Validate_EmailRequired");
                return false;
            }

            Regex regex = new Regex(userSettings.EmailRegex, RegexOptions.ECMAScript);
            if (!regex.IsMatch(email))
            {
                errorMessage = ResourceAccessor.GetString("Validate_EmailStyle");
                return false;
            }

            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.FindUserByEmail(email);

            //验证email是否已经存在
            if (user != null)
            {
                errorMessage = string.Format(ResourceAccessor.GetString("Message_Pattern_RegisterFailedForDuplicateEmailAddress"), SiteUrls.Instance().FindPassword(email));
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// 检测用户密码是否适合站点设置
        /// </summary>
        /// <param name="newPassword">Password to be verified.</param>
        /// <param name="errorMessage">Error message to return.</param>
        /// <returns>True if compliant, otherwise False.</returns>
        public static bool ValidatePassword(string newPassword, out string errorMessage)
        {

            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();
            int minRequiredPasswordLength = userSettings.MinPasswordLength;
            int minRequiredNonAlphanumericCharacters = userSettings.MinRequiredNonAlphanumericCharacters;

            errorMessage = "";

            if (string.IsNullOrEmpty(newPassword))
            {
                errorMessage = ResourceAccessor.GetString("Validate_RequiredPassword");
                return false;
            }

            if (newPassword.Length < minRequiredPasswordLength)
            {
                errorMessage = string.Format(ResourceAccessor.GetString("Validate_Pattern_MinRequiredPasswordLength"), minRequiredPasswordLength);
                return false;
            }

            int nonAlphaNumChars = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                    nonAlphaNumChars++;
            }
            if (nonAlphaNumChars < minRequiredNonAlphanumericCharacters)
            {
                errorMessage = string.Format(ResourceAccessor.GetString("Validate_Pattern_MinRequiredNonAlphanumericCharacters"), minRequiredNonAlphanumericCharacters);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证昵称是否可用
        /// </summary>
        /// <param name="nickName">被验证的昵称</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns></returns>
        public static bool ValidateNickName(string nickName, out string errorMessage)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                errorMessage = ResourceAccessor.GetString("Validate_NickNameRequired");
                return false;
            }
            IUserSettingsManager userSettingsManager = DIContainer.Resolve<IUserSettingsManager>();
            UserSettings userSettings = userSettingsManager.Get();

            if (nickName.Length < userSettings.MinUserNameLength)
            {
                errorMessage = string.Format(ResourceAccessor.GetString("Validate_NickNameTooShort"), userSettings.MinUserNameLength);
                return false;
            }

            if (nickName.Length > userSettings.MaxUserNameLength)
            {
                errorMessage = string.Format(ResourceAccessor.GetString("Validate_NickNameTooLong"), userSettings.MaxUserNameLength);
                return false;
            }

            Regex regex = new Regex(userSettings.NickNameRegex);
            if (!regex.IsMatch(nickName))
            {
                errorMessage = ResourceAccessor.GetString("Validate_NickNameRegex");
                return false;
            }

            AuthorizationService authorizationService = new AuthorizationService();
            if (!authorizationService.IsSuperAdministrator(UserContext.CurrentUser) &&
                userSettings.DisallowedUserNames.Split(',', '，').Any(n => n.Equals(nickName, StringComparison.CurrentCultureIgnoreCase)))
            {
                errorMessage = ResourceAccessor.GetString("Validate_NickNameIsDisallowed");
                return false;
            }


            //验证nickName是否已经存在
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser user = userService.GetUserByNickName(nickName);
            if (user != null)
            {
                errorMessage = ResourceAccessor.GetString("Validate_NickNameIsExisting");
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        #endregion

        #region 生成随机码
        /// <summary>
        /// 随机种子
        /// </summary>
        private static Random RndSeed = new Random();
        /// <summary>
        /// 生成一个随机码
        /// </summary>
        /// <returns></returns>
        public static string GenerateRndNonce()
        {
            return string.Concat(
            Utility.RndSeed.Next(1, 99999999).ToString("00000000"),
            Utility.RndSeed.Next(1, 99999999).ToString("00000000"),
            Utility.RndSeed.Next(1, 99999999).ToString("00000000"),
            Utility.RndSeed.Next(1, 99999999).ToString("00000000"));
        }
        #endregion

        /// <summary>
        /// 是不是合法的请求
        /// </summary>
        /// <remarks>
        /// 用于防盗链的检测、防洪攻击
        /// </remarks>
        /// <returns></returns>
        public static bool IsAllowableReferrer(HttpRequestBase httpRequest)
        {
            if (httpRequest == null || httpRequest.UrlReferrer == null)
                return false;
            string[] domainRules = { };
            
            string urlReferrerDomain = WebUtility.GetServerDomain(httpRequest.UrlReferrer, domainRules);
            string urlDomain = WebUtility.GetServerDomain(httpRequest.Url, domainRules);

            return urlReferrerDomain.Equals(urlDomain, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 加密的操作类
        /// </summary>
        /// <param name="timeliness">时限</param>
        /// <param name="encryptString">encryptString</param>
        /// <param name="key">key</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        private static string EncryptTokenForCookie(string encryptString, string key, string iv)
        {
            string tonkenStr = encryptString;
            return EncryptionUtility.SymmetricEncrypt(SymmetricEncryptType.DES, tonkenStr, iv, key);
        }

        /// <summary>
        /// 解密操作类
        /// </summary>
        /// <param name="token">网络令牌</param>
        /// <param name="isTimeout">是否失效</param>
        /// <param name="key">key</param>
        /// <param name="iv">向量</param>
        /// <returns></returns>
        private static string DecryptTokenForCookie(string token, string key, string iv)
        {
            string encryptString = string.Empty;
            try
            {
                token = token.Replace(" ", "+");
                encryptString = EncryptionUtility.SymmetricDncrypt(SymmetricEncryptType.DES, token, iv, key);
            }
            catch (Exception ex)
            {
                throw new ExceptionFacade("解密操作的时候发生错误", ex);
            }
            return encryptString;
        }

        /// <summary>
        /// 检查是否分布式运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsDistributedDeploy()
        {
            bool distributedDeploy = false;
            if (ConfigurationManager.AppSettings["DistributedDeploy"] != null)
            {
                if (!bool.TryParse(ConfigurationManager.AppSettings["DistributedDeploy"], out distributedDeploy))
                {
                    distributedDeploy = false;
                }
            }

            return distributedDeploy;
        }

        /// <summary>
        /// 检查文件存储是否为分布式运行环境
        /// </summary>
        /// <returns></returns>
        public static bool IsFileDistributedDeploy()
        {
            bool fileDistributedDeploy = false;
            if (ConfigurationManager.AppSettings["FileDistributedDeploy"] != null)
            {
                if (!bool.TryParse(ConfigurationManager.AppSettings["FileDistributedDeploy"], out fileDistributedDeploy))
                {
                    fileDistributedDeploy = false;
                }
            }

            return fileDistributedDeploy;
        }

    }
}
