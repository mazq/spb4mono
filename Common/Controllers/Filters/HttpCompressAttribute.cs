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
using System.IO.Compression;
using System.Configuration;

namespace SpaceBuilder.Common
{
    /// <summary>
    /// 使用Gzip,Deflate压缩
    /// </summary>
    public class HttpCompressAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            if (!IsEnable)
                return;

            if (filterContext.IsChildAction)
                return;

            if (filterContext.Exception != null || filterContext.HttpContext.Error != null)
                return;


            HttpRequestBase request = filterContext.HttpContext.Request;
            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding))
                return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();
            HttpResponseBase response = filterContext.HttpContext.Response;

            //if (response.IsRequestBeingRedirected)
            //    return;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }

        /// <summary>
        /// 是否启用HttpCompress
        /// </summary>
        private bool IsEnable
        {
            get
            {
                bool enableHttpCompress = false;
                bool.TryParse(ConfigurationManager.AppSettings.Get("HttpCompressEnabled"), out enableHttpCompress);                    
                return enableHttpCompress;
            }
        }
    }
}
