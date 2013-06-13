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
    public interface IPauseSiteSettingsManager
    {
        /// <summary>
        /// 获取PauseSiteSettings
        /// </summary>
        /// <returns></returns>
        PauseSiteSettings get();

        /// <summary>
        /// 保存PauseSiteSettings
        /// </summary>
        /// <param name="pauseSiteSettings"></param>
        void Save(PauseSiteSettings pauseSiteSettings);
    }
}
