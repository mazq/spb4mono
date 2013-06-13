//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Tunynet.Imaging;
using System.Collections.Concurrent;
using Tunynet.Caching;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 附件全局设置类
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class AttachmentSettings : IEntity
    {
        #region 全局设置

        private int _maxAttachmentLength = 10240;
        /// <summary>
        /// 附件最大长度
        /// </summary>
        public int MaxAttachmentLength
        {
            get { return _maxAttachmentLength; }
            set { _maxAttachmentLength = value; }
        }

        private int _batchUploadLimit = 10;
        /// <summary>
        /// 批量上传数目限制
        /// </summary>
        public int BatchUploadLimit
        {
            get { return _batchUploadLimit; }
            set { _batchUploadLimit = value; }
        }

        private string _allowedFileExtensions = "zip,rar,xml,txt,gif,jpg,jpeg,png,doc,xls,ppt,pdf,swf,mp3,wma,mmv,rm,avi,mov,qt,docx,pptx,xlsx,pps";
        /// <summary>
        /// 附件允许的文件扩展名
        /// </summary>
        public string AllowedFileExtensions
        {
            get { return _allowedFileExtensions; }
            set { _allowedFileExtensions = value; }
        }

        private int _temporaryAttachmentStorageDay = 3;
        /// <summary>
        /// 临时附件保留的天数
        /// </summary>
        public int TemporaryAttachmentStorageDay
        {
            get { return _temporaryAttachmentStorageDay; }
            set { _temporaryAttachmentStorageDay = value; }
        }

        #endregion

        #region 图片设置

        private WatermarkSettings _watermarkSettings = new WatermarkSettings();
        /// <summary>
        /// 水印设置
        /// </summary>
        public WatermarkSettings WatermarkSettings
        {
            get
            {
                return _watermarkSettings;
            }
            set { _watermarkSettings = value; }
        }

        private int _maxImageWidth = 1080;
        /// <summary>
        /// 图片最大宽度
        /// </summary>
        public int MaxImageWidth
        {
            get { return _maxImageWidth; }
            set { _maxImageWidth = value; }
        }

        private int _maxImageHeight = 1080;
        /// <summary>
        /// 图片最大高度
        /// </summary>
        public int MaxImageHeight
        {
            get { return _maxImageHeight; }
            set { _maxImageHeight = value; }
        }

        private int _inlinedImageWidth = 700;
        /// <summary>
        /// 页面呈现图片的最大宽度
        /// </summary>
        public int InlinedImageWidth
        {
            get { return _inlinedImageWidth; }
            set { _inlinedImageWidth = value; }
        }

        private int _inlinedImageHeight = 500;
        /// <summary>
        /// 页面呈现图片的最大高度
        /// </summary>
        public int InlinedImageHeight
        {
            get { return _inlinedImageHeight; }
            set { _inlinedImageHeight = value; }
        }

        private List<ImageSizeType> _imageSizeTypes;
        /// <summary>
        /// 图片尺寸类型集合
        /// </summary>
        public List<ImageSizeType> ImageSizeTypes
        {
            get
            {
                if (_imageSizeTypes == null)
                {
                    _imageSizeTypes = new List<ImageSizeType>();
                    _imageSizeTypes.Add(new ImageSizeType(ImageSizeTypeKeys.Instance().Small(), new Size(50, 50), ResizeMethod.KeepAspectRatio));
                    _imageSizeTypes.Add(new ImageSizeType(ImageSizeTypeKeys.Instance().Medium(), new Size(100, 100), ResizeMethod.KeepAspectRatio));
                    _imageSizeTypes.Add(new ImageSizeType(ImageSizeTypeKeys.Instance().Big(), new Size(320, 240), ResizeMethod.KeepAspectRatio));
                    _imageSizeTypes.Add(new ImageSizeType(ImageSizeTypeKeys.Instance().Bigger(), new Size(500, 500), ResizeMethod.KeepAspectRatio));
                }
                return _imageSizeTypes;
            }
            set { _imageSizeTypes = value; }
        }

        #endregion

        #region IEntity 成员

        object IEntity.EntityId { get { return typeof(AttachmentSettings).FullName; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}
