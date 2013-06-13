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
using System.Web.Helpers;
using System.Linq;
using Spacebuilder.Common;
namespace Tunynet.Mvc
{
    /// <summary>
    /// 扩展对用户选择器的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperUserSelectorExtensions
    {

        /// <summary>
        /// 输出单用户选择器
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper</param>
        /// <param name="name">控件表单名</param>
        /// <param name="selectedUserId">初始选中用户Id</param>
        /// <param name="searchScope">搜索范围</param>
        /// <param name="selectionAddedCallBack">添加成功事件回调方法</param>
        /// <remarks>仅限登录用户使用</remarks>
        /// <returns></returns>
        public static MvcHtmlString UserSelector(this HtmlHelper htmlHelper, string name, long? selectedUserId = null, UserSelectorSearchScope searchScope = UserSelectorSearchScope.Site, string selectionAddedCallBack = null)
        {
            IEnumerable<long> selectedUserIds = null;
            if (selectedUserId != null)
                selectedUserIds = new List<long> { selectedUserId.Value };
            return htmlHelper.UserSelector(name, 1, selectedUserIds, SelectorWidthType.Medium, searchScope, false, selectionAddedCallBack);
        }

        /// <summary>
        /// 输出多用户选择器
        /// </summary>
        /// <param name="htmlHelper">被扩展的htmlHelper</param>
        /// <param name="name">控件表单名</param>
        /// <param name="selectionLimit">限制选中的用户数</param>
        /// <param name="selectedUserIds">初始选中用户Id集合</param>
        /// <param name="searchScope">搜索范围</param>
        /// <param name="showDropDownMenu">是否显示下拉菜单</param>
        /// <param name="widthType">宽度</param>
        /// <param name="selectionAddedCallBack">添加成功事件回调方法</param>
        /// <remarks>仅限登录用户使用</remarks>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString UserSelector(this HtmlHelper htmlHelper, string name, int selectionLimit, IEnumerable<long> selectedUserIds = null, SelectorWidthType widthType = SelectorWidthType.Long, UserSelectorSearchScope searchScope = UserSelectorSearchScope.FollowedUser, bool showDropDownMenu = true, string selectionAddedCallBack = null)
        {
            if (selectedUserIds == null)
                selectedUserIds = new List<long>();

            IUserService userService = DIContainer.Resolve<IUserService>();

            var selectedUsers = userService.GetFullUsers(selectedUserIds)
             .Select(n => new
            {
                userId = n.UserId,
                displayName = n.DisplayName,
                trueName = n.TrueName,
                nickName = n.NickName,
                userAvatarUrl = SiteUrls.Instance().UserAvatarUrl(n, AvatarSizeType.Small)
            });
            string followGroupsJSON = "[]";
            if (showDropDownMenu)
            {
                var followGroups = new CategoryService().GetOwnerCategories(UserContext.CurrentUser.UserId, TenantTypeIds.Instance().User()).Select(n => new
                {
                    name = n.CategoryName,
                    value = n.CategoryId
                });
                followGroupsJSON = Json.Encode(followGroups);
            }

            string widthClass = "tn-longer";
            switch (widthType)
            {
                case SelectorWidthType.Short:
                    widthClass = "tn-short";
                    break;
                case SelectorWidthType.Medium:
                    widthClass = "tn-medium";
                    break;
                case SelectorWidthType.Long:
                    widthClass = "tn-long";
                    break;
                case SelectorWidthType.Longer:
                    widthClass = "tn-longer";
                    break;
                case SelectorWidthType.Longest:
                    widthClass = "tn-longest";
                    break;
                default:
                    widthClass = "tn-longer";
                    break;
            }

            htmlHelper.ViewData["controlName"] = name;
            htmlHelper.ViewData["widthClass"] = widthClass;
            htmlHelper.ViewData["selectionLimit"] = selectionLimit;
            htmlHelper.ViewData["searchScope"] = searchScope;
            htmlHelper.ViewData["showDropDownMenu"] = showDropDownMenu;
            htmlHelper.ViewData["selectionAddedCallBack"] = selectionAddedCallBack;
            return htmlHelper.EditorForModel("UserSelector", new { selectedUsers = Json.Encode(selectedUsers), followGroups = followGroupsJSON });
        }
    }
    /// <summary>
    /// 用户选择器搜索范围
    /// </summary>
    public enum UserSelectorSearchScope
    {
        /// <summary>
        /// 从关注的用户中搜索
        /// </summary>
        FollowedUser = 0,
        /// <summary>
        /// 全站搜索
        /// </summary>
        Site = 1
    }

    /// <summary>
    /// 内容选择器宽度设置
    /// </summary>
    public enum SelectorWidthType
    {
        /// <summary>
        /// 短的
        /// </summary>
        Short = 0,
        /// <summary>
        /// 中等的
        /// </summary>
        Medium = 1,
        /// <summary>
        /// 长的
        /// </summary>
        Long = 2,
        /// <summary>
        /// 较长的
        /// </summary>
        Longer = 3,
        /// <summary>
        /// 最长的
        /// </summary>
        Longest = 4
    }

}