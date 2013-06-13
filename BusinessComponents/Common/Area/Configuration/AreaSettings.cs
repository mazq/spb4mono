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
    /// 站点地区设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class AreaSettings : IEntity
    {
        private int areaLevel = 4;
        /// <summary>
        /// 默认使用地区的层级
        /// </summary>
        /// <remarks>1-国家级、2-省级（直辖市）、3-市级、4-县级</remarks>
        public int AreaLevel
        {
            get { return areaLevel; }
            set { areaLevel = value; }
        }

        private string rootAreaCode = "A1560000";
        /// <summary>
        /// 地区的根地区Code
        /// </summary>
        public string RootAreaCode
        {
            get { return rootAreaCode; }
            set { rootAreaCode = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId { get { return typeof(AreaSettings).FullName; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
