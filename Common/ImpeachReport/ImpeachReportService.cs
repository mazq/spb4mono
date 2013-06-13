//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Spacebuilder.Common;
using Tunynet;

namespace Spacebuilder.Common
{
    public class ImpeachReportService
    {
        private IImpeachReportRepository impeachReportRepository;

        #region 构造器

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImpeachReportService()
            : this(new ImpeachReportRepository())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="impeachReportRepository">数据访问层实例</param>
        public ImpeachReportService(IImpeachReportRepository impeachReportRepository)
        {
            this.impeachReportRepository = impeachReportRepository;
        }



        #endregion

        #region 处理方法

        /// <summary>
        /// 创建举报
        /// </summary>
        /// <param name="report">要创建的举报实体</param>
        /// <returns></returns>
        public bool Create(ImpeachReportEntity report)
        {
            impeachReportRepository.Insert(report);
            return report.ReportId > 0;
        }

        /// <summary>
        /// 标记为已处理
        /// </summary>
        /// <param name="reportId">要处理的举报Id</param>
        /// <param name="disposerId">处理人Id</param>
        /// <returns></returns>
        public void Dispose(long reportId, long disposerId)
        {
            ImpeachReportEntity report = impeachReportRepository.Get(reportId);
            if (report == null)
            {
                return;
            }
            report.Status = true;
            report.DisposerId = disposerId;
            report.LastModified = DateTime.UtcNow;
            impeachReportRepository.Update(report);

        }

        /// <summary>
        /// 删除举报
        /// </summary>
        /// <param name="reportId">举报Id集合</param>
        public void Delete(long reportId)
        {
            impeachReportRepository.DeleteByEntityId(reportId);
        }

        /// <summary>
        /// 获取举报分页列表
        /// </summary>
        /// <param name="isDisposed">是否已处理</param>
        /// <param name="reason">举报原因</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>举报分页集合</returns>
        public PagingDataSet<ImpeachReportEntity> GetsForAdmin(bool isDisposed, ImpeachReason? reason, int pageSize = 20, int pageIndex = 1)
        {
            return impeachReportRepository.GetsForAdmin(isDisposed, reason, pageSize, pageIndex);
        }

        /// <summary>
        /// 获取单个举报实体
        /// </summary>
        /// <param name="reportId">举报Id</param>
        /// <returns></returns>
        public ImpeachReportEntity GetReport(long reportId)
        {
            return impeachReportRepository.Get(reportId);
        }

        #endregion

    }
}



