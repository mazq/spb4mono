//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;

namespace Tunynet.Common
{

    /// <summary>
    /// 隐私项目
    /// </summary>
    [TableName("tn_PrivacyItems")]
    [PrimaryKey("ItemKey", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class PrivacyItem : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static PrivacyItem New()
        {
            PrivacyItem privacyItem = new PrivacyItem()
            {
                ItemName = string.Empty,
                Description = string.Empty
            };
            return privacyItem;
        }

        #region 需持久化属性

        /// <summary>
        ///隐私项目标识
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///隐私项目分组Id
        /// </summary>
        public int ItemGroupId { get; set; }

        /// <summary>
        ///应用程序Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///隐私项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        ///隐私项目描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///隐私状态
        /// </summary>
        public PrivacyStatus PrivacyStatus { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.ItemKey; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
