//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tunynet.Common;
using System.ComponentModel.DataAnnotations;
using Tunynet.Mvc;
using Spacebuilder.Common;
using System.Web.Mvc;


namespace Spacebuilder.Group
{
    /// <summary>
    /// 编辑群组实体
    /// </summary>
    public class GroupEditModel
    {
        /// <summary>
        ///GroupId
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        ///群组名称
        /// </summary>
        [Display(Name = "名称")]
        [WaterMark(Content = "在此输入群组名称")]
        [Required(ErrorMessage = "请输入群组名称")]
        [StringLength(60, ErrorMessage = "最多允许输入60个字")]
        public string GroupName { get; set; }

        /// <summary>
        ///群组标识（个性网址的关键组成部分）
        /// </summary>
        [Required(ErrorMessage = "请输入群组标识")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "请输入4-16个字")]
        [DataType(DataType.Url)]
        [Remote("ValidateGroupKey", "ChannelGroup", "Group", ErrorMessage = "此群组Key已存在", AdditionalFields = "GroupId")]
        public string GroupKey { get; set; }

        /// <summary>
        ///群组介绍
        /// </summary>
        [Display(Name = "简介")]
        [StringLength(300, ErrorMessage = "最多可以输入300个字")]
        public string Description { get; set; }

        /// <summary>
        ///所在地区
        /// </summary>        
        public string AreaCode { get; set; }

        /// <summary>
        ///logo名称（带部分路径）
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        ///是否公开
        /// </summary>
        [Required(ErrorMessage = "请选择群组类型")]
        public bool IsPublic { get; set; }

        /// <summary>
        ///加入方式
        /// </summary>
        [Required(ErrorMessage = "请选择加入方式")]
        public JoinWay JoinWay { get; set; }

        /// <summary>
        ///是否允许成员邀请（一直允许群管理员邀请）
        /// </summary>
        public bool EnableMemberInvite { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        [WaterMark(Content = "如1+1=？")]
        [StringLength(36, ErrorMessage = "最多输入36个汉字")]
        public string Question { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        [StringLength(36, ErrorMessage = "最多输入36个汉字")]
        public string Answer { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        [Required(ErrorMessage = "请选择类别")]
        public long CategoryId { get; set; }

        /// <summary>
        /// 相关用户Id集合
        /// </summary>
        public string RelatedUserIds { get; set; }

        /// <summary>
        /// 相关标签集合
        /// </summary>
        public string[] RelatedTags { get; set; }

        /// <summary>
        /// 转换成groupEntity类型
        /// </summary>
        /// <returns></returns>
        public GroupEntity AsGroupEntity()
        {
            CategoryService categoryService = new CategoryService();
            GroupEntity groupEntity = null;

            //创建群组
            if (this.GroupId == 0)
            {
                groupEntity = GroupEntity.New();
                groupEntity.UserId = UserContext.CurrentUser.UserId;
                groupEntity.DateCreated = DateTime.UtcNow;
                groupEntity.GroupKey = this.GroupKey;
            }
            //编辑群组
            else
            {
                GroupService groupService = new GroupService();
                groupEntity = groupService.Get(this.GroupId);
            }
            groupEntity.IsPublic = this.IsPublic;
            groupEntity.GroupName = this.GroupName;
            if (Logo != null)
            {
                groupEntity.Logo = this.Logo;
            }
            groupEntity.Description = Formatter.FormatMultiLinePlainTextForStorage(this.Description == null ? string.Empty : this.Description, true);
            groupEntity.AreaCode = this.AreaCode??string.Empty;
            groupEntity.JoinWay = this.JoinWay;
            groupEntity.EnableMemberInvite = this.EnableMemberInvite;
            if (JoinWay == JoinWay.ByQuestion)
            {
                groupEntity.Question = this.Question;
                groupEntity.Answer = this.Answer;
            }
            return groupEntity;
        }
    }

    /// <summary>
    /// 群组实体的扩展类
    /// </summary>
    public static class GroupEntityExtensions
    {
        /// <summary>
        /// 将数据库中的信息转换成EditModel实体
        /// </summary>
        /// <param name="groupEntity"></param>
        /// <returns></returns>
        public static GroupEditModel AsEditModel(this GroupEntity groupEntity)
        {
            return new GroupEditModel
            {
                GroupId = groupEntity.GroupId,
                IsPublic = groupEntity.IsPublic,
                GroupName = groupEntity.GroupName,
                GroupKey = groupEntity.GroupKey,
                Logo = groupEntity.Logo,
                Description = Formatter.FormatMultiLinePlainTextForEdit(groupEntity.Description, true),
                AreaCode = groupEntity.AreaCode,
                JoinWay = groupEntity.JoinWay,
                EnableMemberInvite = groupEntity.EnableMemberInvite,
                CategoryId = groupEntity.Category == null ? 0 : groupEntity.Category.CategoryId,
                Question = groupEntity.Question,
                Answer = groupEntity.Answer
            };
        }
    }
}
