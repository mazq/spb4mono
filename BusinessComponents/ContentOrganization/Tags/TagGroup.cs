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
    /// 标签实体类
    /// </summary>
    [TableName("tn_TagGroups")]
    [PrimaryKey("GroupId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TenantTypeId")]
    [Serializable]
    public class TagGroup : IEntity
    {

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static TagGroup New()
        {
            return new TagGroup()
            {
                TenantTypeId = string.Empty,
                GroupName = string.Empty
            };

        }

        /// <summary>
        /// 分组Id
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        /// 标签分组名
        /// </summary>
        public string GroupName { get; set; }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.GroupId; } }
        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}