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
    /// EmotionSettings管理器接口
    /// </summary>
    public class EmotionSettingsManager : IEmotionSettingsManager
    {
        private ISettingsRepository<EmotionSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
        public EmotionSettingsManager()
        {
            repository = new SettingsRepository<EmotionSettings>();
        }

        /// <summary>
        /// 获取EmotionSettings
        /// </summary>
        /// <returns>表情相关配置</returns>
        public EmotionSettings Get()
        {
            return repository.Get();
        }


        public void Save(EmotionSettings emotionSettings)
        {
            repository.Save(emotionSettings);
        }
    }
}
