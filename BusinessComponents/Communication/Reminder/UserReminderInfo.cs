//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Utilities;
namespace Tunynet.Common
{
    /// <summary>
    /// 用户提醒信息
    /// </summary>
    public class UserReminderInfo
    {
        /// <summary>
        /// 默认构造器
        /// </summary>
        public UserReminderInfo()
        {

        }

        /// <summary>
        /// 有参构造器
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="reminderInfos">提醒信息集合</param>
        public UserReminderInfo(long userId, IList<ReminderInfo> reminderInfos)
        {
            this.UserId = userId;
            this.reminderInfos = reminderInfos;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 处理地址（任务自运行时赋值）
        /// </summary>
        public string ProcessUrl { get; internal set; }

        /// <summary>
        /// 提醒信息类型
        /// </summary>
        public ReminderInfoType ReminderInfoType { get; set; }

        private IList<ReminderInfo> reminderInfos;
        /// <summary>
        /// 提醒信息集合
        /// </summary>
        public IEnumerable<ReminderInfo> ReminderInfos
        {
            get
            {
                if (reminderInfos == null)
                    reminderInfos = new List<ReminderInfo>();
                return reminderInfos.ToReadOnly();
            }
        }

        /// <summary>
        /// 添加提醒信息
        /// </summary>
        /// <param name="reminderInfo">提醒信息</param>
        public void Append(ReminderInfo reminderInfo)
        {
            if (reminderInfos == null)
                reminderInfos = new List<ReminderInfo>();
            reminderInfos.Add(reminderInfo);
        }


        /// <summary>
        /// 设置提醒信息
        /// </summary>
        /// <param name="reminderInfo">提醒信息</param>
        public void SetReminderInfos(IList<ReminderInfo> reminderInfos)
        {
            this.reminderInfos = reminderInfos;
        }
    }
}