//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;
using System;
using Tunynet.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// At用户关联项Url获取
    /// </summary>
    public class CommentAtUserAssociatedUrlGetter : IAtUserAssociatedUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().Comment(); }
        }

        public AssociatedInfo GetAssociatedInfo(long associateId, string tenantTypeId = "")
        {

            Comment comment = new CommentService().Get(associateId);
            if (comment != null && comment.User() != null)
            {
                ICommentUrlGetter commentUrlGetter = CommentUrlGetterFactory.Get(comment.TenantTypeId);
                CommentedObject commentObject = commentUrlGetter.GetCommentedObject(comment.CommentedObjectId);
                return new AssociatedInfo()
                {
                    DetailUrl = commentUrlGetter.GetCommentDetailUrl(comment.CommentedObjectId, comment.Id,commentObject.UserId),
                    Subject = Tunynet.Utilities.HtmlUtility.TrimHtml(comment.Body, 12)
                };
            }

            return null;
        }


        public string GetOwner()
        {
            return "评论";
        }
    }
}