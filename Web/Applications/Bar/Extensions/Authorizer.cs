//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using Spacebuilder.Common;
using Tunynet;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// Bar的权限验证
    /// </summary>
    public static class AuthorizerExtension
    {
        public static bool BarAttachement_Download(this IUser currentUser, Attachment attachment)
        {

            if (new Authorizer().Attachment_Edit(attachment))
                return true;

            //处理仅允许注册用户下载
            //if (thread.OnlyAllowRegisteredUserDownload && currentUser == null)
            //    return false;

            //处理售价
            if (attachment.Price <= 0)
                return true;
            AttachmentDownloadService attachementDownloadService = new AttachmentDownloadService();
            if (currentUser != null && attachementDownloadService.IsDownloaded(currentUser.UserId, attachment.AttachmentId))
                return true;
            if (currentUser != null && attachment.Price < currentUser.TradePoints)
                return true;
            return false;
        }

        /// <summary>
        /// 是否具有创建BarSection的权限
        /// </summary>        
        /// <returns></returns>
        public static bool BarSection_Create(this Authorizer authorizer)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (authorizer.IsAdministrator(BarConfig.Instance().ApplicationId))
                return true;

            IBarSettingsManager barSettingsManager = DIContainer.Resolve<IBarSettingsManager>();
            BarSettings barSetting = barSettingsManager.Get();
            if (!barSetting.EnableUserCreateSection)
                return false;
            if (currentUser.Rank < barSetting.UserRankOfCreateSection)
                return false;
            return true;
        }

        /// <summary>
        /// 是否拥有设置管理员权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="sectionId">帖吧id</param>
        /// <returns>是否拥有设置管理员的权限</returns>
        public static bool BarSection_SetManager(this Authorizer authorizer, long sectionId)
        {
            return authorizer.BarSection_SetManager(new BarSectionService().Get(sectionId));
        }

        /// <summary>
        /// 是否具有设置吧管理员的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="barSection"></param>
        /// <returns></returns>
        public static bool BarSection_SetManager(this Authorizer authorizer, BarSection barSection)
        {
            if (barSection == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            //吧主
            if (barSection.UserId == currentUser.UserId)
                return true;

            
            
            if (authorizer.IsAdministrator(BarConfig.Instance().ApplicationId))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有管理BarSection的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="barSectionId"></param>
        /// <returns></returns>
        public static bool BarSection_Manage(this Authorizer authorizer, long barSectionId)
        {
            BarSection section = new BarSectionService().Get(barSectionId);
            return BarSection_Manage(authorizer, section);
        }

        /// <summary>
        /// 是否具有管理BarSection的权限
        /// </summary>
        /// <param name="barSection"></param>
        /// <returns></returns>
        public static bool BarSection_Manage(this Authorizer authorizer, BarSection barSection)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (barSection == null)
                return false;

            if (barSection.AuditStatus == AuditStatus.Success)
            {
                //吧主
                if (barSection.UserId == currentUser.UserId)
                    return true;

                //吧管理员
                if (authorizer.AuthorizationService.IsTenantManager(currentUser, barSection.TenantTypeId, barSection.SectionId))
                    return true;
            }

            TenantType tenantType = new TenantTypeService().Get(barSection.TenantTypeId);
            int applicationId = BarConfig.Instance().ApplicationId;
            if (tenantType != null)
                applicationId = tenantType.ApplicationId;
            if (authorizer.IsAdministrator(applicationId))
                return true;

            return false;
        }

        /// <summary>
        /// 是否可以看到该贴吧
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="barSectionId"></param>
        /// <returns></returns>
        public static bool BarSection_View(this Authorizer authorizer, long barSectionId)
        {
            return authorizer.BarSection_View(new BarSectionService().Get(barSectionId));
        }

        /// <summary>
        /// 贴吧显示
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="barSection"></param>
        /// <returns></returns>
        public static bool BarSection_View(this Authorizer authorizer, BarSection barSection)
        {
            if (barSection == null)
                return false;

            if (barSection.AuditStatus == AuditStatus.Success)
                return true;

            if (authorizer.BarSection_Manage(barSection))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有删除BarSection的权限
        /// </summary>
        /// <param name="barSection"></param>
        /// <returns></returns>
        public static bool BarSection_Delete(this Authorizer authorizer)
        {
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (authorizer.IsAdministrator(BarConfig.Instance().ApplicationId))
                return true;

            
            

            ////吧主
            //if (barSection.UserId == currentUser.UserId)
            //    return true;

            return false;
        }

        /// <summary>
        /// 是否具有创建BarThread的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="sectionId">所属帖吧Id</param>
        /// <returns></returns>
        public static bool BarThread_Create(this Authorizer authorizer, long sectionId)
        {
            string errorMessage = string.Empty;
            return authorizer.BarThread_Create(sectionId, out errorMessage);
        }

        /// <summary>
        /// 是否具有创建BarThread的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="sectionId">所属帖吧Id</param>
        /// <param name="errorMessage">无权信息提示</param>
        /// <returns></returns>
        public static bool BarThread_Create(this Authorizer authorizer, long sectionId, out string errorMessage)
        {
            errorMessage = string.Empty;
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
            {
                errorMessage = "您需要先登录，才能发帖";
                return false;
            }
            BarSectionService barSectionService = new BarSectionService();
            var barSection = barSectionService.Get(sectionId);
            if (barSection == null)
            {
                errorMessage = "贴吧不存在";
                return false;
            }

            if (authorizer.BarSection_Manage(barSection))
                return true;

            if (!authorizer.AuthorizationService.Check(currentUser, PermissionItemKeys.Instance().Bar_CreateThread()))
            {
                if (currentUser.IsModerated)
                    errorMessage = Resources.Resource.Description_ModeratedUser_CreateBarThreadDenied;
                return false;
            }
            if (barSection.TenantTypeId == TenantTypeIds.Instance().Bar())
            {
                IBarSettingsManager barSettingsManager = DIContainer.Resolve<IBarSettingsManager>();
                BarSettings barSetting = barSettingsManager.Get();
                if (barSetting.OnlyFollowerCreateThread)
                {
                    SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().BarSection());
                    if (subscribeService.IsSubscribed(sectionId, currentUser.UserId))
                        return true;
                    else
                    {
                        errorMessage = "您需要先关注此帖吧，才能发帖";
                        return false;
                    }
                }
                else
                    return true;
            }
            else
            {
                if (authorizer.AuthorizationService.IsTenantMember(currentUser, barSection.TenantTypeId, barSection.SectionId))
                    return true;
                else
                {
                    TenantType tenantType = new TenantTypeService().Get(barSection.TenantTypeId);
                    errorMessage = string.Format("只有加入{0}才能发帖", tenantType.Name);
                    return false;
                }
            }
        }

        /// <summary>
        /// 是否具有编辑帖子的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="threadId">帖子的id</param>
        /// <returns>是否具有对应的权限</returns>
        public static bool BarThread_Edit(this Authorizer authorizer, long threadId)
        {
            BarThreadService service = new BarThreadService();
            BarThread thread = service.Get(threadId);
            if (thread == null)
                return false;
            return BarThread_Edit(authorizer, thread);
        }

        /// <summary>
        /// body是否有显示的权限（主要是针对于回复可见的情况）
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static bool BarThread_BodyShow(this Authorizer authorizer, long threadId)
        {
            BarThreadService service = new BarThreadService();

            BarPostService barPostService = new Bar.BarPostService();

            BarThread thread = service.Get(threadId);

            if (thread == null)
                return false;

            if (!thread.IsHidden)
                return true;

            if (barPostService.IsPosted(UserContext.CurrentUser == null ? 0 : UserContext.CurrentUser.UserId, threadId))
                return true;

            
            return BarThread_Edit(authorizer, thread);
        }

        /// <summary>
        /// 是否具有编辑BarThread的权限
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static bool BarThread_Edit(this Authorizer authorizer, BarThread thread)
        {
            if (thread == null)
                return false;

            BarSection section = thread.BarSection;
            if (section != null && section.AuditStatus == AuditStatus.Success)
            {
                if (UserContext.CurrentUser == null)
                    return false;
                if (thread.UserId == UserContext.CurrentUser.UserId)
                    return true;
            }

            BarSectionService barSectionService = new BarSectionService();
            if (authorizer.BarSection_Manage(barSectionService.Get(thread.SectionId)))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有管理BarThread的权限
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static bool BarThread_Manage(this Authorizer authorizer, BarThread thread)
        {
            if (thread == null)
                return false;

            BarSectionService barSectionService = new BarSectionService();
            return authorizer.BarSection_Manage(barSectionService.Get(thread.SectionId));
        }

        /// <summary>
        /// 是否具有删除BarThread的权限
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static bool BarThread_Delete(this Authorizer authorizer, long threadId)
        {
            BarThread thread = new BarThreadService().Get(threadId);
            return authorizer.BarThread_Delete(thread);
        }

        /// <summary>
        /// 是否具有删除BarThread的权限
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static bool BarThread_Delete(this Authorizer authorizer, BarThread thread)
        {
            return authorizer.BarThread_Edit(thread);
        }

        /// <summary>
        /// 是否具有创建BarThread的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="sectionId">所属帖吧Id</param>
        /// <returns></returns>
        public static bool BarPost_Create(this Authorizer authorizer, long sectionId)
        {
            string errorMessage = string.Empty;
            return authorizer.BarPost_Create(sectionId, out errorMessage);
        }

        /// <summary>
        /// 是否具有创建BarPost的权限
        /// </summary>
        /// <param name="sectionId">所属帖吧Id</param>
        /// <returns></returns>
        public static bool BarPost_Create(this Authorizer authorizer, long sectionId, out string errorMessage)
        {
            IUser currentUser = UserContext.CurrentUser;
            errorMessage = "没有权限回帖";
            BarSectionService barSectionService = new BarSectionService();
            var barSection = barSectionService.Get(sectionId);
            if (barSection == null)
                return false;

            if (barSection.AuditStatus != AuditStatus.Success)
            {
                errorMessage = "由于贴吧未经过审核，所以不允许发帖";
                return false;
            }

            if (!authorizer.AuthorizationService.Check(currentUser, PermissionItemKeys.Instance().Bar_CreatePost()))
            {
                if (currentUser != null && currentUser.IsModerated)
                    errorMessage = Resources.Resource.Description_ModeratedUser_CreateBarPostDenied;
                return false;
            }

            if (barSection.TenantTypeId == TenantTypeIds.Instance().Bar())
            {
                //检查是否需要是关注用户才能发帖
                IBarSettingsManager barSettingsManager = DIContainer.Resolve<IBarSettingsManager>();
                BarSettings barSetting = barSettingsManager.Get();
                if (barSetting.OnlyFollowerCreatePost)
                {
                    if (currentUser == null)
                    {
                        errorMessage = "您需要先登录并关注此帖吧，才能回帖";
                        return false;
                    }
                    SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().BarSection());
                    bool isSubscribed = subscribeService.IsSubscribed(sectionId, currentUser.UserId);
                    if (!isSubscribed)
                        errorMessage = "您需要先关注此帖吧，才能回帖";
                    return isSubscribed;
                }
            }
            else
            {
                if (authorizer.BarSection_Manage(barSection))
                    return true;
                bool isTenantMember = authorizer.AuthorizationService.IsTenantMember(currentUser, barSection.TenantTypeId, barSection.SectionId);
                if (!isTenantMember)
                    errorMessage = "您需要先加入，才能回帖";
                return isTenantMember;
            }

            //站点设置是否启用了匿名发帖
            ISiteSettingsManager siteSettingsManager = DIContainer.Resolve<ISiteSettingsManager>();
            SiteSettings siteSettings = siteSettingsManager.Get();
            if (siteSettings.EnableAnonymousPosting)
                return true;

            if (currentUser == null)
            {
                errorMessage = "您必须先登录，才能回帖";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否具有编辑BarPost的权限
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public static bool BarPost_Edit(this Authorizer authorizer, BarPost post)
        {
            if (authorizer.IsAdministrator(BarConfig.Instance().ApplicationId))
                return true;

            if (post == null)
                return false;

            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;
            if (post.UserId == currentUser.UserId)
                return true;
            BarSectionService barSectionService = new BarSectionService();
            if (authorizer.BarSection_Manage(barSectionService.Get(post.SectionId)))
                return true;

            return false;
        }

        /// <summary>
        /// 是否具有编辑BarPost的权限
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public static bool BarPost_Edit(this Authorizer authorizer, long postId)
        {
            BarPost post = new BarPostService().Get(postId);
            if (post == null)
                return false;
            return authorizer.BarPost_Edit(post);
        }

        /// <summary>
        /// 是否具有删除BarPost的权限
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public static bool BarPost_Delete(this Authorizer authorizer, BarPost post)
        {
            return authorizer.BarPost_Edit(post);
        }

        /// <summary>
        /// 是否具有评分的权利
        /// </summary>
        /// <param name="authorizer">权限项</param>
        /// <param name="threadId">被评论的帖子ID</param>
        /// <returns>是否具有评分的权利</returns>
        public static bool BarRating(this Authorizer authorizer, long threadId, out string errorMessage)
        {
            return authorizer.BarRating(new BarThreadService().Get(threadId), out  errorMessage);
        }

        /// <summary>
        /// 是否具有评分的权利
        /// </summary>
        /// <param name="authorizer">权限项</param>
        /// <param name="threadId">被评论的帖子ID</param>
        /// <returns>是否具有评分的权利</returns>
        public static bool BarRating(this Authorizer authorizer, long threadId)
        {
            string errorMessage;
            return authorizer.BarRating(new BarThreadService().Get(threadId), out  errorMessage);
        }

        /// <summary>
        /// 是否拥有平分的权限
        /// </summary>
        /// <param name="authorizer"></param>
        /// <param name="thread">被评分的帖子</param>
        /// <returns>是否允许评分</returns>
        public static bool BarRating(this Authorizer authorizer, BarThread thread)
        {
            string errorMessage;
            return authorizer.BarRating(thread, out errorMessage);
        }

        /// <summary>
        /// 是否具有评分的权限
        /// </summary>
        /// <returns></returns>
        public static bool BarRating(this Authorizer authorizer, BarThread thread, out string errorMessage)
        {
            BarSettings barSettings = DIContainer.Resolve<IBarSettingsManager>().Get();
            errorMessage = "没有找到对应的帖子";
            if (thread == null)
                return false;
            errorMessage = "您还没有登录";
            IUser currentUser = UserContext.CurrentUser;
            if (currentUser == null)
                return false;

            if (thread.UserId == currentUser.UserId)
            {
                errorMessage = "您不可以给自己的帖子评分哦";
                return false;
            }

            BarRatingService barRatingService = new BarRatingService();
            //是否已经评过分
            errorMessage = "您已经评论过此贴";
            if (barRatingService.IsRated(currentUser.UserId, thread.ThreadId))
                return false;
            errorMessage = "您的剩余积分不够了哦";
            if (barRatingService.GetUserTodayRatingSum(UserContext.CurrentUser.UserId) + barSettings.ReputationPointsMinValue > barSettings.UserReputationPointsPerDay)
                return false;

            IBarSettingsManager barSettingsManager = DIContainer.Resolve<IBarSettingsManager>();
            BarSettings barSetting = barSettingsManager.Get();
            BarSectionService barSectionService = new BarSectionService();
            var barSection = barSectionService.Get(thread.SectionId);
            if (barSection == null)
                return false;
            if (barSection.TenantTypeId == TenantTypeIds.Instance().Bar())
            {

                errorMessage = "此帖吧仅允许关注的用户评分哦";
                if (barSetting.OnlyFollowerCreatePost)
                {
                    SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().BarSection());
                    return subscribeService.IsSubscribed(thread.SectionId, currentUser.UserId);
                }
            }
            else
            {
                if (authorizer.AuthorizationService.IsTenantMember(currentUser, barSection.TenantTypeId, barSection.SectionId))
                    return true;
            }

            errorMessage = "站点没有开启帖子评分";
            if (!barSetting.EnableRating)
                return false;
            return true;
        }
    }
}
