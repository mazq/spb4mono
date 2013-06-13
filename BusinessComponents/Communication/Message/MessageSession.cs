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

namespace Tunynet.Common
{  /// <summary>
    /// 私信的会话
    /// </summary>
    [TableName("tn_MessageSessions")]
    [PrimaryKey("SessionId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class MessageSession : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static MessageSession New()
        {
            return new MessageSession();
        }

        #region 需持久化属性

        /// <summary>
        ///SessionId
        /// </summary>
        public long SessionId { get; protected set; }

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

        #endregion 需持久化属性

        #region 扩展属性
        /// <summary>
        /// 私信会话的最后一条Message
        /// </summary>
        public Message LastMessage
        {
            get
            {
                return new MessageService().Get(this.LastMessageId);
            }
        }
        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.SessionId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}
