//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{

    /// <summary>
    /// 用户举报的数据访问层接口
    /// </summary>
    public interface IImpeachReportRepository : IRepository<ImpeachReportEntity>
    {

        /// <summary>
        /// 获取举报的分页集合
        /// </summary>
        /// <param name="isDisposed">是否已处理</param>
        /// <param name="impeachReason">举报原因</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>举报分页集合</returns>
        PagingDataSet<ImpeachReportEntity> GetsForAdmin(bool isDisposed, ImpeachReason? impeachReason, int pageSize = 20, int pageIndex = 1);

    }
}
