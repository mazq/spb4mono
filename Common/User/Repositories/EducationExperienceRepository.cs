//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 教育经历数据访问
    /// </summary>
    public class EducationExperienceRepository : Repository<EducationExperience>, IEducationExperienceRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取所有教育信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetEducationExperienceOfUser(long userId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
            cacheKey.AppendFormat("GetEducationExperienceOfUser");
            IEnumerable<long> educationExperienceIds = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());
            if (educationExperienceIds == null || educationExperienceIds.Count() == 0)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("id")
                   .From("spb_EducationExperiences")
                   .Where("UserId = @0", userId);

                educationExperienceIds = CreateDAO().FetchFirstColumn(sql).Cast<long>();
                cacheService.Set(cacheKey.ToString(), educationExperienceIds, CachingExpirationType.UsualObjectCollection);
            }

            return educationExperienceIds;
        }

        /// <summary>
        /// 根据用户ID列表获取教育经历ID列表，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns>教育经历ID列表</returns>
        public IEnumerable<long> GetEntityIdsByUserIds(IEnumerable<long> userIds)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("Id")
                   .From("spb_EducationExperiences")
                   .Where("UserId in (@userIds)", new { userIds = userIds });
            return CreateDAO().Fetch<long>(sql);
        }
    }
}