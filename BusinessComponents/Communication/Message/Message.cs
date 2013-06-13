//<TunynetCopyright>
//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-04-17</createdate>
//<author>libsh</author>
//<email>libsh@tunynet.com</email>
//<log date="2012-04-17" version="0.5">创建</log>
//--------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Utilities;

namespace Tunynet.Common
{  /// <summary>
    /// 私信实体
    /// </summary>
    [TableName("tn_Messages")]
    [PrimaryKey("MessageId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "ReceiverUserId")]
    [Serializable]
    public class Message : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Message New()
        {
            Message message = new Message()
            {
                Sender = string.Empty,
                Receiver = string.Empty,
                Subject = string.Empty,
                Body = string.Empty,
                IP = WebUtility.GetIP(),
                IsRead = false,
                MessageType = Common.MessageType.Common,
                DateCreated = DateTime.UtcNow
            };
            return message;
        }

        #region 需持久化属性

        /// <summary>
        ///MessageId
        /// </summary>
        public long MessageId { get; protected set; }

        /// <summary>
        ///发件人UserId
        /// </summary>
        public long SenderUserId { get; set; }

        /// <summary>
        ///发件人的DisplayName
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        ///收件人UserId
        /// </summary>
        public long ReceiverUserId { get; set; }

        /// <summary>
        ///收件人DisplayName
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        ///私信标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///私信内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        ///私信来源IP
        /// </summary>
        public string IP { get; protected set; }

        /// <summary>
        ///发布日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #endregion 需持久化属性

        /// <summary>
        /// 私信类型
        /// </summary>
        [Ignore]
        public MessageType MessageType { get; set; }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.MessageId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}