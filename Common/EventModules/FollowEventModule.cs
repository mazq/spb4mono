//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Events;
using Tunynet.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 关注相关事件
    /// </summary>
    public class FollowEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<FollowEntity>.Instance().After += new CommonEventHandler<FollowEntity, CommonEventArgs>(FollowNoticeModule_After);
            EventBus<FollowEntity>.Instance().After += new CommonEventHandler<FollowEntity, CommonEventArgs>(FollowPointModule_After);
            EventBus<FollowEntity>.Instance().After += new CommonEventHandler<FollowEntity, CommonEventArgs>(FollowUpdateCountModule_After);
            EventBus<FollowEntity>.Instance().After += new CommonEventHandler<FollowEntity, CommonEventArgs>(FollowActivityEventModule_After);
            EventBus<FollowEntity>.Instance().After += new CommonEventHandler<FollowEntity, CommonEventArgs>(CancelFollowEventModule_After);
            EventBus<int, BatchFollowEventArgs>.Instance().After += new CommonEventHandler<int, BatchFollowEventArgs>(BatchFollowPointModule_After);
        }

        /// <summary>
        /// 用户积分处理
        /// </summary>
        /// <param name="sender">关注实体</param>
        /// <param name="eventArgs">事件参数</param>
        void FollowPointModule_After(FollowEntity sender, CommonEventArgs eventArgs)
        {
            IUserService userservice = DIContainer.Resolve<IUserService>();
            IUser followedUser = userservice.GetUser(sender.FollowedUserId);
            if (followedUser == null)
            {
                return;
            }

            string pointItemKey = string.Empty;
            PointService pointService = new PointService();
            string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_FollowUser"), followedUser.DisplayName);

            #region 设置积分项Key

            if (EventOperationType.Instance().Create() == eventArgs.EventOperationType)
                pointItemKey = PointItemKeys.Instance().FollowUser();
            else if (EventOperationType.Instance().Delete() == eventArgs.EventOperationType)
                pointItemKey = PointItemKeys.Instance().CancelFollowUser();

            #endregion

            pointService.GenerateByRole(sender.UserId, pointItemKey, description);

        }

        /// <summary>
        /// 批量关注积分处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void BatchFollowPointModule_After(int sender, BatchFollowEventArgs eventArgs)
        {         
            
            if (EventOperationType.Instance().Create() == eventArgs.EventOperationType && sender > 0)
            {
                string pointItemKey = string.Empty;
                pointItemKey = PointItemKeys.Instance().FollowUser();
                PointService pointService = new PointService();
                
                for(int i=0; i<sender; i++)
                {
                    pointService.GenerateByRole(eventArgs.UserId, pointItemKey, "批量添加关注");                
                }
            }
        
        }

        /// <summary>
        /// 生成通知
        /// </summary>
        /// <param name="sender">关注实体</param>
        /// <param name="eventArgs">事件参数</param>
        void FollowNoticeModule_After(FollowEntity sender, CommonEventArgs eventArgs)
        {
            if (EventOperationType.Instance().Create() == eventArgs.EventOperationType)
            {
                if (sender.IsQuietly)
                    return;
                IUserService userService = DIContainer.Resolve<IUserService>();

                //关注用户
                IUser user = userService.GetUser(sender.UserId);
                if (user == null)
                    return;

                IUser followedUser = userService.GetUser(sender.FollowedUserId);
                if (followedUser == null)
                    return;

                NoticeService service = new NoticeService();
                Notice notice = Notice.New();
                notice.TypeId = NoticeTypeIds.Instance().Hint();
                notice.TemplateName = "FollowUser";
                notice.UserId = followedUser.UserId;
                notice.LeadingActorUserId = user.UserId;
                notice.LeadingActor = user.DisplayName;
                notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(user.UserName));
                notice.RelativeObjectId = followedUser.UserId;
                notice.RelativeObjectName = followedUser.DisplayName;
                notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(followedUser.UserName));

                service.Create(notice);                
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                NoticeService service = new NoticeService();
                IEnumerable<Notice> notices = service.GetTops(sender.FollowedUserId, 20).Where(n => n.TemplateName == "FollowUser").Where(n => n.LeadingActorUserId == sender.UserId);
                foreach (var notice in notices)
                {
                    service.Delete(notice.Id);
                }
            }
        }

        /// <summary>
        /// 关注用户/取消关注动态处理
        /// </summary>
        /// <param name="sender">关注实体</param>
        /// <param name="eventArgs">事件参数</param>
        void FollowActivityEventModule_After(FollowEntity sender, CommonEventArgs eventArgs)
        {
            ActivityService activityService = new ActivityService();
            if (EventOperationType.Instance().Create() == eventArgs.EventOperationType)
            {
                UserService userService = new UserService();
                IUser user = userService.GetUser(sender.UserId);
                if (user == null) return;

                Activity activity = Activity.New();

                activity.ActivityItemKey = ActivityItemKeys.Instance().FollowUser();
                activity.OwnerType = ActivityOwnerTypes.Instance().User();
                activity.OwnerId = sender.UserId;
                activity.OwnerName = user.DisplayName;
                activity.UserId = sender.UserId;
                activity.ReferenceId = sender.FollowedUserId;
                activity.TenantTypeId = TenantTypeIds.Instance().User();

                activityService.Generate(activity, false);

                activityService.TraceBackInboxAboutOwner(sender.UserId, sender.FollowedUserId, ActivityOwnerTypes.Instance().User());
            }
            else if (EventOperationType.Instance().Delete() == eventArgs.EventOperationType)
            {
                activityService.RemoveInboxAboutOwner(sender.UserId, sender.FollowedUserId, ActivityOwnerTypes.Instance().User());
            }
        }

        /// <summary>
        /// 关注后更新缓存
        /// </summary>
        /// <param name="sender">关注实体</param>
        /// <param name="eventArgs">事件参数</param>
        void FollowUpdateCountModule_After(FollowEntity sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                //更新用户缓存
                ICacheService cacheService = DIContainer.Resolve<ICacheService>();
                RealTimeCacheHelper realTimeCacheHelper = EntityData.ForType(typeof(User)).RealTimeCacheHelper;
                if (cacheService.EnableDistributedCache)
                {
                    realTimeCacheHelper.IncreaseEntityCacheVersion(sender.UserId);
                }
                else
                {
                    string cacheKey = realTimeCacheHelper.GetCacheKeyOfEntity(sender.UserId);
                    User user = cacheService.Get<User>(cacheKey);
                    if (user != null)
                    {
                        user.FollowedCount++;
                    }
                }
            }
        }

        /// <summary>
        /// 取消关注后更新缓存
        /// </summary>
        /// <param name="sender">关注实体</param>
        /// <param name="eventArgs">事件参数</param>
        void CancelFollowEventModule_After(FollowEntity sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                CategoryService service = new CategoryService();
                service.ClearCategoriesFromItem(sender.Id, sender.UserId, TenantTypeIds.Instance().User());

                //更新用户缓存
                ICacheService cacheService = DIContainer.Resolve<ICacheService>();
                RealTimeCacheHelper realTimeCacheHelper = EntityData.ForType(typeof(User)).RealTimeCacheHelper;
                if (cacheService.EnableDistributedCache)
                {
                    realTimeCacheHelper.IncreaseEntityCacheVersion(sender.UserId);
                }
                else
                {
                    string cacheKey = realTimeCacheHelper.GetCacheKeyOfEntity(sender.UserId);
                    User user = cacheService.Get<User>(cacheKey);
                    if (user != null && user.FollowedCount > 0)
                    {
                        user.FollowedCount--;
                    }
                }
            }
        }
    }
}