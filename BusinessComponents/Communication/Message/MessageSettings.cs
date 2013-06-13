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
    /// 私信设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class MessageSettings:IEntity
    {
        private int maxReceiver = 5;
        /// <summary>
        /// 最多同时选择收件人个数
        /// </summary>
        public int MaxReceiver
        {
            get { return maxReceiver; }
            set { maxReceiver = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(MessageSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
        
    }
}
