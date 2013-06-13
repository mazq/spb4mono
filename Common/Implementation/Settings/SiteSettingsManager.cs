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
    /// 站点设置实现类
    /// </summary>
    public class SiteSettingsManager : ISiteSettingsManager
    {
        private ISettingsRepository<SiteSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public SiteSettingsManager()
        {
            repository = new SettingsRepository<SiteSettings>();
        }

        SiteSettings siteSettings = null;
        /// <summary>
        /// 获取站点设置
        /// </summary>
        /// <returns></returns>
        public SiteSettings Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存站点设置
        /// </summary>
        /// <returns></returns>
        public void Save(SiteSettings siteSettings)
        {
            repository.Save(siteSettings);
        }

    }
}