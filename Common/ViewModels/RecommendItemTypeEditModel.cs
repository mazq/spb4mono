//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using System.Web.Mvc;

namespace Spacebuilder.Common
{
    public class RecommendItemTypeEditModel
    {
        /// <summary>
        /// 类别ID
        /// </summary>
         [Display(Name = "类别Id")]
        public string TypeId { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        [Display(Name = "类别名称")]
        [Required(ErrorMessage = "请输入类别名称")]
        [StringLength(64, ErrorMessage = "类别名称最多允许输入64个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 租户类别
        /// </summary>
        [Display(Name = "所属")]
        public string TenantTypeId { get; set; }

        /// <summary>
        /// 是否包含标题图
        /// </summary>
        [Display(Name = "标题图")]
        public bool HasFeaturedImage { get; set; }

        /// <summary>
        /// 类别描述
        /// </summary>
        [Display(Name = "类别描述")]
        [StringLength(512, ErrorMessage = "类别描述最多允许输入512个字符")]
        public string Description { get; set; }



        /// <summary>
        /// 转换为RecommendItemType用于数据库存储
        /// </summary>
        /// <returns></returns>
        public RecommendItemType AsRecommendItemType()
        {
            RecommendService recommendService = new RecommendService();
            RecommendItemType recommendType = recommendService.GetRecommendType(TypeId);           
            recommendType.Name = this.Name;
            recommendType.HasFeaturedImage = this.HasFeaturedImage;
            recommendType.Description = this.Description ?? string.Empty;
            return recommendType;
       }

    }

    /// <summary>
    /// 将推荐类别转换为EditModel
    /// </summary>
    /// <returns></returns>
    public static class RecommentItemTypeExtensions
    {
        /// <summary>
        /// 将推荐类别转换为EditModel
        /// </summary>
        /// <returns></returns>
        public static RecommendItemTypeEditModel AsEditModel(this RecommendItemType recommendItemType)
        {
            return new RecommendItemTypeEditModel
            {
                TypeId = recommendItemType.TypeId,
                Name = recommendItemType.Name,
                TenantTypeId = recommendItemType.TenantTypeId,
                HasFeaturedImage = recommendItemType.HasFeaturedImage,
                Description = recommendItemType.Description
            };
        }
    }
}
