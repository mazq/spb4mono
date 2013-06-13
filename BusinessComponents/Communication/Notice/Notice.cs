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
{
    /// <summary>
    /// 通知实体
    /// </summary>
    [TableName("tn_Notices")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class Notice : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Notice New()
        {
            Notice notice = new Notice()
            {
                TemplateName = string.Empty,
                LeadingActor = string.Empty,
                RelativeObjectName = string.Empty,
                RelativeObjectUrl = string.Empty,
                Body = string.Empty,
                DateCreated = DateTime.UtcNow
            };
            return notice;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///应用Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///通知类型Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        ///通知接收人
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///通知模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        ///主角UserID
        /// </summary>
        public long LeadingActorUserId { get; set; }

        /// <summary>
        ///主角
        /// </summary>
        public string LeadingActor { get; set; }

        /// <summary>
        ///相关项对象名称
        /// </summary>
        public string RelativeObjectName { get; set; }

        /// <summary>
        ///相关项对象链接地址
        /// </summary>
        public string RelativeObjectUrl { get; set; }

        /// <summary>
        ///相关项对象Id
        /// </summary>
        public long RelativeObjectId { get; set; }

        /// <summary>
        ///内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///处理状态  0= Unhandled:未处理;1= Handled 已处理
        /// </summary>
        public NoticeStatus Status { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #endregion

        /// <summary>
        /// 主角的空间主页地址
        /// </summary>
        [Ignore]
        public string LeadingActorUrl
        {
            get { return GetExtendedProperty<string>("LeadingActorUrl"); }
            set { SetExtendedProperty("LeadingActorUrl", value); }
        }

        /// <summary>
        /// 所属对象名称（例如：日志）
        /// </summary>
        [Ignore]
        public string Owner
        {
            get { return GetExtendedProperty<string>("OwnerName"); }
            set { SetExtendedProperty("OwnerName", value); }
        }

        /// <summary>
        /// 获取解析过的内容
        /// </summary>
        [Ignore]
        public string ResolvedBody
        {
            get
            {
                if (!string.IsNullOrEmpty(Body))
                    return Body;
                return NoticeBuilder.Instance().Resolve(this);
            }
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
