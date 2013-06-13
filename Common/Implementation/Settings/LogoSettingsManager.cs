//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Common.Configuration;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 标识图全局设置实现类
    /// </summary>
    public class LogoSettingsManager : ILogoSettingsManager
    {
        private ISettingsRepository<LogoSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public LogoSettingsManager()
        {
            repository = new SettingsRepository<LogoSettings>();
        }
        
        /// <summary>
        /// 获取LogoSettings
        /// </summary>
        /// <returns></returns>
        public LogoSettings Get()
        {
            return repository.Get();
        }


        public void Save(LogoSettings logoSettings)
        {
            repository.Save(logoSettings);
        }
    }
}