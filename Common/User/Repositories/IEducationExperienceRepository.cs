//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户教育信息接口
    /// </summary>
    public interface IEducationExperienceRepository : IRepository<EducationExperience>
    {
        /// <summary>
        /// 获取所有教育信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<long> GetEducationExperienceOfUser(long userId);

        /// <summary>
        /// 根据用户ID列表获取教育经历ID列表，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns>教育经历ID列表</returns>
        IEnumerable<long> GetEntityIdsByUserIds(IEnumerable<long> userIds);
    }
}