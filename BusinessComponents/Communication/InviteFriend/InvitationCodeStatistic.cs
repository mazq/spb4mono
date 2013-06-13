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
    /// 用户配额实体
    /// </summary>
    [TableName("tn_InvitationCodeStatistics")]
    [PrimaryKey("UserId", autoIncrement = false)]
    
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class InvitationCodeStatistic : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static InvitationCodeStatistic New()
        {
            InvitationCodeStatistic invitationCodeStatistic = new InvitationCodeStatistic()
            {

            };
            return invitationCodeStatistic;
        }

        #region 需持久化属性

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 未使用的配额
        /// </summary>
        public int CodeUnUsedCount { get; set; }

        /// <summary>
        /// 已经使用的配额
        /// </summary>
        public int CodeUsedCount { get; set; }

        /// <summary>
        /// 购买过的数目
        /// </summary>
        public int CodeBuyedCount { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.UserId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
