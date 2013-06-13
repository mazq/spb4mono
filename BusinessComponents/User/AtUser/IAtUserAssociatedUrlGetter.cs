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
    /// AtUser关联项Url获取器
    /// </summary>
    public interface IAtUserAssociatedUrlGetter
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        string TenantTypeId { get; }

        /// <summary>
        /// 获取关联项信息
        /// </summary>
        /// <param name="associateId">关联项Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        AssociatedInfo GetAssociatedInfo(long associateId, string tenantTypeId = "");

        /// <summary>
        /// 获取所属对象名称（例如：日志）
        /// </summary>
        /// <remarks>没有任何所属时返回空</remarks>
        /// <returns></returns>
        string GetOwner();
    }


    public class AssociatedInfo
    {
        public string DetailUrl { get; set; }

        public string Subject { get; set; }
    }
}
