//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 标签Url获取接口
    /// </summary>
    public interface ITagUrlGetter
    {
        /// <summary>
        /// 获取标签访问Url
        /// </summary>
        /// <param name="tagName">标签名</param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        string GetUrl(string tagName, long ownerId = 0);
    }
}