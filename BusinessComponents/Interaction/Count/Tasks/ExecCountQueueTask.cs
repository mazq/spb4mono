//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Tasks;
using System.Collections.Generic;

namespace Tunynet.Common
{
    /// <summary>
    /// 执行计数队列任务
    /// </summary>
    public class ExecCountQueueTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            new CountRepository().ExecQueue();
        }
    }
}
