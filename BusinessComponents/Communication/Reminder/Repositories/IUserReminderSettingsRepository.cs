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
    /// 用户提醒设置数据访问接口
    /// </summary>
    public interface IUserReminderSettingsRepository : IRepository<UserReminderSettings>
    {
        /// <summary>
        /// 用户获取所有提醒设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户提醒设置集合</returns>
        Dictionary<string, IEnumerable<UserReminderSettings>> GetAllUserReminderSettings(long userId);

        /// <summary>
        /// 用户更新提醒设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userReminderSettings">用户提醒设置集合</param>
        void BatchUpdateUserReminderSettings(long userId, IEnumerable<UserReminderSettings> userReminderSettings);
    }
}