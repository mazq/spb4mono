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
using System.ComponentModel;
using System.Web.Routing;

namespace Spacebuilder.Common
{
    /// <summary>
    /// ControllerContext扩展方法
    /// </summary>
    public static class RequestContextExtension
    {
        /// <summary>
        /// 从RouteData或QueryString中获取参数 
        /// </summary>
        /// <param name="requestContext">RequestContext</param>
        /// <param name="key">参数名称</param>
        /// <returns>字符串类型的参数值</returns>
        public static string GetParameterFromRouteDataOrQueryString(this RequestContext requestContext, string key)
        {
            if (requestContext.RouteData.Values != null && requestContext.RouteData.Values.ContainsKey(key))
            {
                object resultValue = null;
                if (requestContext.RouteData.Values.TryGetValue(key, out resultValue) && resultValue != null)
                {
                    return resultValue.ToString();//.Replace('<', ' ').Replace('>', ' ').Replace("%3C", "&lt;").Replace("%3E", "&gt;");
                }
            }

            return requestContext.HttpContext.Request.QueryString[key];
        }


        /// <summary>
        /// 从RouteData或QueryString中获取参数 
        /// </summary>
        /// <param name="requestContext">RequestContext</param>
        /// <param name="key">参数名称</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>字符串类型的参数值</returns>
        public static int GetParameterFromRouteDataOrQueryString(this RequestContext requestContext, string key, int defaultValue)
        {
            int returnValue = -1;
            if (requestContext.RouteData.Values != null && requestContext.RouteData.Values.ContainsKey(key))
            {
                object resultObject = null;
                if (requestContext.RouteData.Values.TryGetValue(key, out resultObject) && resultObject != null)
                {
                    if (int.TryParse(resultObject.ToString(), out returnValue))
                        return returnValue;
                }
            }

            if (int.TryParse(requestContext.HttpContext.Request.QueryString[key], out returnValue))
                return returnValue;

            return defaultValue;
        }

        /// <summary>
        /// 从RouteData或QueryString中获取参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestContext">RequestContext</param>
        /// <param name="key">参数名称</param>
        public static T GetParameterFromRouteDataOrQueryString<T>(this RequestContext requestContext, string key)
        {
            if (requestContext.RouteData.Values != null && requestContext.RouteData.Values.ContainsKey(key))
            {
                object resultValue = null;
                requestContext.RouteData.Values.TryGetValue(key, out resultValue);
                if (resultValue != null)
                {
                    Type tType = typeof(T);
                    if (tType.IsInterface || tType.IsClass)
                    {
                        if (resultValue is T)
                            return (T)resultValue;
                        else
                            return default(T);
                    }
                    else if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (string.IsNullOrEmpty(resultValue.ToString()))
                            return default(T);
                        return (T)TypeDescriptor.GetConverter(Nullable.GetUnderlyingType(tType)).ConvertFrom(resultValue);                        
                    }
                    else if (tType.IsEnum)
                    {
                        return (T)Enum.Parse(tType, resultValue.ToString());
                    }
                    else
                    {
                        return (T)Convert.ChangeType(resultValue, tType);
                    }
                }
            }

            string temp = requestContext.HttpContext.Request.QueryString[key];
            if (string.IsNullOrEmpty(temp) || temp.Trim().Length == 0)
            {
                return default(T);
            }

            Type type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return (T)Convert.ChangeType(temp, Nullable.GetUnderlyingType(type));
            }
            else if (type.IsEnum)
            {
                return (T)Enum.Parse(type, temp);
            }
            else
            {
                return (T)Convert.ChangeType(temp, typeof(T));
            }
        }
    }
}