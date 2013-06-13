//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Tunynet.Common.Configuration;
using Tunynet.Common;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 每天更新置顶时间到期的帖子任务
    /// </summary>
    public class ExpireStickyThreadsTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            BarThreadService barThreadService = new BarThreadService();
            barThreadService.ExpireStickyThreads();
        }
    }
}
