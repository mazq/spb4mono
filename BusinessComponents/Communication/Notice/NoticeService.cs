//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Utilities;
using Tunynet.Common.Repositories;
using Tunynet.Events;

namespace Tunynet.Common
{
    /// <summary>
    /// 通知业务逻辑类
    /// </summary>
    public class NoticeService
    {
        private INoticeRepository noticeRepository;
        private IUserNoticeSettingsRepository userNoticeSettingsRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public NoticeService()
            : this(new NoticeRepository(), new UserNoticeSettingsRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public NoticeService(INoticeRepository noticeRepository, IUserNoticeSettingsRepository userNoticeSettingsRepository)
        {
            this.noticeRepository = noticeRepository;
            this.userNoticeSettingsRepository = userNoticeSettingsRepository;
        }

        /// <summary>
        /// 创建通知
        /// </summary>
        /// <param name="entity">通知实体</param>
        /// <remarks>已检查通知设置</remarks>
        public void Create(Notice entity)
        {
            EventBus<Notice>.Instance().OnBefore(entity, new CommonEventArgs(EventOperationType.Instance().Create(), entity.ApplicationId));
            if (!IsAllowedSend(entity.UserId, entity.TypeId))
                return;
            noticeRepository.Insert(entity);
            EventBus<Notice>.Instance().OnAfter(entity, new CommonEventArgs(EventOperationType.Instance().Create(), entity.ApplicationId));
        }

        /// <summary>
        /// 删除单条通知
        /// </summary>
        /// <param name="id">通知Id</param>
        public void Delete(long id)
        {
            Notice notice = noticeRepository.Get(id);
            EventBus<Notice>.Instance().OnBefore(notice, new CommonEventArgs(EventOperationType.Instance().Delete(), notice.ApplicationId));
            noticeRepository.DeleteByEntityId(id);
            EventBus<Notice>.Instance().OnAfter(notice, new CommonEventArgs(EventOperationType.Instance().Delete(), notice.ApplicationId));
        }

        /// <summary>
        /// 清空接收人的通知记录
        /// </summary>
        /// <param name="userId">接收人Id</param>
        /// <param name="status">通知状态</param>
        public void ClearAll(long userId, NoticeStatus? status = null)
        {
            noticeRepository.ClearAll(userId, status);
        }

        /// <summary>
        /// 删除用户记录（删除用户时使用）
        /// </summary>
        /// <param name="userId"></param>
        public void CleanByUser(long userId)
        {
            noticeRepository.CleanByUser(userId);
        }


        /// <summary>
        /// 将通知设置为已处理状态
        /// </summary>
        /// <param name="id">通知Id</param>
        public void SetIsHandled(long id)
        {
            Notice notice = noticeRepository.Get(id);
            EventBus<Notice>.Instance().OnBefore(notice, new CommonEventArgs(EventOperationType.Instance().Update(), notice.ApplicationId));
            noticeRepository.SetIsHandled(id);
            EventBus<Notice>.Instance().OnAfter(notice, new CommonEventArgs(EventOperationType.Instance().Update(), notice.ApplicationId));
        }

        /// <summary>
        /// 批量将所有未处理通知修改为已处理状态
        /// </summary>
        /// <param name="userId">接收人Id</param>
        public void BatchSetIsHandled(long userId)
        {
            IEnumerable<Notice> notices = noticeRepository.GetTops(userId, int.MaxValue);
            EventBus<Notice>.Instance().OnBatchBefore(notices, new CommonEventArgs(EventOperationType.Instance().Update()));
            noticeRepository.BatchSetIsHandled(userId);
            EventBus<Notice>.Instance().OnBatchAfter(notices, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 获取单个通知实体
        /// </summary>
        /// <param name="id">通知实体Id</param>
        /// <returns>通知实体</returns>
        public Notice Get(long id)
        {
            return noticeRepository.Get(id);
        }

        /// <summary>
        /// 获取某人的未处理通知数
        /// </summary>
        public int GetUnhandledCount(long userId)
        {
            return noticeRepository.GetUnhandledCount(userId);
        }

        /// <summary>
        /// 获取用户最近几条未处理的通知
        /// </summary>
        /// <param name="topNumber"></param>
        /// <param name="userId">通知接收人Id</param>
        public IEnumerable<Notice> GetTops(long userId, int topNumber)
        {
            //按照创建日期倒序排序，并注意只查询未处理的通知
            return noticeRepository.GetTops(userId, topNumber);
        }

        /// <summary>
        /// 获取用户通知的分页集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">通知状态</param>
        /// <param name="typeId">通知类型Id</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>通知分页集合</returns>
        public PagingDataSet<Notice> Gets(long userId, NoticeStatus? status, int? typeId, int? applicationId, int? pageIndex = null)
        {
            //按照创建日期倒序排序
            return noticeRepository.Gets(userId, status, typeId, applicationId, pageIndex ?? 1);
        }

        /// <summary>
        /// 获取所有未处理的通知
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserReminderInfo> GetUserReminderInfos()
        {
            return noticeRepository.GetUserReminderInfos();
        }

        #region 通知设置
        /// <summary>
        /// 用户对某类型是否允许发送通知
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="typeId">通知类型Id</param>
        /// <returns></returns>
        public bool IsAllowedSend(long userId, int typeId)
        {
            Dictionary<int, bool> userSettings = userNoticeSettingsRepository.GetUserNoticeSettingses(userId);
            if (userSettings.ContainsKey(typeId))
                return userSettings[typeId];
            INoticeSettingsManager noticeSettingsManager = DIContainer.Resolve<INoticeSettingsManager>();
            NoticeSettings noticeSettings = noticeSettingsManager.Get();
            if (!noticeSettings.NoticeTypeSettingses.Any(n=>n.TypeId==typeId))
                return false;
            return noticeSettings.NoticeTypeSettingses.Single(n=>n.TypeId==typeId).IsAllow;
        }

        /// <summary>
        /// 用户更新通知设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="typeIds">通知类型Id集合</param>
        public void UpdateUserNoticeSettings(long userId, Dictionary<int, bool> userNoticeSettings)
        {
            //先删除以前的设置，然后插入新设置值
            //更新用户通知设置集合缓存
            userNoticeSettingsRepository.UpdateUserNoticeSettings(userId, userNoticeSettings);
        }

        /// <summary>
        /// 获取用户的当前设置
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户的当前设置</returns>
        public Dictionary<int, bool> GetUserNoticeSettingses(long userId)
        {
            return userNoticeSettingsRepository.GetUserNoticeSettingses(userId);
        }

        #endregion
    }
}