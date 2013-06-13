//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 关注用户数据访问接口
    /// </summary>
    public interface IFollowRepository : IRepository<FollowEntity>
    {
        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被操作用户Id</param>
        /// <param name="isQuietly">是否悄悄关注</param>
        bool Follow(long userId, long followedUserId, bool isQuietly = false);

        /// <summary>
        /// 判断是否关注了被判定用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        bool IsFollowed(long userId, long toUserId);

        /// <summary>
        /// 是否为双向关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        bool IsMutualFollowed(long userId, long toUserId);

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被操作用户Id</param>
        void CancelFollow(long userId, long followedUserId);

        /// <summary>
        /// 获取用户的最新粉丝数
        /// </summary>
        /// <param name="userId">用户Id</param>
        int GetNewFollowerCount(long userId);

        /// <summary>
        /// 清除最新用户统计
        /// </summary>
        ///<param name="userId">用户Id</param>
        /// <returns></returns>
        void ClearNewFollowerCount(long userId);

        /// <summary>
        /// 移除用户的粉丝
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followerUserId">粉丝的用户Id</param>
        void RemoveFollower(long userId, long followerUserId);

        /// <summary>
        /// 获取关注用户实体
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被操作用户Id</param>
        FollowEntity Get(long userId, long followedUserId);

        /// <summary>
        ///清除用户所用的关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        void CleanByUser(long userId);

        /// <summary>
        /// 获取用户的备注名称
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followedUserId">被关注用户Id</param>
        /// <returns></returns>
        string GetNoteName(long userId, long followedUserId);

        /// <summary>
        /// 获取关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId"><para>用户分组Id</para><remarks>groupId为0时获取未分组的用户，为null时获取所有用户</remarks></param>
        /// <param name="sortBy">排序条件</param>
        /// <param name="pageIndex">页码</param>
        /// <remarks>
        /// groupId为null 则排序sortBy才生效
        /// </remarks>
        PagingDataSet<long> GetFollowedUserIds(long userId, long? groupId, Follow_SortBy? sortBy, int pageIndex);

        /// <summary>
        /// 获取前N条关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">需要获取的记录数</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        IEnumerable<long> GetTopFollowedUserIds(long userId, int topNumber, long? groupId = null, Follow_SortBy? sortBy = null);

        /// <summary>
        /// 获取部分关注用户Id列表
        /// </summary>
        /// <remarks>获取全部关注用户的UserId</remarks>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        IEnumerable<long> GetPortionFollowedUserIds(long userId, long? groupId = null, Follow_SortBy? sortBy = null);

        /// <summary>
        /// 获取前N条关注Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">需要获取的记录数</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        IEnumerable<long> GetTopFollowIds(long userId, int topNumber, long? groupId = null, Follow_SortBy? sortBy = null);

        /// <summary>
        /// 获取关注Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns></returns>
        IEnumerable<long> GetFollowIds(long userId, long? groupId = null, Follow_SortBy? sortBy = null);

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
        PagingDataSet<FollowEntity> GetFollows(long userId, long? groupId, Follow_SortBy? sortBy, int pageIndex);

        /// <summary>
        /// 根据被浏览用户获取我的关注用户中关注他的用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被浏览用户Id</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        IEnumerable<long> GetTopFollowedUserIdsFromUser(long userId, long toUserId, int topNumber);

        /// <summary>
        /// 获取用户粉丝的关注
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="visitorId">访问者用户Id（用来排除当前访问者与访问者的关注用户）</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        IEnumerable<long> GetTopFollowedUserIdsOfFollowers(long userId, long visitorId, int topNumber);

        /// <summary>
        /// 获取用户与被浏览用户的共同关注用户Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        /// <param name="topNumber">获取的数据条数</param>
        /// <returns></returns>
        IEnumerable<long> GetTogetherFollowedUserIds(long userId, long toUserId, int topNumber);

        /// <summary>
        /// 获取粉丝Id列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">查询条件</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PagingDataSet<long> GetFollowerUserIds(long userId, Follow_SortBy sortBy, int pageIndex);

        /// <summary>
        /// 获取某个时间之后新增的粉丝Id列表，用于全文检索的定时任务
        /// </summary>
        /// <param name="lastStart">上次任务的开始时间</param>
        /// <returns></returns>
        IEnumerable<long> GetRecentFollowerUserIds(DateTime lastStart);

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="sortBy">查询条件</param>
        /// <param name="topNumber">要获取的数据记录数</param>
        /// <returns></returns>
        IEnumerable<long> GetTopFollowerUserIds(long userId, Follow_SortBy sortBy, int topNumber);

        /// <summary>
        /// 批量获取关注用户列表，用于全文检索
        /// </summary>
        /// <param name="userIds">用户Id列表</param>
        ///<remarks>默认isQuietly为0，即不查询悄悄关注的</remarks>
        IEnumerable<FollowEntity> GetFollowedUsers(IEnumerable<long> userIds);
    }
}