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
using Tunynet.Utilities;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Search.Repositories;

namespace Tunynet.Common
{
    /// <summary>
    /// 隐私业务逻辑类
    /// </summary>
    public class PrivacyService
    {

        private IPrivacyItemRepository privacyItemRepository;
        private IStopedUserRepository stopedUserRepository;
        private IUserPrivacySettingRepository userPrivacySettingRepository;
        private IUserPrivacySpecifyObjectRepository userPrivacySpecifyObjectRepository;

        #region 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        public PrivacyService()
            : this(new PrivacyItemRepository(), new StopedUserRepository(), new UserPrivacySettingRepository(), new UserPrivacySpecifyObjectRepository())
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="privacyItemRepository">PrivacyItemRepository仓储</param>
        /// <param name="stopedUserRepository">StopedUserRepository仓储</param>
        /// <param name="userPrivacySettingRepository">UserPrivacySettingRepository仓储</param>
        /// <param name="userPrivacySpecifyObjectRepository">UserPrivacySpecifyObjectRepository仓储</param>
        public PrivacyService(IPrivacyItemRepository privacyItemRepository, IStopedUserRepository stopedUserRepository, IUserPrivacySettingRepository userPrivacySettingRepository, IUserPrivacySpecifyObjectRepository userPrivacySpecifyObjectRepository)
        {
            this.privacyItemRepository = privacyItemRepository;
            this.stopedUserRepository = stopedUserRepository;
            this.userPrivacySettingRepository = userPrivacySettingRepository;
            this.userPrivacySpecifyObjectRepository = userPrivacySpecifyObjectRepository;
        }
        #endregion


        //获取隐私规则
        #region 隐私项目

        /// <summary>
        /// 获取PrivacyItem
        /// </summary>
        /// <param name="itemKey">隐私项标识</param>
        /// <returns>PrivacyItem</returns>
        public PrivacyItem GetPrivacyItem(string itemKey)
        {
            return privacyItemRepository.Get(itemKey);
        }

        /// <summary>
        /// 获取隐私项集合
        /// </summary>
        /// <param name="itemGroupId">隐私项目类型Id</param>
        /// <param name="applicationId">应用Id</param>
        /// <returns>隐私项集合</returns>
        public IEnumerable<PrivacyItem> GetPrivacyItems(int? itemGroupId, int? applicationId)
        {
            //done:zhengw,by mazq 怎么实现 “缓存策略：相对稳定列表”？
            //zhengw回复：在PrivacyItem类上加标记[CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]

            //缓存策略：相对稳定列表
            IEnumerable<PrivacyItem> allPrivacyItems = privacyItemRepository.GetAll("DisplayOrder");
            if (allPrivacyItems == null)
                return null;

            if (applicationId.HasValue && applicationId.Value > 0)
                allPrivacyItems = allPrivacyItems.Where(n => n.ApplicationId == applicationId.Value);
            if (itemGroupId.HasValue && itemGroupId.Value > 0)
                allPrivacyItems = allPrivacyItems.Where(n => n.ItemGroupId == itemGroupId.Value);
            return allPrivacyItems;
        }

        /// <summary>
        /// 更新隐私规则
        /// </summary>
        /// <param name="privacyItems">待更新的隐私项目规则集合</param>
        public void UpdatePrivacyItems(IEnumerable<PrivacyItem> privacyItems)
        {
            privacyItemRepository.UpdatePrivacyItems(privacyItems);
        }

        #endregion


        #region 用户隐私设置

        /// <summary>
        /// 把用户加入黑名单
        /// </summary>
        /// <param name="stopedUser">黑名单</param>
        public bool CreateStopedUser(StopedUser stopedUser)
        {
            FollowService followService = new FollowService();
            followService.CancelFollow(stopedUser.UserId, stopedUser.ToUserId);
            followService.CancelFollow(stopedUser.ToUserId, stopedUser.UserId);

            bool isCreat = stopedUserRepository.CreateStopedUser(stopedUser);
            EventBus<StopedUser>.Instance().OnAfter(stopedUser, new CommonEventArgs(EventOperationType.Instance().Create()));
            return isCreat;
        }

        /// <summary>
        /// 把用户从黑名单中删除
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="toUserId">被加入黑名单的UserId</param>
        public void DeleteStopedUser(long userId, long toUserId)
        {
            Dictionary<long, StopedUser> stopedUsers = GetStopedUsers(userId);
            if (!stopedUsers.ContainsKey(toUserId))
                return;
            StopedUser stopedUser = stopedUsers[toUserId];
            
            stopedUserRepository.DeleteStopedUser(stopedUser);
            EventBus<StopedUser>.Instance().OnAfter(stopedUser, new CommonEventArgs(EventOperationType.Instance().Delete()));
        }

        /// <summary>
        /// 获取用户的黑名单
        /// </summary>
        /// <returns><remarks>key=ToUserId,value=StopedUser</remarks></returns>
        public Dictionary<long, StopedUser> GetStopedUsers(long userId)
        {

            

            //缓存策略：常用
            return stopedUserRepository.GetStopedUsers(userId);
        }


        /// <summary>
        /// 更新用户的隐私设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userSettings"><remarks>key=itemKey,value=PrivacyStatus</remarks></param>
        /// <param name="specifyObjects"><remarks>key=itemKey,value=用户指定对象集合</remarks></param>
        public void UpdateUserPrivacySettings(long userId, Dictionary<string, PrivacyStatus> userSettings, Dictionary<string, IEnumerable<UserPrivacySpecifyObject>> specifyObjects)
        {
            //tn_UserPrivacySettings表中若有数据，则更新，否则插入userSettings
            //更新指定对象集合时，将旧数据集合不在新集合中的数据删除，将新集合中的数据不在旧集合中的插入；
            userPrivacySettingRepository.UpdateUserPrivacySettings(userId, userSettings);
            userPrivacySpecifyObjectRepository.UpdateUserPrivacySpecifyObjects(userId, specifyObjects);

        }

        /// <summary>
        /// 清空用户隐私设置（用于恢复到默认设置）
        /// </summary>
        /// <param name="userId"></param>
        public void ClearUserPrivacySettings(long userId)
        {
            //done:zhangp,by zhengw:同时清空用户隐私设置表和指定对象表中的用户数据
            userPrivacySettingRepository.ClearUserPrivacySettings(userId);
        }

        //编辑用户的隐私设置时
        //1、首先获取整站默认设置，然后用用户设置进行覆盖；

        /// <summary>
        /// 获取用户的隐私设置
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns><para>如果用户无设置返回空集合</para><remarks>key=itemKey,value=PrivacyStatus</remarks></returns>
        public Dictionary<string, PrivacyStatus> GetUserPrivacySettings(long userId)
        {
            //缓存策略：常用
            return userPrivacySettingRepository.GetUserPrivacySettings(userId);
        }

        /// <summary>
        /// 获取用户隐私设置指定对象集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="itemKey">隐私项目Key</param>
        /// <returns><remarks>key=specifyObjectTypeId,value=用户指定对象集合</remarks></returns>
        public Dictionary<int, IEnumerable<UserPrivacySpecifyObject>> GetUserPrivacySpecifyObjects(long userId, string itemKey)
        {
            //利用tn_UserPrivacySettings表和tn_UserPrivacySpecifyObjects表关联获取
            //缓存策略：常用
            return userPrivacySpecifyObjectRepository.GetUserPrivacySpecifyObjects(userId, itemKey);
        }

        #endregion

        #region 隐私验证

        /// <summary>
        /// toUserId是不是userId阻止的用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被判定用户Id</param>
        /// <returns>true-是黑名单用户，false-不是</returns>
        public bool IsStopedUser(long userId, long toUserId)
        {
            Dictionary<long, StopedUser> stopedUsers = GetStopedUsers(userId);
            if (stopedUsers != null && stopedUsers.ContainsKey(toUserId))
                return true;
            else
                return false;
        }

        ///<overloads>隐私验证</overloads>
        /// <summary>
        /// 隐私验证
        /// </summary>        
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被验证用户Id</param>
        /// <param name="itemKey">隐私项目Key</param>
        /// <returns>true-验证通过，false-验证失败</returns>
        public bool Validate(long userId, long toUserId, string itemKey)
        {
            if (toUserId == userId)
                return true;

            //被验证用户为超级管理员
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser toUser = userService.GetUser(toUserId);
            if (toUser != null)
            {
                if (toUser.IsInRoles(RoleNames.Instance().SuperAdministrator()))
                    return true;

                //被验证用户为黑名单用户
                if (IsStopedUser(userId, toUserId))
                    return false;
            }
            Dictionary<string, PrivacyStatus> userUserPrivacySettings = GetUserPrivacySettings(userId);
            if (userUserPrivacySettings.ContainsKey(itemKey))
            {
                switch (userUserPrivacySettings[itemKey])
                {
                    case PrivacyStatus.Public:
                        return true;
                    case PrivacyStatus.Part:
                        return toUser != null && ValidateUserPrivacySpecifyObject(userId, toUserId, itemKey);
                    case PrivacyStatus.Private:
                        return false;
                    default:
                        return false;
                }
            }
            var privacyItem = GetPrivacyItem(itemKey);
            if (privacyItem != null)
            {
                switch (privacyItem.PrivacyStatus)
                {
                    case PrivacyStatus.Private:
                        return false;
                    case PrivacyStatus.Part:
                        FollowService followService = new FollowService();
                        return followService.IsFollowed(userId, toUserId);
                    case PrivacyStatus.Public:
                        return true;
                    default:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 用户隐私验证指定对象
        /// </summary>        
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被验证用户Id</param>
        /// <param name="itemKey">隐私项目Key</param>
        /// <returns>true-验证通过，false-验证失败</returns>
        private bool ValidateUserPrivacySpecifyObject(long userId, long toUserId, string itemKey)
        {
            Dictionary<int, IEnumerable<UserPrivacySpecifyObject>> dictionary = GetUserPrivacySpecifyObjects(userId, itemKey);

            if (dictionary == null || dictionary.Count() == 0)
                return false;
            foreach (var pair in dictionary)
            {
                IPrivacySpecifyObjectValidator privacySpecifyUserGetter = DIContainer.ResolveNamed<IPrivacySpecifyObjectValidator>(pair.Key.ToString());
                foreach (var specifyObject in pair.Value)
                {
                    if (privacySpecifyUserGetter.Validate(userId, toUserId, specifyObject.SpecifyObjectId))
                        return true;
                }
            }
            return false;
        }

        #endregion
    }
}