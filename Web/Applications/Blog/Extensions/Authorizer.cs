//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------


using Spacebuilder.Common;
using Tunynet;
using Tunynet.Common;
using System.Collections.Generic;
using System.Linq;

namespace Spacebuilder.Blog
{
    /// <summary>
    /// 日志权限验证
    /// </summary>
    public static class AuthorizerExtension
    {
        /// <summary>
        /// 撰写日志/转载日志
        /// 空间主人撰写空间用户的日志
        /// </summary>
        public static bool BlogThread_Create(this Authorizer authorizer, string spaceKey)
        {
            string errorMessage = string.Empty;
            return authorizer.BlogThread_Create(spaceKey, out errorMessage);
        }

        /// <summary>
        /// 撰写日志/转载日志
        /// 空间主人撰写空间用户的日志
        /// </summary>
        public static bool BlogThread_Create(this Authorizer authorizer, string spaceKey, out string errorMessage)
        {
            IUser currentUser = UserContext.CurrentUser;
            errorMessage = "没有权限写日志";

            if (currentUser == null)
            {
                errorMessage = "您必须先登录，才能写日志";
                return false;
            }

            bool result = spaceKey == currentUser.UserName && authorizer.AuthorizationService.Check(currentUser, PermissionItemKeys.Instance().Blog_Create());
            if (!result && currentUser.IsModerated)
                errorMessage = Resources.Resource.Description_ModeratedUser_CreateBlogThreadDenied;
            return result;
        }

        /// <summary>
        /// 编辑日志
        /// 空间主人或管理员可以编辑空间用户的日志（置顶也使用该规则）
        /// </summary>
        public static bool BlogThread_Edit(this Authorizer authorizer, BlogThread blogThread)
        {
            IUser currentUser = UserContext.CurrentUser;

            if (currentUser == null)
            {
                return false;
            }

            if (blogThread.UserId == currentUser.UserId || authorizer.IsAdministrator(BlogConfig.Instance().ApplicationId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除日志
        /// 空间主人或管理员可以删除空间用户的日志
        /// </summary>
        public static bool BlogThread_Delete(this Authorizer authorizer, BlogThread blogThread)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return false;
            }

            if (blogThread.UserId == currentUser.UserId || authorizer.IsAdministrator(BlogConfig.Instance().ApplicationId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 查看日志
        /// 仅自己可见的只能是日志作者或管理员可以查看
        /// 部分可见的只能是日志作者、指定可见的用户或管理员可以查看
        /// </summary>
        public static bool BlogThread_View(this Authorizer authorizer, BlogThread blogThread)
        {
            if (blogThread.PrivacyStatus == PrivacyStatus.Public)
            {
                return true;
            }

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                return false;
            }

            if (blogThread.UserId == currentUser.UserId || authorizer.IsAdministrator(BlogConfig.Instance().ApplicationId))
            {
                return true;
            }

            if (blogThread.PrivacyStatus == PrivacyStatus.Private)
            {
                return false;
            }

            ContentPrivacyService contentPrivacyService = new ContentPrivacyService();
            if (contentPrivacyService.Validate(blogThread, currentUser.UserId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 管理日志
        /// 管理员可以管理所有日志，精华、管理员推荐也使用该规则
        /// </summary>
        public static bool BlogThread_Manage(this Authorizer authorizer)
        {
            if (authorizer.IsAdministrator(BlogConfig.Instance().ApplicationId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 关注日志
        /// 必须是登录用户
        /// </summary>
        public static bool BlogThread_Follow(this Authorizer authorizer)
        {
            if (UserContext.CurrentUser != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 评论日志
        /// 1.锁定的日志不允许评论；
        /// 2.对于匿名用户，根据站点匿名用户；
        /// </summary>
        public static bool BlogComment_Create(this Authorizer authorizer, BlogThread blogThread)
        {
            //锁定的日志不允许评论
            if (blogThread.IsLocked)
            {
                return false;
            }

            //允许登录用户
            if (UserContext.CurrentUser != null)
            {
                return true;
            }

            //判断是否允许匿名用户评论
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();

            return siteSettings.EnableAnonymousPosting;
        }
    }
}