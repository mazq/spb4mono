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
    /// 应用元数据
    /// </summary>
    [TableName("tn_Applications")]
    [PrimaryKey("ApplicationId", autoIncrement = false)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class ApplicationModel : IEntity
    {
        #region 需持久化属性

        /// <summary>
        ///应用程序Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///Application英文唯一标识
        /// </summary>
        public string ApplicationKey { get; set; }

       

        /// <summary>
        /// 应用描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.ApplicationId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
