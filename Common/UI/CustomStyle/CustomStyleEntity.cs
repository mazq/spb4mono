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

namespace Spacebuilder.UI
{
    /// <summary>
    /// 自定义风格持久化实体
    /// </summary>
    [TableName("spb_CustomStyles")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Usual)]
    [Serializable]
    public class CustomStyleEntity : IEntity
    {

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static CustomStyleEntity New()
        {
            CustomStyleEntity customStyle = new CustomStyleEntity()
            {
                BackgroundImage = string.Empty,
                LastModified = DateTime.UtcNow
            };
            return customStyle;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///呈现区域标识
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        ///拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///定制样式序列化
        /// </summary>
        public string SerializedCustomStyle { get; set; }

        /// <summary>
        ///背景图片名称
        /// </summary>
        public string BackgroundImage { get; set; }

        /// <summary>
        ///最后更新时间
        /// </summary>
        public DateTime LastModified { get; set; }

        #endregion

        #region 扩展属性
        /// <summary>
        /// CustomStyle
        /// </summary>
        [Ignore]
        public CustomStyle CustomStyle { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
