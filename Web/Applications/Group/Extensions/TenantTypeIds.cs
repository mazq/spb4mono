//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Common;

namespace Spacebuilder.Group
{
    /// <summary>
    /// 群组定义的租户类型
    /// </summary>
    public static class TenantTypeIdsExtension
    {
        /// <summary>
        /// 群组应用
        /// </summary>
        /// <param name="TenantTypeIds">被扩展对象</param>
        public static string Group(this TenantTypeIds TenantTypeIds)
        {
            return "101100";
        }
    }
}