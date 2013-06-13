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
    public class ReminderMode
    {
        private static ConcurrentDictionary<int, ReminderMode> registeredReminderModes = new ConcurrentDictionary<int, ReminderMode>();

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ReminderMode()
        {
            registeredReminderModes[ReminderModeIds.Instance().Email()] = new ReminderMode() { ModeId = ReminderModeIds.Instance().Email(), ModeName = "Email提醒", IsLimitUsed = false };
            //registeredReminderModes[ReminderModeIds.Instance().SMS()] = new ReminderMode() { ModeId = ReminderModeIds.Instance().SMS(), ModeName = "短信提醒", IsLimitUsed = true };
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ReminderMode()
        {

        }

        /// <summary>
        /// 获取提醒方式
        /// </summary>
        /// <returns>提醒方式</returns>
        public static IEnumerable<ReminderMode> GetAll()
        {
            return registeredReminderModes.Values;
        }

        /// <summary>
        /// 获取提醒方式
        /// </summary>
        /// <param name="typeId">类型Id</param>
        /// <returns>提醒方式</returns>
        public static ReminderMode Get(int typeId)
        {
            ReminderMode ReminderMode;
            if (registeredReminderModes.TryGetValue(typeId, out ReminderMode))
                return ReminderMode;

            return null;
        }

        /// <summary>
        /// 添加提醒方式
        /// </summary>
        /// <param name="ReminderMode">提醒方式</param>
        public static void Add(ReminderMode ReminderMode)
        {
            if (ReminderMode == null)
                return;
            registeredReminderModes[ReminderMode.ModeId] = ReminderMode;
        }

        /// <summary>
        /// 删除提醒方式
        /// </summary>
        /// <param name="typeId">类型Id</param>
        public static void Remove(int typeId)
        {
            ReminderMode ReminderMode;
            registeredReminderModes.TryRemove(typeId, out ReminderMode);
        }

        #region 属性

        /// <summary>
        /// 提醒方式Id
        /// </summary>
        public int ModeId { get; set; }

        /// <summary>
        /// 提醒方式名称
        /// </summary>
        public string ModeName { get; set; }

        /// <summary>
        /// 是否仅允许特定角色用户使用（若是，则会检查站点提醒设置中提醒方式的角色限制）
        /// </summary>
        public bool IsLimitUsed { get; set; }

        /// <summary>
        /// 提醒方式描述
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
