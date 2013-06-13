//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 163邮箱联系人管理器
    /// </summary>
    public class Email163ContactAccessor : IEmailContactAccessor
    {
        /// <summary>
        /// 邮箱后缀
        /// </summary>
        public string EmailDomainName
        {
            get { return "163.com"; }
        }

        /// <summary>
        /// 获取邮箱联系人
        /// </summary>
        /// <param name="userName">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="isSuccessLogin">是否成功登录</param>
        /// <returns>Key:联系人Email地址，Value：联系人名称</returns>
        public Dictionary<string, string> GetContacts(string userName, string password, out bool isSuccessLogin)
        {
            Dictionary<string, string> contact = new Dictionary<string, string>();
            try
            {
                contact = getContact(userName, password);
                isSuccessLogin = true;
            }
            catch (Exception)
            {
                contact = new Dictionary<string, string>();
                isSuccessLogin = false;
            }
            return contact;
        }

        //邮箱入口定义
        const string mail_163_com = "https://reg.163.com/logins.jsp?username={0}&password={1}&type=1&url=http://entry.mail.163.com/coremail/fcg/ntesdoor2?lightweight%3D1%26verifycookie%3D1%26language%3D-1%26style%3D-1";   //判断用户名和密码是否为空/正确

        /// <summary>
        /// 定义公共的 Cookie Header 变量
        /// </summary>
        protected static string cookieheader = string.Empty;    //定义公共的 Cookie Header 变量

        /// <summary>
        /// 定义下次访问的url变量
        /// </summary>
        protected static string NextUrl = string.Empty;   //定义下次访问的Url变量
        CookieContainer cookieCon = new CookieContainer();
        HttpWebRequest req;
        HttpWebResponse res;

        /// <summary>
        /// 得到网页数据
        /// </summary>
        /// <returns>得到网页HTML数据</returns>
        private string GetHtml(string userName, string password)
        {
            string EntryUrl = GetEntryUrl(userName, password);
            string html = string.Empty;
            try
            {
                html = Process163mail(EntryUrl, userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return html;
        }
        /// <summary>
        /// 分析163
        /// </summary>
        /// <param name="EntryUrl">解析地址</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        private string Process163mail(string EntryUrl, string userName)
        {
            try
            {
                //#1
                string ReturnHtml = GetRequestHtml(EntryUrl, Encoding.GetEncoding("utf-8"));
                NextUrl = ReturnHtml.Substring(ReturnHtml.IndexOf("URL=") + 6);
                NextUrl = NextUrl.Substring(0, NextUrl.IndexOf("\""));

                string[] arr = NextUrl.Split(new string[] { "&#" }, StringSplitOptions.RemoveEmptyEntries);
                string str1 = string.Empty;
                for (int i = 0; i < arr.Length; i++)
                {
                    int j = int.Parse(arr[i]);
                    str1 += Encoding.ASCII.GetString(new byte[] { (byte)j });//str1
                }
                NextUrl = str1;

                //忽略第二次请求地址

                //NextUrl = str2;  
                //#3
                NextUrl = "http://entry.mail.163.com/coremail/fcg/ntesdoor2?lightweight=1&verifycookie=1&language=-1&style=35&username=" + userName.Replace("@163.com", "");
                ReturnHtml = GetRequestHtml(NextUrl, Encoding.GetEncoding("gb2312"));
                //改向到 http://cg1a181.mail.163.com/js3/main.jsp?sid=eELXghnSsySYEzpNbLSSrMNUaUSOCRib 由服务器造成
                string post = "<?xml version=\"1.0\"?><object><array name=\"items\"><object><string name=\"func\">pab:searchContacts</string><object name=\"var\"><array name=\"order\"><object><string name=\"field\">FN</string><boolean name=\"ignoreCase\">true</boolean></object></array></object></object><object><string name=\"func\">user:getSignatures</string></object><object><string name=\"func\">pab:getAllGroups</string></object></array></object>";
                byte[] pb = Encoding.ASCII.GetBytes(post.ToString());

                NextUrl = res.ResponseUri.AbsoluteUri;

                string bookurl = (NextUrl + "&func=global:sequential").Replace("js3", "a");
                req = (HttpWebRequest)HttpWebRequest.Create(new Uri(bookurl.Replace("main.jsp", "s")));
                req.Method = "POST";
                req.ContentType = "application/xml";
                req.ContentLength = pb.Length;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; GTB6; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                req.CookieContainer = cookieCon;

                // 发送数据
                using (Stream s = req.GetRequestStream())
                {
                    s.Write(pb, 0, pb.Length);
                }

                // 获取返回信息
                using (HttpWebResponse wr = (HttpWebResponse)req.GetResponse())
                {
                    StreamReader sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    ReturnHtml = sr.ReadToEnd();
                    sr.Close();
                }
                return ReturnHtml;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 提交到服务器
        /// </summary>
        /// <param name="URL">要提交的URL</param>
        /// <returns>返回HTML内容</returns>
        private string GetRequestHtml(string URL)
        {
            return GetRequestHtml(URL, Encoding.Default);
        }
        private string GetRequestHtml(string URL, Encoding EnCodeing)
        {
            return GetRequestHtml(URL, EnCodeing, ReqMethod.POST);//Encoding.Default
        }
        private string GetRequestHtml(string URL, Encoding EnCodeing, ReqMethod RMethod)
        {
            string html = string.Empty;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(URL);
                req.AllowAutoRedirect = true;
                req.CookieContainer = cookieCon;
                req.Credentials = CredentialCache.DefaultCredentials;
                req.Method = RMethod.ToString();
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; GTB5; Mozilla/4.0(Compatible Mozilla/4.0(Compatible-EmbeddedWB 14.59 http://bsalsa.com/ EmbeddedWB- 14.59  from: http://bsalsa.com/ ; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; InfoPath.2; .NET CLR 3.5.30729; .NET CLR 3.0.30618; CIBA)";

                res = (HttpWebResponse)req.GetResponse();

                if (cookieheader.Equals(string.Empty))
                {
                    cookieheader = req.CookieContainer.GetCookieHeader(new Uri(URL));
                }
                else
                {
                    req.CookieContainer.SetCookies(new Uri(URL), cookieheader);
                }

                html = new StreamReader(res.GetResponseStream(), EnCodeing).ReadToEnd();
            }
            catch (Exception ex)
            {
                html = ex.Message;
            }
            return html;
        }

        /// <summary>
        /// 得到163通讯录的内容
        /// </summary>
        /// <returns>通讯录集合</returns>
        public Dictionary<string, string> getContact(string userName, string password)
        {
            //List<Person> ls = new List<Person>();
            Dictionary<string, string> dicContact = new Dictionary<string, string>();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                //读取XML数据然后进行    选择匹配筛选出来匹配的邮箱
                string resHtml = Encoding.UTF8.GetString(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(this.GetHtml(userName, password))));
                xmlDoc.LoadXml(resHtml);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            XmlNodeList xnl = xmlDoc.SelectNodes("/result/array/object");
            if (xnl == null || xnl.Count <= 0)
                return dicContact;

            XmlNodeList linkNOdes = xnl[0].SelectNodes("array/object");
            foreach (XmlNode linkNode in linkNOdes)
            {
                //Person ps = new Person();
                string name = string.Empty;
                string email = string.Empty;
                foreach (XmlNode xn2 in linkNode.ChildNodes)
                {
                    //取得邮箱地址
                    if (xn2.Attributes["name"].Value == "EMAIL;PREF")
                    {
                        email = xn2.InnerText;
                    }
                    if (xn2.Attributes["name"].Value == "FN")
                    {
                        if (!string.IsNullOrEmpty(xn2.InnerText))
                        {
                            name = xn2.InnerText;
                        }
                        else
                        {
                            name = "暂无名称";
                        }
                    }
                }
                dicContact[email] = name;
                //ls.Add( ps );
            }
            return dicContact;
            //return ls;
        }
        /// <summary>
        /// 枚举获请求用什么方式
        /// </summary>
        private enum ReqMethod
        {
            POST,
            GET
        }

        /// <summary>
        /// 得到请求地址
        /// </summary>
        /// <returns>得到请求地址</returns>
        private string GetEntryUrl(string userName, string password)
        {
            string EntryUrl = mail_163_com;
            return string.Format(EntryUrl, userName, password);
        }
    }
}