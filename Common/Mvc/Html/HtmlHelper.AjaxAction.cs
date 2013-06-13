//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Collections;
using System.Web.Helpers;
using Tunynet.Logging;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对AjaxAction的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperAjaxActionExtensions
    {
        /// <summary>
        /// 异步加载视图
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName">actionName</param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString AjaxAction(this HtmlHelper htmlHelper, string actionName)
        {
            return AjaxAction(htmlHelper, actionName, null, null);
        }

        /// <summary>
        /// 异步加载视图
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString AjaxAction(this HtmlHelper htmlHelper, string actionName, object routeValues)
        {
            return AjaxAction(htmlHelper, actionName, null, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// 异步加载视图
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString AjaxAction(this HtmlHelper htmlHelper, string actionName, RouteValueDictionary routeValues)
        {
            return AjaxAction(htmlHelper, actionName, null, routeValues);
        }

        /// <summary>
        /// 异步加载视图
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString AjaxAction(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            return AjaxAction(htmlHelper, actionName, controllerName, null);
        }

        /// <summary>
        /// 异步加载视图
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString AjaxAction(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues)
        {
            return AjaxAction(htmlHelper, actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// 异步加载视图
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString AjaxAction(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            if (string.IsNullOrEmpty(actionName))
            {
                throw new ArgumentException("参数名称actionName不能为空", "actionName");
            }
            string url = string.Empty;
            try
            {
                url = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues ?? new RouteValueDictionary(), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true /* includeImplicitMvcValues */);
            }
            catch (Exception e)
            {
                LoggerFactory.GetLogger().Log(LogLevel.Error, e, "执行根据ActionName、ControllerName解析Url时发生异常");
                return MvcHtmlString.Empty;
            }
            return AjaxActionHelper(htmlHelper, url);
        }

        /// <summary>
        /// 异步加载路由视图
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static MvcHtmlString AjaxRouteAction(this HtmlHelper htmlHelper, string routeName, object routeValues = null)
        {
            string url = string.Empty;
            try
            {
                url = UrlHelper.GenerateUrl(routeName, null /* actionName */, null /* controllerName */, new RouteValueDictionary(routeValues), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, false /* includeImplicitMvcValues */);
            }
            catch (Exception e)
            {
                LoggerFactory.GetLogger().Log(LogLevel.Error, e, "执行解析路由规则时发生异常");
                return MvcHtmlString.Empty;
            }
            return AjaxActionHelper(htmlHelper, url);
        }

        /// <summary>
        /// 输出局部视图
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static MvcHtmlString AjaxActionHelper(this HtmlHelper htmlHelper, string url)
        {
            TagBuilder builder = new TagBuilder("div");
            Dictionary<string, object> attribute = new Dictionary<string, object>();
            attribute["plugin"] = "ajaxAction";
            attribute["data"] = Json.Encode(new { url = url });
            builder.MergeAttributes(attribute);
            return MvcHtmlString.Create(builder.ToString());
        }

    }
}
