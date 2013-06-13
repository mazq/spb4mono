//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 应用统计数据获取器
    /// </summary>
    public interface IApplicationStatisticDataGetter
    {
        /// <summary>
        /// 获取管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        IEnumerable<ApplicationStatisticData> GetManageableDatas(string tenantTypeId = null);

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        IEnumerable<ApplicationStatisticData> GetStatisticDatas(string tenantTypeId = null);
    }
}