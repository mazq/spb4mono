//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 邀请好友的记录实体
    /// </summary>
    [TableName("tn_InviteFriendRecords")]
    [PrimaryKey("Id", autoIncrement = true)]
    
    [CacheSetting(true,PropertyNamesOfArea="UserId")]
    [Serializable]
    public class InviteFriendRecord:IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        
        public static InviteFriendRecord New()
        {
            InviteFriendRecord inviteFriendRecord = new InviteFriendRecord()
            {
                Code = string.Empty,
                DateCreated = DateTime.UtcNow

            };
            return inviteFriendRecord;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///邀请人
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///受邀人
        /// </summary>
        public long InvitedUserId { get; set; }

        /// <summary>
        ///邀请码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 邀请用户是否得到了奖励
        /// </summary>
        public bool InvitingUserHasBeingRewarded { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
