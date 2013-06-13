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
using PetaPoco;
using System.Collections.Concurrent;
using System.Threading;
using Tunynet.Common.Configuration;

namespace Tunynet.Common.Repositories
{
    /// <summary>
    /// 在线用户Repository
    /// </summary>
    public class OnlineUserRepository : Repository<OnlineUser>, IOnlineUserRepository
    {
        private ICacheService cacheService = DIContainer.Resolve<ICacheService>();

        /// <summary>
        /// 用户离线（注销时调用）
        /// </summary>
        /// <param name="userName"></param>
        public void Offline(string userName)
        {
            //立即从在线用户数据库移除用户

            Sql sql = Sql.Builder;
            sql.Append("delete from tn_OnlineUsers")
               .Where("UserName=@0", userName);
            CreateDAO().Execute(sql);
        }

        /// <summary>
        /// 获取在线登录用户列表
        /// </summary>
        /// <remarks>key=UserName,value=OnlineUser</remarks>
        public Dictionary<string, OnlineUser> GetLoggedUsers()
        {
            //设计要点：
            //1、缓存期限：集合，无需即时，使用一级缓存
            string cacheKey = GetCacheKey_LoggedUsers();
            //done:liuz,by zhengw:使用一级缓存，应调用GetFromFirstLevel方法,而且应该用泛型类型的
            //已修改
            Dictionary<string, OnlineUser> dictionary = cacheService.GetFromFirstLevel<Dictionary<string, OnlineUser>>(cacheKey);

            if (dictionary == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("tn_OnlineUsers")
                    .Where("UserId!=0")
                    .OrderBy("LastActivityTime desc");
                List<OnlineUser> list = CreateDAO().Fetch<OnlineUser>(sql);
                dictionary = new Dictionary<string, OnlineUser>();
                foreach (var onlineUser in list)
                {
                    dictionary[onlineUser.UserName] = onlineUser;
                }
                cacheService.Set(cacheKey, dictionary, CachingExpirationType.ObjectCollection);
            }
            return dictionary;
        }

        /// <summary>
        /// 获取匿名登录用户列表
        /// </summary>
        /// <remarks>key=UserName,value=OnlineUser</remarks>
        public IList<OnlineUser> GetAnonymousUsers()
        {   //设计要点：
            //1、缓存期限：集合，无需即时，使用一级缓存
            string cacheKey = GetCacheKey_AnonymousUsers();
            //done:liuz,by zhengw:使用一级缓存，应调用GetFromFirstLevel方法,而且应该用泛型类型的

            List<OnlineUser> anonymousUsers = cacheService.GetFromFirstLevel<List<OnlineUser>>(cacheKey);
            if (anonymousUsers == null)
            {
                Sql sql = Sql.Builder;
                sql.Select("*")
                    .From("tn_OnlineUsers")
                    .Where("UserId=0")
                    .OrderBy("LastActivityTime desc");
                anonymousUsers = CreateDAO().Fetch<OnlineUser>(sql);
                cacheService.Set(cacheKey, anonymousUsers, CachingExpirationType.ObjectCollection);
            }
            return anonymousUsers;
        }

        /// <summary>
        /// 刷新数据库
        /// </summary>
        /// <remarks>
        /// 通过Task调用
        /// </remarks>
        public void Refresh(ConcurrentDictionary<string, OnlineUser> OnlineUsersForProcess)
        {
            //设计要点：
            //1、把OnlineUsersForProcess更新到数据库，如果UserName存在则更新LastActivityTime，否则添加新在线用户
            //   可参考CountRepository.ExecQueue()
            //2、把超期未活动的用户移除
            //3、更新tn_OnlineUserStatistics : 每日一条记录保留最高记录（依据UserCount）

            PetaPocoDatabase dao = CreateDAO();
            try
            {
                dao.OpenSharedConnection();

                foreach (string key in OnlineUsersForProcess.Keys)
                {
                    OnlineUser onlineUserInDic = null;
                    OnlineUsersForProcess.TryRemove(key, out onlineUserInDic);

                    var sql = Sql.Builder;
                    sql.Select("*")
                        .From("tn_OnlineUsers")
                        .Where("UserName = @0", key);

                    OnlineUser onlineUserInDB = dao.FirstOrDefault<OnlineUser>(sql);
                    if (onlineUserInDB == null)
                    {
                        dao.Insert(onlineUserInDic);
                    }
                    else
                    {
                        sql = Sql.Builder;
                        sql.Append("Update tn_OnlineUsers set LastActivityTime=@0,LastAction=@1", onlineUserInDic.LastActivityTime, onlineUserInDic.LastAction)
                            .Where("UserName=@0", key);
                        dao.Execute(sql);
                    }
                    dao.Execute(Sql.Builder.Append("Update tn_Users set LastActivityTime=@0,LastAction=@1,IpLastActivity=@2", onlineUserInDic.LastActivityTime, onlineUserInDic.LastAction, onlineUserInDic.Ip));
                }

                //移除超期未活动的用户
                UserSettings userSetting = DIContainer.Resolve<IUserSettingsManager>().Get();
                var deleteSql = Sql.Builder;
                deleteSql.Append("delete from tn_OnlineUsers")
                    .Where("LastActivityTime < @0", DateTime.UtcNow.AddMinutes(-userSetting.UserOnlineTimeWindow));
                dao.Execute(deleteSql);

                //更新tn_OnlineUserStatistics : 每日一条记录保留最高记录（依据UserCount）
                int loggedUserCount = GetLoggedUsers().Count();
                int anonymousUserCount = GetAnonymousUsers().Count();
                int total = loggedUserCount + anonymousUserCount;

                var updateSql = Sql.Builder;
                updateSql.Append("update tn_OnlineUserStatistics set LoggedUserCount=@0,AnonymousCount=@1,UserCount = @2", loggedUserCount, anonymousUserCount, total)
                    .Where("UserCount < @0", total)
                    .Where("DateCreated>@0", new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day));

                int num = dao.Execute(updateSql);

                if (num == 0)
                {
                    var selectSql = Sql.Builder;
                    selectSql.Select("*")
                        .From("tn_OnlineUserStatistics")
                        .Where("DateCreated>@0", new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day));
                    OnlineUserStatistic statistic = dao.FirstOrDefault<OnlineUserStatistic>(selectSql);
                    if (statistic == null)
                    {
                        OnlineUserStatistic userStatistic = OnlineUserStatistic.New();
                        userStatistic.DateCreated = DateTime.UtcNow;
                        userStatistic.LoggedUserCount = loggedUserCount;
                        userStatistic.AnonymousCount = anonymousUserCount;
                        userStatistic.UserCount = total;

                        dao.Insert(userStatistic);
                    }
                }
            }
            finally
            {
                dao.CloseSharedConnection();
            }
        }

        /// <summary>
        /// 获取在线匿名用户CacheKey
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey_AnonymousUsers()
        {
            return "OnlineUser_AnonymousUsers";
        }
        /// <summary>
        /// 获取在线登录用户CacheKey
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey_LoggedUsers()
        {
            return "OnlineUser_LoggedUsers";
        }
    }
}
