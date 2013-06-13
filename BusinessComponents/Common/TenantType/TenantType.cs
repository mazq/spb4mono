using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using PetaPoco;

namespace Tunynet.Common
{

    /// <summary>
    /// 用户数据实体类
    /// </summary>
    [TableName("tn_TenantTypes")]
    [PrimaryKey("TenantTypeId", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class TenantType : IEntity
    {
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        /// 租户类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ClassType { get; set; }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.TenantTypeId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}
