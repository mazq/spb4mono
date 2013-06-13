//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 提醒设置
    /// </summary>
    public class ReminderSettingsEditModel
    {
        /// <summary>
        /// 提醒方式集合
        /// </summary>
        public List<ReminderModeSettings> ReminderModeSettings { get; set; }

        /// <summary>
        /// 提醒记录保留的天数
        /// </summary>
        [Required(ErrorMessage = "请输入保留天数")]
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int ReminderRecordStorageDay { get; set; }

        /// <summary>
        /// 转换为提醒设置实体用于存储
        /// </summary>
        /// <returns></returns>
        public ReminderSettings AsReminderSettings()
        {
            var reminderSettings = DIContainer.Resolve<IReminderSettingsManager>().Get();            
            reminderSettings.ReminderRecordStorageDay = ReminderRecordStorageDay;
            reminderSettings.ReminderModeSettingses = ReminderModeSettings;
            return reminderSettings;
        }
    }

    /// <summary>
    /// 提醒设置扩展类
    /// </summary>
    public static class ReminderSettingsEditModelExtensions
    {
        /// <summary>
        /// 转换为EditModel
        /// </summary>
        /// <param name="reminderSettings"></param>
        /// <returns></returns>
        public static ReminderSettingsEditModel AsEditModel(this ReminderSettings reminderSettings)
        {
            return new ReminderSettingsEditModel
            {
                ReminderModeSettings = reminderSettings.ReminderModeSettingses,
                ReminderRecordStorageDay = reminderSettings.ReminderRecordStorageDay
            };
        }
    }
}
