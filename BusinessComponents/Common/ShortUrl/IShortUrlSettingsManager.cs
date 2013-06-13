//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Utilities;

namespace Tunynet.Common
{
    /// <summary>
    /// 短网址配置类
    /// </summary>
    public interface IShortUrlSettingsManager
    {
        /// <summary>
        /// 获取ShortUrlSettings
        /// </summary>
        ShortUrlSettings Get();

        /// <summary>
        /// 保存ShortUrlSettings
        /// </summary>
        void Save(ShortUrlSettings shortUrlSettings);
    }
}
