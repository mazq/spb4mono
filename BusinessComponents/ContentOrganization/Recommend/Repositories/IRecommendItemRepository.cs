//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///推荐内容数据访问接口
    /// </summary>
    public interface IRecommendItemRepository : IRepository<RecommendItem>
    {
        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="itemId">内容Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        bool Delete(long itemId, string tenantTypeId);

        /// <summary>
        /// 定期移除过期的推荐内容
        /// </summary>
        void DeleteExpiredRecommendItems();

        /// <summary>
        /// 获取某种推荐类别下的前N条推荐内容
        /// </summary>
        /// <param name="topNumber">前N条</param>
        /// <param name="recommendTypeId">推荐类别Id</param>
        /// <returns></returns>
        IEnumerable<RecommendItem> GetTops(int topNumber, string recommendTypeId);

        /// <summary>
        /// 获取推荐内容
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <param name="recommendTypeId">推荐类型Id</param>
        RecommendItem Get(long itemId, string recommendTypeId);

        /// <summary>
        /// 获取某种推荐类别下的推荐内容分页集合
        /// </summary>
        /// <param name="topNumber">前N条</param>
        /// <param name="recommendTypeId">推荐类别Id</param>
        /// <returns></returns>
        PagingDataSet<RecommendItem> Gets(string recommendTypeId, int pageIndex);

        /// <summary>
        /// 获取某条内容的所有推荐
        /// </summary>
        /// <param name="itemId">内容Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        IEnumerable<RecommendItem> Gets(long itemId, string tenantTypeId);

        /// <summary>
        /// 分页获取推荐内容后台管理列表
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="recommendTypeId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        PagingDataSet<RecommendItem> GetsForAdmin(string tenantTypeId, string recommendTypeId, bool? isLink, int pageSize, int pageIndex);
    }
}