//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Mail;
using Tunynet;
using Tunynet.Common;
using Tunynet.Email;
using System.Linq;
using Tunynet.Utilities;
using System.Text;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邮件Service的扩展类
    /// </summary>
    public static class EmailServiceExtensions
    {
        /// <summary>
        /// 重新加载全部的Smtp设置
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SmtpSettings> ReLoadSmtpSettings(this EmailService service)
        {
            ISmtpSettingsRepository smtpSettingsRepository = new SmtpSettingsRepository();
            IEnumerable<SmtpSettings> smtpSettings = smtpSettingsRepository.GetAll();
            EmailService.AllSmtpSettings = smtpSettings == null ? new List<SmtpSettings>() : smtpSettings.Select(n => new SmtpSettingsChild(n) as SmtpSettings).ToList();
            return EmailService.AllSmtpSettings;
        }

        /// <summary>
        /// 获取单个的Smtp设置
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SmtpSettings GetSmtpSettings(this EmailService service, long id)
        {
            ISmtpSettingsRepository smtpSettingsRepository = new SmtpSettingsRepository();
            return smtpSettingsRepository.Get(id);
        }

        /// <summary>
        /// 删除一条Smtp设置
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteSmtpSettings(this EmailService service, long id)
        {
            ISmtpSettingsRepository smtpSettingsRepository = new SmtpSettingsRepository();
            return smtpSettingsRepository.DeleteByEntityId(id);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="smtpSettings"></param>
        public static void SaveSmtpSetting(this EmailService service, SmtpSettings smtpSettings)
        {
            ISmtpSettingsRepository smtpSettingsRepository = new SmtpSettingsRepository();
            if (smtpSettings.Id > 0)
                smtpSettingsRepository.Update(smtpSettings);
            else
                smtpSettingsRepository.Insert(smtpSettings);
        }

        /// <summary>
        /// 获取全部的Smtp设置
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IEnumerable<SmtpSettings> GetAll(this EmailService service)
        {
            ISmtpSettingsRepository smtpSettingsRepository = new SmtpSettingsRepository();
            return smtpSettingsRepository.GetAll();
        }


        public static bool TestStmpSettings(this EmailService service, long id, out string errorMessage)
        {
            SmtpSettings smtpSettings = service.GetSmtpSettings(id);

            if (smtpSettings == null)
            {
                errorMessage = "找不到对应的Smtp设置";
                return false;
            }

            errorMessage = string.Empty;

            //获取SmtpClient
            SmtpClient smtpClient = null;

            bool isSuccess = true;
            try
            {
                smtpClient = GetSmtpClient(smtpSettings);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                isSuccess = false;
            }

            //若SmtpClient获取成功，发送邮件
            if (isSuccess && smtpClient != null)
            {
                try
                {
                    MailMessage mail = GetTestEmail(smtpSettings);
                    smtpClient.SendAsync(mail, string.Empty);
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 获取用来测试的Email
        /// </summary>
        /// <param name="settings">Smtp设置</param>
        /// <returns></returns>
        private static MailMessage GetTestEmail(SmtpSettings settings)
        {
            MailMessage mail = new MailMessage();
            mail.Body = string.Format("测试邮件，如果您能够看到此封邮件，则证明您邮箱 {0} 配置是可用的", settings.UserEmailAddress);
            mail.BodyEncoding = Encoding.UTF8;
            mail.From = new System.Net.Mail.MailAddress(settings.UserEmailAddress);
            mail.IsBodyHtml = false;
            mail.Sender = new System.Net.Mail.MailAddress(settings.UserEmailAddress);
            mail.Subject = "测试邮件";
            mail.SubjectEncoding = Encoding.UTF8;
            mail.To.Add(settings.UserEmailAddress);
            return mail;
        }

        /// <summary>
        /// 获取StmpClient
        /// </summary>
        private static SmtpClient GetSmtpClient(SmtpSettings smtpSettings = null)
        {
            SmtpClient client = null;
            IEmailSettingsManager emailSettingsManager = DIContainer.Resolve<IEmailSettingsManager>();
            EmailSettings settings = emailSettingsManager.Get();

            if (smtpSettings == null)
                smtpSettings = settings.SmtpSettings;

            client = new SmtpClient(smtpSettings.Host, smtpSettings.Port);
            client.EnableSsl = smtpSettings.EnableSsl;

            //for SMTP Authentication
            if (smtpSettings.RequireCredentials)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, smtpSettings.Password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
            }

            return client;
        }
    }
}