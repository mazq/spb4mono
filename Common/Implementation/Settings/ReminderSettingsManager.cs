//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Common;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 提醒全局设置实现类
    /// </summary>
    public class ReminderSettingsManager : IReminderSettingsManager
    {
        private ISettingsRepository<ReminderSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public ReminderSettingsManager()
        {
            repository = new SettingsRepository<ReminderSettings>();
        }
        /// <summary>
        /// 获取站外提醒全局设置
        /// </summary>
        /// <returns></returns>
        public ReminderSettings Get()
        {
            return repository.Get();
        }


        public void Save(ReminderSettings reminderSettings)
        {
            repository.Save(reminderSettings);
        }
    }
}