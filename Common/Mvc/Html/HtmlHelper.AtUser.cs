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
using Tunynet.Common;
using Spacebuilder.Common;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Link的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperAtUserExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="textareaId">编辑框的id</param>
        /// <param name="seletorId">出发按钮的id</param>
        /// <returns></returns>
        public static MvcHtmlString AtUser(this HtmlHelper htmlHelper,string textareaId,string seletorId=null)
        {

            htmlHelper.ViewData["textareaId"] = textareaId;
            htmlHelper.ViewData["seletorId"] = seletorId;
            return htmlHelper.DisplayForModel("AtUser");

        }
    }
}
