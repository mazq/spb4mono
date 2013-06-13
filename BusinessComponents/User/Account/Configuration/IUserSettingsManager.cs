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
    /// UserSettings管理器接口
    /// </summary>
    public interface IUserSettingsManager
    {
        /// <summary>
        /// 获取UserSettings
        /// </summary>
        /// <returns></returns>
        UserSettings Get();

        /// <summary>
        /// 保存UserSettings
        /// </summary>
        /// <returns></returns>
        void Save(UserSettings UserSettings);
    }
}
