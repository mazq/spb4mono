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

namespace Tunynet.Mvc
{
    /// <summary>
    /// Tree状态属性设置
    /// </summary>
    public class TreeOptions
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns></returns>
        public static TreeOptions New()
        {
            TreeOptions treeOptions = new TreeOptions()
            {
                IsSinglePath = false,
                SelectBoxType = null,
                IsDrag = false,
                RadioBoxType = null,
                RootId = null,
                IsRemove = false,
                IsRename = false,
                AsyncUrl = string.Empty,
                AdditionalSetOptions = null
            };
            return treeOptions;
        }

        #region 设置根节点值

        /// <summary>
        /// 设置指定根节点的值
        /// </summary>
        public string RootId { get; set; }

        #endregion 设置根节点值

        #region 是否单一路径

        /// <summary>
        /// 是否实现单一路径功能 默认为false
        /// </summary>
        public bool IsSinglePath { get; set; }

        #endregion 是否单一路径

        #region check设置

        /// <summary>
        /// 单/复选类型 默认值 null
        /// </summary>
        public SelectBoxTypes? SelectBoxType { get; set; }

        /// <summary>
        /// 单选勾选框影响的范围分组设置
        /// 如果不启用单选，则默认值为null，若启用单选则默认值为level
        /// (可选择为level 在每一级节点范围当做一个分组，  all 整棵树作为一个分组)
        /// </summary>
        public RadioBoxTypes? RadioBoxType { get; set; }

        #endregion check设置

        #region edit相关设置

        /// <summary>
        /// 可删除功能开关  默认false
        /// </summary>
        public bool IsRemove { get; set; }

        /// <summary>
        /// 可改名功能开关默认为false
        /// </summary>
        public bool IsRename { get; set; }

        /// <summary>
        /// 可拖拽状态开关 默认false
        /// </summary>
        public bool IsDrag { get; set; }

        #endregion edit相关设置

        #region 异步加载设置

        /// <summary>
        /// 异步请求URL
        /// </summary>
        public string AsyncUrl { get; set; }

        #endregion 异步加载设置

        /// <summary>
        ///Tree的额外设置
        /// </summary>
        public Dictionary<string, object> AdditionalSetOptions { get; set; }

        /// <summary>
        /// Tree的额外回调事件
        /// </summary>
        public Dictionary<string, object> AdditionalCallBacks { get; set; }

        #region 连缀方法

        /// <summary>
        /// 指定根节点ID的值
        /// </summary>
        /// <param name="rootIdValue"> 指定设置根节点的值</param>
        /// <returns></returns>
        public TreeOptions SetRootIdValue(string rootIdValue)
        {
            this.RootId = rootIdValue;
            return this;
        }

        /// <summary>
        /// 设置是否开启单一路径功能
        /// </summary>
        /// <param name="isSinglePath">值</param>
        public TreeOptions SinglePath(bool isSinglePath)
        {
            this.IsSinglePath = isSinglePath;
            return this;
        }

        /// <summary>
        /// 设置选项单/复选类型
        /// </summary>
        /// <param name="selectBoxType">勾选框类型(Checkbox 或 Radio）默认checkbox</param>
        public TreeOptions SetSelectBoxType(SelectBoxTypes selectBoxType)
        {
            this.SelectBoxType = selectBoxType;
            return this;
        }

        /// <summary>
        /// Radio 单选框的分组范围
        /// </summary>
        /// <param name="radioboxtype">Level"在每一级节点范围内当做一个分组。All"在整棵树范围内当做一个分组。默认"Level" </param>
        public TreeOptions SetRadioType(RadioBoxTypes radioboxtype)
        {
            this.RadioBoxType = radioboxtype;
            return this;
        }

        /// <summary>
        /// 设置是否可删除状态
        /// </summary>
        /// <param name="isRemove">设置值默认为false</param>
        public TreeOptions SetIsRemove(bool isRemove)
        {
            this.IsRemove = isRemove;
            return this;
        }

        /// <summary>
        /// 设置是否可改名状态
        /// </summary>
        /// <param name="isRename"></param>
        /// <returns></returns>
        public TreeOptions SetIsRename(bool isRename)
        {
            this.IsRename = isRename;
            return this;
        }

        /// <summary>
        ///设置是否为可拖拽状态
        /// </summary>
        /// <param name="isDrag">设置值默认为false</param>
        public TreeOptions SetIsDrag(bool isDrag)
        {
            this.IsDrag = isDrag;
            return this;
        }

        /// <summary>
        /// 设置异步加载请求地址URL
        /// </summary>
        /// <param name="asyncurl"> URL</param>
        public TreeOptions SetAsyncUrl(string asyncurl)
        {
            this.AsyncUrl = asyncurl;
            return this;
        }

        /// <summary>
        /// 添加Tree设置
        /// </summary>
        /// <param name="attributeName">设置名称</param>
        /// <param name="attributeValue">设置值</param>
        /// <returns></returns>
        public TreeOptions MergeTreeSetOption(string attributeName, object attributeValue)
        {
            if (this.AdditionalSetOptions == null)
                this.AdditionalSetOptions = new Dictionary<string, object>();
            this.AdditionalSetOptions[attributeName] = attributeValue;
            return this;
        }

        /// <summary>
        /// 添加额外的Tree的回调函数
        /// </summary>
        /// <param name="callbackname">回调函数名</param>
        /// <param name="functionname">function函数名称</param>
        public TreeOptions MergeTreeCallBack(string callbackname, object functionname)
        {
            if (this.AdditionalCallBacks == null)
                this.AdditionalCallBacks = new Dictionary<string, object>();
            this.AdditionalCallBacks[callbackname] = "|" + functionname + "|";
            return this;
        }

        #endregion 连缀方法

        /// <summary>
        /// Tree属性集合
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> ToUnobtrusiveHtmlAttributes()
        {
            var result = new Dictionary<string, object>();
            //先获取额外的Tree设置
            if (AdditionalSetOptions != null)
                result = new Dictionary<string, object>(AdditionalSetOptions);
            //数据设置
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, object> simpleData = new Dictionary<string, object>();
            //设置默认支持事件
            Dictionary<string, object> callBack = new Dictionary<string, object>();

            #region 设置编辑属性

            //判断是否开启可编辑状态

            bool isRename = false, isRemove = false, enable = false, isCopy = false, isMove = false;
            Dictionary<string, object> edit = new Dictionary<string, object>();
            Dictionary<string, object> drag = new Dictionary<string, object>();
            //首先判断是否开启  拖拽功能
            if (this.IsDrag)
            {
                //编辑设置开启 然后设置关闭 删除 改名  （默认）功能
                enable = true;
                isCopy = true;
                isMove = true;
            }
            //添加drag设置
            drag.TryAdd("isCopy", isCopy);
            drag.TryAdd("isMove", isMove);
            //判断是否开启可改名
            if (this.IsRename)
            {
                //先判断是否已经设定
                if (!enable)
                    enable = true;
                isRename = true;
            }
            //判断是否开启删除
            if (this.IsRemove)
            {
                if (!enable)
                    enable = true;
                isRemove = true;
            }
            //判断是否被开启编辑功能
            if (enable)
            {
                edit.TryAdd("enable", enable);
                //判断是否开启拖拽
                edit.TryAdd("drag", drag);
                edit.TryAdd("showRemoveBtn", isRemove);
                edit.TryAdd("showRenameBtn", isRename);
                result.TryAdd("edit", edit);
            }

            #endregion 设置编辑属性

            #region 设置数据属性

            //设置启用简单数据
            simpleData.TryAdd("enable", true);
            //修正根节点表示值根节点表示
            simpleData.TryAdd("rootPId", RootId);
            //设置简单数据
            data.TryAdd("simpleData", simpleData);
            //合并数据
            result.TryAdd("data", data);

            #endregion 设置数据属性

            #region 设置勾选框

            //判断是否开启勾选框
            if (this.SelectBoxType != null)
            {
                Dictionary<string, object> check = new Dictionary<string, object>();
                //设置勾选框开启
                check.TryAdd("enable", true);
                if (this.SelectBoxType == SelectBoxTypes.Checkbox)
                {
                    //设置勾选框类型
                    check.TryAdd("chkStyle", this.SelectBoxType.ToString().ToLower());
                }
                else
                {
                    //设置勾选类型
                    check.TryAdd("chkStyle", this.SelectBoxType.ToString().ToLower());
                    if (this.RadioBoxType != null)
                    {
                        //设置单选按钮范围
                        check.TryAdd("radioType", this.RadioBoxType.ToString().ToLower());
                    }
                    else
                    {
                        //设置单选默认按钮范围 Level
                        check.TryAdd("radioType", RadioBoxTypes.Level.ToString().ToLower());
                    }
                }

                //合并勾选框
                result.TryAdd("check", check);
            }

            #endregion 设置勾选框

            #region 设置回调函数

            //默认的回调事件添加
            //展开前的回调函数
            //判断是否开启单一路径功能
            if (this.AdditionalCallBacks != null)
            {
                callBack = this.AdditionalCallBacks;
                if (callBack.ContainsKey("onExpand"))
                {
                    callBack["addOnExpand"] = callBack["onExpand"];
                }
                if (callBack.ContainsKey("beforeExpand"))
                {
                    callBack["addBeforeExpand"] = callBack["beforeExpand"];
                }
            }
            if (IsSinglePath)
            {
                callBack["beforeExpand"] = "|" + "beforeExpand" + "|";
                //被展开时的回调函数
                callBack["onExpand"] = "|" + "onExpand" + "|";
            }
            //合并默认回调函数
            if (callBack != null)
                result.TryAdd("callback", callBack);

            #endregion 设置回调函数

            #region 设置异步加载

            //判断是否启用异步加载
            if (!string.IsNullOrEmpty(this.AsyncUrl))
            {
                //异步加载设置
                Dictionary<string, object> async = new Dictionary<string, object>();
                //设置异步开启
                async.TryAdd("enable", true);
                //设置异步提交参数数据类型
                async.TryAdd("contentType", "application/json");
                //设置自动提交父节点属性的参数 参数为parentId
                string[] autoParam = new string[] { "id=parentId" };
                async.TryAdd("autoParam", autoParam);
                //设置获取数据的Url地址
                async.TryAdd("url", this.AsyncUrl);
                //合并异步加载
                result.TryAdd("async", async);
            }

            #endregion 设置异步加载

            //整合设置

            return result;
        }
    }

    /// <summary>
    /// 勾选框类型
    /// </summary>
    public enum SelectBoxTypes
    {
        /// <summary>
        /// 启用复选框
        /// </summary>
        Checkbox = 1,

        /// <summary>
        ///启用单选框
        /// </summary>
        Radio = 2,
    }

    /// <summary>
    /// 单选框的范围
    /// </summary>
    public enum RadioBoxTypes
    {
        /// <summary>
        ///同层级范围
        /// </summary>
        Level = 1,

        /// <summary>
        /// 全局范围
        /// </summary>
        All = 2
    }

    /// <summary>
    /// target属性
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        ///在新窗口打开
        /// </summary>
        Blank = 1,

        /// <summary>
        ///在当前窗口打开
        /// </summary>
        Self = 2,

        /// <summary>
        ///在父窗口打开
        /// </summary>
        Parent = 3,

        /// <summary>
        ///在顶部窗口打开
        /// </summary>
        Top = 4
    }
}