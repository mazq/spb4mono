using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 推荐中获取连接的接口
    /// </summary>
    public interface IRecommendUrlGetter
    {
        /// <summary>
        /// 租户类型id
        /// </summary>
        string TenantTypeId { get; }

        /// <summary>
        /// 详细页面地址
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        string RecommendItemDetail(long itemId);
    }
}
