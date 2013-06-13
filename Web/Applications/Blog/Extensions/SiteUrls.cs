//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Web.Routing;
using Spacebuilder.Blog;
using Tunynet.Mvc;
using Spacebuilder.Blog.Controllers;
using Tunynet.Utilities;
using Tunynet;
using Tunynet.Common;
using Tunynet.FileStore;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 日志链接管理
    /// </summary>
    public static class SiteUrlsExtension
    {
        private static readonly string BlogAreaName = BlogConfig.Instance().ApplicationKey;

        #region 用户空间日志操作

        /// <summary>
        /// 用户空间日志首页
        /// </summary>
        public static string BlogHome(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Home", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 我的日志/他的日志
        /// </summary>
        public static string Blog(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Blog", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 关注的日志
        /// </summary>
        public static string BlogSubscribed(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Subscribed", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 日志列表：存档、标签、分类
        /// </summary>
        public static string BlogList(this SiteUrls siteUrls, string spaceKey, ListType listType, string tag = null, int year = 0, int month = 0, long categoryId = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            routeValueDictionary.Add("listType", listType);

            switch (listType)
            {
                case ListType.Tag:
                    routeValueDictionary.Add("tag", WebUtility.UrlEncode(tag));
                    break;
                case ListType.Archive:
                    routeValueDictionary.Add("year", year);
                    if (month > 0)
                    {
                        routeValueDictionary.Add("month", month);
                    }
                    break;
                case ListType.Category:
                    routeValueDictionary.Add("categoryId", categoryId);
                    break;
                default:
                    break;
            }


            return CachedUrlHelper.Action("List", "Blog", BlogAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 发布/编辑日志
        /// </summary>
        public static string BlogEdit(this SiteUrls siteUrls, string spaceKey, long? threadId, long? ownerId)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("spaceKey", spaceKey);
            if (threadId.HasValue)
            {
                routeValueDictionary.Add("threadId", threadId);
            }
            if (ownerId.HasValue)
            {
                routeValueDictionary.Add("ownerId", ownerId);
            }

            return CachedUrlHelper.Action("Edit", "Blog", BlogAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 转载日志
        /// </summary>
        public static string _BlogReproduce(this SiteUrls siteUrls, string spaceKey, long threadId)
        {
            return CachedUrlHelper.Action("_Reproduce", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "threadId", threadId } });
        }


        /// <summary>
        /// 批量/单个删除日志
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadIds">日志ID</param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _BlogDelete(this SiteUrls siteUrls, string spaceKey, long? threadIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (threadIds.HasValue)
            {
                dic.Add("threadIds", threadIds.Value);
            }

            return CachedUrlHelper.Action("_Delete", "Blog", BlogAreaName, dic);

        }

        /// <summary>
        /// 置顶/取消置顶
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static string _BlogSetSticky(this SiteUrls siteUrls, string spaceKey, long threadId, bool isSticky)
        {
            return CachedUrlHelper.Action("_BlogSetSticky", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "threadId", threadId }, { "isSticky", isSticky } });
        }

        /// <summary>
        /// 设置分类
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadIds">日志id</param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _BlogSetCategories(this SiteUrls siteUrls, string spaceKey, long? threadIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (threadIds.HasValue)
            {
                dic.Add("threadIds", threadIds.Value);
            }

            return CachedUrlHelper.Action("_SetCategories", "Blog", BlogAreaName, dic);

        }

        /// <summary>
        /// 设置标签
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadIds">日志id</param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _BlogSetTags(this SiteUrls siteUrls, string spaceKey, long? threadIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (threadIds.HasValue)
            {
                dic.Add("threadIds", threadIds.Value);
            }

            return CachedUrlHelper.Action("_SetTags", "Blog", BlogAreaName, dic);
        }

        /// <summary>
        /// 日志详情页
        /// </summary>
        public static string BlogDetail(this SiteUrls siteUrls, string spaceKey, long threadId, long? commentId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary { { "spaceKey", spaceKey }, { "threadId", threadId } };
            if (commentId.HasValue)
                dic.Add("commentId", commentId);
            return CachedUrlHelper.Action("Detail", "Blog", BlogAreaName, dic) + (commentId.HasValue ? "#" + commentId.Value : "");
        }

        /// <summary>
        /// 日志管理（主人）
        /// </summary>
        public static string BlogManage(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Manage", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 日志草稿箱
        /// </summary>
        public static string BlogDraft(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Draft", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// Rss订阅
        /// </summary>
        public static string BlogRss(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Rss", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 关注日志
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static string _BlogSubscribe(this SiteUrls siteUrls, string spaceKey, long threadId)
        {
            return CachedUrlHelper.Action("_Subscribe", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "threadId", threadId } });
        }

        /// <summary>
        /// 取消关注日志
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static string _BlogSubscribeCancel(this SiteUrls siteUrls, string spaceKey, long threadId)
        {
            return CachedUrlHelper.Action("_SubscribeCancel", "Blog", BlogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "threadId", threadId } });
        }

        #endregion

        #region 频道及日志搜索

        /// <summary>
        /// 日志全局搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BlogGlobalSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GlobalSearch", "ChannelBlog", BlogAreaName);
        }

        /// <summary>
        /// 日志快捷搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BlogQuickSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_QuickSearch", "ChannelBlog", BlogAreaName);
        }

        /// <summary>
        /// 日志搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BlogPageSearch(this SiteUrls siteUrls, string keyword = "")
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            return CachedUrlHelper.Action("Search", "ChannelBlog", BlogAreaName, dic);
        }

        /// <summary>
        /// 日志搜索自动完成
        /// </summary>
        public static string BlogSearchAutoComplete(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("SearchAutoComplete", "ChannelBlog", BlogAreaName);
        }

        /// <summary>
        /// 日志频道首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BlogChannelHome(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Home", "ChannelBlog", BlogAreaName);
        }

        /// <summary>
        /// 日志排行页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string BlogListByRank(this SiteUrls siteUrls, string rank)
        {
            return CachedUrlHelper.Action("ListByRank", "ChannelBlog", BlogAreaName, new RouteValueDictionary { { "rank", rank } });
        }

        /// <summary>
        /// 日志标签下的日志
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="tagName">标签名</param>
        /// <returns></returns>
        public static string BlogListByTag(this SiteUrls siteUrls, string tagName)
        {
            return CachedUrlHelper.Action("ListByTag", "ChannelBlog", BlogAreaName, new RouteValueDictionary { { "tagName", WebUtility.UrlEncode(tagName.TrimEnd('.')) } });
        }

        /// <summary>
        /// 日志分类列表
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="categoryId">不同分类的id</param>
        /// <returns></returns>
        public static string BlogListByCategory(this SiteUrls siteUrls, string categoryId)
        {
            return CachedUrlHelper.Action("ListByCategory", "ChannelBlog", BlogAreaName, new RouteValueDictionary { { "categoryId", categoryId } });

        }
        #endregion

        #region 日志后台管理

        /// <summary>
        /// 日志后台管理
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="auditStatus">审核状态</param>
        public static string BlogControlPanelManage(this SiteUrls siteUrls, AuditStatus? auditStatus = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (auditStatus.HasValue)
            {
                dic.Add("auditStatus", auditStatus);
            }

            return CachedUrlHelper.Action("ManageBlogs", "ControlPanelBlog", BlogAreaName, dic);

        }

        /// <summary>
        /// 批量更新日志的审核状态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApprove">审核状态</param>
        public static string _BlogControlPanelUpdateAuditStatus(this SiteUrls siteUrls, bool isApprove)
        {
            return CachedUrlHelper.Action("_UpdateAuditStatus", "ControlPanelBlog", BlogAreaName, new RouteValueDictionary() { { "isApprove", isApprove } });

        }

        /// <summary>
        /// 单个更新日志的审核状态
        /// </summary>
        /// <returns></returns>
        public static string _BlogControlPanelUpdateAuditStatu(this SiteUrls siteUrls, long threadId, bool isApprove)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("threadId", threadId);
            dic.Add("isApprove", isApprove);
            return CachedUrlHelper.Action("_UpdateAuditStatu", "ControlPanelBlog", BlogAreaName, dic);
        }

        /// <summary>
        /// 批量/单个删除日志
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadIds">日志ID</param>
        public static string _BlogControlPanelDelete(this SiteUrls siteUrls, long? threadIds = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (threadIds != null)
            {
                dic.Add("threadIds", threadIds);
            }
            return CachedUrlHelper.Action("_Delete", "ControlPanelBlog", BlogAreaName, dic);

        }

        /// <summary>
        /// 批量/单个设置精华（后台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isEssential">是否精华</param>
        /// <returns>批量设置精华</returns>
        public static string _BlogControlPanelSetEssential(this SiteUrls siteUrls, long? threadIds = null, bool? isEssential = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isEssential", isEssential);
            routeValueDictionary.Add("threadIds", threadIds);
            return CachedUrlHelper.Action("_SetEssential", "ControlPanelBlog", BlogAreaName, routeValueDictionary);

        }

        /// <summary>
        /// 批量/单个设置分类
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="threadIds">日志id</param>
        public static string _BlogControlPanelSetCategory(this SiteUrls siteUrls, string threadIds = null)
        {
            if (string.IsNullOrEmpty(threadIds))
            {
                return CachedUrlHelper.Action("_SetCategory", "ControlPanelBlog", BlogAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("_SetCategory", "ControlPanelBlog", BlogAreaName, new RouteValueDictionary { { "threadIds", threadIds } });
            }
        }

        #endregion

    }
}
