//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using Spacebuilder.Bar;
using Tunynet.Globalization;
using Tunynet.Utilities;
using Spacebuilder.Common;
using System.Collections.Generic;
using System.Linq;
using Tunynet;

namespace Spacebuilder.Bar.EventModules
{

    /// <summary>
    /// 处理帖子动态、积分的EventMoudle
    /// </summary>
    public class BarPostEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册EventHandler
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<BarPost, AuditEventArgs>.Instance().After += new CommonEventHandler<BarPost, AuditEventArgs>(BarPostActivityModule_After);
            EventBus<BarPost, AuditEventArgs>.Instance().After += new CommonEventHandler<BarPost, AuditEventArgs>(BarPostPointModule_After);
            EventBus<BarPost, AuditEventArgs>.Instance().After += new CommonEventHandler<BarPost, AuditEventArgs>(BarPostNoticeModule_After);
        }

        /// <summary>
        /// 动态处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void BarPostActivityModule_After(BarPost sender, AuditEventArgs eventArgs)
        {
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //生成动态
            {
                BarThreadService barThreadService = new BarThreadService();
                BarThread barThread = barThreadService.Get(sender.ThreadId);
                if (barThread == null)
                    return;
                if (sender.UserId == barThread.UserId)
                    return;
                var barUrlGetter = BarUrlGetterFactory.Get(barThread.TenantTypeId);
                if (barUrlGetter == null)
                    return;
                if (sender.ParentId > 0)
                {
                    BarPost parentPost = new BarPostService().Get(sender.ParentId);
                    if (parentPost == null)
                        return;
                    if (parentPost.UserId == sender.UserId)
                        return;
                }
                Activity actvity = Activity.New();
                actvity.ActivityItemKey = ActivityItemKeys.Instance().CreateBarPost();
                actvity.ApplicationId = BarConfig.Instance().ApplicationId;
                //仅一级回复可以上传附件
                if (sender.ParentId == 0)
                {
                    AttachmentService attachmentService = new AttachmentService(TenantTypeIds.Instance().BarPost());
                    IEnumerable<Attachment> attachments = attachmentService.GetsByAssociateId(sender.PostId);
                    if (attachments != null && attachments.Any(n => n.MediaType == MediaType.Image))
                        actvity.HasImage = true;
                    //actvity.HasMusic = barThread.HasMusic;
                    //actvity.HasVideo = barThread.HasVideo;
                }

                actvity.IsOriginalThread = true;
                actvity.IsPrivate = barUrlGetter.IsPrivate(barThread.SectionId);
                actvity.OwnerId = barThread.SectionId;
                actvity.OwnerName = barThread.BarSection.Name;
                actvity.OwnerType = barUrlGetter.ActivityOwnerType;
                actvity.ReferenceId = barThread.ThreadId;
                actvity.ReferenceTenantTypeId = TenantTypeIds.Instance().BarThread();
                actvity.SourceId = sender.PostId;
                actvity.TenantTypeId = TenantTypeIds.Instance().BarPost();
                actvity.UserId = sender.UserId;

                //创建从属内容，不向自己的动态收件箱推送动态
                activityService.Generate(actvity, false);
            }
            else if (auditDirection == false) //删除动态
            {
                activityService.DeleteSource(TenantTypeIds.Instance().BarPost(), sender.PostId);
            }
        }

        /// <summary>
        /// 积分处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void BarPostPointModule_After(BarPost sender, AuditEventArgs eventArgs)
        {
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);

            string pointItemKey = string.Empty;
            string eventOperationType = string.Empty;

            if (auditDirection == true) //加积分
            {
                pointItemKey = PointItemKeys.Instance().CreateComment();
                if (eventArgs.OldAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Create();
                else
                    eventOperationType = EventOperationType.Instance().Approved();
            }
            else if (auditDirection == false) //减积分
            {
                pointItemKey = PointItemKeys.Instance().DeleteComment();
                if (eventArgs.NewAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Delete();
                else
                    eventOperationType = EventOperationType.Instance().Disapproved();
            }

            if (!string.IsNullOrEmpty(pointItemKey))
            {
                PointService pointService = new PointService();
                
                
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "回帖", HtmlUtility.TrimHtml(sender.GetBody(), 32));
                pointService.GenerateByRole(sender.UserId, pointItemKey, description);
            }
        }

        /// <summary>
        /// 通知处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void BarPostNoticeModule_After(BarPost sender, AuditEventArgs eventArgs)
        {
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //创建回帖发通知
            {
                BarThreadService barThreadService = new BarThreadService();
                BarThread barThread = barThreadService.Get(sender.ThreadId);
                if (barThread == null)
                    return;

                long toUserId = barThread.UserId;

                //自己给自己的帖子进行回帖，不必通知
                if (sender.UserId == barThread.UserId)
                    return;

                if (sender.ParentId > 0)
                {
                    BarPostService barPostService = new BarPostService();
                    BarPost parentPost = barPostService.Get(sender.ParentId);

                    if (parentPost == null || (parentPost.UserId == sender.UserId))
                        return;
                    toUserId = parentPost.UserId;
                }

                NoticeService noticeService = Tunynet.DIContainer.Resolve<NoticeService>();
                Notice notice = Notice.New();

                notice.UserId = toUserId;
                notice.ApplicationId = BarConfig.Instance().ApplicationId;
                notice.TypeId = NoticeTypeIds.Instance().Reply();
                notice.LeadingActorUserId = sender.UserId;
                notice.LeadingActor = sender.Author;
                notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId));
                notice.RelativeObjectId = sender.PostId;
                notice.RelativeObjectName = HtmlUtility.TrimHtml(barThread.Subject, 64);
                notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().ThreadDetailGotoPost(sender.PostId));
                notice.TemplateName = NoticeTemplateNames.Instance().NewReply();
                noticeService.Create(notice);
            }
        }
    }
}