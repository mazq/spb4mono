//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 输出审核图标的方法
    /// </summary>
    public static class HtmlHelperAuditStatusIconExtensions
    {
        /// <summary>
        /// 输出审核图标的方法
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="auditStatus">审核状态</param>
        /// <returns>审核图标</returns>
        public static MvcHtmlString AuditStatusIcon(this HtmlHelper htmlHelper, AuditStatus auditStatus)
        {
            TagBuilder span = new TagBuilder("span");
            switch (auditStatus)
            {
                case AuditStatus.Again:
                    span.MergeAttribute("class", "tn-icon-colorful tn-icon-colorful-again tn-icon-inline");
                    span.MergeAttribute("title", "需再次审核");
                    break;
                case AuditStatus.Fail:
                    span.MergeAttribute("class", "tn-icon-colorful tn-icon-colorful-stop tn-icon-inline");
                    span.MergeAttribute("title", "未通过");
                    break;
                case AuditStatus.Pending:
                    span.MergeAttribute("class", "tn-icon-colorful tn-icon-colorful-wait tn-icon-inline");
                    span.MergeAttribute("title", "待审核");
                    break;
                default:
                    span.MergeAttribute("class", "tn-icon-colorful tn-icon-colorful-pass tn-icon-inline");
                    span.MergeAttribute("title", "已通过");
                    break;
            }

            return new MvcHtmlString("&nbsp;" + span.ToString());
        }
    }
}
