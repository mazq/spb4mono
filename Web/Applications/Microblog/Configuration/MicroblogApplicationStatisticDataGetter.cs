//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Mvc;
using System.Web.Routing;
using Spacebuilder.Common;
using Tunynet.Utilities;
using Tunynet.Common;

namespace Spacebuilder.Microblog
{
    /// <summary>
    /// 帖吧应用数据Url获取器
    /// </summary>
    public class MicroblogApplicationStatisticDataGetter : IApplicationStatisticDataGetter
    {
        private MicroblogService MicroblogService = new MicroblogService();
        /// <summary>
        /// 获取管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public IEnumerable<ApplicationStatisticData> GetManageableDatas(string tenantTypeId = null)
        {
            IList<ApplicationStatisticData> applicationStatisticDatas = new List<ApplicationStatisticData>();
            Dictionary<string, long> microBlogManageableDatas = MicroblogService.GetManageableDatas(tenantTypeId);
            if (microBlogManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().MicroblogPendingCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().MicroblogPendingCount(), "微博",
                 "微博待审核数", microBlogManageableDatas[ApplicationStatisticDataKeys.Instance().MicroblogPendingCount()])
                {
                    DescriptionPattern = "{0}个微博待审核",
                    Url = SiteUrls.Instance().ManageMicroblogs(AuditStatus.Pending, tenantTypeId: tenantTypeId)
                });
            if (microBlogManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().MicroblogAgainCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().MicroblogAgainCount(), "微博",
                 "微博需再审核数", microBlogManageableDatas[ApplicationStatisticDataKeys.Instance().MicroblogAgainCount()])
                {
                    DescriptionPattern = "{0}个微博需再审核",
                    Url = SiteUrls.Instance().ManageMicroblogs(AuditStatus.Again, tenantTypeId: tenantTypeId)
                });
            return applicationStatisticDatas;
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public IEnumerable<ApplicationStatisticData> GetStatisticDatas(string tenantTypeId = null)
        {
            IList<ApplicationStatisticData> applicationStatisticDatas = new List<ApplicationStatisticData>();
            Dictionary<string, long> microBlogStatisticDatas = MicroblogService.GetStatisticDatas(tenantTypeId);
            if (microBlogStatisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().TotalCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().TotalCount(), "微博",
                 "微博总数", microBlogStatisticDatas[ApplicationStatisticDataKeys.Instance().TotalCount()])
                {
                    DescriptionPattern = "共{0}个微博",
                    Url = SiteUrls.Instance().ManageMicroblogs(tenantTypeId: tenantTypeId)
                });
            if (microBlogStatisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().Last24HCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().Last24HCount(), "微博",
                 "微博24小时新增数", microBlogStatisticDatas[ApplicationStatisticDataKeys.Instance().Last24HCount()])
                {
                    DescriptionPattern = "24小时新增{0}个微博",
                    Url = SiteUrls.Instance().ManageMicroblogs(tenantTypeId: tenantTypeId)
                });
            return applicationStatisticDatas;
        }
    }
}