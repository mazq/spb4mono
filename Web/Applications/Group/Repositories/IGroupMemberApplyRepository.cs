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
using Tunynet;
using Tunynet.Common;
using Tunynet.Caching;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组成员申请仓储接口
    /// </summary>
    public interface IGroupMemberApplyRepository : IRepository<GroupMemberApply>
    {
        /// <summary>
        /// 获取用户申请状态为待处理的群组ID集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        IEnumerable<long> GetPendingApplyGroupIdsOfUser(long userId);

        /// <summary>
        /// 获取群组的加入申请列表
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>加入申请分页数据</returns>
        PagingDataSet<GroupMemberApply> GetGroupMemberApplies(long groupId, GroupMemberApplyStatus? applyStatus, int pageSize, int pageIndex);

        /// <summary>
        /// 获取成员请求书
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        int GetMemberApplyCount(long groupId);

    }
}