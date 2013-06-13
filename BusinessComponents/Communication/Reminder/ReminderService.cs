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
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 提醒业务逻辑类
    /// </summary>
    public class ReminderService
    {
        private IReminderRecordRepository reminderRecordRepository;
        private IUserReminderSettingsRepository userReminderSettingsRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public ReminderService()
            : this(new ReminderRecordRepository(), new UserReminderSettingsRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ReminderService(IReminderRecordRepository reminderRecordRepository, IUserReminderSettingsRepository userReminderSettingsRepository)
        {
            this.reminderRecordRepository = reminderRecordRepository;
            this.userReminderSettingsRepository = userReminderSettingsRepository;
        }

        /// <summary>
        /// 创建提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式</param>
        /// <param name="reminderInfoTypeId">提醒信息类型</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        public void CreateRecords(long userId, int reminderModeId, int reminderInfoTypeId, IEnumerable<long> objectIds)
        {
            reminderRecordRepository.CreateRecords(userId, reminderModeId, reminderInfoTypeId, objectIds);
        }

        /// <summary>
        /// 更新提醒记录（只更新最后提醒时间）
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        /// <param name="objectIds">提醒对象Id集合</param>
        public void UpdateRecoreds(long userId, int reminderModeId, int reminderInfoTypeId, IEnumerable<long> objectIds)
        {
            reminderRecordRepository.UpdateRecoreds(userId, reminderModeId, reminderInfoTypeId, objectIds);
        }

        /// <summary>
        /// 删除用户数据（删除用户时使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            reminderRecordRepository.CleanByUser(userId);
        }

        /// <summary>
        /// 清除垃圾提醒记录
        /// </summary>
        public void DeleteTrashRecords()
        {
            IReminderSettingsManager reminderSettingsManager = DIContainer.Resolve<IReminderSettingsManager>();
            ReminderSettings reminderSettings = reminderSettingsManager.Get();

            reminderRecordRepository.DeleteTrashRecords(reminderSettings.ReminderRecordStorageDay);
        }

        /// <summary>
        /// 获取用户所有的提醒记录
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        public IEnumerable<ReminderRecord> GetRecords(long userId, int reminderModeId, int reminderInfoTypeId)
        {
            return reminderRecordRepository.GetRecords(userId, reminderModeId, reminderInfoTypeId);
        }

        /// <summary>
        /// 是否提醒过
        /// </summary>
        /// <param name="userId">被提醒用户Id</param>
        /// <param name="reminderModeId">提醒方式</param>
        /// <param name="reminderInfoTypeId">提醒信息类型</param>
        /// <param name="objectId">提醒对象Id</param>
        public bool IsExits(long userId, int reminderModeId, int reminderInfoTypeId, long objectId)
        {
            IEnumerable<ReminderRecord> records = GetRecords(userId, reminderModeId, reminderInfoTypeId);
            return records.Any(n => n.ObjectId == objectId);
        }

        #region 提醒设置

        /// <summary>
        /// 用户获取提醒设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="reminderModeId">提醒方式Id</param>
        /// <param name="reminderInfoTypeId">提醒信息类型Id</param>
        /// <returns>用户提醒设置</returns>
        public UserReminderSettings GetUserReminderSettings(long userId, int reminderModeId, int reminderInfoTypeId)
        {
            Dictionary<string, IEnumerable<UserReminderSettings>> allUserReminderSettings = GetAllUserReminderSettings(userId);
            if (!allUserReminderSettings.ContainsKey(reminderModeId.ToString()))
                return null;
            return allUserReminderSettings[reminderModeId.ToString()].FirstOrDefault(n => n.ReminderInfoTypeId == reminderInfoTypeId);
        }

        /// <summary>
        /// 用户获取所有提醒设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>Key:提醒方式Id,Value:用户提醒设置集合</returns>
        public Dictionary<string, IEnumerable<UserReminderSettings>> GetAllUserReminderSettings(long userId)
        {
            Dictionary<string, IEnumerable<UserReminderSettings>> userReminderSettingsesDictionary = userReminderSettingsRepository.GetAllUserReminderSettings(userId);
            if (userReminderSettingsesDictionary == null)
                userReminderSettingsesDictionary = new Dictionary<string, IEnumerable<UserReminderSettings>>();
            foreach (var reminderMode in ReminderMode.GetAll())
            {
                int reminderModeId = reminderMode.ModeId;
                IEnumerable<UserReminderSettings> userReminderSettingses = null;
                userReminderSettingsesDictionary.TryGetValue(reminderModeId.ToString(), out userReminderSettingses);
                IReminderSettingsManager ReminderSettingsManager = DIContainer.Resolve<IReminderSettingsManager>();
                ReminderSettings settings = ReminderSettingsManager.Get();
                if (settings == null || settings.ReminderModeSettingses == null)
                    return new Dictionary<string, IEnumerable<UserReminderSettings>>();
                var reminderModeSettings = settings.ReminderModeSettingses.FirstOrDefault(n => n.ModeId == reminderMode.ModeId);
                if (reminderModeSettings == null)
                    continue;
                IEnumerable<ReminderInfoTypeSettings> defaultUserReminderSettingses = reminderModeSettings.ReminderInfoTypeSettingses;

                if (userReminderSettingses == null || userReminderSettingses.Count() == 0 || defaultUserReminderSettingses.Count() > userReminderSettingses.Count())
                {
                    IEnumerable<string> allowedUserRoleNames = reminderModeSettings.AllowedUserRoleNames;
                    //注册用户代表所有用户
                    if (!allowedUserRoleNames.Contains(RoleNames.Instance().RegisteredUsers()))
                    {
                        IUser user = DIContainer.Resolve<IUserService>().GetUser(userId);
                        if (!user.IsInRoles(allowedUserRoleNames.ToArray()))
                            continue;
                    }
                     
                    //合并
                    if (userReminderSettingses == null)
                        userReminderSettingses = new List<UserReminderSettings>();
                    //1.用户没有设置过
                    if (userReminderSettingses.Count() == 0)
                    {
                        userReminderSettingses = defaultUserReminderSettingses.Select(n =>
                        {
                            UserReminderSettings userReminderSettings = UserReminderSettings.New(n);
                            userReminderSettings.UserId = userId;
                            userReminderSettings.ReminderModeId = reminderModeId;

                            return userReminderSettings;
                        }).ToList();
                    }
                    //2.站点新增过提醒类型，则历史用户需要新增这些类型
                    if (defaultUserReminderSettingses.Count() > userReminderSettingses.Count())
                    {
                        IList<UserReminderSettings> userReminderSettingsesList = new List<UserReminderSettings>(userReminderSettingses);
                        foreach (var setting in defaultUserReminderSettingses)
                        {
                            if (!userReminderSettingses.Any(n => n.ReminderInfoTypeId == setting.ReminderInfoTypeId))
                            {
                                UserReminderSettings userReminderSettings = UserReminderSettings.New(setting);
                                userReminderSettings.UserId = userId;
                                userReminderSettings.ReminderModeId = reminderModeId;
                                userReminderSettingsesList.Add(userReminderSettings);
                            }
                        }
                        userReminderSettingses = userReminderSettingsesList;
                    }
                    userReminderSettingsesDictionary[reminderModeId.ToString()] = userReminderSettingses;
                }
            }
            return userReminderSettingsesDictionary;
        }

        /// <summary>
        /// 用户更新提醒设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userReminderSettings">用户提醒设置集合</param>
        public void BatchUpdateUserReminderSettings(long userId, IEnumerable<UserReminderSettings> userReminderSettings)
        {
            EventBus<UserReminderSettings>.Instance().OnBatchBefore(userReminderSettings, new CommonEventArgs(EventOperationType.Instance().Update()));
            userReminderSettingsRepository.BatchUpdateUserReminderSettings(userId, userReminderSettings);
            EventBus<UserReminderSettings>.Instance().OnBatchAfter(userReminderSettings, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        #endregion



    }
}
