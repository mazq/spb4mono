//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 评论相关Url获取器
    /// </summary>
    public interface ICommentUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        string TenantTypeId { get; }

        /// <summary>
        /// 获取评论详细显示url
        /// </summary>
        /// <remarks>如果无评论详细显示页面，则定位到评论列表的精确位置（需要考虑分页及性能，仅在真正需要浏览时才进行必要的计算）</remarks>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="id">评论Id</param>
        /// <returns></returns>
        string GetCommentDetailUrl(long commentedObjectId, long id, long? userId = null);


        /// <summary>
        /// 获取被评论的对象实体
        /// </summary>
        /// <param name="commentedObjectId"></param>
        /// <returns></returns>
        CommentedObject GetCommentedObject(long commentedObjectId);

    }


    public class CommentedObject
    {
        public string DetailUrl { get; set; }

        public string Name { get; set; }

        public long UserId { get; set; }

        public string Author { get; set; }
    }
}
