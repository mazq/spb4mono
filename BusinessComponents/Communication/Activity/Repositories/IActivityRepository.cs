//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{

    /// <summary>
    /// 动态仓储接口
    /// </summary>
    public interface IActivityRepository : IRepository<Activity>
    {

        #region 动态维护
        /// <summary>
        /// 把动态最后更新时间设置为当前时间
        /// </summary>
        /// <param name="activityId"></param>
        bool UpdateLastModified(long activityId);

        /// <summary>
        /// 检测Owner是否已经存在某类（activityItemKey）动态，如果存在并更新最后更新时间
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="ownerType"></param>
        /// <param name="activityItemKey"></param>
        /// <returns>如果存在返回true，否则返回false</returns>
        bool CheckExistAndUpdateLastModified(long ownerId, int ownerType, string activityItemKey);

        /// <summary>
        /// 主体内容动态的最后更新时间设置为当前时间
        /// </summary>        
        /// <param name="tenantTypeId"></param>
        /// <param name="sourceId"></param>
        void UpdateLastModified(string tenantTypeId, long sourceId);

        /// <summary>
        /// 判断用户是否对同一主体内容产生过从属内容动态，如果产生过则替换成本次操作
        /// </summary>
        /// <param name="activity"></param>
        /// <returns>存在返回true,否则返回false</returns>
        bool CheckExistAndUpdateSource(Activity activity);

        /// <summary>
        /// 将动态加入到用户动态收件箱
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userIds"></param>
        void InsertUserInboxs(long activityId, IEnumerable<long> userIds);

        /// <summary>
        /// 将动态加入到站点动态收件箱
        /// </summary>
        /// <param name="activityId"></param>
        void InsertSiteInbox(long activityId);


        /// <summary>
        /// 更新动态的私有状态
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="isPrivate"></param>
        void UpdatePrivateStatus(long activityId, bool isPrivate);

        /// <summary>
        /// 删除动态
        /// </summary>
        /// <param name="activityId"></param>
        void DeleteActivity(long activityId);

        /// <summary>
        /// 根据userid删除用户动态
        /// </summary>
        /// <param name="userId">用户的id</param>
        void CleanByUser(long userId);

        /// <summary>
        /// 删除动态源时删除动态
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="sourceId">动态源内容id</param>
        void DeleteSource(string tenantTypeId, long sourceId);

        /// <summary>
        /// 从用户收件箱和站点收件箱移除动态
        /// </summary>
        /// <remarks>
        /// 保留动态操作者的收件箱
        /// </remarks>
        /// <param name="activityId"></param>
        void DeleteActivityFromUserInboxAndSiteInbox(long activityId);

        /// <summary>
        /// 从用户收件箱移除动态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="activityId">动态Id</param>
        void DeleteFromUserInbox(long userId, long activityId);

        /// <summary>
        /// 从站点收件箱移除动态
        /// </summary>
        /// <param name="activityId">动态Id</param>
        void DeleteFromSiteInbox(long activityId);

        /// <summary>
        /// 从用户动态收件箱移除OwnerId的所有动态
        /// </summary>
        /// <remarks>
        /// 取消关注/退出群组、屏蔽用户/屏蔽群组时使用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="ownerId">动态拥有者Id</param>
        void RemoveInboxAboutOwner(long userId, long ownerId, int ownerType);

        /// <summary>
        /// 在用户动态收件箱追溯OwnerId的动态
        /// </summary>
        /// <remarks>
        /// 关注用户/加入群组、取消屏蔽用户/取消屏蔽群组时使用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="ownerId">动态拥有者Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="traceBackNumber">追溯OwnerId的动态数</param>
        void TraceBackInboxAboutOwner(long userId, long ownerId, int ownerType, int traceBackNumber);

        #endregion

        #region 动态获取

        /// <summary>
        /// 获取用户的时间线
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="followGroupId"><para>关注用户分组Id</para><remarks>groupId为-1时获取相互关注的用户，为null时获取所有用户</remarks></param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="mediaType"><see cref="MediaType"/></param>
        /// <param name="isOriginalThread">是不是原创主题</param>
        /// <param name="pageIndex">页码</param>
        ///<returns></returns>
        PagingDataSet<Activity> GetMyTimeline(long userId, long? followGroupId, int? applicationId, MediaType? mediaType, bool? isOriginalThread, int pageIndex);

        /// <summary>
        /// 查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="lastActivityId">上次最后呈现的ActivityId</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="userId">返回首个动态操作者Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <returns>自lastActivityId以后进入用户时间线的动态个数</returns>
        int GetNewerCount(long ownerId, long lastActivityId, int? applicationId, out long userId, int? ownerType);

        /// <summary>
        /// 查询自lastActivityId以后进入用户时间线的动态
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="lastActivityId">上次最后呈现的ActivityId</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <returns>lastActivityId</returns>
        IEnumerable<Activity> GetNewerActivities(long ownerId, long lastActivityId, int? applicationId, int? ownerType);

        /// <summary>
        /// 获取拥有者的动态
        /// </summary>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="ownerId">动态拥有者Id</param>        
        /// <param name="applicationId">应用Id</param>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="isOriginalThread">是否原创</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="userId">用户Id</param>
        ///<returns></returns>
        PagingDataSet<Activity> GetOwnerActivities(int ownerType, long ownerId, int? applicationId, MediaType? mediaType, bool? isOriginalThread, bool? isPrivate, int pageIndex, long? userId);

        /// <summary>
        /// 获取站点动态
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="pageIndex">页码</param>
        ///<returns></returns>
        IEnumerable<Activity> GetSiteActivities(int? applicationId, int pageSize, int pageIndex);

        #endregion

        /// <summary>
        /// 获取某条动态
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        Activity Get(string tenantTypeId, long sourceId);
    }
}