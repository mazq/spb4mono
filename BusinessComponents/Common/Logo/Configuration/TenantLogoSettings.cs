//--------------------------------------------------------------
//<version>V0.5</verion>
//<createdate>2012-03-20</createdate>
//<author>zhengw</author>
//<email>zhengw@tunynet.com</email>
//<log date="2012-09-06" version="0.5">创建</log>
//--------------------------------------------------------------
//</TunynetCopyright>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Xml.Linq;
using System.Drawing;
using Tunynet.Imaging;
using Tunynet.Utilities;
using System.IO;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 租户标识图配置类
    /// </summary>
    public class TenantLogoSettings
    {

        private static ConcurrentDictionary<string, TenantLogoSettings> registeredTenantLogoSettings = new ConcurrentDictionary<string, TenantLogoSettings>();

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="xElement">标识图配置节点</param>
        private TenantLogoSettings(XElement xElement)
        {
            if (xElement != null)
            {
                XAttribute att = xElement.Attribute("maxLogoLength");
                if (att != null)
                    int.TryParse(att.Value, out _maxLogoLength);
                att = xElement.Attribute("maxImageHeight");
                if (att != null)
                    int.TryParse(att.Value, out _maxHeight);
                att = xElement.Attribute("maxImageWidth");
                if (att != null)
                    int.TryParse(att.Value, out _maxWidth);

                att = xElement.Attribute("storeProviderName");
                if (att != null)
                    _storeProviderName = att.Value;

                att = xElement.Attribute("tenantLogoDirectory");
                if (att != null)
                    _tenantLogoDirectory = att.Value;

                att = xElement.Attribute("tenantTypeId");
                if (att != null)
                    _tenantTypeId = att.Value;
                IEnumerable<XElement> imageSizeTypeElements = xElement.Elements("imageSizeType");
                if (imageSizeTypeElements != null && imageSizeTypeElements.Count() > 0)
                {
                    _imageSizeTypes = new ConcurrentDictionary<string, KeyValuePair<Size, ResizeMethod>>();
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
                        _imageSizeTypes[keyAttr.Value] = new KeyValuePair<Size, ResizeMethod>(new Size(width, height), resizeMethod);
                    }
                }
            }
        }

        /// <summary>
        /// 获取注册的TenantLogoSettings
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        /// <returns></returns>
        public static TenantLogoSettings GetRegisteredSettings(string tenantTypeId)
        {
            TenantLogoSettings tenantLogoSettings;
            if (registeredTenantLogoSettings.TryGetValue(tenantTypeId, out tenantLogoSettings))
                return tenantLogoSettings;

            return null;
        }

        /// <summary>
        /// 获取所有注册的TenantLogoSettings
        /// </summary>
        /// <returns>TenantLogoSettings集合</returns>
        public static IEnumerable<TenantLogoSettings> GetAll()
        {
            return registeredTenantLogoSettings.Values;
        }

        /// <summary>
        /// 注册TenantLogoSettings
        /// </summary>
        /// <remarks>若xElement下有多个add节点，会同时注册多个TenantLogoSettings</remarks>
        /// <param name="xElement">标识图配置节点，会据此寻找其下所有子节点add</param>
        public static void RegisterSettings(XElement xElement)
        {
            IEnumerable<TenantLogoSettings> settings = xElement.Elements("add").Select(n => new TenantLogoSettings(n));
            foreach (var setting in settings)
            {
                registeredTenantLogoSettings[setting.TenantTypeId] = setting;
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

        private string _tenantLogoDirectory = string.Empty;
        /// <summary>
        /// 租户标识图目录
        /// </summary>
        public string TenantLogoDirectory
        {
            get { return _tenantLogoDirectory; }
        }

        private ConcurrentDictionary<string, KeyValuePair<Size, ResizeMethod>> _imageSizeTypes = null;
        /// <summary>
        /// 图片尺寸类型集合
        /// </summary>
        public ConcurrentDictionary<string, KeyValuePair<Size, ResizeMethod>> ImageSizeTypes
        {
            get { return _imageSizeTypes; }
        }

        private int _maxWidth = 0;
        /// <summary>
        /// 图片最大宽度
        /// </summary>
        public int MaxWidth
        {
            get
            {
                if (_maxWidth > 0)
                    return _maxWidth;

                LogoSettings LogoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                return LogoSettings.MaxWidth;
            }
        }

        private int _maxHeight = 0;
        /// <summary>
        /// 图片最大高度
        /// </summary>
        public int MaxHeight
        {
            get
            {
                if (_maxHeight > 0)
                    return _maxHeight;

                LogoSettings LogoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                return LogoSettings.MaxHeight;
            }
        }

        private int _maxLogoLength = 0;
        /// <summary>
        /// 标识图最大长度(单位：K)
        /// </summary>
        public int MaxLogoLength
        {
            get
            {
                if (_maxLogoLength > 0)
                    return _maxLogoLength;

                LogoSettings LogoSettings = DIContainer.Resolve<ILogoSettingsManager>().Get();
                return LogoSettings.MaxLogoLength;
            }
        }


        #endregion

        #region 扩展方法


        /// <summary>
        /// 验证文件大小是否超出限制
        /// </summary>
        /// <param name="contentLength">需要验证的文件大小</param>
        /// <returns>true-未超出限制,false-超出限制</returns>
        public bool ValidateFileLength(int contentLength)
        {
            return contentLength < MaxLogoLength * 1024;
        }
        
        #endregion

    }
}
