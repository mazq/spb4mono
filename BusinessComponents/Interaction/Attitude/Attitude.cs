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
    /// 顶踩实体
    /// </summary>
    [TableName("tn_Attitudes")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TenantTypeId")]
    [Serializable]
    public class Attitude : IEntity
    {
        /// <summary>
        /// 实体初始化方法
        /// </summary>
        /// <returns></returns>
        public static Attitude New()
        {
            Attitude attitude = new Attitude()
            {
                ObjectId = 0,
                OpposeCount = 0,
                SupportCount = 0,
                TenantTypeId = string.Empty,
                Comprehensive = 0D
            };
            return attitude;
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
        ///支持数
        /// </summary>
        public int SupportCount { get; set; }

        /// <summary>
        ///反对数
        /// </summary>
        public int OpposeCount { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///综合评价值（根据支持反对数加权所得）
        /// </summary>
        public double Comprehensive { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}