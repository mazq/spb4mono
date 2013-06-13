//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 日志Url获取
    /// </summary>
    public class BlogCommentUrlGetter : ICommentUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().BlogThread(); }
        }

        /// <summary>
        /// 获取评论详细显示url
        /// </summary>
        /// <remarks>如果无评论详细显示页面，则定位到评论列表的精确位置（需要考虑分页及性能，仅在真正需要浏览时才进行必要的计算）</remarks>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="id">评论Id</param>
        /// <param name="userId">被评论对象作者Id</param>
        /// <returns></returns>
        public string GetCommentDetailUrl(long commentedObjectId, long id, long? userId = null)
        {
            if (!userId.HasValue || userId <= 0) return string.Empty;

            string userName = UserIdToUserNameDictionary.GetUserName(userId.Value);
            return SiteUrls.Instance().BlogDetail(userName, commentedObjectId, id);
        }

        /// <summary>
        /// 获取被评论对象url
        /// </summary>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="userId">被评论对象作者Id</param>
        /// <returns></returns>
        public string GetCommentedObjectUrl(long commentedObjectId, long? userId = null)
        {
            if (!userId.HasValue || userId <= 0) return string.Empty;

            string userName = UserIdToUserNameDictionary.GetUserName(userId.Value);
            return SiteUrls.Instance().BlogDetail(userName, commentedObjectId);
        }

        /// <summary>
        /// 获取被评论对象名称
        /// </summary>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCommentedObjectName(long commentedObjectId, string tenantTypeId)
        {
            if (tenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                BlogThread blog = new BlogService().Get(commentedObjectId);
                if (blog != null)
                {
                    return blog.Subject;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取被评论对象url
        /// </summary>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="userId">被评论对象作者Id</param>
        /// <returns></returns>
        public string GetCommentedObjectUrl(long commentedObjectId, long? userId = null, string tenantTypeId = null)
        {
            if (!userId.HasValue || userId <= 0) return string.Empty;
            if (tenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                string userName = UserIdToUserNameDictionary.GetUserName(userId.Value);
                return SiteUrls.Instance().BlogDetail(userName, commentedObjectId);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取被评论对象(部分)
        /// </summary>
        /// <param name="commentedObjectId"></param>
        /// <returns></returns>
        public CommentedObject GetCommentedObject(long commentedObjectId)
        {
            BlogThread blogThread = new BlogService().Get(commentedObjectId);
            if (blogThread != null)
            {
                CommentedObject commentedObject = new CommentedObject();
                commentedObject.DetailUrl = SiteUrls.Instance().BlogDetail(blogThread.User.UserName, commentedObjectId);
                commentedObject.Name = blogThread.Subject;
                commentedObject.Author = blogThread.Author;
                commentedObject.UserId = blogThread.UserId;
                return commentedObject;
            }
            return null;
        }
    }
}