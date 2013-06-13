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
    /// 用户数据实体类
    /// </summary>
    [TableName("tn_OwnerData")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class OwnerData : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static OwnerData New()
        {
            OwnerData applicationData = new OwnerData()
            {
                OwnerId = 0,
                TenantTypeId = string.Empty,
                Datakey = string.Empty,
                LongValue = 0,
                DecimalValue = 0,
                StringValue = string.Empty
            };
            return applicationData;
        }

        #region 需持久化属性

        ///<summary>
        ///Id
        ///</summary>
        public long Id { get; set; }

        ///<summary>
        ///UserId
        ///</summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        ///<summary>
        ///数据键值（要求在用户的DataKey中唯一）
        ///</summary>
        public string Datakey { get; set; }

        ///<summary>
        ///long数据值
        ///</summary>
        public long LongValue { get; set; }

        ///<summary>
        ///decimal数据值
        ///</summary>
        public decimal DecimalValue { get; set; }

        ///<summary>
        ///字符串数据值
        ///</summary>
        public string StringValue { get; set; }


        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
