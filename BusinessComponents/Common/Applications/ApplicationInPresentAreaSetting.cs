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
    /// 应用的呈现区域设置实体
    /// </summary>
    [TableName("tn_ApplicationInPresentAreaSettings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable)]
    [Serializable]
    public class ApplicationInPresentAreaSetting : IEntity
    {

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        ///应用Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///呈现区域标识
        /// </summary>
        public string PresentAreaKey { get; set; }

        /// <summary>
        ///是否为呈现区域内置应用，内置应用默认创建，并且不允许卸载
        /// </summary>
        public bool IsBuiltIn { get; set; }

        /// <summary>
        ///是否在呈现区域自动安装
        /// </summary>
        public bool IsAutoInstall { get; set; }

        /// <summary>
        ///应用在该呈现区域是否产生数据
        /// </summary>
        public bool IsGenerateData { get; set; }

        #endregion


        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
