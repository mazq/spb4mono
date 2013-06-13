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
    /// <summary>
    /// 广告位编辑实体
    /// </summary>
    public class AdvertisingPositionEditModel
    {
        #region 需持久化属性

        /// <summary>
        /// 广告位Id（编辑时用）
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///广告位Id（创建时用）
        /// </summary>
        [Required(ErrorMessage = "请输入广告位编码")]
        [Remote("ValidatePositionId", "ControlPanelOperation", "Common", ErrorMessage = "此广告位编码已存在")]
        [StringLength(25, ErrorMessage = "最多可输入25个数字")]
        [RegularExpression("\\d+", ErrorMessage = "只能输入数字")]
        public string PositionId { get; set; }

        /// <summary>
        ///投放区域
        /// </summary>
        [Required(ErrorMessage = "请选择投放区域")]
        public string PresentAreaKey { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        [Required(ErrorMessage="请输入描述")]
        [StringLength(TextLengthSettings.TEXT_DESCRIPTION_MAXLENGTH, ErrorMessage = "最多可输入200字")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        /// <summary>
        ///示意图
        /// </summary>
        public string FeaturedImage { get; set; }

        /// <summary>
        ///宽度
        /// </summary>
        [Required(ErrorMessage = "请输入建议广告宽度")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入大于0的数字")]
        public int Width { get; set; }

        /// <summary>
        ///高度
        /// </summary>
        [Required(ErrorMessage = "请输入建议广告高度")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入大于0的数字")]
        public int Height { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        #endregion

        /// <summary>
        /// 将EditModel转换为广告位实体
        /// </summary>
        /// <returns></returns>
        public AdvertisingPosition AsAdvertisingPosition()
        {
            AdvertisingService advertisingService = new AdvertisingService();
            AdvertisingPosition position = advertisingService.GetPosition(PositionId);

            if (position == null)
            {
                position = AdvertisingPosition.New();
                position.PositionId = PositionId;
            }
            position.Description = Description ?? string.Empty;
            position.FeaturedImage = FeaturedImage ?? string.Empty;
            position.PresentAreaKey = PresentAreaKey;
            position.Height = Height;
            position.Width = Width;
            position.IsEnable = IsEnable;
            return position;
        }
    }

    /// <summary>
    /// 广告位的扩展类
    /// </summary>
    public static class AdvertisingPositionEditModelExtensions
    {
        /// <summary>
        /// 将广告位实体转换为EditModel
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static AdvertisingPositionEditModel AsAdvertisingPositionEditModel(this AdvertisingPosition position)
        {
            return new AdvertisingPositionEditModel
            {
                PositionId = position.PositionId,
                Description = position.Description,
                FeaturedImage = position.FeaturedImage,
                PresentAreaKey = position.PresentAreaKey,
                Height = position.Height,
                Width = position.Width,
                IsEnable = position.IsEnable,
                Id = position.PositionId
            };
        }
    }
}
