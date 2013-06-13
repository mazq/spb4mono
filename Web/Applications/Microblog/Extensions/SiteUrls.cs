//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Tunynet;
using Tunynet.Common;
using Tunynet.Mvc;
using Tunynet.Utilities;
using Spacebuilder.Microblog;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 微博Url配置
    /// </summary>
    public static class SiteUrlsExtensions
    {
        //Microblog的AreaName
        private static readonly string MicroblogAreaName = "Microblog";

        public static string _MicroblogChildComment(this SiteUrls siteUrls, long? parentId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (parentId.HasValue)
                dic.Add("parentId", parentId);
            return CachedUrlHelper.Action("_ChildComment", "ChannelMicroblog", MicroblogAreaName, dic);
        }

        /// <summary>
        /// 绑定第三方帐号提示信息
        /// </summary>
        public static string _BindThirdAccount(this SiteUrls siteUrls, string accountTypeKey)
        {
            return CachedUrlHelper.Action("_BindThirdAccount", "ChannelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "accountTypeKey", accountTypeKey } });
        }

        #region 发布微博相关

        /// <summary>
        /// 上传图片
        /// </summary>
        public static string _UploadImages(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Create_UploadImages", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 上传图片列表
        /// </summary>
        public static string _ListImages_Microblog(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Create_ListImages", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 话题列表（新建话题，热门话题）
        /// </summary>
        public static string _ListTopics(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Create_AddTopic", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 添加音乐
        /// </summary>
        public static string _AddMusic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Create_AddMusic", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 添加视频
        /// </summary>
        public static string _AddVideo(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Create_AddVideo", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }


        #endregion

        /// <summary>
        /// 新建微博
        /// </summary>
        public static string _CreatMicroblog(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Create", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 微博详细显示页面
        /// </summary>
        public static string ShowMicroblog(this SiteUrls siteUrls, string spaceKey, long MicroblogId, long? commentId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary { { "spaceKey", spaceKey }, { "microblogId", MicroblogId } };
            if (commentId.HasValue)
            {
                dic.Add("commentId", commentId);
            }

            return CachedUrlHelper.Action("Detail", "Microblog", MicroblogAreaName, dic) + (commentId.HasValue ? "#" + commentId.Value : "");
        }

        #region 微博搜索

        public static string SearchAutoComplete(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("SearchAutoComplete", "ChannelMicroblog", MicroblogAreaName);
        }
        /// <summary>
        /// 获取搜索热词
        /// </summary>
        /// <returns></returns>
        public static string GetMicroblogHotWords(this SiteUrls siteUrls, int topNumber)
        {
            return CachedUrlHelper.Action("GetMicroblogHotWords", "ChannelMicroblog", MicroblogAreaName, new RouteValueDictionary() { { "topNumber", topNumber } });
        }

        /// <summary>
        /// 获取搜索历史
        /// </summary>
        /// <returns></returns>
        public static string GetMicroblogSearchHistories(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("GetMicroblogSearchHistories", "ChannelMicroblog", MicroblogAreaName);
        }
        //<summary>
        //微博快捷搜索
        //</summary>
        //<returns></returns>
        public static string MicroblogQuickSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_QuickSearch", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 微博搜索
        /// </summary>
        public static string MicroblogSearch(this SiteUrls siteUrls, string keyword)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            return CachedUrlHelper.Action("Search", "ChannelMicroblog", MicroblogAreaName, dic);
        }

        /// <summary>
        /// 微博全局搜索
        /// </summary>
        public static string MicroblogGlobalSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GlobalSearch", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        public static string _DeleteAttachment_Microblog(this SiteUrls siteUrls, string spaceKey, long attachmentId)
        {
            return CachedUrlHelper.Action("_Create_DeleteAttachment", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "attachmentId", attachmentId } });
        }

        #endregion

        #region Menu

        /// <summary>
        /// 添加我关注的话题
        /// </summary>
        public static string _AddFollowTopic(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_AddFollowTopic", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        #endregion

        #region 微博管理

        /// <summary>
        /// 根据审核状态搜索微博
        /// </summary>
        public static string ManageMicroblogs(this SiteUrls siteUrls, AuditStatus? auditStatus = null, DateTime? startdate = null, DateTime? enddate = null, string tenantTypeId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();

            if (auditStatus != null)
            {
                dic.Add("auditStatus", auditStatus);
            }
            if (!string.IsNullOrEmpty(tenantTypeId))
                dic.Add("tenantTypeId", tenantTypeId);
            if (startdate.HasValue)
            {
                dic.Add("startdate", startdate);
            }
            if (enddate.HasValue)
            {
                dic.Add("enddate", enddate);
            }
            return CachedUrlHelper.Action("ManageMicroblogs", "ControlPanelMicroblog", MicroblogAreaName, dic);
        }

        /// <summary>
        /// 删除微博
        /// </summary>
        public static string DeleteMicroblogs(this SiteUrls siteUrls, string microblogIds = "")
        {
            if (string.IsNullOrEmpty(microblogIds))
            {
                return CachedUrlHelper.Action("DeleteMicroblogs", "ControlPanelMicroblog", MicroblogAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("DeleteMicroblogs", "ControlPanelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "microblogIds", microblogIds } });
            }
        }

        /// <summary>
        ///更新审核状态
        /// </summary>
        public static string UpdateMicroblogAuditStatus(this SiteUrls siteUrls, bool? isApproved = null, string microblogIds = "")
        {
            if (string.IsNullOrEmpty(microblogIds))
            {
                return CachedUrlHelper.Action("UpdateMicroblogAuditStatus", "ControlPanelMicroblog", MicroblogAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("UpdateMicroblogAuditStatus", "ControlPanelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "microblogIds", microblogIds }, { "isApproved", isApproved } });
            }
        }

        /// <summary>
        /// 管理微博话题
        /// </summary>
        public static string ManageMicroblogTopics(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("ManageMicroblogTopics", "ControlPanelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 管理话题分组
        /// </summary>
        public static string ManageTopicGroups(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("ManageTopicGroups", "ControlPanelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 删除微博话题
        /// </summary>
        public static string DeleteTopics(this SiteUrls siteUrls, string topicIds = null)
        {
            if (string.IsNullOrEmpty(topicIds))
            {
                return CachedUrlHelper.Action("DeleteTopics", "ControlPanelMicroblog", MicroblogAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("DeleteTopics", "ControlPanelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "topicIds", topicIds } });
            }
        }

        /// <summary>
        /// 添加、编辑话题
        /// </summary>
        public static string EditTopic(this SiteUrls siteUrls, long topicId = 0)
        {
            if (topicId > 0)
            {
                return CachedUrlHelper.Action("EditTopic", "ControlPanelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "topicId", topicId } });
            }
            else
            {
                return CachedUrlHelper.Action("EditTopic", "ControlPanelMicroblog", MicroblogAreaName);
            }
        }

        /// <summary>
        /// 设置话题分组页
        /// </summary>
        public static string _SetTopicGroup(this SiteUrls siteUrls, string topicIds = "")
        {
            if (string.IsNullOrEmpty(topicIds))
            {
                return CachedUrlHelper.Action("_SetTopicGroup", "ControlPanelMicroblog", MicroblogAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("_SetTopicGroup", "ControlPanelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "topicIds", topicIds } });
            }
        }

        /// <summary>
        /// 设置话题分组
        /// </summary>
        public static string _SetTopicGroup(this SiteUrls siteUrls, string topicIds, long groupId)
        {
            return CachedUrlHelper.Action("_SetTopicGroup", "ControlPanelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "topicIds", topicIds }, { "groupId", groupId } });
        }

        /// <summary>
        /// 添加、编辑话题分组
        /// </summary>
        public static string _EditTopicGroup(this SiteUrls siteUrls, long topicGroupId = 0)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            if (topicGroupId > 0)
            {
                routeValueDictionary.Add("topicGroupId", topicGroupId);
            }
            return CachedUrlHelper.Action("_EditTopicGroup", "ControlPanelMicroblog", MicroblogAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除话题分组
        /// </summary>
        public static string DeleteTopicGroups(this SiteUrls siteUrls, string topicGroupIds = null)
        {
            if (string.IsNullOrEmpty(topicGroupIds))
            {
                return CachedUrlHelper.Action("DeleteTopicGroups", "ControlPanelMicroblog", MicroblogAreaName);
            }
            else
            {
                return CachedUrlHelper.Action("DeleteTopicGroups", "ControlPanelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "topicGroupIds", topicGroupIds } });
            }
        }

        #endregion

        /// <summary>
        /// 我的微博
        /// </summary>
        public static string Mine(this SiteUrls siteUrls, string spaceKey, string type)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary() { { "spaceKey", spaceKey } };

            if (!string.IsNullOrEmpty(type))
            {
                routeValues.Add("type", type);
            }

            return CachedUrlHelper.Action("Mine", "Microblog", MicroblogAreaName, routeValues);
        }

        /// <summary>
        /// 我的微博局部页
        /// </summary>
        public static string _ListMyMicroblogs(this SiteUrls siteUrls, string spaceKey, string type)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary() { { "spaceKey", spaceKey } };

            if (!string.IsNullOrEmpty(type))
            {
                routeValues.Add("type", type);
            }

            return CachedUrlHelper.Action("_ListMyMicroblogs", "Microblog", MicroblogAreaName, routeValues);
        }

        /// <summary>
        /// 我的微博
        /// </summary>
        public static string ListReferred(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("ListReferred", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 我的收藏
        /// </summary>
        public static string ListFavorites(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("ListFavorites", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除当前微博
        /// </summary>
        public static string Delete(this SiteUrls siteUrls, string spaceKey, long microblogId)
        {
            return CachedUrlHelper.Action("Delete", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "microblogId", microblogId } });
        }

        /// <summary>
        /// 收藏微博
        /// </summary>
        public static string Favorite(this SiteUrls siteUrls, string spaceKey, long itemId, long userId)
        {
            return CachedUrlHelper.Action("Favorite", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "itemId", itemId }, { "userId", userId } });
        }

        /// <summary>
        /// 转发微博
        /// </summary>
        public static string _ForwardMicroblog(this SiteUrls siteUrls, string spaceKey, long microblogId)
        {
            return CachedUrlHelper.Action("_ForwardMicroblog", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "microblogId", microblogId } });
        }



        #region 微博列表控件

        /// <summary>
        /// 微博显示控件（列表）
        /// </summary>
        /// <param name="siteUrls">被扩展对象</param>
        /// <param name="microblogId">微博Id</param>
        /// <returns></returns>
        public static string _Microblog(this SiteUrls siteUrls, long? microblogId = null)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();

            if (microblogId.HasValue)
            {
                routeValues = new RouteValueDictionary() { { "microblogId", microblogId } };
            }

            return CachedUrlHelper.Action("_Microblog", "MicroblogActivity", MicroblogAreaName, routeValues);
        }

        /// <summary>
        /// 微博图片列表
        /// </summary>
        public static string _Microblog_Attachments_Images(this SiteUrls siteUrls, long userId, long microblogId, long? forwardMicroblogId = null)
        {

            return CachedUrlHelper.Action("_Microblog_Attachments_Images", "MicroblogActivity", MicroblogAreaName, new RouteValueDictionary { { "userId", userId }, { "microblogId", microblogId }, { "forwardMicroblogId", forwardMicroblogId } });
        }

        /// <summary>
        /// 视频展示
        /// </summary>
        public static string _Microblog_Attachments_Video(this SiteUrls siteUrls, long userId, long microblogId, string alias, long? forwardMicroblogId = null)
        {
            return CachedUrlHelper.Action("_Microblog_Attachments_Video", "MicroblogActivity", MicroblogAreaName, new RouteValueDictionary { { "microblogId", microblogId }, { "Alias", alias }, { "forwardMicroblogId", forwardMicroblogId } });
        }

        /// <summary>
        /// 音乐展示
        /// </summary>
        public static string _Microblog_Attachments_Music(this SiteUrls siteUrls, long userId, long microblogId, string alias, long? forwardMicroblogId = null)
        {
            return CachedUrlHelper.Action("_Microblog_Attachments_Music", "MicroblogActivity", MicroblogAreaName, new RouteValueDictionary { { "microblogId", microblogId }, { "Alias", alias }, { "forwardMicroblogId", forwardMicroblogId } });
        }

        /// <summary>
        /// 评论控件
        /// </summary>
        public static string _Comment_Microblog(this SiteUrls siteUrls, long commentedObjectId, long ownerId, string tenantTypeId, SortBy_Comment sortBy = SortBy_Comment.DateCreatedDesc, string subject = null, string originalAuthor = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("commentedObjectId", commentedObjectId);
            dic.Add("ownerId", ownerId);
            dic.Add("tenantTypeId", tenantTypeId);
            dic.Add("sortBy", sortBy);
            if (!string.IsNullOrEmpty(subject))
                dic.Add("subject", subject);
            if (!string.IsNullOrEmpty(originalAuthor))
                dic.Add("originalAuthor", originalAuthor);

            return CachedUrlHelper.Action("_Comment", "ChannelMicroblog", MicroblogAreaName, dic);
        }

        #endregion

        ///// <summary>
        ///// 评论列表
        ///// </summary>
        //public static string _ShowMicroblogInList_Comment(this SiteUrls siteUrls, string spaceKey, long microblogId)
        //{
        //    return CachedUrlHelper.Action("_ShowMicroblogInList_Comment", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "microblogId", microblogId } });
        //}

        /// <summary>
        /// 获取最新的微博数
        /// </summary>
        public static string _GetNewerCount(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_GetNewerCount", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 获取最新的微博动态
        /// </summary>
        public static string _ListNewerActivities(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_ListNewerActivities", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 获取更多最新的动态
        /// </summary>
        public static string _ListMoreActivities(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_ListMoreActivities", "Microblog", MicroblogAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 微博广场首页（列表模式）
        /// </summary>
        /// <returns></returns>
        public static string Microblog(this SiteUrls siteUrls, SortBy_Microblog sortBy = SortBy_Microblog.DateCreated, long tagGroupId = 0)
        {
            return CachedUrlHelper.Action("Microblog", "ChannelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "sortBy", sortBy }, { "tagGroupId", tagGroupId } });
        }

        /// <summary>
        /// 之前微博
        /// </summary>
        /// <returns></returns>
        public static string _OlderMicroblog(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_OlderMicroblog", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 标签分组微博
        /// </summary>
        /// <returns></returns>
        public static string _GetMicroblogByTagGroup(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GetMicroblogByTagGroup", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 最新微博
        /// </summary>
        /// <returns></returns>
        public static string _NewerMicroblogList(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_NewerMicroblogList", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 最新微博数
        /// </summary>
        /// <returns></returns>
        public static string _GetNewerMicroblogCount(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GetNewerCount", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 微博广场瀑布流页（图片模式）
        /// </summary>
        /// <returns></returns>
        public static string MicroblogWaterfall(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Waterfall", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 微博广场瀑布流数据页
        /// </summary>
        /// <returns></returns>
        public static string _Waterfall(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_Waterfall", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 标签分组微博(图片模式)
        /// </summary>
        /// <returns></returns>
        public static string _WaterfallGetMicroblogByTagGroup(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_WaterfallGetMicroblogByTagGroup", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 微博排行榜
        /// </summary>
        /// <returns></returns>
        public static string MicroblogRanking(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Ranking", "ChannelMicroblog", MicroblogAreaName);
        }

        /// <summary>
        /// 微博话题详细页
        /// </summary>
        /// <returns></returns>
        public static string MicroblogTopic(this SiteUrls siteUrls, string topic)
        {
            return CachedUrlHelper.Action("Topic", "ChannelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "topic", WebUtility.UrlEncode(topic.TrimEnd('.')) } });
        }

        #region GroupSpace

        /// <summary>
        /// 群组话题详细页
        /// </summary>
        /// <returns></returns>
        public static string GroupTopic(this SiteUrls siteUrls, string tagName, string spaceKey = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            dic.Add("tagName", tagName);
            return CachedUrlHelper.Action("TopicDetail", "GroupSpaceMicroblog", MicroblogAreaName, dic);
        }

        /// <summary>
        /// 微博详细显示页面
        /// </summary>
        public static string GroupMicroblogDetail(this SiteUrls siteUrls, string spaceKey, long microblogId, long? commentId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary { { "spaceKey", spaceKey }, { "microblogId", microblogId } };
            if (commentId.HasValue)
            {
                dic.Add("commentId", commentId);
            }
            return CachedUrlHelper.Action("Detail", "GroupSpaceMicroblog", MicroblogAreaName, dic) + (commentId.HasValue ? "#" + commentId.Value : "");
        }

        #endregion


        /// <summary>
        /// 微博话题详细页中的微博列表
        /// </summary>
        public static string _TopicMicroblogList(this SiteUrls siteUrls, long tagId, int pageSize = 20, int pageIndex = 1)
        {
            return CachedUrlHelper.Action("_TopicMicroblogList", "ChannelMicroblog", MicroblogAreaName, new RouteValueDictionary { { "tagId", tagId }, { "pageSize", pageSize }, { "pageIndex", pageIndex } });
        }

        /// <summary>
        /// 微博的评论列表
        /// </summary>
        /// <param name="url"></param>
        /// <param name="tenantType">租户类型id</param>
        /// <param name="commentedObjectId">被评论对象的id</param>
        /// <param name="pageIndex">被评论的页码</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>微博的评论列表</returns>
        public static string _MicroblogCommentList(this SiteUrls url, string tenantType, long commentedObjectId, int pageIndex = 1, SortBy_Comment sortBy = SortBy_Comment.DateCreated)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("tenantType", tenantType);
            dic.Add("commentedObjectId", commentedObjectId);
            if (pageIndex > 1)
                dic.Add("pageIndex", pageIndex);
            if (sortBy != SortBy_Comment.DateCreated)
                dic.Add("sortBy", sortBy);
            return CachedUrlHelper.Action("_CommentList", "ChannelMicroblog", MicroblogAreaName, dic);
        }

        /// <summary>
        /// 微博中获取一条评论
        /// </summary>
        /// <returns>获取一条评论</returns>
        public static string _MicroblogOneComment(this SiteUrls url, long? id = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (id.HasValue)
                dic.Add("id", id);
            return CachedUrlHelper.Action("_OneComment", "ChannelMicroblog", MicroblogAreaName, dic);
        }
    }

}
