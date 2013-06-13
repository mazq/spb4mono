//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 积分项
    /// </summary>
    public static class PointItemKeysExtension
    {
        /// <summary>
        /// 创建微博
        /// </summary>
        public static string Blog_CreateThread(this PointItemKeys pointItemKeys)
        {
            return "Blog_CreateThread";
        }

        /// <summary>
        /// 删除微博
        /// </summary>
        public static string Blog_DeleteThread(this PointItemKeys pointItemKeys)
        {
            return "Blog_DeleteThread";
        }
    }
}
