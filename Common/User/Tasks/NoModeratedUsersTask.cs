//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using Tunynet.Tasks;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 定期解除符合解除管制标准的用户任务
    /// </summary>
    public class NoModeratedUsersTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            IUserService userService = DIContainer.Resolve<IUserService>();
            userService.NoModeratedUsers();
        }
    }
}