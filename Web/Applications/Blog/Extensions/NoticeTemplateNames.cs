//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 扩展NoticeTemplateNames
    /// </summary>
    public static class NoticeTemplateNamesExtension
    {
        
        /// <summary>
        /// 日志有新评论
        /// </summary>
        public static string NewBlogComment(this NoticeTemplateNames noticeTemplateNames)
        {
            return "NewBlogComment";
        }

    }
}