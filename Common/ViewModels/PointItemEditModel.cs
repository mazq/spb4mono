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

namespace Spacebuilder.Common
{
    /// <summary>
    /// 编辑积分项目
    /// </summary>
    public class PointItemEditModel
    {
        /// <summary>
        ///积分项目标识
        /// </summary>
        public string ItemKey { get; set; }

        /// <summary>
        ///应用程序Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        ///项目名称
        /// </summary>
        [Display(Name = "操作")]
        [Required(ErrorMessage = "项目名称为必填项")]
        public string ItemName { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///经验积分值
        /// </summary>
        [Display(Name = "经验")]
        [Required(ErrorMessage = "经验积分值为必填项")]
        public int ExperiencePoints { get; set; }

        /// <summary>
        ///威望积分值
        /// </summary>
        [Display(Name = "威望")]
        [Required(ErrorMessage = "威望积分值为必填项")]
        public int ReputationPoints { get; set; }

        /// <summary>
        ///交易积分值
        /// </summary>
        [Display(Name = "金币")]
        [Required(ErrorMessage = "交易积分值为必填项")]
        public int TradePoints { get; set; }

        /// <summary>
        ///交易积分值2
        /// </summary>
        public int TradePoints2 { get; set; }

        /// <summary>
        ///交易积分值3
        /// </summary>
        public int TradePoints3 { get; set; }

        /// <summary>
        ///交易积分值4
        /// </summary>
        public int TradePoints4 { get; set; }

        /// <summary>
        /// 转换为PointItem
        /// </summary>
        /// <returns>PointItem</returns>
        public PointItem AsPointItem()
        {
            PointItem pointItem = new PointService().GetPointItem(ItemKey);
            pointItem.ItemKey = this.ItemKey;
            pointItem.ItemName = this.ItemName;
            pointItem.TradePoints = this.TradePoints;
            pointItem.ReputationPoints = this.ReputationPoints;
            pointItem.ExperiencePoints = this.ExperiencePoints;
            return pointItem;
        }
    }

    /// <summary>
    /// 积分项目扩展类
    /// </summary>
    public static class PointItemExtensions
    {
        /// <summary>
        /// 积分项目转化为Model
        /// </summary>
        public static PointItemEditModel AsEditModel(this PointItem pointItem)
        {

            return new PointItemEditModel
            {
                ItemKey = pointItem.ItemKey,
                ItemName = pointItem.ItemName,
                TradePoints = pointItem.TradePoints,
                ReputationPoints = pointItem.ReputationPoints,
                ExperiencePoints = pointItem.ExperiencePoints
            };
        }
    }

}
