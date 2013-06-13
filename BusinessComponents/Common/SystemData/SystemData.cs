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
    /// 系统数据实体类
    /// </summary>
    [TableName("tn_SystemData")]
    [PrimaryKey("Datakey", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class SystemData : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static SystemData New()
        {
            SystemData systemData = new SystemData()
            {

            };
            return systemData;
        }

        #region 需持久化属性

        /// <summary>
        ///数据键值
        /// </summary>
        public string Datakey { get; set; }

        /// <summary>
        ///long数据值
        /// </summary>
        public long LongValue { get; set; }

        /// <summary>
        ///decimal数据值
        /// </summary>
        public decimal DecimalValue { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Datakey; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
