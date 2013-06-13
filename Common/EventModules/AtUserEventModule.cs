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
using Tunynet;
using Tunynet.Utilities;


namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// 处理微博相关事件处理程序
    /// </summary>
    public class AtUserEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序 
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<long, AtUserEventArgs>.Instance().BatchAfter += new BatchEventHandler<long, AtUserEventArgs>(AtUserNoticeEventModule_After);
        }

        /// <summary>
        /// At用户通知
        /// </summary>
        /// <param name="sender">用户名集合</param>
        /// <param name="eventArgs">事件参数</param>
        private void AtUserNoticeEventModule_After(IEnumerable<long> sender, AtUserEventArgs eventArgs)
        {
            if (sender.Count() == 0)
                return;

            NoticeService service = new NoticeService();
            IUserService userService = DIContainer.Resolve<IUserService>();

            User eventArgsUser = userService.GetFullUser(eventArgs.UserId);

            foreach (var userId in sender)
            {
                //关注用户
                IUser user = userService.GetUser(userId);
                if (user == null || user.UserId == eventArgs.UserId)
                    continue;

                IAtUserAssociatedUrlGetter urlGetter = AtUserAssociatedUrlGetterFactory.Get(eventArgs.TenantTypeId);
                if (urlGetter == null)
                    continue;
                AssociatedInfo ai = urlGetter.GetAssociatedInfo(eventArgs.AssociateId);

                Notice notice = Notice.New();
                notice.TypeId = NoticeTypeIds.Instance().Hint();
                notice.UserId = user.UserId;
                notice.LeadingActorUserId = eventArgs.UserId;
                notice.LeadingActor = eventArgsUser.DisplayName;
                notice.LeadingActorUrl = SiteUrls.FullUrl(SiteUrls.Instance().SpaceHome(eventArgs.UserId));
                notice.RelativeObjectId = eventArgs.AssociateId;
                notice.RelativeObjectName = HtmlUtility.TrimHtml(ai.Subject, 12);
                notice.RelativeObjectUrl = SiteUrls.FullUrl(ai.DetailUrl);
                notice.Owner = urlGetter.GetOwner();
                notice.TemplateName = "AtUser";

                service.Create(notice);
            }
        }

    }
}