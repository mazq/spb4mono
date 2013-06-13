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
using System.Web.Routing;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Html编辑器的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperHtmlEditorExtensions
    {
        /// <summary>
        /// 输出Html编辑器
        /// </summary> 
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="name">编辑器name属性</param>
        /// <param name="value">编辑器内容</param>
        /// <param name="options">异步提交表单选项</param>
        /// <returns>MvcForm</returns>
        public static MvcHtmlString HtmlEditor(this HtmlHelper htmlHelper, string name, string value = null, HtmlEditorOptions options = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("参数名称name不能为空", "name");
            }
            options = options ?? new HtmlEditorOptions(HtmlEditorMode.Enhanced);
            TagBuilder builder = new TagBuilder("span");
            builder.AddCssClass("mceEditor defaultSkin");
            builder.InnerHtml = htmlHelper.TextArea(name, value ?? string.Empty, options.ToUnobtrusiveHtmlAttributes()).ToString();
            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// 利用ViewModel输出Html编辑器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper">被扩展的htmlHelper实例</param>
        /// <param name="expression">获取ViewModel中的对应的属性</param>
        /// <param name="options">编辑器设置选项</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString HtmlEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, HtmlEditorOptions options = null)
        {
            options = options ?? new HtmlEditorOptions(HtmlEditorMode.Enhanced);
            TagBuilder builder = new TagBuilder("span");
            builder.AddCssClass("mceEditor defaultSkin");
            builder.InnerHtml = htmlHelper.TextAreaFor(expression, options.ToUnobtrusiveHtmlAttributes()).ToString();
            return MvcHtmlString.Create(builder.ToString());
        }
    }

    /// <summary>
    /// HtmlEditor显示方式
    /// </summary>
    public enum HtmlEditorMode
    {
        /// <summary>
        /// 简单模式
        /// </summary>
        Simple,

        /// <summary>
        /// 标准模式
        /// </summary>
        Standard,

        /// <summary>
        /// 高级模式
        /// </summary>
        Enhanced
    }


}
