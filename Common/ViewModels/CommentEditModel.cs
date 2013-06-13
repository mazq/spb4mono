//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.Utilities;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 评论编辑的实体
    /// </summary>
    public class CommentEditModel
    {
        /// <summary>
        /// 评论的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父评论Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "请输入回复")]
        [DataType(DataType.MultilineText)]
        [WaterMark(Content = "请输入回复")]
        public string Body { get; set; }

        /// <summary>
        /// 被评论对象Id
        /// </summary>
        public long CommentedObjectId { get; set; }

        /// <summary>
        /// 被回复UserId（一级ToUserId为0）
        /// </summary>
        public long ToUserId { get; set; }

        /// <summary>
        /// 被回复人名称（一级ToUserDisplayName为空字符串）
        /// </summary>
        public string ToUserDisplayName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        /// 是否是悄悄话
        /// </summary>
        [Display(Name = "悄悄话")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// 是否匿名评论
        /// </summary>
        [Display(Name = "匿名")]
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [WaterMark(Content = "姓名")]
        [Required(ErrorMessage = "请输入姓名")]
        public string Author { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [WaterMark(Content = "手机号码")]
        [Required(ErrorMessage = "请输入手机号码")]
        [RegularExpression(@"^0{0,1}(13[0-9]|14[0-9]|15[0-9]|18[0-9])[0-9]{8}$", ErrorMessage = "请输入正确的手机号")]
        public string Contact { get; set; }

        /// <summary>
        /// 数据是否经过验证
        /// </summary>
        public bool IsValidate
        {
            get
            {
                if (string.IsNullOrEmpty(Body))
                    return false;
                if (CommentedObjectId == 0)
                    return false;
                if (OwnerId == 0)
                    return false;
                if (string.IsNullOrEmpty(TenantTypeId))
                    return false;

                //默认用户并且，没有添加联系信息的用户是不被允许的
                if (UserContext.CurrentUser == null && (string.IsNullOrEmpty(this.Contact) || string.IsNullOrEmpty(this.Author)))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// 转换成数据库存储的评论对象
        /// </summary>
        /// <returns></returns>
        public Comment AsComment()
        {
            bool notLogin = UserContext.CurrentUser == null;

            Comment comment = Comment.New();
            comment.ParentId = this.ParentId;
            comment.CommentedObjectId = this.CommentedObjectId;
            comment.OwnerId = this.OwnerId;
            comment.TenantTypeId = this.TenantTypeId;
            comment.Subject = this.Subject ?? string.Empty;
            comment.Body = new EmotionService().EmoticonTransforms(this.Body);
            comment.IsPrivate = this.IsPrivate;
            comment.ChildCount = 0;
            comment.IsAnonymous = notLogin;
            comment.ToUserDisplayName = this.ToUserId <= 0 ? string.Empty : UserIdToUserNameDictionary.GetUserName(this.ToUserId);
            comment.ToUserId = this.ToUserId;
            comment.UserId = notLogin ? 0 : UserContext.CurrentUser.UserId;
            comment.AuditStatus = AuditStatus.Success;
            comment.Author = notLogin ? this.Author : UserContext.CurrentUser.DisplayName;
            comment.Contact = notLogin ? this.Contact : string.Empty;
            return comment;
        }
    }

    /// <summary>
    /// 评论的扩展类
    /// </summary>
    public static class CommentViewExtensions
    {
        /// <summary>
        /// 转换成EditModel
        /// </summary>
        /// <param name="comment">评论的实体</param>
        /// <returns>被转换后的实体</returns>
        public static CommentEditModel AsEditModel(this Comment comment)
        {
            if (comment == null)
                return null;
            return new CommentEditModel
            {
                Author = comment.Author,
                IsPrivate = comment.IsPrivate,
                Body = comment.Body,
                CommentedObjectId = comment.CommentedObjectId,
                Contact = comment.Contact,
                IsAnonymous = comment.IsAnonymous,
                OwnerId = comment.OwnerId,
                ParentId = comment.ParentId,
                Subject = comment.Subject,
                TenantTypeId = comment.TenantTypeId,
                ToUserDisplayName = comment.User() != null ? comment.User().DisplayName : string.Empty,
                ToUserId = comment.UserId,
                Id = comment.Id
            };
        }
    }
}