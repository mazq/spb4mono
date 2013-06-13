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
using System.Collections.Concurrent;

namespace Tunynet.Common
{
    /// <summary>
    /// 第三方账号类型
    /// </summary>
    [TableName("tn_AccountTypes")]
    [PrimaryKey("AccountTypeKey", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class AccountType : IEntity
    {

        #region 构造函数

        /// <summary>
        /// 创建第三方账号类型
        /// </summary>
        /// <param name="user"></param>
        public static AccountType New()
        {
            
            AccountType accountType = new AccountType()
            {
                AccountTypeKey = string.Empty,
                AppKey = string.Empty,
                AppSecret = string.Empty,
                OfficialMicroBlogAccount = string.Empty
            };
            return accountType;
        }

        #endregion

        #region 需持久化属性

        /// <summary>
        ///第三方账号类型标识
        /// </summary>
        public string AccountTypeKey { get; set; }

        /// <summary>
        /// 第三方账号获取器实现类Type值（如：Spacebuilder.Group.GroupConfig,Spacebuilder.Group）
        /// </summary>
        public string ThirdAccountGetterClassType { get; set; }

        /// <summary>
        ///网站接入应用标识
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        ///网站接入应用加密串
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        ///是否同步发布微博
        /// </summary>
        public bool IsSync { get; set; }

        /// <summary>
        ///绑定成功时是否分享一条微博
        /// </summary>
        public bool IsShareMicroBlog { get; set; }

        /// <summary>
        ///是否关注指定微博
        /// </summary>
        public bool IsFollowMicroBlog { get; set; }

        /// <summary>
        ///官方微博账号
        /// </summary>
        public string OfficialMicroBlogAccount { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.AccountTypeKey; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}