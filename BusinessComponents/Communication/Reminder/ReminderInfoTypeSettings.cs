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
    /// 提醒信息类型设置实体类
    /// </summary>
    public class ReminderInfoTypeSettings
    {
        /// <summary>
        /// 提醒信息类型Id
        /// </summary>
        public int ReminderInfoTypeId { get; set; }
        /// <summary>
        /// 发送提醒的时间阀值
        /// </summary>
        public int ReminderThreshold { get; set; }
        /// <summary>
        /// 是否启用提醒
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 是否重复提醒
        /// </summary>
        public bool IsRepeated { get; set; }
        /// <summary>
        /// 重复提醒间隔时间，多长时间（单位：分钟）发送一次提醒
        /// </summary>
        public int RepeatInterval { get; set; }
    }
}
