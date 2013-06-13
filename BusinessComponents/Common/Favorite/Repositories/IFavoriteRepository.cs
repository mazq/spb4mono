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
    /// 用户收藏的接口
    /// </summary>
    public interface IFavoriteRepository : IRepository<FavoriteEntity>
    {
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>true-收藏成功,false-收藏失败</returns>
        bool Favorite(long objectId, long userId, string tenantTypeId);

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>true-取消成功,false-取消失败</returns>
        bool CancelFavorited(long objectId, long userId, string tenantTypeId);

        /// <summary>
        /// 判断是否收藏
        /// </summary>
        /// <param name="objectId">被收藏对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>true-已收藏，false-未收藏</returns>
        bool IsFavorited(long objectId, long userId, string tenantTypeId);

        /// <summary>
        /// 获取收藏对象Id分页数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示内容数</param>
        ///<returns></returns>
        PagingDataSet<long> GetPagingObjectIds(long userId, string tenantTypeId, int pageIndex, int? pageSize = null);

        /// <summary>
        /// 获取前N个收藏对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">要获取Id的个数</param>
        ///<returns></returns>
        IEnumerable<long> GetTopObjectIds(long userId, string tenantTypeId, int topNumber);

        /// <summary>
        /// 获取全部收藏对象Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<returns></returns>
        IEnumerable<long> GetAllObjectIds(long userId, string tenantTypeId);

        /// <summary>
        /// 根据收藏对象获取UserId
        /// </summary>
        /// <param name="objectId">收藏对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        IEnumerable<long> GetUserIdsOfObject(long objectId, string tenantTypeId);

        /// <summary>
        /// 根据收藏对象获取收藏了此对象的前N个用户Id集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="topNumber">要获取记录数</param>
        /// <returns></returns>
        IEnumerable<long> GetTopUserIdsOfObject(long objectId, string tenantTypeId, int topNumber);

        /// <summary>
        /// 根据收藏对象获取收藏了此对象的用户Id分页集合
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingDataSet<long> GetPagingUserIdsOfObject(long objectId, string tenantTypeId, int pageIndex);

        /// <summary>
        /// 根据收藏对象获取同样收藏此对象的关注用户
        /// </summary>
        /// <param name="objectId">对象Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        IEnumerable<long> GetFollowedUserIdsOfObject(long objectId, long userId, string tenantTypeId);

        /// <summary>
        /// 获取被收藏数
        /// </summary>
        /// <param name="objectId">收藏对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        int GetFavoritedUserCount(long objectId, string tenantTypeId);
    }
}
