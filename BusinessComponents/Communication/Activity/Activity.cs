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
    /// 动态的实体类
    /// </summary>
    [TableName("tn_Activities")]
    [PrimaryKey("ActivityId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "OwnerId")]
    [Serializable]
    public class Activity : IEntity
    {

        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Activity New()
        {
            Activity activity = new Activity()
            {
                OwnerName = string.Empty,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                TenantTypeId = string.Empty,
                ReferenceTenantTypeId = string.Empty
            };
            return activity;
        }

        #region 需持久化属性

        /// <summary>
        ///ActivityId
        /// </summary>
        public long ActivityId { get; protected set; }

        /// <summary>
        ///拥有者Id
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        ///动态拥有者类型
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        ///拥有者名称
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        ///动态项目标识
        /// </summary>
        public string ActivityItemKey { get; set; }

        /// <summary>
        ///应用Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///操作者Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///动态源内容id（例如：日志动态的日志Id）
        /// </summary>
        public long SourceId { get; set; }

        /// <summary>
        ///涉及的Id（例如：评论动态的评论对象Id）
        /// </summary>
        public long ReferenceId { get; set; }

        /// <summary>
        ///涉及对象的租户类型Id
        /// </summary>
        public string ReferenceTenantTypeId { get; set; }

        /// <summary>
        ///是否私有（仅允许自己查看）
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        ///是否原创主题
        /// </summary>
        public bool IsOriginalThread { get; set; }

        /// <summary>
        ///是否包含视频
        /// </summary>
        public bool HasVideo { get; set; }

        /// <summary>
        ///是否包含音乐
        /// </summary>
        public bool HasMusic { get; set; }

        /// <summary>
        ///是否包含图片
        /// </summary>
        public bool HasImage { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///最后更新时间
        /// </summary>
        public DateTime LastModified { get; set; }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return this.ActivityId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion
    }
}
