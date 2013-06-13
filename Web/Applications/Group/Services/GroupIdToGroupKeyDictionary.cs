//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System.Collections.Concurrent;
using Tunynet;

namespace Spacebuilder.Group
{
    /// <summary>
    /// GroupId与GroupKey的查询器
    /// </summary>
    public abstract class GroupIdToGroupKeyDictionary
    {
        private static ConcurrentDictionary<long, string> dictionaryOfGroupIdToGroupKey = new ConcurrentDictionary<long, string>();
        private static ConcurrentDictionary<string, long> dictionaryOfGroupKeyToGroupId = new ConcurrentDictionary<string, long>();

        #region Instance

        private static volatile GroupIdToGroupKeyDictionary _defaultInstance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 获取GroupIdToGroupKeyAccessor实例
        /// </summary>
        /// <returns></returns>
        private static GroupIdToGroupKeyDictionary Instance()
        {
            if (_defaultInstance == null)
            {
                lock (lockObject)
                {
                    if (_defaultInstance == null)
                    {
                        _defaultInstance = DIContainer.Resolve<GroupIdToGroupKeyDictionary>();
                        if (_defaultInstance == null)
                            throw new ExceptionFacade("未在DIContainer注册GroupIdToGroupKeyDictionary的具体实现类");
                    }
                }
            }
            return _defaultInstance;
        }

        #endregion

        /// <summary>
        /// 根据群组Id获取群组Key
        /// </summary>
        /// <returns>
        /// 群组Key
        /// </returns>
        protected abstract string GetGroupKeyByGroupId(long groupId);

        /// <summary>
        /// 根据群组Key获取群组Id
        /// </summary>
        /// <returns>
        /// 群组Id
        /// </returns>
        protected abstract long GetGroupIdByGroupKey(string groupKey);


        /// <summary>
        /// 通过groupId获取groupKey
        /// </summary>
        /// <param name="GroupId">GroupId</param>
        public static string GetGroupKey(long groupId)
        {
            if (dictionaryOfGroupIdToGroupKey.ContainsKey(groupId))
                return dictionaryOfGroupIdToGroupKey[groupId];
            string groupKey = Instance().GetGroupKeyByGroupId(groupId);
            if (!string.IsNullOrEmpty(groupKey))
            {
                dictionaryOfGroupIdToGroupKey[groupId] = groupKey;
                if (!dictionaryOfGroupKeyToGroupId.ContainsKey(groupKey))
                    dictionaryOfGroupKeyToGroupId[groupKey] = groupId;
                return groupKey;
            }
            return string.Empty;
        }

        /// <summary>
        /// 通过groupKey获取groupId
        /// </summary>
        /// <param name="GroupKey"></param>
        /// <returns></returns>
        public static long GetGroupId(string groupKey)
        {
            if (dictionaryOfGroupKeyToGroupId.ContainsKey(groupKey))
                return dictionaryOfGroupKeyToGroupId[groupKey];
            long groupId = Instance().GetGroupIdByGroupKey(groupKey);
            if (groupId > 0)
            {
                dictionaryOfGroupKeyToGroupId[groupKey] = groupId;
                if (!dictionaryOfGroupIdToGroupKey.ContainsKey(groupId))
                    dictionaryOfGroupIdToGroupKey[groupId] = groupKey;
            }
            return groupId;
        }

        /// <summary>
        /// 移除GroupId
        /// </summary>
        /// <param name="groupId">groupId</param>
        internal static void RemoveGroupId(long groupId)
        {
            string groupKey;
            dictionaryOfGroupIdToGroupKey.TryRemove(groupId, out groupKey);
        }

        /// <summary>
        /// 移除GroupKey
        /// </summary>
        /// <param name="groupKey">groupKey</param>
        internal static void RemoveGroupKey(string groupKey)
        {
            long groupId;
            dictionaryOfGroupKeyToGroupId.TryRemove(groupKey, out groupId);
        }
    }
}