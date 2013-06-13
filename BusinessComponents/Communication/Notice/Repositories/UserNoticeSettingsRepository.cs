//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using Tunynet.Common;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 通知数据访问
    /// </summary>
    public class UserNoticeSettingsRepository : Repository<UserNoticeSettings>, IUserNoticeSettingsRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取用户通知设置
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户设置</returns>
        public Dictionary<int, bool> GetUserNoticeSettingses(long userId)
        {
            string cacheKey = GetCacheKey_UserNoticeSettingses(userId);
            Dictionary<int, bool> userNoticeSettingses = cacheService.Get<Dictionary<int, bool>>(cacheKey);
            if (userNoticeSettingses != null)
                return userNoticeSettingses;
            var sql_Select = PetaPoco.Sql.Builder;
            sql_Select.Select("TypeId,IsAllowable")
                .From("tn_UserNoticeSettings")
                .Where("UserId=@0", userId);
            dynamic userSettings = CreateDAO().Fetch<dynamic>(sql_Select);
            userNoticeSettingses = new Dictionary<int, bool>();
            foreach (var item in userSettings)
                userNoticeSettingses[item.TypeId] = item.IsAllowable == 1;
            cacheService.Add(cacheKey, userNoticeSettingses, CachingExpirationType.ObjectCollection);
            return userNoticeSettingses;
        }

        /// <summary>
        /// 更新用户请求设置
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userNoticeSettings">用户的设置</param>
        public void UpdateUserNoticeSettings(long userId, Dictionary<int, bool> userNoticeSettings)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();
            string cacheKey = GetCacheKey_UserNoticeSettingses(userId);
            Dictionary<int, bool> userNoticeSettingsFormDB = cacheService.Get<Dictionary<int, bool>>(cacheKey);
            if (userNoticeSettingsFormDB == null)
            {
                var sql_Seletct = Sql.Builder;
                sql_Seletct.Select("*").From("tn_UserNoticeSettings").Where("UserId=@0", userId);
                IEnumerable<dynamic> userSettings = dao.Fetch<dynamic>(sql_Seletct);

                userNoticeSettingsFormDB = new Dictionary<int, bool>();
                foreach (var item in userSettings)
                    userNoticeSettingsFormDB[item.TypeId] = item.IsAllowable == 1;
            }
            Dictionary<int, bool> diffNoticeSettings = new Dictionary<int, bool>();
            Dictionary<int, bool> newNoticeSettings = new Dictionary<int, bool>();
            var sql = new List<PetaPoco.Sql>();
            foreach (var item in userNoticeSettings)
            {
                if (userNoticeSettingsFormDB != null && userNoticeSettingsFormDB.ContainsKey(item.Key) && userNoticeSettingsFormDB[item.Key] != item.Value)
                {
                    sql.Add(PetaPoco.Sql.Builder.Append("update tn_UserNoticeSettings set IsAllowable=@0 where UserId=@1 and TypeId=@2", item.Value, userId, item.Key));
                }
                if (userNoticeSettingsFormDB == null || !userNoticeSettingsFormDB.ContainsKey(item.Key))
                    sql.Add(PetaPoco.Sql.Builder.Append("insert into tn_UserNoticeSettings(UserId,TypeId,IsAllowable) values(@0,@1,@2)", userId, item.Key, item.Value));
            }
            dao.Execute(sql);
            dao.CloseSharedConnection();

            if (userNoticeSettingsFormDB != null)
                cacheService.Set(cacheKey, userNoticeSettings, CachingExpirationType.ObjectCollection);
        }

        #region Help Methods

        /// <summary>
        /// 获取用户通知设置的CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        private string GetCacheKey_UserNoticeSettingses(long userId)
        {
            return string.Format("UserNoticeSettingses::UserId-{0}", userId);
        }

        #endregion
    }
}