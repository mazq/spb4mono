using System;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用于表单呈现的MessageSession实体
    /// </summary>
    public class MessageSessionEditModel
    {
        /// <summary>
        ///SessionId
        /// </summary>
        public long SessionId { get; set; }

        /// <summary>
        ///会话拥有者UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///会话参与人UserId
        /// </summary>
        public long OtherUserId { get; set; }

        /// <summary>
        ///会话中最新的私信MessageId
        /// </summary>
        public long LastMessageId { get; set; }

        /// <summary>
        ///信息数统计
        /// </summary>
        public int MessageCount { get; set; }

        /// <summary>
        ///未读信息数统计（用来显示未读私信统计数和和标示会话的阅读状态）
        /// </summary>
        public int UnreadMessageCount { get; set; }

        /// <summary>
        ///消息类型
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        ///最后回复日期
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// 转换为MessageSession用于数据库存储
        /// </summary>
        public MessageSession AsMessageSession()
        {
            MessageSession messageSession;
            if (SessionId > 0)
            {
                MessageService messageService = new MessageService();
                messageSession = messageService.GetSession(SessionId);
                messageSession.LastMessageId = this.LastMessageId;
                messageSession.MessageCount = this.MessageCount;
                messageSession.MessageType = this.MessageType;
                messageSession.OtherUserId = this.OtherUserId;
                messageSession.UnreadMessageCount = this.UnreadMessageCount;
                messageSession.UserId = this.UserId;
            }
            else
            {
                messageSession = MessageSession.New();
                messageSession.LastMessageId = this.LastMessageId;
                messageSession.MessageCount = this.MessageCount;
                messageSession.MessageType = this.MessageType;
                messageSession.OtherUserId = this.OtherUserId;
                messageSession.UnreadMessageCount = this.UnreadMessageCount;
                messageSession.UserId = this.UserId;
            }
            return messageSession;
        }
    }

    /// <summary>
    /// MessageSession扩展
    /// </summary>
    public static class MessageSessionExtensions
    {
        /// <summary>
        /// 转换成MessageSessionEditModel
        /// </summary>
        /// <param name="messageSession"></param>
        /// <returns></returns>
        public static MessageSessionEditModel AsEditModel(this MessageSession messageSession)
        {
            return new MessageSessionEditModel
            {
                LastMessageId = messageSession.LastMessageId,
                LastModified = messageSession.LastModified,
                MessageCount = messageSession.MessageCount,
                MessageType = messageSession.MessageType,
                OtherUserId = messageSession.OtherUserId,
                SessionId = messageSession.SessionId,
                UnreadMessageCount = messageSession.UnreadMessageCount,
                UserId = messageSession.UserId
            };
        }
    }
}