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
using Tunynet.Utilities;
using Spacebuilder.Common;

namespace Spacebuilder.Microblog.EventModules
{
    /// <summary>
    /// 处理微博操作日志
    /// </summary>
    public class MicroblogOperationLogEventModule : IEventMoudle
    {


        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void IEventMoudle.RegisterEventHandler()
        {
            EventBus<MicroblogEntity>.Instance().After += new CommonEventHandler<MicroblogEntity, CommonEventArgs>(MicroblogOperationLogEventModule_After);
        }


        /// <summary>
        /// 微博操作日志事件处理
        /// </summary>
        private void MicroblogOperationLogEventModule_After(MicroblogEntity senders, CommonEventArgs eventArgs)
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
                entry.Source = MicroblogConfig.Instance().ApplicationName;
                entry.OperationType = eventArgs.EventOperationType;
                entry.OperationObjectName = StringUtility.Trim(senders.Body, 20);
                entry.OperationObjectId = senders.MicroblogId;
                entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType, entry.ApplicationId), "微博", entry.OperationObjectName);

                OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                logService.Create(entry);
            }
        }
    }
}