//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 提醒方式实体类
    /// </summary>
    public class ReminderModeSettings
    {

        #region 属性

        /// <summary>
        /// 提醒方式Id
        /// </summary>
        public int ModeId { get; set; }

        private List<ReminderInfoTypeSettings> reminderInfoTypeSettingses;
        /// <summary>
        /// 提醒类型设置
        /// </summary>
        public List<ReminderInfoTypeSettings> ReminderInfoTypeSettingses
        {
            get
            {
                if (reminderInfoTypeSettingses == null)
                {
                    reminderInfoTypeSettingses = new List<ReminderInfoTypeSettings>();
                }
                return reminderInfoTypeSettingses;
            }
            set
            {
                reminderInfoTypeSettingses = value;
            }
        }

        private List<string> allowedUserRoleNames;
        /// <summary>
        /// 允许的用户角色
        /// </summary>
        public List<string> AllowedUserRoleNames
        {
            get
            {
                if (allowedUserRoleNames == null)
                {
                    allowedUserRoleNames = new List<string>();
                }
                return allowedUserRoleNames;
            }
            set
            {
                allowedUserRoleNames = value;
            }
        }

        #endregion
    }
}
