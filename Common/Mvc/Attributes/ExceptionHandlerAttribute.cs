//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Mvc;
using Tunynet;

namespace Spacebuilder.Common
{
    

    /// <summary>
    /// 用于处理异常的过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 发生异常时，跳转至异常信息显示页
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnException(ExceptionContext filterContext)
        {

            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (filterContext.IsChildAction)
                return;

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            if (filterContext.ExceptionHandled)
                return;

            Exception exception = filterContext.Exception;

            ExceptionFacade exceptionFacade = new ExceptionFacade(exception.Message, exception);
            exceptionFacade.Log();

            // If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method),
            // ignore it.
            if (new HttpException(null, exception).GetHttpCode() != 500)
                return;

            //过滤异步请求
            if (filterContext.HttpContext.Request.Headers.Get("X-Requested-With") != null)
                return;

            if (exception.InnerException != null && exception.InnerException is ExceptionFacade)
                exceptionFacade = exception.InnerException as ExceptionFacade;

            if (exceptionFacade == null)
                return;

            if (!filterContext.HttpContext.IsCustomErrorEnabled)
                return;

            
            filterContext.Result = new RedirectResult(SiteUrls.Instance().SystemMessage(filterContext.Controller.TempData, new SystemMessageViewModel { StatusMessageType = Tunynet.Mvc.StatusMessageType.Error, Title = "出错了", Body = exceptionFacade.Message }));
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            // Certain versions of IIS will sometimes use their own error page when
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

    }
}
