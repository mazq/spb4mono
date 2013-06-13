//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Repositories;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户隐私设置指定对象仓储
    /// </summary>
    public class UserPrivacySpecifyObjectRepository : Repository<UserPrivacySpecifyObject>, IUserPrivacySpecifyObjectRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 更新用户隐私设置指定对象
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="specifyObjects"><remarks>key=itemKey,value=用户指定对象集合</remarks></param>
        public void UpdateUserPrivacySpecifyObjects(long userId, Dictionary<string, IEnumerable<UserPrivacySpecifyObject>> specifyObjects)
        {
            //done:zhangp,by zhengw:
            //先遍历集合specifyObjects
            //再使用sql语句来获取当前itemKey下的所有指定对象集合oldItemSpecifyObjects
            //比较oldItemSpecifyObjects和newItemSpecifyObjects
            //1.如果newItemSpecifyObjects为null，则清空
            //2.新旧数据取交集，然后进行删除和插入


            //done:zhangp,by zhengw:这些局部变量没必要在外边实例化，后面肯定会被覆盖、
            //回复：已修改

            PetaPocoDatabase dao = CreateDAO();

            var sql = Sql.Builder;
            List<UserPrivacySpecifyObject> userPrivacySpecifyObjects;
            IEnumerable<UserPrivacySpecifyObject> pairs;
            IEnumerable<UserPrivacySpecifyObject> deletePairs;
            IEnumerable<UserPrivacySpecifyObject> insertPairs;


            if (specifyObjects == null || specifyObjects.Count() == 0)
                return;
            dao.OpenSharedConnection();
            foreach (var item in specifyObjects)
            {
                sql = Sql.Builder;
                sql.Select("tn_UserPrivacySpecifyObjects.*")
                   .From("tn_UserPrivacySpecifyObjects")
                   .InnerJoin("tn_UserPrivacySettings")
                   .On("tn_UserPrivacySettings.Id = tn_UserPrivacySpecifyObjects.UserPrivacySettingId")
                   .Where("UserId = @0", userId)
                   .Where("ItemKey = @0", item.Key);
                userPrivacySpecifyObjects = dao.Fetch<UserPrivacySpecifyObject>(sql);
                //done:zhangp,by zhengw:userPrivacySpecifyObjects.Count为0时就不更新了？我觉得可以把这个判断去掉，两个边界情况都可以使用求交集的办法
                //回复：已修改
                
                sql = Sql.Builder;

                sql.Select("Id")
                   .From("tn_UserPrivacySettings")
                   .Where("UserId = @0", userId)
                   .Where("ItemKey = @0", item.Key);
                long userPrivacySettingId = dao.FirstOrDefault<long>(sql);

                if (item.Value == null)
                {
                    foreach (var oldSpecifyObject in userPrivacySpecifyObjects)
                        base.Delete(oldSpecifyObject);
                }

                pairs = userPrivacySpecifyObjects.Intersect(item.Value);
                deletePairs = userPrivacySpecifyObjects.Except(pairs);
                foreach (var deletePair in deletePairs)
                    base.Delete(deletePair);
                insertPairs = item.Value.Except(pairs);
                foreach (var insertPair in insertPairs)
                {
                    insertPair.UserPrivacySettingId = userPrivacySettingId;
                    base.Insert(insertPair);
                }

            }

            dao.CloseSharedConnection();
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }


        /// <summary>
        /// 获取用户隐私设置指定对象集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="itemKey">隐私项目Key</param>
        /// <returns><remarks>key=specifyObjectTypeId,value=用户指定对象集合</remarks></returns>
        public Dictionary<int, IEnumerable<UserPrivacySpecifyObject>> GetUserPrivacySpecifyObjects(long userId, string itemKey)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
            cacheKey.AppendFormat("UserPrivacySpecifyObject::ItemKey-{0}", itemKey);

            List<long> ids = cacheService.Get<List<long>>(cacheKey.ToString());
            if (ids == null)
            {
                var sql = Sql.Builder;
                sql.Select("tn_UserPrivacySpecifyObjects.Id")
                   .From("tn_UserPrivacySpecifyObjects")
                   .InnerJoin("tn_UserPrivacySettings")
                   .On("tn_UserPrivacySettings.Id = tn_UserPrivacySpecifyObjects.UserPrivacySettingId")
                   .Where("UserId = @0", userId);
                if (itemKey != null)
                    sql.Where("ItemKey = @0", itemKey);

                ids = CreateDAO().Fetch<long>(sql);

                cacheService.Add(cacheKey.ToString(), ids, CachingExpirationType.ObjectCollection);
            }

            var objects = PopulateEntitiesByEntityIds(ids);
            Dictionary<int, IEnumerable<UserPrivacySpecifyObject>> dictionary = new Dictionary<int, IEnumerable<UserPrivacySpecifyObject>>();
            if (objects == null)
                return dictionary;
            IEnumerable<int> specifyObjectTypeIds = objects.Select(n => n.SpecifyObjectTypeId).Distinct();
            foreach (var item in specifyObjectTypeIds)
            {
                List<UserPrivacySpecifyObject> newObj = objects.Where(n => n.SpecifyObjectTypeId == item).ToList();
                dictionary[item] = newObj;
                //dictionary.Add(item, objects.Where(n => n.SpecifyObjectTypeId == item));
            }
            return dictionary;
        }
    }
}
