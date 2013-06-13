//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spacebuilder.Common;
using Tunynet.Common;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 微博评论的实体
    /// </summary>
    public class MicroblogCommentEditModel : CommentEditModel
    {
        /// <summary>
        /// 原作者名字
        /// </summary>
        public string OriginalAuthor { get; set; }

        /// <summary>
        /// 当前评论的id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否转发微博
        /// </summary>
        public bool ForwardMicrobo { get; set; }

        /// <summary>
        /// 是否评论原作者
        /// </summary>
        public bool CommentOriginalAuthor { get; set; }
    }

    /// <summary>
    /// 微博评论的扩展类
    /// </summary>
    public static class MicroblogCommentExtensions
    {
        /// <summary>
        /// 转换成MicroblogCommentEditModel
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>转换之后的EditModel</returns>
        public static MicroblogCommentEditModel AsMicroblogCommentEditModel(this Comment comment)
        {
            return new MicroblogCommentEditModel
            {
                CommentedObjectId = comment.CommentedObjectId,
                ParentId = comment.ParentId,
                IsPrivate = comment.IsPrivate,
                TenantTypeId = comment.TenantTypeId,
                OwnerId = comment.OwnerId,
                ToUserId = comment.ToUserId,
                Id = comment.Id
            };
        }
    }
}