//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Spacebuilder.Common;
using Tunynet.Tasks;
using Tunynet.Common;
using System;
using System.Configuration;

namespace Spacebuilder.Search.Tasks
{
    /// <summary>
    /// 关注用户定时增量索引
    /// </summary>
    public class FollowUserIndexTask : ITask
    {
        private static FollowUserSearcher searcher;
        private FollowService followService = new FollowService();

        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            if (searcher == null)
            {
                //检查是否分布式运行环境
                bool distributedDeploy = Utility.IsDistributedDeploy();

                if (distributedDeploy)
                {
                    searcher = new FollowUserSearcher(SearchEngineService.GetSearchEngine("~/App_Data/IndexFiles/FollowUser"));
                }
                else
                {
                    searcher = (FollowUserSearcher)SearcherFactory.GetSearcher(FollowUserSearcher.CODE);
                }
            }

            //根据上次执行时间从数据库取关注实体列表
            DateTime lastStart = new DateTime(1900, 1, 1);
            if (taskDetail != null && taskDetail.LastStart.HasValue)
            {
                lastStart = taskDetail.LastStart.Value;
            }

            IEnumerable<long> userIds = followService.GetRecentFollowerUserIds(lastStart);

            searcher.Update(userIds);
        }
    }
}
