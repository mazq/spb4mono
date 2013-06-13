//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common.Repositories;
using Tunynet.Common.Configuration;

namespace Tunynet.Common
{
    /// <summary>
    /// 动态业务逻辑类
    /// </summary>
    public class ActivityService
    {
        private IActivityRepository activityRepository;

        private IActivityItemRepository activityItemRepository;

        private IActivityItemUserSettingRepository activityItemUserSettingRepository;


        #region 构造器

        /// <summary>
        /// 构造器
        /// </summary>
        public ActivityService()
            : this(new ActivityRepository(), new ActivityItemRepository(), new ActivityItemUserSettingRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="activityRepository">ActivityRepository仓储</param>
        /// <param name="activityItemRepository">activityItemRepository仓储</param>
        /// <param name="activityItemUserSettingRepository">activityItemUserSettingRepository仓储</param>
        public ActivityService(IActivityRepository activityRepository, IActivityItemRepository activityItemRepository, IActivityItemUserSettingRepository activityItemUserSettingRepository)
        {
            this.activityRepository = activityRepository;
            this.activityItemRepository = activityItemRepository;
            this.activityItemUserSettingRepository = activityItemUserSettingRepository;
        }
        #endregion


        #region 动态维护

        /// <summary>
        /// 生成动态
        /// </summary>
        /// <param name="activity">待生成的动态</param>
        /// <param name="toMyInbox">是否加入我的动态收件箱</param>
        public void Generate(Activity activity, bool toMyInbox)
        {
            ActivityItem activityItem = GetActivityItem(activity.ActivityItemKey);
            if (activityItem == null)
                return;
            //1、首先检查IsOnlyOnce是否为true，如果为true并且已经生成过则仅更新动态的最后更新时间
            if (activityItem.IsOnlyOnce)
            {
                //检测是否已经存在，并更新动态的最后更新时间
                if (activityRepository.CheckExistAndUpdateLastModified(activity.OwnerId, activity.OwnerType, activity.ActivityItemKey))
                    return;
            }

            //2、如果存在ReferenceId，更新OwnerType+TenantTypeId+ReferenceId对应动态的LastModified；
            if (activity.ReferenceId > 0)
            {
                //对应主体内容动态的最后更新时间设置为当前时间（可能有多个Owner）
                activityRepository.UpdateLastModified(activity.ReferenceTenantTypeId, activity.ReferenceId);
                //3、判断用户是否对同一主体内容产生过从属内容动态，如果产生过则替换成本次操作；  处理类似连续回复的情况
                if (activityRepository.CheckExistAndUpdateSource(activity))
                    return;
            }

            //生成拥有者动态
            activityRepository.Insert(activity);

            //该类操作是否需要给用户推送动态
            if (!activityItem.IsUserReceived)
                return;

            //4、toMyInbox = true，也会加入自己的时间线（动态收件箱）
            if (toMyInbox)
            {
                //加入自己的时间线（动态收件箱）
                activityRepository.InsertUserInboxs(activity.ActivityId, new List<long> { activity.UserId });
            }

            //私有内容不推送给其他用户
            if (activity.IsPrivate)
                return;

            //5、通过IsReceiveActivity()检查，是否给该粉丝推送动态，推送时确保 ActivityId+UserId 已存在不再添加；
            IActivityReceiverGetter receiverGetter = DIContainer.ResolveNamed<IActivityReceiverGetter>(activity.OwnerType.ToString());
            IEnumerable<long> receiverUserIds = receiverGetter.GetReceiverUserIds(this, activity);
            activityRepository.InsertUserInboxs(activity.ActivityId, receiverUserIds);

            //推送给站点动态
            if (activityItem.IsSiteReceived && activity.OwnerType == ActivityOwnerTypes.Instance().User())
                activityRepository.InsertSiteInbox(activity.ActivityId);
        }

        /// <summary>
        /// 删除拥有者动态
        /// </summary>
        /// <param name="activityId">动态Id</param>
        public void DeleteActivity(long activityId)
        {
            activityRepository.DeleteActivity(activityId);
        }

        /// <summary>
        /// 根据用户删除用户动态（删除用户时使用）
        /// </summary>
        public void CleanByUser(long userId)
        {
            activityRepository.CleanByUser(userId);
        }


        /// <summary>
        /// 删除动态源时删除动态
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="sourceId">动态源内容id</param>
        public void DeleteSource(string tenantTypeId, long sourceId)
        {
            activityRepository.DeleteSource(tenantTypeId, sourceId);
        }

        /// <summary>
        /// 更新动态的私有状态
        /// </summary>
        public void UpdatePrivateStatus(long activityId, bool isPrivate)
        {
            //仅isPrivate发生变化才执行以下操作：
            //1、从false->true：参考Generate()中创建用户收件箱、站点收件箱 （注意不要让同一用户及站点收件箱的ActivityId重复）
            //2、从true->false：移除该ActivityId对应的用户收件箱（动态拥有者的收件箱除外）、站点收件箱；

            Activity activity = activityRepository.Get(activityId);
            if (activity == null)
                return;

            //若没有发生变化，则返回
            if (isPrivate == activity.IsPrivate)
                return;

            activityRepository.UpdatePrivateStatus(activityId, isPrivate);

            if (isPrivate)
            {
                //保留动态操作者的收件箱
                activityRepository.DeleteActivityFromUserInboxAndSiteInbox(activityId);
            }
            else
            {
                //创建用户收件箱、站点收件箱
                ActivityItem activityItem = GetActivityItem(activity.ActivityItemKey);
                if (activityItem == null)
                    return;

                if (!activityItem.IsUserReceived)
                    return;

                IActivityReceiverGetter receiverGetter = DIContainer.ResolveNamed<IActivityReceiverGetter>(activity.OwnerType.ToString());
                IEnumerable<long> receiverUserIds = receiverGetter.GetReceiverUserIds(this, activity);
                activityRepository.InsertUserInboxs(activity.ActivityId, receiverUserIds);

                //推送给站点动态
                if (activityItem.IsSiteReceived && activity.OwnerType == ActivityOwnerTypes.Instance().User())
                    activityRepository.InsertSiteInbox(activity.ActivityId);
            }
        }

        /// <summary>
        /// 把动态最后更新时间设置为当前时间
        /// </summary>
        /// <param name="activityId"></param>
        public void UpdateLastModified(long activityId)
        {
            activityRepository.UpdateLastModified(activityId);
        }

        /// <summary>
        /// 从用户收件箱移除动态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="activityId">动态Id</param>
        public void DeleteFromUserInbox(long userId, long activityId)
        {
            activityRepository.DeleteFromUserInbox(userId, activityId);
        }

        /// <summary>
        /// 从站点收件箱移除动态
        /// </summary>
        /// <param name="activityId">动态Id</param>
        public void DeleteFromSiteInbox(long activityId)
        {
            activityRepository.DeleteFromSiteInbox(activityId);
        }

        /// <summary>
        /// 从用户动态收件箱移除OwnerId的所有动态
        /// </summary>
        /// <remarks>
        /// 取消关注/退出群组、屏蔽用户/屏蔽群组时使用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <param name="ownerId">动态拥有者Id</param>
        public void RemoveInboxAboutOwner(long userId, long ownerId, int ownerType)
        {
            //缓存需要即时
            activityRepository.RemoveInboxAboutOwner(userId, ownerId, ownerType);
        }

        /// <summary>
        /// 在用户动态收件箱追溯OwnerId的动态
        /// </summary>
        /// <remarks>
        /// 关注用户/加入群组、取消屏蔽用户/取消屏蔽群组时使用
        /// </remarks>
        /// <param name="userId">UserId</param>
        /// <param name="ownerId">动态拥有者Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        public void TraceBackInboxAboutOwner(long userId, long ownerId, int ownerType)
        {
            //缓存需要即时
            //1、仅追溯ActivitySettings.TraceBackNumber条非私有的动态；
            IActivitySettingsManager activitySettingsManager = DIContainer.Resolve<IActivitySettingsManager>();
            ActivitySettings activitySettings = activitySettingsManager.Get();

            activityRepository.TraceBackInboxAboutOwner(userId, ownerId, ownerType, activitySettings.TraceBackNumber);
        }

        #endregion


        #region 动态获取

        /// <summary>
        /// 获取某条动态
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public Activity Get(long activityId)
        {
            return activityRepository.Get(activityId);
        }

        /// <summary>
        /// 获取某条动态
        /// </summary>
        /// <returns></returns>
        public Activity Get(string tenantTypeId, long sourceId)
        {
            return activityRepository.Get(tenantTypeId, sourceId);
        }

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
        public PagingDataSet<Activity> GetMyTimeline(long userId, long? followGroupId, int? applicationId, MediaType? mediaType, bool? isOriginalThread, int pageIndex)
        {
            return activityRepository.GetMyTimeline(userId, followGroupId, applicationId, mediaType, isOriginalThread, pageIndex);
        }

        /// <summary>
        /// 查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="lastActivityId">上次最后呈现的ActivityId</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="operatorName">返回首个动态操作者名称</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <returns>自lastActivityId以后进入用户时间线的动态个数</returns>
        public int GetNewerCount(long ownerId, long lastActivityId, int? applicationId, out string operatorName, int? ownerType = null)
        {
            //done:mazq,by zhengw:这里根据GetNewerActivity方法获取就行了吧？
            //mazq回复：性能有差异
            if (ownerType == null)
            {
                ownerType = ActivityOwnerTypes.Instance().User();
            }
            long userId = 0;
            int newerCount = activityRepository.GetNewerCount(ownerId, lastActivityId, applicationId, out userId, ownerType);
            operatorName = string.Empty;
            if (userId > 0)
            {
                var user = DIContainer.Resolve<IUserService>().GetUser(userId);
                if (user != null)
                    operatorName = user.DisplayName;
            }
            return newerCount;
        }

        /// <summary>
        /// 查询自lastActivityId以后进入用户时间线的动态
        /// </summary>
        /// <param name="ownerId">拥有者Id</param>
        /// <param name="lastActivityId">上次最后呈现的ActivityId</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="ownerType">动态拥有者类型</param>
        /// <returns>lastActivityId</returns>
        public IEnumerable<Activity> GetNewerActivities(long ownerId, long lastActivityId, int? applicationId, int? ownerType = null)
        {
            //根据lastActivityId获取到LastModified，获取晚于lastActivity.LastModified的动态并按LastModified倒排序
            if (ownerType == null)
            {
                ownerType = ActivityOwnerTypes.Instance().User();
            }
            return activityRepository.GetNewerActivities(ownerId, lastActivityId, applicationId, ownerType);
        }

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
        public PagingDataSet<Activity> GetOwnerActivities(int ownerType, long ownerId, int? applicationId, MediaType? mediaType, bool? isOriginalThread, bool? isPrivate, int pageIndex, long? userId = null)
        {
            //按tn_Activities的LastModified倒排序，仅获取IsPrivate=false的动态
            return activityRepository.GetOwnerActivities(ownerType, ownerId, applicationId, mediaType, isOriginalThread, isPrivate, pageIndex, userId);
        }

        /// <summary>
        /// 获取站点动态
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <param name="pageIndex">页码</param>
        ///<returns></returns>
        public IEnumerable<Activity> GetSiteActivities(int? applicationId, int pageSize, int pageIndex)
        {
            //需要tn_ActivitySiteInbox与tn_Activities关联，按tn_Activities的LastModified倒排序
            return activityRepository.GetSiteActivities(applicationId, pageSize, pageIndex);
        }

        #endregion


        #region 动态设置

        /// <summary>
        /// 获取所有的动态项目
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ActivityItem> GetActivityItems()
        {
            return activityItemRepository.GetAll("DisplayOrder");
        }

        /// <summary>
        /// 获取动态项目
        /// </summary>
        /// <param name="itemKey">动态项目标识</param>
        /// <returns></returns>
        public ActivityItem GetActivityItem(string itemKey)
        {
            return GetActivityItems().FirstOrDefault(i => i.ItemKey.Equals(itemKey, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 更新动态项目
        /// </summary>
        /// <param name="activityItems">动态项目集合</param>
        public void UpdateActivityItems(IEnumerable<ActivityItem> activityItems)
        {
            //仅允许更新IsUserReceived、IsSiteReceived
            activityItemRepository.UpdateActivityItems(activityItems);
        }

        /// <summary>
        /// 更新用户的动态设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userSettigns"><remarks>key=itemKey,value=IsReceived</remarks></param>
        public void UpdateActivityItemUserSettings(long userId, Dictionary<string, bool> userSettigns)
        {
            activityItemUserSettingRepository.UpdateActivityItemUserSettings(userId, userSettigns);
        }

        /// <summary>
        /// 获取用户的动态设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns><para>如果用户无设置返回空集合</para><remarks>key=itemKey,value=IsReceived</remarks></returns>
        public Dictionary<string, bool> GetActivityItemUserSettings(long userId)
        {
            return activityItemUserSettingRepository.GetActivityItemUserSettings(userId);
        }

        //编辑用户的动态设置时
        //1、首先获取整站默认设置，然后用用户设置进行覆盖；

        #endregion


    }
}
