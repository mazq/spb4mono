//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 常见邮件服务器配置
    /// </summary>
    public class EmailServiceProviderService
    {
        #region Instance

        private EmailServiceProviderService()
        { }

        private static volatile EmailServiceProviderService _defaultInstance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 获取EmailServiceProviderService实例
        /// </summary>
        public static EmailServiceProviderService Instance()
        {
            if (_defaultInstance == null)
            {
                lock (lockObject)
                {
                    if (_defaultInstance == null)
                    {
                        _defaultInstance = new EmailServiceProviderService();
                    }
                }
            }

            return _defaultInstance;
        }

        #endregion

        /// <summary>
        /// 获取Email.config配置
        /// </summary>
        public Dictionary<string, EmailServiceProvider> GetEmailConfig()
        {
            Dictionary<string, EmailServiceProvider> dictEmailProvider;
            string cacheKey = "EmailServiceProviders";
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            //从缓存中获取Email配置
            dictEmailProvider = cacheService.Get<Dictionary<string, EmailServiceProvider>>(cacheKey);
            if (dictEmailProvider == null)
            {
                dictEmailProvider = new Dictionary<string, EmailServiceProvider>();
                //获取配置文件（一般在Web\Config\Email.Config）
                HttpContext httpContext = HttpContext.Current;
                string filePath = WebUtility.GetPhysicalFilePath("~/Config/Email.config");
                FileInfo fileInfo;
                //读取配置文件，并组装dictEmailProvider
                try
                {
                    fileInfo = new FileInfo(filePath);
                }
                catch
                {
                    throw new ApplicationException("Email.config配置文件找不到");
                }
                FileStream reader = fileInfo.OpenRead();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(reader);
                reader.Close();
                //逐个读取Email.config中emailServiceProvider节点，找出address相同的节点，组装成EmailServiceProvider
                foreach (XmlNode node in xmlDocument.GetElementsByTagName("emailServiceProvider"))
                {
                    if (node != null)
                    {
                        string domainNameValue = node.Attributes.GetNamedItem("DomainName").InnerText; //读取服务器地址
                        EmailServiceProvider emailServerProvider = new EmailServiceProvider(node);
                        dictEmailProvider[domainNameValue] = emailServerProvider;
                    }
                }
                cacheService.Add(cacheKey, dictEmailProvider, CachingExpirationType.Stable);  //放入缓存
            }
            return dictEmailProvider;
        }


        /// <summary>
        /// 根据Email获取邮件服务器网站主页      
        /// </summary>
        /// <param name="emailAddess">email地址</param>
        public string GetEmailSiteUrl(string emailAddess)
        {
            string siteUrl = string.Empty;
            int emailIndex = emailAddess.IndexOf("@") + 1;
            string domainName = emailAddess.Substring(emailIndex);//取出email后缀
            Dictionary<string, EmailServiceProvider> dictionaryEmailService = GetEmailConfig();
            if (dictionaryEmailService != null && dictionaryEmailService.ContainsKey(domainName))
                siteUrl = dictionaryEmailService[domainName].SiteUrl;//获取邮件服务器的网站主页
            return siteUrl;

        }



    }
}
