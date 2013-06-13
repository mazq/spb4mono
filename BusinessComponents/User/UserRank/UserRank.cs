//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using PetaPoco;
using Tunynet.Caching;
using System;
using Tunynet;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户等级实体
    /// </summary>
    [TableName("tn_UserRanks")]
    [PrimaryKey("Rank", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class UserRank : IEntity
    {
        #region 需持久化属性

        /// <summary>
        ///级别（从1开始）
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        ///积分下限
        /// </summary>
        public int PointLower { get; set; }

        /// <summary>
        ///等级名称
        /// </summary>
        public string RankName { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Rank; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
