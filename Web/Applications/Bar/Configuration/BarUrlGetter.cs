//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using System.Linq;
using System.Collections.Generic;
using System;
using Tunynet.Common;
using Spacebuilder.Common;

namespace Spacebuilder.Bar
{
    public class BarUrlGetter : IBarUrlGetter
    {
        /// <summary>
        /// 租户类型id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().Bar(); }
        }

        /// <summary>
        /// 动态拥有者类型
        /// </summary>
        public int ActivityOwnerType
        {
            get { return ActivityOwnerTypes.Instance().BarSection(); }
        }

        /// <summary>
        /// 是否为私有状态
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public bool IsPrivate(long sectionId)
        {
            return false;
        }

        /// <summary>
        /// 帖子详细显示页面
        /// </summary>
        /// <param name="threadId">帖子id</param>
        /// <param name="onlyLandlord">只看楼主</param>
        /// <param name="sortBy">排序方式</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>帖子详细显示页面</returns>
        public string ThreadDetail(long threadId, bool onlyLandlord = false, SortBy_BarPost sortBy = SortBy_BarPost.DateCreated, int pageIndex = 1, long? anchorPostId = null, bool isAnchorPostList = false, long? childPostIndex = null)
        {
            return SiteUrls.Instance().ThreadDetail(threadId, onlyLandlord, sortBy, pageIndex, anchorPostId, isAnchorPostList, childPostIndex);
        }

        /// <summary>
        /// 贴吧详细显示页面
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>贴吧详细显示页面</returns>
        public string SectionDetail(long sectionId, SortBy_BarThread? sortBy = null, bool? isEssential = null, long? categoryId = null)
        {
            return SiteUrls.Instance().SectionDetail(sectionId, sortBy, isEssential, categoryId);
        }

        /// <summary>
        /// 创建/编辑帖子页面
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <param name="threadId">帖子id</param>
        /// <returns>创建/编辑帖子页面</returns>
        public string Edit(long sectionId, long? threadId)
        {
            return SiteUrls.Instance().BarThreadEdit(sectionId, threadId);
        }

        /// <summary>
        /// 编辑回帖页面
        /// </summary>
        /// <param name="threadId">帖子id</param>
        /// <param name="postId">回帖id</param>
        /// <returns>编辑回帖页面</returns>
        public string EditPost(long threadId, long? postId)
        {
            return SiteUrls.Instance().EditPost(threadId, postId);
        }

        /// <summary>
        /// 用户帖子页面
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="sectionId">帖子id</param>
        /// <returns>用户帖子页面</returns>
        public string UserThreads(long userId, long? sectionId = null)
        {
            return SiteUrls.Instance().UserThreads(userId, false);
        }

        /// <summary>
        /// 用户的回帖页面
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="sectionId">帖子id</param>
        /// <returns>用户的回帖页面</returns>
        public string UserPosts(long userId, long? sectionId = null)
        {
            return SiteUrls.Instance().UserThreads(userId, true);
        }

        /// <summary>
        /// 管理帖子页面
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public string ManageThreads(long sectionId)
        {
            return SiteUrls.Instance().ManageThreadsForSection(sectionId);
        }

        /// <summary>
        /// 管理回帖页面
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>管理回帖页面</returns>
        public string ManagePosts(long sectionId)
        {
            return SiteUrls.Instance().ManagePostsForSection(sectionId);
        }

        /// <summary>
        /// 管理贴吧类别页面
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>管理贴吧类别页面</returns>
        public string ManageCategories(long sectionId)
        {
            return SiteUrls.Instance().ManageThreadCategoriesForSection(sectionId);
        }

        /// <summary>
        /// 标签下的帖子页面
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>标签下的帖子页面</returns>
        public string ListByTag(string tagName, long? sectionId = null, SortBy_BarThread? sortBy = null, bool? isEssential = null)
        {
            return SiteUrls.Instance().ListsByTag(tagName, sortBy, isEssential);
        }

        /// <summary>
        /// 后台管理首页
        /// </summary>
        /// <returns></returns>
        public string BackstageHome()
        {
            return SiteUrls.Instance().ManageThreads();
        }


        /// <summary>
        /// 后台管理局部页
        /// </summary>
        /// <returns></returns>
        public string _ManageSubMenu()
        {
            return "~/Applications/Bar/Views/ControlPanelBar/_ManageBarRightMenuShortcut.cshtml";
        }

        /// <summary>
        /// 用户首页
        /// </summary>
        /// <param name="userId">用户首页</param>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>用户首页</returns>
        public string UserSpaceHome(long userId, long? sectionId = null)
        {
            return SiteUrls.Instance().UserThreads(userId);
        }

    }
}