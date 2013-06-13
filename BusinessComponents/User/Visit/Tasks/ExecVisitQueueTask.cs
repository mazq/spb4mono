//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Tasks;
using System.Collections.Generic;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    //执行频率必须小于实体缓存时间
    /// <summary>
    /// 执行访客记录队列任务
    /// </summary>
    /// <remarks>
    /// 分布式部署时，要求每个web服务器都要部署，不允许集中部署
    /// </remarks>
    public class ExecVisitQueueTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            new VisitRepository().ExecQueue();
        }
    }
}
