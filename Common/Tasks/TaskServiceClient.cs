//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Spacebuilder.Tasks
{
    /// <summary>
    /// 定时任务执行服务WCF客户端
    /// </summary>
    public class TaskServiceClient : ClientBase<ITaskService>
    {
        /// <summary>
        /// 无参数构造函数，不能直接调用，保留的目的是WCF所需
        /// </summary>
        public TaskServiceClient()
            : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="endpointName">与客户端web.config中client\endpoint的name属性一致</param>
        public TaskServiceClient(string endpointName)
            : base(endpointName)
        {
        }

        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <param name="taskId">定时任务的Id</param>
        public void RunTask(int taskId)
        {
            this.Channel.RunTask(taskId);
        }
    }
}
