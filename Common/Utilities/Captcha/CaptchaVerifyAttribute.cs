//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using Tunynet;

namespace Spacebuilder.Common
{
    public class CaptchaVerifyAttribute : ActionFilterAttribute
    {
        private const string CaptchaVerifyError = "验证码输入错误";
        private VerifyScenarios scenarios = VerifyScenarios.Post;

        /// <summary>
        /// 带参构造器
        /// </summary>
        /// <param name="scenarios">验证码使用场景</param>
        public CaptchaVerifyAttribute(VerifyScenarios scenarios = VerifyScenarios.Post)
        {
            this.scenarios = scenarios;
        }

        /// <summary>
        /// 执行Action时
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CaptchaUtility.UseCaptcha(scenarios, true))
            {
                try
                {
                    ICaptchaManager captchaManager = DIContainer.Resolve<ICaptchaManager>();
                    ControllerBase controllerBase = filterContext.Controller;
                    if (!captchaManager.IsCaptchaValid(filterContext))
                    {
                        controllerBase.ViewData.ModelState.AddModelError("Captcha", CaptchaVerifyError);
                    }
                    else if (controllerBase.ViewData.ModelState.IsValid)
                    {
                        //表单通过验证时，重设计数
                        CaptchaUtility.ResetLimitTryCount(scenarios);
                    }
                }
                catch
                {
                    throw new ExceptionFacade("检查验证码时，出现异常");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
