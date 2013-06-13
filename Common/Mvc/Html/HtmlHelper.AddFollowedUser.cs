//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 添加关注控件
    /// </summary>
    public static class HtmlHelperAddFollowedUserExtensions
    {
        /// <summary>
        /// 添加关注控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="followedUserId">被关注用户Id</param>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="followedButtonType">已关注按钮样式</param>
        /// <param name="followButtonType">加关注按钮样式</param>
        /// <returns></returns>
        public static MvcHtmlString FollowUser(this HtmlHelper htmlHelper, long followedUserId, string buttonName = "关注", FollowedButtonTypes followedButtonType = FollowedButtonTypes.Default, FollowButtonTypes followButtonType = FollowButtonTypes.Button)
        {
            htmlHelper.ViewData["followedUserId"] = followedUserId;
            htmlHelper.ViewData["followedButtonType"] = followedButtonType;
            htmlHelper.ViewData["followButtonType"] = followButtonType;
            htmlHelper.ViewData["buttonName"] = buttonName;

            return htmlHelper.DisplayForModel("FollowUser");
        }
    }

    /// <summary>
    /// 已关注按钮样式
    /// </summary>
    public enum FollowedButtonTypes
    {
        /// <summary>
        /// 默认样式
        /// </summary>
        Default=0,

        /// <summary>
        /// 按钮模式，带边框
        /// </summary>
        Button=1
    }

    /// <summary>
    /// 加关注按钮样式
    /// </summary>
    public enum FollowButtonTypes
    {
        /// <summary>
        /// 链接
        /// </summary>
        Link=0,

        /// <summary>
        /// 按钮
        /// </summary>
        Button=1
    }
}
