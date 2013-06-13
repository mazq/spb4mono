//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    //默认每10分钟执行一次
    /// <summary>
    /// 定期发送提醒任务
    /// </summary>
    public class SendReminderTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            IEnumerable<IReminderInfoAccessor> reminderInfoTypeAccessors = DIContainer.Resolve<IEnumerable<IReminderInfoAccessor>>();
            //提醒信息类型查询实例集合
            IEnumerable<IReminderSender> reminderSenders = DIContainer.Resolve<IEnumerable<IReminderSender>>();
            //发送提醒实例集合
            ReminderService reminderService = new ReminderService();//提醒业务逻辑实例
            IReminderSettingsManager reminderSettingsManager = DIContainer.Resolve<IReminderSettingsManager>();
            ReminderSettings reminderSettings = reminderSettingsManager.Get();//站点提醒设置
            ConcurrentDictionary<int, ConcurrentDictionary<long, List<UserReminderInfo>>> waitSendDict = new ConcurrentDictionary<int, ConcurrentDictionary<long, List<UserReminderInfo>>>();

            //遍历各种提醒信息类型
            foreach (var reminderInfoTypeAccessor in reminderInfoTypeAccessors)
            {
                int reminderInfoTypeId = reminderInfoTypeAccessor.ReminderInfoTypeId;
                ReminderInfoType reminderInfoType = ReminderInfoType.Get(reminderInfoTypeAccessor.ReminderInfoTypeId);
                if (reminderInfoType == null)
                    continue;
                //查询该信息类型下的提醒信息集合
                IEnumerable<UserReminderInfo> userReminderInfos = reminderInfoTypeAccessor.GetUserReminderInfos();
                foreach (var userReminderInfo in userReminderInfos.ToList())
                {
                    if (userReminderInfo.UserId <= 0)
                        continue;
                    foreach (var reminderMode in ReminderMode.GetAll())
                    {
                        int reminderModeId = reminderMode.ModeId;

                        UserReminderSettings userReminderSettings = reminderService.GetUserReminderSettings(userReminderInfo.UserId, reminderModeId, reminderInfoTypeId);
                        if (userReminderSettings == null)
                            continue;
                        //1.	判断用户是否启用了提醒并可以使用该提醒方式；
                        if (!userReminderSettings.IsEnabled)
                            continue;

                        if (!waitSendDict.ContainsKey(reminderModeId))
                            waitSendDict[reminderModeId] = new ConcurrentDictionary<long, List<UserReminderInfo>>();
                        if (!waitSendDict[reminderModeId].ContainsKey(userReminderInfo.UserId))
                            waitSendDict[reminderModeId][userReminderInfo.UserId] = new List<UserReminderInfo>();

                        //2.	从用户设置中取发送提醒时间阈值，遍历提醒信息集合筛选出超过此值的提醒信息集合；
                        IEnumerable<ReminderInfo> reminderInfos = userReminderInfo.ReminderInfos.ToList();

                        reminderInfos = reminderInfos.Where(n => n.DateCreated.AddMinutes(userReminderSettings.ReminderThreshold) <= DateTime.UtcNow).ToList();
                        //3.	再判断是否重复提醒用户
                        if (userReminderSettings.IsRepeated)
                        {
                            //   根据相应提醒记录的最后提醒时间和当前时间求差值是否大于重复提醒时间间隔进行筛选，如果提醒记录不存在也应筛选出
                            IEnumerable<ReminderRecord> records = reminderService.GetRecords(userReminderInfo.UserId, reminderModeId, reminderInfoTypeId);
                            reminderInfos = reminderInfos.Where(n =>
                                                      {
                                                          ReminderRecord record = records.FirstOrDefault(m => m.ObjectId == n.ObjectId);
                                                          if (record == null)
                                                              return true;
                                                          TimeSpan ts = DateTime.UtcNow - record.LastReminderTime;
                                                          return ts.TotalMinutes > userReminderSettings.RepeatInterval;
                                                      }).ToList();
                            if (reminderInfos.Count() == 0)
                                continue;
                            IEnumerable<long> objectIds = reminderInfos.Select(n => n.ObjectId);
                            //更新最后提醒时间
                            IEnumerable<long> requireUpdatedObjectIds = objectIds.Where(n => reminderService.IsExits(userReminderInfo.UserId, reminderModeId, reminderInfoTypeId, n));
                            reminderService.UpdateRecoreds(userReminderInfo.UserId, reminderModeId, reminderInfoTypeId, requireUpdatedObjectIds);
                            //如果记录不存在则创建
                            IEnumerable<long> requireCreatedObjectIds = objectIds.Except(requireUpdatedObjectIds);
                            reminderService.CreateRecords(userReminderInfo.UserId, reminderModeId, reminderInfoTypeId, requireCreatedObjectIds);
                        }
                        else
                        {
                            // 根据是否提醒过筛选发送提醒信息集合
                            reminderInfos = reminderInfos.Where(n => !reminderService.IsExits(userReminderInfo.UserId, reminderModeId, reminderInfoTypeId, n.ObjectId)).ToList();
                            if (reminderInfos.Count() == 0)
                                continue;
                            //创建提醒记录
                            reminderService.CreateRecords(userReminderInfo.UserId, reminderModeId, reminderInfoTypeId, reminderInfos.Select(n => n.ObjectId));
                        }

                        userReminderInfo.SetReminderInfos(reminderInfos.ToList());
                        //生成处理地址
                        userReminderInfo.ProcessUrl = reminderInfoTypeAccessor.GetProcessUrl(userReminderInfo.UserId);

                        waitSendDict[reminderModeId][userReminderInfo.UserId].Add(userReminderInfo);
                    }
                }
            }

            if (waitSendDict != null && waitSendDict.Count > 0)
            {
                foreach (var reminderSender in reminderSenders)
                {
                    if (!waitSendDict.ContainsKey(reminderSender.ReminderModeId))
                    {
                        continue;
                    }

                    foreach (KeyValuePair<long, List<UserReminderInfo>> pair in waitSendDict[reminderSender.ReminderModeId])
                    {
                        //发送提醒
                        reminderSender.SendReminder(pair.Value);
                    }
                }
            }
        }
    }
}