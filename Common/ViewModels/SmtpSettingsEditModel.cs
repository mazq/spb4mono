using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Email;
using Tunynet;
using System.ComponentModel;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邮件设置
    /// </summary>
    public class SmtpSettingsEditModel
    {
        /// <summary>
        /// 当前设置的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// smtp服务器的域名或IP
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// smtp服务器端口号
        /// </summary>
        [RegularExpression(@"[\d]+", ErrorMessage = "端口号必须是数字")]
        [Required(ErrorMessage = "请输入Smtp端口号")]
        public int Port { get; set; }

        /// <summary>
        /// smtp服务器是否启用ssl
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// smtp服务器是否需要验证身份
        /// </summary>
        public bool RequireCredentials { get; set; }

        /// <summary>
        /// 登录smtp服务器的用户名,可以不带 @后的域名部分
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录smtp服务器的用户邮件地址（可能与UserName，也可能不同）
        /// </summary>
        [Required(ErrorMessage = "请输入用户邮箱地址")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "请输入正确邮箱")]
        public string UserEmailAddress { get; set; }

        /// <summary>
        /// 登录smtp服务器的密码
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱密码")]
        public string Password { get; set; }

        private bool forceSmtpUserAsFromAddress;

        /// <summary>
        /// 强制smtp登录用户作为发件人
        /// </summary>
        public bool ForceSmtpUserAsFromAddress
        {
            get { return forceSmtpUserAsFromAddress; }
            set { forceSmtpUserAsFromAddress = value; }
        }

        private int dailyLimit = 30;

        /// <summary>
        /// 每日发送邮件上限（如果超过此上限，则将不再尝试使用此邮箱发送邮件）
        /// </summary>
        [Description("该邮箱在一天以内被允许发送邮件的最大数")]
        public int DailyLimit
        {
            get { return dailyLimit; }
            set { dailyLimit = value; }
        }

        public bool IsValidate
        {
            get
            {
                if (string.IsNullOrEmpty(this.Host))
                    return false;

                if (string.IsNullOrEmpty(this.Password))
                    return false;

                if (this.Port < 0)
                    return false;

                if (string.IsNullOrEmpty(this.UserEmailAddress))
                    return false;

                if (string.IsNullOrEmpty(this.UserName))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// 转换为数据存储实体
        /// </summary>
        /// <returns></returns>
        public SmtpSettings AsSmtpSettings()
        {
            SmtpSettings settings = new SmtpSettings();
            if (this.Id > 0)
                settings = new EmailService().GetSmtpSettings(this.Id);

            settings.Host = this.Host;
            settings.DailyLimit = this.DailyLimit;
            settings.EnableSsl = this.EnableSsl;
            settings.ForceSmtpUserAsFromAddress = this.ForceSmtpUserAsFromAddress;
            settings.Host = this.Host;
            settings.Password = this.Password;
            settings.Port = this.Port;
            settings.RequireCredentials = this.RequireCredentials;
            settings.UserEmailAddress = this.UserEmailAddress;
            settings.UserName = this.UserName;

            return settings;
        }
    }

    /// <summary>
    /// Smtp设置的扩展类
    /// </summary>
    public static class SmtpSettingExtensions
    {
        /// <summary>
        /// 将数据库实体转换成EditModel
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static SmtpSettingsEditModel AsEditModel(this SmtpSettings settings)
        {
            return new SmtpSettingsEditModel
            {
                DailyLimit = settings.DailyLimit,
                EnableSsl = settings.EnableSsl,
                ForceSmtpUserAsFromAddress = settings.ForceSmtpUserAsFromAddress,
                Host = settings.Host,
                Id = settings.Id,
                Password = settings.Password,
                Port = settings.Port,
                RequireCredentials = settings.RequireCredentials,
                UserEmailAddress = settings.UserEmailAddress,
                UserName = settings.UserName
            };
        }
    }
}
