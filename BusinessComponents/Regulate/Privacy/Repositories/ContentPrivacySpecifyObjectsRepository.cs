//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{

    /// <summary>
    /// 内容隐私设置指定对象仓储
    /// </summary>
    public class ContentPrivacySpecifyObjectsRepository : Repository<ContentPrivacySpecifyObject>, IContentPrivacySpecifyObjectsRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取内容隐私设置指定对象
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="contentId">内容Id</param>
        /// <returns><remarks>key=specifyObjectTypeId,value=内容指定对象集合</remarks></returns>
        public Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> GetPrivacySpecifyObjects(string tenantTypeId, long contentId)
        {
            Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> objectsDic = new Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>>();


            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "TenantTypeId", tenantTypeId));
            cacheKey.AppendFormat("ContentPrivacySpecifyObject");
            cacheKey.AppendFormat(":ContentId-{0}", contentId);
            List<long> ids = cacheService.Get<List<long>>(cacheKey.ToString());
            if (ids == null)
            {
                var sql = Sql.Builder;
                sql.Select("Id")
                   .From("tn_ContentPrivacySpecifyObjects")
                   .Where("ContentId = @0", contentId);

                if (!string.IsNullOrEmpty(tenantTypeId))
                    sql.Where("TenantTypeId = @0", tenantTypeId);
                ids = CreateDAO().Fetch<long>(sql);

                cacheService.Add(cacheKey.ToString(), ids, CachingExpirationType.ObjectCollection);
            }

            var objects = PopulateEntitiesByEntityIds(ids);
            if (objects == null)
                return objectsDic;
            IEnumerable<int> specifyObjectTypeIds = objects.Select(n => n.SpecifyObjectTypeId).Distinct();
            foreach (var item in specifyObjectTypeIds)
            {
                List<ContentPrivacySpecifyObject> contentPrivacySpecifyObjects = new List<ContentPrivacySpecifyObject>();
                foreach (var objectItem in objects)
                    if (objectItem.SpecifyObjectTypeId == item)
                        contentPrivacySpecifyObjects.Add(objectItem);
                objectsDic.Add(item, contentPrivacySpecifyObjects);
            }
            return objectsDic;
        }


        /// <summary>
        /// 更新内容隐私设置
        /// </summary>
        /// <param name="privacyable">可隐私接口</param>
        /// <param name="specifyObjects"><remarks>key=specifyObjectTypeId,value=内容指定对象集合</remarks></param>
        public void UpdatePrivacySettings(IPrivacyable privacyable, Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> specifyObjects)
        {
            
            //回复：已修改
            var sql = Sql.Builder;
            //List<ContentPrivacySpecifyObject> userPrivacySpecifyObjects;
            //IEnumerable<ContentPrivacySpecifyObject> pairs;
            //IEnumerable<ContentPrivacySpecifyObject> deletePairs;
            //IEnumerable<ContentPrivacySpecifyObject> insertPairs;

            PetaPocoDatabase dao = CreateDAO();
            dao.OpenSharedConnection();

            sql.Append("delete from tn_ContentPrivacySpecifyObjects")
                   .Where("ContentId = @0", privacyable.ContentId)
                   .Where("TenantTypeId = @0", privacyable.TenantTypeId);
            dao.Execute(sql);

            if (specifyObjects == null)
                specifyObjects = new Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>>();

            foreach (var item in specifyObjects)
            {
                //sql = Sql.Builder;
                //done:zhangp,by zhengw:SpecifyObjectId=>SpecifyObjectTypeId
                //回复：已修改
                

                foreach (var insertPair in item.Value)
                {
                    dao.Insert(insertPair);
                }

                //sql.Select("*")
                //   .From("tn_ContentPrivacySpecifyObjects")
                //   .Where("ContentId = @0", privacyable.ContentId)
                //   .Where("TenantTypeId = @0", privacyable.TenantTypeId)
                //   .Where("SpecifyObjectTypeId = @0", item.Key);

                //userPrivacySpecifyObjects = dao.Fetch<ContentPrivacySpecifyObject>(sql);

                //done:zhangp,by zhengw:userPrivacySpecifyObjects.Count为0时就不更新了？我觉得可以把这个判断去掉，两个边界情况都可以使用求交集的办法
                //回复：已修改

                //pairs = userPrivacySpecifyObjects.Intersect(item.Value);
                //deletePairs = userPrivacySpecifyObjects.Except(pairs); 
                //foreach (var deletePair in deletePairs)
                //    dao.Delete(deletePair);
                //insertPairs = item.Value.Except(pairs);
                //foreach (var insertPair in insertPairs)
                //    dao.Insert(insertPair);

            }

            dao.CloseSharedConnection();
            RealTimeCacheHelper.IncreaseAreaVersion("TenantTypeId", privacyable.TenantTypeId);
        }
    }
}
