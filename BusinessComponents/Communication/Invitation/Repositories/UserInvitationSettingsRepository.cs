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
    public class UserInvitationSettingsRepository : Repository<UserInvitationSettings>, IUserInvitationSettingsRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 用户获取请求设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>请求类型-是否接收设置集合</returns>
        public Dictionary<string, bool> GetUserInvitationSettingses(long userId)
        {
            string cacheKey = GetCacheKey_UserInvitationSettingses(userId);
            Dictionary<string, bool> userInvitationSettingses = cacheService.Get<Dictionary<string, bool>>(cacheKey);
            if (userInvitationSettingses == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("InvitationTypeKey,IsAllowable")
                    .From("tn_UserInvitationSettings")
                    .Where("UserId=@0", userId);
                IEnumerable<dynamic> records = CreateDAO().Fetch<dynamic>(sql);

                userInvitationSettingses = records.ToDictionary(n => (string)n.InvitationTypeKey, n => (byte)n.IsAllowable == 1);
                cacheService.Set(cacheKey, userInvitationSettingses, CachingExpirationType.ObjectCollection);
            }
            return userInvitationSettingses;
        }



        /// <summary>
        /// 用户更新请求设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="typeKey2IsAllowDictionary">请求类型-是否接收设置集合</param>
        public void UpdateUserInvitationSettings(long userId, Dictionary<string, bool> typeKey2IsAllowDictionary)
        {
            int effectLineCount = 0;
            Dictionary<string, bool> dictionaryUserInvitationSettingses = GetUserInvitationSettingses(userId);

            var sqls = new List<PetaPoco.Sql>();
            foreach (var typeKey in typeKey2IsAllowDictionary.Keys)
            {
                if (!dictionaryUserInvitationSettingses.ContainsKey(typeKey))
                {
                    var sql = PetaPoco.Sql.Builder.Append("insert into tn_UserInvitationSettings(UserId,InvitationTypeKey,IsAllowable) values(@0,@1,@2)", userId, typeKey, typeKey2IsAllowDictionary[typeKey]);
                    sqls.Add(sql);
                }
                else if (typeKey2IsAllowDictionary[typeKey] != dictionaryUserInvitationSettingses[typeKey])
                {
                    var sql = PetaPoco.Sql.Builder.Append("update tn_UserInvitationSettings set IsAllowable=@0  where UserId=@1 and InvitationTypeKey=@2", typeKey2IsAllowDictionary[typeKey], userId, typeKey);
                    sqls.Add(sql);
                }
            }
            effectLineCount = CreateDAO().Execute(sqls);

            //更新缓存
            if (effectLineCount > 0)
            {
                string cacheKey = GetCacheKey_UserInvitationSettingses(userId);
                Dictionary<string, bool> dictionaryUserInvitationSettingsesCache = cacheService.Get<Dictionary<string, bool>>(cacheKey);
                if (dictionaryUserInvitationSettingsesCache != null)
                {
                    cacheService.Set(cacheKey, typeKey2IsAllowDictionary, CachingExpirationType.ObjectCollection);
                }
            }
        }

        /// <summary>
        /// 获取请求通知设置的CacheKey
        /// </summary>
        /// <param name="userId">用户Id</param>
        private string GetCacheKey_UserInvitationSettingses(long userId)
        {
            return string.Format("UserInvitationSettingses::UserId-{0}", userId);
        }
    }
}