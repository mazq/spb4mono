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
    /// 访客记录实体
    /// </summary>
    [TableName("tn_Visit")]
    [PrimaryKey("Id", autoIncrement = false)]
    [CacheSetting(true)]
    [Serializable]
    public class Visit : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Visit New()
        {
            Visit visit = new Visit()
            {
                Visitor = string.Empty,
                ToObjectName = string.Empty,
                LastVisitTime = DateTime.UtcNow

            };
            return visit;
        }

        #region 需持久化属性

        /// <summary>
        ///id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///租户类型id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///访客用户id
        /// </summary>
        public long VisitorId { get; set; }

        /// <summary>
        ///访客名称
        /// </summary>
        public string Visitor { get; set; }

        /// <summary>
        ///被访问对象id
        /// </summary>
        public long ToObjectId { get; set; }

        /// <summary>
        ///被访问对象名称
        /// </summary>
        public string ToObjectName { get; set; }

        /// <summary>
        ///最后访问时间
        /// </summary>
        public DateTime LastVisitTime { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
