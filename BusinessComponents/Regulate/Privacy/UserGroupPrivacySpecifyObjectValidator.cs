//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tunynet.Common
{
    /// <summary>
    /// 隐私指定关注分组验证器
    /// </summary>
    public class UserGroupPrivacySpecifyObjectValidator : IPrivacySpecifyObjectValidator
    {
        /// <summary>
        /// 验证指定对象针对toUserId是否具有隐私权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="toUserId">被验证用户</param>
        /// <param name="specifyObjectId">指定对象Id</param>
        /// <returns>true-成功，false-失败</returns>
        bool IPrivacySpecifyObjectValidator.Validate(long userId, long toUserId, long specifyObjectId)
        {
            //done:zhengw,by mazq  
            //1、所有分组Id是什么？
            //2、相互关注等特殊规则怎么制定的，编码的人遵照什么来编码
            //zhengw回复：使用FollowSpecifyGroupIds.All获取所有分组Id，具体参见：Examples\BusinessComponents\User\Follow\FollowEnum.cs中的FollowSpecifyGroupIds类

            //如果specifyObjectId为所有分组Id，则仅判断下toUserId是不是关注的人即可
            FollowService followService = new FollowService();
            if (specifyObjectId == FollowSpecifyGroupIds.All)
                return followService.IsFollowed(userId, toUserId);
            else if (specifyObjectId == FollowSpecifyGroupIds.Mutual)
                return followService.IsMutualFollowed(userId, toUserId);
            else
            {
                
                FollowEntity follow = followService.Get(userId, toUserId);
                if (follow == null)
                    return false;
                IEnumerable<Category> categories = new CategoryService().GetCategoriesOfItem(follow.Id, userId, TenantTypeIds.Instance().User());
                if (categories == null)
                    return false;
                return categories.Any(n => n.CategoryId == specifyObjectId);
            }
        }
    }
}