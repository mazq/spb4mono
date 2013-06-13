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
    /// 在线用户实体类
    /// </summary>
    [TableName("tn_OnlineUserStatistics")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Usual)]
    [Serializable]
    public class OnlineUserStatistic : IEntity
    {

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static OnlineUserStatistic New()
        {
            OnlineUserStatistic onlineUserStatistic = new OnlineUserStatistic()
            {
                DateCreated = DateTime.UtcNow

            };
            return onlineUserStatistic;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        ///在线登录用户数
        /// </summary>
        public int LoggedUserCount { get; set; }

        /// <summary>
        ///在线匿名用户数
        /// </summary>
        public int AnonymousCount { get; set; }

        /// <summary>
        ///在线用户数
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
