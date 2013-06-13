//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Search;

namespace Spacebuilder.Search
{
    /// <summary>
    /// Searcher接口
    /// </summary>
    public interface ISearcher
    {
        /// <summary>
        /// 搜索类型代码
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 水印
        /// </summary>
        string WaterMark { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否前台显示
        /// </summary>
        bool IsDisplay { get; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        int DisplayOrder { get; }

        /// <summary>
        /// 是否基于Lucene实现
        /// </summary>
        bool IsBaseOnLucene { get; }

        /// <summary>
        /// Lucene索引路径（完整物理路径，支持unc）
        /// </summary>
        string IndexPath { get; }

        /// <summary>
        /// 是否作为快捷搜索
        /// </summary>
        bool AsQuickSearch { get; }

        /// <summary>
        /// 处理快捷搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        string QuickSearchActionUrl(string keyword);

        /// <summary>
        /// 处理当前应用搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        string PageSearchActionUrl(string keyword);

        /// <summary>
        /// 处理全局搜索的URL
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns></returns>
        string GlobalSearchActionUrl(string keyword);

        /// <summary>
        /// 重建索引
        /// </summary>        
        void RebuildIndex();

        /// <summary>
        /// 关联的搜索引擎实例
        /// </summary>
        ISearchEngine SearchEngine { get; }

    }
}
