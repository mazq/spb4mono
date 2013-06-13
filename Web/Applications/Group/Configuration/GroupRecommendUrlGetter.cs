//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 照片推荐Url获取器
    /// </summary>
    public class GroupRecommendUrlGetter : IRecommendUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().Group(); }
        }

        /// <summary>
        /// 详细页面地址
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <returns></returns>
        public string RecommendItemDetail(long itemId)
        {
            GroupEntity group = new GroupService().Get(itemId);
            if (group == null)
                return string.Empty;
            string userName = UserIdToUserNameDictionary.GetUserName(group.UserId);
            return SiteUrls.Instance().GroupHome(itemId);
        }
    }
}