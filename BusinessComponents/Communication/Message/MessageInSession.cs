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
{   /// <summary>
    /// 私信和会话的关系
    /// </summary>
    [TableName("tn_MessagesInSessions")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "SessionId")]
    [Serializable]
    public class MessageInSession : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static MessageInSession New()
        {
            MessageInSession messagesInSession = new MessageInSession()
            {
            };
            return messagesInSession;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///会话Id
        /// </summary>
        public long SessionId { get; set; }

        /// <summary>
        ///私信Id
        /// </summary>
        public long MessageId { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}