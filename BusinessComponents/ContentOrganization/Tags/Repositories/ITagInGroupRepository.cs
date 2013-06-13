//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 标签与分组关联仓储接口，实现特殊方法
    /// </summary>
    public interface ITagInGroupRepository : IRepository<TagInGroup>
    {
        /// <summary>
        /// 添加标签与分组关联
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="groupId">拥有者Id</param>
        /// <param name="TenantTypeId">租户类型Id</param>
        int AddTagInGroup(string tagName, long groupId, string TenantTypeId);


        /// <summary>
        /// 批量添加标签与分组关联
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="groupId">拥有者Id</param>
        /// <param name="TenantTypeId">租户类型Id</param>
        int BatchAddTagsInGroup(IEnumerable<string> tagNames, long groupId, string TenantTypeId);

        /// <summary>
        /// 清除分组的所有标签
        /// </summary>
        /// <param name="groupId">分组Id</param>
        int ClearTagsFromGroup(long groupId);

        /// <summary>
        /// 获取分组下的标签
        /// </summary>
        /// <param name="groupId">分组Id</param>
        IEnumerable<string> GetTagsOfGroup(long groupId);

        /// <summary>
        /// 批量添加分组给标签
        /// </summary>
        /// <param name="groupIds">分组Id集合</param>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        int BatchAddGroupsToTag(IEnumerable<long> groupIds, string tagName, string tenantTypeId);
    }
}