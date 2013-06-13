//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户提醒设置数据访问类
    /// </summary>
    public class UserReminderSettingsRepository : Repository<UserReminderSettings>, IUserReminderSettingsRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 用户获取所有提醒设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户提醒设置集合（Key：提醒方式Id，Value：提醒设置实体）</returns>
        public Dictionary<string, IEnumerable<UserReminderSettings>> GetAllUserReminderSettings(long userId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
            cacheKey.Append("UserReminderSettings");

            IEnumerable<UserReminderSettings> reminderSettingsList = cacheService.Get<IEnumerable<UserReminderSettings>>(cacheKey.ToString());
            if (reminderSettingsList == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("*")
                    .From("tn_UserReminderSettings")
                    .Where("UserId = @0", userId);

                reminderSettingsList = CreateDAO().Fetch<UserReminderSettings>(sql);

                if (reminderSettingsList != null && reminderSettingsList.Count() > 0)
                {
                    cacheService.Add(cacheKey.ToString(), reminderSettingsList, CachingExpirationType.Stable);
                }
            }

            Dictionary<string, IEnumerable<UserReminderSettings>> userReminderSettings = new Dictionary<string, IEnumerable<UserReminderSettings>>();
            if (reminderSettingsList != null && reminderSettingsList.Count() > 0)
            {
                foreach (int modeId in ReminderMode.GetAll().Select(n => n.ModeId))
                {
                    userReminderSettings.Add(modeId.ToString(), reminderSettingsList.Where(n => n.ReminderModeId == modeId).ToList());
                }
            }

            return userReminderSettings;
        }

        /// <summary>
        /// 用户批量更新提醒设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userReminderSettings">用户提醒设置集合</param>
        public void BatchUpdateUserReminderSettings(long userId, IEnumerable<UserReminderSettings> userReminderSettings)
        {
            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();
            bool hasSetting = false;
            var sql = PetaPoco.Sql.Builder;
            sql.Select("Id")
               .From("tn_UserReminderSettings")
               .Where("userId=@0", userId);

            List<long> ids = dao.Fetch<long>(sql);

            IEnumerable<UserReminderSettings> oldUserReminderSettings = PopulateEntitiesByEntityIds(ids);

            foreach (var userReminderSetting in userReminderSettings)
            {
                foreach (var oldUserReminderSetting in oldUserReminderSettings)
                {
                    if (oldUserReminderSetting.ReminderInfoTypeId == userReminderSetting.ReminderInfoTypeId && oldUserReminderSetting.ReminderModeId == userReminderSetting.ReminderModeId)
                    {
                        hasSetting = true;
                        if (oldUserReminderSetting.ReminderThreshold == userReminderSetting.ReminderThreshold &&
                            oldUserReminderSetting.IsEnabled == userReminderSetting.IsEnabled &&
                            oldUserReminderSetting.IsRepeated == userReminderSetting.IsRepeated &&
                            oldUserReminderSetting.RepeatInterval == userReminderSetting.RepeatInterval)
                        {
                            break;
                        }
                        else
                        {
                            oldUserReminderSetting.IsEnabled = userReminderSetting.IsEnabled;
                            oldUserReminderSetting.IsRepeated = userReminderSetting.IsRepeated;
                            oldUserReminderSetting.ReminderThreshold = userReminderSetting.ReminderThreshold;
                            oldUserReminderSetting.RepeatInterval = userReminderSetting.RepeatInterval;
                            dao.Update(oldUserReminderSetting);
                            break;
                        }
                    }

                }
                if (!hasSetting)
                {
                    dao.Insert(userReminderSetting);
                }
                hasSetting = false;

            }
            dao.CloseSharedConnection();

            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }
    }
}