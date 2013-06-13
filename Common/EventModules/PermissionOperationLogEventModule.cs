//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Tunynet;
using Tunynet.Common;
using Tunynet.Events;
using Tunynet.Globalization;
using Tunynet.Logging;

namespace Spacebuilder.Common.EventModules
{
    /// <summary>
    /// Permission操作日志处理
    /// </summary>
    public class PermissionOperationLogEventMoudle : IEventMoudle
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<PermissionItemInUserRole>.Instance().BatchAfter += new BatchEventHandler<PermissionItemInUserRole, CommonEventArgs>(PermissionOperationLogEventMoudle_BatchAfter);
        }

        void PermissionOperationLogEventMoudle_BatchAfter(IEnumerable<PermissionItemInUserRole> senders, CommonEventArgs eventArgs)
        {
            //只记录批量更新操作
            if (eventArgs.EventOperationType != EventOperationType.Instance().Update())
                return;
            OperationLogService logService = Tunynet.DIContainer.Resolve<OperationLogService>();
            PermissionService permissionService = new PermissionService();

            OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);

            entry.ApplicationId = 0;
            entry.Source = string.Empty;
            entry.OperationType = eventArgs.EventOperationType;
            IEnumerable<string> roleNames = senders.Select(n => n.RoleName).Distinct();
            entry.OperationObjectName = string.Join(",", roleNames);
            entry.OperationObjectId = 0;
            entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType), "权限", entry.OperationObjectName);
            logService.Create(entry);
        }
    }
}