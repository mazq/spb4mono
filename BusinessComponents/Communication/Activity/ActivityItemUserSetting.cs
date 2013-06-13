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
    /// 动态项目用户设置
    /// </summary>
    [TableName("tn_ActivityUserSettings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Usual)]
    [Serializable]
    public class ActivityItemUserSetting : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static ActivityItemUserSetting New()
        {
            ActivityItemUserSetting activityItemUserSetting = new ActivityItemUserSetting()
            {

            };
            return activityItemUserSetting;
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
        ///动态项目标识
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///是否接收
        /// </summary>
        public bool IsReceived { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
