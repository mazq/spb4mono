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

namespace Tunynet.Common
{
    //done:zhangp,by zhengw:这个类没完成
    //回复：已修改

    //done:zhangp,by zhengw:注意处理警告信息,公开方法要求必须加注释
    //回复：已修改

    /// <summary>
    /// 内容隐私业务逻辑类
    /// </summary>
    public class ContentPrivacyService
    {

        private IContentPrivacySpecifyObjectsRepository contentPrivacySpecifyObjectsRepository;

        /// <summary>
        /// 构造器
        /// </summary>
        public ContentPrivacyService() : this(new ContentPrivacySpecifyObjectsRepository()) { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="contentPrivacySpecifyObjectsRepository">内容隐私仓储</param>
        public ContentPrivacyService(IContentPrivacySpecifyObjectsRepository contentPrivacySpecifyObjectsRepository)
        {
            this.contentPrivacySpecifyObjectsRepository = contentPrivacySpecifyObjectsRepository;
        }

        //done:zhengw,by mazq  已经是一个独立类了，就不要加Content了
        //zhengw回复：已修改
        /// <summary>
        /// 更新内容隐私设置
        /// </summary>
        /// <param name="privacyable">可隐私接口</param>
        /// <param name="specifyObjects"><remarks>key=specifyObjectTypeId,value=内容指定对象集合</remarks></param>
        public void UpdatePrivacySettings(IPrivacyable privacyable, Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> specifyObjects)
        {
            //更新指定对象集合时，将旧数据集合不在新集合中的数据删除，将新集合中的数据不在旧集合中的插入；
            contentPrivacySpecifyObjectsRepository.UpdatePrivacySettings(privacyable, specifyObjects);
        }

        /// <summary>
        /// 获取内容隐私设置指定对象
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <param name="contentId">内容Id</param>
        /// <returns><remarks>key=specifyObjectTypeId,value=内容指定对象集合</remarks></returns>
        public Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> GetPrivacySpecifyObjects(string tenantTypeId, long contentId)
        {
            //缓存策略：常用
            return contentPrivacySpecifyObjectsRepository.GetPrivacySpecifyObjects(tenantTypeId, contentId);
        }

        #region 内容隐私验证
        /// <summary>
        /// 内容隐私验证
        /// </summary>        
        /// <param name="privacyable">可隐私接口</param>
        /// <param name="toUserId">被验证用户Id</param>
        /// <returns>true-验证通过，false-验证失败</returns>
        public bool Validate(IPrivacyable privacyable, long toUserId)
        {
            if (privacyable.PrivacyStatus == PrivacyStatus.Public)
                return true;
            if (privacyable.PrivacyStatus == PrivacyStatus.Private)
                return false;

            if (toUserId == privacyable.UserId)
                return true;

            //被验证用户为超级管理员
            IUserService userService = DIContainer.Resolve<IUserService>();
            IUser toUser = userService.GetUser(toUserId);
            if (toUser.IsInRoles(RoleNames.Instance().SuperAdministrator()))
                return true;

            //被验证用户为黑名单用户
            PrivacyService privacyService = new PrivacyService();
            if (privacyService.IsStopedUser(privacyable.UserId, toUserId))
                return false;

            //判断指定对象可见
            Dictionary<int, IEnumerable<ContentPrivacySpecifyObject>> dictionary = GetPrivacySpecifyObjects(privacyable.TenantTypeId, privacyable.ContentId);
            if (dictionary == null || dictionary.Count() == 0)
                return false;
            foreach (var pair in dictionary)
            {
                IPrivacySpecifyObjectValidator privacySpecifyUserGetter = DIContainer.ResolveNamed<IPrivacySpecifyObjectValidator>(pair.Key.ToString());
                foreach (var specifyObject in pair.Value)
                {
                    if (privacySpecifyUserGetter.Validate(privacyable.UserId, toUserId, specifyObject.SpecifyObjectId))
                        return true;
                }
            }
            return false;
        }

        #endregion
    }
}