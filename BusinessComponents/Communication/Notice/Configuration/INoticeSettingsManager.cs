//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// NoticeSettings管理器接口
    /// </summary>
    public interface INoticeSettingsManager
    {
        /// <summary>
        /// 获取NoticeSettings
        /// </summary>
        /// <returns></returns>
        NoticeSettings Get();

        /// <summary>
        /// 保存NoticeSettings
        /// </summary>
        /// <returns></returns>
        void Save(NoticeSettings noticeSettings);
    }
}
