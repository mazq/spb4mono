//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Tunynet.Repositories;
using PetaPoco;
using System;
using Tunynet.Caching;
using System.Text;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    ///广告数据访问仓储
    /// </summary>
    public class AdvertisingRepository : Repository<Advertising>, IAdvertisingRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="presentAreaKey">投放区域</param>
        /// <param name="positionId">广告位</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isExpired">是否过期</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        public PagingDataSet<Advertising> GetAdvertisingsForAdmin(string presentAreaKey, string positionId, DateTime? startDate, DateTime? endDate, bool? isExpired, bool? isEnable, int pageSize, int pageIndex)
        {
            Sql sql = Sql.Builder;
            sql.Select("tn_Advertisings.*")
                .From("tn_Advertisings");

            if (!string.IsNullOrEmpty(presentAreaKey) || !string.IsNullOrEmpty(positionId))
            {
                Sql getIdsSql = Sql.Builder;
                getIdsSql.Select("tn_AdvertisingsInPosition.AdvertisingId").From("tn_AdvertisingsInPosition")
                    .LeftJoin("tn_AdvertisingPosition")
                    .On("tn_AdvertisingPosition.PositionId = tn_AdvertisingsInPosition.PositionId");

                if (!string.IsNullOrEmpty(positionId))
                {
                    getIdsSql.Where("tn_AdvertisingsInPosition.PositionId =@0", positionId);
                }
                if (!string.IsNullOrEmpty(presentAreaKey))
                {
                    getIdsSql.Where("tn_AdvertisingPosition.PresentAreaKey=@0", presentAreaKey);
                }
                List<long>  advertisingIds = CreateDAO().Fetch<long>(getIdsSql);

                if (advertisingIds == null || advertisingIds.Count == 0)
                {
                    advertisingIds = new List<long> { 0 };
                }
                sql.Where("AdvertisingId in (@0)", advertisingIds);
            }            

            if (startDate.HasValue)
            {
                sql.Where("StartDate >= @0", startDate);
            }
            if (endDate.HasValue)
            {
                sql.Where("EndDate <= @0", endDate);
            }
            if (isExpired.HasValue)
            {
                if (isExpired.Value)
                {
                    sql.Where("EndDate < @0", DateTime.UtcNow);
                }
                else
                {
                    sql.Where("EndDate > @0", DateTime.UtcNow);
                }
            }
            if (isEnable.HasValue)
            {
                sql.Where("IsEnable = @0", isEnable);
            }
            sql.OrderBy("DisplayOrder asc");
            return GetPagingEntities(pageSize, pageIndex, sql);
        }

        /// <summary>
        /// 获取群组统计数据
        /// </summary>
        /// <returns></returns>
        public long GetAdvertisingCount()
        {
            long advertisingCount = 0;
            string cacheKey = "GetAsvertisingCount";
            if (cacheService.Get(cacheKey) != null)
            {
                advertisingCount = long.Parse(cacheService.Get(cacheKey).ToString());
                return advertisingCount;
            }
            Sql sql = Sql.Builder;
            sql.Select("count(*)")
                .From("tn_Advertisings");
            advertisingCount = CreateDAO().FirstOrDefault<long>(sql);

            cacheService.Add(cacheKey, advertisingCount, CachingExpirationType.UsualSingleObject);

            return advertisingCount;
        }

        /// <summary>
        /// 清除广告的所有广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        public void ClearPositionsFromAdvertising(long advertisingId)
        {
            Sql sql = Sql.Builder;
            sql.Append("delete from tn_AdvertisingsInPosition where AdvertisingId=@0", advertisingId);
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 为广告批量设置广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <param name="positionIds">广告位Id集合</param>
        public void AddPositionsToAdvertising(long advertisingId, IEnumerable<string> positionIds)
        {
            List<Sql> sqls = new List<Sql>();
            foreach (var positionId in positionIds)
            {
                Sql sql = Sql.Builder.Append("insert into tn_AdvertisingsInPosition (AdvertisingId,PositionId) values(@0,@1)", advertisingId, positionId);
                sqls.Add(sql);
            }
            CreateDAO().Execute(sqls);
        }

        /// <summary>
        /// 根据广告Id取所有的广告位
        /// </summary>
        /// <param name="advertisingId">广告Id</param>
        /// <returns></returns>
        public IEnumerable<AdvertisingPosition> GetPositionsByAdvertisingId(long advertisingId)
        {
            Sql sql = Sql.Builder;
            sql.Select("tn_AdvertisingPosition.*")
                .From("tn_AdvertisingPosition")
                .InnerJoin("tn_AdvertisingsInPosition").On("tn_AdvertisingsInPosition.PositionId=tn_AdvertisingPosition.PositionId")
                .Where("tn_AdvertisingsInPosition.AdvertisingId=@0", advertisingId);
            return CreateDAO().Fetch<AdvertisingPosition>(sql);
        }
    }
}
