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
    /// 积分类型实体类
    /// </summary>
    [TableName("tn_PointCategories")]
    [PrimaryKey("CategoryKey", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class PointCategory : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static PointCategory New()
        {
            PointCategory pointCategorie = new PointCategory()
            {
                CategoryName = string.Empty,
                Unit = string.Empty,
                Description = string.Empty

            };
            return pointCategorie;
        }

        #region 需持久化属性

        /// <summary>
        ///积分类型标识
        /// </summary>
        public string CategoryKey { get; set; }

        /// <summary>
        ///类型名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        ///单位名称
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        ///每人每日该类限额（0表示无限制）
        /// </summary>
        public int QuotaPerDay { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.CategoryKey; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}




