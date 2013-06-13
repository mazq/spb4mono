//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Spacebuilder.Common;
using System.Web.Mvc;
using Tunynet.Common;
using Tunynet.Utilities;
using System.ComponentModel;

namespace Spacebuilder.Bar
{
    /// <summary>
    /// 创建编辑帖吧的EditModel
    /// </summary>
    public class BarSectionEditModel
    {
        /// <summary>
        /// 帖吧Id
        /// </summary>
        public long SectionId { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [Display(Name = "排序字段")]
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// 帖吧名
        /// </summary>
        [Display(Name = "帖吧名", Description = "创建成功后将无法修改")]
        [Required(ErrorMessage = "请输入帖吧名称")]
        [StringLength(TextLengthSettings.TEXT_NAME_MAXLENGTH, ErrorMessage = "{0}最多允许输入{1}个字")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Display(Name = "类别", Description = "创建成功后将无法修改")]        
        [Required(ErrorMessage = "请选择帖吧所属的类别")]        
        public long CategoryId { get; set; }

        /// <summary>
        /// 吧主
        /// </summary>
        [Display(Name = "吧主")]
        public string UserId { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        [Display(Name = "管理员")]
        public string ManagerUserIds { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [AllowHtml]
        [DataType(DataType.Html)]
        [Required(ErrorMessage = "请填写帖吧描述")]
        [StringLength(600, ErrorMessage = "{0}最多允许输入{1}个字")]
        public string Description { get; set; }

        /// <summary>
        /// 帖子分类
        /// </summary>
        [Display(Name = "帖子分类")]
        public ThreadCategoryStatus ThreadCategoryStatus { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }

        
        

        public string LogoImage { get; set; }

        /// <summary>
        /// 是否拥有logo
        /// </summary>
        /// <returns>是否拥有图表</returns>
        public bool HasLogoImage()
        {
            BarSection section = new BarSectionService().Get(this.SectionId);
            if (section != null)
                return section.HasLogoImage;
            return false;
        }

        /// <summary>
        /// 将ViewModel转换成数据库存储实体
        /// </summary>
        /// <returns></returns>
        public BarSection AsBarSection()
        {
            BarSectionService barSectionService = new BarSectionService();

            BarSection barSection = null;
            if (this.SectionId > 0)
                barSection = barSectionService.Get(this.SectionId);
            else
            {
                barSection = BarSection.New();
                barSection.TenantTypeId = TenantTypeIds.Instance().Bar();
            }
            barSection.Name = this.Name;
            barSection.Description = this.Description;
            barSection.IsEnabled = this.IsEnabled;
            barSection.ThreadCategoryStatus = this.ThreadCategoryStatus;
            return barSection;
        }

        /// <summary>
        /// 获取前台编辑的实体
        /// </summary>
        /// <returns>获取前台编辑的实体</returns>
        public BarSection GetBarSectionByEditForManager()
        {
            BarSection section = new BarSectionService().Get(this.SectionId);
            if (section == null)
                return null;
            section.Description = this.Description;
            section.ThreadCategoryStatus = this.ThreadCategoryStatus;
            return section;
        }
    }

    /// <summary>
    /// 帖吧的扩展类
    /// </summary>
    public static class BarSectionExtensions
    {
        /// <summary>
        /// 将数据库中的信息转换成EditModel实体
        /// </summary>
        /// <param name="barSection"></param>
        /// <returns></returns>
        public static BarSectionEditModel AsEditModel(this BarSection barSection)
        {
            return new BarSectionEditModel
            {
                Name = barSection.Name,
                Description = barSection.Description,
                ThreadCategoryStatus = barSection.ThreadCategoryStatus,
                IsEnabled = barSection.IsEnabled,
                SectionId = barSection.SectionId,
                LogoImage = barSection.LogoImage,
                DisplayOrder = barSection.DisplayOrder
            };
        }
    }
}