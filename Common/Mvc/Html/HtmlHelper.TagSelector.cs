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
    /// 扩展对标签选择器的HtmlHelper输出方法
    /// </summary>
    public static class HtmlHelperTagSelectorExtensions
    {

        /// <summary>
        /// 输出标签选择器（用于可根据内容项直接获取到选中标签的情况）
        /// </summary>
        /// <remarks>仅限登录用户使用</remarks>
        /// <param name="htmlHelper"></param>
        /// <param name="itemId">内容项Id</param>
        /// <param name="ownerId">标签拥有者Id（可优先从该拥有者的标签中获取，并提供下拉列表选择）</param>
        /// <param name="tagInputName"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="topNumber">默认下拉列表中最多显示多少条数据</param>
        /// <returns></returns>
        public static MvcHtmlString TagSelector(this HtmlHelper htmlHelper, string tagInputName, string tenantTypeId, long? itemId, long? ownerId = null, int topNumber = 50)
        {
            IEnumerable<string> selectedTagNames = new List<string>();
            TagService tagService = new TagService(tenantTypeId);
            if (itemId != null && itemId.Value > 0)
            {
                IEnumerable<ItemInTag> itemInTags = tagService.GetItemInTagsOfItem(itemId.Value);
                selectedTagNames = itemInTags.Select(n => n.TagName);
            }
            return htmlHelper.TagSelector(tagInputName, selectedTagNames, tenantTypeId, ownerId, topNumber);
        }

        /// <summary>
        /// 输出标签选择器（用于不能直接根据内容项获取标签的情况，例如：设置相关标签）
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="tagInputName">表单项名称</param>
        /// <param name="selectedTagNames">选中的标签组</param>
        /// <param name="tenantTypeId">标签租户类型Id</param>
        /// <param name="ownerId">标签拥有者Id（可优先从该拥有者的标签中获取，并提供下拉列表选择）</param>
        /// <param name="topNumber">默认下拉列表中最多显示多少条数据</param>
        /// <returns></returns>
        public static MvcHtmlString TagSelector(this HtmlHelper htmlHelper, string tagInputName, IEnumerable<string> selectedTagNames, string tenantTypeId = null, long? ownerId = null, int topNumber = 50)
        {
            TagService tagService = new TagService(tenantTypeId);
            IEnumerable<string> ownerTagNames = new List<string>();
            if (ownerId.HasValue && ownerId.Value > 0)
                ownerTagNames = tagService.GetTopOwnerTags(ownerId.Value, topNumber, tenantTypeId).Select(n => n.Key.TagName);
            TagSettings tagSettings = DIContainer.Resolve<ITagSettingsManager>().Get();

            return htmlHelper.EditorForModel("TagSelector", new { tagInputName = tagInputName, ownerId = ownerId, selectedTagNames = string.Join(",", selectedTagNames), ownerTagNames = ownerTagNames, tagSettings = tagSettings });
        }

    }
}