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
    /// UrlShortner接口
    /// </summary>
    public interface IUrlShortner
    {

        /// <summary>
        /// 进行短网址处理
        /// </summary>
        /// <param name="url">待处理的Url</param>
        /// <returns>缩短后的网址</returns>
        string Shortner(string url);

    }
}
