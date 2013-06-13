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
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Smtp设置在SPB4.0中的继承类
    /// </summary>
    public class SmtpSettingsChild : SmtpSettings
    {
        public SmtpSettingsChild() { }

        public SmtpSettingsChild(SmtpSettings smtpSettings)
        {
            this.DailyLimit = smtpSettings.DailyLimit;
            this.EnableSsl = smtpSettings.EnableSsl;
            this.ForceSmtpUserAsFromAddress = smtpSettings.ForceSmtpUserAsFromAddress;
            this.Host = smtpSettings.Host;
            this.Id = smtpSettings.Id;
            this.IsDeletedInDatabase = smtpSettings.IsDeletedInDatabase;
            this.Password = smtpSettings.Password;
            this.Port = smtpSettings.Port;
            this.RequireCredentials = smtpSettings.RequireCredentials;
            this.UserEmailAddress = smtpSettings.UserEmailAddress;
            this.UserName = smtpSettings.UserName;
        }

        public override int TodaySendCount
        {
            get
            {
                CountService countservice = new CountService(TenantTypeIds.Instance().Email());
                return countservice.GetStageCount(CountTypes.Instance().UseCount(), 1, base.Id);
            }

            set
            {
                CountService countservice = new CountService(TenantTypeIds.Instance().Email());
                countservice.ChangeCount(CountTypes.Instance().UseCount(), base.Id, 0, value - this.TodaySendCount, true);
            }
        }
    }
}