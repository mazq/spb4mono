//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户通知设置数据访问接口
    /// </summary>
    public interface IUserNoticeSettingsRepository : IRepository<UserNoticeSettings>
    {

        /// <summary>
        /// 获取用户通知设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Dictionary<int, bool> GetUserNoticeSettingses(long userId);

        /// <summary>
        /// 用户更新通知设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="typeIds">通知类型Id集合</param>
        void UpdateUserNoticeSettings(long userId, Dictionary<int, bool> userNoticeSettings);
    }
}