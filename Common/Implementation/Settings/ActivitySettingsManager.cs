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
    /// 动态全局设置实现类
    /// </summary>
    public class ActivitySettingsManager : IActivitySettingsManager
    {
        private ISettingsRepository<ActivitySettings> repository;

        /// <summary>
        /// 构造器
        /// </summary>
        public ActivitySettingsManager()
        {
            repository = new SettingsRepository<ActivitySettings>();
        }

        /// <summary>
        /// 获取动态设置
        /// </summary>
        /// <returns></returns>
        public ActivitySettings Get()
        {
            return repository.Get();            
        }

        /// <summary>
        /// 保存动态设置
        /// </summary>
        /// <param name="activitySettings"></param>
        public void Save(ActivitySettings activitySettings)
        {
            repository.Save(activitySettings);
        }
    }
}