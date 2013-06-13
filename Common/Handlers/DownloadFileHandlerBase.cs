//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 下载文件Handler基类
    /// </summary>
    public abstract class DownloadFileHandlerBase : IHttpHandler
    {
        //输出附件时缓冲区大小
        protected static readonly int BufferLength = 32 * 1024;

        /// <summary>
        /// 是否已经在客户端缓存
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lastModified">必须是utc时间</param>
        /// <returns></returns>
        protected static bool IsCacheOK(HttpContext context, DateTime lastModified)
        {
            DateTime lastModifiedInClient = DateTime.MinValue;

            #region If-Modified-Since(Last-Modified)
            string ifModifiedSince = context.Request.Headers["If-Modified-Since"];
            if (!string.IsNullOrEmpty(ifModifiedSince))
            {
                if (DateTime.TryParse(ifModifiedSince, out lastModifiedInClient))
                {
                    if (Math.Abs(((TimeSpan)lastModified.Subtract(lastModifiedInClient.ToUniversalTime())).TotalSeconds) <= 1)
                    {
                        return true;
                    }
                }
            }
            #endregion

            #region If-None-Match(ETag)
            string etagInClient = context.Request.Headers["If-None-Match"];
            if (!string.IsNullOrEmpty(etagInClient) && (etagInClient == lastModified.Ticks.ToString()))
            {
                return true;
            }

            #endregion

            return false;
        }


        /// <summary>
        /// 设置ContentType以及缓存
        /// </summary>
        protected void SetResponsesDetails(HttpContext context, string contentType, string fileName, DateTime lastModified)
        {
            contentType = contentType.ToLower();
            //string disposition;
            if (contentType.IndexOf("image") > -1 || contentType.IndexOf("application/x-shockwave-flash") > -1 || contentType.IndexOf("audio/x-pn-realaudio-plugin") > -1 || contentType.IndexOf("video/x-ms-wmv") > -1 || contentType.IndexOf("video/quicktime") > -1)
            {
                context.Response.ContentType = contentType;
            }
            else
            {
                //解决下载中文文件和特殊字符文件名称乱码的问题 By小蔡 
                context.Response.ContentType = contentType;
                //当客户端使用IE时，对其进行编码；使用 ToHexString 代替传统的 UrlEncode()
                if (context.Request.UserAgent.ToLower().IndexOf("msie") > -1)
                    fileName = ToHexString(fileName);

                //为了向客户端输出空格，需要在当客户端使用 Firefox 时特殊处理
                if (context.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                else
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            }

            // Browser cache settings
            context.Response.Cache.SetCacheability(HttpCacheability.Private);
            context.Response.Cache.SetLastModified(lastModified);
            context.Response.Cache.SetETag(lastModified.Ticks.ToString());
            context.Response.Cache.SetAllowResponseInBrowserHistory(true);
        }

        #region IHttpHandler 成员

        /// <exclude/>
        public virtual bool IsReusable
        {
            get { return true; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
        }

        #endregion

        #region 文件名编码
        /// <summary>
        /// Encodes non-US-ASCII characters in a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Determines if the character needs to be encoded.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";

            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Encodes a non-US-ASCII character.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }

            return builder.ToString();
        }

        #endregion
    }
}
