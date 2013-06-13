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

namespace Tunynet.UI
{
    /// <summary>
    /// 初始化常用操作实体
    /// </summary>
    [TableName("tn_CommonOperations")]
    [PrimaryKey("Id", autoIncrement = true)]
    //不使用缓存
    [CacheSetting(false)]
    [Serializable]
    public class CommonOperation : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static CommonOperation New()
        {
            CommonOperation commonOperation = new CommonOperation()
            {

            };
            return commonOperation;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///NavigationId
        /// </summary>
        public int NavigationId { get; set; }

        /// <summary>
        ///UserId
        /// </summary>
        public long UserId { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
