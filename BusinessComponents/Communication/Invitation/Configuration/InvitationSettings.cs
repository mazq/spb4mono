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
    /// 站点请求设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class InvitationSettings : IEntity
    {
        private List<InvitationTypeSettings> invitationTypeSettingses = null;
        /// <summary>
        /// 站点请求设置集合
        /// </summary>
        public List<InvitationTypeSettings> InvitationTypeSettingses
        {
            get
            {
                if (invitationTypeSettingses == null)
                {
                    invitationTypeSettingses = new List<InvitationTypeSettings>();
                    IEnumerable<InvitationType> invitationTypes = InvitationType.GetAll();
                    foreach (var invitationType in invitationTypes)
                    {
                        invitationTypeSettingses.Add(new InvitationTypeSettings { TypeKey = invitationType.Key, IsAllow = true });
                    }
                }
                return invitationTypeSettingses;
            }
            set { invitationTypeSettingses = value; }
        }


        #region IEnity 成员

        object IEntity.EntityId
        {
            get { return typeof(InvitationSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}