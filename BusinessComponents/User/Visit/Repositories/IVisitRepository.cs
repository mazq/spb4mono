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
    /// VisitRecord数据访问接口
    /// </summary>
    public interface IVisitRepository : IRepository<Visit>
    {
        /// <summary>
        /// 执行队列
        /// </summary>
        void ExecQueue();

        /// <summary>
        /// 删除N天前的访客记录
        /// </summary>
        /// <param name="beforeDays">间隔天数</param>
        void Clean(int? beforeDays);

        /// <summary>
        /// 根据用户id删除访客记录
        /// </summary>
        /// <param name="userId">用户的id</param>
        void CleanByUser(long userId);

        /// <summary>
        /// 删除被访问对象的所有访问记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型id</param>
        /// <param name="toObjectId">被访问对象id</param>
        void CleanByToObjectId(string tenantTypeId, long toObjectId);

        /// <summary>
        /// 获取访客记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="visitorId">访客用户Id</param>
        /// <param name="toObjectId">被访问对象Id</param>
        Visit Get(string tenantTypeId, long visitorId, long toObjectId);

        /// <summary>
        /// 获取访客记录（我去看过谁的内容）前topNumber条记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="visitorId">访客用户Id</param>
        /// <param name="topNumber">条数</param>
        /// <returns>访客记录列表（我去看过谁的内容）</returns>
        IEnumerable<Visit> GetTopMyVisits(string tenantTypeId, long visitorId, int topNumber);

        /// <summary>
        /// 获取访客记录（谁来看过我的内容）前topNumber条记录
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="toObjectId">被访问对象Id</param>
        /// <param name="topNumber">条数</param>
        /// <returns>访客记录列表（谁来看过我的内容）</returns>
        IEnumerable<Visit> GetTopVisits(string tenantTypeId, long toObjectId, int topNumber);


    }
}
