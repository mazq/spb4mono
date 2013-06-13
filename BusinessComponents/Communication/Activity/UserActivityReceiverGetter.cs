//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户动态接收人获取器
    /// </summary>
    public class UserActivityReceiverGetter : IActivityReceiverGetter
    {
        //done:zhengw,by mazq 缺少注释
        //zhengw回复：已修改

        /// <summary>
        /// 获取接收人UserId集合
        /// </summary>
        /// <param name="activityService">动态业务逻辑类</param>
        /// <param name="activity">动态</param>
        /// <returns></returns>
        IEnumerable<long> IActivityReceiverGetter.GetReceiverUserIds(ActivityService activityService, Activity activity)
        {
            //1、获取用户的所有粉丝，然后通过IsReceiveActivity()检查，是否给该粉丝推送动态；
            FollowService followService = new FollowService();
            IEnumerable<long> followerUserIds = followService.GetTopFollowerUserIds(activity.OwnerId, Follow_SortBy.DateCreated_Desc, ValueUtility.GetSqlMaxInt());
            if (followerUserIds == null)
                return new List<long>();
            return followerUserIds.Where(n => IsReceiveActivity(activityService, n, activity));
        }

        /// <summary>
        /// 检查用户是否接收动态
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="activityItemKey">动态项目标识</param>
        /// <param name="fromOwnerId">动态拥有者</param>
        /// <returns>接收动态返回true，否则返回false</returns>
        private bool IsReceiveActivity(ActivityService activityService, long userId, Activity activity)
        {
            //首先检查是否屏蔽用户
            UserBlockService userBlockService = new UserBlockService();
            if (userBlockService.IsBlockedUser(userId, activity.OwnerId))
                return false;

            //检查用户是否接收该动态项目
            Dictionary<string, bool> userSettings = activityService.GetActivityItemUserSettings(userId);
            if (userSettings.ContainsKey(activity.ActivityItemKey))
                return userSettings[activity.ActivityItemKey];
            else
            {
                //如果用户没有设置从默认设置获取
                ActivityItem activityItem = activityService.GetActivityItem(activity.ActivityItemKey);
                if (activityItem != null)
                    return activityItem.IsUserReceived;
                else
                    return true;
            }
        }

    }
}
