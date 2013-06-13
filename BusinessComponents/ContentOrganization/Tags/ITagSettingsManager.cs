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
    /// TagSettings管理器接口
    /// </summary>
    public interface ITagSettingsManager
    {
        /// <summary>
        /// 获取TagSettings
        /// </summary>
        /// <returns>标签相关配置</returns>
        TagSettings Get();

        /// <summary>
        /// 保存TagSettings
        /// </summary>
        /// <returns>标签相关配置</returns>
        void Save(TagSettings tagSettings);
    }
}
