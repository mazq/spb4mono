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
using System.Text.RegularExpressions;
using System.Xml;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 126邮箱联系人管理器
    /// </summary>
    public class Email126ContactAccessor : IEmailContactAccessor
    {
        /// <summary>
        /// 邮箱后缀
        /// </summary>
        public string EmailDomainName
        {
            get { return "126.com"; }
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
            Dictionary<string, string> contacts = new Dictionary<string, string>();

            try
            {
                //en = type;
                //记录登陆邮箱的用户名和密码
                StringBuilder sb = new StringBuilder();
                sb.Append("&domain=126.com");//域
                sb.Append("&language=-1");//语言
                sb.Append("&bCookie=");
                sb.Append("&username=" + userName);
                sb.Append("&savelogin=");
                sb.Append("url2=http://mail.126.com/errorpage/err_126.htm");
                sb.Append("&user=" + userName.Substring(0, userName.IndexOf("@")));//登录名
                sb.Append("&password=" + password);//密码
                sb.Append("&style=-1"); //样式
                sb.Append("&secure=");
                b = Encoding.ASCII.GetBytes(sb.ToString());

                contacts = getContact(userName, password);
                isSuccessLogin = true;
            }
            catch (Exception)
            {
                contacts = new Dictionary<string, string>();
                isSuccessLogin = false;
            }

            return contacts;
        }

        //邮箱入口定义
        const string mail126 = "http://reg.163.com/login.jsp?type=1&product=mail126&url=http://entry.mail.126.com/cgi/ntesdoor?hid%3D10010102%26lightweight%3D1%26verifycookie%3D1%26language%3D0%26style%3D-1";

        /// <summary>
        /// 定义公共的 Cookie Header 变量
        /// </summary>
        protected static string cookieheader = string.Empty;

        /// <summary>
        /// 定义下次访问的Url变量
        /// </summary>
        protected static string NextUrl = string.Empty;
        CookieContainer cookieCon = new CookieContainer();

        Regex reg;
        Match m;
        byte[] b;

        /// <summary>
        /// 得到网页数据
        /// </summary>
        /// <returns>得到网页HTML数据</returns>
        private string GetHtml(string userName, string password)
        {
            string EntryUrl = GetEntryUrl(userName, password);
            return Process126mail(EntryUrl, userName);
        }

        /// <summary>
        /// 分析126
        /// </summary>
        /// <param name="EntryUrl">解析地址</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        private string Process126mail(string EntryUrl, string userName)
        {
            try
            {
                #region//第一次请求登陆地址
                //第一请求126登陆邮箱地址 "http://reg.163.com/login.jsp?type=1&product=mail126&url=http://entry.mail.126.com/cgi/ntesdoor?hid%3D10010102%26lightweight%3D1%26verifycookie%3D1%26language%3D0%26style%3D-1";
                //获取请求的内容
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(new Uri(EntryUrl));
                hwr.Method = "POST";
                hwr.KeepAlive = false;
                hwr.ContentType = "application/x-www-form-urlencoded";
                hwr.ContentLength = b.Length;
                hwr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; GTB6; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                hwr.CookieContainer = cookieCon;

                //// 发送数据
                using (Stream s = hwr.GetRequestStream())
                {
                    s.Write(b, 0, b.Length);
                }
                string rb = string.Empty;

                // 获取返回信息
                using (HttpWebResponse wr = (HttpWebResponse)hwr.GetResponse())
                {
                    StreamReader sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    rb = sr.ReadToEnd();
                    sr.Close();
                }

                reg = new Regex(@"HTTP-EQUIV=REFRESH CONTENT=""[\d];URL=(.+?)"">");
                m = reg.Match(rb.ToUpper());
                if (m.Success)
                {
                    EntryUrl = m.Groups[1].Value.ToLower();
                }

                EntryUrl = TransCode(EntryUrl);

                //把用户名和密码添加到头部cookies
                Uri nurl1 = new Uri("http://www.163.com");
                Uri nurl2 = new Uri("http://reg.163.com");
                Uri nurl3 = new Uri(EntryUrl);
                foreach (System.Net.Cookie cookie in cookieCon.GetCookies(nurl1))
                {
                    cookie.Domain = nurl3.Host;
                }
                cookieCon.Add(cookieCon.GetCookies(nurl1));

                foreach (System.Net.Cookie cookie in cookieCon.GetCookies(nurl2))
                {
                    cookie.Domain = nurl3.Host;
                }
                cookieCon.Add(cookieCon.GetCookies(nurl2));

                foreach (System.Net.Cookie cookie in cookieCon.GetCookies(nurl3))
                {
                    cookie.Domain = ".126.com";
                    cookie.Expires = DateTime.Now.AddHours(1);
                }
                cookieCon.Add(cookieCon.GetCookies(nurl3));
                #endregion

                //忽略第二次请求新地址

                #region//第三次请求新地址
                //根据第二个地址获取第三个地址

                string rUrl = string.Empty;
                string threrrUrl = "http://entry.mail.126.com/cgi/ntesdoor?username=" + userName + "&hid=10010102&lightweight=1&verifycookie=1&language=0&style=-1";
                hwr = (HttpWebRequest)HttpWebRequest.Create(new Uri(threrrUrl));
                hwr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; GTB6; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                hwr.CookieContainer = cookieCon;
                hwr.Method = "HEAD";
                hwr.AllowAutoRedirect = false;

                // 获取返回信息
                using (HttpWebResponse wr = (HttpWebResponse)hwr.GetResponse())
                {
                    rUrl = wr.GetResponseHeader("Location");   //获取到rurl http://cwebmail.mail.126.com/js4/main.jsp?sid=WBGSoZlBkEKZafrfivBBqbkxsHBowIPx
                }
                #endregion

                #region////第四次请求新地址获得数据
                //根据第三个地址获取到http://cwebmail.mail.126.com/js4/s?sid=WBGSoZlBkEKZafrfivBBqbkxsHBowIPx&func=global:sequential&from=nav&action=showHideFolder&showAd=false&userType=newuser&uid=xxxx@sina.com

                //然后请求分析获取联系人存为XML数据
                string post = "<?xml version=\"1.0\"?><object><array name=\"items\"><object><string name=\"func\">pab:searchContacts</string><object name=\"var\"><array name=\"order\"><object><string name=\"field\">FN</string><boolean name=\"desc\">false</boolean><boolean name=\"ignoreCase\">true</boolean></object></array></object></object><object><string name=\"func\">pab:getAllGroups</string></object></array></object>";
                byte[] pb = Encoding.ASCII.GetBytes(post.ToString());
                string bookurl = (rUrl + "&func=global:sequential").Replace("js3", "a");
                hwr = (HttpWebRequest)HttpWebRequest.Create(new Uri(bookurl.Replace("main.jsp", "s")));
                hwr.Method = "POST";
                hwr.ContentType = "application/xml";
                hwr.ContentLength = pb.Length;
                hwr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; GTB6; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                hwr.CookieContainer = cookieCon;

                // 发送数据
                using (Stream s = hwr.GetRequestStream())
                {
                    s.Write(pb, 0, pb.Length);
                }

                // 获取返回信息
                using (HttpWebResponse wr = (HttpWebResponse)hwr.GetResponse())
                {
                    StreamReader sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    rb = sr.ReadToEnd();
                    sr.Close();
                }
                #endregion

                return rb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //正则过滤
        private static string TransCode(string str)
        {
            Regex r = new Regex(@"&#([\d]{1,5})", RegexOptions.None);
            StringBuilder s = new StringBuilder();

            foreach (Match m in r.Matches(str))
            {
                char c = (char)int.Parse(m.Groups[1].Value);
                s.Append(c.ToString());
            }
            return s.ToString();
        }
        /// <summary>
        /// 得到126通讯录的内容
        /// </summary>
        /// <returns>通讯录集合</returns>
        private Dictionary<string, string> getContact(string userName, string password)
        {
            //List<Person> ls = new List<Person>();
            Dictionary<string, string> dicContact = new Dictionary<string, string>();
            //读取XML数据然后进行    选择匹配筛选出来匹配的邮箱
            string resHtml = Encoding.UTF8.GetString(Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(this.GetHtml(userName, password))));
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
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
            }
            return dicContact;
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
            string EntryUrl = string.Empty;
            EntryUrl = mail126;
            return string.Format(EntryUrl, userName, password);
        }
    }
}