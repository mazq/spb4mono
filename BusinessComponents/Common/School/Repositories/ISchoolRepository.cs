//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 地区访问的接口
    /// </summary>
    public interface ISchoolRepository : IRepository<School>
    {
        /// <summary>
        /// 交换学校排列顺序
        /// </summary>
        void ChangeDisplayOrder(long id, long referenceId);

        /// <summary>
        /// 获取学校
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="keyword"></param>
        /// <param name="schoolType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagingDataSet<School> Gets(string areaCode, string keyword, SchoolType? schoolType, int pageSize, int pageIndex);
    }
}
