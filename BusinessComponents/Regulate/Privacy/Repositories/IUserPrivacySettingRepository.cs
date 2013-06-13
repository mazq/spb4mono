//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户隐私设置接口
    /// </summary>
    public interface IUserPrivacySettingRepository
    {
        /// <summary>
        /// 更新用户的隐私设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userSettings"><remarks>key=itemKey,value=PrivacyStatus</remarks></param>
        void UpdateUserPrivacySettings(long userId, Dictionary<string, PrivacyStatus> userSettings);


        /// <summary>
        /// 获取用户的隐私设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns><para>如果用户无设置返回空集合</para><remarks>key=itemKey,value=PrivacyStatus</remarks></returns>
        Dictionary<string, PrivacyStatus> GetUserPrivacySettings(long userId);

        /// <summary>
        /// 清空用户隐私设置（用于恢复到默认设置）
        /// </summary>
        /// <param name="userId"></param>
        void ClearUserPrivacySettings(long userId);
      
    }
}
