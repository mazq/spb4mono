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
    /// 群组成员仓储接口
    /// </summary>
    public interface IGroupMemberRepository : IRepository<GroupMember>
    {
        /// <summary>
        /// 获取单个群组成员
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        GroupMember GetMember(long groupId, long userId);

        /// <summary>
        /// 获取群组所有成员用户Id集合(用于推送动态）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        IEnumerable<long> GetUserIdsOfGroup(long groupId);

        /// <summary>
        /// 获取群组管理员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns>若没有找到，则返回空集合</returns>
        IEnumerable<long> GetGroupManagers(long groupId);

        /// <summary>
        /// 获取群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="hasManager">是否包含管理员</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>群组成员分页数据</returns>
        PagingDataSet<GroupMember> GetGroupMembers(long groupId, bool hasManager, SortBy_GroupMember sortBy, int pageSize, int pageIndex);


        /// <summary>
        /// 获取我关注的用户中同时加入某个群组的群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        IEnumerable<GroupMember> GetGroupMembersAlsoIsMyFollowedUser(long groupId, long userId);
        /// <summary>
        /// 在线群组成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IEnumerable<GroupMember> GetOnlineGroupMembers(long groupId);

        /// <summary>
        /// 获取群组下的所有成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IEnumerable<GroupMember> GetAllMembersOfGroup(long groupId);
    }
}