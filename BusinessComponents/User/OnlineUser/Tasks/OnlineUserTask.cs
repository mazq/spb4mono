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

namespace Tunynet.Common.Tasks
{
    /// <summary>
    /// 在线用户定期执行任务
    /// </summary>
    /// <remarks>
    /// 分布式部署时，要求每个web服务器都要部署，不允许集中部署
    /// 建议每5分钟执行一次
    /// </remarks>
    public class OnlineUserTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            new OnlineUserService().Refresh();
        }
    }
}

