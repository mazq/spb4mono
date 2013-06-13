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
using Spacebuilder.Group;

namespace Spacebuilder.Microblog
{
    public class GroupUrlGetter : IMicroblogUrlGetter
    {
        GroupService groupService = new GroupService();

        /// <summary>
        /// 租户类型id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().Group(); }
        }

        /// <summary>
        /// 动态拥有者类型
        /// </summary>
        public int ActivityOwnerType
        {
            get { return ActivityOwnerTypes.Instance().Group(); }
        }

        /// <summary>
        /// 是否为私有动态
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public bool IsPrivate(long ownerId)
        {
            GroupEntity group = groupService.Get(ownerId);
            if (group == null)
                return false;
            return !group.IsPublic;
        }

        /// <summary>
        /// 拥有者名称
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string GetOwnerName(long ownerId)
        {
            GroupEntity group = groupService.Get(ownerId);
            if (group == null)
                return string.Empty;
            return group.GroupName;
        }

        /// <summary>
        /// 群组话题详细页
        /// </summary>
        /// <returns></returns>
        public string TopicDetail(string tagName, long ownerId = 0)
        {
            var tag = new TagService(TenantTypeIds.Instance().Microblog()).Get(tagName);
            if (tag == null)
                return string.Empty;
            string spaceKey = GroupIdToGroupKeyDictionary.GetGroupKey(ownerId);
            return SiteUrls.Instance().GroupTopic(tagName, spaceKey);
        }

        /// <summary>
        /// 微博详细显示页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="microblogId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public string MicroblogDetail(long microblogId, long? commentId = null)
        {
            string spaceKey = string.Empty;
            MicroblogService microblogService = new MicroblogService();
            MicroblogEntity microblog = microblogService.Get(microblogId);
            if (microblog == null)
            {
                return string.Empty;
            }
            else
            {
                if (microblog.OriginalMicroblog != null)
                {
                    microblog = microblog.OriginalMicroblog;
                    if (microblog == null)
                        return string.Empty;
                }
                spaceKey = GroupIdToGroupKeyDictionary.GetGroupKey(microblog.OwnerId);
            }
            return SiteUrls.Instance().GroupMicroblogDetail(spaceKey, microblogId, commentId);
        }

        /// <summary>
        /// 获取拥有者链接
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string GetOwnerUrl(long ownerId)
        {
            GroupEntity group = groupService.Get(ownerId);
            if (group == null)
                return string.Empty;
            return SiteUrls.Instance().GroupHome(group.GroupKey);
        }

        /// <summary>
        /// 后台导航
        /// </summary>
        /// <returns></returns>
        public string _ManageSubMenu()
        {
            return "~/Applications/Group/Views/ControlPanelGroup/_ManageGroupSideMenuShortcut.cshtml";
        }
    }
}