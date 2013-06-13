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
    /// AreaSettings管理器接口
    /// </summary>
    public interface IAreaSettingsManager
    {
        /// <summary>
        /// 获取AreaSettings
        /// </summary>
        /// <returns></returns>
        AreaSettings Get();

        /// <summary>
        /// 保存AreaSettings
        /// </summary>
        /// <param name="areaSettings"></param>
        void Save(AreaSettings areaSettings);
    }
}
