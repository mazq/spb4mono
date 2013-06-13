//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Repositories;
using Tunynet.Utilities;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 短网址配置类
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class ShortUrlSettings : IEntity
    {
        private string shortUrlDomain = "";
        /// <summary>
        /// 内置短网址处理主域名
        /// </summary>
        public string ShortUrlDomain
        {
            get { return shortUrlDomain; }
            set { shortUrlDomain = value; }
        }

        private bool isEnableOtherShortner = false;
        /// <summary>
        /// 启用第三方短网址处理
        /// </summary>
        public bool IsEnableOtherShortner
        {
            get { return isEnableOtherShortner; }
            set { isEnableOtherShortner = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(ShortUrlSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}