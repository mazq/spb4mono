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
    /// 分类与内容的关联项实体
    /// </summary>
    [TableName("tn_ItemsInCategories")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "ItemId,CategoryId")]
    [Serializable]
    public class ItemInCategory : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static ItemInCategory New()
        {
            ItemInCategory itemsInCategorie = new ItemInCategory()
            {

            };
            return itemsInCategorie;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///类别Id
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        ///内容项Id
        /// </summary>
        public long ItemId { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}

