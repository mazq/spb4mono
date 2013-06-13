//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet;
using Tunynet.Events;
using Tunynet.Globalization;
using Tunynet.Logging;
using Spacebuilder.Common;

namespace Spacebuilder.Bar.EventModules
{
    /// <summary>
    /// 处理贴吧操作日志
    /// </summary>
    public class BarOperationLogEventModule : IEventMoudle
    {


        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void IEventMoudle.RegisterEventHandler()
        {
            //贴吧
            EventBus<BarSection>.Instance().After += new CommonEventHandler<BarSection, CommonEventArgs>(BarSectionOperationLogEventModule_After);

            //贴吧
            EventBus<BarThread>.Instance().After += new CommonEventHandler<BarThread, CommonEventArgs>(BarThreadOperationLogEventModule_After);

        }


        /// <summary>
        /// 贴吧操作日志事件处理
        /// </summary>
        private void BarSectionOperationLogEventModule_After(BarSection senders, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete()
               || eventArgs.EventOperationType == EventOperationType.Instance().Approved()
               || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved()
               || eventArgs.EventOperationType == EventOperationType.Instance().SetEssential()
               || eventArgs.EventOperationType == EventOperationType.Instance().SetSticky()
               || eventArgs.EventOperationType == EventOperationType.Instance().CancelEssential()
               || eventArgs.EventOperationType == EventOperationType.Instance().CancelSticky())
            {
                OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);
                entry.ApplicationId = entry.ApplicationId;
                entry.Source = BarConfig.Instance().ApplicationName;
                entry.OperationType = eventArgs.EventOperationType;
                entry.OperationObjectName = senders.Name;
                entry.OperationObjectId = senders.SectionId;
                entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType, entry.ApplicationId), "贴吧", entry.OperationObjectName);

                OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                logService.Create(entry);
            }
        }

        /// <summary>
        /// 帖子操作日志事件处理
        /// </summary>
        private void BarThreadOperationLogEventModule_After(BarThread senders, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().Delete()
              || eventArgs.EventOperationType == EventOperationType.Instance().Approved()
              || eventArgs.EventOperationType == EventOperationType.Instance().Disapproved()
              || eventArgs.EventOperationType == EventOperationType.Instance().SetEssential()
              || eventArgs.EventOperationType == EventOperationType.Instance().SetSticky()
              || eventArgs.EventOperationType == EventOperationType.Instance().CancelEssential()
              || eventArgs.EventOperationType == EventOperationType.Instance().CancelSticky())
            {
                OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);

                entry.ApplicationId = entry.ApplicationId;
                entry.Source = BarConfig.Instance().ApplicationName;
                entry.OperationType = eventArgs.EventOperationType;
                entry.OperationObjectName = senders.Subject;
                entry.OperationObjectId = senders.ThreadId;
                entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType), "帖子", entry.OperationObjectName);

                OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                logService.Create(entry);
            }
        }
    }
}