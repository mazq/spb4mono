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
    /// 提醒信息查询接口
    /// </summary>
    public interface IReminderInfoAccessor
    {
        /// <summary>
        /// 提醒信息类型Id
        /// </summary>
        int ReminderInfoTypeId { get; }

        /// <summary>
        /// 获取处理地址
        /// </summary>
        string GetProcessUrl(long userId);
        
        /// <summary>
        /// 获取用户提醒信息集合
        /// </summary>
        /// <returns>用户提醒信息集合</returns>
        IEnumerable<UserReminderInfo> GetUserReminderInfos();
    }
}
