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

namespace Spacebuilder.Group
{
    /// <summary>
    ///群组成员申请Repository
    /// </summary>
    public class GroupMemberApplyRepository : Repository<GroupMemberApply>, IGroupMemberApplyRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取用户申请状态为待处理的群组ID集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public IEnumerable<long> GetPendingApplyGroupIdsOfUser(long userId)
        {
            
            //以下语句可以改为：RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion,"UserId",userId)+"PendingApplyGroupIdsOfUser"
            //已修改，前边的也要改吗
            string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId) + "PendingApplyGroupIdsOfUser";
            IEnumerable<long> groupIds = cacheService.Get<IEnumerable<long>>(cacheKey);
            if (groupIds == null)
            {
                
                //已修改
                Sql sql = Sql.Builder;
                sql.Select("GroupId")
                    .From("spb_GroupMemberApplies")
                    .Where("UserId = @0", userId)
                    .Where("ApplyStatus=@0", GroupMemberApplyStatus.Pending);
                groupIds = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey, groupIds, CachingExpirationType.UsualObjectCollection);
            }
            return groupIds;
        }

        
        //已加
        /// <summary>
        /// 获取群组的加入申请列表
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="applyStatus">申请状态</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>加入申请分页数据</returns>
        public PagingDataSet<GroupMemberApply> GetGroupMemberApplies(long groupId, GroupMemberApplyStatus? applyStatus, int pageSize, int pageIndex)
        {
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.UsualObjectCollection,
            () =>
            {
                StringBuilder cacheKey = new StringBuilder();
                cacheKey.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "GroupId", groupId));
                cacheKey.Append("GroupMemberApplies");
                
                //ok
                
                
                
                return cacheKey.ToString();
            },
            () =>
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("spb_GroupMemberApplies")
                    .Where("GroupId = @0", groupId);
                if (applyStatus.HasValue)
                {
                    sql.Where("ApplyStatus = @0", applyStatus.Value);
                }
                sql.OrderBy("ApplyStatus asc").OrderBy("ApplyDate desc");
                return sql;
            });
        }

        /// <summary>
        /// 获取成员请求书
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <returns></returns>
        public int GetMemberApplyCount(long groupId)
        {
            int version = RealTimeCacheHelper.GetAreaVersion("GroupId", groupId);
            string cacheKey = string.Format("MemberApplyCount:{0}::GroupId:{1}", version, groupId);

            int? count = cacheService.Get(cacheKey) as int?;

            if (count == null)
            {
                var sql = Sql.Builder;

                sql.Select("Count(Id)")
                   .From("spb_GroupMemberApplies")
                   .Where("GroupId = @0", groupId)
                   .Where("ApplyStatus=@0", GroupMemberApplyStatus.Pending);
                count = CreateDAO().ExecuteScalar<int>(sql);

                cacheService.Add(cacheKey, count, CachingExpirationType.UsualSingleObject);
            }

            return count == null ? 0 : count.Value;
        }
    }
}