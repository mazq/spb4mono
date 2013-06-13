//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Tunynet;
using Tunynet.Search;
using Autofac;
using System.Collections.Concurrent;
using Spacebuilder.Common;

namespace Spacebuilder.Search
{
    public static class SearcherFactory
    {
        public const string GlobalSearchCode = "GlobalSearcher";
        public const string GlobalSearchName = "所有";

        //以indexPath为key，存储SearchEngine实例
        public static ConcurrentDictionary<string, ISearchEngine> searchEngines = new ConcurrentDictionary<string, ISearchEngine>();

        /// <summary>
        /// 获取所有Searcher
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ISearcher> GetSearchers()
        {
            return DIContainer.Resolve<IEnumerable<ISearcher>>().OrderBy(s => s.DisplayOrder);
        }

        /// <summary>
        /// 获取用于前台显示的Searcher
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ISearcher> GetDisplaySearchers()
        {
            return DIContainer.Resolve<IEnumerable<ISearcher>>().Where(s => s.IsDisplay).OrderBy(s => s.DisplayOrder);
        }

        /// <summary>
        /// 获取用于快捷搜索的Searcher
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ISearcher> GetQuickSearchers(int num, string areaName = null)
        {
            IEnumerable<ISearcher> quickSearchers = DIContainer.Resolve<IEnumerable<ISearcher>>().Where(s => s.AsQuickSearch).OrderBy(s => s.DisplayOrder).Take(num);

            if (!string.IsNullOrEmpty(areaName) && areaName != "Common")
            {
                areaName += "Searcher";

                List<ISearcher> quickSearcherList = new List<ISearcher>();

                ISearcher currentSearcher = quickSearchers.Where(n => n.Code == areaName).FirstOrDefault();
                if (currentSearcher != null)
                    quickSearcherList.Add(currentSearcher);

                IEnumerable<ISearcher> otherSearchers = quickSearchers.Where(n => n.Code != areaName);
                foreach (var otherSearcher in otherSearchers)
                {
                    quickSearcherList.Add(otherSearcher);
                }

                return quickSearcherList;
            }

            return quickSearchers;
        }

        /// <summary>
        /// 获取所有基于Lucene的Searcher
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ISearcher> GetSearchersOfBaseLucene()
        {
            return DIContainer.Resolve<IEnumerable<ISearcher>>().Where(s => s.IsBaseOnLucene).OrderBy(s => s.DisplayOrder);
        }

        /// <summary>
        /// 依据搜索器编码获取ISearcher
        /// </summary>
        /// <param name="code">搜索器的编码，各ISearcher的code必须唯一，以应用的ApplicationKey作为前缀，例如UserSearcher或User_Searcher</param>
        /// <returns><see cref="ISearcher"/></returns>
        public static ISearcher GetSearcher(string code)
        {
            //return DIContainer.Resolve<IEnumerable<ISearcher>>().Where(s => s.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return DIContainer.ResolveNamed<ISearcher>(code);
        }

        /// <summary>
        /// 依据indexPath获取SearchEngine
        /// </summary>
        /// <param name="indexPath">索引路径</param>
        /// <returns></returns>
        public static ISearchEngine GetSearchEngine(string indexPath)
        {
            ISearchEngine searchEngine = null;

            if (!searchEngines.TryGetValue(indexPath, out searchEngine))
            {
                if (Utility.IsDistributedDeploy())
                {
                    //使用分布式搜索引擎
                    searchEngine = new SearchEngineServiceClient("SearchEngineService", indexPath);
                }
                else
                {
                    //使用本地搜索引擎
                    searchEngine = new SearchEngine(indexPath);
                }

                searchEngines.TryAdd(indexPath, searchEngine);
            }

            return searchEngine;
        }

    }
}
