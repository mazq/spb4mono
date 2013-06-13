//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// MessageSettings管理器接口
    /// </summary>
    public class MessageSettingsManager : IMessageSettingsManager
    {
        private ISettingsRepository<MessageSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public MessageSettingsManager()
        {
            repository = new SettingsRepository<MessageSettings>();
        }

        /// <summary>
        /// 获取MessageSettings
        /// </summary>
        /// <returns>私信相关配置</returns>
        public MessageSettings Get()
        {
            return repository.Get();
        }


        public void Save(MessageSettings messageSettings)
        {
            repository.Save(messageSettings);
        }
    }
}
