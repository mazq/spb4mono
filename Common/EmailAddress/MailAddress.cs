//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet;
using Tunynet.Common;
using PetaPoco;
using Tunynet.Caching;
using Tunynet.Utilities;
using Spacebuilder.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 邮寄地址的实体
    /// </summary>
    [TableName("spb_MailAddress")]
    [PrimaryKey("AddressId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "AddressId,UserId")]
    [Serializable]
    public class MailAddress : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static MailAddress New()
        {
            MailAddress mailAddress = new MailAddress()
            {
                DateCreated = DateTime.UtcNow,
                LastModified=DateTime.UtcNow
            };

            return mailAddress;
        }

        #region 需持久化属性
        /// <summary>
        ///邮寄地址ID
        /// </summary>
        public long AddressId { get; protected set; }

        /// <summary>
        ///收件人
        /// </summary>
        public string Addressee { get; set; }

        /// <summary>
        ///联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        ///邮寄地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///邮编
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        ///用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        [SqlBehavior(~SqlBehaviorFlags.Update)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///修改日期
        /// </summary>
        public DateTime LastModified { get; set; }
        #endregion

        #region 扩展属性及方法

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.AddressId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion     
    }

}