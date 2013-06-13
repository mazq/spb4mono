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
    
    //好了
    /// <summary>
    /// 工作经历数据访问
    /// </summary>
    public class WorkExperienceRepository : Repository<WorkExperience>, IWorkExperienceRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取所有用户工作信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<long> GetWorkExperienceOfUser(long userId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "userId", userId));
            cacheKey.AppendFormat("GetWorkExperienceOfUser");
            IEnumerable<long> workExperienceIds = cacheService.Get<IEnumerable<long>>(cacheKey.ToString());
            if (workExperienceIds == null || workExperienceIds.Count() == 0)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("id")
                .From("spb_WorkExperiences")
                .Where("UserId = @0", userId);
                workExperienceIds = CreateDAO().FetchFirstColumn(sql).Cast<long>();
                cacheService.Set(cacheKey.ToString(), workExperienceIds, CachingExpirationType.UsualObjectCollection);
            }
            return workExperienceIds;
        }

        /// <summary>
        /// 根据用户ID列表获取工作经历ID列表，本方法现用于用户搜索功能的索引生成
        /// </summary>
        /// <param name="userIds">用户ID列表</param>
        /// <returns>工作经历ID列表</returns>
        public IEnumerable<long> GetEntityIdsByUserIds(IEnumerable<long> userIds)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Select("Id")
                   .From("spb_WorkExperiences")
                   .Where("UserId in (@userIds)", new { userIds = userIds });
            return CreateDAO().Fetch<long>(sql);
        }
    }
}