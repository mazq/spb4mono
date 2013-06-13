using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;

namespace Tunynet.Common
{ /// <summary>
    /// 顶踩记录的数据访问接口
    /// </summary>
    public interface IAttitudeRecordRepository : IRepository<AttitudeRecord>
    {
        /// <summary>
        /// 获取参与用户的Id集合
        /// </summary>
        /// <param name="objectId">操作对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="IsSupport">用户是否支持（true为支持，false为反对）</param>
        /// <param name="topNumber">条数</param>
        IEnumerable<long> GetTopOperatedUserIds(long objectId, string tenantTypeId, bool IsSupport, int topNumber);

        /// <summary>
        /// 获取操作对象的Id集合
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        ///<param name="userId">用户ID</param>
        ///<param name="pageSize">每页的内容数</param>
        ///<param name="pageIndex">页码</param>
        PagingEntityIdCollection GetObjectIds(string tenantTypeId,long userId, int pageSize, int pageIndex);
    }
}