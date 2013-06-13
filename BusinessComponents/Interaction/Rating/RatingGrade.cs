//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{   /// <summary>
    /// 星级评级等级统计实体
    /// </summary>
    [TableName("tn_RatingGrades")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "ObjectId")]
    [Serializable]
    public class RatingGrade : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        /// <returns></returns>
        public static RatingGrade New()
        {
            RatingGrade ratingGrade = new RatingGrade()
            {
                ObjectId = 0,
                RateCount = 0,
                RateNumber = 0,
                TenantTypeId = string.Empty
            };
            return ratingGrade;
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
        ///星级统计总数
        /// </summary>
        public int RateCount { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}