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
using Tunynet.Utilities;
using Tunynet.Mvc;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 推荐内容编辑实体
    /// </summary>
    public class RecommendItemEditModel
    {
        #region 需持久化属性

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 作者ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        ///租户类型Id
        /// </summary>
        [Display(Name = "租户类型")]
        [Required(ErrorMessage = "请选择租户类型")]
        public string TenantTypeId { get; set; }

        /// <summary>
        /// IsLink
        /// </summary>
        //[StringLength(255, ErrorMessage = "最大允许255个字符")]
        public bool IsLink { get; set; }

        /// <summary>
        /// 外链链接地址
        /// </summary>
        [Required(ErrorMessage = "请输入链接地址")]
        [RegularExpression(@"(https?):\/\/([A-z0-9]+[_\-]?\.)*[A-z0-9]+\-?[A-z0-9]+\.[A-z]{2,}(\/.*)*\/?",ErrorMessage="请输入合法的链接地址")]
        [WaterMark(Content = "http://")]
        public string LinkAddress { get; set; }

        /// <summary>
        ///推荐类型Id
        /// </summary>
        [Display(Name = "推荐类型")]
        [Required(ErrorMessage = "请选择推荐类型")]
        public string TypeId { get; set; }

        /// <summary>
        ///内容实体Id
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        ///推荐标题（默认为内容名称或标题，允许推荐人修改）
        /// </summary>
        [Display(Name = "标题")]
        [Required(ErrorMessage = "请输入标题")]
        [WaterMark(Content = "请输入标题")]
        [StringLength(100, ErrorMessage = "最大允许100个字符")]
        [DataType(DataType.Text)]
        public string RecommendItemName { get; set; }

        /// <summary>
        ///推荐标题图(存储图片文件名或完整图片链接地址)
        /// </summary>
        /// 
        [Display(Name = "标题图")]
        [StringLength(256, ErrorMessage = "最大允许256个字符")]
        public string FeaturedImage { get; set; }

        /// <summary>
        ///推荐期限
        /// </summary>
        [Display(Name = "截止日期")]
        public DateTime? ExpiredDate { get; set; }

        #endregion

        /// <summary>
        /// 转换为RecommendItem用于数据库存储
        /// </summary>
        public RecommendItem AsRecommendItem()
        {
            RecommendItem item = null;
            //创建
            if (Id == 0)
            {
                item = RecommendItem.New();
                item.DateCreated = DateTime.UtcNow;
            }//编辑
            else
            {
                RecommendService recommendService = new RecommendService();
                item = recommendService.Get(Id);
                //处理删除后再点确定按钮报错情况
                if (item == null)
                {
                    Id = 0;
                    item = RecommendItem.New();
                    item.DateCreated = DateTime.UtcNow;
                }
            }
            if (ExpiredDate == null)
            {
                item.ExpiredDate = DateTime.UtcNow.AddYears(100);
            }
            else
            {
                item.ExpiredDate = ExpiredDate.Value;
            }
            if (!string.IsNullOrEmpty(FeaturedImage))
            {
                item.FeaturedImage = FeaturedImage;
            }
            item.UserId = UserId;
            item.IsLink = IsLink;
            item.ItemName = RecommendItemName;
            item.ReferrerId = UserContext.CurrentUser.UserId;
            item.ReferrerName = UserContext.CurrentUser.DisplayName;
            item.ItemId = ItemId;
            item.TenantTypeId = TenantTypeId;
            item.TypeId = TypeId;
            item.LinkAddress = LinkAddress ?? string.Empty;
            return item;
        }

    }

    /// <summary>
    /// RecommendItem扩展
    /// </summary>
    public static class RecommendItemExtensions
    {
        /// <summary>
        /// 转换成RecommendItemEditModel
        /// </summary>
        public static RecommendItemEditModel AsEditModel(this RecommendItem item)
        {
            return new RecommendItemEditModel
            {
                UserId = item.UserId,
                Id = item.Id,
                TenantTypeId = item.TenantTypeId,
                TypeId = item.TypeId,
                ItemId = item.ItemId,
                RecommendItemName = WebUtility.UrlDecode(item.ItemName),
                FeaturedImage = item.FeaturedImage,
                ExpiredDate = item.ExpiredDate,
                IsLink = item.IsLink,
                LinkAddress = string.IsNullOrEmpty(item.LinkAddress)?"http://":item.LinkAddress
            };
        }
    }
}
