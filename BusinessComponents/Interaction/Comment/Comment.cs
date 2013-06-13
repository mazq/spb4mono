//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    [TableName("tn_Comments")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "CommentedObjectId,UserId,OwnerId,ParentId")]
    [Serializable]
    public class Comment : SerializablePropertiesBase, IEntity, IAuditable
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Comment New()
        {
            Comment comment = new Comment()
            {
                Author = string.Empty,
                ToUserDisplayName = string.Empty,
                Subject = string.Empty,
                IP = WebUtility.GetIP(),
                DateCreated = DateTime.UtcNow
            };
            return comment;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///父评论Id（一级ParentId等于Id）
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        ///被评论对象Id
        /// </summary>
        public long CommentedObjectId { get; set; }

        /// <summary>
        ///租户类型Id（4位ApplicationId+2位顺序号）
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///评论人UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///评论人名称
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///被回复UserId（一级ToUserId为0）
        /// </summary>
        public long ToUserId { get; set; }

        /// <summary>
        ///被回复人名称（一级ToUserDisplayName为空字符串）
        /// </summary>
        public string ToUserDisplayName { get; set; }

        /// <summary>
        ///标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///评论内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///是否属于悄悄话
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        ///审核状态
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///子级评论数量
        /// </summary>
        public int ChildCount { get; set; }

        /// <summary>
        ///是否匿名评论
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        ///评论人IP
        /// </summary>
        public string IP { get; protected set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [Ignore]
        public string Contact
        {
            get { return GetExtendedProperty<string>("Contact", string.Empty); }
            set { SetExtendedProperty("Contact", value); }
        }

        #endregion

        /// <summary>
        /// 审核项标识
        /// </summary>
        [Ignore]
        public string AuditItemKey
        {
            get
            {
                return AuditItemKeys.Instance().Comment();
            }
        }

        /// <summary>
        /// 获取评论的详细显示Url
        /// </summary>
        /// <returns></returns>
        public string GetCommentDetailUrl()
        {
            ICommentUrlGetter urlGetter = CommentUrlGetterFactory.Get(this.TenantTypeId);
            if (urlGetter != null)
                return urlGetter.GetCommentDetailUrl(this.CommentedObjectId, this.Id, OwnerId);
            else
                return string.Empty;
        }

        /// <summary>
        /// 获取被评论对象的Url
        /// </summary>
        /// <returns></returns>
        public string GetCommentedObjectUrl()
        {
            ICommentUrlGetter urlGetter = CommentUrlGetterFactory.Get(this.TenantTypeId);
            if (urlGetter != null)
                //return urlGetter.GetCommentedObjectUrl(this.CommentedObjectId, OwnerId);
                return urlGetter.GetCommentedObject(this.CommentedObjectId).DetailUrl;
            else
                return string.Empty;
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}