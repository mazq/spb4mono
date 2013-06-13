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

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 屏蔽 数据访问借口
    /// </summary>
    public interface IUserBlockRepository : IRepository<UserBlockedObject>
    {
        /// <summary>
        /// 获取y用户的屏蔽对象列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="objectType">被屏蔽的类型</param>
        IEnumerable<UserBlockedObject> GetBlockedObjects(long userId, int objectType);

        /// <summary>
        /// 清除根据用户删除数据（删除 用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>是否成功清除数据</returns>
        bool CleanByUser(long userId);
    }
}
