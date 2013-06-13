//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// ActivitySettings管理器接口
    /// </summary>
    public interface IPrivacySettingsManager
    {
        /// <summary>
        /// 获取PrivacySettings
        /// </summary>
        /// <returns></returns>
        PrivacySettings Get();

        /// <summary>
        /// 保存PrivacySettings
        /// </summary>
        /// <param name="privacySettings"></param>
        void Save(PrivacySettings privacySettings);
    }
}
