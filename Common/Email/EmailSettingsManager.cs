//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Email;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{

    /// <summary>
    /// 生成EmailSettings
    /// </summary>
    public class EmailSettingsManager : IEmailSettingsManager
    {
        private ISettingsRepository<EmailSettings> repository;

        /// <summary>
        /// 构造器
        /// </summary>
        public EmailSettingsManager()
        {
            repository = new SettingsRepository<EmailSettings>();
        }

        #region IEmailSettingsManager 成员

        /// <summary>
        /// 获取EmailSettings
        /// </summary>
        /// <returns></returns>
        EmailSettings IEmailSettingsManager.Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存EmailSettings
        /// </summary>
        /// <param name="emailSettings"></param>
        public void Save(EmailSettings emailSettings)
        {
            repository.Save(emailSettings);
        }


        #endregion
    }

}