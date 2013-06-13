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
    public class ActivitySettings : IEntity
    {
        private int traceBackNumber = 10;
        /// <summary>
        /// 关注用户/加入群组时追溯添加的动态条数
        /// </summary>
        public int TraceBackNumber
        {
            get { return traceBackNumber; }
            set { traceBackNumber = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(ActivitySettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}
