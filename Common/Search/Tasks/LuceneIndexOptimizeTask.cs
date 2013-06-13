//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Search;
using Tunynet.Tasks;
using System.Configuration;
using Spacebuilder.Common;

namespace Spacebuilder.Search.Tasks
{
    /// <summary>
    /// 优化Lucene索引
    /// </summary>
    /// <remarks>
    /// 建议2H进行一次
    /// </remarks>
    public class LuceneIndexOptimizeTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            //检查是否分布式运行环境
            bool distributedDeploy = Utility.IsDistributedDeploy();

            if (distributedDeploy)
            {
                foreach (SearchEngine searchEngine in SearchEngineService.searchEngines.Values)
                {
                    searchEngine.Optimize();
                }
            }
            else
            {
                IEnumerable<ISearcher> searchers = SearcherFactory.GetSearchersOfBaseLucene();

                foreach (var searcher in searchers)
                {
                    if (searcher.SearchEngine is SearchEngine)
                    {
                        ((SearchEngine)searcher.SearchEngine).Optimize();
                    }
                }
            }
        }
    }
}
