//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Caching;
using Tunynet.Common.Repositories;
using Tunynet.Events;
using Tunynet.Common.Configuration;
using Tunynet.FileStore;
using System.Drawing;
using Tunynet.Imaging;

namespace Tunynet.Common
{
    /// <summary>
    /// 图片尺寸类型
    /// </summary>
    public class ImageSizeType
    {

        /// <summary>
        /// 构造器
        /// </summary>
        public ImageSizeType() { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="imageSizeTypeKey">图片尺寸类型Key</param>
        /// <param name="size">图片尺寸</param>
        /// <param name="resizeMethod">缩放方式</param>
        public ImageSizeType(string imageSizeTypeKey, Size size, ResizeMethod resizeMethod)
        {
            this.ImageSizeTypeKey = imageSizeTypeKey;
            this.Size = size;
            this.ResizeMethod = resizeMethod;
        }
        /// <summary>
        /// 图片尺寸类型Key
        /// </summary>
        public string ImageSizeTypeKey { get; set; }

        /// <summary>
        /// 图片尺寸
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// 缩放方式
        /// </summary>
        public ResizeMethod ResizeMethod { get; set; }
    }
}
