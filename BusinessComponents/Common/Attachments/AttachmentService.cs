//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Common.Configuration;
using Tunynet.FileStore;

namespace Tunynet.Common
{
    /// <summary>
    /// 附件业务逻辑类
    /// </summary>
    public class AttachmentService : AttachmentService<Attachment>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public AttachmentService(string tenantTypeId)
            : base(tenantTypeId)
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="attachmentRepository"></param>
        /// <param name="tenantAttachmentSettings"></param>
        /// <param name="storeProvider"></param>
        public AttachmentService(IAttachmentRepository<Attachment> attachmentRepository, TenantAttachmentSettings tenantAttachmentSettings, IStoreProvider storeProvider)
            : base(attachmentRepository, tenantAttachmentSettings, storeProvider)
        {

        }
    }
}
