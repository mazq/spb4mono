//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 用户关联实体类
    /// </summary>
    [TableName("tn_AtUsers")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserName")]
    [Serializable]
    public class AtUserEntity : IEntity
    {

        private AtUserEntity()
        {
        }

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static AtUserEntity New()
        {
            AtUserEntity favorite = new AtUserEntity()
            {
                UserName = string.Empty,
                TenantTypeId = string.Empty
            };

            return favorite;
        }

        #region 需持久化属性

        /// <summary>
        /// 标识列
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///关联的用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///关联项Id
        /// </summary>
        public long AssociateId { get; set; }

        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}