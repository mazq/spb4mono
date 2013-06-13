//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Repositories;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// 私信逻辑类
    /// </summary>
    public class MessageService
    {
        private IMessageRepository messageRepository;
        private IMessageSessionRepository messageSessionRepository;
        private IMessageInSessionRepository messageInSessionRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public MessageService()
            : this(new MessageRepository(), new MessageSessionRepository(), new MessageInSessionRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public MessageService(IMessageRepository messageRepository, IMessageSessionRepository messageSessionRepository, IMessageInSessionRepository messageInSessionRepository)
        {
            this.messageRepository = messageRepository;
            this.messageSessionRepository = messageSessionRepository;
            this.messageInSessionRepository = messageInSessionRepository;
        }

        #region 私信

        /// <summary>
        /// 创建私信
        /// </summary>
        /// <param name="message">待创建的私信实体</param>
        /// <returns>是否删除成功：true-成功，false-不成功</returns>
        public bool Create(Message message)
        {
            if (message == null)
                return false;

            EventBus<Message>.Instance().OnBefore(message, new CommonEventArgs(EventOperationType.Instance().Create()));
            long id = 0;
            long.TryParse(messageRepository.Insert(message).ToString(), out id);
            EventBus<Message>.Instance().OnAfter(message, new CommonEventArgs(EventOperationType.Instance().Create()));

            return id > 0;
        }

        /// <summary>
        /// 删除私信
        /// </summary>
        /// <param name="messageId">私信Id</param>
        /// <param name="sessionId">私信会话Id</param>
        /// <returns>是否删除成功：true-成功，false-不成功</returns>
        public bool Delete(long messageId, long sessionId)
        {
            Message message = messageRepository.Get(messageId);
            if (message == null)
                return false;

            EventBus<Message>.Instance().OnBefore(message, new CommonEventArgs(EventOperationType.Instance().Delete()));
            int affectCount = messageRepository.Delete(message, sessionId);
            EventBus<Message>.Instance().OnAfter(message, new CommonEventArgs(EventOperationType.Instance().Delete()));

            return affectCount > 0;
        }

        /// <summary>
        /// 获取私信实体
        /// </summary>
        /// <param name="messageId">私信Id</param>
        public Message Get(long messageId)
        {
            return messageRepository.Get(messageId);
        }

        /// <summary>
        /// 设置私信为已读
        /// </summary>
        /// <param name="sessionId">私信会话Id</param>
        /// <param name="userId">会话拥有者Id</param>
        /// <returns>是否更新成功</returns>
        public bool SetIsRead(long sessionId, long userId)
        {
            bool isRead = messageRepository.SetIsRead(sessionId, userId);
            return isRead;
        }

        /// <summary>
        /// 获取用户前N个私信
        /// </summary>
        ///<param name="userId">UserId</param>
        ///<param name="sortBy">私信排序字段</param>
        ///<param name="topNumber">要获取的记录数</param>
        public IEnumerable<Message> GetTops(long userId, SortBy_Message? sortBy, int topNumber)
        {
            return messageRepository.GetTopMessagesOfUser(userId, sortBy, topNumber);
        }

        /// <summary>
        ///获取会话中前N条私信
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <param name="topNumber">要获取的记录数</param>
        public IEnumerable<Message> GetTops(long sessionId, int topNumber)
        {
            IEnumerable<object> sessionids = messageInSessionRepository.GetMessageIds(sessionId, topNumber);
            IEnumerable<Message> messages;
            if (sessionids.Count() > topNumber)
            {
                messages = messageRepository.PopulateEntitiesByEntityIds(sessionids.Take(topNumber));
            }
            else
            {
                messages = messageRepository.PopulateEntitiesByEntityIds(sessionids);
            }

            return messages;
        }

        /// <summary>
        ///获取会话中所有私信
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <param name="topNumber">获取记录条数</param>
        public IEnumerable<Message> GetMessagesOfSession(long sessionId, int topNumber)
        {
            IEnumerable<object> sessionids = messageInSessionRepository.GetMessageIds(sessionId, topNumber);
            return messageRepository.PopulateEntitiesByEntityIds(sessionids);
        }

        /// <summary>
        /// 获取未读私信数
        /// </summary>
        /// <param name="userId">私信拥有者UserId</param>
        /// <returns></returns>
        public int GetUnreadCount(long userId)
        {
            return messageRepository.GetUnReadCount(userId);
        }

        #endregion 私信

        #region 私信会话

        /// <summary>
        /// 获取私信实体
        /// </summary>
        /// <param name="messageId">私信Id</param>
        public MessageSession GetSession(long messageId)
        {
            return messageSessionRepository.Get(messageId);
        }

        /// <summary>
        /// 删除私信会话
        /// </summary>
        /// <param name="sessionId">私信会话Id</param>
        public bool DeleteSession(long sessionId)
        {
            return messageSessionRepository.DeleteByEntityId(sessionId) > 0;
        }

        /// <summary>
        /// 获取用户私信会话
        /// </summary>
        ///<param name="userId">用户Id</param>
        ///<param name="pageIndex">页码</param>
        public PagingDataSet<MessageSession> GetSessionsOfUser(long userId, int pageIndex)
        {
            return messageSessionRepository.GetSessionsOfUser(userId, pageIndex);
        }

        /// <summary>
        /// 获取私信会话分页数据（后台用）
        /// </summary>
        /// <param name="type">私信类型</param>
        /// <param name="userId">用户Id（用来搜索用户Id相关的私信会话）</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>私信会话分页数据</returns>
        public PagingDataSet<MessageSession> GetSessions(MessageType? type, long? userId, int pageIndex)
        {
            return messageSessionRepository.GetSessions(type, userId, pageIndex);
        }

        /// <summary>
        /// 清除用户的私信会话
        /// </summary>
        /// <param name="userId">用户Id</param>
        public void ClearSessionsFromUser(long userId)
        {
            messageSessionRepository.ClearSessionsFromUser(userId);
        }

        /// <summary>
        /// 获取前N个私信会话
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="topNumber">获取记录条数</param>
        /// <param name="hasUnread">是否仅获取未读会话</param>
        public IEnumerable<MessageSession> GetTopSessions(long userId, int topNumber, bool hasUnread = false)
        {
            return messageSessionRepository.GetTopSessions(userId, topNumber, hasUnread);
        }

        #endregion 私信会话

        #region 客服消息

        /// <summary>
        /// 客服消息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="userName">发件人</param>
        /// <param name="roleName">角色</param>
        /// <param name="minRank">最小等级</param>
        /// <param name="maxRank">最大等级</param>
        /// <returns></returns>
        public PagingDataSet<MessageSession> GetCustomerMessages(int pageIndex, int pageSize, string userName,string roleName,long minRank=0,long maxRank=0)
        {
            return messageSessionRepository.GetCustomerMessages(pageIndex,pageSize,userName,roleName,minRank,maxRank);
        }
        #endregion

        /// <summary>
        /// 获取所有未读的私信
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserReminderInfo> GetUserReminderInfos()
        {
            return messageRepository.GetUserReminderInfos();
        }
    }
}
