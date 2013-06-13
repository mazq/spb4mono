//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{

    /// <summary>
    /// 私信类别
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 普通消息 用于会员之间的通信
        /// </summary>
        Common,
        /// <summary>
        /// 系统消息
        /// </summary>
        System,

        /// <summary>
        /// 客服消息
        /// </summary>
        CustomerService,

        /// <summary>
        /// 咨询求助
        /// </summary>
        Recourse,

        /// <summary>
        /// 投诉消息
        /// </summary>
        Complain,

        /// <summary>
        /// 意见建议
        /// </summary>
        Advice
    }


    /// <summary>
    /// 系统内置的私信发送UserId
    /// </summary>
    public enum BuildinMessageUserId
    {
        /// <summary>
        /// 作为系统消息的 发送人UserId
        /// </summary>
        System = -101,

        /// <summary>
        /// 作为客服消息的 发送人UserId 及 咨询求助、投诉消息、意见建议、Bug报告的 接收人UserId 
        /// </summary>
        CustomerService = -102,
    }

    /// <summary>
    /// 私信排序字段
    /// </summary>
    public enum SortBy_Message
    {

        /// <summary>
        /// 是否已读
        /// </summary>
        IsRead,

        /// <summary>
        /// 最新私信
        /// </summary>
        DateCreated_Desc
    }
}
