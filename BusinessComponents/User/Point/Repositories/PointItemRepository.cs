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
    /// 积分项目Repository
    /// </summary>
    public class PointItemRepository : Repository<PointItem>, IPointItemRepository
    {
        // 缓存服务
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 更新积分项目
        /// </summary>
        /// <param name="entity">待更新的积分项目</param>
        public override void Update(PointItem entity)
        {
            //注意：ItemId、ApplicationId、ItemName、DisplayOrder不允许修改
            var sql = Sql.Builder;
            sql.Append("Update tn_PointItems set ExperiencePoints = @0, ReputationPoints = @1, TradePoints = @2, TradePoints2 = @3, TradePoints3 = @4, TradePoints4 = @5, Description = @6 where ItemKey = @7", entity.ExperiencePoints, entity.ReputationPoints, entity.TradePoints, entity.TradePoints2, entity.TradePoints3, entity.TradePoints4, entity.Description, entity.ItemKey);
            CreateDAO().Execute(sql);
            //done:zhangp,by zhengw: 不要再次获取实体
            //清除缓存

            base.OnUpdated(entity);
        }

        /// <summary>
        /// 获取积分项目集合
        /// </summary>
        /// <param name="applicationId">应用Id</param>
        /// <returns>如果无满足条件的积分项目返回空集合</returns>
        public IEnumerable<PointItem> GetPointItems(int? applicationId)
        {
            //done:zhangp,by zhengw: 怎么没有使用缓存？注意PointItem列表 需要使用CachingExpirationType.RelativelyStable缓存。
            //排序条件：DisplayOrder正序
            
            //done:zhangp,by zhengw:没有应用Id这个分区版本，应该使用全局版本
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.GlobalVersion));
            //done:zhangp,by zhengw:加ApplicationId做什么？选择的关键词应该能表示积分项目才对，建议使用PointItems
            //回复：已修改
            cacheKey.AppendFormat("PointItems");
            IEnumerable<string> itemKeys = cacheService.Get<IEnumerable<string>>(cacheKey.ToString());
            if (itemKeys == null)
            {
                var sql = Sql.Builder;
                sql.Select("ItemKey")
                   .From("tn_PointItems");
                if (applicationId.HasValue)
                    sql.Where("ApplicationId = @0", applicationId);
                sql.OrderBy("DisplayOrder");
                itemKeys = CreateDAO().Fetch<string>(sql);
                cacheService.Add(cacheKey.ToString(), itemKeys, CachingExpirationType.RelativelyStable);
            }
            return PopulateEntitiesByEntityIds<string>(itemKeys);
        }
    }
}