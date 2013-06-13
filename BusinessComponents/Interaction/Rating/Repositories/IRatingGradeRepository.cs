//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common
{ /// <summary>
    /// 星级统计数据访问接口
    /// </summary>
    public interface IRatingGradeRepository : IRepository<RatingGrade>
    {
        /// <summary>
        /// 获取指定评价选项信息
        /// </summary>
        /// <param name="objectId">评价数据Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        IEnumerable<RatingGrade> GetRatingGrades(long objectId, string tenantTypeId);

        /// <summary>
        /// 清空相关联的等级统计信息
        /// </summary>
        /// <param name="objectId"> 操作Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        void ClearRatingGradesOfObjectId(long objectId, string tenantTypeId);
    }
}