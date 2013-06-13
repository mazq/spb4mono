//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Security;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 验证码管理类
    /// </summary>
    public static class CaptchaUtility
    {
        private static ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        #region 检查验证码出现条件

        /// <summary>
        /// 是否开始使用验证码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="scenarios">验证码使用场景</param>
        /// <returns></returns>
        public static bool UseCaptcha(VerifyScenarios scenarios = VerifyScenarios.Post, bool isLimitCount = false)
        {
            CaptchaSettings verificationCodeSettings = CaptchaSettings.Instance();
            if (!verificationCodeSettings.EnableCaptcha)
                return false;
            IUser currentUser = UserContext.CurrentUser;
            if (scenarios == VerifyScenarios.Register || currentUser == null && scenarios == VerifyScenarios.Post)
                return true;
            //后台登陆
            if (scenarios == VerifyScenarios.Login && currentUser != null)
                return true;

            if (currentUser == null && scenarios == VerifyScenarios.Post && verificationCodeSettings.EnableAnonymousCaptcha)
                return true;
            string userName = GetUserName();

            if (scenarios == VerifyScenarios.Login && UserIdToUserNameDictionary.GetUserId(userName) == 0)
                return false;

            string cacheKey = GetCacheKey_LimitTryCount(userName, scenarios);
            int? limitTryCount = cacheService.Get(cacheKey) as int?;

            if (limitTryCount.HasValue
                && ((scenarios == VerifyScenarios.Login && limitTryCount >= verificationCodeSettings.CaptchaLoginCount)
                || (scenarios == VerifyScenarios.Post && limitTryCount >= verificationCodeSettings.CaptchaPostCount)))
                return true;

            if (isLimitCount)
            {
                if (limitTryCount.HasValue)
                {
                    limitTryCount++;
                }
                else
                {
                    limitTryCount = 1;
                }
                cacheService.Set(cacheKey, limitTryCount, CachingExpirationType.SingleObject);
            }

            return false;
        }

        
        /// <summary>
        /// 重置使用验证码前尝试次数
        /// </summary>
        /// <param name="userName">用户名</param>
        public static void ResetLimitTryCount(VerifyScenarios scenarios)
        {
            string cacheKey = GetCacheKey_LimitTryCount(GetUserName(), scenarios);
            int? limitTryCount = cacheService.Get(cacheKey) as int?;
            if (limitTryCount.HasValue)
                cacheService.Remove(cacheKey);
        }

        #endregion

        private static string GetUserName()
        {
            HttpContext httpContext = HttpContext.Current;
            string userName = string.Empty;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null)
                userName = currentUser.UserName;
            else if (!string.IsNullOrEmpty(httpContext.Request.Form["UserName"]))
                userName = httpContext.Request.Form["UserName"];
            return userName;
        }

        #region CacheKey

        /// <summary>
        ///尝试次数统计的CacheKey
        /// </summary>
        private static string GetCacheKey_LimitTryCount(string userName, VerifyScenarios scenarios)
        {
            return string.Format("LimitTryCount::UserName-{0}:Scenarios-{1}", userName, scenarios);
        }

        #endregion
    }
}