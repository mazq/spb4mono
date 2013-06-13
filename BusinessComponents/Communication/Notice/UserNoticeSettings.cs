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
    /// 用户通知设置类
    /// </summary>
    [TableName("tn_UserNoticeSettings")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class UserNoticeSettings : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static UserNoticeSettings New()
        {
            UserNoticeSettings userNoticeSetting = new UserNoticeSettings();
            return userNoticeSetting;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///类型Id
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 是否允许发送
        /// </summary>
        public bool IsAllowable { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
