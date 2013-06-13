//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Common;
using System.Linq;
using Tunynet.Utilities;
using Spacebuilder.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    ///群组成员申请Repository
    /// </summary>
    public class GroupMemberRepository : Repository<GroupMember>, IGroupMemberRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 删除群组成员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int Delete(GroupMember entity)
        {
            int affectCount = base.Delete(entity);
            if (affectCount > 0)
            {
                List<Sql> sqls = new List<Sql>();
                sqls.Add(Sql.Builder.Append("update spb_Groups set MemberCount = MemberCount - 1 where GroupId = @0", entity.GroupId));
                sqls.Add(Sql.Builder.Append("delete from spb_GroupMemberApplies where UserId = @0 and GroupId = @1", entity.UserId, entity.GroupId));
                CreateDAO().Execute(sqls);
                
                //已修改
            }
            return affectCount;
        }

        /// <summary>
        /// 添加群组成员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override object Insert(GroupMember entity)
        {
            Sql sql = Sql.Builder;
            
            
            sql.Select("count(*)")
                .From("spb_GroupMembers")
                .Where("UserId = @0 and GroupId = @1", entity.UserId, entity.GroupId);
            int result = CreateDAO().FirstOrDefault<int>(sql);
            if (result > 0)
            {
                return 0;
            }
            else
            {
                Sql updateSql = Sql.Builder;
                updateSql.Append("update spb_Groups set MemberCount = MemberCount + 1 where GroupId = @0", entity.GroupId);
                CreateDAO().Execute(updateSql);
                return base.Insert(entity);
            }
        }

        /// <summary>
        /// 获取单个群组成员
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public GroupMember GetMember(long groupId, long userId)
        {
            
            //已修改
            
            string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "GroupId", groupId) + "SingleMember" + userId;
            GroupMember groupMember = cacheService.Get<GroupMember>(cacheKey);

            if (groupMember == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("spb_GroupMembers")
                    .Where("GroupId = @0 and UserId = @1", groupId, userId);
                groupMember = CreateDAO().FirstOrDefault<GroupMember>(sql);
                cacheService.Add(cacheKey, groupMember, CachingExpirationType.SingleObject);
            }
            return groupMember;
        }

        /// <summary>
        /// 获取群组所有成员用户Id集合(用于推送动态）
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetUserIdsOfGroup(long groupId)
        {
            Sql sql = Sql.Builder;
            sql.Select("UserId")
                .From("spb_GroupMembers")
                .Where("GroupId = @0", groupId);
            IEnumerable<long> userIds = CreateDAO().Fetch<long>(sql);
            return userIds;
        }

        /// <summary>
        /// 获取群组管理员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns>若没有找到，则返回空集合</returns>
        public IEnumerable<long> GetGroupManagers(long groupId)
        {
            string cacheKey = "GroupManagers" + groupId + "-" + RealTimeCacheHelper.GetAreaVersion("GroupId", groupId);
            IEnumerable<long> managerIds = cacheService.Get<IEnumerable<long>>(cacheKey);
            if (managerIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("UserId")
                    .From("spb_GroupMembers")
                    .Where("GroupId = @0 and IsManager = 1", groupId);
                managerIds = CreateDAO().Fetch<long>(sql);
                
                //已修改
                cacheService.Add(cacheKey, managerIds, CachingExpirationType.UsualObjectCollection);
            }
            return managerIds;
        }

        /// <summary>
        /// 获取群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="hasManager">是否包含管理员</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>       
        /// <returns>群组成员分页数据</returns>
        public PagingDataSet<GroupMember> GetGroupMembers(long groupId, bool hasManager, SortBy_GroupMember sortBy, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
                () =>
                {
                    StringBuilder cacheKey = new StringBuilder();
                    cacheKey.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "GroupId", groupId));
                    cacheKey.AppendFormat("PagingGroupMembers::GroupId-{0}:hasManager-{1}", groupId, hasManager);
                    return cacheKey.ToString();
                },
                () =>
                {
                    Sql sql = Sql.Builder;
                    sql.Select("*")
                        .From("spb_GroupMembers")
                        .Where("GroupId = @0", groupId);
                    if (!hasManager)
                        sql.Where("IsManager = 0");
                    sql.OrderBy("IsManager desc");
                    switch (sortBy)
                    {
                        case SortBy_GroupMember.DateCreated_Asc:
                            sql.OrderBy("JoinDate asc");
                            break;
                        case SortBy_GroupMember.DateCreated_Desc:
                            sql.OrderBy("JoinDate desc");
                            break;
                        default:
                            sql.OrderBy("JoinDate asc");
                            break;
                    }
                    return sql;
                });
        }

        /// <summary>
        /// 获取我关注的用户中同时加入某个群组的群组成员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">当前用户的userId</param>
        /// <returns></returns>
        public IEnumerable<GroupMember> GetGroupMembersAlsoIsMyFollowedUser(long groupId, long userId)
        {
            string cacheKey = GetCacheKey_GroupMembersAlsoIsMyFollowedUser(groupId, userId);
            IEnumerable<long> groupMemberIds = cacheService.Get<IList<long>>(cacheKey);

            if (groupMemberIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("distinct spb_GroupMembers.Id")
                   .From("spb_GroupMembers")
                   .InnerJoin("tn_Follows F")
                   .On("F.FollowedUserId = spb_GroupMembers.UserId")
                   .Where("spb_GroupMembers.GroupId = @0", groupId)
                   .Where("F.UserId = @0", userId);
                IEnumerable<object> temGroupMemberIds = CreateDAO().FetchTopPrimaryKeys(PrimaryMaxRecords, "spb_GroupMembers.Id", sql);
                groupMemberIds = temGroupMemberIds.Cast<long>();
                cacheService.Add(cacheKey, groupMemberIds, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds<long>(groupMemberIds);
        }
        /// <summary>
        /// 在线群组成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<GroupMember> GetOnlineGroupMembers(long groupId)
        {
            string cacheKey = GetCacheKey_OnlineGroupMembers(groupId);
            IEnumerable<long> groupMemberIds = cacheService.Get<IList<long>>(cacheKey);
            if (groupMemberIds == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("spb_GroupMembers.Id")
                    .From("spb_GroupMembers")
                    .InnerJoin("tn_OnlineUsers")
                    .On("tn_OnlineUsers.UserId=spb_GroupMembers.UserId")
                    .Where("spb_GroupMembers.GroupId=@0", groupId);
                IEnumerable<object> tempIds = CreateDAO().FetchTopPrimaryKeys(PrimaryMaxRecords, "spb_GroupMembers.Id", sql);
                groupMemberIds = tempIds.Cast<long>();
                cacheService.Add(cacheKey, groupMemberIds, CachingExpirationType.ObjectCollection);
            }
            return PopulateEntitiesByEntityIds<long>(groupMemberIds);
        }

        private string GetCacheKey_GroupMembersAlsoIsMyFollowedUser(long groupId, long userId)
        {
            return string.Format("GroupMembersAlsoIsMyFollowedUser:groupId-{0}:userId-{1}", groupId, userId);
        }

        private string GetCacheKey_OnlineGroupMembers(long groupId)
        {
            return string.Format("OnlineGroupMembers:groupId-{0}", groupId);
        }



        /// <summary>
        /// 获取群组下的所有成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<GroupMember> GetAllMembersOfGroup(long groupId)
        {
            var sql = Sql.Builder;
            sql.Select("*")
            .From("spb_GroupMembers")
            .Where("GroupId=@0", groupId);
            IEnumerable<GroupMember> members = CreateDAO().Fetch<GroupMember>(sql);
            return members;
        }
    }
}