//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 站点提醒设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class ReminderSettings : IEntity
    {
        private List<ReminderModeSettings> reminderModeSettingses;
        /// <summary>
        /// 提醒方式集合
        /// </summary>
        public List<ReminderModeSettings> ReminderModeSettingses
        {
            get
            {
                if (reminderModeSettingses == null)
                {
                    reminderModeSettingses = new List<ReminderModeSettings>{
                        new ReminderModeSettings{ 
                                ModeId = ReminderModeIds.Instance().Email(), 
                                ReminderInfoTypeSettingses = new List<ReminderInfoTypeSettings>{
                                new ReminderInfoTypeSettings{ ReminderInfoTypeId = ReminderInfoTypeIds.Instance().Invitation(), IsEnabled=true, IsRepeated=false, ReminderThreshold=24*60*3, RepeatInterval=24*60*7 },
                                new ReminderInfoTypeSettings{ ReminderInfoTypeId = ReminderInfoTypeIds.Instance().Message(), IsEnabled=true, IsRepeated=false,  ReminderThreshold=24*60*3, RepeatInterval=24*60*7 },
                                new ReminderInfoTypeSettings{ ReminderInfoTypeId = ReminderInfoTypeIds.Instance().Notice(), IsEnabled=true, IsRepeated=false,  ReminderThreshold=24*60*3, RepeatInterval=24*60*7 }
                                },
                                AllowedUserRoleNames = new List<string>(){RoleNames.Instance().RegisteredUsers()}
                        }
                    //},
                    //new ReminderModeSettings{ 
                    //            ModeId = ReminderModeIds.Instance().SMS(), 
                    //            ReminderInfoTypeSettingses = new List<ReminderInfoTypeSettings>{
                    //            new ReminderInfoTypeSettings{ ReminderInfoTypeId = ReminderInfoTypeIds.Instance().Invitation(), IsEnabled=true, IsRepeated=false, ReminderThreshold=30, RepeatInterval=0 },
                    //            new ReminderInfoTypeSettings{ ReminderInfoTypeId = ReminderInfoTypeIds.Instance().Message(), IsEnabled=true, IsRepeated=false,  ReminderThreshold=30, RepeatInterval=0 },
                    //            new ReminderInfoTypeSettings{ ReminderInfoTypeId = ReminderInfoTypeIds.Instance().Notice(), IsEnabled=false, IsRepeated=false,  ReminderThreshold=30, RepeatInterval=0 }
                    //            },
                    //            AllowedUserRoleNames = new List<string>{RoleNames.Instance().SuperAdministrator(), RoleNames.Instance().ContentAdministrator()}
                    //            }
                        };
                }
                return reminderModeSettingses;
            }
            set { reminderModeSettingses = value; }
        }

        private int _reminderRecordStorageDay = 30;
        /// <summary>
        /// 提醒记录保留的天数
        /// </summary>
        public int ReminderRecordStorageDay
        {
            get { return _reminderRecordStorageDay; }
            set { _reminderRecordStorageDay = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(ReminderSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}
