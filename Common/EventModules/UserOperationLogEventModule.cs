//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
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
    /// User操作日志处理
    /// </summary>
    public class UserOperationLogEventMoudle : IEventMoudle, IOperationLogSpecificPartProcesser<User>
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        public void RegisterEventHandler()
        {
            EventBus<User>.Instance().After += new CommonEventHandler<User, CommonEventArgs>(UserOperationLogProcesser_After);
            EventBus<User>.Instance().BatchAfter += new BatchEventHandler<User, CommonEventArgs>(UserOperationLogEventMoudle_BatchAfter);
            EventBus<User, DeleteUserEventArgs>.Instance().After += new CommonEventHandler<User, DeleteUserEventArgs>(UserOperationLogEventMoudle_After);
        }

        void UserOperationLogEventMoudle_BatchAfter(IEnumerable<User> senders, CommonEventArgs eventArgs)
        {
            //只有批量激活用户、取消激活用户、管制用户、取消管制用户时，才记录到操作日志
            if (eventArgs.EventOperationType == EventOperationType.Instance().ActivateUser() ||
            eventArgs.EventOperationType == EventOperationType.Instance().CancelActivateUser() ||
            eventArgs.EventOperationType == EventOperationType.Instance().ModerateUser() ||
            eventArgs.EventOperationType == EventOperationType.Instance().CancelModerateUser())
            {
                OperationLogService logService = new OperationLogService();
                logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                IUserService userService = DIContainer.Resolve<IUserService>();

                OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);
                entry.ApplicationId = 0;
                entry.Source = string.Empty;
                entry.OperationType = eventArgs.EventOperationType;
                IEnumerable<string> userNames = senders.Select(n => n.UserName);
                entry.OperationObjectName = string.Join(",", userNames);
                entry.OperationObjectId = 0;
                entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType), entry.OperationObjectName);
                logService.Create(entry);
            }
        }

        void UserOperationLogEventMoudle_After(User sender, DeleteUserEventArgs eventArgs)
        {
            OperationLogService logService = new OperationLogService();
            logService = Tunynet.DIContainer.Resolve<OperationLogService>();

            OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);
            entry.ApplicationId = 0;
            entry.Source = string.Empty;
            entry.OperationType = EventOperationType.Instance().DeleteUser();
            entry.OperationObjectName = sender.UserName;
            entry.OperationObjectId = 0;
            entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + entry.OperationType), entry.OperationObjectName);
            logService.Create(entry);
        }

        void UserOperationLogProcesser_After(User sender, CommonEventArgs eventArgs)
        {
            if (eventArgs.EventOperationType == EventOperationType.Instance().BanUser() ||
                eventArgs.EventOperationType == EventOperationType.Instance().UnbanUser() ||
                eventArgs.EventOperationType == EventOperationType.Instance().Update())
            {
                OperationLogService logService = new OperationLogService();
                logService = Tunynet.DIContainer.Resolve<OperationLogService>();
                IUserService userService = DIContainer.Resolve<IUserService>();

                OperationLogEntry entry = new OperationLogEntry(eventArgs.OperatorInfo);
                entry.ApplicationId = 0;
                entry.Source = string.Empty;
                entry.OperationObjectName = sender.UserName;
                entry.OperationObjectId = 0;
                if (eventArgs.EventOperationType == EventOperationType.Instance().Update())
                {
                    entry.OperationType = "UpdateUser";
                    entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_Update"), "用户", entry.OperationObjectName);
                }
                else
                {
                    entry.OperationType = eventArgs.EventOperationType;
                    entry.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventArgs.EventOperationType), entry.OperationObjectName);
                }
                logService.Create(entry);
            }
        }

        /// <summary>
        /// 处理操作日志具体信息部分（把User、eventOperationType转化成ISpecificOperationLogInformation）
        /// </summary>
        /// <param name="entity">日志操作对象</param>
        /// <param name="eventOperationType">操作类型</param>
        /// <param name="operationLogSpecificPart">具体的操作日志信息接口</param>
        void IOperationLogSpecificPartProcesser<User>.Process(User entity, string eventOperationType, IOperationLogSpecificPart operationLogSpecificPart)
        {
            operationLogSpecificPart.ApplicationId = 0;
            operationLogSpecificPart.Source = string.Empty;
            operationLogSpecificPart.OperationObjectName = entity.DisplayName;
            operationLogSpecificPart.OperationObjectId = entity.UserId;
            operationLogSpecificPart.OperationType = eventOperationType;

            operationLogSpecificPart.Description = string.Format(ResourceAccessor.GetString("OperationLog_Pattern_" + eventOperationType), entity.DisplayName);
        }

        /// <summary>
        /// 处理带历史数据变更的操作日志部分
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="eventOperationType"></param>
        /// <param name="historyData"></param>
        /// <param name="operationLogSpecificPart"></param>
        public void Process(User entity, string eventOperationType, User historyData, IOperationLogSpecificPart operationLogSpecificPart)
        {
            //未涉及
        }
    }
}