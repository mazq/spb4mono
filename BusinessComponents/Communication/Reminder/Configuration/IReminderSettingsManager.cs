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
    /// ReminderSettings管理器接口
    /// </summary>
    public interface IReminderSettingsManager
    {
        /// <summary>
        /// 获取ReminderSettings
        /// </summary>
        /// <returns></returns>
        ReminderSettings Get();

        /// <summary>
        /// 获取ReminderSettings
        /// </summary>
        /// <returns></returns>
        void Save(ReminderSettings reminderSettings);
    }
}
