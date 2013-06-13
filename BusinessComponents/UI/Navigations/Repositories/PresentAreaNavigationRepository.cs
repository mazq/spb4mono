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

namespace Tunynet.UI
{
    /// <summary>
    /// PresentAreaNavigation仓储实现
    /// </summary>
    public class PresentAreaNavigationRepository : Repository<PresentAreaNavigation>, IPresentAreaNavigationRepository
    {

        /// <summary>
        /// 获取呈现区域实例的所有PresentAreaNavigation
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实例OwnerId</param>
        public IEnumerable<PresentAreaNavigation> GetNavigations(string presentAreaKey, long ownerId)
        {
            ICacheService cacheService = DIContainer.Resolve<ICacheService>();

            string cacheKey = RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "OwnerId", ownerId) + "PresentAreaKey:" + presentAreaKey;
            IEnumerable<object> ids = cacheService.Get<IEnumerable<object>>(cacheKey);
            if (ids == null)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Select("Id")
                    .From("tn_PresentAreaNavigations")
                    .Where("PresentAreaKey = @0", presentAreaKey)
                    .Where("OwnerId=@0", ownerId)
                    .OrderBy("Depth", "DisplayOrder");

                ids = CreateDAO().FetchFirstColumn(sql);

                cacheService.Add(cacheKey, ids, CachingExpirationType.RelativelyStable);
            }
            return PopulateEntitiesByEntityIds(ids);
        }

        /// <summary>
        /// 强制所有呈现区域实例Owner安装导航
        /// </summary>
        /// <param name="initialNavigation">初始化导航实体</param>
        public void ForceOwnerCreate(InitialNavigation initialNavigation)
        {
            if (initialNavigation == null)
                return;

            var sql = PetaPoco.Sql.Builder;
            PresentAreaNavigation presentAreaNavigation = initialNavigation.AsPresentAreaNavigation();
            sql.Append("insert into tn_PresentAreaNavigations (OwnerId,PresentAreaKey,NavigationId,ParentNavigationId,Depth,"
                                    + "ApplicationId,NavigationType,NavigationText,ResourceName,NavigationUrl,"
                                    + "UrlRouteName,ImageUrl,NavigationTarget,DisplayOrder,OnlyOwnerVisible,IsLocked,IsEnabled) "
            + "select distinct OwnerId,@0,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15 "
            + "from  tn_PresentAreaNavigations where PresentAreaKey=@0",
            initialNavigation.PresentAreaKey, initialNavigation.NavigationId, initialNavigation.ParentNavigationId, initialNavigation.Depth,
            initialNavigation.ApplicationId, initialNavigation.NavigationType, initialNavigation.NavigationText, initialNavigation.ResourceName, initialNavigation.NavigationUrl,
            initialNavigation.UrlRouteName, initialNavigation.ImageUrl, initialNavigation.NavigationTarget, initialNavigation.DisplayOrder, initialNavigation.OnlyOwnerVisible, initialNavigation.IsLocked, initialNavigation.IsEnabled
            );
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 强制所有呈现区域实例Owner更新导航
        /// </summary>
        /// <param name="initialNavigation">初始化导航实体</param>
        public void ForceOwnerUpdate(InitialNavigation initialNavigation)
        {
            if (initialNavigation == null)
                return;

            var sql = PetaPoco.Sql.Builder;
            sql.Append("update tn_PresentAreaNavigations set NavigationText =@0, ResourceName=@1, NavigationUrl=@2,UrlRouteName=@3,ImageUrl=@4,NavigationTarget=@5,IsEnabled=@6 ", initialNavigation.NavigationText,
                initialNavigation.ResourceName, initialNavigation.NavigationUrl, initialNavigation.UrlRouteName, initialNavigation.ImageUrl, initialNavigation.NavigationTarget, initialNavigation.IsEnabled)
                .Append("where NavigationId = @0", initialNavigation.NavigationId);

            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 强制所有呈现区域实例Owner卸载导航
        /// </summary>
        /// <param name="initialNavigations">初始化导航实体集合</param>
        public void ForceOwnerDelete(IEnumerable<InitialNavigation> initialNavigations)
        {
            if (initialNavigations == null || initialNavigations.Count() == 0)
                return;

            List<PetaPoco.Sql> sqls = new List<PetaPoco.Sql>();
            foreach (var initialNavigation in initialNavigations)
            {
                var sql = PetaPoco.Sql.Builder;
                sql.Append("delete from tn_PresentAreaNavigations where PresentAreaKey = @0 and NavigationId=@1",
                    initialNavigation.PresentAreaKey, initialNavigation.NavigationId);

                sqls.Add(sql);
            }

            CreateDAO().Execute(sqls);
        }

        /// <summary>
        /// 清除呈现区域实例Owner的所有导航
        /// </summary>
        /// <param name="presentAreaKey">呈现区域标识</param>
        /// <param name="ownerId">呈现区域实体OwnerId</param>
        public void ClearOwnerNavigations(string presentAreaKey, long ownerId)
        {
            var sql = PetaPoco.Sql.Builder;
            sql.Append("delete from tn_PresentAreaNavigations where PresentAreaKey = @0 and OwnerId=@1", presentAreaKey, ownerId);
            RealTimeCacheHelper.IncreaseAreaVersion("OwnerId", ownerId);
            CreateDAO().Execute(sql);
        }

    }
}
