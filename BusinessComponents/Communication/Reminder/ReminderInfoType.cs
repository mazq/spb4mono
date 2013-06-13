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
    /// 提醒信息类型实体类
    /// </summary>
    public class ReminderInfoType
    {
        private static ConcurrentDictionary<int, ReminderInfoType> registeredReminderInfoTypes = new ConcurrentDictionary<int, ReminderInfoType>();

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ReminderInfoType()
        {
            registeredReminderInfoTypes[ReminderInfoTypeIds.Instance().Message()] = new ReminderInfoType() { TypeId = ReminderInfoTypeIds.Instance().Message(), TypeName = "私信" };
            registeredReminderInfoTypes[ReminderInfoTypeIds.Instance().Notice()] = new ReminderInfoType() { TypeId = ReminderInfoTypeIds.Instance().Notice(), TypeName = "通知" };
            registeredReminderInfoTypes[ReminderInfoTypeIds.Instance().Invitation()] = new ReminderInfoType() { TypeId = ReminderInfoTypeIds.Instance().Invitation(), TypeName = "请求" };
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ReminderInfoType()
        {

        }

        /// <summary>
        /// 获取提醒信息类型
        /// </summary>
        /// <returns>提醒信息类型</returns>
        public static IEnumerable<ReminderInfoType> GetAll()
        {
            return registeredReminderInfoTypes.Values;
        }

        /// <summary>
        /// 获取提醒信息类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        /// <returns>提醒信息类型</returns>
        public static ReminderInfoType Get(int typeId)
        {
            ReminderInfoType reminderInfoType;
            if (registeredReminderInfoTypes.TryGetValue(typeId, out reminderInfoType))
                return reminderInfoType;

            return null;
        }

        /// <summary>
        /// 添加提醒信息类型
        /// </summary>
        /// <param name="ReminderInfoType">提醒信息类型</param>
        public static void Add(ReminderInfoType ReminderInfoType)
        {
            if (ReminderInfoType == null)
                return;
            registeredReminderInfoTypes[ReminderInfoType.TypeId] = ReminderInfoType;
        }

        /// <summary>
        /// 删除提醒信息类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        public static void Remove(int typeId)
        {
            ReminderInfoType ReminderInfoType;
            registeredReminderInfoTypes.TryRemove(typeId, out ReminderInfoType);
        }

        #region 属性

        /// <summary>
        /// 类型Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
