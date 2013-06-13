//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Concurrent;
using Tunynet;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 查询UserID与UserName的查询器
    /// </summary>
    public abstract class UserIdToUserNameDictionary
    {

        private static ConcurrentDictionary<long, string> dictionaryOfUserIdToUserName = new ConcurrentDictionary<long, string>();
        private static ConcurrentDictionary<string, long> dictionaryOfUserNameToUserId = new ConcurrentDictionary<string, long>();

        #region Instance

        private static volatile UserIdToUserNameDictionary _defaultInstance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 获取UserIdToUserNameAccessor实例
        /// </summary>
        /// <returns></returns>
        private static UserIdToUserNameDictionary Instance()
        {
            if (_defaultInstance == null)
            {
                lock (lockObject)
                {
                    if (_defaultInstance == null)
                    {
                        _defaultInstance = DIContainer.Resolve<UserIdToUserNameDictionary>();
                        if (_defaultInstance == null)
                            throw new ExceptionFacade("未在DIContainer注册UserIdToUserNameDictionary的具体实现类");
                    }
                }
            }
            return _defaultInstance;
        }

        #endregion

        /// <summary>
        /// 根据用户Id获取用户名
        /// </summary>
        /// <returns>
        /// 用户名
        /// </returns>
        protected abstract string GetUserNameByUserId(long userId);

        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <returns>
        /// 用户Id
        /// </returns>
        protected abstract long GetUserIdByUserName(string userName);


        /// <summary>
        /// 通过UserId获取UserName
        /// </summary>
        /// <param name="userId">userId</param>
        public static string GetUserName(long userId)
        {
            if (dictionaryOfUserIdToUserName.ContainsKey(userId))
                return dictionaryOfUserIdToUserName[userId];
            string userName = Instance().GetUserNameByUserId(userId);
            if (!string.IsNullOrEmpty(userName))
            {
                dictionaryOfUserIdToUserName[userId] = userName;
                if (!dictionaryOfUserNameToUserId.ContainsKey(userName))
                    dictionaryOfUserNameToUserId[userName] = userId;
                return userName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过UserName获取UserId
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static long GetUserId(string userName)
        {
            if (dictionaryOfUserNameToUserId.ContainsKey(userName))
                return dictionaryOfUserNameToUserId[userName];
            long userId = Instance().GetUserIdByUserName(userName);
            if (userId > 0)
            {
                dictionaryOfUserNameToUserId[userName] = userId;
                if (!dictionaryOfUserIdToUserName.ContainsKey(userId))
                    dictionaryOfUserIdToUserName[userId] = userName;
            }
            return userId;
        }

        /// <summary>
        /// 移除UserId
        /// </summary>
        /// <param name="userId">userId</param>
        internal static void RemoveUserId(long userId)
        {
            string userName;
            dictionaryOfUserIdToUserName.TryRemove(userId, out userName);
        }

        /// <summary>
        /// 移除UserId
        /// </summary>
        /// <param name="userName">userName</param>
        internal static void RemoveUserName(string userName)
        {
            long userId;
            dictionaryOfUserNameToUserId.TryRemove(userName, out userId);
        }
    }
}
