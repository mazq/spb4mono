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
    /// 用户隐私设置
    /// </summary>
    [TableName("tn_UserPrivacySettings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true,  PropertyNamesOfArea = "UserId",ExpirationPolicy = EntityCacheExpirationPolicies.Usual)]
    [Serializable]
    public class UserPrivacySetting : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static UserPrivacySetting New()
        {
            UserPrivacySetting userPrivacySetting = new UserPrivacySetting()
            {
               
            };
            return userPrivacySetting;
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
        ///类型Id
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///隐私状态
        /// </summary>
        public PrivacyStatus PrivacyStatus { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
