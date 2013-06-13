//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Tunynet.Common;
using Tunynet.Events;
using Tunynet.Globalization;
using Spacebuilder.Common;
using Tunynet.Utilities;

namespace Spacebuilder.Blog.EventModules
{
    /// <summary>
    /// 处理日志动态、积分的EventMoudle
    /// </summary>
    public class BlogThreadEventModule : IEventMoudle
    {
        private BlogService blogService = new BlogService();
        private PointService pointService = new PointService();
        private OwnerDataService ownerDataService = new OwnerDataService(TenantTypeIds.Instance().User());
        private SubscribeService subscribeService = new SubscribeService(TenantTypeIds.Instance().BlogThread());

        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void IEventMoudle.RegisterEventHandler()
        {
            EventBus<BlogThread, AuditEventArgs>.Instance().After += new CommonEventHandler<BlogThread, AuditEventArgs>(BlogThreadActivityModule_After);
            EventBus<BlogThread, AuditEventArgs>.Instance().After += new CommonEventHandler<BlogThread, AuditEventArgs>(BlogThreadPointModule_After);
            EventBus<BlogThread>.Instance().After += new CommonEventHandler<BlogThread, CommonEventArgs>(BlogThreadPointModuleForManagerOperation_After);
            EventBus<BlogThread>.Instance().After += new CommonEventHandler<BlogThread, CommonEventArgs>(BlogThreadAcitivityPrivicyChangeEventModule_After);
            EventBus<Comment, AuditEventArgs>.Instance().After += new CommonEventHandler<Comment, AuditEventArgs>(BlogCommentActivityEventModule_After);
        }

        /// <summary>
        /// 动态处理程序
        /// </summary>
        /// <param name="blogThread"></param>
        /// <param name="eventArgs"></param>
        private void BlogThreadActivityModule_After(BlogThread blogThread, AuditEventArgs eventArgs)
        {
            //生成动态
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();

            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);

            if (auditDirection == true)
            {
                //初始化Owner为用户的动态
                Activity activityOfUser = Activity.New();
                activityOfUser.ActivityItemKey = ActivityItemKeys.Instance().CreateBlogThread();
                activityOfUser.ApplicationId = BlogConfig.Instance().ApplicationId;

                //判断是否有图片、音频、视频
                AttachmentService attachmentService = new AttachmentService(TenantTypeIds.Instance().BlogThread());
                IEnumerable<Attachment> attachments = attachmentService.GetsByAssociateId(blogThread.ThreadId);
                if (attachments != null && attachments.Any(n => n.MediaType == MediaType.Image))
                {
                    activityOfUser.HasImage = true;
                }

                activityOfUser.HasMusic = false;
                activityOfUser.HasVideo = false;
                activityOfUser.IsOriginalThread = !blogThread.IsReproduced;
                activityOfUser.IsPrivate = blogThread.PrivacyStatus != PrivacyStatus.Public ? true : false;
                activityOfUser.UserId = blogThread.UserId;
                activityOfUser.ReferenceId = 0;
                activityOfUser.ReferenceTenantTypeId = string.Empty;
                activityOfUser.SourceId = blogThread.ThreadId;
                activityOfUser.TenantTypeId = TenantTypeIds.Instance().BlogThread();
                activityOfUser.OwnerId = blogThread.UserId;
                activityOfUser.OwnerName = blogThread.Author;
                activityOfUser.OwnerType = ActivityOwnerTypes.Instance().User();
                
                
                //生成动态
                activityService.Generate(activityOfUser, true);
            }
            //删除动态
            else if (auditDirection == false)
            {
                activityService.DeleteSource(TenantTypeIds.Instance().BlogThread(), blogThread.ThreadId);
            }
        }

        /// <summary>
        /// 评论日志动态处理程序
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="eventArgs"></param>
        private void BlogCommentActivityEventModule_After(Comment comment, AuditEventArgs eventArgs)
        {
            NoticeService noticeService = new NoticeService();
            BlogThread blogThread = null;

            if (comment.TenantTypeId == TenantTypeIds.Instance().BlogThread())
            {
                //生成动态
                ActivityService activityService = new ActivityService();
                AuditService auditService = new AuditService();
                bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
                if (auditDirection == true)
                {
                    //创建评论的动态[关注评论者的粉丝可以看到该评论]
                    Activity activity = Activity.New();
                    activity.ActivityItemKey = ActivityItemKeys.Instance().CreateBlogComment();
                    activity.ApplicationId = BlogConfig.Instance().ApplicationId;

                    BlogService blogService = new BlogService();
                    blogThread = blogService.Get(comment.CommentedObjectId);
                    if (blogThread == null || blogThread.UserId == comment.UserId)
                    {
                        return;
                    }
                    activity.IsOriginalThread = true;
                    activity.IsPrivate = false;
                    activity.OwnerId = comment.UserId;
                    activity.OwnerName = comment.Author;
                    activity.OwnerType = ActivityOwnerTypes.Instance().User();
                    activity.ReferenceId = blogThread.ThreadId;
                    activity.ReferenceTenantTypeId = TenantTypeIds.Instance().BlogThread();
                    activity.SourceId = comment.Id;
                    activity.TenantTypeId = TenantTypeIds.Instance().Comment();
                    activity.UserId = comment.UserId;

                    activityService.Generate(activity, false);

                    //创建评论的动态[关注该日志的用户可以看到该评论]
                    Activity activityOfBlogComment = Activity.New();
                    activityOfBlogComment.ActivityItemKey = activity.ActivityItemKey;
                    activityOfBlogComment.ApplicationId = activity.ApplicationId;
                    activityOfBlogComment.IsOriginalThread = activity.IsOriginalThread;
                    activityOfBlogComment.IsPrivate = activity.IsPrivate;
                    activityOfBlogComment.ReferenceId = activity.ReferenceId;
                    activityOfBlogComment.ReferenceTenantTypeId = activity.ReferenceTenantTypeId;
                    activityOfBlogComment.SourceId = activity.SourceId;
                    activityOfBlogComment.TenantTypeId = activity.TenantTypeId;
                    activityOfBlogComment.UserId = activity.UserId;

                    activityOfBlogComment.OwnerId = blogThread.ThreadId;
                    activityOfBlogComment.OwnerName = blogThread.ResolvedSubject;
                    activityOfBlogComment.OwnerType = ActivityOwnerTypes.Instance().Blog();

                    activityService.Generate(activityOfBlogComment, false);
                }
                else if (auditDirection == false)
                {
                    activityService.DeleteSource(TenantTypeIds.Instance().Comment(), comment.Id);
                }
            }
        }

        /// <summary>
        /// 审核状态发生变化时处理积分
        /// </summary>
        /// <param name="blogThread">日志</param>
        /// <param name="eventArgs">事件</param>
        private void BlogThreadPointModule_After(BlogThread blogThread, AuditEventArgs eventArgs)
        {
            AuditService auditService = new AuditService();

            string pointItemKey = string.Empty;
            string eventOperationType = string.Empty;

            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);

            if (auditDirection == true) //加积分
            {
                pointItemKey = PointItemKeys.Instance().Blog_CreateThread();
                if (eventArgs.OldAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Create();
                else
                    eventOperationType = EventOperationType.Instance().Approved();
            }
            else if (auditDirection == false) //减积分
            {
                pointItemKey = PointItemKeys.Instance().Blog_DeleteThread();
                if (eventArgs.NewAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Delete();
                else
                    eventOperationType = EventOperationType.Instance().Disapproved();
            }

            if (!string.IsNullOrEmpty(pointItemKey))
            {
                PointService pointService = new PointService();

                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "日志", blogThread.Subject);
                pointService.GenerateByRole(blogThread.UserId, pointItemKey, description);
            }
        }

        /// <summary>
        /// 处理加精操作加积分
        /// </summary>
        /// <param name="blogThread">日志</param>
        /// <param name="eventArgs">事件</param>
        private void BlogThreadPointModuleForManagerOperation_After(BlogThread blogThread, CommonEventArgs eventArgs)
        {
            NoticeService noticeService = new NoticeService();
            string pointItemKey = string.Empty;
            if (eventArgs.EventOperationType == EventOperationType.Instance().SetEssential())
            {
                pointItemKey = PointItemKeys.Instance().EssentialContent();

                PointService pointService = new PointService();
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventArgs.EventOperationType), "日志", blogThread.ResolvedSubject);
                pointService.GenerateByRole(blogThread.UserId, pointItemKey, description);
                if (blogThread.UserId > 0)
                {
                    Notice notice = Notice.New();
                    notice.UserId = blogThread.UserId;
                    notice.ApplicationId = BlogConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActor = blogThread.Author;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(blogThread.UserId)));
                    notice.RelativeObjectName = HtmlUtility.TrimHtml(blogThread.Subject, 64);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().BlogDetail(blogThread.User.UserName, blogThread.ThreadId));
                    notice.TemplateName = NoticeTemplateNames.Instance().ManagerSetEssential();
                    noticeService.Create(notice);
                }
            }
        }

        /// <summary>
        /// 隐私状态发生变化时，同时更新动态的私有状态
        /// </summary>
        /// <param name="blogThread">日志</param>
        /// <param name="eventArgs">事件</param>
        private void BlogThreadAcitivityPrivicyChangeEventModule_After(BlogThread blogThread, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create() ||
            eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                ActivityService activityService = new ActivityService();
                Activity activity = activityService.Get(TenantTypeIds.Instance().BlogThread(), blogThread.ThreadId);
                if (activity == null)
                {
                    return;
                }

                bool newIsPrivate = blogThread.PrivacyStatus != PrivacyStatus.Public;
                if (activity.IsPrivate != newIsPrivate)
                {
                    activityService.UpdatePrivateStatus(activity.ActivityId, newIsPrivate);
                }
            }
        }
    }
}