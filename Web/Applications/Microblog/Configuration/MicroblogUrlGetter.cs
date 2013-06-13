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
    public class MicroblogUrlGetter : IMicroblogUrlGetter
    {
        /// <summary>
        /// 租户类型id
        /// </summary>
        public string TenantTypeId
        {
            get { return TenantTypeIds.Instance().User(); }
        }

        /// <summary>
        /// 动态拥有者类型
        /// </summary>
        public int ActivityOwnerType
        {
            get { return ActivityOwnerTypes.Instance().User(); }
        }

        /// <summary>
        /// 是否为私有动态
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public bool IsPrivate(long ownerId)
        {
            return false;
        }

        /// <summary>
        /// 微博话题详细页
        /// </summary>
        /// <returns></returns>
        public string TopicDetail(string tagName, long ownerId = 0)
        {
            return SiteUrls.Instance().MicroblogTopic(tagName);
        }

        /// <summary>
        /// 微博详细显示页
        /// </summary>
        /// <param name="spaceKey"></param>
        /// <param name="MicroblogId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public string MicroblogDetail(long microblogId, long? commentId = null)
        {
            MicroblogEntity microblog = new MicroblogService().Get(microblogId);
            if (microblog == null || microblog.User == null)
            {
                return string.Empty;
            }

            return SiteUrls.Instance().ShowMicroblog(microblog.User.UserName, microblogId, commentId);
        }

        /// <summary>
        /// 拥有者名称
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string GetOwnerName(long ownerId)
        {
            var user = new UserService().GetFullUser(ownerId);
            if (user == null)
                return string.Empty;
            return user.DisplayName;
        }

        /// <summary>
        /// 获取拥有者链接
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public string GetOwnerUrl(long ownerId)
        {
            return SiteUrls.Instance().SpaceHome(ownerId);
        }

        /// <summary>
        /// 微博后台导航
        /// </summary>
        /// <returns></returns>
        public string _ManageSubMenu()
        {
            return "~/Applications/Microblog/Views/ControlPanelMicroblog/_ManageMicroblogSideMenuShortcut.cshtml";
        }
    }
}