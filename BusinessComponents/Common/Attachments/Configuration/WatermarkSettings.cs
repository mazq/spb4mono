//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using Tunynet.Imaging;
using Tunynet.Utilities;
using System;
using Tunynet.Caching;
using System.ComponentModel.DataAnnotations;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 水印设置
    /// </summary>
    [Serializable]
    public class WatermarkSettings
    {
        private WatermarkType watermarkType = WatermarkType.Text;
        /// <summary>
        /// 水印类型
        /// </summary>
        public WatermarkType WatermarkType
        {
            get { return watermarkType; }
            set { watermarkType = value; }
        }

        private AnchorLocation watermarkLocation = AnchorLocation.RightBottom;
        /// <summary>
        /// 水印位置
        /// </summary>
        public AnchorLocation WatermarkLocation
        {
            get { return watermarkLocation; }
            set { watermarkLocation = value; }
        }

        private string watermarkText = "tunynet";
        /// <summary>
        /// 水印文字
        /// </summary>
        public string WatermarkText
        {
            get { return watermarkText; }
            set { watermarkText = value; }
        }

        private string watermarkImageName = "watermark.png";
        /// <summary>
        /// 水印图片名称
        /// </summary>
        public string WatermarkImageName
        {
            get { return watermarkImageName; }
            set { watermarkImageName = value; }
        }

        private int watermarkMinWidth = 300;
        /// <summary>
        /// 添加水印的图片最小宽度(px)
        /// </summary>
        public int WatermarkMinWidth
        {
            get { return watermarkMinWidth; }
            set { watermarkMinWidth = value; }
        }

        private int watermarkMinHeight = 300;
        /// <summary>
        /// 添加水印的图片最小高度(px)
        /// </summary>
        public int WatermarkMinHeight
        {
            get { return watermarkMinHeight; }
            set { watermarkMinHeight = value; }
        }

        private float watermarkOpacity = 0.6F;
        /// <summary>
        /// 水印不透明度
        /// </summary>
        /// <remarks>
        /// 取值范围 0.1~1.0
        /// </remarks>
        public float WatermarkOpacity
        {
            get { return watermarkOpacity; }
            set { watermarkOpacity = value; }
        }


        private string watermarkImageDirectory = "~/Images/";
        /// <summary>
        /// 水印图片所在目录
        /// </summary>
        public string WatermarkImageDirectory
        {
            get { return watermarkImageDirectory; }
        }

        private string watermarkImagePhysicalPath = null;
        /// <summary>
        /// 水印图片所在完整物理路径
        /// </summary>
        public string WatermarkImagePhysicalPath
        {
            get
            {
                if (string.IsNullOrEmpty(watermarkImagePhysicalPath))
                    watermarkImagePhysicalPath = WebUtility.GetPhysicalFilePath(System.IO.Path.Combine(this.WatermarkImageDirectory, this.WatermarkImageName));

                return watermarkImagePhysicalPath;
            }
        }
    }

    /// <summary>
    /// 水印类型
    /// </summary>
    public enum WatermarkType
    {
        /// <summary>
        /// 禁用水印
        /// </summary>
        [Display(Name="禁用")]
        None,

        /// <summary>
        /// 文字水印
        /// </summary>
        [Display(Name="文字")]
        Text,

        /// <summary>
        /// 图像水印
        /// </summary>
        [Display(Name = "图像")]
        Image
    }
}
