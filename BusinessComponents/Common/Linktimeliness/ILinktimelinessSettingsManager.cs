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
    /// LinktimelinessSettings管理器接口
    /// </summary>
    public interface ILinktimelinessSettingsManager
    {
        /// <summary>
        /// 获取LinktimelinessSettings
        /// </summary>
        /// <returns></returns>
        LinktimelinessSettings Get();

        /// <summary>
        /// 保存LinktimelinessSettings
        /// </summary>
        /// <returns></returns>
        void Save(LinktimelinessSettings linktimelinessSettings);
    }
}
