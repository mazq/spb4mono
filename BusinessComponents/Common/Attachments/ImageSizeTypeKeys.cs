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

namespace Tunynet.Common
{
    /// <summary>
    /// 图片尺寸类型
    /// </summary>
    public class ImageSizeTypeKeys
    {
        #region Instance

        private static volatile ImageSizeTypeKeys _instance = null;
        private static readonly object lockObject = new object();

        /// <summary>
        /// 创建图片尺寸类型
        /// </summary>
        /// <returns></returns>
        public static ImageSizeTypeKeys Instance()
        {
            if (_instance == null)
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ImageSizeTypeKeys();
                    }
                }
            }
            return _instance;
        }

        private ImageSizeTypeKeys()
        { }

        #endregion Instance

        /// <summary>
        /// 原始尺寸
        /// </summary>
        public string Original()
        {
            return "Original";
        }

        /// <summary>
        /// 最大尺寸
        /// </summary>
        /// <remarks>建议配置为800*600，较少使用，主要用于全屏浏览</remarks>
        public string Biggest()
        {
            return "Biggest";
        }


        /// <summary>
        /// 较大尺寸
        /// </summary>
        /// <remarks>建议配置为500*500，用于主内容区详细展示</remarks>
        public string Bigger()
        {
            return "Bigger";
        }

        /// <summary>
        /// 大尺寸
        /// </summary>
        /// <remarks>建议配置为320*240，用于幻灯片展示</remarks>
        public string Big()
        {
            return "Big";
        }

        /// <summary>
        /// 中尺寸
        /// </summary>
        /// <remarks>建议配置为100*100，用于缩略图展示</remarks>
        public string Medium()
        {
            return "Medium";
        }


        /// <summary>
        /// 小尺寸
        /// </summary>
        /// <remarks>建议配置为50*50，用于小型缩略图展示</remarks>
        public string Small()
        {
            return "Small";
        }

    }
}
