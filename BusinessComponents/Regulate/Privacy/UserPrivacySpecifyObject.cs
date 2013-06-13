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
    /// 用户隐私设置指定对象
    /// </summary>
    [TableName("tn_UserPrivacySpecifyObjects")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class UserPrivacySpecifyObject : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static UserPrivacySpecifyObject New()
        {
            UserPrivacySpecifyObject userPrivacySpecifyObject = new UserPrivacySpecifyObject()
            {
                SpecifyObjectName = string.Empty,
                DateCreated = DateTime.UtcNow

            };
            return userPrivacySpecifyObject;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///用户隐私设置Id
        /// </summary>
        public long UserPrivacySettingId { get; set; }

        /// <summary>
        ///被指定对象类型
        /// </summary>
        public int SpecifyObjectTypeId { get; set; }

        /// <summary>
        ///被指定对象Id
        /// </summary>
        public long SpecifyObjectId { get; set; }

        /// <summary>
        ///被指定对象名称
        /// </summary>
        public string SpecifyObjectName { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
