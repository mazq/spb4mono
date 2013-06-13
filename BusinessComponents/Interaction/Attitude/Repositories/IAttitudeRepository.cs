using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;

namespace Tunynet.Common
{  /// <summary>
    /// 顶踩的数据访问接口
    /// </summary>
    public interface IAttitudeRepository : IRepository<Attitude>
    {
        /// <summary>
        /// 获取顶踩信息
        /// </summary>
        /// <param name="objectId">操作对象</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="isFromCache">是否从缓存中获取</param>
        Attitude Get(long objectId, string tenantTypeId, bool isFromCache = true);

        /// <summary>
        /// 对操作对象进行顶操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">操作用户Id</param>
        /// <param name="mode">顶踩的操作模式</param>
        /// <returns>是否操作成功，Ture-成功</returns>
        bool Support(long objectId, string tenantTypeId, long userId, AttitudeMode mode = AttitudeMode.Bidirection);

        /// <summary>
        /// 对操作对象进行踩操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">操作用户Id</param>
        /// <returns>是否操作成功，Ture-成功</returns>
        bool Oppose(long objectId, string tenantTypeId, long userId);

        /// <summary>
        /// 用户当前操作
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="userId">用户的UserId</param>
        /// <returns>用户当前所做的操作:True-顶,false-踩,null-未做任何操作</returns>
        bool? IsSupport(long objectId, string tenantTypeId, long userId);

        /// <summary>
        /// 获取操作对象的Id集合
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<param name="sortBy">顶踩排序字段</param>
        ///<param name="pageSize">每页的内容数</param>
        ///<param name="pageIndex">页码</param>
        PagingEntityIdCollection GetObjectIds(string tenantTypeId, SortBy_Attitude sortBy, int pageSize, int pageIndex);
    }
}