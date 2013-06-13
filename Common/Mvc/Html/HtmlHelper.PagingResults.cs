//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Collections;
using System.Web.Helpers;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperPagingResultsExtensions
    {

        /// <summary>
        /// 生成分页统计结果
        /// </summary>
        /// <param name="htmlHelper">被扩展的HtmlHelper实例</param>
        /// <param name="pagingDataSet">分页集合</param>
        /// <returns></returns>
        public static MvcHtmlString PagingResults(this HtmlHelper htmlHelper, IPagingDataSet pagingDataSet)
        {
            TagBuilder builder = new TagBuilder("span");
            builder.AddCssClass("tn-page-results tn-text-note");
            long startCount = (pagingDataSet.PageIndex - 1) * pagingDataSet.PageSize + 1;
            long endCount = pagingDataSet.PageIndex * pagingDataSet.PageSize;
            if (endCount > pagingDataSet.TotalRecords)
                endCount = pagingDataSet.TotalRecords;
            builder.SetInnerText(string.Format("{0}-{1} 共{2}", startCount, endCount, pagingDataSet.TotalRecords));

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}