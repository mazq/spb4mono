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
    public interface IActivitySettingsManager
    {
        /// <summary>
        /// 获取ActivitySettings
        /// </summary>
        /// <returns></returns>
        ActivitySettings Get();

        /// <summary>
        /// 保存ActivitySettings
        /// </summary>
        /// <param name="activitySettings"></param>
        void Save(ActivitySettings activitySettings);
    }
}
