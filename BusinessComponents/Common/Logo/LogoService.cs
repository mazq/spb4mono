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
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Repositories;
using Tunynet.Utilities;
using Tunynet.FileStore;
using Tunynet.Common.Configuration;
using System.IO;
using Tunynet.Imaging;
using System.Drawing;

namespace Tunynet.Common
{
    /// <summary>
    /// 标识图业务逻辑类
    /// </summary>
    public class LogoService
    {
        //Logo文件扩展名
        private static readonly string LogoFileExtension = "jpg";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantTypeId">租户类型Id</param>
        public LogoService(string tenantTypeId)
        {
            this.TenantLogoSettings = TenantLogoSettings.GetRegisteredSettings(tenantTypeId);
            if (this.TenantLogoSettings == null)
                throw new ExceptionFacade("没有注册租户附件设置");
            this.StoreProvider = DIContainer.ResolveNamed<IStoreProvider>(this.TenantLogoSettings.StoreProviderName);
        }

        /// <summary>
        /// 文件存储Provider
        /// </summary>
        public IStoreProvider StoreProvider { get; private set; }

        /// <summary>
        /// 租户标识图设置
        /// </summary>
        public TenantLogoSettings TenantLogoSettings { get; private set; }

        /// <summary>
        /// 上传Logo
        /// </summary>
        /// <param name="associateId"></param>
        /// <param name="stream"></param>
        /// <returns>上传文件的相对路径（包含文件名）</returns>
        public string UploadLogo(object associateId, Stream stream)
        {
            string relativeFileName=string.Empty;
            if (stream != null)
            {
                ILogoSettingsManager logoSettingsManager = DIContainer.Resolve<ILogoSettingsManager>();
                LogoSettings logoSettings = logoSettingsManager.Get();

                //检查是否需要缩放原图
                Image image = Image.FromStream(stream);
                if (image.Height > this.TenantLogoSettings.MaxHeight || image.Width > this.TenantLogoSettings.MaxWidth)
                {
                    stream = ImageProcessor.Resize(stream, this.TenantLogoSettings.MaxWidth, this.TenantLogoSettings.MaxHeight, logoSettings.ResizeMethod);
                }

                string relativePath = GetLogoRelativePath(associateId);
                string fileName = GetLogoFileName(associateId);
                relativeFileName = relativePath + "\\" + fileName;

                StoreProvider.AddOrUpdateFile(relativePath, fileName, stream);
                stream.Dispose();

                //根据不同租户类型的设置生成不同尺寸的图片，用于图片直连访问
                if (this.TenantLogoSettings.ImageSizeTypes != null && this.TenantLogoSettings.ImageSizeTypes.Count > 0)
                {
                    foreach (var imageSizeType in this.TenantLogoSettings.ImageSizeTypes.Values)
                    {
                        string sizedFileName = StoreProvider.GetSizeImageName(fileName, imageSizeType.Key, imageSizeType.Value);
                        StoreProvider.DeleteFile(relativePath, sizedFileName);
                        IStoreFile file = StoreProvider.GetResizedImage(relativePath, fileName, imageSizeType.Key, imageSizeType.Value);
                    }
                }
            }

            return relativeFileName;
        }

        /// <summary>
        /// 删除Logo
        /// </summary>
        /// <param name="associateId">关联Id</param>
        public void DeleteLogo(object associateId)
        {
            //删除文件系统的Logo
            StoreProvider.DeleteFolder(GetLogoRelativePath(associateId));
        }

        /// <summary>
        /// 获取直连URL
        /// </summary>
        /// <param name="associateId">关联Id</param>
        /// <returns></returns>
        public string GetDirectlyUrl(object associateId)
        {
            string filename = GetLogoFileName(associateId);
            return StoreProvider.GetDirectlyUrl(GetLogoRelativePath(associateId), filename);
        }

        /// <summary>
        /// 获取Logo
        /// </summary>
        /// <param name="associateId">关联Id</param>
        /// <returns></returns>
        public IStoreFile GetLogo(object associateId)
        {
            return StoreProvider.GetFile(GetLogoRelativePath(associateId), GetLogoFileName(associateId));
        }

        /// <summary>
        /// 获取不同尺寸的Logo
        /// </summary>
        /// <param name="associateId"></param>
        /// <param name="imageSizeTypeKey"></param>
        /// <returns></returns>
        public IStoreFile GetResizedLogo(object associateId, string imageSizeTypeKey)
        {
            IStoreFile logoFile = null;
            if (TenantLogoSettings.ImageSizeTypes == null || !TenantLogoSettings.ImageSizeTypes.ContainsKey(imageSizeTypeKey))
                logoFile = GetLogo(associateId);
            else
            {
                var pair = TenantLogoSettings.ImageSizeTypes[imageSizeTypeKey];
                logoFile = StoreProvider.GetResizedImage(GetLogoRelativePath(associateId), GetLogoFileName(associateId), pair.Key, pair.Value);
            }
            return logoFile;
        }

        /// <summary>
        /// 获取Logo存储的相对路径
        /// </summary>
        /// <param name="associateId">associateId</param>
        public string GetLogoRelativePath(object associateId)
        {
            string idString = associateId.ToString().PadLeft(15, '0');
            return StoreProvider.JoinDirectory(this.TenantLogoSettings.TenantLogoDirectory, idString.Substring(0, 5), idString.Substring(5, 5), idString.Substring(10, 5));
        }

        /// <summary>
        /// 获取Logo文件名称
        /// </summary>
        /// <param name="associateId">associateId</param>
        public string GetLogoFileName(object associateId)
        {
            return string.Format("{0}.{1}", associateId, LogoFileExtension);
        }
    }
}