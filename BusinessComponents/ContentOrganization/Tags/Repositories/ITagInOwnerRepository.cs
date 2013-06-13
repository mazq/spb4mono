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
    /// 标签和拥有者关联项，需要的数据服务接口
    /// </summary>
    public interface ITagInOwnerRepository : IRepository<TagInOwner>
    {
        /// <summary>
        /// 添加标签与拥有者关联
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns>返回影响行数</returns>
        int AddTagInOwner(string tagName, string tenantTypeId, long ownerId);

        /// <summary>
        /// 添加标签与拥有者关联
        /// </summary>
        ///<param name="tagInOwner">待创建实体</param>
        /// <returns>返回实体主键</returns>
        long AddTagInOwner(TagInOwner tagInOwner);

        /// <summary>
        /// 清除拥有者的所有标签
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        int ClearTagsFromOwner(long ownerId, string tenantTypeId);

        /// <summary>
        /// 获取标签标签与拥有者关系
        /// </summary>
        /// <param name="ownerId">拥有者</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        IEnumerable<TagInOwner> GetTagInOwners(long ownerId, string tenantTypeId);

        /// <summary>
        /// 分页获取tn_TagsInOwners表的数据(用于建索引)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagingDataSet<TagInOwner> GetTagInOwners(int pageIndex, int pageSize);

        /// <summary>
        /// 获取拥有者的标签列表
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="keyword">标签关键字</param>
        /// <param name="topNumber">前N个标签</param>
        IEnumerable<TagInOwner> GetTopTagInOwners(long ownerId, string tenantTypeId, string keyword, int topNumber);
    }
}