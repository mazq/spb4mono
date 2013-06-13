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
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户黑名单接口
    /// </summary>
    public interface IStopedUserRepository : IRepository<StopedUser>
    {
        /// <summary>
        /// 获取用户的黑名单
        /// </summary>
        /// <returns><remarks>key=ToUserId,value=StopedUser</remarks></returns>
        Dictionary<long, StopedUser> GetStopedUsers(long userId);

        /// <summary>
        /// 把用户加入黑名单
        /// </summary>
        /// <param name="stopedUser">黑名单</param>
        bool CreateStopedUser(StopedUser stopedUser);

        /// <summary>
        /// 把用户从黑名单中删除
        /// <param name="stopedUser">黑名单</param>
        /// </summary>
        void DeleteStopedUser(StopedUser stopedUser);
    }
}
