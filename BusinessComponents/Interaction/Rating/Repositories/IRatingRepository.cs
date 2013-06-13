//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 星级评价数据访问接口
    /// </summary>
    public interface IRatingRepository : IRepository<Rating>
    {
        /// <summary>
        /// 获取星级评价信息
        /// </summary>
        /// <param name="objectId">操作对象</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        Rating Get(long objectId, string tenantTypeId);

        /// <summary>
        /// 获取前N条操作对象Id
        /// </summary>
        /// <param name="topNumber">获取前N条</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        IEnumerable<long> GetTopObjectIds(int topNumber, string tenantTypeId, long ownerId = 0);

        /// <summary>
        /// 对操作对象进行星级评价操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <param name="rateNumber">星级类型</param>
        /// <param name="ownerId">拥有者Id</param>
        bool Rated(long objectId, string tenantTypeId, long userId, int rateNumber, long ownerId = 0);

        /// <summary>
        /// 用户当前操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <returns>用户是否做过评价：True-评价过， False-没做过操作</returns>
        bool IsRated(long objectId, string tenantTypeId, long userId);

        /// <summary>
        /// 获取操作对象Id集合  用于分页
        /// </summary>
        /// <param name="tenantTypeId"> 租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="ownerId">拥有者Id</param>
        PagingEntityIdCollection GetPagingObjectIds(string tenantTypeId, int pageIndex = 1, long ownerId = 0);
    }
}