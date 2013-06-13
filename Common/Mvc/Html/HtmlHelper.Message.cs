//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tunynet.Common;
using Tunynet.Utilities;

namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对Message的HtmlHelper使用方法
    /// </summary>
    public static class HtmlHelperMessageExtensions
    {
        /// <summary>
        /// 预置提示信息
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString PresetMessage(this HtmlHelper htmlHelper, string messageContent, bool isHighlight)
        {
            htmlHelper.ViewData["IsHighlight"] = isHighlight;
            htmlHelper.ViewData["MessageContent"] = messageContent;
            return htmlHelper.DisplayForModel("PresetMessage");
        }

        /// <summary>
        /// 操作结果提示信息
        /// </summary>
        /// <param name="statusMessageData">信息实体，参数为null时会自动检查ViewData和TempData是否有Key=“StatusMessageData”对应的值</param>
        /// <param name="hintMillisecondForHide">消息类型为提示信息时是否自动隐藏的秒数（小于零时不自动隐藏，默认不自动隐藏)</param>
        /// <param name="successMillisecondForHide">消息类型为成功时是否自动隐藏的秒数（小于零时不自动隐藏，默认为2秒)</param>
        public static MvcHtmlString StatusMessage(this HtmlHelper htmlHelper, StatusMessageData statusMessageData = null, int hintMillisecondForHide = -1, int successMillisecondForHide = 2)
        {
            if (statusMessageData == null)
            {
                if (htmlHelper.ViewData != null)
                {
                    statusMessageData = htmlHelper.ViewData.Get<StatusMessageData>("StatusMessageData", null);
                }

                if (statusMessageData == null && htmlHelper.ViewContext.TempData != null)
                {
                    statusMessageData = htmlHelper.ViewContext.TempData.Get<StatusMessageData>("StatusMessageData", null);
                }
            }
            if (statusMessageData == null)
                return MvcHtmlString.Empty;
            

            htmlHelper.ViewData["StatusMessageData"] = statusMessageData;
            htmlHelper.ViewData["HintMillisecondForHide"] = hintMillisecondForHide;
            htmlHelper.ViewData["SuccessMillisecondForHide"] = successMillisecondForHide;
            return htmlHelper.DisplayForModel("StatusMessage");
        }
    }
    /// <summary>
    /// 辅助传输StatusMessage数据
    /// </summary>
    [Serializable]
    public sealed class StatusMessageData
    {
        private StatusMessageType messageType;
        /// <summary>
        /// 提示消息类别
        /// </summary>
        public StatusMessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        private string messageContent = string.Empty;
        /// <summary>
        /// 信息内容
        /// </summary>
        public string MessageContent
        {
            get { return messageContent; }
            set { messageContent = value; }
        }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="messageContent">消息内容</param>
        public StatusMessageData(StatusMessageType messageType, string messageContent)
        {
            this.messageType = messageType;
            this.messageContent = messageContent;
        }
    }


    /// <summary>
    /// 提示消息类别
    /// </summary>
    public enum StatusMessageType
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 错误
        /// </summary>
        Error = -1,

        /// <summary>
        /// 提示信息
        /// </summary>
        Hint = 0
    }

}