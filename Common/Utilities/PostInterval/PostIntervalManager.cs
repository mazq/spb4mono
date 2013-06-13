//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 发帖时间间隔工具类
    /// </summary>
    public static class PostIntervalManager
    {

        private static ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 检查内容发帖时间间隔
        /// </summary>
        public static bool CheckPostInterval(PostIntervalType type)
        {
            DateTime dt = DateTime.Now;

            IUser user = UserContext.CurrentUser;
            if (user == null || !PostIntervalSettings.Instance().EnablePostInterval)
                return false;

            string cacheKey = GetCahceKey_PostInterval(user.UserName);
            bool checkPostInterval = false;

            PostIntervalEntity postIntervalEntity = cacheService.Get<PostIntervalEntity>(cacheKey);
            if (postIntervalEntity == null)
                postIntervalEntity = new PostIntervalEntity();

            if (postIntervalEntity.LastPostDates == null || !postIntervalEntity.LastPostDates.ContainsKey(type))
            {
                postIntervalEntity.LastPostDates = new Dictionary<PostIntervalType, DateTime>();
                postIntervalEntity.LastPostDates[type] = DateTime.MinValue;
            }

            if (postIntervalEntity.PostContentCounts == null || !postIntervalEntity.PostContentCounts.ContainsKey(type))
            {
                postIntervalEntity.PostContentCounts = new Dictionary<PostIntervalType, int>();
                postIntervalEntity.PostContentCounts[type] = 0;
            }

            TimeSpan interval = dt.Subtract(postIntervalEntity.LastPostDates[type]);

            int seconds = PostIntervalSettings.Instance().PostIntervals[type];

            if (interval.TotalSeconds <= seconds)
            {
                if (PostIntervalSettings.Instance().PostCounts[type] <= 0)
                    return true;
                else if (postIntervalEntity.PostContentCounts[type] >= PostIntervalSettings.Instance().PostCounts[type])
                {
                    postIntervalEntity.LastPostDates[type] = DateTime.Now.AddSeconds(seconds);
                    checkPostInterval = true;
                }

                postIntervalEntity.PostContentCounts[type]++;
            }
            else
            {
                postIntervalEntity.PostContentCounts[type] = 0;
                postIntervalEntity.LastPostDates[type] = DateTime.Now;
            }


            cacheService.Set(cacheKey, postIntervalEntity, CachingExpirationType.RelativelyStable);

            return checkPostInterval;
        }

        #region CacheKey

        private static string GetCahceKey_PostInterval(string userName)
        {
            return "PostInterval:" + userName;
        }

        #endregion
    }
}
