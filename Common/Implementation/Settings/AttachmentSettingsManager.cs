//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common.Configuration;
using System.Drawing;
using Tunynet.Imaging;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Tunynet.Common;
using Spacebuilder.Common.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 附件设置实现类
    /// </summary>
    public class AttachmentSettingsManager : IAttachmentSettingsManager
    {
        private ISettingsRepository<AttachmentSettings> repository;
        /// <summary>
        /// 构造器
        /// </summary>
         public AttachmentSettingsManager()
        {
            repository = new SettingsRepository<AttachmentSettings>();
        }

        /// <summary>
        /// 获取附件设置
        /// </summary>
        /// <returns></returns>
        public AttachmentSettings Get()
        {
            return repository.Get();
        }


        public void Save(AttachmentSettings attachmentSettings)
        {
            repository.Save(attachmentSettings);
        }
    }
}