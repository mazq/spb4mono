//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Tasks;

namespace Spacebuilder.Tasks
{
    /// <summary>
    /// 定时任务执行服务WCF服务端
    /// </summary>
    public class TaskServiceWCF : ITaskService
    {
        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <param name="taskId">定时任务的Id</param>
        public void RunTask(int taskId)
        {
            TaskSchedulerFactory.GetScheduler().Run(taskId);
        }
    }
}
