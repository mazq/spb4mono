//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using System.Collections.Concurrent;

namespace Tunynet.Common.Repositories
{
    public interface IOnlineUserRepository : IRepository<OnlineUser>
    {
        /// <summary>
        /// 用户离线（注销时调用）
        /// </summary>
        /// <param name="userName"></param>
        void Offline(string userName);
        /// <summary>
        /// 获取在线登录用户列表
        /// </summary>
        /// <remarks>key=UserName,value=OnlineUser</remarks>
        Dictionary<string, OnlineUser> GetLoggedUsers();
        /// <summary>
        /// 获取匿名登录用户列表
        /// </summary>
        /// <remarks>key=UserName,value=OnlineUser</remarks>
        IList<OnlineUser> GetAnonymousUsers();
        /// <summary>
        /// 刷新数据库
        /// </summary>
        /// <remarks>
        /// 通过Task调用
        /// </remarks>
        void Refresh(ConcurrentDictionary<string, OnlineUser> OnlineUsersForProcess);
       
        
    }
}
