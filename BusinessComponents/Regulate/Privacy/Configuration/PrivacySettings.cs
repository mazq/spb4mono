//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 动态配置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class PrivacySettings:IEntity
    {
        private int specifyUserMaxCount = 10;
        /// <summary>
        /// 允许用户指定可见人数的最大值
        /// </summary>
        public int SpecifyUserMaxCount
        {
            get { return specifyUserMaxCount; }
            set { specifyUserMaxCount = value; }
        }

        private int stopUserMaxCount = 100;
        /// <summary>
        /// 黑名单人数限制值
        /// </summary>
        public int StopUserMaxCount 
        {
            get { return stopUserMaxCount; }
            set { stopUserMaxCount = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(PrivacySettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
        
    }
}
