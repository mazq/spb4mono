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
    /// 应用数据实体类
    /// </summary>
    [TableName("tn_ApplicationData")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true)]
    [Serializable]
    public class ApplicationData : IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static ApplicationData New()
        {
            ApplicationData applicationData = new ApplicationData()
            {

            };
            return applicationData;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///ApplicationId
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///数据键值（要求Application内唯一）
        /// </summary>
        public string Datakey { get; set; }

        /// <summary>
        ///long数据值
        /// </summary>
        public long LongValue { get; set; }

        /// <summary>
        ///decimal数据值
        /// </summary>
        public decimal DecimalValue { get; set; }

        /// <summary>
        ///字符串数据值
        /// </summary>
        public string StringValue { get; set; }


        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Datakey; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
