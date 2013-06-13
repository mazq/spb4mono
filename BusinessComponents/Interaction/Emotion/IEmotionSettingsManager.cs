//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// EmotionSettings管理器接口
    /// </summary>
    public interface IEmotionSettingsManager
    {
        /// <summary>
        /// 获取EmotionSettings
        /// </summary>
        /// <returns>表情相关配置</returns>
        EmotionSettings Get();

        /// <summary>
        /// 保存EmotionSettings
        /// </summary>
        /// <returns>表情相关配置</returns>
        void Save(EmotionSettings emotionSettings);
    }
}
