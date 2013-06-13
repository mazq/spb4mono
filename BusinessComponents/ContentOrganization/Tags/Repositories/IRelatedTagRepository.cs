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
    /// 相关标签仓储接口，实现特殊方法
    /// </summary>
    public interface IRelatedTagRepository : IRepository<RelatedTag>
    {
        /// <summary>
        /// 添加相关标签
        /// </summary>
        /// <remarks>
        /// 会为标签添加双向的关联关系,例如:
        /// TagA关联到TagB
        /// TagB关联到TagA
        /// </remarks>
        /// <param name="tagNames">相关标签名称集合</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tagId">标签Id</param>
        /// <returns> 影响行数</returns>
        int AddRelatedTagsToTag(string[] tagNames, string tenantTypeId, long ownerId, long tagId);

        /// <summary>
        /// 清除关联的标签
        /// </summary>
        /// <remarks>会删除双向的关联关系</remarks>
        /// <param name="relatedTagId">关联的标签Id</param>
        /// <param name="tagId">被关联的标签Id</param>
        int DeleteRelatedTagFromTag(long relatedTagId, long tagId);

        /// <summary>
        /// 清除拥有者的所有标签
        /// </summary>
        /// <param name="tagId">被关联的标签Id</param>
        int ClearRelatedTagsFromTag(long tagId);

        /// <summary>
        /// 获取相关标签
        /// </summary>
        /// <param name="tagId">被关联的标签Id</param>
        /// <returns>获取相关联的Id集合</returns>
        IEnumerable<long> GetRelatedTagIds(long tagId);
    }
}