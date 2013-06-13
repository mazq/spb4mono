//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Text;
using System.Reflection;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 关注用户Repository
    /// </summary>
    public class FollowRepository : Repository<FollowEntity>, IFollowRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();


        /// <summary>
        /// 把实体follow更新到数据库
        /// </summary>
        /// <param name="follow"></param>
        public override void Update(FollowEntity follow)
        {
            if (follow == null)
                return;

            base.Update(follow);

            #region 更新缓存

            string cacheKey = GetCacheKey_FollowUserId_NoteName(follow.UserId);

            Dictionary<long, string> id_Notename = cacheService.Get<Dictionary<long, string>>(cacheKey);

            id_Notename = id_Notename ?? new Dictionary<long, string>();
            id_Notename[follow.FollowedUserId] = follow.NoteName;

            cacheService.Set(cacheKey, id_Notename, CachingExpirationType.UsualObjectCollection);

            #endregion
        }

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被关注用户Id</param>
        /// <param name="isQuietly">是否悄悄关注</param>
        public bool Follow(long userId, long followedUserId, bool isQuietly = false)
        {
            if (userId <= 0 || followedUserId <= 0 || userId == followedUserId)
                return false;

            int affectCount = 0;
            //是否互相关注
            bool isMutual = false;
            List<Sql> sqls = new List<Sql>();

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            //关注用户
            FollowEntity reverseFollow = Get(followedUserId, userId);
            if (reverseFollow != null)
            {
                if (!reverseFollow.IsQuietly && !isQuietly)
                {
                    //如果双方都是关注 则状态为互相关注
                    isMutual = true;
                    sqls.Add(Sql.Builder.Append("update tn_Follows set IsMutual = @0 where UserId = @1 and FollowedUserId = @2", isMutual, followedUserId, userId));
                    dao.Execute(sqls);
                }
            }

            FollowEntity follow = dao.FirstOrDefault<FollowEntity>("select * from tn_Follows where UserId = @0 and FollowedUserId = @1", userId, followedUserId);
            if (follow != null && !follow.IsQuietly)
                return false;
            
            if (follow == null || follow.Id == 0)
            {
                sqls.Add(Sql.Builder.Append("INSERT INTO tn_Follows (UserId, FollowedUserId, IsQuietly, IsNewFollower, DateCreated, IsMutual) VALUES (@0,@1,@2,@3,@4,@5)", userId, followedUserId, isQuietly, !isQuietly, DateTime.UtcNow, isMutual));
                //  更新用户的关注数
                sqls.Add(Sql.Builder.Append("UPDATE tn_Users SET FollowedCount = FollowedCount + 1 WHERE UserId = @0 ", userId));
            }
            else if (follow.IsQuietly && !isQuietly)
            {
                sqls.Add(Sql.Builder.Append("update tn_Follows set IsQuietly = 0,IsMutual = @2 where UserId = @0 and FollowedUserId = @1", userId, followedUserId, isMutual));
            }

            if (!isQuietly)
            {
                //如果用户不是悄悄关注则对方用户粉丝数增加
                sqls.Add(Sql.Builder.Append("UPDATE tn_Users SET FollowerCount = FollowerCount + 1 WHERE UserId = @0 ", followedUserId));
            }

            using (var transaction = dao.GetTransaction())
            {
                affectCount = dao.Execute(sqls);
                transaction.Complete();
            }

            dao.CloseSharedConnection();

            #region 更新缓存

            if (affectCount > 0)
            {
                RealTimeCacheHelper userRealTimeCacheHelper = EntityData.ForType(typeof(IUser)).RealTimeCacheHelper;
                userRealTimeCacheHelper.IncreaseEntityCacheVersion(userId);
                if (!isQuietly)
                {
                    //如果不是悄悄的就要更新缓存
                    userRealTimeCacheHelper.IncreaseEntityCacheVersion(followedUserId);
                    //更新粉丝列表
                    RealTimeCacheHelper.IncreaseAreaVersion("UserId", followedUserId);
                    //更新对方新增粉丝数
                    string cacheKey_Count = GetCacheKey_FollowerCount(followedUserId);
                    int? followerCount = cacheService.Get(cacheKey_Count) as int?;
                    if (followerCount != null)
                    {
                        cacheService.Set(cacheKey_Count, followerCount + 1, CachingExpirationType.UsualSingleObject);
                    }
                }

                if (isMutual)
                {
                    List<long> mutualFolloweds = null;
                    //获取双向关注的CacheKey
                    string cacheKey_MutualFollowed = GetCacheKey_IsMutualFollowed(userId);
                    mutualFolloweds = cacheService.Get<List<long>>(cacheKey_MutualFollowed);
                    if (mutualFolloweds != null)
                    {
                        mutualFolloweds.Add(followedUserId);
                        cacheService.Set(cacheKey_MutualFollowed, mutualFolloweds, CachingExpirationType.UsualObjectCollection);
                    }

                    cacheKey_MutualFollowed = GetCacheKey_IsMutualFollowed(followedUserId);
                    mutualFolloweds = cacheService.Get<List<long>>(cacheKey_MutualFollowed);
                    if (mutualFolloweds != null)
                    {
                        mutualFolloweds.Add(userId);
                        cacheService.Set(cacheKey_MutualFollowed, mutualFolloweds, CachingExpirationType.UsualObjectCollection);
                    }
                }

                //更新关注列表
                RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
                string cacheKey = GetCacheKey_FollowUserId_NoteName(userId);
                Dictionary<long, string> id_NoteName = cacheService.Get<Dictionary<long, string>>(cacheKey);
                if (id_NoteName != null)
                {
                    id_NoteName[followedUserId] = string.Empty;
                    cacheService.Set(cacheKey, id_NoteName, CachingExpirationType.UsualObjectCollection);
                }
            }

            #endregion 更新缓存

            return affectCount > 0;
        }

        /// <summary>
        /// 判断是否关注了被判定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        public bool IsFollowed(long userId, long toUserId)
        {
            if (userId <= 0 || toUserId <= 0)
                return false;

            string cacheKey = GetCacheKey_FollowUserId_NoteName(userId);
            Dictionary<long, string> id_NoteName = cacheService.Get<Dictionary<long, string>>(cacheKey);
            if (id_NoteName == null)
            {
                var sql = Sql.Builder;
                sql.Select("FollowedUserId , NoteName")
                   .From("tn_Follows")
                   .Where("UserId = @0", userId);

                IEnumerable<dynamic> iRelevance = CreateDAO().Fetch<dynamic>(sql);

                id_NoteName = new Dictionary<long, string>();

                foreach (var item in iRelevance)
                {
                    id_NoteName[item.FollowedUserId] = item.NoteName;
                }
                cacheService.Add(cacheKey, id_NoteName, CachingExpirationType.UsualObjectCollection);
            }

            return id_NoteName != null && id_NoteName.ContainsKey(toUserId);
        }

        /// <summary>
        /// 是否为双向关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        public bool IsMutualFollowed(long userId, long toUserId)
        {
            string cacheKey = GetCacheKey_IsMutualFollowed(userId);
            List<long> mutualFolloweds = cacheService.Get<List<long>>(cacheKey);
            if (mutualFolloweds == null)
            {
                var sql = Sql.Builder;
                sql.Select("FollowedUserId")
                   .From("tn_Follows")
                   .Where("UserId = @0 and IsMutual = 1", userId);

                mutualFolloweds = CreateDAO().Fetch<long>(sql);
                if (mutualFolloweds != null)
                {
                    cacheService.Add(cacheKey, mutualFolloweds, CachingExpirationType.UsualObjectCollection);
                }
            }

            return mutualFolloweds.Contains(toUserId);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被关注用户Id</param>
        public void CancelFollow(long userId, long followedUserId)
        {
            if (userId <= 0 || followedUserId <= 0)
                return;
            int affectCount = 0;
            List<Sql> sqls = new List<Sql>();

            FollowEntity follow = Get(userId, followedUserId);
            //如果有关注过记录则从数据库中删除
            if (follow != null)
            {
                //删除关注记录
                sqls.Add(Sql.Builder.Append("delete tn_Follows where UserId =@0 and FollowedUserId=@1", userId, followedUserId));
                //  更新用户的关注数
                sqls.Add(Sql.Builder.Append("UPDATE tn_Users SET  FollowedCount = FollowedCount-1  WHERE UserId =@0 ", userId));
                if (!follow.IsQuietly)
                {
                    //如果用户不是悄悄关注而对方用户粉丝数增加
                    sqls.Add(Sql.Builder.Append("UPDATE tn_Users SET  FollowerCount = FollowerCount-1  WHERE UserId =@0 ", followedUserId));
                }

                PetaPocoDatabase dao = CreateDAO();
                using (var transaction = dao.GetTransaction())
                {
                    affectCount = dao.Execute(sqls);
                    transaction.Complete();
                }

                if (affectCount > 0)
                {
                    if (RealTimeCacheHelper.EnableCache)
                    {
                        RealTimeCacheHelper userRealTimeCacheHelper = EntityData.ForType(typeof(IUser)).RealTimeCacheHelper;
                        userRealTimeCacheHelper.IncreaseEntityCacheVersion(userId);
                        if (!follow.IsQuietly)
                        {
                            //如果不是悄悄的就要更新对方用户缓存
                            userRealTimeCacheHelper.IncreaseEntityCacheVersion(followedUserId);
                        }

                        cacheService.MarkDeletion(GetCacheKey_Follow(userId, followedUserId), follow, CachingExpirationType.SingleObject);

                        //更新粉丝的列表 递增版本号
                        RealTimeCacheHelper.IncreaseAreaVersion("UserId", followedUserId);
                        RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);

                        //获取双向关注的CacheKey
                        string cacheKey = GetCacheKey_IsMutualFollowed(userId);
                        List<long> mutualFolloweds = cacheService.Get<List<long>>(cacheKey);
                        if (mutualFolloweds != null)
                        {
                            if (mutualFolloweds.Contains(followedUserId))
                            {
                                mutualFolloweds.Remove(followedUserId);
                                Sql sql = Sql.Builder;
                                sql.Append("update tn_Follows set IsMutual =0 where UserId=@0 and FollowedUserId=@1", followedUserId, userId);
                                dao.Execute(sql);
                                cacheService.Set(cacheKey, mutualFolloweds, CachingExpirationType.UsualObjectCollection);
                            }
                        }

                        cacheKey = GetCacheKey_FollowUserId_NoteName(userId);
                        Dictionary<long, string> id_NoteName = cacheService.Get<Dictionary<long, string>>(cacheKey);
                        if (id_NoteName != null)
                        {
                            if (id_NoteName.ContainsKey(followedUserId))
                            {
                                //移除缓存的记录
                                id_NoteName.Remove(followedUserId);
                                cacheService.Set(cacheKey, id_NoteName, CachingExpirationType.UsualObjectCollection);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取用户的最新粉丝数
        /// </summary>
        /// <param name="userId">用户Id</param>
        public int GetNewFollowerCount(long userId)
        {
            if (userId <= 0)
                return 0;

            var sql = Sql.Builder;
            string cacheKey = GetCacheKey_FollowerCount(userId);

            int? followerCount = cacheService.Get(cacheKey) as int?;
            if (followerCount == null)
            {
                sql.Select("COUNT(*)")
                   .From("tn_Follows")
                   .Where("FollowedUserId = @0 and IsNewFollower = 1", userId);

                followerCount = CreateDAO().FirstOrDefault<int>(sql);
                cacheService.Add(cacheKey, followerCount, CachingExpirationType.UsualSingleObject);
            }

            return followerCount ?? 0;
        }

        /// <summary>
        /// 清除最新用户统计
        /// </summary>
        ///<param name="userId">用户Id</param>
        /// <returns></returns>
        public void ClearNewFollowerCount(long userId)
        {
            if (userId <= 0)
                return;

            string cacheKey = GetCacheKey_FollowerCount(userId);
            int? followerCount = cacheService.Get(cacheKey) as int?;

            if (followerCount == null || followerCount > 0)
            {
                var sql = Sql.Builder.Append("update tn_Follows set IsNewFollower = 0 where FollowedUserId = @0 and IsNewFollower = 1", userId);
                CreateDAO().Execute(sql);
                cacheService.Set(cacheKey, 0, CachingExpirationType.UsualSingleObject);
            }
        }

        /// <summary>
        /// 移除用户的粉丝
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followerUserId">粉丝的用户Id</param>
        public void RemoveFollower(long userId, long followerUserId)
        {
            if (userId <= 0 || followerUserId <= 0)
                return;

            List<Sql> sqls = new List<Sql>();
            int affectCount = 0;
            FollowEntity follow = Get(followerUserId, userId);

            if (follow != null)
            {
                sqls.Add(Sql.Builder.Append("delete tn_Follows where UserId = @0 and FollowedUserId = @1", followerUserId, userId));
                sqls.Add(Sql.Builder.Append("UPDATE tn_Users SET FollowedCount = FollowedCount - 1 WHERE UserId = @0 ", followerUserId));
                sqls.Add(Sql.Builder.Append("UPDATE tn_Users SET FollowerCount = FollowerCount - 1 WHERE UserId = @0 ", userId));

                PetaPocoDatabase dao = CreateDAO();
                using (var transaction = dao.GetTransaction())
                {
                    affectCount = dao.Execute(sqls);
                    transaction.Complete();
                }

                if (affectCount > 0)
                {
                    if (RealTimeCacheHelper.EnableCache)
                    {
                        RealTimeCacheHelper userRealTimeCacheHelper = EntityData.ForType(typeof(IUser)).RealTimeCacheHelper;
                        userRealTimeCacheHelper.IncreaseEntityCacheVersion(userId);
                        userRealTimeCacheHelper.IncreaseEntityCacheVersion(followerUserId);

                        cacheService.MarkDeletion(GetCacheKey_Follow(userId, followerUserId), follow, CachingExpirationType.SingleObject);

                        //更新粉丝列表的缓存
                        RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
                        RealTimeCacheHelper.IncreaseAreaVersion("UserId", followerUserId);

                        string cacheKey = GetCacheKey_FollowUserId_NoteName(followerUserId);
                        Dictionary<long, string> id_NoteName = cacheService.Get<Dictionary<long, string>>(cacheKey);
                        if (id_NoteName != null)
                        {
                            if (id_NoteName.ContainsKey(userId))
                            {
                                id_NoteName.Remove(userId);
                                cacheService.Set(cacheKey, id_NoteName, CachingExpirationType.UsualObjectCollection);
                            }
                        }

                        cacheKey = GetCacheKey_IsMutualFollowed(userId);
                        List<long> mutualFolloweds = cacheService.Get<List<long>>(cacheKey);
                        if (mutualFolloweds != null && mutualFolloweds.Contains(followerUserId))
                        {
                            mutualFolloweds.Remove(followerUserId);
                            cacheService.Set(cacheKey, mutualFolloweds, CachingExpirationType.UsualObjectCollection);
                        }

                        cacheKey = GetCacheKey_IsMutualFollowed(followerUserId);
                        mutualFolloweds = cacheService.Get<List<long>>(cacheKey);
                        if (mutualFolloweds != null && mutualFolloweds.Contains(userId))
                        {
                            mutualFolloweds.Remove(userId);
                            cacheService.Set(cacheKey, mutualFolloweds, CachingExpirationType.UsualObjectCollection);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取关注用户实体
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被操作用户Id</param>
        public FollowEntity Get(long userId, long followedUserId)
        {
            if (userId <= 0 || followedUserId <= 0)
                return null;

            string cacheKey = GetCacheKey_Follow(userId, followedUserId);
            FollowEntity entity = cacheService.Get<FollowEntity>(cacheKey);

            if (entity == null)
            {
                var sql = Sql.Builder;
                sql.Where("UserId = @0 and FollowedUserId = @1", userId, followedUserId);

                entity = CreateDAO().FirstOrDefault<FollowEntity>(sql);
                if (RealTimeCacheHelper.EnableCache)
                {
                    cacheService.Add(cacheKey, entity, CachingExpirationType.SingleObject);
                }
            }

            return entity;
        }

        /// <summary>
        /// 获取用户的备注名称
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被关注用户Id</param>
        /// <returns></returns>
        public string GetNoteName(long userId, long followedUserId)
        {
            if (userId <= 0 || followedUserId <= 0)
                return string.Empty;

            string cacheKey = GetCacheKey_FollowUserId_NoteName(userId);
            Dictionary<long, string> id_NoteName = cacheService.Get<Dictionary<long, string>>(cacheKey);
            if (id_NoteName == null)
            {
                var sql = Sql.Builder;
                sql.Select("FollowedUserId, NoteName")
                   .From("tn_Follows")
                   .Where("UserId = @0", userId);

                IEnumerable<dynamic> iId_NoteName = CreateDAO().Fetch<dynamic>(sql);

                id_NoteName = iId_NoteName.ToDictionary<dynamic, long, string>(n => n.FollowedUserId, n => n.NoteName);
                if (id_NoteName != null)
                {
                    cacheService.Add(cacheKey, id_NoteName, CachingExpirationType.UsualObjectCollection);
                }
            }

            return id_NoteName != null && id_NoteName.ContainsKey(followedUserId) ? id_NoteName[followedUserId] : string.Empty;
        }

        /// <summary>
        /// 获取前N条关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">需要获取的记录数</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowedUserIds(long userId, int topNumber, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            if (userId <= 0)
                return null;

            IEnumerable<long> userIds = GetPortionFollowedUserIds(userId, groupId, sortBy);

            return userIds.Take(topNumber <= MaxTopNumber ? topNumber : MaxTopNumber);
        }

        /// <summary>
        /// 获取关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        public IEnumerable<long> GetPortionFollowedUserIds(long userId, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            IEnumerable<long> userIds = null;

            string cacheKey = GetCacheKey_GetTopFollowedUserIds(userId, groupId, sortBy);
            userIds = cacheService.Get<IEnumerable<long>>(cacheKey);
            if (userIds == null)
            {
                string primaryKey = string.Empty;
                var sql = Sql.Builder;

                if (!groupId.HasValue || groupId < 0)
                {
                    primaryKey = "FollowedUserId";
                    sql.Select(primaryKey)
                       .From("tn_Follows")
                       .Where("UserId = @0", userId);

                    if (groupId == (int)FollowSpecifyGroupIds.Quietly)
                        sql.Where("IsQuietly = 1");
                    else if (groupId == (int)FollowSpecifyGroupIds.Mutual)
                        sql.Where("IsMutual = 1");
                    else
                        sql.Where("IsQuietly = 0");
                }
                else
                {
                    sql.Where("exists(select 1 from tn_ItemsInCategories IIC where IIC.CategoryId = @0 and IIC.ItemId = tn_Follows.Id)", groupId);
                }

                sql.OrderBy("Id desc");

                userIds = CreateDAO().FetchTopPrimaryKeys(MaxTopNumber, primaryKey, sql).Select(n => (long)n);
                if (userIds != null)
                {
                    cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
                }
            }

            return userIds;
        }

        /// <summary>
        /// 获取关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">用户分组Id</param>
        /// <param name="sortBy">排序条件</param>
        /// <param name="pageIndex">页码</param>
        ///<remarks> 默认isQuietly为 false (groupId isMutual isMutual)三者参数为互斥关系，
        /// 当isQuietly为false并且groupId为null 则排序sortBy才生效</remarks>
        public PagingDataSet<long> GetFollowedUserIds(long userId, long? groupId, Follow_SortBy? sortBy, int pageIndex)
        {
            if (userId <= 0)
                return null;

            List<Sql> sqls = new List<Sql>();
            var sql = Sql.Builder;
            string primaryValue = "FollowedUserId";

            sql.Select("FollowedUserId")
               .From("tn_Follows");

            if (groupId == null)
            {
                switch (sortBy)
                {
                    case Follow_SortBy.FollowerCount_Desc:
                        sql.InnerJoin("tn_Users")
                           .On("tn_Users.UserId = tn_Follows.FollowedUserId");
                        break;
                    case Follow_SortBy.LastContent_Desc:
                        sql.LeftJoin("(select OwnerId,Max(ActivityId) ActivityId from tn_Activities group by OwnerId) A")
                           .On("A.OwnerId = tn_Follows.FollowedUserId");
                        break;
                }
            }

            sql.Where("tn_Follows.UserId = @0", userId);

            if (groupId.HasValue)
            {
                switch (groupId)
                {
                    case (int)FollowSpecifyGroupIds.UnGrouped:
                        sql.Where("not exists (select 1 from tn_ItemsInCategories IIC where exists (select 1 from tn_Categories C where C.OwnerId = @0 and C.TenantTypeId = @1 and IIC.CategoryId = C.CategoryId) and IIC.ItemId = tn_Follows.Id)", userId, TenantTypeIds.Instance().User());
                        break;
                    case (int)FollowSpecifyGroupIds.Quietly:
                        sql.Where("IsQuietly = 1");
                        break;
                    case (int)FollowSpecifyGroupIds.Mutual:
                        sql.Where("IsMutual = 1");
                        break;
                    default:
                        sql.Where("exists(select 1 from tn_ItemsInCategories IIC where IIC.CategoryId = @0 and IIC.ItemId = tn_Follows.Id)", groupId);
                        break;
                }

                sql.OrderBy("Id desc");
            }

            if (groupId == null)
            {
                switch (sortBy)
                {
                    case Follow_SortBy.DateCreated_Desc:
                        sql.OrderBy("Id DESC");
                        break;
                    case Follow_SortBy.FollowerCount_Desc:
                        sql.OrderBy("FollowerCount Desc");
                        break;
                    case Follow_SortBy.LastContent_Desc:
                        sql.OrderBy("A.ActivityId Desc");
                        break;
                }
            }

            PetaPocoDatabase dao = CreateDAO();

            PagingEntityIdCollection peic = null;
            if (pageIndex < CacheablePageCount)
            {
                string cacheKey = GetCacheKey_GetFollowedUserIds(userId, groupId, sortBy);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, primaryValue, sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, primaryValue, sql);
            }

            if (peic != null)
            {
                PagingDataSet<long> pds = new PagingDataSet<long>(peic.GetPagingEntityIds(pageSize, pageIndex).Cast<long>());
                pds.PageSize = pageSize;
                pds.PageIndex = pageIndex;
                pds.TotalRecords = peic.TotalRecords;
                return pds;
            }

            return null;
        }


        /// <summary>
        /// 批量获取关注用户列表，用于全文检索
        /// </summary>
        /// <param name="userIds">用户Id列表</param>
        public IEnumerable<FollowEntity> GetFollowedUsers(IEnumerable<long> userIds)
        {
            IEnumerable<FollowEntity> followedUsers = null;

            if (userIds == null || userIds.Count<long>() == 0)
            {
                return followedUsers;
            }

            var sql = Sql.Builder;
            sql.Select("DISTINCT *")
               .From("tn_Follows")
               .Where("UserId IN (@userIds)", new { userIds = userIds })
               .Where("IsQuietly = 0")
               .OrderBy("UserId ASC");

            followedUsers = CreateDAO().Fetch<FollowEntity>(sql);

            return followedUsers;
        }

        /// <summary>
        /// 获取前N条关注Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">需要获取的记录数</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowIds(long userId, int topNumber, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            if (userId <= 0)
                return null;

            IEnumerable<long> userIds = GetFollowIds(userId, groupId, sortBy);

            return userIds.Take(topNumber <= MaxTopNumber ? topNumber : MaxTopNumber);
        }

        /// <summary>
        /// 获取关注Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        public IEnumerable<long> GetFollowIds(long userId, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            IEnumerable<long> userIds = null;

            string cacheKey = GetCacheKey_GetTopFollowIds(userId, groupId, sortBy);
            userIds = cacheService.Get<IEnumerable<long>>(cacheKey);
            if (userIds == null)
            {
                string primaryKey = string.Empty;
                var sql = Sql.Builder;

                if (!groupId.HasValue || groupId < 0)
                {
                    sql.From("tn_Follows")
                       .Where("UserId = @0", userId);

                    if (groupId == (int)FollowSpecifyGroupIds.Quietly)
                        sql.Where("IsQuietly = 1");
                    else if (groupId == (int)FollowSpecifyGroupIds.Mutual)
                        sql.Where("IsMutual = 1");
                    else
                        sql.Where("IsQuietly = 0");
                }
                else
                {
                    sql.Select("Id")
                       .From("tn_Follows")
                       .Where("IsQuietly = 0")
                       .Where("UserId = @0", userId)
                       .Where("exists (select 1 from tn_ItemsInCategories IIC Where IIC.CategoryId = @0 and tn_Follows.Id = IIC.ItemId)", groupId);
                }

                userIds = CreateDAO().FetchTopPrimaryKeys<FollowEntity>(MaxTopNumber, sql).Select(n => (long)n);
                if (userIds != null)
                {
                    cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
                }
            }

            return userIds;
        }

        /// <summary>
        /// 获取关注Id分页列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">用户分组Id</param>
        /// <param name="sortBy">排序条件</param>
        /// <param name="pageIndex">页码</param>
        ///<remarks> 
        ///默认isQuietly为 false (groupId isMutual isMutual)三者参数为互斥关系，
        ///当isQuietly为false并且groupId为null 则排序sortBy才生效
        /// </remarks>
        public PagingDataSet<FollowEntity> GetFollows(long userId, long? groupId, Follow_SortBy? sortBy, int pageIndex)
        {
            if (userId <= 0)
                return null;

            List<Sql> sqls = new List<Sql>();
            var sql = Sql.Builder;
            string primaryValue = "Id";

            sql.Select(primaryValue)
               .From("tn_Follows");

            if (groupId == null)
            {
                switch (sortBy)
                {
                    case Follow_SortBy.FollowerCount_Desc:
                        sql.InnerJoin("tn_Users")
                           .On("tn_Users.UserId = tn_Follows.FollowedUserId");
                        break;
                    case Follow_SortBy.LastContent_Desc:
                        sql.LeftJoin("(select OwnerId,Max(ActivityId) ActivityId from tn_Activities group by OwnerId) A")
                           .On("A.OwnerId = tn_Follows.FollowedUserId");
                        break;
                }
            }

            sql.Where("tn_Follows.UserId = @0", userId);

            if (groupId.HasValue)
            {
                switch (groupId)
                {
                    case (int)FollowSpecifyGroupIds.UnGrouped:
                        sql.Where(" not exists (select 1 from tn_ItemsInCategories IIC where exists (select 1 from tn_Categories C where C.OwnerId = @0 and C.TenantTypeId = @1 and IIC.CategoryId = C.CategoryId) and IIC.ItemId = Id)", userId, TenantTypeIds.Instance().User());
                        break;
                    case (int)FollowSpecifyGroupIds.Quietly:
                        sql.Where("IsQuietly = 1");
                        break;
                    case (int)FollowSpecifyGroupIds.Mutual:
                        sql.Where("IsMutual = 1");
                        break;
                    default:
                        primaryValue = "ItemId";
                        sql = Sql.Builder;
                        sql.Select(primaryValue)
                           .From("tn_ItemsInCategories")
                           .Where("CategoryId = @0 ", groupId);
                        break;
                }
            }

            if (groupId == null)
            {
                switch (sortBy)
                {
                    case Follow_SortBy.DateCreated_Desc:
                        sql.OrderBy("DateCreated DESC");
                        break;
                    case Follow_SortBy.FollowerCount_Desc:
                        sql.OrderBy("FollowerCount Desc");
                        break;
                    case Follow_SortBy.LastContent_Desc:
                        sql.OrderBy("A.ActivityId Desc");
                        break;
                }
            }

            PetaPocoDatabase dao = CreateDAO();
            PagingEntityIdCollection peic = null;
            if (pageIndex < CacheablePageCount)
            {
                string cacheKey = GetCacheKey_GetFollowedIds(userId, groupId, sortBy);
                peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
                if (peic == null)
                {
                    peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, primaryValue, sql);
                    peic.IsContainsMultiplePages = true;
                    cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
                }
            }
            else
            {
                peic = dao.FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize, pageIndex, primaryValue, sql);
            }

            if (peic != null)
            {
                PagingDataSet<FollowEntity> pds = new PagingDataSet<FollowEntity>(PopulateEntitiesByEntityIds(peic.GetPagingEntityIds(pageSize, pageIndex).Cast<long>()));
                pds.PageSize = pageSize;
                pds.PageIndex = pageIndex;
                pds.TotalRecords = peic.TotalRecords;
                return pds;
            }

            return null;
        }

        /// <summary>
        /// 根据被浏览用户获取我的关注用户中关注他的用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被浏览用户Id</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowedUserIdsFromUser(long userId, long toUserId, int topNumber)
        {
            if (userId <= 0 || toUserId <= 0)
                return null;
            IEnumerable<long> userIds = null;
            var sql = Sql.Builder;
            sql.Append("select userId from tn_Follows where UserId in (select FollowedUserId from tn_Follows where UserId = @0) and FollowedUserId = @1 and IsQuietly = 0", userId, toUserId);

            string cacheKey = GetCacheKey_GetTopFollowedIdsFromUser(userId, toUserId);
            userIds = cacheService.Get<IEnumerable<long>>(cacheKey);
            if (userIds == null)
            {
                userIds = CreateDAO().FetchTopPrimaryKeys(MaxTopNumber, "UserId", sql).Cast<long>();
                if (userIds != null)
                {
                    cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
                }
            }

            if (userIds == null)
                return userIds;

            return userIds.Take(topNumber <= MaxTopNumber ? topNumber : MaxTopNumber);
        }

        /// <summary>
        /// 获取用户粉丝的关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="visitorId">访问者用户Id（用来排除当前访问者与访问者的关注用户）</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowedUserIdsOfFollowers(long userId, long visitorId, int topNumber)
        {
            if (userId <= 0)
                return null;

            IEnumerable<long> userIds = null;

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            string cacheKey = GetCacheKey_GetTopFollowedIdsOfFollowers(userId);
            userIds = cacheService.Get<IEnumerable<long>>(cacheKey);
            if (userIds == null)
            {
                var followedIdsSql = GenerateSql_FollowedIds(userId, visitorId);
                if (followedIdsSql != null && !string.IsNullOrEmpty(followedIdsSql.SQL))
                {
                    userIds = dao.FetchTopPrimaryKeys(MaxTopNumber, "FollowedUserId", followedIdsSql).Cast<long>();
                    cacheService.Add(cacheKey, userIds, CachingExpirationType.ObjectCollection);
                }
            }

            dao.CloseSharedConnection();

            if (userIds == null)
                return userIds;

            return userIds.Take(topNumber <= MaxTopNumber ? topNumber : MaxTopNumber);
        }

        /// <summary>
        /// 获取用户与被浏览用户的共同关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被浏览用户Id</param>
        /// <param name="topNumber">获取的数据条数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTogetherFollowedUserIds(long userId, long toUserId, int topNumber)
        {
            if (userId <= 0 || toUserId <= 0)
                return null;

            string cacheKey_Ids = GetCacheKey_GetTogetherFollowedIds(userId, toUserId);

            IEnumerable<long> followedIds = cacheService.Get<IEnumerable<long>>(cacheKey_Ids);
            if (followedIds == null)
            {
                IEnumerable<long> relevance_UserId = GetTopFollowedUserIds(userId, MaxTopNumber);
                IEnumerable<long> relevance_ToUserId = GetTopFollowedUserIds(toUserId, MaxTopNumber);

                if (relevance_ToUserId != null && relevance_UserId != null)
                {
                    followedIds = relevance_UserId.Intersect(relevance_ToUserId);
                    cacheService.Set(cacheKey_Ids, followedIds, CachingExpirationType.UsualObjectCollection);
                }
            }

            return followedIds.Take(topNumber);
        }

        /// <summary>
        /// 获取粉丝Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">查询条件</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<long> GetFollowerUserIds(long userId, Follow_SortBy sortBy, int pageIndex)
        {
            if (userId <= 0)
                return null;

            var sql = GenerateSql_FollowerIds(userId, sortBy);

            PagingEntityIdCollection peic = null;

            if (pageIndex <= CacheablePageCount)
            {
                peic = GetFollowerUserIdsFromCache(userId, sortBy, sql);
            }
            else
            {
                peic = CreateDAO().FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "tn_Follows.UserId", sql);
            }

            if (peic != null)
            {
                PagingDataSet<long> pds = new PagingDataSet<long>(peic.GetPagingEntityIds(pageSize, pageIndex).Cast<long>());
                pds.PageSize = pageSize;
                pds.PageIndex = pageIndex;
                pds.TotalRecords = peic.TotalRecords;
                return pds;
            }

            return null;
        }

        /// <summary>
        /// 获取某个时间之后新增的粉丝Id列表，用于全文检索的定时任务
        /// </summary>
        /// <param name="lastStart">上次任务的开始时间</param>
        /// <returns></returns>
        public IEnumerable<long> GetRecentFollowerUserIds(DateTime lastStart)
        {
            var sql = Sql.Builder;
            sql.Select("DISTINCT UserId")
               .From("tn_Follows")
               .Where("DateCreated >= @0", lastStart)
               .OrderBy("UserId DESC");

            IEnumerable<long> userIds = CreateDAO().FetchFirstColumn(sql).Select(n => (long)n);
            return userIds;
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">查询条件</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowerUserIds(long userId, Follow_SortBy sortBy, int topNumber)
        {
            if (userId <= 0)
                return null;
            var sql = GenerateSql_FollowerIds(userId, sortBy);

            PagingEntityIdCollection peic = GetFollowerUserIdsFromCache(userId, sortBy, sql);
            return peic.GetTopEntityIds(topNumber).Cast<long>();
        }

        /// <summary>
        ///清除用户所用的关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void CleanByUser(long userId)
        {
            if (userId <= 0)
                return;

            CreateDAO().OpenSharedConnection();


            List<Sql> sqls = new List<Sql>();

            var sql = Sql.Builder;

            sqls.Add(Sql.Builder.Append("update tn_Users")
                        .Append("set FollowerCount = FollowerCount - 1")
                        .Where("FollowerCount > 0 and exists (select 1 from tn_Follows where FollowedUserId = tn_Users.UserId and UserId = @0)", userId));

            sqls.Add(Sql.Builder.Append("update tn_Users")
                        .Append("set FollowedCount = FollowedCount - 1")
                        .Where("FollowedCount > 0 and exists (select 1 from tn_Follows where UserId = tn_Users.UserId and FollowedUserId = @0)", userId));

            sqls.Add(Sql.Builder.Append("delete tn_Follows where UserId = @0 or FollowedUserId = @0", userId));

            CreateDAO().Execute(sqls);

            CreateDAO().CloseSharedConnection();
        }

        #region Private Method

        /// <summary>
        /// 获取用户粉丝的关注的SQL
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="visitorId">访问者用户Id（用来排除当前访问者与访问者的关注用户）</param>
        private Sql GenerateSql_FollowedIds(long userId, long visitorId)
        {
            //获取粉丝的SQL
            var followersSql = Sql.Builder;
            followersSql.Append("select UserId from tn_Follows where FollowedUserId = @0 "
                                 + "and UserId <> @1"
                                 , userId, visitorId);
            IEnumerable<object> followedIds = CreateDAO().FetchTopPrimaryKeys(10, "UserId", followersSql);
            //获取关注的SQL
            var followedIdsSql = Sql.Builder;
            if (followedIds != null && followedIds.Count() > 0)
            {
                followedIdsSql.Append("select UserId from tn_Users inner join (select DISTINCT FollowedUserId from tn_Follows where UserId in (@FollowedId)"
                                       + "and FollowedUserId <> @UserId and FollowedUserId <> @VisitorId and IsQuietly = 0 and tn_Follows.UserId <> @VisitorId) F on tn_Users.UserId = F.FollowedUserId order by FollowerCount desc"
                                       , new { FollowedId = followedIds }, new { UserId = userId }, new { VisitorId = visitorId });
            }
            return followedIdsSql;
        }

        /// <summary>
        ///从缓存中获取粉丝列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">排序类型</param>
        /// <param name="sql">sql语句</param>
        private PagingEntityIdCollection GetFollowerUserIdsFromCache(long userId, Follow_SortBy sortBy, Sql sql)
        {
            string cacheKey = GetCacheKey_GetFollowerIds(userId, sortBy);
            PagingEntityIdCollection peic = cacheService.Get<PagingEntityIdCollection>(cacheKey);
            if (peic == null)
            {
                peic = CreateDAO().FetchPagingPrimaryKeys(PrimaryMaxRecords, pageSize * CacheablePageCount, 1, "tn_Follows.UserId", sql);

                peic.IsContainsMultiplePages = true;

                cacheService.Add(cacheKey, peic, CachingExpirationType.ObjectCollection);
            }

            return peic;
        }

        /// <summary>
        ///    获取粉丝列表的SQL
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">排序类型</param>
        private Sql GenerateSql_FollowerIds(long userId, Follow_SortBy sortBy)
        {
            var sql = Sql.Builder;
            sql.Select("tn_Follows.UserId")
               .From("tn_Follows");

            StringBuilder sb = new StringBuilder();

            switch (sortBy)
            {  //根据关注时间排序
                case Follow_SortBy.DateCreated_Desc:
                    sb.Append("tn_Follows.Id DESC,");
                    break;
                //根据粉丝数来排序
                case Follow_SortBy.FollowerCount_Desc:
                    sql.InnerJoin("tn_Users")
                       .On("tn_Users.UserId = tn_Follows.UserId");
                    sb.Append("tn_Users.FollowerCount Desc,");
                    break;
                default:
                    
                    sb.Append("tn_Follows.Id DESC,");
                    break;
                
            }

            sql.Where("tn_Follows.IsQuietly = 0 AND tn_Follows.FollowedUserId = @0", userId);

            sql.OrderBy(sb.ToString().TrimEnd(','));

            return sql;
        }

        #region GetCacheKey

        /// <summary>
        /// 获取关注用户Id的列表缓存CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">用户分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        private string GetCacheKey_GetTopFollowedUserIds(long userId, long? groupId, Follow_SortBy? sortBy)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("TopFollowedUserIds{0}::uid:{1}-gid:{2}-sort-{3}", areaVersion, userId, groupId, ((int)(sortBy ?? Follow_SortBy.DateCreated_Desc)).ToString());
        }

        /// <summary>
        /// 获取关注用Id的列表缓存CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">用户分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        private string GetCacheKey_GetTopFollowIds(long userId, long? groupId, Follow_SortBy? sortBy)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            RealTimeCacheHelper categoryRealTimeCacheHelper = EntityData.ForType(typeof(ItemInCategory)).RealTimeCacheHelper;
            int categoryAreaVersion = categoryRealTimeCacheHelper.GetAreaVersion("OwnerId", userId);
            return string.Format("TopFollowIds{0}-{4}::uid:{1}-gid:{2}-sort-{3}", areaVersion, userId, groupId, ((int)(sortBy ?? Follow_SortBy.DateCreated_Desc)).ToString(), categoryAreaVersion);
        }

        /// <summary>
        /// 获取关注的CacheKey
        /// </summary>
        private string GetCacheKey_FollowUserId_NoteName(long userId)
        {
            return string.Format("FollowedUserId_NoteName::UserId-{0}", userId);
        }

        /// <summary>
        /// 获取最新粉丝数的CacheKey
        /// <param name="userId">用户Id</param>
        /// </summary>
        private string GetCacheKey_FollowerCount(long userId)
        {
            return string.Format("FollowerCount::UserId:{0}", userId);
        }

        /// <summary>
        /// 获取关注列表的我关注的也关注缓存
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被浏览用户Id</param>
        private string GetCacheKey_GetTopFollowedIdsFromUser(long userId, long toUserId)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("TopFollowedIdsFromUser{0}::UserId:{1}-ToUserId:{2}", areaVersion, userId, toUserId);
        }

        /// <summary>
        /// 获取关注实体的CacheKey
        /// </summary>
        /// <param name="userId">s\用户Id</param>
        /// <param name="toUserId">被关注用户Id</param>
        private string GetCacheKey_Follow(long userId, long toUserId)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("FollowEntity{0}::UserId:{1}-ToUserId:{2}", areaVersion, userId, toUserId);
        }

        /// <summary>
        /// 获取共同关注列表缓存
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被浏览用户Id</param>
        private string GetCacheKey_GetTogetherFollowedIds(long userId, long toUserId)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("TogetherFollowedIds{0}::UserId:{1}-ToUserId:{2}", areaVersion, userId, toUserId);
        }

        /// <summary>
        /// 获取粉丝的关注的列表缓存CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        private string GetCacheKey_GetTopFollowedIdsOfFollowers(long userId)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("TopFollowedIdsOfFollowers{0}::UserId:{1}:", areaVersion, userId);
        }

        /// <summary>
        /// 获取是否双向关注的缓存CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        private string GetCacheKey_IsMutualFollowed(long userId)
        {
            return string.Format("IsMutualFollowed::UserId:{0}", userId);
        }

        /// <summary>
        /// 获取关注的列表缓存
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序类型</param>
        private string GetCacheKey_GetFollowedIds(long userId, long? groupId, Follow_SortBy? sortBy)
        {
            RealTimeCacheHelper categoryRealTimeCacheHelper = EntityData.ForType(typeof(ItemInCategory)).RealTimeCacheHelper;
            int categoryAreaVersion = categoryRealTimeCacheHelper.GetAreaVersion("OwnerId", userId);
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("FollowedIds{0}-{4}::UserId:{1}-GourpId:{2}-SortBy:{3}", areaVersion, userId, groupId.HasValue ? groupId.ToString() : string.Empty, sortBy.HasValue ? ((int)sortBy).ToString() : string.Empty, categoryAreaVersion);
        }

        /// <summary>
        /// 获取关注用户的列表缓存
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序类型</param>
        private string GetCacheKey_GetFollowedUserIds(long userId, long? groupId, Follow_SortBy? sortBy)
        {
            RealTimeCacheHelper categoryRealTimeCacheHelper = EntityData.ForType(typeof(ItemInCategory)).RealTimeCacheHelper;
            int categoryAreaVersion = categoryRealTimeCacheHelper.GetAreaVersion("OwnerId", userId);
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("FollowedUserIds{0}-{4}::UserId:{1}-GourpId:{2}-SortBy:{3}", areaVersion, userId, groupId.HasValue ? groupId.ToString() : string.Empty, sortBy.HasValue ? ((int)sortBy).ToString() : string.Empty, categoryAreaVersion);
        }

        /// <summary>
        /// 获取粉丝Id列表缓存
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">排序类型</param>
        private string GetCacheKey_GetFollowerIds(long userId, Follow_SortBy sortBy)
        {
            int areaVersion = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            return string.Format("FollowerIds{0}::UserId:{1}-SortBy:{2}", areaVersion, userId, sortBy);
        }

        #endregion GetCacheKey

        #endregion Private Method

        #region 配置属性

        
        private int cacheablePageCount = 30;

        /// <summary>
        /// 可缓存的列表缓存页数
        /// </summary>
        protected override int CacheablePageCount
        {
            get { return cacheablePageCount; }
        }

        private int pageSize = 20;
        private int MaxTopNumber = 1000;
        private int InitNumber = 10;

        #endregion 配置属性
    }
}
