//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using Tunynet.Utilities;
using System.Web.Mvc.Html;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 默认验证码管理器
    /// </summary>
    public class DefaultCaptchaManager : ICaptchaManager
    {
        private string _captchaInputName = "CaptchaInputName";
        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="showCaptchaImage">默认是否显示验证码图片</param>
        /// <returns></returns>
        public MvcHtmlString GenerateCaptcha<TModel>(HtmlHelper<TModel> htmlHelper, bool showCaptchaImage = false)
        {
            return htmlHelper.EditorForModel("DefaultCaptchaInput", new { InputName = _captchaInputName, ShowCaptchaImage = showCaptchaImage });
        }

        /// <summary>
        /// 验证码是否输入正确
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool IsCaptchaValid(ActionExecutingContext filterContext)
        {
            ControllerBase controllerBase = filterContext.Controller;
            string captchaText = controllerBase.ControllerContext.HttpContext.Request.Form[_captchaInputName];
            if (string.IsNullOrEmpty(captchaText))
                return false;

            string cookieName = CaptchaSettings.Instance().CaptchaCookieName;
            HttpCookie coookie = filterContext.HttpContext.Request.Cookies[cookieName];

            string cookieCaptcha = string.Empty;

            if (coookie != null)
            {
                if (!string.IsNullOrEmpty(coookie.Value))
                {
                    try
                    {
                        cookieCaptcha = VerificationCodeManager.GetCachedTextAndForceExpire(filterContext.HttpContext, coookie.Value);
                    }
                    catch { }
                }
            }

            //从cookie未获取验证码时，提供一个随机数
            if (cookieCaptcha == null)
                cookieCaptcha = DateTime.UtcNow.Ticks.ToString();

            if (!string.IsNullOrEmpty(captchaText)
                && !captchaText.Equals(cookieCaptcha, StringComparison.CurrentCultureIgnoreCase))
                return false;
            return true;
        }
    }
}