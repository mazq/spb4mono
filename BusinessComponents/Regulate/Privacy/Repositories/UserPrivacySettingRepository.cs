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
    /// 用户隐私设置仓储
    /// </summary>
    public class UserPrivacySettingRepository : Repository<UserPrivacySetting>, IUserPrivacySettingRepository
    {

        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 更新用户的隐私设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userSettings"><remarks>key=itemKey,value=PrivacyStatus</remarks></param>
        public void UpdateUserPrivacySettings(long userId, Dictionary<string, PrivacyStatus> userSettings)
        {
            PetaPocoDatabase dao = CreateDAO();

            var sql = Sql.Builder;
            sql.Select("*")
               .From("tn_UserPrivacySettings")
               .Where("UserId = @0", userId);
            //done:zhangp,by zhengw:应该共享一个数据库连接
            //回复：已经修改
            dao.OpenSharedConnection();
            UserPrivacySetting setting;
            List<UserPrivacySetting> settings = dao.Fetch<UserPrivacySetting>(sql);

            if (userSettings == null)
                return;
            foreach (var item in userSettings)
            {
                setting = settings.Find(n => n.ItemKey == item.Key);
                if (setting != null)
                {
                    var sqlDel = Sql.Builder;
                    sqlDel.Append("delete from tn_UserPrivacySpecifyObjects where UserPrivacySettingId in (select Id from tn_UserPrivacySettings where UserId = @0 and ItemKey = @1)", userId, item.Key);
                    dao.Execute(sqlDel);
                    EntityData.ForType(typeof(UserPrivacySpecifyObject)).RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
                    setting.PrivacyStatus = item.Value;
                    dao.Update(setting);
                    OnUpdated(setting);
                }
                else
                {
                    setting = new UserPrivacySetting();
                    setting.UserId = userId;
                    setting.ItemKey = item.Key;
                    setting.PrivacyStatus = item.Value;
                    base.Insert(setting);
                }
            }
            dao.CloseSharedConnection();
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }

        
        ///<summary>
        /// 获取用户的隐私设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns><para>如果用户无设置返回空集合</para><remarks>key=itemKey,value=PrivacyStatus</remarks></returns>
        public Dictionary<string, PrivacyStatus> GetUserPrivacySettings(long userId)
        {
            StringBuilder cacheKey = new StringBuilder(RealTimeCacheHelper.GetListCacheKeyPrefix(CacheVersionType.AreaVersion, "UserId", userId));
            cacheKey.AppendFormat("UserPrivacySetting");

            List<long> ids = cacheService.Get<List<long>>(cacheKey.ToString());
            if (ids == null)
            {
                var sql = Sql.Builder;
                sql.Select("Id")
                   .From("tn_UserPrivacySettings")
                   .Where("UserId = @0", userId);
                ids = CreateDAO().Fetch<long>(sql);
                cacheService.Add(cacheKey.ToString(), ids, CachingExpirationType.ObjectCollection);
            }
            IEnumerable<UserPrivacySetting> settings = PopulateEntitiesByEntityIds<long>(ids);
            Dictionary<string, PrivacyStatus> dictionary = new Dictionary<string, PrivacyStatus>();

            foreach (var item in settings)
            {
                //done:zhangp,by zhengw:建议使用索引器添加值
                //回复：已经修改
                dictionary[item.ItemKey] = item.PrivacyStatus;
            }
            return dictionary;
        }



        /// <summary>
        /// 清空用户隐私设置（用于恢复到默认设置）
        /// </summary>
        /// <param name="userId"></param>
        public void ClearUserPrivacySettings(long userId)
        {
            var sql = Sql.Builder;
            sql.Append("delete from tn_UserPrivacySpecifyObjects where UserPrivacySettingId in (select Id from tn_UserPrivacySettings where UserId = @0)", userId);
            sql.Append("delete from tn_UserPrivacySettings  where UserId = @0", userId);
            CreateDAO().Execute(sql);
            EntityData.ForType(typeof(UserPrivacySpecifyObject)).RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
            RealTimeCacheHelper.IncreaseAreaVersion("UserId", userId);
        }
    }
}

