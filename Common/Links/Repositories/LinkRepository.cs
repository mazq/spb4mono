//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 友情链接数据访问
    /// </summary>
    public class LinkRepository : Repository<LinkEntity>, ILinkRepository
    {
        ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取Owner友情链接
        /// </summary>
        /// <param name="ownerType">拥有者类型</param>
        /// <param name="ownerId">拥有者Id</param>
        /// <returns></returns>
        public IEnumerable<LinkEntity> GetsOfOwner(int ownerType, long ownerId, int topNumber)
        {
            return GetTopEntities(topNumber, Tunynet.Caching.CachingExpirationType.ObjectCollection, () =>
            {
                StringBuilder cacheKey = new StringBuilder();
                cacheKey.Append(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId));
                cacheKey.AppendFormat("GetLinksOfOwner::ownerType-{0}", ownerType);
                return cacheKey.ToString();
            }, () =>
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("spb_Links")
                    .Where("ownerType=@0 and ownerId=@1", ownerType, ownerId)
                    .OrderBy("displayorder");
                return sql;
            });
        }

        /// <summary>
        /// 获取站点友情链接(后台管理)
        /// </summary>
        /// <param name="categoryId">分页标识</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public IEnumerable<LinkEntity> GetsOfSiteForAdmin(long? categoryId)
        {
            Sql sql = Sql.Builder.Append("select * from spb_Links");
            if (categoryId.HasValue)
            {
                sql.InnerJoin("tn_ItemsInCategories")
                   .On("spb_Links.LinkId=tn_ItemsInCategories.ItemId")
                   .Where("categoryId=@0 and ownerType=@1", categoryId.Value, OwnerTypes.Instance().Site())
                   .OrderBy("displayorder");
            }
            else
            {
                sql.Where("ownerType=@0", OwnerTypes.Instance().Site())
                    .OrderBy("displayorder");
            }

            return GetPagingEntities(1000, 1, sql);
        }
    }
}
