//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using Tunynet.Utilities;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展异步删除按钮
    /// </summary>
    public static class HtmlHelperAjaxDeleteButtonExtensions
    {
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="htmlHelper">被扩展的hemlHelper实例</param>
        /// <param name="ajaxdeletebutton">异步执行删除按钮</param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString AjaxDeleteButton(this HtmlHelper htmlHelper, AjaxDeleteButton ajaxdeletebutton)
        {
            //定义属性字典
            Dictionary<string, object> result = new Dictionary<string, object>();
            //建立标签元素
            TagBuilder builder = new TagBuilder("a");
            //判断html属性字典是否有数据
            //如果存在则导入到属性字典内
            if (ajaxdeletebutton.HtmlAttributes != null)

                result = new Dictionary<string, object>(ajaxdeletebutton.HtmlAttributes);
            //定义data属性字典
            Dictionary<string, object> data = new Dictionary<string, object>();

            //把错误提示信息添加到data字典里
            data.TryAdd("confirm", ajaxdeletebutton.Confirm);

            //将目标元素属性信息添加到data字典里
            data.TryAdd("deleteTarget", ajaxdeletebutton.DeleteTarget);

            //将成功回调函数名称添加data字典（如果存在）
            if (!string.IsNullOrEmpty(ajaxdeletebutton.Success))
            {
                data.TryAdd("SuccessFn", ajaxdeletebutton.Success);
            }
            //将失败回调函数名称添加data字典(如果存在)
            if (!string.IsNullOrEmpty(ajaxdeletebutton.Error))
            {
                data.TryAdd("ErrorFn", ajaxdeletebutton.Error);
            }

            //添加用于ajax操作的标识
            result["plugin"] = "AjaxDeleteButton";

            //添加data 属性
            result["data"] = Json.Encode(data);

            //将属性字典result导入到标签内属性内
            builder.MergeAttributes(result);

            //添加标签的href属性
            builder.MergeAttribute("href", ajaxdeletebutton.Url);

            //判断删除控件按钮文字是否有值
            if (!string.IsNullOrEmpty(ajaxdeletebutton.Text))
                builder.InnerHtml = ajaxdeletebutton.Text;
            else

                //判断删除控件按钮图标是否有值
                if (ajaxdeletebutton.Icon != null)

                    builder.InnerHtml = htmlHelper.Icon(ajaxdeletebutton.Icon.Value).ToString();
                else
                    return MvcHtmlString.Empty;

            //判断删除控件按钮提示信息是否有值
            if (!string.IsNullOrEmpty(ajaxdeletebutton.Tooltip))
            {
                builder.MergeAttribute("title", ajaxdeletebutton.Tooltip);
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}