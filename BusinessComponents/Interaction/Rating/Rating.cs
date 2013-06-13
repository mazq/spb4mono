//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{  /// <summary>
    /// 星级评价实体
    /// </summary>
    [TableName("tn_Ratings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TenantTypeId,OwnerId")]
    [Serializable]
    public class Rating : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns></returns>
        public static Rating New()
        {
            Rating rating = new Rating()
            {
                Comprehensive = 0,
                Id = 0,
                ObjectId = 0,
                OwnerId = 0,
                RateCount = 0,
                TenantTypeId = string.Empty,
                RateSum = 0
            };
            return rating;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///操作对象Id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///评价总数
        /// </summary>
        public int RateCount { get; set; }

        /// <summary>
        ///评价结果
        /// </summary>
        public float Comprehensive { get; set; }

        /// <summary>
        /// 评价总分值
        /// </summary>
        public int RateSum { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}