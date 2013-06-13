//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Tunynet.Common.Configuration;

namespace Tunynet.Common
{
    /// <summary>
    /// 清理垃圾临时附件任务
    /// </summary>
    public class DeleteTrashTemporaryAttachmentsTask : ITask
    {
        /// <summary>
        /// 任务执行的内容
        /// </summary>
        /// <param name="taskDetail">任务配置状态信息</param>
        public void Execute(TaskDetail taskDetail)
        {
            IEnumerable<TenantAttachmentSettings> allTenantAttachmentSettings = TenantAttachmentSettings.GetAll();
            foreach (var tenantAttachmentSettings in allTenantAttachmentSettings)
            {
                AttachmentService service = new AttachmentService(tenantAttachmentSettings.TenantTypeId);
                service.DeleteTrashTemporaryAttachments();
            }
        }
    }
}
