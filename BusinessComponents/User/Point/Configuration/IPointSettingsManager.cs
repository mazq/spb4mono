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
    /// PointSettings管理器接口
    /// </summary>
    public interface IPointSettingsManager
    {
        /// <summary>
        /// 获取PointSettings
        /// </summary>
        /// <returns></returns>
        PointSettings Get();

        /// <summary>
        /// 保存PointSettings
        /// </summary>
        /// <param name="pointSettings"></param>
        void Save(PointSettings pointSettings);

    }
}
