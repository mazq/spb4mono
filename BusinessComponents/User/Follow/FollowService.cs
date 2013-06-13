//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using System;

namespace Tunynet.Common
{
    /// <summary>
    /// 关注用户业务逻辑类
    /// </summary>
    public class FollowService
    {
        private IFollowRepository followRepository;
        private CategoryService categoryService;

        /// <summary>
        /// 构造器
        /// </summary>
        public FollowService()
            : this(new FollowRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="followRepository">follow仓储</param>
        public FollowService(IFollowRepository followRepository)
        {
            this.followRepository = followRepository;
            categoryService = new CategoryService();
        }

        #region Follow

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followUserId">被关注用户Id</param>
        /// <param name="isQuietly">是否悄悄关注</param>
        public bool Follow(long userId, long followUserId, bool isQuietly = false)
        {
            FollowEntity follow = FollowEntity.New();
            follow.UserId = userId;
            follow.FollowedUserId = followUserId;
            follow.IsQuietly = isQuietly;

            EventBus<FollowEntity>.Instance().OnBefore(follow, new CommonEventArgs(EventOperationType.Instance().Create()));
            bool isSuccess = followRepository.Follow(userId, followUserId, isQuietly);
            EventBus<FollowEntity>.Instance().OnAfter(follow, new CommonEventArgs(EventOperationType.Instance().Create()));

            return isSuccess;
        }

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserIds">被关注用户Id集合</param>
        /// <param name="isQuietly">是否悄悄关注</param>
        public void BatchFollow(long userId, IList<long> followedUserIds, bool isQuietly = false)
        {
            foreach (var followedUserId in followedUserIds)
            {
                followRepository.Follow(userId, followedUserId, isQuietly);
            }
            EventBus<int, BatchFollowEventArgs>.Instance().OnAfter(followedUserIds.Count, new BatchFollowEventArgs(EventOperationType.Instance().Create(), userId));
        }

        /// <summary>
        /// 更新关注实体
        /// </summary>
        /// <param name="follow">关注用户实体</param>
        public void Update(FollowEntity follow)
        {
            EventBus<FollowEntity>.Instance().OnBefore(follow, new CommonEventArgs(EventOperationType.Instance().Update()));
            followRepository.Update(follow);
            EventBus<FollowEntity>.Instance().OnAfter(follow, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 判断是否关注了被判定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        /// <returns>true-关注,false-没关注</returns>
        public bool IsFollowed(long userId, long toUserId)
        {
            IEnumerable<string> groupNames;
            return IsFollowed(userId, toUserId, out groupNames);
        }

        /// <summary>
        /// 判断是否关注了被判定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        /// <param name="groupNames">被关注用户所属分组</param>
        /// <returns>true-关注,false-没关注</returns>
        public bool IsFollowed(long userId, long toUserId, out IEnumerable<string> groupNames)
        {
            bool isFollow = followRepository.IsFollowed(userId, toUserId);

            groupNames = null;
            if (isFollow)
            {
                FollowEntity followEntity = followRepository.Get(userId, toUserId);
                groupNames = categoryService.GetCategoriesOfItem(followEntity.Id, userId, TenantTypeIds.Instance().User()).Select(n => n.CategoryName);
            }

            return isFollow;
        }

        /// <summary>
        /// 判断是否关注了被判定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        /// <param name="isQuietly">是否去为悄悄关注</param>
        /// <returns>true-关注,false-没关注</returns>
        public bool IsFollowed(long userId, long toUserId, out bool isQuietly)
        {
            bool isFollow = followRepository.IsFollowed(userId, toUserId);
            isQuietly = false;

            if (isFollow)
            {
                FollowEntity follow = Get(userId, toUserId);
                isQuietly = follow != null ? follow.IsQuietly : isQuietly;
            }

            return isFollow;
        }

        /// <summary>
        /// 是否为双向关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        /// <returns>true-关注,false-没关注</returns>
        public bool IsMutualFollowed(long userId, long toUserId)
        {
            return followRepository.IsMutualFollowed(userId, toUserId);
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被关注用户Id</param>
        public void CancelFollow(long userId, long followedUserId)
        {
            FollowEntity follow = followRepository.Get(userId, followedUserId);
            EventBus<FollowEntity>.Instance().OnBefore(follow, new CommonEventArgs(EventOperationType.Instance().Delete()));
            followRepository.CancelFollow(userId, followedUserId);
            EventBus<FollowEntity>.Instance().OnAfter(follow, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 获取用户的最新粉丝数
        /// </summary>
        /// <remarks>用于新增粉丝提醒</remarks>
        /// <param name="userId">用户Id</param>
        public int GetNewFollowerCount(long userId)
        {
            return followRepository.GetNewFollowerCount(userId);
        }

        /// <summary>
        /// 清除最新用户统计
        /// </summary>
        /// <remarks>用于清空提醒信息</remarks>
        ///<param name="userId">用户Id</param>
        /// <returns></returns>
        public void ClearNewFollowerCount(long userId)
        {
            followRepository.ClearNewFollowerCount(userId);
        }

        /// <summary>
        /// 移除用户的粉丝
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followerUserId">粉丝的用户Id</param>
        public void RemoveFollower(long userId, long followerUserId)
        {
            followRepository.RemoveFollower(userId, followerUserId);

            FollowEntity follow = followRepository.Get(followerUserId, userId);
            EventBus<FollowEntity>.Instance().OnAfter(follow, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 获取关注信息实体
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被关注用户Id</param>
        /// <returns></returns>
        public FollowEntity Get(long userId, long followedUserId)
        {
            return followRepository.Get(userId, followedUserId);
        }

        /// <summary>
        /// 获取用户的备注名称
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被关注用户Id</param>
        /// <returns></returns>
        public string GetNoteName(long userId, long followedUserId)
        {
            return followRepository.GetNoteName(userId, followedUserId);
        }

        /// <summary>
        /// 获取关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">需要获取的记录数</param>
        /// <param name="groupId">群组Id</param>
        /// <param name="sortBy">排序规则</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowedUserIds(long userId, int topNumber, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            return followRepository.GetTopFollowedUserIds(userId, topNumber, groupId, sortBy);
        }

        /// <summary>
        /// 获取部分关注用户Id列表（前1000条）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">群组Id</param>
        /// <param name="sortBy">排序规则</param>
        /// <returns></returns>
        public IEnumerable<long> GetPortionFollowedUserIds(long userId, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            return followRepository.GetPortionFollowedUserIds(userId, groupId, sortBy);
        }

        /// <summary>
        /// 获取关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId"><para>用户分组Id</para><remarks>groupId为0时获取未分组的用户，为null时获取所有用户</remarks></param>
        /// <param name="sortBy">排序条件</param>
        /// <param name="pageIndex">页码</param>
        ///<remarks>
        ///groupId为null 则排序sortBy才生效
        /// </remarks>
        public PagingDataSet<long> GetFollowedUserIds(long userId, long? groupId, Follow_SortBy? sortBy, int pageIndex)
        {
            return followRepository.GetFollowedUserIds(userId, groupId, sortBy, pageIndex);
        }

        /// <summary>
        /// 获取前N个关注实体集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">需要获取的记录数</param>
        /// <param name="groupId">群组Id</param>
        /// <param name="sortBy">排序规则</param>
        /// <returns></returns>
        public IEnumerable<FollowEntity> GetTopFollows(long userId, int topNumber, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            IEnumerable<long> followIds = followRepository.GetTopFollowIds(userId, topNumber, groupId, sortBy);
            if (followIds != null)
            {
                return followRepository.PopulateEntitiesByEntityIds(followIds);
            }

            return null;
        }

        /// <summary>
        /// 获取关注实体集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">群组Id</param>
        /// <param name="sortBy">排序规则</param>
        /// <returns></returns>
        public IEnumerable<FollowEntity> GetFollows(long userId, long? groupId = null, Follow_SortBy? sortBy = null)
        {
            IEnumerable<long> followIds = followRepository.GetFollowIds(userId, groupId, sortBy);

            if (followIds != null)
            {
                return followRepository.PopulateEntitiesByEntityIds(followIds);
            }

            return null;
        }

        /// <summary>
        /// 获取关注实体分页集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">群组Id</param>
        /// <param name="sortBy">排序规则</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public PagingDataSet<FollowEntity> GetFollows(long userId, long? groupId, Follow_SortBy? sortBy, int pageIndex)
        {
            return followRepository.GetFollows(userId, groupId, sortBy, pageIndex);
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
            return followRepository.GetFollowerUserIds(userId, sortBy, pageIndex);
        }

        /// <summary>
        /// 获取某个时间之后新增的粉丝Id列表，用于全文检索的定时任务
        /// </summary>
        /// <param name="lastStart">上次任务的开始时间</param>
        /// <returns></returns>
        public IEnumerable<long> GetRecentFollowerUserIds(DateTime lastStart)
        {
            return followRepository.GetRecentFollowerUserIds(lastStart);
        }

        /// <summary>
        /// 批量获取关注用户Id列表，用于全文检索
        /// </summary>
        /// <param name="userIds">用户Id列表</param>
        ///<remarks>默认isQuietly为0，即不查询悄悄关注的</remarks>
        public IEnumerable<FollowEntity> GetFollowedUsers(IEnumerable<long> userIds)
        {
            return followRepository.GetFollowedUsers(userIds);
        }

        /// <summary>
        /// 获取粉丝Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">查询条件</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowerUserIds(long userId, Follow_SortBy sortBy, int topNumber)
        {
            return followRepository.GetTopFollowerUserIds(userId, sortBy, topNumber);
        }

        /// <summary>
        /// 根据被浏览用户获取我的关注用户中关注他的用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被浏览用户Id</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowedUserIdsFromUser(long userId, long toUserId, int topNumber)
        {
            return followRepository.GetTopFollowedUserIdsFromUser(userId, toUserId, topNumber);
        }

        /// <summary>
        /// 获取用户粉丝的关注
        /// </summary>
        /// <param name="userId">空间主人用户Id</param>
        /// <param name="visitorId">访问者用户Id（用来排除当前访问者与访问者的关注用户）</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTopFollowedUserIdsOfFollowers(long userId, long visitorId, int topNumber)
        {
            return followRepository.GetTopFollowedUserIdsOfFollowers(userId, visitorId, topNumber);
        }

        /// <summary>
        /// 获取用户与被浏览用户的共同关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        /// <param name="topNumber">获取数据的条数</param>
        /// <returns></returns>
        public IEnumerable<long> GetTogetherFollowedUserIds(long userId, long toUserId, int topNumber)
        {
            return followRepository.GetTogetherFollowedUserIds(userId, toUserId, topNumber);
        }

        /// <summary>
        /// 清除用户关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void CleanByUser(long userId)
        {
            followRepository.CleanByUser(userId);

            FollowEntity follow = FollowEntity.New();
            follow.UserId = userId;
            EventBus<FollowEntity>.Instance().OnAfter(follow, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        #endregion Follow
    }
}
