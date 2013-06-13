//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using Tunynet.Common.Configuration;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    
    /// <summary>
    /// 隐私设置实现类
    /// </summary>
    public class PrivacySettingsManager : IPrivacySettingsManager
    {
        private ISettingsRepository<PrivacySettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public PrivacySettingsManager()
        {
            repository = new SettingsRepository<PrivacySettings>();
        }

        /// <summary>
        /// 获取隐私设置
        /// </summary>
        /// <returns></returns>
        public PrivacySettings Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存隐私设置
        /// </summary>
        /// <returns></returns>
        public void Save(PrivacySettings privacySettings)
        {
            repository.Save(privacySettings);
        }
    }
}