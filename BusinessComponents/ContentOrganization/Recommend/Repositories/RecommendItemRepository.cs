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
    ///推荐内容数据访问仓储
    /// </summary>
    public class RecommendItemRepository : Repository<RecommendItem>, IRecommendItemRepository
    {
        private int pageSize = 20;
        /// <summary>
        /// 创建推荐内容实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override object Insert(RecommendItem entity)
        {
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_RecommendItems")
                .Where("TypeId = @0", entity.TypeId)
                .Where("ItemId = @0", entity.ItemId);
            
            //liuz回复：已修改
            RecommendItem recommendItem = CreateDAO().FirstOrDefault<RecommendItem>(sql);

            if (recommendItem == null || entity.IsLink)
            {
                return base.Insert(entity);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除推荐内容
        /// </summary>
        /// <param name="itemId">内容Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public bool Delete(long itemId, string tenantTypeId)
        {
            Sql sql = Sql.Builder;
            sql.Append("delete from tn_RecommendItems where ItemId = @0 and TenantTypeId = @1", itemId, tenantTypeId);
            int result = CreateDAO().Execute(sql);
            return result > 0;
        }

        /// <summary>
        /// 定期移除过期的推荐内容
        /// </summary>
        public void DeleteExpiredRecommendItems()
        {
            Sql sql = Sql.Builder;
            sql.Append("delete from tn_RecommendItems where ExpiredDate < @0", DateTime.UtcNow);
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 获取推荐内容
        /// </summary>
        /// <param name="itemId">推荐内容Id</param>
        /// <param name="recommendTypeId">推荐类型Id</param>
        public RecommendItem Get(long itemId, string recommendTypeId)
        {
            Sql sql = Sql.Builder;
            sql.Select("*")
                .From("tn_RecommendItems")
                .Where("ItemId=@0 and TypeId=@1", itemId, recommendTypeId);
            RecommendItem recommendItem = CreateDAO().FirstOrDefault<RecommendItem>(sql);
            return recommendItem;
        }

        /// <summary>
        /// 获取某种推荐类别下的前N条推荐内容
        /// </summary>
        /// <param name="topNumber">前N条</param>
        /// <param name="recommendTypeId">推荐类别Id</param>
        /// <returns></returns>
        public IEnumerable<RecommendItem> GetTops(int topNumber, string recommendTypeId)
        {
            //设计要点
            //1、需要使用缓存，并使用分区版本recommendTypeId
            return GetTopEntities(topNumber, CachingExpirationType.UsualObjectCollection, () =>
            {
                
                //liuz回复:以修改
                StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TypeId", recommendTypeId ?? string.Empty));
                cacheKey.Append("topRecommendItems");
                return cacheKey.ToString();
            }, () =>
            {
                var sql = Sql.Builder;
                sql.Select("*")
                    .From("tn_RecommendItems")
                    .Where("TypeId= @0", recommendTypeId ?? string.Empty)
                    .OrderBy("DisplayOrder desc");
                return sql;
            }
            );

        }

        /// <summary>
        /// 获取某种推荐类别下的推荐内容分页集合
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="recommendTypeId">推荐类别Id</param>
        /// <returns></returns>
        public PagingDataSet<RecommendItem> Gets(string recommendTypeId, int pageIndex)
        {
            //设计要点
            //1、需要使用缓存，并使用分区版本recommendTypeId
            return GetPagingEntities(pageSize, pageIndex, CachingExpirationType.ObjectCollection,
                () =>
                {
                    
                    //liuz回复：已修改
                    StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TypeId", recommendTypeId ?? string.Empty));
                    cacheKey.AppendFormat("pagingRecommendItems");
                    return cacheKey.ToString();
                }, () =>
                {
                    Sql sql = Sql.Builder;
                    sql.Select("*")
                        .From("tn_RecommendItems")
                        .Where("TypeId=@0", recommendTypeId ?? string.Empty);
                    return sql;
                }
                );
        }

        /// <summary>
        /// 获取某条内容的所有推荐
        /// </summary>
        /// <param name="itemId">内容Id</param>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public IEnumerable<RecommendItem> Gets(long itemId, string tenantTypeId)
        {
            //设计要点
            //1、不需要使用缓存
            Sql sql = Sql.Builder;
            sql.Select("Id")
                .From("tn_RecommendItems")
                .Where("ItemId=@0 and TenantTypeId=@1", itemId, tenantTypeId);
            IEnumerable<long> recommendIds = CreateDAO().Fetch<long>(sql);
            return PopulateEntitiesByEntityIds<long>(recommendIds);
        }

        /// <summary>
        /// 分页获取推荐内容后台管理列表
        /// </summary>
        /// <param name="tenantTypeId"></param>
        /// <param name="recommendTypeId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PagingDataSet<RecommendItem> GetsForAdmin(string tenantTypeId, string recommendTypeId, bool? isLink, int pageSize = 20, int pageIndex = 1)
        {
            //设计要点
            //1、不需要使用缓存
            Sql sql = Sql.Builder;

            sql.Select("Id")
                .From("tn_RecommendItems");

            if (!string.IsNullOrEmpty(tenantTypeId))
                sql.Where("TenantTypeId=@0", tenantTypeId);
            else
                sql.Where("TenantTypeId!=@0", TenantTypeIds.Instance().User());

            if (!string.IsNullOrEmpty(recommendTypeId))
                sql.Where("TypeId=@0", recommendTypeId);

            if (isLink != null)
                sql.Where("IsLink=@0", isLink.Value);
            sql.OrderBy("DisplayOrder desc");
            return GetPagingEntities(pageSize, pageIndex, sql);
        }
    }
}