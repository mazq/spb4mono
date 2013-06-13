//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 定期清理垃圾提醒记录
    /// </summary>
    public class DeleteTrashReminderRecordsTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            ReminderService reminderService = new ReminderService();
            reminderService.DeleteTrashRecords();
        }
    }
}