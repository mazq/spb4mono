//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Tunynet;
using PetaPoco;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 用户举报的数据访问层实现
    /// </summary>
    public class ImpeachReportRepository : Repository<ImpeachReportEntity>, IImpeachReportRepository
    {
        /// <summary>
        /// 获取分页的举报列表
        /// </summary>
        /// <param name="isDisposed">是否已处理</param>
        /// <param name="impeachReason">举报原因</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>举报分页集合</returns>
        public PagingDataSet<ImpeachReportEntity> GetsForAdmin(bool isDisposed, ImpeachReason? impeachReason, int pageSize = 20, int pageIndex = 1)
        {
            Sql sql = Sql.Builder.Select("*").From("spb_ImpeachReports");
            Sql whereSql = Sql.Builder.Where("Status=@0", isDisposed);
            if (impeachReason.HasValue)
            {
                whereSql.Where("Reason=@0", impeachReason.Value);
            }

            Sql orderBySql = Sql.Builder;
            if (!isDisposed)
            {
                orderBySql.OrderBy("ReportId desc");
            }
            else
            {
                orderBySql.OrderBy("LastModified desc");
            }
            sql.Append(whereSql).Append(orderBySql);
            return GetPagingEntities(pageSize, pageIndex, sql);
        }
    }
}
