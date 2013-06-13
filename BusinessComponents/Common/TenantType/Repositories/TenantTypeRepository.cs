//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 租户类型Repository
    /// </summary>
    public class TenantTypeRepository : Repository<TenantType>, ITenantTypeRepository
    {

        public ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 依据服务或应用获取租户类型
        /// </summary>
        /// <param name="serviceKey">服务标识</param>
        /// <param name="applicationId">应用Id</param>
        /// <returns>如未满足条件的TenantType则返回空集合</returns>
        public IEnumerable<TenantType> Gets(string serviceKey, int? applicationId = null)
        {
            string cacheKey = "TenantType::ServiceKey" + serviceKey+applicationId;
            IEnumerable<TenantType> tenantTypes = cacheService.Get<IEnumerable<TenantType>>(cacheKey);
            if (tenantTypes == null)
            {
                Sql sql = Sql.Builder;
                
                //reply:已修复
                if (!string.IsNullOrEmpty(serviceKey))
                {
                    sql.Select("tn_TenantTypes.*")
                       .From("tn_TenantTypes")
                       .InnerJoin("tn_TenantTypesInServices TTIS")
                       .On("tn_TenantTypes.TenantTypeId = TTIS.TenantTypeId")
                       .Where("TTIS.ServiceKey = @0", serviceKey);

                    if (applicationId.HasValue)
                        sql.Where("tn_TenantTypes.ApplicationId = @0", applicationId);
                }
                else
                {
                    sql.Select("*")
                       .From("tn_TenantTypes");

                    if (applicationId.HasValue)
                        sql.Where("ApplicationId = @0", applicationId);
                }

                tenantTypes = CreateDAO().Fetch<TenantType>(sql);

                
                //reply:已修复
                cacheService.Add(cacheKey, tenantTypes, CachingExpirationType.ObjectCollection);
            }

            return tenantTypes;
        }
    }
}