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
using Tunynet;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 相关标签实体
    /// </summary>
    [TableName("tn_RelatedTags")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TagId")]
    [Serializable]
    public class RelatedTag : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static RelatedTag New()
        {
            RelatedTag relatedTags = new RelatedTag();
            return relatedTags;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///标签Id
        /// </summary>
        public long TagId { get; set; }

        /// <summary>
        ///相关标签Id
        /// </summary>
        public long RelatedTagId { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}