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
    /// 用户隐私设置指定对象接口
    /// </summary>
    public interface IUserPrivacySpecifyObjectRepository
    {
        /// <summary>
        /// 更新用户隐私设置指定对象
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userSettings"><remarks>key=itemKey,value=PrivacyStatus</remarks></param>
        /// <param name="specifyObjects"><remarks>key=itemKey,value=用户指定对象集合</remarks></param>
        void UpdateUserPrivacySpecifyObjects(long userId, Dictionary<string, IEnumerable<UserPrivacySpecifyObject>> specifyObjects);
        
       
        /// <summary>
        /// 获取用户隐私设置指定对象集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="itemKey">隐私项目Key</param>
        /// <returns><remarks>key=specifyObjectTypeId,value=用户指定对象集合</remarks></returns>
        Dictionary<int, IEnumerable<UserPrivacySpecifyObject>> GetUserPrivacySpecifyObjects(long userId, string itemKey);

    }
}
