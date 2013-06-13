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
using System.Resources;

namespace Tunynet.UI
{
    /// <summary>
    /// 设置Controller的通用TitlePart
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TitleFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// TitlePart
        /// </summary>        
        public string TitlePart { get; set; }

        /// <summary>
        /// TitlePart的资源名称
        /// </summary>
        public string TitlePartResourceName { get; set; }

        /// <summary>
        /// 是否在Title输出SiteName
        /// </summary>
        public bool IsAppendSiteName { get; set; }

        /// <summary>
        /// 重写OnResultExecuting
        /// </summary>
        /// <param name="filterContext">ResultExecutingContext</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {            
            if (filterContext.IsChildAction)
                return;

            IPageResourceManager pageResourceManager = DIContainer.ResolvePerHttpRequest<IPageResourceManager>();
            pageResourceManager.IsAppendSiteName = IsAppendSiteName;

            if (string.IsNullOrEmpty(TitlePart) && !string.IsNullOrEmpty(TitlePartResourceName))                
                TitlePart = Tunynet.Globalization.ResourceAccessor.GetString(TitlePartResourceName);
            
            if (!string.IsNullOrEmpty(TitlePart))
                pageResourceManager.AppendTitleParts(TitlePart);
        }
    }
}
