using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Bar
{
    public class BarCommentUrlGetter : ICommentUrlGetter
    {

        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().BarThread(); }
        }

        /// <summary>
        /// 获取被评论对象名称
        /// </summary>
        /// <param name="commentedObjectId">被评论对象Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public string GetCommentedObjectName(long commentedObjectId, string tenantTypeId)
        {
            if (tenantTypeId == TenantTypeIds.Instance().BarThread())
            {
                BarThread barThread = new BarThreadService().Get(commentedObjectId);
                if (barThread != null)
                {
                    return barThread.Subject;
                }
            }
            return string.Empty;
        }

        public string GetCommentDetailUrl(long commentedObjectId, long id, long? userId = null)
        {
            return null;
        }

        public string GetCommentedObjectUrl(long commentedObjectId, long? userId = null)
        {
            return null;
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
            if (tenantTypeId == TenantTypeIds.Instance().BarThread())
            {
                return SiteUrls.Instance().ThreadDetail(commentedObjectId);
            }
            return string.Empty;
        }


        public CommentedObject GetCommentedObject(long commentedObjectId)
        {
            BarThread barThread = new BarThreadService().Get(commentedObjectId);
            if (barThread != null)
            {
                CommentedObject commentedObject = new CommentedObject();
                commentedObject.DetailUrl = SiteUrls.Instance().ThreadDetail(commentedObjectId);
                commentedObject.Name = barThread.Subject;
                commentedObject.Author = barThread.Author;
                commentedObject.UserId = barThread.UserId;
                return commentedObject;
            }
            return null;
        }
    }
}