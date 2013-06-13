//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;

namespace Spacebuilder.Group
{
    //设计要点：
    //1、缓存分区：GroupId；
    
    
    /// <summary>
    /// 群组成员申请实体
    /// </summary>
    [TableName("spb_GroupMemberApplies")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId,GroupId")]
    [Serializable]
    public class GroupMemberApply : IEntity
    {
        /// <summary>
        /// 群组成员申请实体
        /// </summary>
        public static GroupMemberApply New()
        {
            GroupMemberApply groupMemberApplie = new GroupMemberApply()
            {
                ApplyReason = string.Empty,
                ApplyDate = DateTime.UtcNow
            };
            return groupMemberApplie;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///群组Id
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        ///用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///申请理由
        /// </summary>
        public string ApplyReason { get; set; }

        /// <summary>
        ///申请状态
        /// </summary>
        public GroupMemberApplyStatus ApplyStatus { get; set; }

        /// <summary>
        ///申请日期
        /// </summary>
        public DateTime ApplyDate { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}