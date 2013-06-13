//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using System.Net;
using System;
using System.Web;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Google短网址实现类
    /// </summary>
    public class GoogleUrlShortner : IUrlShortner
    {
        private const string m_postFormat = "&user=toolbar@google.com&url={0}";

        /// <summary>
        /// 短网址处理
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Shortner(string url)
        {
            string post = String.Format(m_postFormat, HttpUtility.UrlEncode(url));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://goo.gl/api/url");
            request.ServicePoint.Expect100Continue = false;
            request.Method = "POST";
            request.UserAgent = "toolbar";
            request.ContentLength = post.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Cache-Control", "no-cache");

            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                requestStream.Write(postBuffer, 0, postBuffer.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader responseReader = new StreamReader(responseStream))
            {
                string json = responseReader.ReadToEnd();
                GroupCollection groups = Regex.Match(json, "\"short_url\":\"(?<ShrotUrl>[^\"]*)\"").Groups;
                return groups["ShrotUrl"].Value;
            }

        }
    }
}