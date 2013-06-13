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
    /// 用户动态设置仓储
    /// </summary>
    public class ActivityItemUserSettingRepository : IActivityItemUserSettingRepository
    {

        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 缓存设置
        /// </summary>
        protected static RealTimeCacheHelper RealTimeCacheHelper { get { return EntityData.ForType(typeof(ActivityItemUserSetting)).RealTimeCacheHelper; } }


        /// <summary>
        /// 默认PetaPocoDatabase实例
        /// </summary>
        protected PetaPocoDatabase CreateDAO()
        {
            return PetaPocoDatabase.CreateInstance();
        }


        /// <summary>
        /// 更新用户动态设置
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userSettings">用户设置</param>
        public void UpdateActivityItemUserSettings(long userId, Dictionary<string, bool> userSettings)
        {
            Dictionary<string, bool> userSettingsFromDB = GetActivityItemUserSettings(userId);
            List<Sql> sqls = new List<Sql>();
            foreach (var userSetting in userSettings)
            {
                var sql = Sql.Builder;
                if (userSettingsFromDB.ContainsKey(userSetting.Key) && userSettingsFromDB[userSetting.Key] != userSetting.Value)
                {
                    sql.Append("update tn_ActivityItemUserSettings set IsReceived=@0", userSetting.Value).Where("UserId=@0", userId).Where("ItemKey=@0", userSetting.Key);
                    sqls.Add(sql);
                }
                else if (!userSettingsFromDB.ContainsKey(userSetting.Key))
                {
                    sql.Append("insert into tn_ActivityItemUserSettings(UserId,ItemKey,IsReceived) values(@0,@1,@2)", userId, userSetting.Key, userSetting.Value);
                    sqls.Add(sql);
                }
            }
            CreateDAO().Execute(sqls);

            //done:zhengw,by mazq 直接递增分区版本即可，代码过于啰嗦，userSettingsFromCache正常情况下应该存在
            //zhengw回复：已修改

            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        /// <summary>
        /// 获取用户设置
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户设置</returns>
        public Dictionary<string, bool> GetActivityItemUserSettings(long userId)
        {
            string cacheKey = GetCacheKey_ActivityItemUserSettings(userId);
            Dictionary<string, bool> userSettings = cacheService.Get<Dictionary<string, bool>>(cacheKey);
            if (userSettings != null)
                return userSettings;

            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_ActivityItemUserSettings")
                .Where("UserId=@0", userId);
            List<ActivityItemUserSetting> userSettingsFromDB = CreateDAO().Fetch<ActivityItemUserSetting>(sql);
            userSettings = userSettingsFromDB.ToDictionary(n => (string)n.ItemKey, m => (bool)m.IsReceived);
            //done:zhengw,by mazq 缓存策略采用UsualObjectCollection
            //zhengw回复：已修改
            cacheService.Add(cacheKey, userSettings, CachingExpirationType.UsualObjectCollection);
            return userSettings;
        }

        /// <summary>
        /// 获取用户设置的cachekey
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户设置的cachekey</returns>
        private string GetCacheKey_ActivityItemUserSettings(long userId)
        {
            string cachekey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId);
            return cachekey + "::ActivityItemUserSettings";
        }
    }
}