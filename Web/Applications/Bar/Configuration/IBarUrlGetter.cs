//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 贴吧中获取连接的接口
    /// </summary>
    public interface IBarUrlGetter
    {
        /// <summary>
        /// 租户类型id
        /// </summary>
        string TenantTypeId { get; }

        /// <summary>
        /// 动态拥有者类型
        /// </summary>
        int ActivityOwnerType { get; }

        /// <summary>
        /// 是否为私有状态
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        bool IsPrivate(long sectionId);

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="threadId">帖子的id</param>
        /// <returns></returns>
        string ThreadDetail(long threadId, bool onlyLandlord = false, SortBy_BarPost sortBy = SortBy_BarPost.DateCreated, int pageIndex = 1, long? anchorPostId = null, bool isAnchorPostList = false, long? childPostIndex = null);

        /// <summary>
        /// 贴吧详细显示页面
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>贴吧详细显示页面</returns>
        string SectionDetail(long sectionId, SortBy_BarThread? sortBy = null, bool? isEssential = null, long? categoryId = null);

        /// <summary>
        /// 编辑\创建帖子
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <param name="threadId">帖子id</param>
        /// <returns>编辑\创建帖子</returns>
        string Edit(long sectionId, long? threadId = null);

        /// <summary>
        /// 编辑回帖的方法
        /// </summary>
        /// <param name="threadId">帖子的id</param>
        /// <param name="postId">回帖的id</param>
        /// <returns>编辑回帖的方法</returns>
        string EditPost(long threadId, long? postId = null);

        /// <summary>
        /// 用户首页
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="sectionId">帖子id</param>
        /// <returns>用户首页</returns>
        string UserSpaceHome(long userId, long? sectionId = null);

        /// <summary>
        /// 用户的帖子页面
        /// </summary>
        /// <param name="sectionId">帖子id</param>
        /// <param name="userId">用户id</param>
        /// <returns>用户帖子的详细</returns>
        string UserThreads(long userId, long? sectionId = null);

        /// <summary>
        /// 用户的回帖页面
        /// </summary>
        /// <param name="sectionId">贴吧的id</param>
        /// <param name="userId">用户id</param>
        /// <returns>用户回帖的页面</returns>
        string UserPosts(long userId, long? sectionId = null);

        /// <summary>
        /// 管理帖子（前台页面）
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <returns></returns>
        string ManageThreads(long sectionId);

        /// <summary>
        /// 管理回帖（前台页面）
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>回帖管理</returns>
        string ManagePosts(long sectionId);

        /// <summary>
        /// 管理类别
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <returns>类别管理</returns>
        string ManageCategories(long sectionId);

        /// <summary>
        /// 标签下的帖子
        /// </summary>
        /// <param name="sectionId">贴吧id</param>
        /// <param name="tagName">标签名</param>
        /// <returns>标签下的帖子</returns>
        string ListByTag(string tagName, long? sectionId = null, SortBy_BarThread? sortBy = null, bool? isEssential = null);

        /// <summary>
        /// 后台管理首页
        /// </summary>
        /// <returns>后台管理首页</returns>
        string BackstageHome();

        /// <summary>
        /// 后台快捷操局部页面的连接
        /// </summary>
        /// <returns></returns>
        string _ManageSubMenu();
    }
}