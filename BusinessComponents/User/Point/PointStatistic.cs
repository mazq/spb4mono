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
    /// 积分统计实体类
    /// </summary>
    [TableName("tn_PointStatistics")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Normal, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class PointStatistic : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static PointStatistic New()
        {
            PointStatistic pointStatistic = new PointStatistic()
            {
                StatisticalDay = DateTime.UtcNow.Day,
                StatisticalMonth = DateTime.UtcNow.Month,
                StatisticalYear = DateTime.UtcNow.Year
            };
            return pointStatistic;
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
        ///积分类型标识
        /// </summary>
        public string PointCategoryKey { get; set; }

        /// <summary>
        ///积分值
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        ///统计年份
        /// </summary>
        public int StatisticalYear { get; set; }

        /// <summary>
        ///统计月份
        /// </summary>
        public int StatisticalMonth { get; set; }

        /// <summary>
        ///统计月份的第几天
        /// </summary>
        public int StatisticalDay { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
