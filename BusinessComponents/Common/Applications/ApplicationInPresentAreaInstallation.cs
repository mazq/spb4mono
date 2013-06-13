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
    /// 应用在呈现区域安装记录实体
    /// </summary>
    [TableName("tn_ApplicationInPresentAreaInstallations")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, ExpirationPolicy = EntityCacheExpirationPolicies.Stable, PropertyNamesOfArea = "OwnerId")]
    [Serializable]
    public class ApplicationInPresentAreaInstallation : IEntity
    {

        #region 需持久化属性

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        ///呈现区域实例拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///应用程序Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///呈现区域标识
        /// </summary>
        public string PresentAreaKey { get; set; }

        #endregion


        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
