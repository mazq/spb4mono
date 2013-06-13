//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 星级评价记录实体
    /// </summary>
    [TableName("tn_RatingRecords")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TenantTypeId,ObjectId")]
    [Serializable]
    public class RatingRecord : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static RatingRecord New()
        {
            RatingRecord ratingRecord = new RatingRecord()
            {
                DateCreated = DateTime.UtcNow,
                RateNumber = 1,
                TenantTypeId = string.Empty
            };
            return ratingRecord;
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
        ///星级评价等级类型
        /// </summary>
        public int RateNumber { get; set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}