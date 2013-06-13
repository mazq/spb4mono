//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Xml.Linq;
using System.Drawing;
using Tunynet.Imaging;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 租户附件配置类
    /// </summary>
    public class TenantAttachmentSettings
    {

        private static ConcurrentDictionary<string, TenantAttachmentSettings> registeredTenantAttachmentSettings = new ConcurrentDictionary<string, TenantAttachmentSettings>();

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="xElement">附件配置节点</param>
        private TenantAttachmentSettings(XElement xElement)
        {
            if (xElement != null)
            {
                XAttribute att = xElement.Attribute("enableWatermark");
                if (att != null)
                    bool.TryParse(att.Value, out _enableWatermark);
                att = xElement.Attribute("maxAttachmentLength");
                if (att != null)
                    int.TryParse(att.Value, out _maxAttachmentLength);
                att = xElement.Attribute("maxImageHeight");
                if (att != null)
                    int.TryParse(att.Value, out _maxImageHeight);
                att = xElement.Attribute("maxImageWidth");
                if (att != null)
                    int.TryParse(att.Value, out _maxImageWidth);

                att = xElement.Attribute("inlinedImageWidth");
                if (att != null)
                    int.TryParse(att.Value, out _inlinedImageWidth);
                att = xElement.Attribute("inlinedImageWidth");
                if (att != null)
                    int.TryParse(att.Value, out _inlinedImageHeight);

                att = xElement.Attribute("storeProviderName");
                if (att != null)
                    _storeProviderName = att.Value;
                att = xElement.Attribute("tenantAttachmentDirectory");
                if (att != null)
                    _tenantAttachmentDirectory = att.Value;
                att = xElement.Attribute("tenantTypeId");
                if (att != null)
                    _tenantTypeId = att.Value;
                att = xElement.Attribute("allowedFileExtensions");
                if (att != null)
                    _allowedFileExtensions = att.Value;
                IEnumerable<XElement> imageSizeTypeElements = xElement.Elements("imageSizeType");
                if (imageSizeTypeElements != null && imageSizeTypeElements.Count() > 0)
                {
                    _imageSizeTypes = new List<ImageSizeType>();
                    foreach (var imageSizeTypeElement in imageSizeTypeElements)
                    {
                        XAttribute keyAttr = imageSizeTypeElement.Attribute("key");
                        if (keyAttr == null)
                            continue;
                        int width = 0;
                        XAttribute widthAttr = imageSizeTypeElement.Attribute("width");
                        if (widthAttr != null)
                            int.TryParse(widthAttr.Value, out width);
                        int height = 0;
                        XAttribute heightAttr = imageSizeTypeElement.Attribute("height");
                        if (heightAttr != null)
                            int.TryParse(heightAttr.Value, out height);
                        ResizeMethod resizeMethod = ResizeMethod.KeepAspectRatio;
                        XAttribute resizeMethodAttr = imageSizeTypeElement.Attribute("resizeMethod");
                        if (resizeMethodAttr != null)
                            Enum.TryParse<ResizeMethod>(resizeMethodAttr.Value, out resizeMethod);
                        _imageSizeTypes.Add(new ImageSizeType(keyAttr.Value, new Size(width, height), resizeMethod));
                    }
                }
            }
        }

        /// <summary>
        /// 获取注册的TenantAttachmentSettings
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public static TenantAttachmentSettings GetRegisteredSettings(string tenantTypeId)
        {
            TenantAttachmentSettings tenantAttachmentSettings;
            if (registeredTenantAttachmentSettings.TryGetValue(tenantTypeId, out tenantAttachmentSettings))
                return tenantAttachmentSettings;

            return null;
        }

        /// <summary>
        /// 获取所有注册的TenantAttachmentSettings
        /// </summary>
        /// <returns>TenantAttachmentSettings集合</returns>
        public static IEnumerable<TenantAttachmentSettings> GetAll()
        {
            return registeredTenantAttachmentSettings.Values;
        }

        /// <summary>
        /// 注册TenantAttachmentSettings
        /// </summary>
        /// <remarks>若xElement下有多个add节点，会同时注册多个TenantAttachmentSettings</remarks>
        /// <param name="xElement">附件配置节点，会据此寻找其下所有子节点add</param>
        public static void RegisterSettings(XElement xElement)
        {
            IEnumerable<TenantAttachmentSettings> settings = xElement.Elements("add").Select(n => new TenantAttachmentSettings(n));
            foreach (var setting in settings)
            {
                registeredTenantAttachmentSettings[setting.TenantTypeId] = setting;
            }
        }

        #region 属性

        private string _storeProviderName = string.Empty;
        /// <summary>
        /// 文件存储功能提供者名称
        /// </summary>
        public string StoreProviderName
        {
            get { return _storeProviderName; }
        }

        private string _tenantTypeId = string.Empty;
        /// <summary>
        /// 租户类型Id
        /// </summary>
        public string TenantTypeId
        {
            get { return _tenantTypeId; }
        }

        private string _tenantAttachmentDirectory = string.Empty;
        /// <summary>
        /// 租户附件目录
        /// </summary>
        public string TenantAttachmentDirectory
        {
            get { return _tenantAttachmentDirectory; }
        }

        private bool _enableWatermark = true;
        /// <summary>
        /// 是否允许使用水印
        /// </summary>
        public bool EnableWatermark
        {
            get { return _enableWatermark; }
        }

        private List<ImageSizeType> _imageSizeTypes;
        /// <summary>
        /// 图片尺寸类型集合
        /// </summary>
        public List<ImageSizeType> ImageSizeTypes
        {
            get
            {
                if (_imageSizeTypes != null)
                    return _imageSizeTypes;
                AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
                return attachmentSettings.ImageSizeTypes;
            }
        }

        private int _maxImageWidth = 0;
        /// <summary>
        /// 图片最大宽度
        /// </summary>
        public int MaxImageWidth
        {
            get
            {
                if (_maxImageWidth > 0)
                    return _maxImageWidth;

                AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
                return attachmentSettings.MaxImageWidth;
            }
        }

        private int _maxImageHeight = 0;
        /// <summary>
        /// 图片最大高度
        /// </summary>
        public int MaxImageHeight
        {
            get
            {
                if (_maxImageHeight > 0)
                    return _maxImageHeight;

                AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
                return attachmentSettings.MaxImageHeight;
            }
        }

        private int _maxAttachmentLength = 0;
        /// <summary>
        /// 附件最大长度(单位：K)
        /// </summary>
        public int MaxAttachmentLength
        {
            get
            {
                if (_maxAttachmentLength > 0)
                    return _maxAttachmentLength;

                AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
                return attachmentSettings.MaxAttachmentLength;
            }
        }

        private string _allowedFileExtensions = string.Empty;
        /// <summary>
        /// 附件允许的文件扩展名
        /// </summary>
        public string AllowedFileExtensions
        {
            get
            {
                if (!string.IsNullOrEmpty(_allowedFileExtensions))
                    return _allowedFileExtensions;

                AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
                return attachmentSettings.AllowedFileExtensions;
            }
        }

        private int _inlinedImageWidth = 0;
        /// <summary>
        /// 页面呈现图片的最大宽度
        /// </summary>
        public int InlinedImageWidth
        {
            get
            {
                if (_inlinedImageWidth > 0)
                    return _inlinedImageWidth;

                AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
                return attachmentSettings.InlinedImageWidth;
            }
        }

        private int _inlinedImageHeight = 0;
        /// <summary>
        /// 页面呈现图片的最大高度
        /// </summary>
        public int InlinedImageHeight
        {
            get
            {
                if (_inlinedImageHeight > 0)
                    return _inlinedImageHeight;

                AttachmentSettings attachmentSettings = DIContainer.Resolve<IAttachmentSettingsManager>().Get();
                return attachmentSettings.InlinedImageHeight;
            }
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 验证是否支持当前文件扩展名
        /// </summary>
        /// <param name="fileName">文件名（带后缀）</param>
        /// <returns>true-支持,false-不支持</returns>
        public bool ValidateFileExtensions(string fileName)
        {
            string fileExtension = fileName.Substring(fileName.LastIndexOf(".") + 1);
            string[] extensions = AllowedFileExtensions.Split(',');

            return extensions.Where(n => n.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)).Count() > 0;
        }

        /// <summary>
        /// 验证文件大小是否超出限制
        /// </summary>
        /// <param name="contentLength">需要验证的文件大小</param>
        /// <returns>true-未超出限制,false-超出限制</returns>
        public bool ValidateFileLength(int contentLength)
        {
            return contentLength < MaxAttachmentLength * 1024;
        }

        #endregion

    }
}
