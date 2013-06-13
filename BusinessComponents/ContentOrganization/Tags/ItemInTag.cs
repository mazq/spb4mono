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
    /// <summary>
    /// 标签与内容的关联项实体
    /// </summary>
    [TableName("tn_ItemsInTags")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "ItemId,TagName")]
    [Serializable]
    public class ItemInTag : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static ItemInTag New()
        {
            ItemInTag tagInTag = new ItemInTag();
            return tagInTag;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 标签与拥有者关联Id
        /// </summary>
        public long TagInOwnerId { get; set; }

        /// <summary>
        ///内容项Id
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}

