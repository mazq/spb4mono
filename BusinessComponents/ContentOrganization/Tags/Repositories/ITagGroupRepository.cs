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
    /// 标签分组仓储接口，实现特殊方法
    /// </summary>
    public interface ITagGroupRepository : IRepository<TagGroup>
    {
        /// <summary>
        /// 获取全部标签分组
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        IEnumerable<TagGroup> GetGroups(string tenantTypeId);

        /// <summary>
        /// 根据标签获取标签分组
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        IEnumerable<TagGroup> GetGroupsOfTag(string tagName, string tenantTypeId);
    }
}