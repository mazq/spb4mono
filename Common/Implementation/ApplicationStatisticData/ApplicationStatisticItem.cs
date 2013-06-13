//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.DirectoryServices;
using Microsoft.Win32;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 应用统计数据
    /// </summary>
    public class ApplicationStatisticItem
    {
        public ApplicationStatisticItem()
        {

        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="shortName">简称</param>
        /// <param name="totalCount">总数</param>
        /// <param name="last24HCount">24小时新增数</param>
        public ApplicationStatisticItem(string shortName, long totalCount, long last24HCount)
        {
            this.ShortName = shortName;
            this.TotalCount = totalCount;
            this.Last24HCount = last24HCount;
        }

        /// <summary>
        /// 统计项简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 最近24小时新增数
        /// </summary>
        public long Last24HCount { get; set; }


        /// <summary>
        /// 统计项链接
        /// </summary>
        public string Url { get; set; }

        

    }
}