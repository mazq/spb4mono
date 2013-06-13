//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using Tunynet.Caching;
namespace Tunynet.Common
{
    /// <summary>
    /// 星级评价的配置设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class RatingSettings:IEntity
    {
        /// <summary>
        /// 是否允许修改星级评价
        /// </summary>
        public bool IsModify { get; set; }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(RatingSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
        
    }
}