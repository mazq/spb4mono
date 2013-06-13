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
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// ApplicationInPresentAreaInstallation仓储
    /// </summary>
    public class ApplicationInPresentAreaInstallationRepository : Repository<ApplicationInPresentAreaInstallation>, IApplicationInPresentAreaInstallationRepository
    {

        /// <summary>
        /// 依据presentAreaKey、ownerId、applicationId获取对应的ApplicationInPresentAreaInstallation
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        /// <param name="applicationId">applicationId</param>
        public ApplicationInPresentAreaInstallation Fetch(string presentAreaKey, long ownerId, int applicationId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Where("PresentAreaKey = @0", presentAreaKey)
               .Where("OwnerId=@0", ownerId)
               .Where("ApplicationId=@0", applicationId);
            
            return CreateDAO().FirstOrDefault<ApplicationInPresentAreaInstallation>(sql);
        }

        /// <summary>
        /// 获取呈现区域实例拥有者已安装的ApplicationId列表
        /// </summary>
        /// <param name="presentAreaKey">区域区域Id</param>
        /// <param name="ownerId">呈现区域实例拥有者Id</param>
        /// <returns>返回在呈现区域安装的应用Id集合</returns>
        public IEnumerable<int> GetInstalledApplicationIds(string presentAreaKey, long ownerId)
        {
            string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId) + "PresentAreaKey:" + presentAreaKey;

            ICacheService cacheService = DIContainer.Resolve<ICacheService>();
            IEnumerable<int> applicationIds = cacheService.Get<IEnumerable<int>>(cacheKey);
            if (applicationIds == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("ApplicationId")
                    .From("tn_ApplicationInPresentAreaInstallations")
                    .Where("PresentAreaKey = @0", presentAreaKey)
                    .Where("OwnerId=@0", ownerId);
                IEnumerable<object> applicationIds_object = CreateDAO().FetchFirstColumn(sql);

                applicationIds = applicationIds_object.Cast<int>();
                cacheService.Add(cacheKey, applicationIds, CachingExpirationType.UsualObjectCollection);
            }

            return applicationIds;
        }



    }
}
