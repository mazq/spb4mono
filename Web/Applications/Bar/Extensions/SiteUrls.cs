//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Mvc;
using System.Web.Routing;
using Spacebuilder.Common;
using Tunynet.Utilities;
using Tunynet.Common;
using Spacebuilder.Group;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 帖吧链接管理
    /// </summary>
    public static class SiteUrlsExtension
    {
        private static readonly string BarAreaName = BarConfig.Instance().ApplicationKey;

        /// <summary>
        /// 帖吧首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BarHome(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Home", "Bar", BarAreaName);
        }

        #region 帖吧搜索

        /// <summary>
        /// 帖吧全局搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BarGlobalSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GlobalSearch", "Bar", BarAreaName);
        }

        /// <summary>
        /// 帖吧快捷搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BarQuickSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_QuickSearch", "Bar", BarAreaName);
        }

        /// <summary>
        /// 帖吧搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BarPageSearch(this SiteUrls siteUrls, string keyword)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            return CachedUrlHelper.Action("Search", "Bar", BarAreaName, dic);
        }

        public static string SearchBarAutoComplete(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("SearchAutoComplete", "Bar", BarAreaName);
        }

        #endregion

        #region 帖吧

        /// <summary>
        /// 创建帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string CreateSection(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("CreateSection", "Bar", BarAreaName);
        }

        /// <summary>
        /// 帖吧的详细显示页面
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId">帖吧的Id</param>
        /// <returns>帖吧详细像是页面的链接</returns>
        public static string SectionDetail(this SiteUrls siteUrls, long sectionId, SortBy_BarThread? sortBy = null, bool? isEssential = null, long? categoryId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (isEssential.HasValue)
                routeValueDictionary.Add("isEssential", isEssential);
            if (categoryId.HasValue)
                routeValueDictionary.Add("categoryId", categoryId);
            return CachedUrlHelper.RouteUrl("Channel_Bar_SectionDetail", routeValueDictionary);
        }

        /// <summary>
        /// 帖吧成员列表页面
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId">帖吧的Id</param>
        /// <returns>帖吧详细像是页面的链接</returns>
        public static string SectionMembers(this SiteUrls siteUrls, long sectionId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("SectionMembers", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除帖吧管理员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId">帖吧的Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>帖吧详细像是页面的链接</returns>
        public static string DeleteManager(this SiteUrls siteUrls, long sectionId, long userId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            routeValueDictionary.Add("userId", userId);
            return CachedUrlHelper.Action("DeleteManager", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 帖吧成员中我关注的人
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId">帖吧的Id</param>
        /// <returns>帖吧详细像是页面的链接</returns>
        public static string MyFollowedUsersForSection(this SiteUrls siteUrls, long sectionId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("MyFollowedUsers", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 根据分类显示帖吧列表页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="categoryId">帖吧的Id</param>
        /// <param name="sortBy">帖吧依据</param>
        /// <returns></returns>
        public static string ListSections(this SiteUrls siteUrls, long? categoryId = null, SortBy_BarSection? sortBy = null, string nameKeyword = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (categoryId.HasValue && categoryId.Value > 0)
                routeValueDictionary.Add("categoryId", categoryId);
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (!string.IsNullOrEmpty(nameKeyword))
                routeValueDictionary.Add("nameKeyword", WebUtility.UrlEncode(nameKeyword));
            return CachedUrlHelper.Action("ListSections", "Bar", BarAreaName, routeValueDictionary);
        }


        /// <summary>
        /// 分类帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="categoryId">帖吧分类Id</param>
        /// <returns>帖吧详细像是页面的链接</returns>
        public static string _CategorySections(this SiteUrls siteUrls, long? categoryId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (categoryId.HasValue && categoryId.Value > 0)
                routeValueDictionary.Add("categoryId", categoryId);
            return CachedUrlHelper.Action("_CategorySections", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 关注按钮
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public static string _SubscribeSectionButton(this SiteUrls siteUrls, long sectionId)
        {
            return CachedUrlHelper.Action("_SubscribeSectionButton", "Bar", BarAreaName, new RouteValueDictionary { { "sectionId", sectionId } });
        }

        /// <summary>
        /// 关注帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public static string SubscribeSection(this SiteUrls siteUrls, long sectionId)
        {
            return CachedUrlHelper.Action("SubscribeSection", "Bar", BarAreaName, new RouteValueDictionary { { "sectionId", sectionId } });
        }

        /// <summary>
        /// 取消关注帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public static string CancelSubscribeSection(this SiteUrls siteUrls, long sectionId)
        {
            return CachedUrlHelper.Action("CancelSubscribeSection", "Bar", BarAreaName, new RouteValueDictionary { { "sectionId", sectionId } });
        }

        /// <summary>
        /// 删除帖吧Logo
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public static string _DeleteBarSectionLogo(this SiteUrls siteUrls, long sectionId)
        {
            return CachedUrlHelper.Action("_DeleteBarSectionLogo", "Bar", BarAreaName, new RouteValueDictionary { { "sectionId", sectionId } });
        }

        #endregion

        #region 帖子

        /// <summary>
        /// 编辑帖子的方法
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="SectionId">帖吧的id</param>
        /// <param name="threadId">帖子的id(如果没有即发表新帖)</param>
        /// <returns>编辑帖子的链接</returns>
        public static string BarThreadEdit(this SiteUrls siteUrls, long SectionId, long? threadId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("SectionId", SectionId);
            if (threadId.HasValue && threadId > 0)
                routeValueDictionary.Add("threadId", threadId);
            return CachedUrlHelper.Action("Edit", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 帖子的详细显示页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadId">帖子的id</param>
        /// <param name="pageIndex">回复内容的id</param>
        /// <param name="onlyLandlord">是否只看楼主</param>
        /// <param name="sortBy">回帖的排序方式</param>
        /// <param name="anchorPostId">回帖Id锚点</param>
        /// <param name="isAnchorPostList">是否定位到回帖列表</param>
        /// <param name="childPostIndex">自己回复的列表页码</param>
        /// <returns>帖子的详细显示链接</returns>
        public static string ThreadDetail(this SiteUrls siteUrls, long threadId, bool onlyLandlord = false, SortBy_BarPost sortBy = SortBy_BarPost.DateCreated, int? pageIndex = 1, long? anchorPostId = null, bool isAnchorPostList = false, long? childPostIndex = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("threadId", threadId);
            if (pageIndex.HasValue && pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (onlyLandlord)
                routeValueDictionary.Add("onlyLandlord", onlyLandlord);
            if (sortBy == SortBy_BarPost.DateCreated_Desc)
                routeValueDictionary.Add("sortBy", sortBy);
            string anchor = string.Empty;
            if (childPostIndex.HasValue && childPostIndex > 1)
            {
                BarPost post = new BarPostService().Get(anchorPostId ?? 0);
                if (post.ParentId > 0)
                    routeValueDictionary.Add("postId", post.ParentId);
                else
                    routeValueDictionary.Add("postId", anchorPostId);
                routeValueDictionary.Add("childPostIndex", childPostIndex);
            }
            if (anchorPostId.HasValue && anchorPostId.Value > 0)
                anchor = "#" + anchorPostId;
            else if (isAnchorPostList == true)
                anchor = "#reply";
            return CachedUrlHelper.RouteUrl("Channel_Bar_ThreadDetail", routeValueDictionary) + anchor;
        }


        /// <summary>
        /// 标签下的帖子
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="tagName">标签名</param>
        /// <returns>标签下的帖子名</returns>
        public static string ListsByTag(this SiteUrls siteUrls, string tagName, SortBy_BarThread? sortBy = null, bool? isEssential = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("tagName", WebUtility.UrlEncode(tagName.TrimEnd('.')));
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (isEssential.HasValue)
                routeValueDictionary.Add("isEssential", isEssential.Value);

            return CachedUrlHelper.RouteUrl("Channel_Bar_Tag", routeValueDictionary);
        }

        /// <summary>
        /// 帖子排行
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sortBy"></param>
        /// <param name="isEssential"></param>
        /// <returns></returns>
        public static string BarThreadRank(this SiteUrls siteUrls, SortBy_BarThread? sortBy = null, bool? isEssential = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (isEssential.HasValue)
                routeValueDictionary.Add("isEssential", isEssential.Value);

            return CachedUrlHelper.Action("Rank", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 标签地图
        /// </summary>
        /// <param name="siteUrls"></param>
        public static string TagMap(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("TagMap", "Bar", BarAreaName);
        }

        /// <summary>
        /// 用户帖子页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="userId"></param>
        /// <param name="isPosted"></param>
        /// <returns></returns>
        public static string UserThreads(this SiteUrls siteUrls, long userId, bool? isPosted = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", UserIdToUserNameDictionary.GetUserName(userId));
            if (isPosted.HasValue)
                routeValueDictionary.Add("isPosted", isPosted.Value);
            return CachedUrlHelper.RouteUrl("Channel_Bar_UserBar", routeValueDictionary);
        }


        /// <summary>
        /// 用户关注的帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        public static string UserFollowedSections(this SiteUrls siteUrls, long userId)
        {
            return CachedUrlHelper.Action("UserFollowedSections", "Bar", BarAreaName, new RouteValueDictionary { { "userId", userId } });
        }

        /// <summary>
        /// 移动帖子
        /// </summary>
        /// <param name="threadId">帖子id</param>
        /// <param name="categoryId">帖子id</param>
        /// <param name="siteUrls"></param>
        /// <returns>移动帖子</returns>
        public static string _MoveThread(this SiteUrls siteUrls, long threadId, long? categoryId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("threadId", threadId);
            if (categoryId.HasValue)
                dic.Add("categoryId", categoryId);
            return CachedUrlHelper.Action("_MoveThread", "Bar", BarAreaName, dic);
        }

        /// <summary>
        /// 帖吧选择页面
        /// </summary>
        /// <param name="categoryId">选中的类别</param>
        /// <param name="selectedSectionId">被选中的帖吧</param>
        /// <returns>帖吧选择的页面</returns>
        public static string _BarSetionSelectItem(this SiteUrls urls, string name, long? categoryId = null, long? selectedSectionId = null, string tenantTypeId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("name", name);
            if (categoryId.HasValue)
                dic.Add("categoryId", categoryId);
            if (selectedSectionId.HasValue)
                dic.Add("selectedSectionId", selectedSectionId);
            if (!string.IsNullOrEmpty(tenantTypeId))
                dic.Add("tenantTypeId", tenantTypeId);
            return CachedUrlHelper.Action("_BarSetionSelectItem", "Bar", BarAreaName, dic);
        }

        #endregion

        #region 评分

        /// <summary>
        /// 帖子的评分
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadId">被评论的帖子的id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>帖子的评分</returns>
        public static string _ListBarRatings(this SiteUrls siteUrls, long threadId, int pageIndex = 1)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("threadId", threadId);
            if (pageIndex > 1)
                dic.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("_ListBarRatings", "Bar", BarAreaName, dic);
        }

        /// <summary>
        /// 创建帖吧评论
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadId">被评论的id</param>
        /// <returns>创建帖吧评论</returns>
        public static string _CreatBarRating(this SiteUrls siteUrls, long threadId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("threadId", threadId);
            return CachedUrlHelper.Action("_CreatBarRating", "Bar", BarAreaName, dic);
        }

        /// <summary>
        /// 获取一条新的评分
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="ratingId">评分id</param>
        /// <returns>获取一条新的评分</returns>
        public static string _OneBarRating(this SiteUrls siteUrls, long? ratingId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (ratingId.HasValue)
                dic.Add("ratingId", ratingId);
            return CachedUrlHelper.Action("_OneBarRating", "Bar", BarAreaName, dic);
        }

        #endregion

        #region 回帖

        /// <summary>
        /// 通过postid跳转到他的显示页面
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public static string ThreadDetailGotoPost(this SiteUrls siteUrls, long anchorPostId)
        {
            return WebUtility.ResolveUrl(String.Format("~/Service/Bar/BarUrl.ashx?anchorPostId={0}", anchorPostId));
        }


        /// <summary>
        /// 回帖列表
        /// </summary>
        /// <param name="ParentId">父级回帖id</param>
        /// <param name="siteUrls"></param>
        /// <param name="ThreadId">主题帖id</param>
        /// <returns></returns>
        public static string _ListBarPost(this SiteUrls siteUrls, long ThreadId, long? ParentId = null, long pageIndex = 1, bool showBeforPage = false, bool showAfterPage = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("ThreadId", ThreadId);
            if (showBeforPage)
                routeValueDictionary.Add("showBeforPage", showBeforPage);
            if (!showAfterPage)
                routeValueDictionary.Add("showAfterPage", showAfterPage);
            if (ParentId.HasValue)
                routeValueDictionary.Add("ParentId", ParentId);
            if (pageIndex > 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("_ListPost", "Bar", BarAreaName, routeValueDictionary);

        }

        /// <summary>
        /// 一条子级回复
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="postId">回复的id</param>
        /// <returns>子级回复的链接</returns>
        public static string _BarPost(this SiteUrls siteUrls, long? postId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (postId.HasValue)
                routeValueDictionary.Add("postId", postId);
            return CachedUrlHelper.Action("_BarPost", "Bar", BarAreaName, routeValueDictionary);

        }

        /// <summary>
        /// 子级回复的链接（发帖以及回复列表）
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>子级回复的链接</returns>
        public static string _ChildPost(this SiteUrls siteUrls, long parentId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("parentId", parentId);
            return CachedUrlHelper.Action("_ChildPost", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除回帖
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="postId">被删除的回帖id</param>
        /// <returns>删除回帖链接</returns>
        public static string DeletePost(this SiteUrls siteUrls, long postId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("postId", postId);
            return CachedUrlHelper.Action("DeletePost", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 编辑回帖方法
        /// </summary>
        /// <param name="threadId">主题帖id</param>
        /// <param name="postId">回帖id</param>
        /// <returns>编辑回帖的链接</returns>
        public static string EditPost(this SiteUrls siteUrls, long threadId, long? postId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("threadId", threadId);
            if (postId.HasValue)
                routeValueDictionary.Add("postId", postId);
            return CachedUrlHelper.Action("EditPost", "Bar", BarAreaName, routeValueDictionary);
        }
        #endregion

        #region 管理页面-后台

        /// <summary>
        /// 帖子分类设置
        /// </summary>
        /// <param name="urls">连接</param>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>帖子分类设置</returns>
        public static string _ThreadCategoriesSettings(this SiteUrls urls, long sectionId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("_ThreadCategoriesSettings", "Bar", BarAreaName, dic);
        }

        /// <summary>
        /// 获取管理数据Url
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns>管理数据Url</returns>
        public static string _GetManageableData(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GetManageableData", "ControlPanelBar", BarAreaName);
        }

        /// <summary>
        /// 获取统计数据Url
        /// </summary>
        /// <param name="siteUrls"></param>        
        /// <returns>管理数据Url</returns>
        public static string _GetStatisticData(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GetStatisticData", "ControlPanelBar", BarAreaName);
        }

        /// <summary>
        /// 管理贴子
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>管理贴子页面</returns>
        public static string ManageThreads(this SiteUrls siteUrls, AuditStatus? auditStatus = null, string tenantTypeId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (auditStatus.HasValue)
                dic.Add("AuditStatus", auditStatus);
            if (!string.IsNullOrEmpty(tenantTypeId))
                dic.Add("tenantTypeId", tenantTypeId);
            return CachedUrlHelper.Action("ManageThreads", "ControlPanelBar", BarAreaName, dic);
        }

        /// <summary>
        /// 管理回帖
        /// </summary>        
        /// <param name="siteUrls"></param>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>管理回帖页面</returns>
        public static string ManagePosts(this SiteUrls siteUrls, AuditStatus? auditStatus = null, string tenantTypeId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (auditStatus.HasValue)
                dic.Add("auditStatus", auditStatus);
            if (!string.IsNullOrEmpty(tenantTypeId))
                dic.Add("tenantTypeId", tenantTypeId);
            return CachedUrlHelper.Action("ManagePosts", "ControlPanelBar", BarAreaName, dic);
        }

        /// <summary>
        /// 管理帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="auditStatus">审核状态</param>
        /// <param name="sectionId">帖吧Id</param>
        /// <returns>帖吧管理页面</returns>
        public static string ManageBars(this SiteUrls siteUrls, long? sectionId = null, AuditStatus? auditStatus = null)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (sectionId.HasValue)
                dictionary.Add("sectionId", sectionId);
            if (auditStatus.HasValue)
                dictionary.Add("auditStatus", auditStatus);
            return CachedUrlHelper.Action("ManageBars", "ControlPanelBar", BarAreaName, dictionary);
        }

        /// <summary>
        /// 帖吧设置
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns>帖吧设置页面</returns>
        public static string SectionSettings(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("SectionSettings", "ControlPanelBar", BarAreaName);
        }

        /// <summary>
        /// 编辑帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId">帖吧id</param>
        /// <returns>编辑帖吧名字</returns>
        public static string EditSection(this SiteUrls siteUrls, long? sectionId = null)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (sectionId.HasValue)
                dictionary.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("EditSection", "ControlPanelBar", BarAreaName, dictionary);

        }

        /// <summary>
        /// 删除帖吧
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId">帖吧id</param>
        /// <returns>删除帖吧的链接</returns>
        public static string DeleteSection(this SiteUrls siteUrls, long sectionId)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            dictionary.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("DeleteSection", "ControlPanelBar", BarAreaName, dictionary);
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadId">帖子id</param>
        /// <returns>删除帖子的链接</returns>
        public static string DeleteThread(this SiteUrls siteUrls, long? threadId = null)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (threadId.HasValue)
                dictionary.Add("threadId", threadId);
            return CachedUrlHelper.Action("Delete", "Bar", BarAreaName, dictionary);
        }

        /// <summary>
        /// 批量设置帖子的审核状态（后台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过</param>
        /// <returns></returns>
        public static string BatchUpdateThreadAuditStatus(this SiteUrls siteUrls, bool isApproved = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isApproved", isApproved);
            return CachedUrlHelper.Action("BatchUpdateThreadAuditStatus", "ControlPanelBar", BarAreaName, routeValueDictionary);
        }
        /// <summary>
        /// 设置帖子的审核状态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过</param>
        /// <returns></returns>
        public static string BatchUpdateThreadAuditStatus(this SiteUrls siteUrls, long threadId, bool isApproved = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isApproved", isApproved);
            routeValueDictionary.Add("threadId", threadId);
            return CachedUrlHelper.Action("BatchUpdateThreadAuditStatu", "ControlPanelBar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量设置精华（后台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isEssential">是否精华</param>
        /// <returns>批量设置精华</returns>
        public static string BatchSetEssential(this SiteUrls siteUrls, bool isEssential = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isEssential", isEssential);
            return CachedUrlHelper.Action("BatchSetEssential", "ControlPanelBar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量置顶（后台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isSticky">是否置顶</param>
        /// <param name="stickyDate">置顶日期</param>
        /// <returns>批量置顶</returns>
        public static string BatchSetSticky(this SiteUrls siteUrls, bool isSticky = true, DateTime stickyDate = new DateTime())
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isSticky", isSticky);
            if (stickyDate == new DateTime())
                stickyDate = DateTime.UtcNow;
            routeValueDictionary.Add("stickyDate", stickyDate);
            return CachedUrlHelper.Action("BatchSetSticky", "ControlPanelBar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量删除帖子（后台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns>批量删除帖子</returns>
        public static string BatchDeleteThread(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("BatchDeleteThread", "ControlPanelBar", BarAreaName);
        }

        /// <summary>
        /// 设置置顶时间
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string _SetStickyDate(this SiteUrls siteUrls, long? threadIds = null, bool showTips = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (threadIds.HasValue)
                routeValueDictionary.Add("threadIds", threadIds);
            if (!showTips)
                routeValueDictionary.Add("showTips", showTips);
            routeValueDictionary.Add("_", DateTime.UtcNow.Ticks);
            return CachedUrlHelper.Action("_SetStickyDate", "Bar", BarAreaName, routeValueDictionary);
        }

        #endregion

        #region 管理页面-前台

        #region 页面
        /// <summary>
        /// 管理单一帖吧的帖子（前台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="sectionId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static string ManageThreadsForSection(this SiteUrls siteUrls, long sectionId, int pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            if (pageIndex > 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("ManageThreads", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 单一帖吧管理回帖页面（前台）
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="sectionId">帖吧Id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>管理回帖页面（前台）</returns>
        public static string ManagePostsForSection(this SiteUrls siteUrl, long sectionId, int pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            if (pageIndex > 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("ManagePosts", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 管理帖吧的分类信息（前台）
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="sectionId">帖吧id</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns>管理帖吧的分类信息</returns>
        public static string ManageThreadCategoriesForSection(this SiteUrls siteUrl, long sectionId, int pageIndex = 1)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            if (pageIndex > 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("ManageThreadCategories", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 编辑帖吧的描述（前台）
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="sectionId">帖吧的id</param>
        /// <returns>编辑帖吧的描述</returns>
        public static string EditSectionInfo(this SiteUrls siteUrl, long sectionId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("EditSection", "Bar", BarAreaName, routeValueDictionary);

        }
        #endregion

        #region 批量操作-帖子
        /// <summary>
        /// 批量设置帖子的审核状态（前台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过</param>
        /// <returns></returns>
        public static string BatchUpdateThreadAuditStatusForSection(this SiteUrls siteUrls, long sectionId, bool isApproved = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isApproved", isApproved);
            routeValueDictionary.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("BatchUpdateThreadAuditStatus", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量设置精华（前台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isEssential">是否精华</param>
        /// <param name="threadId">需要置顶的帖子</param>
        /// <param name="sectionId">帖吧的id</param>
        /// <returns>批量设置精华</returns>
        public static string BatchSetEssentialForSection(this SiteUrls siteUrls, long sectionId, bool isEssential = true, long? threadId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isEssential", isEssential);
            routeValueDictionary.Add("sectionId", sectionId);
            if (threadId.HasValue)
                routeValueDictionary.Add("threadIds", threadId);
            return CachedUrlHelper.Action("BatchSetEssential", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量置顶（前台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isSticky">是否置顶</param>
        /// <param name="stickyDate">置顶日期</param>
        /// <returns>批量置顶</returns>
        public static string BatchSetStickyForSection(this SiteUrls siteUrls, bool isSticky = true, DateTime stickyDate = new DateTime(), long? threadId = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isSticky", isSticky);
            if (stickyDate == new DateTime())
                stickyDate = DateTime.UtcNow;
            routeValueDictionary.Add("stickyDate", stickyDate);
            if (threadId.HasValue)
                routeValueDictionary.Add("threadIds", threadId);
            return CachedUrlHelper.Action("BatchSetSticky", "Bar", BarAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 批量删除帖子（前台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns>批量删除帖子</returns>
        public static string BatchDeleteThreadForSection(this SiteUrls siteUrls, long sectionId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("BatchDeleteThread", "Bar", BarAreaName, dic);
        }
        #endregion

        #region 类别

        /// <summary>
        /// 编辑类别的局部页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="OwnerId">拥有者id（帖吧id）</param>
        /// <param name="CategoryId">分类Id</param>
        /// <returns>编辑分类的局部页面</returns>
        public static string _EditThreadCategory(this SiteUrls siteUrls, long OwnerId, long CategoryId = 0)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("OwnerId", OwnerId);
            if (CategoryId != 0)
                dic.Add("CategoryId", CategoryId);
            return CachedUrlHelper.Action("_EditThreadCategory", "Bar", BarAreaName, dic);
        }

        /// <summary>
        /// 改变分类的排序
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns>改变分类的排序</returns>
        public static string ChangeSectionCategoryDisplayOrder(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("ChangeDisplayOrder", "Bar", BarAreaName);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="categoryId">分类id</param>
        /// <returns>改变分类的排序</returns>
        public static string DeleteThreadCategory(this SiteUrls siteUrls, long categoryId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("categoryId", categoryId);
            return CachedUrlHelper.Action("DeleteThreadCategory", "Bar", BarAreaName, dic);
        }

        #endregion

        #region 批量操作-回帖

        /// <summary>
        /// 回帖批量设置审核状态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过审核</param>
        /// <returns>回帖批量设置审核</returns>
        public static string BatchUpdatePostAuditStatus(this SiteUrls siteUrls, bool isApproved = true)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!isApproved)
                dic.Add("isApproved", isApproved);
            return CachedUrlHelper.Action("BatchUpdatePostAuditStatus", "Bar", BarAreaName, dic);
        }

        /// <summary>
        /// 批量/单挑删除回帖
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postIds"></param>
        /// <returns></returns>
        public static string BatchDeletePosts(this SiteUrls url, long? postIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (postIds.HasValue)
                dic.Add("postIds", postIds);
            return CachedUrlHelper.Action("BatchDeletePosts", "Bar", BarAreaName, dic);
        }

        #endregion

        #endregion

        #region 批量操作-贴吧

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="urls">被扩展的链接</param>
        /// <param name="isApproved">是否通过审核</param>
        /// <returns>批量更新帖子的审核状态</returns>
        public static string BatchUpdateSectionAuditStatus(this SiteUrls urls, bool isApproved = true, long? sectionId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!isApproved)
                dic.Add("isApproved", isApproved);
            if (sectionId.HasValue)
                dic.Add("sectionIds", sectionId);
            return CachedUrlHelper.Action("BatchUpdateSectionAuditStatus", "ControlPanelBar", BarAreaName, dic);
        }

        /// <summary>
        /// 更新帖吧的审核状态
        /// </summary>
        /// <param name="sectionId">被更新的帖吧id</param>
        /// <param name="isApproved">是否通过审核</param>
        /// <returns>更新帖吧的审核状态</returns>
        public static string BatchUpdateSectionAuditStatu(this SiteUrls urls, long sectionId, bool isApproved)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("isApproved", isApproved);
            dic.Add("sectionId", sectionId);
            return CachedUrlHelper.Action("BatchUpdateSectionAuditStatu", "ControlPanelBar", BarAreaName, dic);
        }

        #endregion

        #region 群组贴吧

        /// <summary>
        /// 群组贴吧帖子详细显示页面
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="threadId">帖子的id</param>
        /// <param name="pageIndex">回复内容的id</param>
        /// <param name="onlyLandlord">是否只看楼主</param>
        /// <param name="sortBy">回帖的排序方式</param>
        /// <param name="anchorPostId">回帖Id锚点</param>
        /// <param name="isAnchorPostList">是否定位到回帖列表</param>
        /// <param name="childPostIndex">自己回复的列表页码</param>
        /// <returns>帖子的详细显示链接</returns>
        public static string GroupThreadDetail(this SiteUrls urls, string spaceKey, long threadId, bool onlyLandlord = false, SortBy_BarPost sortBy = SortBy_BarPost.DateCreated, int? pageIndex = 1, long? anchorPostId = null, bool isAnchorPostList = false, long? childPostIndex = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);

            routeValueDictionary.Add("threadId", threadId);

            if (pageIndex.HasValue && pageIndex != 1)
                routeValueDictionary.Add("pageIndex", pageIndex);
            if (onlyLandlord)
                routeValueDictionary.Add("onlyLandlord", onlyLandlord);
            if (sortBy == SortBy_BarPost.DateCreated_Desc)
                routeValueDictionary.Add("sortBy", sortBy);
            string anchor = string.Empty;
            if (childPostIndex.HasValue && childPostIndex > 1)
            {
                BarPost post = new BarPostService().Get(anchorPostId ?? 0);
                if (post.ParentId > 0)
                    routeValueDictionary.Add("postId", post.ParentId);
                else
                    routeValueDictionary.Add("postId", anchorPostId);
                routeValueDictionary.Add("childPostIndex", childPostIndex);
            }
            if (anchorPostId.HasValue && anchorPostId.Value > 0)
                anchor = "#" + anchorPostId;
            else if (isAnchorPostList == true)
                anchor = "#reply";
            return CachedUrlHelper.RouteUrl("Group_Bar_ThreadDetail", routeValueDictionary) + anchor;
        }

        /// <summary>
        /// 群组贴吧首页
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="spaceKey">群组groupKey</param>
        /// <param name="categoryId">分类id</param>
        /// <param name="isEssential">是否精华</param>
        /// <param name="sortBy">排序方式</param>
        /// <param name="pageIndex">回帖页码</param>
        /// <returns>群组贴吧首页</returns>
        public static string GroupSectionDetail(this SiteUrls urls, string spaceKey, long? categoryId = null, bool? isEssential = null, SortBy_BarThread? sortBy = null, int pageIndex = 1, bool? isPosted = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (categoryId.HasValue)
                dic.Add("categoryId", categoryId);
            if (isEssential.HasValue)
                dic.Add("isEssential", isEssential);
            if (sortBy.HasValue)
                dic.Add("sortBy", sortBy);
            if (pageIndex > 1)
                dic.Add("pageIndex", pageIndex);
            if (isPosted.HasValue)
                dic.Add("isPosted", isPosted);
            return CachedUrlHelper.Action("SectionDetail", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 群组贴吧标签下的帖子
        /// </summary>
        /// <param name="spaceKey">群组groupKey</param>
        /// <param name="tagName">标签名</param>
        /// <param name="sortBy">排序字段</param>
        /// <param name="isEssential">是否精华</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>群组贴吧标签下的帖子</returns>
        public static string GroupThreadListByTag(this SiteUrls urls, string spaceKey, string tagName, SortBy_BarThread? sortBy = null, bool? isEssential = null, int pageIndex = 1)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            dic.Add("tagName", tagName);
            if (sortBy.HasValue)
                dic.Add("sortBy", sortBy);
            if (isEssential.HasValue)
                dic.Add("isEssential", isEssential);
            if (pageIndex > 1)
                dic.Add("pageIndex", pageIndex);
            return CachedUrlHelper.Action("ListByTag", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 创建/编辑页面
        /// </summary>
        /// <param name="spaceKey">群组groupKey</param>
        /// <param name="threadId">帖子id</param>
        /// <returns>创建/编辑页面</returns>
        public static string GroupThreadEdit(this SiteUrls urls, string spaceKey, long? threadId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (threadId.HasValue)
                dic.Add("threadId", threadId);
            return CachedUrlHelper.Action("Edit", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 群组中的编辑回帖页面
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="threadId">帖子id</param>
        /// <param name="postId">回帖id</param>
        /// <returns>群组中的编辑回帖页面</returns>
        public static string GroupEditPost(this SiteUrls urls, string spaceKey, long threadId, long? postId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            dic.Add("threadId", threadId);
            dic.Add("action", "EditPost");
            if (postId.HasValue)
                dic.Add("postId", postId);
            return CachedUrlHelper.RouteUrl("Group_Bar_Common", dic);
        }

        /// <summary>
        /// 群组中用户的帖子
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="userName">用户名</param>
        /// <param name="isPosted">是否是回帖</param>
        /// <returns>群组中用户的帖子</returns>
        public static string GroupUserThreads(this SiteUrls urls, string spaceKey, bool isPosted = false)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (isPosted)
                dic.Add("isPosted", isPosted);
            return CachedUrlHelper.Action("UserThreads", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 群组管理帖子页面
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="spaceKey">群组groupKey</param>
        /// <returns>群组管理帖子页面</returns>
        public static string GroupManageThreads(this SiteUrls urls, string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("ManageThreads", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 群组管理回帖页面
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="spaceKey">群组groupKey</param>
        /// <returns>群组管理回帖页面</returns>
        public static string GroupManagePosts(this SiteUrls urls, string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("ManagePosts", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 群组管理帖子分类页面
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="spaceKey">群组groupKey</param>
        /// <returns>群组管理帖子分类页面</returns>
        public static string GroupManageThreadCategories(this SiteUrls urls, string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("ManageThreadCategories", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 群组贴吧搜索页面
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="spaceKey">群组名</param>
        /// <param name="keyWord">关键字</param>
        /// <returns>群组贴吧搜索页面</returns>
        public static string GroupBarThreadSearch(this SiteUrls urls, string spaceKey, string keyWord = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);

            if (!string.IsNullOrEmpty(keyWord))
                dic.Add("keyWord", keyWord);

            return CachedUrlHelper.Action("Search", "GroupSpaceBar", BarAreaName, dic);
        }

        /// <summary>
        /// 标签云图
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string Tags(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Tags", "GroupSpaceBar", BarAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 群组贴吧搜索页面
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="sectionId"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public static string GroupBarThreadSearch(this SiteUrls urls, long sectionId, string keyWord = null)
        {
            string spaceKey = GroupIdToGroupKeyDictionary.GetGroupKey(sectionId);
            return urls.GroupBarThreadSearch(spaceKey, keyWord);
        }

        #endregion
    }

}
