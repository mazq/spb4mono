//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Events;
using Tunynet.Common.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户屏蔽业务逻辑类
    /// </summary>
    public class UserBlockService
    {

        private IUserBlockRepository userBlockRepository = null;

        /// <summary>
        /// 屏蔽用户屏蔽群组
        /// </summary>
        public UserBlockService() : this(new UserBlockRepository()) { }

        /// <summary>
        /// 带参数的构造方法
        /// </summary>
        /// <param name="userBlockRepository"></param>
        public UserBlockService(IUserBlockRepository userBlockRepository)
        {
            this.userBlockRepository = userBlockRepository;
        }

        /// <summary>
        /// 屏蔽用户
        /// </summary>
        /// <param name="userId">屏蔽人UserId</param>
        /// <param name="blockedUserId">被屏蔽UserId</param>
        /// <param name="blockedDisplayName">被屏蔽用户DisplayName</param>
        public bool BlockUser(long userId, long blockedUserId, string blockedDisplayName)
        {
            return Create(userId, BlockedObjectTypes.Instance().User(), blockedUserId, blockedDisplayName);
        }

        /// <summary>
        /// 屏蔽群组
        /// </summary>
        /// <param name="userId">屏蔽人UserId</param>
        /// <param name="blockedGroupId">被屏蔽群组Id</param>
        /// <param name="blockedGroupName">被屏蔽群组名称</param>
        public bool BlockGroup(long userId, long blockedGroupId, string blockedGroupName)
        {
            return Create(userId, BlockedObjectTypes.Instance().Group(), blockedGroupId, blockedGroupName);
        }

        /// <summary>
        /// 添加屏蔽对象
        /// </summary>
        /// <param name="userId">屏蔽人UserId</param>
        /// <param name="objectType">被屏蔽对象类型</param>
        /// <param name="objectId">被屏蔽对象Id</param>
        /// <param name="objectName">被屏蔽对象名称</param>
        public bool Create(long userId, int objectType, long objectId, string objectName)
        {
            //在Repository添加被屏蔽对象时，需要检查userId +  objectType + objectId 是否已存在，如果已存在则不添加
            UserBlockedObject userBlockedObject = new UserBlockedObject
            {
                ObjectId = objectId,
                ObjectName = objectName,
                ObjectType = objectType,
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };
            EventBus<UserBlockedObject>.Instance().OnBefore(userBlockedObject, new CommonEventArgs(EventOperationType.Instance().Create()));
            if (IsBlocked(userId, objectType, objectId))
                return false;
            object blockId = userBlockRepository.Insert(userBlockedObject) ?? 0;
            if (Convert.ToInt64(blockId) > 0)
            {
                EventBus<UserBlockedObject>.Instance().OnAfter(userBlockedObject, new CommonEventArgs(EventOperationType.Instance().Create()));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除被屏蔽对象
        /// </summary>
        /// <param name="id">Id</param>
        public bool Delete(long id)
        {
            UserBlockedObject userBlockedObject = userBlockRepository.Get(id);
            EventBus<UserBlockedObject>.Instance().OnBefore(userBlockedObject, new CommonEventArgs(EventOperationType.Instance().Delete()));
            if (userBlockedObject != null && userBlockRepository.Delete(userBlockedObject) > 0)
            {
                EventBus<UserBlockedObject>.Instance().OnAfter(userBlockedObject, new CommonEventArgs(EventOperationType.Instance().Delete()));
                return true;
            }
            return false;
        }


        /// <summary>
        /// 用户是否被屏蔽
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="checkingUserId">被检查的UserId</param>
        /// <returns>如果checkingUserId被userId屏蔽，则返回true</returns>
        public bool IsBlockedUser(long userId, long checkingUserId)
        {
            return IsBlocked(userId, BlockedObjectTypes.Instance().User(), checkingUserId);
        }

        /// <summary>
        /// 群组是否被屏蔽
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="checkingGroupId">被检查的群组Id</param>
        /// <returns>如果checkingGroupId被userId屏蔽，则返回true</returns>
        public bool IsBlockedGroup(long userId, long checkingGroupId)
        {
            return IsBlocked(userId, BlockedObjectTypes.Instance().Group(), checkingGroupId);
        }

        /// <summary>
        /// 是否被屏蔽
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="objectType">被屏蔽对象类型</param>
        /// <param name="objectId">被屏蔽对象Id</param>
        /// <returns>如果objectId被userId屏蔽，则返回true</returns>
        public bool IsBlocked(long userId, int objectType, long objectId)
        {
            //在GetBlockedObjects()返回的集合中进行检查
            IEnumerable<UserBlockedObject> userBlockedObjects = GetBlockedObjects(userId, objectType);
            return userBlockedObjects.Any(n => n.UserId == userId && n.ObjectType == objectType && n.ObjectId == objectId);
        }


        /// <summary>
        /// 获取用户的屏蔽用户列表
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public IEnumerable<UserBlockedObject> GetBlockedUsers(long userId)
        {
            return GetBlockedObjects(userId, BlockedObjectTypes.Instance().User());
        }

        /// <summary>
        /// 获取用户的屏蔽群组列表
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public IEnumerable<UserBlockedObject> GetBlockedGroups(long userId)
        {
            return GetBlockedObjects(userId, BlockedObjectTypes.Instance().Group());
        }

        /// <summary>
        /// 获取y用户的屏蔽对象列表
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="objectType">被屏蔽对象类型</param>
        /// <returns></returns>
        public IEnumerable<UserBlockedObject> GetBlockedObjects(long userId, int objectType)
        {
            //缓存策略：CachingExpirationType.RelativelyStable
            return userBlockRepository.GetBlockedObjects(userId, objectType);
        }

        /// <summary>
        /// 清除数据根据用户id（删除用户的时候使用）
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>是否成功清除数据</returns>
        public bool CleanByUser(long userId)
        {
            return userBlockRepository.CleanByUser(userId);
        }
    }
}
