//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Events;
using Spacebuilder.Search;
using Tunynet.Common;
using Spacebuilder.Bar;
using System.Collections.Generic;
using System.Linq;
using Tunynet.Globalization;
using Tunynet;
using Spacebuilder.Group;
using Spacebuilder.Common;
using Tunynet.Utilities;

namespace Spacebuilder.Bar.EventModules
{
    /// <summary>
    /// 处理帖子动态、积分的EventMoudle
    /// </summary>
    public class BarThreadEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<BarThread, AuditEventArgs>.Instance().After += new CommonEventHandler<BarThread, AuditEventArgs>(BarThreadActivityModule_After);
            EventBus<BarThread, AuditEventArgs>.Instance().After += new CommonEventHandler<BarThread, AuditEventArgs>(BarThreadPointModule_After);
            EventBus<BarThread>.Instance().After += new CommonEventHandler<BarThread, CommonEventArgs>(BarThreadPointModuleForManagerOperation_After);
            EventBus<GroupEntity>.Instance().Before += new CommonEventHandler<GroupEntity, CommonEventArgs>(DeleteGroup_Before);
        }

        /// <summary>
        /// 动态处理程序
        /// </summary>
        /// <param name="barThread"></param>
        /// <param name="eventArgs"></param>
        private void BarThreadActivityModule_After(BarThread barThread, AuditEventArgs eventArgs)
        {
            
            //1、通过审核的内容才生成动态；（不满足）
            //2、把通过审核的内容设置为未通过审核或者删除内容，需要移除动态；（不满足）
            //3、把未通过审核的内容通过审核，需要添加动态； （不满足）
            //4、详见动态需求说明
            
            

            
            
            

            //生成动态
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //生成动态
            {
                if (barThread.BarSection == null)
                    return;

                var barUrlGetter = BarUrlGetterFactory.Get(barThread.TenantTypeId);
                if (barUrlGetter == null)
                    return;

                //生成Owner为帖吧的动态
                Activity actvityOfBar = Activity.New();
                actvityOfBar.ActivityItemKey = ActivityItemKeys.Instance().CreateBarThread();
                actvityOfBar.ApplicationId = BarConfig.Instance().ApplicationId;

                AttachmentService attachmentService = new AttachmentService(TenantTypeIds.Instance().BarThread());
                IEnumerable<Attachment> attachments = attachmentService.GetsByAssociateId(barThread.ThreadId);
                if (attachments != null && attachments.Any(n => n.MediaType == MediaType.Image))
                    actvityOfBar.HasImage = true;

                actvityOfBar.IsOriginalThread = true;
                actvityOfBar.IsPrivate = false;
                actvityOfBar.UserId = barThread.UserId;
                actvityOfBar.ReferenceId = 0;//没有涉及到的实体
                actvityOfBar.ReferenceTenantTypeId = string.Empty;
                actvityOfBar.SourceId = barThread.ThreadId;
                actvityOfBar.TenantTypeId = TenantTypeIds.Instance().BarThread();
                actvityOfBar.OwnerId = barThread.SectionId;
                actvityOfBar.OwnerName = barThread.BarSection.Name;
                actvityOfBar.OwnerType = barUrlGetter.ActivityOwnerType;
                activityService.Generate(actvityOfBar, false);

                if (!barUrlGetter.IsPrivate(barThread.SectionId))
                {
                    //生成Owner为用户的动态
                    Activity actvityOfUser = Activity.New();
                    actvityOfUser.ActivityItemKey = actvityOfBar.ActivityItemKey;
                    actvityOfUser.ApplicationId = actvityOfBar.ApplicationId;
                    actvityOfUser.HasImage = actvityOfBar.HasImage;
                    actvityOfUser.HasMusic = actvityOfBar.HasMusic;
                    actvityOfUser.HasVideo = actvityOfBar.HasVideo;
                    actvityOfUser.IsOriginalThread = actvityOfBar.IsOriginalThread;
                    actvityOfUser.IsPrivate = actvityOfBar.IsPrivate;
                    actvityOfUser.UserId = actvityOfBar.UserId;
                    actvityOfUser.ReferenceId = actvityOfBar.ReferenceId;
                    actvityOfUser.SourceId = actvityOfBar.SourceId;

                    actvityOfUser.TenantTypeId = actvityOfBar.TenantTypeId;
                    actvityOfUser.OwnerId = barThread.UserId;
                    actvityOfUser.OwnerName = barThread.User.DisplayName;
                    actvityOfUser.OwnerType = ActivityOwnerTypes.Instance().User();
                    activityService.Generate(actvityOfUser, false);
                }
            }
            else if (auditDirection == false) //删除动态
            {
                activityService.DeleteSource(TenantTypeIds.Instance().BarThread(), barThread.ThreadId);
            }
        }

        /// <summary>
        /// 审核状态发生变化时处理积分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void BarThreadPointModule_After(BarThread sender, AuditEventArgs eventArgs)
        {
            string pointItemKey = string.Empty;
            string eventOperationType = string.Empty;
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //加积分
            {
                pointItemKey = PointItemKeys.Instance().Bar_CreateThread();
                if (eventArgs.OldAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Create();
                else
                    eventOperationType = EventOperationType.Instance().Approved();
            }
            else if (auditDirection == false) //减积分
            {
                pointItemKey = PointItemKeys.Instance().Bar_DeleteThread();
                if (eventArgs.NewAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Delete();
                else
                    eventOperationType = EventOperationType.Instance().Disapproved();
            }
            if (!string.IsNullOrEmpty(pointItemKey))
            {
                PointService pointService = new PointService();

                
                
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "帖子", sender.Subject);
                pointService.GenerateByRole(sender.UserId, pointItemKey, description);
            }
        }

        /// <summary>
        /// 处理加精、置顶等操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void BarThreadPointModuleForManagerOperation_After(BarThread sender, CommonEventArgs eventArgs)
        {
            NoticeService noticeService = new NoticeService();
            string pointItemKey = string.Empty;
            if (eventArgs.EventOperationType == EventOperationType.Instance().SetEssential())
            {
                pointItemKey = PointItemKeys.Instance().EssentialContent();
                if (sender.UserId > 0)
                {
                    Notice notice = Notice.New();
                    notice.UserId = sender.UserId;
                    notice.ApplicationId = BarConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActor = sender.Author;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId)));
                    notice.RelativeObjectName = HtmlUtility.TrimHtml(sender.Subject, 64);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().ThreadDetail(sender.ThreadId));
                    notice.TemplateName = NoticeTemplateNames.Instance().ManagerSetEssential();
                    noticeService.Create(notice);
                }
            }
            else if (eventArgs.EventOperationType == EventOperationType.Instance().SetSticky())
            {
                pointItemKey = PointItemKeys.Instance().StickyContent();
                if (sender.UserId > 0)
                {
                    Notice notice = Notice.New();
                    notice.UserId = sender.UserId;
                    notice.ApplicationId = BarConfig.Instance().ApplicationId;
                    notice.TypeId = NoticeTypeIds.Instance().Hint();
                    notice.LeadingActor = sender.Author;
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(sender.UserId)));
                    notice.RelativeObjectName = HtmlUtility.TrimHtml(sender.Subject, 64);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(SiteUrls.Instance().ThreadDetail(sender.ThreadId));
                    notice.TemplateName = NoticeTemplateNames.Instance().ManagerSetSticky();
                    noticeService.Create(notice);
                }
            }

            if (!string.IsNullOrEmpty(pointItemKey))
            {
                PointService pointService = new PointService();
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventArgs.EventOperationType), "帖子", sender.Subject);
                pointService.GenerateByRole(sender.UserId, pointItemKey, description);


            }
        }

        /// <summary>
        /// 删除群组是删除贴吧相关的
        /// </summary>
        /// <param name="group"></param>
        /// <param name="eventArgs"></param>
        private void DeleteGroup_Before(GroupEntity group, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                BarPostService barPostService = new BarPostService();
                BarThreadService barThreadService = new BarThreadService();
                PagingDataSet<BarThread> barThreads = barThreadService.Gets(group.GroupId);

                new BarSectionService().Delete(group.GroupId);
                barThreadService.DeletesBySectionId(group.GroupId);
                foreach (var barThread in barThreads)
                {
                    barPostService.DeletesByThreadId(barThread.ThreadId);
                }
            }
        }
    }
}