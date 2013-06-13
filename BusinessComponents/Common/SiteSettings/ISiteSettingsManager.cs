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
    /// SiteSettings管理器接口
    /// </summary>
    public interface ISiteSettingsManager
    {
        /// <summary>
        /// 获取SiteSettings
        /// </summary>
        /// <returns></returns>
        SiteSettings Get();

        /// <summary>
        /// 保存SiteSettings
        /// </summary>
        /// <param name="siteSettings"></param>
        void Save(SiteSettings siteSettings);
    }
}
