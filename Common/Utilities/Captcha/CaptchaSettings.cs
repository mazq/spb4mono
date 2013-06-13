//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Linq;

namespace Spacebuilder.Common
{
    
    /// <summary>
    /// 验证码配置类
    /// </summary>
    public class CaptchaSettings
    {
        private string enableCaptcha = string.Empty;
        #region 单例
        private static volatile CaptchaSettings _instance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static CaptchaSettings Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new CaptchaSettings();
                    }
                }
            }
            return _instance;
        }

        #endregion
        #region 配置属性

        private int captchaLoginCount = 0;
        /// <summary>
        /// 显示验证码登录失败的次数
        /// </summary>
        public int CaptchaLoginCount
        {
            get { return captchaLoginCount; }
        }

        private int captchaPostCount = 0;
        /// <summary>
        /// 显示验证码连续发帖的次数
        /// </summary>
        public int CaptchaPostCount
        {
            get { return captchaPostCount; }
        }

        private string captchaCookieName;
        /// <summary>
        /// 验证码cookiename
        /// </summary>
        public string CaptchaCookieName
        {
            get { return captchaCookieName; }
        }

        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public bool EnableCaptcha
        {
            get
            {
                if (enableCaptcha.Equals("Enable", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 是否仅对匿名用户启用验证码
        /// </summary>
        public bool EnableAnonymousCaptcha
        {
            get
            {
                if (enableCaptcha.Equals("Anonymous", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                return false;
            }
        }

        private bool enableLineNoise = false;
        /// <summary>
        /// 是否使用干扰线
        /// </summary>
        public bool EnableLineNoise
        {
            get { return enableLineNoise; }
        }

        private CaptchaCharacterSet characterSet = CaptchaCharacterSet.LowercaseLetters;
        /// <summary>
        /// 验证码字符集
        /// </summary>
        public CaptchaCharacterSet CharacterSet
        {
            get { return characterSet; }
        }

        private int minCharacterCount = 4;
        /// <summary>
        /// 最小字符数
        /// </summary>
        public int MinCharacterCount
        {
            get { return maxCharacterCount; }
        }

        private int maxCharacterCount = 5;
        /// <summary>
        /// 最大字符数
        /// </summary>
        public int MaxCharacterCount
        {
            get { return maxCharacterCount; }
        }


        #endregion
        /// <summary>
        /// 私有构造器
        /// </summary>
        private CaptchaSettings()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:Enable"))
                enableCaptcha = ConfigurationManager.AppSettings["Captcha:Enable"].ToString();

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:CookieName"))
                captchaCookieName = ConfigurationManager.AppSettings["Captcha:CookieName"].ToString();

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:LoginCount"))
                captchaLoginCount = Convert.ToInt32(ConfigurationManager.AppSettings["Captcha:LoginCount"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:PostCount"))
                captchaPostCount = Convert.ToInt32(ConfigurationManager.AppSettings["Captcha:PostCount"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:EnableLineNoise"))
                bool.TryParse(ConfigurationManager.AppSettings["Captcha:EnableLineNoise"], out enableLineNoise);

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:CharacterSet"))
                Enum.TryParse<CaptchaCharacterSet>(ConfigurationManager.AppSettings["Captcha:CharacterSet"], out characterSet);

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:MinCharacterCount"))
                minCharacterCount = Convert.ToInt32(ConfigurationManager.AppSettings["Captcha:MinCharacterCount"].ToString());

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Captcha:MaxCharacterCount"))
                maxCharacterCount = Convert.ToInt32(ConfigurationManager.AppSettings["Captcha:MaxCharacterCount"].ToString());
        }

    }
}
