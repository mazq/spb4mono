//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
namespace Tunynet.Mvc
{
    /// <summary>
    /// 异步删除按钮类
    /// </summary>
    public class AjaxDeleteButton
    {
        /// <summary>
        ///删除触发的url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 按钮文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 按钮图标
        /// </summary>
        public IconTypes? Icon { get; set; }

        /// <summary>
        /// 按钮提示信息
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// 确认删除时提示信息
        /// </summary>
        public string Confirm { get; set; }

        /// <summary>
        /// 目标删除元素选择器
        /// </summary>
        public string DeleteTarget { get; set; }

        /// <summary>
        /// 错误回调函数名
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 成功回调函数名
        /// </summary>
        public string Success { get; set; }

        /// <summary>
        /// 删除按钮html属性集合
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }

        #region 连缀方法

        /// <summary>
        /// 设置错误回调函数名
        /// </summary>
        /// <param name="error">方法名</param>
        /// <returns></returns>
        public AjaxDeleteButton SetOnErrorCallBack(string error)
        {
            this.Error = error;
            return this;
        }

        /// <summary>
        /// 设置成功回调函数名
        /// </summary>
        /// <param name="success">方法名</param>
        public AjaxDeleteButton SetOnSuccessCallBack(string success)
        {
            this.Success = success;
            return this;
        }

        /// <summary>
        /// 设置URL
        /// </summary>
        /// <param name="url">URL值</param>
        /// <returns>删除按钮URL</returns>
        public AjaxDeleteButton SetUrl(string url)
        {
            this.Url = url;
            return this;
        }

        /// <summary>
        /// 设置删除按钮显示文本
        /// </summary>
        /// <param name="text">显示文本</param>
        /// <returns>删除按钮显示的文本</returns>
        public AjaxDeleteButton SetText(string text)
        {
            this.Text = text;
            return this;
        }

        /// <summary>
        /// 设置删除按钮图标
        /// </summary>
        /// <param name="icon">按钮图标</param>
        /// <returns>删除按钮显示的图标</returns>
        public AjaxDeleteButton SetIcon(IconTypes icon)
        {
            this.Icon = icon;
            return this;
        }

        /// <summary>
        /// 设置按钮提示信息
        /// </summary>
        /// <param name="tooltip">提示信息</param>
        /// <returns>删除按钮的提示信息</returns>
        public AjaxDeleteButton SetTooltip(string tooltip)
        {
            this.Tooltip = tooltip;
            return this;
        }

        /// <summary>
        /// 添加删除按钮html属性
        /// </summary>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">属性值</param>
        /// <returns>如果存在，则覆盖</returns>
        public AjaxDeleteButton MergeHtmlAttribute(string attributeName, object attributeValue)
        {
            if (this.HtmlAttributes == null)
                this.HtmlAttributes = new Dictionary<string, object>();
            this.HtmlAttributes[attributeName] = attributeValue;
            return this;
        }

        /// <summary>
        /// 设置删除按钮删除提示信息
        /// </summary>
        /// <param name="confirm">删除提示信息</param>
        /// <returns>删除按钮的提示信息</returns>
        public AjaxDeleteButton SetConfirm(string confirm)
        {
            this.Confirm = confirm;
            return this;
        }

        /// <summary>
        /// 设置目标元素信息
        /// </summary>
        /// <param name="deletetarget">目标元素信息</param>
        /// <returns>删除按钮的上层元素信息</returns>
        public AjaxDeleteButton SetDeleteTarget(string deletetarget)
        {
            this.DeleteTarget = deletetarget;
            return this;
        }

        #endregion 连缀方法
    }
}