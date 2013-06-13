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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 验证码管理器接口
    /// </summary>
    public interface ICaptchaManager
    {
        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="showCaptchaImage">默认是否显示验证码图片(仅针对验证码图片可以不立即显示的情况)</param>
        /// <returns></returns>
        MvcHtmlString GenerateCaptcha<TModel>(HtmlHelper<TModel> htmlHelper, bool showCaptchaImage = false);

        /// <summary>
        /// 验证码是否输入正确
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="captchaInputName">输入框名称</param>
        /// <returns></returns>
        bool IsCaptchaValid(ActionExecutingContext filterContext);
    }
}
