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
    /// 封装zTree控件
    /// </summary>
    public static class HtmlHelperTreeExtensions
    {
        /// <summary>
        /// 构建数节点
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlhelper实例</param>
        /// <param name="name"> 树选择器的名称</param>
        /// <param name="treeOptions">树的设置</param>
        /// <param name="treeNodes">树节点集合如果不填则通过ViewData传值</param>
        /// <exception cref="ArgumentNullException">ViewData为空时</exception>
        /// <returns></returns>
        public static MvcHtmlString Tree(this HtmlHelper htmlHelper, string name, TreeOptions treeOptions, IEnumerable<TreeNode> treeNodes = null)
        {
            if (treeNodes == null)
                if (htmlHelper.ViewData[name] != null)
                    treeNodes = htmlHelper.ViewData[name] as IEnumerable<TreeNode>;
            if (treeNodes == null)
                throw new ExceptionFacade("ViewData没有对name赋值");

            //定义属性字典
            Dictionary<string, object> result = new Dictionary<string, object>();
            //data属性字典
            Dictionary<string, object> data = new Dictionary<string, object>();
            //添加树节点数据
            data.TryAdd("TreeNodes", treeNodes.Select(n => n.ToUnobtrusiveHtmlAttributes()));
            //添加树属性数据
            data.TryAdd("Settings", treeOptions.ToUnobtrusiveHtmlAttributes());
            //将数据添加到集合中
            //建立标签元素
            TagBuilder builder = new TagBuilder("ul");

            //添加初始化数据树
            //添加用于脚本操作的标识
            result["plugin"] = "Tree";

            result.Add("data", Json.Encode(data));

            builder.MergeAttributes(result);
            builder.MergeAttribute("id", name);
            //样式
            builder.MergeAttribute("class", "ztree");
            return MvcHtmlString.Create(builder.ToString().Replace("&quot;|", "").Replace("|&quot;", ""));
        }
    }
}