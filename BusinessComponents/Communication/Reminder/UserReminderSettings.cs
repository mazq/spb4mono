//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using System.Collections;


namespace Tunynet.Common
{

    /// <summary>
    /// 用户提醒设置实体
    /// </summary>
    [TableName("tn_UserReminderSettings")]
    [PrimaryKey("Id", autoIncrement = true)] 
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class UserReminderSettings : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
     
        public static UserReminderSettings New(ReminderInfoTypeSettings reminderInfoTypeSettings)
        {
            UserReminderSettings userReminderSetting = new UserReminderSettings();
            userReminderSetting.IsEnabled = reminderInfoTypeSettings.IsEnabled;
            userReminderSetting.IsRepeated = reminderInfoTypeSettings.IsRepeated;
            userReminderSetting.ReminderInfoTypeId = reminderInfoTypeSettings.ReminderInfoTypeId;
            userReminderSetting.ReminderThreshold = reminderInfoTypeSettings.ReminderThreshold;
            userReminderSetting.RepeatInterval = reminderInfoTypeSettings.RepeatInterval;            
            return userReminderSetting;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///提醒方式(Email=1，手机=2)
        /// </summary>
        public int ReminderModeId { get; set; }

        /// <summary>
        ///提醒信息类型（私信=1，通知=2，请求=3）
        /// </summary>
        public int ReminderInfoTypeId { get; set; }

        /// <summary>
        ///发送提醒的时间阀值（单位为分钟），超过此值，发现有未处理的信息将发送提醒
        /// </summary>
        public int ReminderThreshold { get; set; }

        /// <summary>
        ///是否启用提醒
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///是否重复提醒
        /// </summary>
        public bool IsRepeated { get; set; }

        /// <summary>
        ///重复提醒间隔时间，多长时间（单位：分钟）发送一次提醒
        /// </summary>
        public int RepeatInterval { get; set; }



        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}
