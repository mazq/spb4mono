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
using Tunynet;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 实体的实体类
    /// </summary>
    [TableName("tn_Invitations")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId,SenderUserId")]
    [Serializable]
    public class Invitation : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Invitation New()
        {
            Invitation invitation = new Invitation()
            {
                InvitationTypeKey = string.Empty,
                Sender = string.Empty,
                RelativeObjectName = string.Empty,
                RelativeObjectUrl = string.Empty,
                DateCreated = DateTime.UtcNow

            };
            return invitation;
        }

        #region 需持久化属性

        /// <summary>
        ///id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///应用id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///请求类型key
        /// </summary>
        public string InvitationTypeKey { get; set; }

        /// <summary>
        ///请求接收人用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///请求发送人用户id
        /// </summary>
        public long SenderUserId { get; set; }

        /// <summary>
        ///请求发送人
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        ///相关项对象名称
        /// </summary>
        public string RelativeObjectName { get; set; }

        /// <summary>
        ///相关项对象id
        /// </summary>
        public long RelativeObjectId { get; set; }

        /// <summary>
        ///相关项对象链接地址
        /// </summary>
        public string RelativeObjectUrl { get; set; }

        /// <summary>
        ///请求状态  0= Unhandled:未处理；1= Accept接受；2=Refuse 拒绝；
        /// </summary>
        public InvitationStatus Status { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #endregion

        /// <summary>
        /// 附注
        /// </summary>
        [Ignore]
        public string Remark
        {
            get { return GetExtendedProperty<string>("Remark"); }
            set { SetExtendedProperty("Remark", value); }
        }

        /// <summary>
        /// 请求发送人的空间主页地址
        /// </summary>
        [Ignore]
        public string SenderUrl
        {
            get { return GetExtendedProperty<string>("SenderUrl"); }
            set { SetExtendedProperty("SenderUrl", value); }
        }

        /// <summary>
        /// 获取解析过的请求内容
        /// </summary>
        /// <param name="status">请求处理状态</param>
        /// <returns>解析过的请求内容</returns>
        public string GetResolvedBody()
        {            
            return InvitationBuilder.Instance().Resolve(this, Status);
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
