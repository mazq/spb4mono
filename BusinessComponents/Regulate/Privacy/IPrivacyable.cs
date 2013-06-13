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
    /// 可隐私接口
    /// </summary>
    public interface IPrivacyable
    {
        //done:zhengw,by mazq 要求显性实现
        //zhengw回复：已加在接口上
        //mazq回复：？

        /// <summary>
        /// 内容项Id
        /// </summary>
        /// <remarks>
        /// 具体实现类显性实现
        /// </remarks>
        long ContentId { get; }

        //done:zhengw,by mazq 要求显性实现
        //zhengw回复：已加在接口上
        /// <summary>
        /// 内容项作者Id
        /// </summary>
        /// <remarks>
        /// 具体实现类显性实现
        /// </remarks>
        long UserId { get; }

        /// <summary>
        /// 隐私状态
        /// </summary>
        /// <remarks>一定不要显性实现</remarks>
        PrivacyStatus PrivacyStatus { get; set; }

        /// <summary>
        /// 租户类型Id
        /// </summary>
        /// <remarks>
        /// 具体实现类显性实现
        /// </remarks>
        string TenantTypeId { get; }

    }
}
