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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 标签分组编辑实体
    /// </summary>
    public class TagGroupEditModel
    {
        // 摘要:
        //     分组Id
        public long GroupId { get; set; }
        //
        // 摘要:
        //     标签分组名
        [Display(Name = "分组名称")]
        [Required(ErrorMessage = "请输入分组名称")]
        [StringLength(32, ErrorMessage = "最大长度允许32个字符")]
        [WaterMark(Content = "请输入分组名称")]
        [DataType(DataType.Text)]
        public string GroupName { get; set; }

        //
        // 摘要:
        //     租户类型Id
        public string TenantTypeId { get; set; }


        /// <summary>
        /// 转换为TagGroup用于数据库存储
        /// </summary>
        public TagGroup AsTagGroup()
        {
            TagGroup tagGroup = null;

            if (GroupId == 0)
            {
                tagGroup = TagGroup.New();
            }
            else
            {
                TagService tagService = new TagService(TenantTypeIds.Instance().Tag());
                tagGroup = tagService.GetGroup(GroupId);
            }
            tagGroup.GroupName = GroupName;
            tagGroup.TenantTypeId= TenantTypeId;

            return tagGroup;
        }
    }
    /// <summary>
    /// 标签的扩展方法
    /// </summary>
    public static class TagGroupEditModelExtensions
    {
        /// <summary>
        /// 数据库中的对象转换为EditModel
        /// </summary>
        /// <returns></returns>
        public static TagGroupEditModel AsTagGroupEditModel(this TagGroup tagGroup)
        {
            return new TagGroupEditModel
            {
                 GroupId=tagGroup.GroupId,
                 GroupName=tagGroup.GroupName,
                 TenantTypeId=tagGroup.TenantTypeId
            };
        }
    }
}
