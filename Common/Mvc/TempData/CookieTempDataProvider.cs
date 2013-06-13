//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 使用cookie存储TempData
    /// </summary>
    public class CookieTempDataProvider : ITempDataProvider
    {
        internal const string TempDataCookieKey = "__ControllerTempData";
        HttpContextBase _httpContext;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="httpContext"></param>
        public CookieTempDataProvider(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            _httpContext = httpContext;
        }
        /// <summary>
        /// http上下文
        /// </summary>
        public HttpContextBase HttpContext
        {
            get
            {
                return _httpContext;
            }
        }
        /// <summary>
        /// 加载TempData
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            HttpCookie cookie = _httpContext.Request.Cookies[TempDataCookieKey];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                IDictionary<string, object> deserializedDictionary = Base64StringToDictionary(cookie.Value);

                cookie.Expires = DateTime.MinValue;
                cookie.Value = string.Empty;

                if (_httpContext.Response != null && _httpContext.Response.Cookies != null)
                {
                    HttpCookie responseCookie = _httpContext.Response.Cookies[TempDataCookieKey];
                    if (responseCookie != null)
                    {
                        cookie.Expires = DateTime.MinValue;
                        cookie.Value = string.Empty;
                    }
                }

                return deserializedDictionary;
            }

            return new Dictionary<string, object>();
        }
        /// <summary>
        /// 保存TempData
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="values"></param>
        protected virtual void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            string cookieValue = DictionaryToBase64String(values);

            var cookie = new HttpCookie(TempDataCookieKey);
            cookie.HttpOnly = true;
            cookie.Value = cookieValue;

            _httpContext.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 将base64字符串转为字典
        /// </summary>
        /// <param name="base64EncodedSerializedTempData"></param>
        /// <returns></returns>
        public static IDictionary<string, object> Base64StringToDictionary(string base64EncodedSerializedTempData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedSerializedTempData);
            using (var memStream = new MemoryStream(bytes))
            {
                var binFormatter = new BinaryFormatter();
                return binFormatter.Deserialize(memStream, null) as IDictionary<string, object>;
            }
        }
        /// <summary>
        /// 将字典转为base64字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string DictionaryToBase64String(IDictionary<string, object> values)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Seek(0, SeekOrigin.Begin);
                var binFormatter = new BinaryFormatter();
                binFormatter.Serialize(memStream, values);
                memStream.Seek(0, SeekOrigin.Begin);
                byte[] bytes = memStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }
        /// <summary>
        /// 实现加载TempData方法
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        IDictionary<string, object> ITempDataProvider.LoadTempData(ControllerContext controllerContext)
        {
            return LoadTempData(controllerContext);
        }

        /// <summary>
        /// 实现保存TempData方法
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        void ITempDataProvider.SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            SaveTempData(controllerContext, values);
        }
    }
}
