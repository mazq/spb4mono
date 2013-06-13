//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Common;
using System.Text;
using PetaPoco;
using Tunynet.Repositories;
using System.Linq;
using Tunynet.Common.Configuration;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 用户黑名单仓储
    /// </summary>
    public class StopedUserRepository : Repository<StopedUser>, IStopedUserRepository
    {

        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();


        /// <summary>
        /// 获取用户的黑名单
        /// </summary>
        /// <returns><remarks>key=ToUserId,value=StopedUser</remarks></returns>
        public Dictionary<long, StopedUser> GetStopedUsers(long userId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
            cacheKey.AppendFormat("StopedUser");
            List<long> ids = cacheService.Get<List<long>>(cacheKey.ToString());

            if (ids == null)
            {
                var sql = Sql.Builder;
                sql.Select("Id")
                   .From("tn_StopedUsers")
                   .Where("UserId = @0", userId);
                ids = CreateDAO().Fetch<long>(sql);

                cacheService.Add(cacheKey.ToString(), ids, CachingExpirationType.ObjectCollection);
            }

            var stopUsers = PopulateEntitiesByEntityIds<long>(ids);
            Dictionary<long, StopedUser> dictionary = new Dictionary<long, StopedUser>();
            foreach (var stopUser in stopUsers)
            {
                //done:zhangp,by zhengw:建议使用dictionary[stopUser.ToUserId] = stopUser;因为使用Add时如果有重复的Key可能会报错
                //回复：已经修改
                dictionary[stopUser.ToUserId] = stopUser;
            }

            return dictionary;
        }


        /// <summary>
        /// 把用户加入黑名单
        /// </summary>
        /// <param name="stopedUser">黑名单</param>
        public bool CreateStopedUser(StopedUser stopedUser)
        {
            
            Dictionary<long, StopedUser> stopUsers = GetStopedUsers(stopedUser.UserId);
            if (stopUsers != null && stopUsers.Count() > new PrivacySettings().StopUserMaxCount)
                return false;
            Insert(stopedUser);
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", stopedUser.UserId);
            return true;
        }

        /// <summary>
        /// 把用户从黑名单中删除
        /// </summary>
        /// <param name="stopedUser">黑名单用户</param>
        public void DeleteStopedUser(StopedUser stopedUser)
        {
            Delete(stopedUser);
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", stopedUser.UserId);
        }
    }
}
