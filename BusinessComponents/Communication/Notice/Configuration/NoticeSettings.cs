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
    /// 站点通知设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class NoticeSettings:IEntity
    {
        private List<NoticeTypeSettings> noticeTypeSettingses = null;
        /// <summary>
        /// 站点请求设置集合
        /// </summary>
        public List<NoticeTypeSettings> NoticeTypeSettingses
        {
            get
            {
                if (noticeTypeSettingses == null)
                {
                    noticeTypeSettingses = new List<NoticeTypeSettings>();
                    IEnumerable<NoticeType> noticeTypes = NoticeType.GetAll();
                    foreach (var noticeType in noticeTypes)
                    {
                        noticeTypeSettingses.Add(new NoticeTypeSettings { TypeId = noticeType.TypeId, IsAllow = true });
                    }
                }
                return noticeTypeSettingses;
            }
            set { noticeTypeSettingses = value; }
        }

        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(NoticeSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }        

        #endregion        
    }
}
