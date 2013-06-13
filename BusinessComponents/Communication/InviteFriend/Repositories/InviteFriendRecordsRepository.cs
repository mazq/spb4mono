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
using Tunynet.Caching;
using PetaPoco;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 邀请好友的记录
    /// </summary>
    public class InviteFriendRecordsRepository : Repository<InviteFriendRecord>, IInviteFriendRecordsRepository
    {
        private int pageSize = 12;
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        //done:zhengw,by mazq 这个方法为什么要重写呢？
        //zhengw回复：已删除


        
        /// <summary>
        /// 获取我的邀请好友记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>被邀请的好友Id列表</returns>
        public PagingEntityIdCollection GetMyInviteFriendRecords(long userId, int pageIndex = 1)
        {
            PetaPocoDatabase dao = CreateDAO();

            //仿照其他分页方法做
            var sql = Sql.Builder;
            sql.Select("InvitedUserId")
                .From("tn_InviteFriendRecords")
                //done:bianchx by libsh，不需要加判断
                //回复：已经删除了对应的判断。
                .Where("UserId = @0", userId)
                .OrderBy("DateCreated desc");
            PagingEntityIdCollection peic = null;

            if (pageIndex <= CacheablePageCount)
            {
                string cacheKey = GetCacheKey_InviteUserIds(userId);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "InvitedUserId", sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, "InvitedUserId", sql);
            }

            return peic;
        }

        /// <summary>
        /// 通过被邀请人ID获取邀请人
        /// </summary>
        /// <param name="userId">被邀请人ID</param>
        /// <returns></returns>
        public InviteFriendRecord GetInvitingUserId(long userId)
        {
            var sql_Get = PetaPoco.Sql.Builder;
            PetaPocoDatabase dao = CreateDAO();
            sql_Get.Select("*")
                .From("tn_InviteFriendRecords")
                .Where("InvitedUserId = @0", userId);
            InviteFriendRecord record = dao.FirstOrDefault<InviteFriendRecord>(sql_Get);
            return record;
        }

        /// <summary>
        /// 获取被邀请的好友Id集合的CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        private string GetCacheKey_InviteUserIds(long userId)
        {
            //done:bianchx by libsh,版本号直接加到前缀上就行了
            //回复：已经修改了对应的方法
            string cacheKeyPrefix = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId);
            return string.Format("{0}::InviteUserIds", cacheKeyPrefix);
        }

        /// <summary>
        /// 清除用户资料（删除用户时使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            //清除用户数据的时候不需要考虑缓存
            PetaPoco.Sql sql_delete = PetaPoco.Sql.Builder.Append("Delete from tn_InviteFriendRecords where UserId=@0 or InvitedUserId=@0", userId);
            CreateDAO().Execute(sql_delete);
        }

        /// <summary>
        /// 记录邀请用户奖励
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void RewardingUser(long userId)
        {
            var sql_Set = PetaPoco.Sql.Builder
                .Append("update tn_InviteFriendRecords set InvitingUserHasBeingRewarded = 1 where userId = @0", userId);
            CreateDAO().Execute(sql_Set);
        }

    }
}
