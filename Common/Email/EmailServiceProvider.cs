//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Xml;
using Tunynet.Email;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邮件服务器配置
    /// </summary>
    public class EmailServiceProvider
    {

        /// <summary>
        /// 邮件服务器网站主页
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// 邮件服务器地址后缀
        /// </summary>
        /// <example>@163.com</example>
        public string DomainName { get; set; }

        /// <summary>
        /// STMP服务器设置
        /// </summary>
        public SmtpSettings SmtpSettings { get; set; }

       
        /// <summary>
        /// EmailServiceProvider构造器，读取Email.config
        /// </summary>
        /// <param name="xNode">单个节点</param>
        public EmailServiceProvider(XmlNode xNode)
        {
            SiteUrl = xNode.Attributes.GetNamedItem("SiteUrl").InnerText;  //邮件服务器网站主页
            DomainName = xNode.Attributes.GetNamedItem("DomainName").InnerText; //邮件服务器地址 
            if (xNode.HasChildNodes)
            {
                XmlNode smtp = xNode.SelectSingleNode("smtp");
                if (smtp!=null)
                {
                    SmtpSettings = new SmtpSettings();
                    SmtpSettings.Host = smtp.Attributes.GetNamedItem("Host").InnerText;//服务器域名
                    SmtpSettings.Port = Convert.ToInt32(smtp.Attributes.GetNamedItem("Port").InnerText);//端口号
                    SmtpSettings.EnableSsl = Convert.ToBoolean(smtp.Attributes.GetNamedItem("EnableSsl").InnerText);//smtp服务器是否启用ssl
                    SmtpSettings.RequireCredentials = Convert.ToBoolean(smtp.Attributes.GetNamedItem("RequireCredentials").InnerText);//smtp服务器是否需要验证身份
                    SmtpSettings.UserEmailAddress = smtp.Attributes.GetNamedItem("UserEmailAddress").InnerText; //用户邮件地址
                    SmtpSettings.UserName = smtp.Attributes.GetNamedItem("UserName").InnerText;//帐号
                    SmtpSettings.Password = smtp.Attributes.GetNamedItem("Password").InnerText;  //密码
                    SmtpSettings.ForceSmtpUserAsFromAddress = Convert.ToBoolean(smtp.Attributes.GetNamedItem("ForceSmtpUserAsFromAddress").InnerText);//smtp服务器是否强制smtp登录用户作为发件人
                }
            }
        }
    }
}
