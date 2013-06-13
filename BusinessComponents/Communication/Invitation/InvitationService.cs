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
    /// 请求业务逻辑类
    /// </summary>
    public class InvitationService
    {
        private IInvitationRepository invitationRepository;
        private IUserInvitationSettingsRepository userInvitationSettingsRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public InvitationService()
            : this(new InvitationRepository(), new UserInvitationSettingsRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public InvitationService(IInvitationRepository InvitationRepository, IUserInvitationSettingsRepository userInvitationSettingsRepository)
        {
            this.invitationRepository = InvitationRepository;
            this.userInvitationSettingsRepository = userInvitationSettingsRepository;
        }

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="invitation">请求实体</param>
        /// <remarks>已检查请求设置</remarks>
        public void Create(Invitation invitation)
        {
            if (!IsAllowedSend(invitation.UserId, invitation.InvitationTypeKey))
                return;
            //触发事件
            EventBus<Invitation>.Instance().OnBefore(invitation, new CommonEventArgs(EventOperationType.Instance().Create(), invitation.ApplicationId));
            invitationRepository.Insert(invitation);
            //触发事件
            EventBus<Invitation>.Instance().OnAfter(invitation, new CommonEventArgs(EventOperationType.Instance().Create(), invitation.ApplicationId));
        }

        /// <summary>
        /// 删除单条请求
        /// </summary>
        /// <param name="id">请求Id</param>
        public void Delete(long id)
        {
            Invitation invitation = Get(id);
            if (invitation == null)
                return;

            //触发事件
            EventBus<Invitation>.Instance().OnBefore(invitation, new CommonEventArgs(EventOperationType.Instance().Delete(), invitation.ApplicationId));
            invitationRepository.DeleteByEntityId(id);
            //触发事件
            EventBus<Invitation>.Instance().OnAfter(invitation, new CommonEventArgs(EventOperationType.Instance().Delete(), invitation.ApplicationId));
        }

        /// <summary>
        /// 清空接收人的请求记录
        /// </summary>
        /// <param name="userId">接收人Id</param>
        public void ClearAll(long userId)
        {
            invitationRepository.ClearAll(userId);
        }

        /// <summary>
        /// 删除用户的所有记录（删除用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        public void CleanByUser(long userId)
        {
            invitationRepository.CleanByUser(userId);
        }

        /// <summary>
        /// 设置邀请状态
        /// </summary>
        /// <param name="id">请求id</param>
        /// <param name="status">需设置的请求状态</param>
        public void SetStatus(long id, InvitationStatus status)
        {
            //请求状态必须是接受或拒绝
            if (status == InvitationStatus.Unhandled)
                return;
            Invitation invitation = Get(id);
            if (invitation == null)
                return;
            //触发事件
            EventBus<Invitation>.Instance().OnBefore(invitation, new CommonEventArgs(EventOperationType.Instance().Update(), invitation.ApplicationId));

            invitationRepository.SetStatus(invitation, status);
            //触发事件
            EventBus<Invitation>.Instance().OnAfter(invitation, new CommonEventArgs(EventOperationType.Instance().Update(), invitation.ApplicationId));

        }

        /// <summary>
        /// 批量设置邀请状态
        /// </summary>
        /// <param name="userId">请求接收人Id</param>
        /// <param name="status">需设置的请求状态</param>
        public void BatchSetStatus(long userId, InvitationStatus status)
        {
            //请求状态必须是接受或拒绝
            if (status == InvitationStatus.Unhandled)
                return;

            IEnumerable<Invitation> invitations = GetTops(userId, int.MaxValue);
            if (invitations == null || invitations.Count() == 0)
                return;

            //触发事件
            EventBus<Invitation>.Instance().OnBatchBefore(invitations, new CommonEventArgs(EventOperationType.Instance().Update()));

            invitationRepository.BatchSetStatus(userId, status);
            //触发事件
            EventBus<Invitation>.Instance().OnBatchAfter(invitations, new CommonEventArgs(EventOperationType.Instance().Update()));
        }

        /// <summary>
        /// 获取单个请求实体
        /// </summary>
        /// <param name="id">请求实体Id</param>
        /// <returns>请求实体</returns>
        public Invitation Get(long id)
        {
            return invitationRepository.Get(id);
        }

        /// <summary>
        /// 获取我请求过的用户id
        /// </summary>
        /// <param name="senderUserId">发送者id</param>
        /// <param name="invitationTypeKey">请求类型</param>
        /// <param name="applicationId">类型id</param>
        /// <returns>我请求过的用户id</returns>
        public IEnumerable<long> GetMyInvitationUserId(long senderUserId, string invitationTypeKey, int applicationId)
        {
            return invitationRepository.GetMyInvitationUserId(senderUserId, invitationTypeKey, applicationId);
        }

        /// <summary>
        /// 是否发送过邀请
        /// </summary>
        /// <param name="senderUserId">发送者id</param>
        /// <param name="invitationTypeKey">请求类型</param>
        /// <param name="toUserId">被验证的用户Id</param>
        /// <param name="applicationId">类型id</param>
        /// <returns>是否发送过</returns>
        public bool IsSendedInvitation(long senderUserId, long toUserId, string invitationTypeKey, int applicationId)
        {
            IEnumerable<long> MyInvitationUserId = GetMyInvitationUserId(senderUserId, invitationTypeKey, applicationId);
            if (MyInvitationUserId == null || MyInvitationUserId.Count() <= 0)
                return false;
            return MyInvitationUserId.Contains(toUserId);
        }

        /// <summary>
        /// 获取某人的未处理请求数
        /// </summary>
        public int GetUnhandledCount(long userId)
        {
            return invitationRepository.GetUnhandledCount(userId);
        }


        /// <summary>
        /// 获取用户最近几条未处理的请求
        /// </summary>
        /// <param name="topNumber"></param>
        /// <param name="userId">请求接收人Id</param>
        public IEnumerable<Invitation> GetTops(long userId, int topNumber)
        {
            //按照创建日期倒序排序，并注意只查询未处理的请求
            return invitationRepository.GetTops(userId, topNumber);
        }

        /// <summary>
        /// 获取用户请求的分页集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">请求状态</param>
        /// <param name="invitationTypeKey">请求类型Key</param>
        /// <param name="applicationId">应用Id</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>请求分页集合</returns>
        public PagingDataSet<Invitation> Gets(long userId, InvitationStatus? status, string invitationTypeKey, int? applicationId, int? pageIndex = null)
        {
            //按照创建日期倒序排序
            return invitationRepository.Gets(userId, status, invitationTypeKey, applicationId, pageIndex);
        }

        #region 请求设置

        /// <summary>
        /// 用户对某类型是否允许发送请求
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="invitationTypeKey">请求类型Key</param>
        /// <returns></returns>
        public bool IsAllowedSend(long userId, string invitationTypeKey)
        {
            Dictionary<string, bool> userInvitationSettingses = GetUserInvitationSettingses(userId);
            if (userInvitationSettingses.ContainsKey(invitationTypeKey))
                return userInvitationSettingses[invitationTypeKey];
            else
            {
                IInvitationSettingsManager invitationSettingsManager = DIContainer.Resolve<IInvitationSettingsManager>();
                InvitationSettings settings = invitationSettingsManager.Get();
                if (settings.InvitationTypeSettingses.Any(n => n.TypeKey == invitationTypeKey))
                    return settings.InvitationTypeSettingses.Single(n => n.TypeKey == invitationTypeKey).IsAllow;
            }
            return false;
        }

        //编辑用户的请求设置时
        //1、首先获取整站默认设置，然后用用户设置进行覆盖；

        /// <summary>
        /// 用户获取请求设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>请求类型-是否接收设置集合</returns>
        /// <returns><para>如果用户无设置返回空集合</para><remarks>key=itemKey,value=IsReceived</remarks></returns>
        public Dictionary<string, bool> GetUserInvitationSettingses(long userId)
        {
            Dictionary<string, bool> userInvitationSettingses = userInvitationSettingsRepository.GetUserInvitationSettingses(userId);
            if (userInvitationSettingses == null)
                userInvitationSettingses = new Dictionary<string, bool>();
            return userInvitationSettingses;
        }

        /// <summary>
        /// 用户更新请求设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="typeKey2IsAllowDictionary">请求类型-是否接收设置集合</param>
        public void UpdateUserInvitationSettings(long userId, Dictionary<string, bool> typeKey2IsAllowDictionary)
        {
            userInvitationSettingsRepository.UpdateUserInvitationSettings(userId, typeKey2IsAllowDictionary);
        }

        #endregion

        /// <summary>
        /// 获取所有未处理的请求
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserReminderInfo> GetUserReminderInfos()
        {
            return invitationRepository.GetUserReminderInfos();
        }
    }
}