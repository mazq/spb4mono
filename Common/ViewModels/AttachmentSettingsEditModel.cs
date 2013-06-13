//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using Tunynet.Common;
using Tunynet;
using Tunynet.Utilities;
using System.Web.Mvc;
using Spacebuilder.Common.Configuration;
using Tunynet.Common.Configuration;
using Tunynet.Imaging;

namespace Spacebuilder.Common
{
    /// <summary>
    /// 附件设置
    /// </summary>
    public class AttachmentSettingsEditModel
    {
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public AttachmentSettingsEditModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userProfileSettings">用户资料设置</param>
        /// <param name="userSettings">用户设置</param>
        /// <param name="inviteFriendSettings">邀请朋友设置</param>
        public AttachmentSettingsEditModel(AttachmentSettings attachmentSettings)
        {
            if (attachmentSettings != null)
            {
                AllowedFileExtensions = attachmentSettings.AllowedFileExtensions;
                BatchUploadLimit = attachmentSettings.BatchUploadLimit;
                InlinedImageHeight = attachmentSettings.InlinedImageHeight;
                InlinedImageWidth = attachmentSettings.InlinedImageWidth;
                MaxAttachmentLength = attachmentSettings.MaxAttachmentLength;
                MaxImageHeight = attachmentSettings.MaxImageHeight;
                MaxImageWidth = attachmentSettings.MaxImageWidth;
                TemporaryAttachmentStorageDay = attachmentSettings.TemporaryAttachmentStorageDay;

                WatermarkSettings watermarkSettings = attachmentSettings.WatermarkSettings;
                if (watermarkSettings != null)
                {
                    WatermarkImageName = watermarkSettings.WatermarkImageName;
                    WatermarkLocation = watermarkSettings.WatermarkLocation;
                    WatermarkMinHeight = watermarkSettings.WatermarkMinHeight;
                    WatermarkMinWidth = watermarkSettings.WatermarkMinWidth;
                    WatermarkOpacity = watermarkSettings.WatermarkOpacity;
                    WatermarkText = watermarkSettings.WatermarkText;
                    WatermarkType = watermarkSettings.WatermarkType;
                }
            }
        }

        #region 需持久化属性

        #region 附件设置

        /// <summary>
        /// 附件允许的文件扩展名
        /// </summary>     
        public string AllowedFileExtensions { get; set; }

        /// <summary>
        /// 批量上传数目限制 
        /// </summary>
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int BatchUploadLimit { get; set; }

        /// <summary>
        /// 页面呈现图片的最大高度
        /// </summary>     
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int InlinedImageHeight { get; set; }

        /// <summary>
        /// 页面呈现图片的最大宽度
        /// </summary>  
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int InlinedImageWidth { get; set; }

        /// <summary>
        /// 附件最大长度
        /// </summary> 
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int MaxAttachmentLength { get; set; }

        /// <summary>
        /// 图片最大高度
        /// </summary>    
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int MaxImageHeight { get; set; }

        /// <summary>
        /// 图片最大宽度
        /// </summary>  
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int MaxImageWidth { get; set; }

        /// <summary>
        /// 临时附件保留的天数
        /// </summary>     
        [RegularExpression("\\d+", ErrorMessage = "请输入数字")]
        public int TemporaryAttachmentStorageDay { get; set; }

        #endregion

        #region 水印设置

        /// <summary>
        /// 水印图片名称
        /// </summary>
        public string WatermarkImageName { get; set; }

        /// <summary>
        /// 水印位置
        /// </summary>
        public AnchorLocation WatermarkLocation { get; set; }

        /// <summary>
        /// 添加水印的图片最小高度(px)
        /// </summary>
        public int WatermarkMinHeight { get; set; }

        /// <summary>
        /// 添加水印的图片最小宽度(px)
        /// </summary>
        public int WatermarkMinWidth { get; set; }

        /// <summary>
        /// 水印不透明度(取值范围 0.1~1.0)
        /// </summary>
        public float WatermarkOpacity { get; set; }

        /// <summary>
        /// 水印文字
        /// </summary>
        public string WatermarkText { get; set; }

        /// <summary>
        /// 水印类型
        /// </summary>
        public WatermarkType WatermarkType { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// 转换为userProfileSettings用于数据库存储
        /// </summary>
        public AttachmentSettings AsAttachmentSettings()
        {
            AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
            attachmentSettings.AllowedFileExtensions = AllowedFileExtensions;
            attachmentSettings.BatchUploadLimit = BatchUploadLimit;
            attachmentSettings.InlinedImageHeight = InlinedImageHeight;
            attachmentSettings.InlinedImageWidth = InlinedImageWidth;
            attachmentSettings.MaxAttachmentLength = MaxAttachmentLength;
            attachmentSettings.MaxImageHeight = MaxImageHeight;
            attachmentSettings.MaxImageWidth = MaxImageWidth;
            attachmentSettings.TemporaryAttachmentStorageDay = TemporaryAttachmentStorageDay;

            WatermarkSettings watermarkSettings = attachmentSettings.WatermarkSettings ?? new WatermarkSettings();
            watermarkSettings.WatermarkType = WatermarkType;
            if (this.WatermarkType != WatermarkType.None)
            {
                watermarkSettings.WatermarkImageName = WatermarkImageName;
                watermarkSettings.WatermarkLocation = WatermarkLocation;
                watermarkSettings.WatermarkMinHeight = WatermarkMinHeight;
                watermarkSettings.WatermarkMinWidth = WatermarkMinWidth;
                watermarkSettings.WatermarkOpacity = WatermarkOpacity;
                watermarkSettings.WatermarkText = WatermarkText;
            }
            attachmentSettings.WatermarkSettings = watermarkSettings;
            return attachmentSettings;
        }
    }

}