//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 应用统计数据
    /// </summary>
    public class ApplicationStatisticData
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="dataKey">统计项标识</param>
        /// <param name="shortName"></param>
        /// <param name="name">统计项名称</param>
        /// <param name="value">统计项数值</param>
        public ApplicationStatisticData(string dataKey, string shortName, string name, long value)
        {
            this.DataKey = dataKey;
            this.ShortName = shortName;
            this.Name = name;
            this.Value = value;
        }
        /// <summary>
        /// 统计项标识
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// 统计项简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 统计项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述模式（例如：有{0}个日志待审核）
        /// </summary>
        public string DescriptionPattern { get; set; }

        /// <summary>
        /// 统计项数值
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// 统计项链接
        /// </summary>
        public string Url { get; set; }
    }
}