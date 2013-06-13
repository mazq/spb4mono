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

namespace Tunynet.UI
{
    /// <summary>
    /// 用于controller的Theme相关属性标注
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ThemedAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        public ThemedAttribute(string presentAreaKey)
        {
            this.PresentAreaKey = presentAreaKey;
        }

        /// <summary>
        /// 呈现区域标识
        /// </summary>        
        public string PresentAreaKey { get; private set; }

        /// <summary>
        /// 是否属于应用模块
        /// </summary>
        public bool IsApplication { get; set; }


        private PresentAreaService presentAreaService = null;
        /// <summary>
        /// PresentAreaService
        /// </summary>
        protected virtual PresentAreaService PresentAreaService
        {
            get
            {
                if (presentAreaService == null)
                    presentAreaService = new PresentAreaService();

                return presentAreaService;
            }
            set { presentAreaService = value; }
        }

        #region IResultFilter

        /// <summary>
        /// 加载皮肤css文件及根据需要重设站点logo
        /// </summary>
        /// <param name="filterContext"><see cref="ResultExecutingContext"/></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            //过滤异步请求
            if (filterContext.HttpContext.Request.Headers.Get("X-Requested-With") != null)
                return;

            ThemeService.IncludeCss(PresentAreaKey, filterContext.RequestContext);
        }

        #endregion

    }
}
