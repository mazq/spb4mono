//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Imaging;
using Tunynet.Caching;

namespace Tunynet.Common.Configuration
{
    /// <summary>
    /// 标识图全局设置类
    /// </summary>
    [Serializable]
    [CacheSetting(true)]
    public class LogoSettings : IEntity
    {
        #region 全局设置

        private int _maxLogoLength = 10240;
        /// <summary>
        /// 标识图最大长度
        /// </summary>
        public int MaxLogoLength
        {
            get { return _maxLogoLength; }
            set { _maxLogoLength = value; }
        }

        private string _allowedFileExtensions = "gif,jpg,jpeg,png";
        /// <summary>
        /// 附件允许的文件扩展名
        /// </summary>
        public string AllowedFileExtensions
        {
            get { return _allowedFileExtensions; }
            set { _allowedFileExtensions = value; }
        }

        #endregion

        #region 标识图设置

        private int _maxWidth = 500;
        /// <summary>
        /// 标识图最大宽度
        /// </summary>
        public int MaxWidth
        {
            get { return _maxWidth; }
            set { _maxWidth = value; }
        }

        private int _maxHeight = 500;
        /// <summary>
        /// 标识图最大高度
        /// </summary>
        public int MaxHeight
        {
            get { return _maxHeight; }
            set { _maxHeight = value; }
        }

        private ResizeMethod resizeMethod;
        /// <summary>
        /// 标识图超过最大尺寸限制时，裁剪图像所采用的缩放方式
        /// </summary>
        public ResizeMethod ResizeMethod
        {
            get { return resizeMethod; }
            set { resizeMethod = value; }
        }

        #endregion

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

        #region IEntity成员

        object IEntity.EntityId { get { return typeof(LogoSettings).FullName; } }

        bool IEntity.IsDeletedInDatabase { get; set; }

        #endregion

    }
}
