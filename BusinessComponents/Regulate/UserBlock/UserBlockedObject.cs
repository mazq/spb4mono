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
    /// 用户屏蔽的对象
    /// </summary>
    [TableName("tn_UserBlockedObjects")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class UserBlockedObject : IEntity
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 被屏蔽的类型
        /// </summary>
        public int ObjectType { get; set; }

        /// <summary>
        /// 被屏蔽对象id
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// 被屏蔽对象名
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 屏蔽创建名
        /// </summary>
        public DateTime DateCreated { get; set; }


        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}
