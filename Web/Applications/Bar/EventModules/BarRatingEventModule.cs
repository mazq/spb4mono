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

namespace Spacebuilder.Bar.EventModules
{
    
    /// <summary>
    /// 处理帖子动态、积分的EventMoudle
    /// </summary>
    public class BarRatingEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册EventHandler
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<BarRating>.Instance().After += new CommonEventHandler<BarRating, CommonEventArgs>(BarRatingEventModule_After);
        }

        void BarRatingEventModule_After(BarRating sender, CommonEventArgs eventArgs)
        {
            ActivityService activityService = new ActivityService();
            if (eventArgs.EventOperationType == EventOperationType.Instance().Create())
            {
                Activity actvity = Activity.New();
                actvity.ActivityItemKey = ActivityItemKeys.Instance().CreateBarRating();
                actvity.ApplicationId = BarConfig.Instance().ApplicationId;

                BarThreadService barThreadService = new BarThreadService();
                BarThread barThread = barThreadService.Get(sender.ThreadId);
                if (barThread == null)
                    return;
                var barUrlGetter = BarUrlGetterFactory.Get(barThread.TenantTypeId);
                if (barUrlGetter == null)
                    return;

                actvity.IsOriginalThread = true;
                actvity.IsPrivate = barUrlGetter.IsPrivate(barThread.SectionId);
                actvity.OwnerId = barThread.SectionId;
                actvity.OwnerName = barThread.BarSection.Name;
                actvity.OwnerType = barUrlGetter.ActivityOwnerType;
                actvity.ReferenceId = barThread.ThreadId;
                actvity.ReferenceTenantTypeId = TenantTypeIds.Instance().BarThread();
                actvity.SourceId = sender.RatingId;
                actvity.TenantTypeId = TenantTypeIds.Instance().BarRating();
                actvity.UserId = sender.UserId;

                //自己回复自己时，不向自己的动态收件箱推送动态
                if (actvity.UserId == barThread.UserId)
                    activityService.Generate(actvity, false);
                else
                    activityService.Generate(actvity, true);
            }
            else
            {
                activityService.DeleteSource(TenantTypeIds.Instance().BarRating(), sender.RatingId);
            }
        }
    }
}