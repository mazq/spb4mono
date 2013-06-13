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
    /// 顶踩记录实体
    /// </summary>
    [TableName("tn_AttitudeRecords")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "ObjectId")]
    [Serializable]
    public class AttitudeRecord : IEntity
    {
        /// <summary>
        /// 实体初始化方法
        /// </summary>
        /// <returns></returns>
        public static AttitudeRecord New()
        {
            AttitudeRecord attitudeRecord = new AttitudeRecord()
            {
                TenantTypeId = string.Empty,
                ObjectId = 0,
                UserId = 0
            };
            return attitudeRecord;
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
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///用户是否支持（true为支持false为反对）
        /// </summary>
        public bool IsSupport { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}