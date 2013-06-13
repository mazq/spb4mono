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

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 屏蔽用户的数据访问
    /// </summary>
    public class UserBlockRepository : Repository<UserBlockedObject>, IUserBlockRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 获取被屏蔽对象列表
        /// </summary>
        /// <param name="userId">>UserId</param>
        /// <param name="objectType">被屏蔽对象类型</param>
        /// <returns>被屏蔽对象列表</returns>
        public IEnumerable<UserBlockedObject> GetBlockedObjects(long userId, int objectType)
        {
            //操作数据表：tn_UserBlockedObjects
            //根据上层传递下来的数据从数据库中查询出数据并且返回。关于缓存的处理
            //缓存策略：CachingExpirationType.RelativelyStable
            string cacheKey = GetCacheKey_BlockedObjects(userId, objectType);
            IEnumerable<long> blockedObjects = cacheService.Get<IEnumerable<long>>(cacheKey);

            if (blockedObjects == null)
            {
                var sql_Select = PetaPoco.Sql.Builder;
                sql_Select.Select("Id").From("tn_UserBlockedObjects")
                    .Where("UserId=@0", userId)
                    .Where("ObjectType=@0", objectType);
                blockedObjects = CreateDAO().Fetch<long>(sql_Select);
                if (blockedObjects != null)
                    cacheService.Add(cacheKey, blockedObjects, CachingExpirationType.ObjectCollection);
            }

            return base.PopulateEntitiesByEntityIds(blockedObjects);
        }

        /// <summary>
        /// 获取cachekey
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="objectType">保存数据类型</param>
        /// <returns></returns>
        private string GetCacheKey_BlockedObjects(long userId, int objectType)
        {
            int version = RealTimeCacheHelper.GetAreaVersion("UserId", userId);
            
            return string.Format("BlockedObjects-list;userId-{0};objectType-{1};Version-{2}", userId, objectType, version);
        }

        /// <summary>
        /// 清除用户数据（删除用户时使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>是否成功删除</returns>
        public bool CleanByUser(long userId)
        {
            var sql_Delete = new List<PetaPoco.Sql>();
            sql_Delete.Add(PetaPoco.Sql.Builder.Append("delete from tn_UserBlockedObjects where UserId=@0", userId));
            sql_Delete.Add(PetaPoco.Sql.Builder.Append("delete from tn_UserBlockedObjects where ObjectId=@0 and ObjectType=@1", userId, BlockedObjectTypes.Instance().User()));
            return CreateDAO().Execute(sql_Delete) > 0;
        }
    }
}
