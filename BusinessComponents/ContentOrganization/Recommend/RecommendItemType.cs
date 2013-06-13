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
using Tunynet;

namespace Tunynet.Common
{
    //设计要点：
    //1、主键TypeId禁止update时更新；
    //2、缓存分区：TenantTypeId；

    /// <summary>
    /// 推荐类别
    /// </summary>
    [TableName("tn_RecommendItemTypes")]
    [PrimaryKey("TypeId", autoIncrement = false)]
    [CacheSetting(true, PropertyNamesOfArea = "TenantTypeId")]
    [Serializable]
    public class RecommendItemType : IEntity
    {
        /// <summary>
        /// 推荐类别
        /// </summary>
        public static RecommendItemType New()
        {
            RecommendItemType recommendItemType = new RecommendItemType()
            {
                Name = string.Empty,
                Description = string.Empty,
                DateCreated = DateTime.UtcNow

            };
            return recommendItemType;
        }

        #region 需持久化属性
        
        //已修改
        /// <summary>
        ///创建后不允许修改，建议格式为：6位TenantTypeId +2位顺序号
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public string TypeId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///推荐类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///推荐类型描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///是否包含标题图
        /// </summary>
        public bool HasFeaturedImage { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.TypeId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
