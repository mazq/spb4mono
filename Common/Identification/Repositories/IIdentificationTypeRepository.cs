//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 认证标识接口
    /// </summary>
    public interface IIdentificationTypeRepository : IRepository<IdentificationType>
    {

        /// <summary>
        /// 获取身份认证标识
        /// </summary>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        IEnumerable<IdentificationType> GetIdentificationTypes(bool? isEnabled);
    }
}
