//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;
using Tunynet.Events;
using Tunynet.Globalization;
using Tunynet.Utilities;
using Spacebuilder.Group;

namespace Spacebuilder.Microblog.EventModules
{
    /// <summary>
    /// 处理微博相关事件处理程序
    /// </summary>
    public class MicroblogEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序 
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<MicroblogEntity, AuditEventArgs>.Instance().After += new CommonEventHandler<MicroblogEntity, AuditEventArgs>(MicroblogActivityModule_After);
            EventBus<MicroblogEntity, AuditEventArgs>.Instance().After += new CommonEventHandler<MicroblogEntity, AuditEventArgs>(MicroblogPointModule_After);
        }

        /// <summary>
        /// 微博动态
        /// </summary>
        /// <param name="sender">微博实体</param>
        /// <param name="eventArgs">事件参数</param>
        private void MicroblogActivityModule_After(MicroblogEntity sender, AuditEventArgs eventArgs)
        {
            ActivityService activityService = new ActivityService();
            bool? auditDirection = new AuditService().ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);

            if (auditDirection == true)
            {
                var microblogUrlGetter = MicroblogUrlGetterFactory.Get(sender.TenantTypeId);
                if (microblogUrlGetter == null)
                    return;

                Activity actvity = Activity.New();
                actvity.ActivityItemKey = ActivityItemKeys.Instance().CreateMicroblog();
                actvity.ApplicationId = MicroblogConfig.Instance().ApplicationId;
                actvity.HasImage = sender.HasPhoto;
                actvity.HasMusic = sender.HasMusic;
                actvity.HasVideo = sender.HasVideo;
                actvity.IsOriginalThread = !sender.IsForward;
                actvity.IsPrivate = false;
                actvity.SourceId = sender.MicroblogId;
                actvity.TenantTypeId = TenantTypeIds.Instance().Microblog();
                actvity.UserId = sender.UserId;
                //生成Owner为用户的动态
                if (microblogUrlGetter.ActivityOwnerType == ActivityOwnerTypes.Instance().User())
                {
                    Activity actvityOfUser = Activity.New();
                    actvityOfUser.ActivityItemKey = actvity.ActivityItemKey;
                    actvityOfUser.ApplicationId = actvity.ApplicationId;
                    actvityOfUser.HasImage = actvity.HasImage;
                    actvityOfUser.HasMusic = actvity.HasMusic;
                    actvityOfUser.HasVideo = actvity.HasVideo;
                    actvityOfUser.IsOriginalThread = actvity.IsOriginalThread;
                    actvityOfUser.IsPrivate = actvity.IsPrivate;
                    actvityOfUser.UserId = actvity.UserId;
                    actvityOfUser.ReferenceId = actvity.ReferenceId;
                    actvityOfUser.SourceId = actvity.SourceId;
                    actvityOfUser.TenantTypeId = actvity.TenantTypeId;

                    actvityOfUser.OwnerId = sender.UserId;
                    actvityOfUser.OwnerName = sender.User.DisplayName;
                    actvityOfUser.OwnerType = ActivityOwnerTypes.Instance().User();
                    activityService.Generate(actvityOfUser, true);
                }
                else
                {
                    actvity.OwnerId = sender.OwnerId;
                    actvity.OwnerType = microblogUrlGetter.ActivityOwnerType;
                    actvity.OwnerName = microblogUrlGetter.GetOwnerName(sender.OwnerId);
                    activityService.Generate(actvity, false);

                    //生成Owner为用户的动态
                    if (!microblogUrlGetter.IsPrivate(sender.OwnerId))
                    {
                        Activity actvityOfUser = Activity.New();
                        actvityOfUser.ActivityItemKey = actvity.ActivityItemKey;
                        actvityOfUser.ApplicationId = actvity.ApplicationId;
                        actvityOfUser.HasImage = actvity.HasImage;
                        actvityOfUser.HasMusic = actvity.HasMusic;
                        actvityOfUser.HasVideo = actvity.HasVideo;
                        actvityOfUser.IsOriginalThread = actvity.IsOriginalThread;
                        actvityOfUser.IsPrivate = actvity.IsPrivate;
                        actvityOfUser.UserId = actvity.UserId;
                        actvityOfUser.ReferenceId = actvity.ReferenceId;
                        actvityOfUser.SourceId = actvity.SourceId;
                        actvityOfUser.TenantTypeId = actvity.TenantTypeId;

                        actvityOfUser.OwnerId = sender.UserId;
                        actvityOfUser.OwnerName = sender.User.DisplayName;
                        actvityOfUser.OwnerType = ActivityOwnerTypes.Instance().User();
                        activityService.Generate(actvityOfUser, false);
                    }
                }
            }
            else if (auditDirection == false) //删除动态
            {
                activityService.DeleteSource(TenantTypeIds.Instance().Microblog(), sender.MicroblogId);
            }
        }

        /// <summary>
        /// 审核状态发生变化时处理积分
        /// </summary>
        /// <param name="sender">微博实体</param>
        /// <param name="eventArgs">时间参数</param>
        private void MicroblogPointModule_After(MicroblogEntity sender, AuditEventArgs eventArgs)
        {
            string pointItemKey = string.Empty, eventOperationType = string.Empty;
            ActivityService activityService = new ActivityService();
            AuditService auditService = new AuditService();

            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true) //加积分
            {
                pointItemKey = PointItemKeys.Instance().Microblog_CreateMicroblog();
                if (eventArgs.OldAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Create();
                else
                    eventOperationType = EventOperationType.Instance().Approved();
            }
            else if (auditDirection == false) //减积分
            {
                pointItemKey = PointItemKeys.Instance().Microblog_DeleteMicroblog();
                if (eventArgs.NewAuditStatus == null)
                    eventOperationType = EventOperationType.Instance().Delete();
                else
                    eventOperationType = EventOperationType.Instance().Disapproved();
            }

            if (!string.IsNullOrEmpty(pointItemKey))
            {
                PointService pointService = new PointService();

                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_" + eventOperationType), "微博", Tunynet.Utilities.HtmlUtility.TrimHtml(sender.Body, 15));
                pointService.GenerateByRole(sender.UserId, pointItemKey, description);
            }
        }
    }
}