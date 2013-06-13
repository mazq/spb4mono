//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using PetaPoco;
using Tunynet.Caching;

namespace Tunynet.Common
{
    /// <summary>
    /// 关注用户实体类
    /// </summary>
    [TableName("tn_Follows")]
    [PrimaryKey("Id", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "UserId")]
    [Serializable]
    public class FollowEntity : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static FollowEntity New()
        {
            FollowEntity followedUser = new FollowEntity()
            {
                NoteName = string.Empty,
                IsNewFollower = true,
                DateCreated = DateTime.UtcNow
            };
            return followedUser;
        }

        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        ///关注用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///被关注用户Id
        /// </summary>
        public long FollowedUserId { get; set; }

        /// <summary>
        ///备注名称
        /// </summary>
        public string NoteName { get; set; }

        /// <summary>
        ///是否为悄悄关注
        /// </summary>
        public bool IsQuietly { get; set; }

        /// <summary>
        ///是否为互相关注
        /// </summary>
        public bool IsMutual { get; set; }

        /// <summary>
        ///是否为新增粉丝
        /// </summary>
        public bool IsNewFollower { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime DateCreated { get; set; }
        
        #endregion 需持久化属性

        #region IEntity 成员

        object IEntity.EntityId { get { return this.Id; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}