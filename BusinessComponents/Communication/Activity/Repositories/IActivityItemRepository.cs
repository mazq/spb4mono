//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{

    /// <summary>
    /// 动态项目仓储接口
    /// </summary>
    public interface IActivityItemRepository : IRepository<ActivityItem>
    {

        void UpdateActivityItems(IEnumerable<ActivityItem> activityItems);
    }
}
