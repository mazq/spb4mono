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
using Tunynet;
using Tunynet.Caching;
using Tunynet.Common;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TableName("spb_IdentificationTypes")]
    [PrimaryKey("IdentificationTypeId", autoIncrement = true)]
    [CacheSetting(false)]
    [Serializable]
    public class IdentificationType : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static IdentificationType New()
        {
            IdentificationType identificationType = new IdentificationType()
            {
                Name = string.Empty,
                Description = string.Empty,
                CreaterId = 0,
                DateCreated = DateTime.UtcNow

            };
            return identificationType;
        }

        #region 需持久化属性

        /// <summary>
        ///认证标识Id
        /// </summary>
        public long IdentificationTypeId { get; protected set; }

        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///创建人Id
        /// </summary>
        public long CreaterId { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// 认证标识图
        /// </summary>
        public string IdentificationTypeLogo { get; set; }

        #endregion

        public IUser GetUser
        {
            get {
                return new UserService().GetUser(this.CreaterId);
            }
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return this.IdentificationTypeId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}