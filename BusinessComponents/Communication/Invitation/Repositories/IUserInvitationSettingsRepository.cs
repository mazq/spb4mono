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
    /// 用户请求设置数据访问接口
    /// </summary>
    public interface IUserInvitationSettingsRepository : IRepository<UserInvitationSettings>
    {

        /// <summary>
        /// 用户获取请求设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>请求类型-是否接收设置集合</returns>
        Dictionary<string, bool> GetUserInvitationSettingses(long userId);

        /// <summary>
        /// 用户更新请求设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="typeKey2IsAllowDictionary">请求类型-是否接收设置集合</param>
        void UpdateUserInvitationSettings(long userId, Dictionary<string, bool> typeKey2IsAllowDictionary);

    }
}