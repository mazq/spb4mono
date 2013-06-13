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
    /// 邀请好友设置
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class InviteFriendSettings:IEntity
    {
        /// <summary>
        /// 邀请码是否仅允许使用一次
        /// </summary>
        private bool allowInvitationCodeUseOnce = false;

        /// <summary>
        /// 邀请码是否仅允许使用一次(仅在注册选项为邀请注册的时候才允许管理员修改，其他时候。都修改false)
        /// </summary>
        public bool AllowInvitationCodeUseOnce
        {
            get { return allowInvitationCodeUseOnce; }
            set { allowInvitationCodeUseOnce = value; }
        }


        /// <summary>
        /// 邀请码有效期（单位：天）
        /// </summary>
        private int invitationCodeTimeLiness = 7;

        /// <summary>
        /// 邀请码有效期（单位：天）
        /// </summary>
        public int InvitationCodeTimeLiness
        {
            get { return invitationCodeTimeLiness; }
            set { invitationCodeTimeLiness = value; }
        }

        /// <summary>
        /// 设置购买邀请码所需的交易积分
        /// </summary>
        private int invitationCodeUnitPrice = 1000;

        /// <summary>
        /// 设置购买邀请码所需的交易积分
        /// </summary>
        public int InvitationCodeUnitPrice
        {
            get { return invitationCodeUnitPrice; }
            set { invitationCodeUnitPrice = value; }
        }

        /// <summary>
        /// 默认用户邀请码配额
        /// </summary>
        private int defaultUserInvitationCodeCount = 0;

        /// <summary>
        /// 默认用户邀请码配额
        /// </summary>
        public int DefaultUserInvitationCodeCount
        {
            get { return defaultUserInvitationCodeCount; }
            set { defaultUserInvitationCodeCount = value; }
        }


        #region IEntity 成员

        object IEntity.EntityId
        {
            get { return typeof(InviteFriendSettings).FullName; }
        }

        bool IEntity.IsDeletedInDatabase { get; set; }
        
        #endregion        
    }
}