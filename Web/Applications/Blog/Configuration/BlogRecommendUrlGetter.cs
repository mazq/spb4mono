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
    /// 日志推荐Url获取器
    /// </summary>
    public class BlogRecommendUrlGetter : IRecommendUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().BlogThread(); }
        }

        /// <summary>
        /// 详细页面地址
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <returns></returns>
        public string RecommendItemDetail(long itemId)
        {
            BlogThread blogThread = new BlogService().Get(itemId);
            if (blogThread == null)
                return string.Empty;
            string userName = UserIdToUserNameDictionary.GetUserName(blogThread.UserId);
            return SiteUrls.Instance().BlogDetail(userName, itemId);
        }
    }
}