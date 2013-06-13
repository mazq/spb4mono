//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 贴吧推荐Url获取器
    /// </summary>
    public class BarSectionRecommendUrlGetter : IRecommendUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().BarSection(); }
        }

        /// <summary>
        /// 贴吧详细页面地址
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <returns></returns>
        public string RecommendItemDetail(long itemId)
        {
            BarSection barSection = new BarSectionService().Get(itemId);
            if (barSection == null)
                return string.Empty;
            return SiteUrls.Instance().SectionDetail(itemId);
        }
    }
}