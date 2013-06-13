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
    /// MessageSettings管理器接口
    /// </summary>
    public interface IMessageSettingsManager
    {
        /// <summary>
        /// 获取MessageSettings
        /// </summary>
        /// <returns>私信相关配置</returns>
        MessageSettings Get();

        /// <summary>
        /// 保存MessageSettings
        /// </summary>
        /// <returns>私信相关配置</returns>
        void Save(MessageSettings messageSettings);
    }
}
