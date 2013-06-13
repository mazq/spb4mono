//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 等级统计数据访问
    /// </summary>
    public class RatingGradeRepository : Repository<RatingGrade>, IRatingGradeRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取指定评价选项信息
        /// </summary>
        /// <param name="objectId">评价数据Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public IEnumerable<RatingGrade> GetRatingGrades(long objectId, string tenantTypeId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "ObjectId", objectId));
            cacheKey.Append("RatingGrades-tenantTypeId:" + tenantTypeId);

            IEnumerable<RatingGrade> ratingGrades = null;
            ratingGrades = cacheService.Get<IEnumerable<RatingGrade>>(cacheKey.ToString());
            if (ratingGrades == null)
            {
                var sql = Sql.Builder;
                sql.Where("TenantTypeId = @0 and ObjectId = @1", tenantTypeId, objectId);
                ratingGrades = CreateDAO().Fetch<RatingGrade>(sql);
                cacheService.Add(cacheKey.ToString(), ratingGrades, CachingExpirationType.ObjectCollection);
            }
            return ratingGrades;
        }

        /// <summary>
        /// 清空相关联的等级统计信息
        /// </summary>
        /// <param name="objectId"> 操作Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        public void ClearRatingGradesOfObjectId(long objectId, string tenantTypeId)
        {
            var sql = Sql.Builder;
            sql.Append("DELETE FROM tn_RatingGrades WHERE ObjectId= @0 and TenantTypeId= @1 ", objectId, tenantTypeId);
            CreateDAO().Execute(sql);
        }
    }
}