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
    /// 标签实体类
    /// </summary>
    [TableName("tn_Tags")]
    [PrimaryKey("TagId", autoIncrement = true)]
    [CacheSetting(true, PropertyNamesOfArea = "TenantTypeId")]
    [Serializable]
    public class Tag : SerializablePropertiesBase, IEntity
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static Tag New()
        {
            Tag tag = new Tag()
            {
                TagName = string.Empty,
                Description = string.Empty,
                FeaturedImage = string.Empty,
                TenantTypeId = string.Empty,
                OwnerCount = 1,
                ItemCount = 1,
                AuditStatus = AuditStatus.Success,
                DateCreated = DateTime.UtcNow
            };
            return tag;
        }

        #region 需持久化属性

        /// <summary>
        ///标签Id
        /// </summary>
        public long TagId { get; protected set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        public string TenantTypeId { get; set; }

        /// <summary>
        ///标签名称
        /// </summary>
        public string TagName { get; set; }

        private string displayName;
        /// <summary>
        /// 标签显示名
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(displayName))
                    return TagName;

                return displayName;
            }
            set
            {
                displayName = value;
            }
        }

        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///标签标题图
        /// </summary>
        public string FeaturedImage { get; set; }

        /// <summary>
        ///是否为特色标签
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// 内容项数目
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// 标签使用数
        /// </summary>
        public int OwnerCount { get; set; }

        /// <summary>
        ///审核状态
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime DateCreated { get; protected set; }

        #endregion 需持久化属性

        /// <summary>
        /// 相关对象Id
        /// </summary>
        [Ignore]
        public string RelatedObjectIds
        {
            get { return GetExtendedProperty<string>("RelatedObjectIds"); }
            set { SetExtendedProperty("RelatedObjectIds", value); }
        }

        /// <summary>
        /// 拥有者Id
        /// </summary>
        [Ignore]
        public long OwnerId { get; set; }

        /// <summary>
        /// 24小时内的讨论次数
        /// </summary>
        [Ignore]
        public int PreDayItemCount
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().Tag());
                return countService.GetStageCount(CountTypes.Instance().ItemCounts(), 1, this.TagId);
            }
        }

        /// <summary>
        /// 最近7天的讨论次数
        /// </summary>
        [Ignore]
        public int PreWeekItemCount
        {
            get
            {
                CountService countService = new CountService(TenantTypeIds.Instance().Tag());
                return countService.GetStageCount(CountTypes.Instance().ItemCounts(), 7, this.TagId);
            }
        }
        /// <summary>
        /// 分组名
        /// </summary>
        [Ignore]
        public string GroupName
        {
            get
            {
                TagService tagService = new TagService(TenantTypeIds.Instance().Tag());
                IEnumerable<TagGroup> tagGroups = tagService.GetGroupsOfTag(this.TagName, this.TenantTypeId);
                if (tagGroups.Count() > 0)
                {
                    return tagGroups.First().GroupName;
                }
                else
                {
                    return null;
                }
            }
        }
        #region IEntity 成员

        object IEntity.EntityId { get { return this.TagId; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion IEntity 成员
    }
}