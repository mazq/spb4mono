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
    /// 积分项目实体类
    /// </summary>
    [TableName("tn_PointItems")]
    [PrimaryKey("ItemKey", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class PointItem : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static PointItem New()
        {
            PointItem pointItem = new PointItem()
            {
                ItemName = string.Empty,
                Description = string.Empty

            };
            return pointItem;
        }

        #region 需持久化属性

        /// <summary>
        ///积分项目标识
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///应用程序Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///经验积分值
        /// </summary>
        public int ExperiencePoints { get; set; }

        /// <summary>
        ///威望积分值
        /// </summary>
        public int ReputationPoints { get; set; }

        /// <summary>
        ///交易积分值
        /// </summary>
        public int TradePoints { get; set; }

        /// <summary>
        ///交易积分值2
        /// </summary>
        public int TradePoints2 { get; set; }

        /// <summary>
        ///交易积分值3
        /// </summary>
        public int TradePoints3 { get; set; }

        /// <summary>
        ///交易积分值4
        /// </summary>
        public int TradePoints4 { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.ItemKey; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
