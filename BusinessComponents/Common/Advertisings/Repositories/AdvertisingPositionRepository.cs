//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;
using System;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///广告位数据访问仓储
    /// </summary>
    public class AdvertisingPositionRepository : Repository<AdvertisingPosition>, IAdvertisingPositionRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取广告位列表
        /// </summary>
        /// <param name="presentAreaKey">投放区域</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        /// <param name="isEnable">是否启用</param>
        /// <returns></returns>
        public IEnumerable<AdvertisingPosition> GetPositionsForAdmin(string presentAreaKey, int height, int width, bool? isEnable)
        {
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_AdvertisingPosition");
            if (!string.IsNullOrEmpty(presentAreaKey))
            {
                sql.Where("PresentAreaKey=@0", presentAreaKey);
            }
            if (height > 0 && width > 0)
            {
                sql.Where("Height=@0 and Width=@1", height, width);
            }
            if (isEnable.HasValue)
            {
                sql.Where("IsEnable=@0", isEnable);
            }
            sql.OrderBy("Width desc");
            return CreateDAO().Fetch<AdvertisingPosition>(sql);
        }

        /// <summary>
        /// 获取广告位统计数据
        /// </summary>
        /// <returns></returns>
        public long GetPositionCount()
        {
            string cacheKey = "GetPositionCount";
            long positionCount = 0;
            if (cacheService.Get(cacheKey) != null)
            {
                positionCount = long.Parse(cacheService.Get(cacheKey).ToString());
                return positionCount;
            }
            Sql sql = Sql.Builder;
            sql.Select("count(*)")
                .From("tn_AdvertisingPosition");
            positionCount = CreateDAO().FirstOrDefault<long>(sql);

            cacheService.Add(cacheKey, positionCount, CachingExpirationType.UsualSingleObject);

            return positionCount;
        }

        /// <summary>
        /// 根据广告位Id取所有的广告
        /// </summary>
        /// <param name="positionId">广告位Id</param>
        /// <param name="isEnable">是否启用</param>
        /// <returns></returns>
        public IEnumerable<Advertising> GetAdvertisingsByPositionId(string positionId, bool? isEnable)
        {
            Sql sql = Sql.Builder
                        .Select("tn_Advertisings.*")
                        .From("tn_Advertisings")
                        .InnerJoin("tn_AdvertisingsInPosition").On("tn_AdvertisingsInPosition.AdvertisingId=tn_Advertisings.AdvertisingId")
                        .Where("tn_AdvertisingsInPosition.PositionId=@0", positionId)
                        .Where("tn_Advertisings.EndDate>@0", DateTime.UtcNow);
            if (isEnable.HasValue)
            {
                sql.Where("tn_Advertisings.IsEnable=@0", isEnable.Value);
            }

            return CreateDAO().Fetch<Advertising>(sql);
        }

    }
}
