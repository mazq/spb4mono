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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 发送邮件成功的页面
    /// </summary>
    [Serializable]
    public class SendEmailSucceedViewModel
    {
        public SendEmailSucceedViewModel()
        {
            this.sendStatus = SendStatus.Ok;
        }

        /// <summary>
        /// 成功页面的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 成功页面的主要部分
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 成功页面的描述部分
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户的邮箱
        /// </summary>
        public string AccountEmail { get; set; }

        /// <summary>
        /// 按钮的描述
        /// </summary>
        public string ButtonDescription { get; set; }

        /// <summary>
        /// 链接的描述
        /// </summary>
        public string LinkDescription { get; set; }

        /// <summary>
        /// 链接的地址
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// 用来查看邮箱链接的位置
        /// </summary>
        public string EmailLink { get; set; }

        /// <summary>
        /// 发送邮件状态
        /// </summary>
        public SendStatus sendStatus { get; set; }
    }

    /// <summary>
    /// 发送邮件成功Model的扩展方法
    /// </summary>
    public static class SendEmailSucceedViewModelFactory
    {
        /// <summary>
        /// 获取注册默认发送邮件成功提示
        /// </summary>
        /// <param name="accountEmail">用户的邮箱</param>
        /// <returns>默认SendEmailSucceedViewModel</returns>
        public static SendEmailSucceedViewModel GetRegisterSendEmailSucceedViewModel(string accountEmail)
        {
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            
            IUser user = new UserService().FindUserByEmail(accountEmail);
            string token = null;
            if (user != null)
                token = Utility.EncryptTokenForValidateEmail(0.004, user.UserId);
            return new SendEmailSucceedViewModel
            {
                AccountEmail = accountEmail,
                Body = string.Format("邮箱确认邮件已经发送到[{0}]，点击邮件里的确认链接即可登录[{1}]", accountEmail, siteSettings.SiteName),
                ButtonDescription = "立即查看邮箱",
                Description = "如未收到可以{0}",
                link = SiteUrls.Instance()._ActivateByEmail(accountEmail, token),
                LinkDescription = "重新发送",
                Title = "马上激活邮件，完成注册吧！",
                EmailLink = EmailServiceProviderService.Instance().GetEmailSiteUrl(accountEmail)
            };
        }

        /// <summary>
        /// 获取忘记密码的ViewModl
        /// </summary>
        /// <param name="accountEmail">用户的邮箱</param>
        /// <returns>默认SendEmailSucceedViewModel</returns>
        public static SendEmailSucceedViewModel GetFindPasswordSendEmailSucceedViewModel(string accountEmail)
        {
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            return new SendEmailSucceedViewModel
            {
                AccountEmail = accountEmail,
                Body = "获取密码的信息已经发送到您的邮箱，请到您的邮箱中获取",
                ButtonDescription = "立即查看邮箱",
                Description = "如未收到可以{0}",
                link = SiteUrls.Instance().FindPassword(accountEmail, true),
                LinkDescription = "重新发送",
                Title = "信息已经发送到你的邮箱！",
                EmailLink = EmailServiceProviderService.Instance().GetEmailSiteUrl(accountEmail)
            };
        }
    }

    public enum SendStatus
    {
        Ok = 1, No = 2
    }
}
