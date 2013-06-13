//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet;
using Tunynet.Caching;
using Spacebuilder.Common;
using Tunynet.Common;

namespace Spacebuilder.Group
{
    //设计要点：
    //1、缓存分区：GroupId、UserId；

    /// <summary>
    /// 群组成员实体
    /// </summary>
    [TableName("spb_GroupMembers")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId,GroupId")]
    [Serializable]
    public class GroupMember:IEntity
    {
        /// <summary>
        /// 群组成员实体
        /// </summary>
        public static GroupMember New()
        {
            GroupMember groupMember = new GroupMember()
            {
                JoinDate = DateTime.UtcNow
            };
            return groupMember;
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
        ///是否群管理员
        /// </summary>
        public bool IsManager { get; set; }

        /// <summary>
        ///加入日期
        /// </summary>
        public DateTime JoinDate { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

        /// <summary>
        /// 群组成员
        /// </summary>
        [Ignore]
        public User User
        {
            get
            {
                IUserService userService = DIContainer.Resolve<IUserService>();
                return userService.GetFullUser(this.UserId);
            }
        }
    }
}