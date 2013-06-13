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
    public abstract class UserIdToNickNameDictionary
    {

        private static ConcurrentDictionary<long, string> dictionaryOfUserIdToNickName = new ConcurrentDictionary<long, string>();
        private static ConcurrentDictionary<string, long> dictionaryOfNickNameToUserId = new ConcurrentDictionary<string, long>();

        #region Instance

        private static volatile UserIdToNickNameDictionary _defaultInstance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 获取UserIdToUserNameAccessor实例
        /// </summary>
        /// <returns></returns>
        private static UserIdToNickNameDictionary Instance()
        {
            if (_defaultInstance == null)
            {
                lock (lockObject)
                {
                    if (_defaultInstance == null)
                    {
                        _defaultInstance = DIContainer.Resolve<UserIdToNickNameDictionary>();
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
        protected abstract string GetNickNameByUserId(long userId);

        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <returns>
        /// 用户Id
        /// </returns>
        protected abstract long GetUserIdByNickName(string userName);


        /// <summary>
        /// 通过UserId获取UserName
        /// </summary>
        /// <param name="userId">userId</param>
        public static string GetNickName(long userId)
        {
            if (dictionaryOfUserIdToNickName.ContainsKey(userId))
                return dictionaryOfUserIdToNickName[userId];
            string userName = Instance().GetNickNameByUserId(userId);
            if (!string.IsNullOrEmpty(userName))
            {
                dictionaryOfUserIdToNickName[userId] = userName;
                if (!dictionaryOfNickNameToUserId.ContainsKey(userName))
                    dictionaryOfNickNameToUserId[userName] = userId;
                return userName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过UserName获取UserId
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static long GetUserId(string nickName)
        {
            if (dictionaryOfNickNameToUserId.ContainsKey(nickName))
                return dictionaryOfNickNameToUserId[nickName];
            long userId = Instance().GetUserIdByNickName(nickName);
            if (userId > 0)
            {
                dictionaryOfNickNameToUserId[nickName] = userId;
                if (!dictionaryOfUserIdToNickName.ContainsKey(userId))
                    dictionaryOfUserIdToNickName[userId] = nickName;
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
            dictionaryOfUserIdToNickName.TryRemove(userId, out userName);
        }

        /// <summary>
        /// 移除UserId
        /// </summary>
        /// <param name="userName">userName</param>
        internal static void RemoveUserName(string userName)
        {
            long userId;
            dictionaryOfNickNameToUserId.TryRemove(userName, out userId);
        }
    }
}
