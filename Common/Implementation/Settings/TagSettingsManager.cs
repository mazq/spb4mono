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
    public class TagSettingsManager : ITagSettingsManager
    {
        private ISettingsRepository<TagSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public TagSettingsManager()
        {
            repository = new SettingsRepository<TagSettings>();
        }
        /// <summary>
        /// 获取地区设置
        /// </summary>
        /// <returns></returns>
        public TagSettings Get()
        {
            return repository.Get();
        }


        public void Save(TagSettings tagSettings)
        {
            repository.Save(tagSettings);
        }
    }
}