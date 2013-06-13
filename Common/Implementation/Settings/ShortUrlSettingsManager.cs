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
    /// 短网址配置管理器
    /// </summary>
    public class ShortUrlSettingsManager : IShortUrlSettingsManager
    {
        private ISettingsRepository<ShortUrlSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public ShortUrlSettingsManager()
        {
            repository = new SettingsRepository<ShortUrlSettings>();
        }
        ShortUrlSettings shortUrlSettings = null;
        /// <summary>
        /// 获取ShortUrlSettings
        /// </summary>
        /// <returns>短网址相关配置</returns>
        public ShortUrlSettings Get()
        {
            return repository.Get();
        }


        public void Save(ShortUrlSettings shortUrlSettings)
        {
            repository.Save(shortUrlSettings);
        }
    }
}
