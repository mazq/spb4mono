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
    /// @用户Repository接口
    /// </summary>
    public interface IAtUserRepository : IRepository<AtUserEntity>
    {

        /// <summary>
        /// 批量创建At用户
        /// </summary>
        /// <param name="userIds">用户Id集合</param>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        bool BatchCreateAtUser(List<long> userIds, long associateId, string tenantTypeId);

        /// <summary>
        /// 获取用户关联内容的Id分页集合
        /// </summary>
        /// <param name="userId">关联用户Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingDataSet<long> GetPagingAssociateIds(long userId, string tenantTypeId, int pageIndex);

        /// <summary>
        /// 获取用户关联内容的用户名集合
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        List<long> GetAtUserIds(long associateId, string tenantTypeId);

        /// <summary>
        /// 清除关注用户
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        void ClearAtUsers(long associateId, string tenantTypeId);
    }
}