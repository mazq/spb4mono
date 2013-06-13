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
    /// 用户等级的数据访问接口
    /// </summary>
    public interface IUserRankRepository : IRepository<UserRank>
    {
        /// <summary>
        /// 依据现行规则重置所有用户等级
        /// </summary>
         void ResetAllUser();
    }
}
