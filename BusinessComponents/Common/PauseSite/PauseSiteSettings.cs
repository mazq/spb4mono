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
    [CacheSetting(true)]
    [Serializable]
    public class PauseSiteSettings:IEntity
    {
        /// <summary>
        /// 站点状态
        /// </summary>
        private bool isEnable = true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }

        ///// <summary>
        ///// 是否允许登陆后台
        ///// </summary>
        //private bool allowLoginBackGround = false;
        //public bool AllowLoginBackGround
        //{
        //    get { return allowLoginBackGround; }
        //    set { allowLoginBackGround = value; }
        //}

        /// <summary>
        /// 暂停页面类型
        /// </summary>
        private bool pausePageType = false;
        public bool PausePageType 
        {
            get { return pausePageType; }
            set { pausePageType = value; }
        }

        /// <summary>
        /// 暂停公告
        /// </summary>
        public string pauseAnnouncement = string.Empty;
        public string PauseAnnouncement
        {
            get { return pauseAnnouncement; }
            set { pauseAnnouncement = value; }
        }

        /// <summary>
        /// 外链
        /// </summary>
        public string pauseLink = string.Empty;
        public string PauseLink
        {
            get { return pauseLink; }
            set { pauseLink = value; }
        }


        #region IEntity 成员

        object IEntity.EntityId { get { return typeof(SiteSettings).FullName; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }


}
