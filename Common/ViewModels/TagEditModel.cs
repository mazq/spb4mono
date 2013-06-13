//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using System.ComponentModel.DataAnnotations;
using Tunynet.Mvc;
using System.Web.Mvc;
using Tunynet.Utilities;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 标签编辑实体
    /// </summary>
    public class TagEditModel
    {
        //
        // 摘要:
        //     标签Id
        public long TagId { get; set; }

        //
        // 摘要:
        //     标签名称
        [Display(Name = "标签名称")]
        [Required(ErrorMessage = "请输入内容")]
        [StringLength(64, ErrorMessage = "最大长度允许64个字符")]
        [WaterMark(Content = "请输入标签名称")]
        [DataType(DataType.Text)]
        [Remote("ValidateTagName", "Channel", "Common",ErrorMessage = "该内容已存在", AdditionalFields = "TagId")]
        public string TagName { get; set; }

        //
        // 摘要:
        //     标签显示名
        [Display(Name = "标签显示名")]
        [StringLength(64, ErrorMessage = "最大长度允许64个字符")]
        [DataType(DataType.Text)]
        public string DisplayName { get; set; }

        //
        // 摘要:
        //     租户类型Id
        public string TenantTypeId { get; set; }

        //
        // 摘要:
        //     标签分组Id
        private long groupId;
        [Display(Name = "标签分组Id")]
        public long GroupId
        {
            get
            {
                if (groupId == 0 && !string.IsNullOrEmpty(TagName) && !string.IsNullOrEmpty(TenantTypeId))
                {
                    TagService tagService = new TagService(string.Empty);
                    IEnumerable<TagGroup> GroupsOfTag = tagService.GetGroupsOfTag(TagName, TenantTypeId);
                    //如果该标签设置了分组
                    if (GroupsOfTag.Count() != 0)
                    {
                        groupId = GroupsOfTag.First().GroupId;
                    }
                }
                return groupId;
            }
            set { groupId = value; }
        }

        //
        // 摘要:
        //     标签分组Id
        private long topicGroupId;

        [Display(Name = "话题分组Id")]
        public long TopicGroupId
        {
            get
            {
                if (topicGroupId == 0 && !string.IsNullOrEmpty(TagName) && !string.IsNullOrEmpty(TenantTypeId))
                {
                    TagService tagService = new TagService(TenantTypeId);
                    IEnumerable<TagGroup> GroupsOfTag = tagService.GetGroupsOfTag(TagName, TenantTypeId);
                    //如果该标签设置了分组
                    if (GroupsOfTag.Count() != 0)
                    {
                        topicGroupId = GroupsOfTag.Last().GroupId;
                    }
                }
                return topicGroupId;
            }
            set { topicGroupId = value; }
        }

        //
        // 摘要:
        //     是否为特色标签
        private bool isFeatured;
        public bool IsFeatured { get { return isFeatured; } set { isFeatured = value; } }

        private string description;
        // 摘要:
        //     描述
        [AllowHtml]
        [StringLength(250, ErrorMessage = "最大长度允许250个字符")]
        [DataType(DataType.Html)]
        public string Description { get { return description == null ? "" : description; } set { description = value; } }


        // 摘要：
        //     图片名称
        private string featuredImage;
        public string FeaturedImage { get { return featuredImage == null ? "" : featuredImage; } set { featuredImage = value; } }

        // 摘要：
        //     相关标签
        public string[] RelatedTags { get; set; }

        //
        // 摘要:
        //     相关对象Id
        public string RelatedObjectIds { get; set; }

        /// <summary>
        /// 转换为Tag用于数据库存储
        /// </summary>
        public Tag AsTag()
        {
            Tag tag = null;

            if (TagId == 0)
            {
                tag = Tag.New();
            }
            else
            {
                TagService tagService = new TagService(TenantTypeIds.Instance().Tag());
                tag = tagService.Get(TagId);
            }
            tag.TagName = TagName;
            tag.DisplayName = DisplayName;
            tag.TenantTypeId = TenantTypeId;
            tag.IsFeatured = IsFeatured;
            tag.Description = Description;
            tag.FeaturedImage = FeaturedImage;
            tag.RelatedObjectIds = RelatedObjectIds == null ? "" : RelatedObjectIds;

            return tag;
        }
    }

    /// <summary>
    /// 标签的扩展方法
    /// </summary>
    public static class TagEditModelExtensions
    {
        /// <summary>
        /// 数据库中的对象转换为EditModel
        /// </summary>
        /// <returns></returns>
        public static TagEditModel AsTagEditModel(this Tag tag)
        {
            return new TagEditModel
            {
                TagId = tag.TagId,
                TagName = tag.TagName,
                IsFeatured = tag.IsFeatured,
                DisplayName = tag.DisplayName,
                TenantTypeId = tag.TenantTypeId,
                Description = tag.Description,
                FeaturedImage = tag.FeaturedImage,
                RelatedObjectIds = tag.RelatedObjectIds
            };
        }
    }
}
