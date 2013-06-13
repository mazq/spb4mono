//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Mail;
using Tunynet;
using Tunynet.Common;
using Tunynet.Email;
using System.Linq;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邮件Service的扩展类
    /// </summary>
    public static class TenantTypeIdsExtensions
    {
        /// <summary>
        /// 邮件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string Email(this TenantTypeIds ids)
        {
            return "000091";
        }
    }

    public static class CountTypesExtensions
    {
        /// <summary>
        /// 使用次数
        /// </summary>
        /// <returns></returns>
        public static string UseCount(this CountTypes types)
        {
            return "UseCount";
        }
    }
}