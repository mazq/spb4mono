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

namespace Tunynet.Common
{
    /// <summary>
    /// 用户黑名单
    /// </summary>
    [TableName("tn_StopedUsers")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true,  PropertyNamesOfArea = "UserId", ExpirationPolicy=EntityCacheExpirationPolicies.Usual)]
    [Serializable]
    public class StopedUser : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static StopedUser New()
        {
            StopedUser stopedUser = new StopedUser()
            {
                ToUserDisplayName = string.Empty

            };
            return stopedUser;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///被阻止用户Id
        /// </summary>
        public long ToUserId { get; set; }

        /// <summary>
        ///被阻止用户名称
        /// </summary>
        public string ToUserDisplayName { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
