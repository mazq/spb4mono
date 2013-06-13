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
    /// 权限实体类
    /// </summary>
    [TableName("tn_AuditItems")]
    [PrimaryKey("ItemKey", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class AuditItem : IEntity
    {
        #region 需持久化属性

        /// <summary>
        ///动态项目标志
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///应用程序id
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
