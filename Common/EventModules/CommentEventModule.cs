//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common;
using Tunynet.Globalization;
using Tunynet;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 创建和删除评论的积分处理
    /// </summary>
    public class CommentEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册创建和删除评论的积分处理
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<Comment>.Instance().After += new CommonEventHandler<Comment, CommonEventArgs>(CommentCreatAndDeleteEventModel_After);
            EventBus<Comment, AuditEventArgs>.Instance().After += new CommonEventHandler<Comment, AuditEventArgs>(CommentNoticeEventModule_After);
            EventBus<Comment>.Instance().BatchAfter += new BatchEventHandler<Comment, CommonEventArgs>(CommentsDeleteEventModel_After);
        }

        /// <summary>
        /// 通知处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void CommentNoticeEventModule_After(Comment sender, AuditEventArgs eventArgs)
        {
            AuditService auditService = new AuditService();
            bool? auditDirection = auditService.ResolveAuditDirection(eventArgs.OldAuditStatus, eventArgs.NewAuditStatus);
            if (auditDirection == true)
            {
                var urlGetter = CommentUrlGetterFactory.Get(sender.TenantTypeId);
                var commentedObject = urlGetter.GetCommentedObject(sender.CommentedObjectId);
                var senderUser = sender.User();
                if (urlGetter == null || commentedObject == null)
                    return;
                //日志有新评论时，自动通知原作者
                var toUserIds = new List<long> { commentedObject.UserId };
                if (sender.ParentId > 0)
                {
                    toUserIds.Add(sender.ToUserId);
                }
                foreach (var toUserId in toUserIds)
                {
                    //通知的对象排除掉自己
                    if (toUserId == sender.UserId)
                    {
                        continue;
                    }
                    Notice notice = Notice.New();
                    notice.UserId = toUserId;
                    notice.ApplicationId = 0;
                    notice.TypeId = NoticeTypeIds.Instance().Reply();
                    notice.LeadingActor = senderUser != null ? senderUser.DisplayName : "匿名用户";
                    notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(UserIdToUserNameDictionary.GetUserName(sender.UserId)));
                    notice.RelativeObjectName = HtmlUtility.TrimHtml(commentedObject.Name,60);
                    notice.RelativeObjectUrl = SiteUrls.FullUrl(urlGetter.GetCommentDetailUrl(sender.CommentedObjectId, sender.Id, commentedObject.UserId))??string.Empty;
                    notice.TemplateName = sender.ParentId > 0 ? NoticeTemplateNames.Instance().NewReply() : NoticeTemplateNames.Instance().NewComment();
                    new NoticeService().Create(notice);
                }
            }
        }

        /// <summary>
        /// 评论的积分处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void CommentCreatAndDeleteEventModel_After(Comment sender, CommonEventArgs eventArgs)
        {
            PointService pointService = new PointService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_CreateComment"), sender.Author, sender.Subject);
                pointService.GenerateByRole(sender.UserId, PointItemKeys.Instance().CreateComment(), description);
            }

            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete())
            {
                string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_DeleteComment"), sender.Author, sender.Subject);
                pointService.GenerateByRole(sender.UserId, PointItemKeys.Instance().DeleteComment(), description);
            }
        }

        /// <summary>
        /// 删除被评论对象时评论的积分处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void CommentsDeleteEventModel_After(IEnumerable<Comment> sender, CommonEventArgs eventArgs)
        {

            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete() && sender!=null)
            {

                PointService pointService = new PointService();
                foreach (var item in sender)
                {
                    string description = string.Format(ResourceAccessor.GetString("PointRecord_Pattern_DeleteComment"), item.Author, item.Subject);
                    pointService.GenerateByRole(item.UserId, PointItemKeys.Instance().DeleteComment(), description);
                }
            }
        }
    }
}
