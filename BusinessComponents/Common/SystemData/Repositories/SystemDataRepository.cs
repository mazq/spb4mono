//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 系统数据Repository
    /// </summary>
    public class SystemDataRepository : Repository<SystemData>, ISystemDataRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="dataKey">数据标识</param>
        /// <param name="number">待变更的数值</param>
        public void Change(string dataKey, long number)
        {
            //当DataKey不存在时，插入新数据

            PetaPocoDatabase dao = CreateDAO();
            SystemData systemData = Get(dataKey);
            if (systemData == null)
            {
                systemData = new SystemData();
                systemData.Datakey = dataKey;
                systemData.LongValue = number;
                dao.Insert(systemData);
            }
            else
            {
                systemData.LongValue += number;
                dao.Update(systemData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(systemData);
        }

        /// <summary>
        /// 变更系统数据
        /// </summary>
        /// <param name="dataKey">数据标识</param>
        /// <param name="number">待变更的数值</param>
        public void Change(string dataKey, decimal number)
        {
            //当DataKey不存在时，插入新数据

            PetaPocoDatabase dao = CreateDAO();

            SystemData systemData = Get(dataKey);

            if (systemData == null)
            {
                systemData = new SystemData();
                systemData.Datakey = dataKey;
                systemData.DecimalValue = number;
                dao.Insert(systemData);
            }
            else
            {
                systemData.DecimalValue += number;
                dao.Update(systemData);
            }
            RealTimeCacheHelper.IncreaseEntityCacheVersion(systemData);
        }
       
    }
}