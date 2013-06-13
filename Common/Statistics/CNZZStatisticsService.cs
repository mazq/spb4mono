//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using Tunynet;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// CNZZ统计类
    /// </summary>
    public class CNZZStatisticsService
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public static bool Enable
        {
            get
            {
                StatisticsAccount account = new CNZZStatisticsService().GetAccount();
                if (account == null)
                    return false;

                SystemDataService systemDataService = new SystemDataService();
                return systemDataService.GetLong("CNZZStatisticsEnable") == 1;
            }
            set
            {
                SystemDataService systemDataService = new SystemDataService();
                long enable = systemDataService.GetLong("CNZZStatisticsEnable");
                if (value)
                    systemDataService.Change("CNZZStatisticsEnable", 1 - enable);
                else
                    systemDataService.Change("CNZZStatisticsEnable", 0 - enable);
            }
        }

        /// <summary>
        /// 获取展示统计页面的连接
        /// </summary>
        /// <returns></returns>
        public string GetCNZZStatisticsPageLink()
        {
            SystemDataService service = new SystemDataService();
            long siteId = service.GetLong("CNZZStatisticsSiteId");
            long password = service.GetLong("CNZZStatisticsPassword");

            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();

            if (siteId <= 0 || password <= 0)
                return string.Empty;

            return string.Format("http://wss.cnzz.com/user/companion/spacebuilder_login.php?site_id={0}&password={1}", siteId, password);
        }

        /// <summary>
        /// 获取统计账户信息
        /// </summary>
        /// <returns></returns>
        public StatisticsAccount GetAccount()
        {
            SystemDataService systemDataService = new SystemDataService();
            StatisticsAccount account = new StatisticsAccount();
            long siteId = systemDataService.GetLong("CNZZStatisticsSiteId");
            long password = systemDataService.GetLong("CNZZStatisticsPassword");

            if (siteId <= 0 || password <= 0)
                return null;

            account.UserName = siteId.ToString();
            account.Password = password.ToString();
            return account;
        }

        /// <summary>
        /// 创建一个新的统计账户
        /// </summary>
        /// <returns></returns>
        public StatisticsAccount CreatNewAccount()
        {
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();

            string key = EncryptionUtility.MD5(siteSettings.MainSiteRootUrl + "A4ba4oqS").ToLower();
            string creatLink = string.Format("http://wss.cnzz.com/user/companion/spacebuilder.php?domain={0}&key={1}", siteSettings.MainSiteRootUrl, key);
            string webMatter = HttpCollects.GetHTMLContent(creatLink, Encoding.GetEncoding("gb2312"), null);

            string errorMessage = null;

            switch (webMatter.Trim())
            {
                case "":
                    errorMessage = "远程站点没有响应";
                    break;

                case "-1":
                    errorMessage = "key输入有误";
                    break;
                case "-2":
                    errorMessage = "该域名长度有误（1~64）";
                    break;
                case "-3":
                    errorMessage = "域名输入有误（比如输入汉字）";
                    break;
                case "-4":
                    errorMessage = "域名插入数据库有误";
                    break;
                case "-5":
                    errorMessage = "同一个IP用户调用页面超过阀值，阀值暂定为10";
                    break;
            }

            if (!string.IsNullOrEmpty(errorMessage))
                throw new Exception("创建统计帐号出错，错误信息：" + errorMessage);

            string siteIdStr = webMatter.Substring(0, webMatter.IndexOf('@'));
            string passwordStr = webMatter.Substring(webMatter.IndexOf('@') + 1);
            long siteId = 0;
            long password = 0;

            StatisticsAccount account = new StatisticsAccount();

            if (long.TryParse(siteIdStr, out siteId) && long.TryParse(passwordStr, out password))
            {
                account.Password = password.ToString();
                account.UserName = siteId.ToString();
                return account;
            }

            return null;
        }

        /// <summary>
        /// 保存统计帐号
        /// <param name="account">被保存的统计帐号</param>
        /// </summary>
        public void SaveStatisticsAccount(StatisticsAccount account)
        {
            if (account == null || string.IsNullOrEmpty(account.Password) || string.IsNullOrEmpty(account.UserName))
                return;

            SystemDataService systemDataService = new SystemDataService();
            long siteId = systemDataService.GetLong("CNZZStatisticsSiteId");
            long password = systemDataService.GetLong("CNZZStatisticsPassword");

            long accountSiteId = 0;
            if (!long.TryParse(account.UserName, out accountSiteId))
                return;

            long accountPassword = 0;
            if (!long.TryParse(account.Password, out accountPassword))
                return;

            if (accountSiteId != siteId)
                systemDataService.Change("CNZZStatisticsSiteId", accountSiteId - siteId);
            if (accountPassword != password)
                systemDataService.Change("CNZZStatisticsPassword", accountPassword - password);

            Enable = true;

            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();

            siteSettings.StatScript = GetStatisticsCode();

            siteSettingsManager.Save(siteSettings);
        }

        /// <summary>
        /// 获取统计代码
        /// </summary>
        /// <returns></returns>
        public string GetStatisticsCode()
        {
            SystemDataService service = new SystemDataService();
            long siteId = service.GetLong("CNZZStatisticsSiteId");
            if (siteId > 0)
                return string.Format("<script src='http://pw.cnzz.com/c.php?id={0}&l=2' language='JavaScript' charset='gb2312'></script>", siteId);

            return string.Empty;
        }

        /// <summary>
        /// 初始化统计信息
        /// </summary>
        public static void Initialize()
        {
            CNZZStatisticsService service = new CNZZStatisticsService();

            StatisticsAccount account = service.GetAccount();
            if (account == null)
                account = service.CreatNewAccount();
            service.SaveStatisticsAccount(account);
        }
    }
}
