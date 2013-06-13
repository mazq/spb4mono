//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 树节点的实体类
    /// </summary>
    public class TreeNode
    {
        /// <summary>
        ///  用户创建新实体时使用
        /// </summary>
        /// <returns></returns>
        public static TreeNode New()
        {
            TreeNode treenode = new TreeNode()
            {
                Target = null,
                IconName = string.Empty,
                Url = string.Empty
            };
            return treenode;
        }

        /// <summary>
        /// 标识Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 父节点标识
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标的Css选择器名称
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        /// Url 链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// target属性值
        /// </summary>
        public TargetType? Target { get; set; }

        /// <summary>
        /// 被选中属性 默认为false
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 是否展开状态 默认false
        /// </summary>
        public bool IsOpened { get; set; }

        /// <summary>
        /// 是否为父节点主要设置用于异步加载
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// 节点的额外属性
        /// </summary>
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        #region 连缀方法

        /// <summary>
        /// 设置树节点是否为父节点
        /// </summary>
        /// <param name="isparent">值</param>
        public TreeNode SetParent(bool isparent)
        {
            this.IsParent = isparent;
            return this;
        }

        /// <summary>
        /// 设置树节点的的id
        /// </summary>
        /// <param name="id">id值</param>
        public TreeNode SetId(string id)
        {
            this.Id = id;
            return this;
        }

        /// <summary>
        /// 设置节点的父标识
        /// </summary>
        /// <param name="parentid">父节点的id</param>
        public TreeNode SetParentId(string parentid)
        {
            this.ParentId = parentid;
            return this;
        }

        /// <summary>
        /// 设置节点的名称
        /// </summary>
        /// <param name="name">节点名称</param>
        public TreeNode SetName(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// 设置图标CSS选择器的名称
        /// </summary>
        /// <param name="iconname">图标选择器名称</param>
        public TreeNode SetIconName(string iconname)
        {
            this.IconName = iconname;
            return this;
        }

        /// <summary>
        ///  设置URL
        /// </summary>
        /// <param name="url">URL</param>
        public TreeNode SetUrl(string url)
        {
            this.Url = url;
            return this;
        }

        /// <summary>
        /// 设置target
        /// </summary>
        /// <param name="target">target值</param>
        public TreeNode SetTarget(TargetType target)
        {
            this.Target = target;
            return this;
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="ischecked">选中状态值</param>
        public TreeNode SetChecked(bool ischecked)
        {
            this.IsChecked = ischecked;
            return this;
        }

        /// <summary>
        /// 设置是否展开
        /// </summary>
        /// <param name="isopened">是否展开值</param>
        public TreeNode Open(bool isopened)
        {
            this.IsOpened = isopened;
            return this;
        }

        /// <summary>
        /// 添加节点属性
        /// </summary>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">属性值</param>
        /// <returns></returns>
        public TreeNode MergeNodeAttribute(string attributeName, object attributeValue)
        {
            if (this.AdditionalAttributes == null)
                this.AdditionalAttributes = new Dictionary<string, object>();
            this.AdditionalAttributes[attributeName] = attributeValue;
            return this;
        }

        #endregion 连缀方法

        /// <summary>
        /// 获取集合
        /// </summary>
        public Dictionary<string, object> ToUnobtrusiveHtmlAttributes()
        {
            var result = new Dictionary<string, object>();

            if (AdditionalAttributes != null)
                result = new Dictionary<string, object>(AdditionalAttributes);

            result.TryAdd("id", this.Id);
            result.TryAdd("isParent", this.IsParent);
            result.TryAdd("pId", this.ParentId);
            result.TryAdd("name", this.Name);
            result.TryAdd("iconSkin", this.IconName);
            result.TryAdd("url", this.Url);
            if (this.Target != null)
                result.TryAdd("target", "_" + this.Target.ToString().ToLower());
            result.TryAdd("checked", this.IsChecked);
            result.TryAdd("open", this.IsOpened);
            return result;
        }
    }
}