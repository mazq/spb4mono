//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Web;
using System.Configuration;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 生成验证码
    /// </summary>
    public class CaptchaHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpContextBase currentContext = new HttpContextWrapper(context);
            bool isremove = false;
            if (!string.IsNullOrEmpty(context.Request.QueryString["isremove"]))
                bool.TryParse(context.Request.QueryString["isremove"], out isremove);

            string cookieName = CaptchaSettings.Instance().CaptchaCookieName;
            bool enableLineNoise = CaptchaSettings.Instance().EnableLineNoise;
            CaptchaCharacterSet characterSet = CaptchaSettings.Instance().CharacterSet;
            int minCharacterCount = CaptchaSettings.Instance().MinCharacterCount;
            int maxCharacterCount = CaptchaSettings.Instance().MaxCharacterCount;
            string generatedKey = string.Empty;
            bool addCooikes = false;
            //创建或从缓存取验证码
            string key = null;
            if (context.Request.Cookies[cookieName] != null)
                key = context.Request.Cookies[cookieName].Value;
            if (isremove && !string.IsNullOrEmpty(key))
                VerificationCodeManager.GetCachedTextAndForceExpire(currentContext, getCurrentLevelKey(key));

            System.IO.MemoryStream ms = null;
            if (!string.IsNullOrEmpty(key))
                ms = VerificationCodeManager.GetCachedImageStream(getCurrentLevelKey(key));

            if (ms == null)
            {
                Size size = new Size(85, 30);
                VerificationCodeImage image = VerificationCodeManager.GenerateAndCacheImage(currentContext, size, 300, out generatedKey, characterSet, enableLineNoise, minCharacterCount, maxCharacterCount);

                ms = VerificationCodeManager.GetCachedImageStream(getCurrentLevelKey(generatedKey));
                VerificationCodeManager.CacheText(currentContext, image.Text, getCurrentLevelKey(generatedKey), false, 300);
                addCooikes = true;
            }
            if (addCooikes)
            {
                HttpCookie cookie = new HttpCookie(cookieName, generatedKey);
                context.Response.Cookies.Add(cookie);
            }
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "image/Jpeg";
            context.Response.BinaryWrite(ms.ToArray());
            //context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 获取某一级别的Key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private string getCurrentLevelKey(string publicKey)
        {
            return publicKey;
        }
    }
}