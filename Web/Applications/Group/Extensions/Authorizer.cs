//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 扩展权限方法
    /// </summary>
    public static class AuthorizerGroupExtension
    {
        /// <summary>
        /// 是否具有创建Group的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <returns></returns>
        public static bool Group_Create(this Authorizer authorizer)
        {
            string errorMessage = string.Empty;
            return authorizer.Group_Create(out errorMessage);
        }

        /// <summary>
        /// 是否具有创建Group的权限
        /// </summary>        
        /// <param name="authorizer"></param>
        /// <param name="errorMessage">无权信息提示</param>
        /// <returns></returns>
        public static bool Group_Create(this Authorizer authorizer, out string errorMessage)
        {
            errorMessage = string.Empty;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                errorMessage = "您需要先登录，才能创建群组";
                return false;
            }

            if (authorizer.IsAdministrator(GroupConfig.Instance().ApplicationId))
                return true;

            if (currentUser.Rank < GroupConfig.Instance().MinUserRankOfCreateGroup)
            {
                errorMessage = string.Format("只有等级达到{0}级，才能创建群组，您现在的等级是{1}", GroupConfig.Instance().MinUserRankOfCreateGroup, currentUser.Rank);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否拥有设置管理员权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="groupId">帖群id</param>
        /// <returns>是否拥有设置管理员的权限</returns>
        public static bool Group_SetManager(this Authorizer authorizer, long groupId)
        {
            return authorizer.Group_SetManager(new GroupService().Get(groupId));
        }

        /// <summary>
        /// 是否具有设置群管理员的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool Group_SetManager(this Authorizer authorizer, GroupEntity group)
        {
            if (group == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            //群主
            if (group.UserId == currentUser.UserId)
                return true;

            if (authorizer.IsAdministrator(GroupConfig.Instance().ApplicationId))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有管理Group的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static bool Group_Manage(this Authorizer authorizer, long groupId)
        {
            GroupEntity group = new GroupService().Get(groupId);
            return Group_Manage(authorizer, group);
        }

        /// <summary>
        /// 是否具有删除访客记录的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool Group_DeleteVisitor(this Authorizer authorizer, long groupId, long userId)
        {
            bool result = false;
            GroupEntity group = new GroupService().Get(groupId);
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser != null && currentUser.UserId == userId)
            {
                result = true;
            }
            else
            {
                result = Group_Manage(authorizer, group);
            }
            return result;
        }
        /// <summary>
        /// 是否拥有删除群组成员的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="group"></param>
        /// <param name="userId">被删除的用户Id</param>
        /// <returns>是否拥有删除群组成员的权限</returns>
        public static bool Group_DeleteMember(this Authorizer authorizer, GroupEntity group, long userId)
        {
            if (group == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            //群主
            if (group.UserId == currentUser.UserId)
                return true;

            if (authorizer.IsAdministrator(GroupConfig.Instance().ApplicationId))
                return true;
            GroupService groupService = new GroupService();
            //群管理员
            if (groupService.IsManager(group.GroupId, currentUser.UserId) && !groupService.IsManager(group.GroupId, userId))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 是否具有删除群组动态的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool Group_DeleteGroupActivity(this Authorizer authorizer, Activity activity)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            if (authorizer.Group_Manage(activity.OwnerId))
                return true;
            if (currentUser.UserId == activity.UserId)
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有管理Group的权限
        /// </summary>
        /// <param name="Group"></param>
        /// <returns></returns>
        public static bool Group_Manage(this Authorizer authorizer, GroupEntity group)
        {
            
            
            if (group == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (authorizer.IsAdministrator(GroupConfig.Instance().ApplicationId))
                return true;

            if (currentUser.IsInRoles("ContentAdministrator"))
                return true;

            //群主
            if (group.UserId == currentUser.UserId)
                return true;

            GroupService groupService = new GroupService();
            //群管理员
            if (groupService.IsManager(group.GroupId, currentUser.UserId))
                return true;

            return false;
        }


        /// <summary>
        /// 是否具有邀请好友加入群组的权限
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool Group_Invite(this Authorizer authorizer, GroupEntity group)
        {
            if (group == null)
                return false;
            if (UserContext.CurrentUser == null)
                return false;

            GroupService groupService = new GroupService();
            if (authorizer.Group_Manage(group))
                return true;
            if (group.EnableMemberInvite && groupService.IsMember(group.GroupId, UserContext.CurrentUser.UserId))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有浏览群组的权限
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool Group_View(this Authorizer authorizer, GroupEntity group)
        {
            if (group == null)
                return false;

            if (group.AuditStatus == AuditStatus.Success)
            {
                if (group.IsPublic)
                    return true;

                if (UserContext.CurrentUser == null)
                    return false;

                if (authorizer.Group_Manage(group))
                    return true;

                GroupService groupService = new GroupService();
                if (groupService.IsMember(group.GroupId, UserContext.CurrentUser.UserId))
                    return true;
            }

            if (authorizer.IsAdministrator(GroupConfig.Instance().ApplicationId))
                return true;

            return false;
        }

    }
}