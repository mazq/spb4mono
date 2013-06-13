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

namespace Tunynet.Common
{
    /// <summary>
    /// 站点Tag设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class TagSettings:IEntity
    {
        private int maxTagsCount = 5;
        /// <summary>
        /// 最大标签数
        /// </summary>
        public int MaxTagsCount
        {
            get { return maxTagsCount; }
            set { maxTagsCount = value; }
        }
        #region IEnity 成员

        object IEntity.EntityId
        {
            get { return typeof(TagSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }
       
        #endregion        
    }
}
