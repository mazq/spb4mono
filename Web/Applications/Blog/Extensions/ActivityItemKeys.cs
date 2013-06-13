//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Blog
{

    /// <summary>
    /// 日志动态项
    /// </summary>
    public static class ActivityItemKeysExtension
    {
        /// <summary>
        /// 发布日志
        /// </summary>
        public static string CreateBlogThread(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBlogThread";
        }

        /// <summary>
        /// 日志评论
        /// </summary>
        public static string CreateBlogComment(this ActivityItemKeys activityItemKeys)
        {
            return "CreateBlogComment";
        }
    }

}
