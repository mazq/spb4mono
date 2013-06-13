//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Spacebuilder.Common;
using Tunynet.Search;
using Tunynet.Tasks;

namespace Spacebuilder.Search.Tasks
{
    /// <summary>
    /// 定时提交索引，用于NRT近实时搜索
    /// </summary>
    public class LuceneIndexCommitTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail = null)
        {
            //检查是否分布式运行环境
            bool distributedDeploy = Utility.IsDistributedDeploy();

            if (distributedDeploy)
            {
                foreach (SearchEngine searchEngine in SearchEngineService.searchEngines.Values)
                {
                    searchEngine.Commit();
                }
            }
            else
            {
                IEnumerable<ISearcher> searchers = SearcherFactory.GetSearchersOfBaseLucene();

                foreach (var searcher in searchers)
                {
                    if (searcher.SearchEngine is SearchEngine)
                    {
                        ((SearchEngine)searcher.SearchEngine).Commit();
                    }
                }
            }
        }

    }
}
