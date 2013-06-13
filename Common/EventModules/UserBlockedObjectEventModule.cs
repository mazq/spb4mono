//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using Tunynet.Events;
using Tunynet.Globalization;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 关注相关事件
    /// </summary>
    public class UserBlockedObjectEventModule : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<UserBlockedObject>.Instance().After += new CommonEventHandler<UserBlockedObject, CommonEventArgs>(UserBlockedObjectActivityEventModule_After);
        }

        /// <summary>
        /// 屏蔽用户/取消屏蔽时动态处理
        /// </summary>
        /// <param name="sender">关注实体</param>
        /// <param name="eventArgs">事件参数</param>
        void UserBlockedObjectActivityEventModule_After(UserBlockedObject sender, CommonEventArgs eventArgs)
        {
            int ownerType = -1;
            if (sender.ObjectType == BlockedObjectTypes.Instance().User())
                ownerType = ActivityOwnerTypes.Instance().User();
            else if (sender.ObjectType == BlockedObjectTypes.Instance().Group())
                ownerType = ActivityOwnerTypes.Instance().Group();
            else
                return;
            ActivityService activityService = new ActivityService();
            if (EventOperationType.Instance().Create() == eventArgs.EventOperationType)
            {
                activityService.RemoveInboxAboutOwner(sender.UserId, sender.ObjectId, ownerType);                
            }
            else if (EventOperationType.Instance().Delete() == eventArgs.EventOperationType)
            {
                activityService.TraceBackInboxAboutOwner(sender.UserId, sender.ObjectId, ownerType);
            }
        }
    }
}