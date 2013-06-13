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
    /// 输出隐私图标的方法
    /// </summary>
    public static class HtmlHelperPrivacyStatusIconExtensions
    {
        /// <summary>
        /// 输出审核图标的方法
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="auditStatus">审核状态</param>
        /// <returns>审核图标</returns>
        public static MvcHtmlString PrivacyStatusIcon(this HtmlHelper htmlHelper, PrivacyStatus privacyStatus)
        {
            TagBuilder span = new TagBuilder("span");
            switch (privacyStatus)
            {
                case PrivacyStatus.Part:
                    span.MergeAttribute("class", "tn-icon tn-icon-limit tn-icon-inline");
                    span.MergeAttribute("title", "部分人可见");
                    break;
                case PrivacyStatus.Private:
                    span.MergeAttribute("class", "tn-icon-colorful tn-icon-colorful-secret tn-icon-inline");
                    span.MergeAttribute("title", "仅自己可见");
                    break;
                default:
                    break;
            }

            return new MvcHtmlString(span.ToString());
        }
    }
}
