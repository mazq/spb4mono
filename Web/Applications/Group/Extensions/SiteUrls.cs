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
using Tunynet;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组链接管理
    /// </summary>
    public static class SiteUrlsGroupExtension
    {
        private static readonly string GroupAreaName = GroupConfig.Instance().ApplicationKey;

        /// <summary>
        /// 频道群组首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string ChannelGroupHome(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Home", "ChannelGroup", GroupAreaName);
        }

        
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string CreateGroup(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("Create", "ChannelGroup", GroupAreaName);
        }
        #region 群组频道

        /// <summary>
        /// 用户加入群组（群组无验证时）
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string JoinGroup(this SiteUrls siteUrls, long groupId)
        {
            return CachedUrlHelper.Action("JoinGroup", "ChannelGroup", GroupAreaName, new RouteValueDictionary { { "groupId", groupId } });
        }

        /// <summary>
        /// 退出群组
        /// </summary>
        /// <param name="spaceKey">群组标识</param>
        /// <returns></returns>
        public static string _QuitGroup(this SiteUrls siteUrls, long groupId)
        {
            return CachedUrlHelper.Action("_QuitGroup", "ChannelGroup", GroupAreaName, new RouteValueDictionary { { "groupId", groupId } });
        }

        /// <summary>
        /// 用户加入群组（群组有验证时）
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string _EditApply(this SiteUrls siteUrls, long groupId)
        {
            return CachedUrlHelper.Action("_EditApply", "ChannelGroup", GroupAreaName, new RouteValueDictionary { { "groupId", groupId } });
        }

        /// <summary>
        /// 用户加入群组（通过问题验证）
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string _ValidateQuestion(this SiteUrls siteUrls, long groupId)
        {
            return CachedUrlHelper.Action("_ValidateQuestion", "ChannelGroup", GroupAreaName, new RouteValueDictionary { { "groupId", groupId } });
        }

        #endregion

        #region 群组空间

        /// <summary>
        ///  查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        public static string _GetNewerGroupActivities(this SiteUrls siteUrls, string spaceKey, int? applicationId = null, long? lastActivityId = 0)
        {
            return CachedUrlHelper.Action("_GetNewerActivities", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "applicationId", applicationId }, { "lastActivityId", lastActivityId } });
        }

        /// <summary>
        ///  查询自lastActivityId以后又有多少动态进入用户的时间线
        /// </summary>
        /// <returns></returns>
        public static string GetNewerGroupActivityCount(this SiteUrls siteUrls, string spaceKey, int? applicationId = null)
        {
            return CachedUrlHelper.Action("GetNewerGroupActivityCount", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "applicationId", applicationId } });
        }
        /// <summary>
        /// 群组首页动态列表
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _NewGroupActivities(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_NewGroupActivities", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        public static string _ListActivities(this SiteUrls siteUrls, string spaceKey, int? pageIndex = 1, int? applicationId = null, MediaType? mediaType = null, bool? isOriginal = null, long? userId = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            if (pageIndex.Value > 1)
            {
                dic.Add("pageIndex", pageIndex);
            }
            if (applicationId != null)
            {
                dic.Add("applicationId", applicationId);
            }
            if (mediaType != null)
            {
                dic.Add("mediaType", mediaType);
            }
            if (isOriginal != null)
            {
                dic.Add("isOriginal", isOriginal);
            }
            if (userId != null)
            {
                dic.Add("userId", userId);
            }
            return CachedUrlHelper.Action("_ListActivities", "GroupSpace", GroupAreaName, dic);
        }

        /// <summary>
        /// 删除群组动态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _DeleteGroupActivity(this SiteUrls siteUrls, string spaceKey, long activityId)
        {
            return CachedUrlHelper.Action("_DeleteGroupActivity", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "activityId", activityId } });
        }

        /// <summary>
        /// 群组空间首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string GroupHome(this SiteUrls siteUrls, long groupId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", GroupIdToGroupKeyDictionary.GetGroupKey(groupId));
            return CachedUrlHelper.Action("Home", "GroupSpaceTheme", GroupAreaName, dic);
        }


        /// <summary>
        /// 群组空间首页
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string GroupHome(this SiteUrls siteUrls, string spaceKey)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("spaceKey", spaceKey);
            return CachedUrlHelper.Action("Home", "GroupSpaceTheme", GroupAreaName, dic);
        }

        /// <summary>
        /// 设置公告
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _EditAnnouncement(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_EditAnnouncement", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除访客记录
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string DeleteGroupVisitor(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteGroupVisitor", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 群组资料
        /// </summary>
        /// <param name="spaceKey">群组标识</param>
        /// <returns></returns>
        public static string _GroupProfile(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_GroupProfile", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static string BatchRemoveMember(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteMember", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 批量处理申请
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public static string BatchUpdateMemberAuditStatus(this SiteUrls siteUrls, string spaceKey, bool isApproved)
        {
            return CachedUrlHelper.Action("ApproveMemberApply", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "isApproved", isApproved } });
        }

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="id">申请Id</param>
        /// <returns>删除申请链接</returns>
        public static string DeleteMemberApply(this SiteUrls siteUrls, string spaceKey, long id)
        {
            return CachedUrlHelper.Action("DeleteMemberApply", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "id", id } });
        }

        /// <summary>
        /// 邀请好友
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string _Invite(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_Invite", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 成员管理
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static string ManageMembers(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("ManageMembers", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 申请处理
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <param name="groupId"></param>
        /// <param name="applyStatus"></param>
        /// <returns></returns>
        public static string ManageMemberApplies(this SiteUrls siteUrls, string spaceKey, GroupMemberApplyStatus? applyStatus = null)
        {
            RouteValueDictionary route = new RouteValueDictionary();
            route.Add("spaceKey", spaceKey);
            if (applyStatus != null)
                route.Add("applyStatus", applyStatus);
            return CachedUrlHelper.Action("ManageMemberApplies", "GroupSpaceSettings", GroupAreaName, route);
        }

        /// <summary>
        /// 管理员和普通成员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string Members(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("Members", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 关注的成员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string MyFollowedUsers(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("MyFollowedUsers", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string DeleteManager(this SiteUrls siteUrls, string spaceKey, long userId)
        {
            return CachedUrlHelper.Action("DeleteManager", "GroupSpace", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "userId", userId } });
        }

        #endregion

        #region 群组空间设置

        /// <summary>
        /// 删除群组logo
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey">群组标识</param>
        /// <returns></returns>
        public static string _DeleteGroupLogo(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("_DeleteGroupLogo", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }

        /// <summary>
        /// 编辑群组
        /// </summary>
        /// <returns></returns>
        public static string EditGroup(this SiteUrls siteUrls, string spaceKey)
        {
            return CachedUrlHelper.Action("EditGroup", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey } });
        }
        #endregion

        /// <summary>
        /// 用户群组页
        /// </summary>
        /// <param name="siteUrls"></param>
        public static string _CreateGroup(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_CreateGroup", "ChannelGroup", GroupAreaName);
        }

        /// <summary>
        /// 群组地区导航内容块
        /// </summary>
        /// <returns></returns>
        public static string _AreaGroups(this SiteUrls siteUrls, long topNumber = 5, string areaCode = null, long? categoryId = null, SortBy_Group? sortBy = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(areaCode))
                routeValueDictionary.Add("areaCode", areaCode);
            if (categoryId.HasValue && categoryId.Value > 0)
                routeValueDictionary.Add("categoryId", categoryId);
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (topNumber != 0)
            {
                routeValueDictionary.Add("topNumber", topNumber);
            }
            return CachedUrlHelper.Action("_AreaGroups", "ChannelGroup", GroupAreaName, routeValueDictionary);
        }
        /// <summary>
        /// 发现群组
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="nameKeyword"></param>
        /// <param name="areaCode"></param>
        /// <param name="categoryId"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public static string FindGroup(this SiteUrls siteUrls, string areaCode = null, long? categoryId = null, SortBy_Group? sortBy = null, string nameKeyword = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(areaCode))
                routeValueDictionary.Add("areaCode", areaCode);
            if (categoryId.HasValue && categoryId.Value > 0)
                routeValueDictionary.Add("categoryId", categoryId);
            if (sortBy.HasValue)
                routeValueDictionary.Add("sortBy", sortBy);
            if (!string.IsNullOrEmpty(nameKeyword))
                routeValueDictionary.Add("nameKeyword", WebUtility.UrlEncode(nameKeyword));
            return CachedUrlHelper.Action("FindGroup", "ChannelGroup", GroupAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 标签下的群组
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="tagName"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public static string ListByTag(this SiteUrls siteUrls, string tagName, SortBy_Group? sortBy = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            if (sortBy.HasValue)
            {
                routeValueDictionary.Add("sortBy", sortBy);
            }
            routeValueDictionary.Add("tagName", tagName);
            return CachedUrlHelper.Action("ListByTag", "ChannelGroup", GroupAreaName, routeValueDictionary);
        }
        /// <summary>
        /// 用户加入的群组
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string UserJoinedGroups(this SiteUrls siteUrls, string spaceKey = null, int pageIndex = 1, bool isGetMore = false)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
                dic.Add("spaceKey", spaceKey);
            if (pageIndex > 1)
                dic.Add("pageIndex", pageIndex);
            if (isGetMore)
                dic.Add("isGetMore", isGetMore);
            return CachedUrlHelper.Action("UserJoinedGroups", "ChannelGroup", GroupAreaName, dic);
        }
        /// <summary>
        /// 用户创建的群组
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public static string UserCreatedGroups(this SiteUrls siteUrls, string spaceKey = null)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(spaceKey))
            {
                dic.Add("spaceKey", spaceKey);
            }
            return CachedUrlHelper.Action("UserCreatedGroups", "ChannelGroup", GroupAreaName, dic);
        }
        /// <summary>
        /// 标签云图
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string GroupTagMap(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("GroupTagMap", "ChannelGroup", GroupAreaName);
        }
        #region 群组搜索
        /// <summary>
        /// 群组全局搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string GroupGlobalSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_GlobalSearch", "ChannelGroup", GroupAreaName);
        }

        /// <summary>
        /// 群组快捷搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string GroupQuickSearch(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("_QuickSearch", "ChannelGroup", GroupAreaName);
        }

        /// <summary>
        /// 群组搜索
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <returns></returns>
        public static string GroupPageSearch(this SiteUrls siteUrls, string keyword = "", string areaCode = "")
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(keyword))
            {
                dic.Add("keyword", keyword);
            }
            if (!string.IsNullOrEmpty(areaCode))
            {
                dic.Add("NowAreaCode", areaCode);
            }
            return CachedUrlHelper.Action("Search", "ChannelGroup", GroupAreaName, dic);
        }

        /// <summary>
        /// 群组搜索自动完成
        /// </summary>
        public static string GroupSearchAutoComplete(this SiteUrls siteUrls)
        {
            return CachedUrlHelper.Action("SearchAutoComplete", "ChannelGroup", GroupAreaName);
        }
        #endregion

        #region 管理页面-后台

        public static string ManageGroups(this SiteUrls siteUrls, AuditStatus? auditStatus = null)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (auditStatus.HasValue)
                dictionary.Add("auditStatus", auditStatus);
            return CachedUrlHelper.Action("ManageGroups", "ControlPanelGroup", GroupAreaName, dictionary);
        }

        /// <summary>
        /// 批量设置群组的审核状态（后台）
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过</param>
        /// <returns></returns>
        public static string BatchUpdateGroupAuditStatus(this SiteUrls siteUrls, bool isApproved = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("isApproved", isApproved);
            return CachedUrlHelper.Action("BatchUpdateGroupAuditStatus", "ControlPanelGroup", GroupAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 设置群组的审核状态
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="isApproved">是否通过</param>
        /// <returns></returns>
        public static string BatchUpdateGroupAuditStatu(this SiteUrls siteUrls, long groupId, bool isApproved = true)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("groupId", groupId);
            routeValueDictionary.Add("isApproved", isApproved);
            return CachedUrlHelper.Action("BatchUpdateGroupAuditStatu", "ControlPanelGroup", GroupAreaName, routeValueDictionary);
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="siteUrls"></param>
        /// <param name="groupId">群组Id</param>
        /// <returns>删除群组的链接</returns>
        public static string DeleteGroup(this SiteUrls siteUrls, long groupId)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            dictionary.Add("groupId", groupId);
            return CachedUrlHelper.Action("DeleteGroup", "ControlPanelGroup", GroupAreaName, dictionary);
        }

        /// <summary>
        /// 更换群主
        /// </summary>
        /// <returns></returns>
        public static string _ChangeGroupOwner(this SiteUrls siteUrls, string spaceKey, string returnUrl)
        {
            return CachedUrlHelper.Action("_ChangeGroupOwner", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "returnUrl", WebUtility.UrlEncode(returnUrl) } });
        }

        /// <summary>
        /// 删除申请
        /// </summary>
        /// <param name="Id">申请ID</param>
        /// <returns></returns>
        public static string DeleteMemberApply(this SiteUrls siteUrls, long id, string spaceKey)
        {
            return CachedUrlHelper.Action("DeleteMemberApply", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "id", id } });
        }



        /// <summary>
        ///  设置/取消 群组管理员
        /// </summary>
        /// <param name="groupId">群组Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="isManager">是/否管理员</param>
        /// <returns></returns>
        public static string SetManager(this SiteUrls siteUrls, long userId, bool isManager, string spaceKey)
        {
            return CachedUrlHelper.Action("SetManager", "GroupSpaceSettings", GroupAreaName, new RouteValueDictionary { { "spaceKey", spaceKey }, { "userId", userId }, { "isManager", isManager } });
        }

        #endregion

    }

}
