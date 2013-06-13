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

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 帖吧应用数据Url获取器
    /// </summary>
    public class BarApplicationStatisticDataGetter : IApplicationStatisticDataGetter
    {
        private BarSectionService barSectionService = new BarSectionService();
        private BarThreadService barThreadService = new BarThreadService();
        private BarPostService barPostService = new BarPostService();

        /// <summary>
        /// 获取管理数据
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id（可以获取该应用下针对某种租户类型的统计计数，默认不进行筛选）</param>
        /// <returns></returns>
        public IEnumerable<ApplicationStatisticData> GetManageableDatas(string tenantTypeId = null)
        {
            tenantTypeId = tenantTypeId ?? TenantTypeIds.Instance().Bar();
            IList<ApplicationStatisticData> applicationStatisticDatas = new List<ApplicationStatisticData>();
            Dictionary<string, long> barSectionManageableDatas = barSectionService.GetManageableDatas(tenantTypeId);
            Dictionary<string, long> barThreadManageableDatas = barThreadService.GetManageableDatas(tenantTypeId);
            Dictionary<string, long> barPostManageableDatas = barPostService.GetManageableDatas(tenantTypeId);
            if (tenantTypeId == TenantTypeIds.Instance().Bar())
            {
                if (barSectionManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().SectionPendingCount()))
                    applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().SectionPendingCount(), "帖吧",
                     "帖吧待审核数", barSectionManageableDatas[ApplicationStatisticDataKeys.Instance().SectionPendingCount()])
                    {
                        DescriptionPattern = "{0}个帖吧待审核",
                        Url = SiteUrls.Instance().ManageBars(auditStatus: AuditStatus.Pending)
                    });
                if (barSectionManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().SectionAgainCount()))
                    applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().SectionAgainCount(), "帖吧",
                     "帖吧需再审核数", barSectionManageableDatas[ApplicationStatisticDataKeys.Instance().SectionAgainCount()])
                    {
                        DescriptionPattern = "{0}个帖吧需再审核",
                        Url = SiteUrls.Instance().ManageBars(auditStatus: AuditStatus.Again)
                    });
            }
            if (barThreadManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().PendingCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().PendingCount(), "帖子",
                 "帖子待审核数", barThreadManageableDatas[ApplicationStatisticDataKeys.Instance().PendingCount()])
                {
                    DescriptionPattern = "{0}个帖子待审核",
                    Url = SiteUrls.Instance().ManageThreads(AuditStatus.Pending, tenantTypeId)
                });
            if (barThreadManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().AgainCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().AgainCount(), "帖子",
                 "帖子需再审核数", barThreadManageableDatas[ApplicationStatisticDataKeys.Instance().AgainCount()])
                {
                    DescriptionPattern = "{0}个帖子需再审核",
                    Url = SiteUrls.Instance().ManageThreads(AuditStatus.Again, tenantTypeId)
                });

            if (barPostManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().PostPendingCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().PostPendingCount(), "回帖",
                 "回帖待审核数", barPostManageableDatas[ApplicationStatisticDataKeys.Instance().PostPendingCount()])
                {
                    DescriptionPattern = "{0}个回帖待审核",
                    Url = SiteUrls.Instance().ManagePosts(AuditStatus.Pending, tenantTypeId)
                });
            if (barPostManageableDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().PostAgainCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().PostAgainCount(), "回帖",
                 "回帖需再审核数", barPostManageableDatas[ApplicationStatisticDataKeys.Instance().PostAgainCount()])
                {
                    DescriptionPattern = "{0}个回帖需再审核",
                    Url = SiteUrls.Instance().ManagePosts(AuditStatus.Again, tenantTypeId)
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
            Dictionary<string, long> barThreadStatisticDatas = barThreadService.GetStatisticDatas(tenantTypeId);
            if (barThreadStatisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().TotalCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().TotalCount(), "帖子",
                 "帖子总数", barThreadStatisticDatas[ApplicationStatisticDataKeys.Instance().TotalCount()])
                {
                    DescriptionPattern = "共{0}个帖子",
                    Url = SiteUrls.Instance().ManageThreads(tenantTypeId: tenantTypeId)
                });
            if (barThreadStatisticDatas.ContainsKey(ApplicationStatisticDataKeys.Instance().Last24HCount()))
                applicationStatisticDatas.Add(new ApplicationStatisticData(ApplicationStatisticDataKeys.Instance().Last24HCount(), "帖子",
                 "帖子24小时新增数", barThreadStatisticDatas[ApplicationStatisticDataKeys.Instance().Last24HCount()])
                {
                    DescriptionPattern = "24小时新增{0}个帖子",
                    Url = SiteUrls.Instance().ManageThreads(tenantTypeId: tenantTypeId)
                });
            return applicationStatisticDatas;
        }
    }
}