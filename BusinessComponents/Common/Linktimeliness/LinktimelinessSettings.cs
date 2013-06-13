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
    /// 链接时效性配置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class LinktimelinessSettings:IEntity
    {
        private int _highlinktimeliness = 1;
        /// <summary>
        /// 高链接时效性期限配置(单位：天)
        /// </summary>
        public int Highlinktimeliness 
        {
            get { return _highlinktimeliness; }
            set { _highlinktimeliness = value; }
        }

        private int _lowlinktimeliness = 7;
        /// <summary>
        /// 低链接时效性期限配置(单位：天)
        /// </summary>
        public int Lowlinktimeliness 
        {
            get { return _lowlinktimeliness; }
            set { _lowlinktimeliness = value; }
        }


        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(LinktimelinessSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
        
    }
}