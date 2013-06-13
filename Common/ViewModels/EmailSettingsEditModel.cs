using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Email;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邮件设置
    /// </summary>
    public class EmailSettingsEditModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmailSettingsEditModel()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmailSettingsEditModel(EmailSettings emailSettings)
        {
            if (emailSettings != null)
            {
                AdminEmailAddress = emailSettings.AdminEmailAddress;
                BatchSendLimit = emailSettings.BatchSendLimit;
                NoReplyAddress = emailSettings.NoReplyAddress;
                NumberOfTries = emailSettings.NumberOfTries;
                SendTimeInterval = emailSettings.SendTimeInterval;
                if (emailSettings.SmtpSettings != null)
                {
                    Host = emailSettings.SmtpSettings.Host;
                    Password = emailSettings.SmtpSettings.Password;
                    Port = emailSettings.SmtpSettings.Port;
                    UserEmailAddress = emailSettings.SmtpSettings.UserEmailAddress;
                    EnableSsl = emailSettings.SmtpSettings.EnableSsl;
                    ForceSmtpUserAsFromAddress = emailSettings.SmtpSettings.ForceSmtpUserAsFromAddress;
                    UserName = emailSettings.SmtpSettings.UserName;
                    RequireCredentials = emailSettings.SmtpSettings.RequireCredentials;
                }
            }
        }

        /// <summary>
        /// 管理员Email地址
        /// </summary>
        [Display(Name = "管理员Email地址")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "请输入正确邮箱")]
        [Required(ErrorMessage = "请输入邮箱")]
        public string AdminEmailAddress { get; set; }

        /// <summary>
        /// 每次从队列批量发送邮件的最大数量
        /// </summary>
        [Display(Name = "每次从队列批量发送邮件的最大数量")]
        [RegularExpression(@"\d+", ErrorMessage = "请输入整数，-1表示没有限制")]
        [Required(ErrorMessage = "必填")]
        public int BatchSendLimit { get; set; }

        /// <summary>
        /// NoReply邮件地址
        /// </summary>
        [Display(Name = "NoReply邮件地址")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "请输入正确邮箱")]
        public string NoReplyAddress { get; set; }

        /// <summary>
        ///  尝试发送次数
        /// </summary>
        [Display(Name = " 尝试发送次数")]
        [RegularExpression(@"\d+", ErrorMessage = "请输入整数")]
        [Required(ErrorMessage = "必填")]
        public int NumberOfTries { get; set; }

        /// <summary>
        ///  邮件发送间隔(以分钟为单位)
        /// </summary>
        [Display(Name = "邮件发送间隔")]
        [RegularExpression(@"\d+", ErrorMessage = "请输入整数")]
        [Required(ErrorMessage = "必填")]
        public int SendTimeInterval { get; set; }

        /// <summary>
        /// smtp服务器的域名或IP
        /// </summary>
        [Display(Name = "smtp服务器的域名或IP")]
        [RegularExpression(@"[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?$|([1-9]|[1-9]\\d|1\\d{2}|2[0-4]\\d|25[0-5])(\\.(\\d|[1-9]\\d|1\\d{2}|2[0-4]\\d|25[0-5])){3}", ErrorMessage = "请输入正确域名或IP")]
        public string Host { get; set; }

        /// <summary>
        /// 登录smtp服务器的密码
        /// </summary>
        [Display(Name = "登录smtp服务器的密码")]
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// smtp服务器端口号
        /// </summary>
        [Display(Name = "smtp服务器端口号")]
        [RegularExpression(@"\d+", ErrorMessage = "请输入端口号")]
        [Required(ErrorMessage = "必填")]
        public int Port { get; set; }

        /// <summary>
        /// 录smtp服务器的用户邮件地址（可能与UserName，也可能不同）
        /// </summary>
        [Display(Name = "用户邮件地址")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "请输入正确邮箱")]
        [Required(ErrorMessage = "请输入邮箱")]
        public string UserEmailAddress { get; set; }

        /// <summary>
        /// smtp服务器是否启用ssl
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// 强制smtp登录用户作为发件人
        /// </summary>
        public bool ForceSmtpUserAsFromAddress { get; set; }


        /// <summary>
        /// 是否是检测
        /// </summary>
        public bool IsCheckOut { get; set; }


        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 是否需要身份验证
        /// </summary>
        public bool RequireCredentials { get; set; }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <returns></returns>
        public EmailSettings AsEmailSettings()
        {
            var emailSettings = DIContainer.Resolve<IEmailSettingsManager>().Get();
            emailSettings.SmtpSettings = new SmtpSettings();
            emailSettings.AdminEmailAddress = AdminEmailAddress;
            emailSettings.BatchSendLimit = BatchSendLimit;
            emailSettings.NoReplyAddress = NoReplyAddress;
            emailSettings.NumberOfTries = NumberOfTries;
            emailSettings.SendTimeInterval = SendTimeInterval;
            emailSettings.SmtpSettings.Host = Host;
            emailSettings.SmtpSettings.Password = Password;
            emailSettings.SmtpSettings.Port = Port;
            emailSettings.SmtpSettings.UserEmailAddress = UserEmailAddress;
            emailSettings.SmtpSettings.EnableSsl = EnableSsl;
            emailSettings.SmtpSettings.ForceSmtpUserAsFromAddress = ForceSmtpUserAsFromAddress;
            emailSettings.SmtpSettings.UserName = UserName;
            emailSettings.SmtpSettings.RequireCredentials = RequireCredentials;
            return emailSettings;
        }
    }
}
