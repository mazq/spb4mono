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
    /// 用户请求设置实体类
    /// </summary>
    [TableName("tn_UserInvitationSettings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class UserInvitationSettings : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static UserInvitationSettings New()
        {
            UserInvitationSettings userInvitationSetting = new UserInvitationSettings()
            {
                InvitationTypeKey = string.Empty
            };
            return userInvitationSetting;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///请求类型KEY
        /// </summary>
        public string InvitationTypeKey { get; set; }

        /// <summary>
        ///是否允许接受
        /// </summary>
        public bool IsAllowable { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
