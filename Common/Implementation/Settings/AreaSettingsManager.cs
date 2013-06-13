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
    /// 地区全局设置实现类
    /// </summary>
    public class AreaSettingsManager : IAreaSettingsManager
    {
        private ISettingsRepository<AreaSettings> repository;

        /// <summary>
        /// 构造器
        /// </summary>
        public AreaSettingsManager()
        {
            repository = new SettingsRepository<AreaSettings>();
        }

        /// <summary>
        /// 获取地区设置
        /// </summary>
        /// <returns></returns>
        public AreaSettings Get()
        {
            return repository.Get();
        }

        /// <summary>
        /// 保存地区设置
        /// </summary>
        /// <returns></returns>
        public void Save(AreaSettings areaSettings)
        {
            repository.Save(areaSettings);
        }
    }
}