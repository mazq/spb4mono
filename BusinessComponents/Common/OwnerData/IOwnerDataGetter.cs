//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// OwnerData数据访问接口
    /// </summary>
    public interface IOwnerDataGetter
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        long ApplicationId { get; }

        /// <summary>
        /// DataKey
        /// </summary>
        string DataKey { get; }

        /// <summary>
        /// 要显示的名称
        /// </summary>
        string DataName { get; }

        /// <summary>
        /// 链接数据地址
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        string GetDataUrl(string spaceKey, long? ownerId = null);
    }
}