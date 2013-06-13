//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// Notice扩展
    /// </summary>
    public static class NoticeExtensions
    {
        /// <summary>
        /// 获取主角链接地址
        /// </summary>
        /// <param name="notice">被扩展的notice</param>
        /// <returns></returns>
        public static string GetLeadingActorUrl(this Notice notice)
        {
            return SiteUrls.Instance().SpaceHome(notice.LeadingActorUserId);
        }
    }
}
