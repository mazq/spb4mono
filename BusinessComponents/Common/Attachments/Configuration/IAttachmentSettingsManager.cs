//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common.Configuration
{

    /// <summary>
    /// AttachmentSettings管理器接口
    /// </summary>
    public interface IAttachmentSettingsManager
    {
        /// <summary>
        /// 获取AttachmentSettings
        /// </summary>
        /// <returns></returns>
        AttachmentSettings Get();

        /// <summary>
        /// 保存AttachmentSettings
        /// </summary>
        /// <param name="areaSettings"></param>
        void Save(AttachmentSettings attachmentSettings);
    }
}
