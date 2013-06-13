//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 友情链接数据访问接口
    /// </summary>
    public interface ILinkRepository : IRepository<LinkEntity>
    {
        /// <summary>
        /// 获取Owner友情链接
        /// </summary>
        /// <param name="ownerType">拥有者类型</param>
        /// <param name="ownerId">拥有者</param>
        /// <param name="topNumber">获取数量</param>
        /// <returns></returns>
        IEnumerable<LinkEntity> GetsOfOwner(int ownerType, long ownerId,int topNumber);

        /// <summary>
        /// 获取站点友情链接(后台管理)
        /// </summary>
        /// <param name="categoryId">分类标识</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        IEnumerable<LinkEntity> GetsOfSiteForAdmin(long? categoryId);
    }
}