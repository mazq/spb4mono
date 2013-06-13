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
    /// 身份认证申请接口
    /// </summary>
    public interface IIdentificationRepository : IRepository<Identification>
    {
        /// <summary>
        ///分页检索身份认证
        /// </summary>
        ///<param name="query">查询条件</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        PagingDataSet<Identification> GetIdentifications(IdentificationQuery query, int pageIndex, int pageSize);

        /// <summary>
        /// 获取某人的某项(或所有)认证实体
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="IdentificationTypeId">身份标识ID</param>
        /// <returns></returns>
        List<Identification> GetUserIdentifications(long userId, long IdentificationTypeId = 0);

        /// <summary>
        /// 获取身份认证标识
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="status">是否只获取通过认证的认证标识</param>
        /// <returns></returns>
        IEnumerable<IdentificationType> GetIdentificationTypes(long userId,bool status=true);
    }
}
