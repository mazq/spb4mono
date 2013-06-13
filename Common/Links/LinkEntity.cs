//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;
using System.Collections.Generic;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 友情链接实体
    /// </summary>
    [TableName("spb_Links")]
    [PrimaryKey("LinkId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "OwnerId")]
    [Serializable]
    public class LinkEntity : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static LinkEntity New()
        {
            LinkEntity link = new LinkEntity()
            {
                ImageUrl = string.Empty,
                Description = string.Empty,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
            return link;
        }

        #region 需持久化属性
        /// <summary>
        ///友情链接ID
        /// </summary>
        public long LinkId { get; protected set; }

        /// <summary>
        ///友情链接拥有者类型
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        ///链接拥有者Id（如用户Id/群组Id）
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///链接名称
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        ///链接类型（0 - 文字链接 1- 图像链接）
        /// </summary>
        public LinkType LinkType { get; set; }

        /// <summary>
        ///链接地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        ///Logo地址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///链接说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///排序，默认与主键相同
        /// </summary>
        public long DisplayOrder { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public DateTime DateCreated { get; protected set; }

        /// <summary>
        ///修改时间
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///PropertyNames
        /// </summary>
        public string PropertyNames { get; set; }

        /// <summary>
        ///PropertyValues
        /// </summary>
        public string PropertyValues { get; set; }

        #endregion

        #region 扩展属性及方法


        [Ignore]
        public IEnumerable<Category> Categories
        {
            get
            {
                return new CategoryService().GetCategoriesOfItem(this.LinkId, 0, TenantTypeIds.Instance().Link());
            }
        }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.LinkId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
