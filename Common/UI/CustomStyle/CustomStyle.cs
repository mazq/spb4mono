//------------------------------------------------------------------------------
// <copyright company="Tunynet">
//     Copyright (c) Tunynet Inc.  All rights reserved.
// </copyright> 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spacebuilder.UI
{
    /// <summary>
    /// 自定义风格实体
    /// </summary>
    [Serializable]
    public class CustomStyle
    {
        /// <summary>
        /// 新建实体时使用
        /// </summary>
        public static CustomStyle New()
        {
            CustomStyle customStyle = new CustomStyle()
            {
                BackgroundImage = string.Empty,
                ImageUrl = string.Empty,
                LastModified = DateTime.UtcNow
            };
            return customStyle;
        }

        private int headerHeight;
        /// <summary>
        /// 顶部高度
        /// </summary>
        public int HeaderHeight
        {
            get { return headerHeight; }
            set { headerHeight = value; }
        }
        
        
        private bool isUseBackgroundImage = true;
        /// <summary>
        /// 是否使用背景图片
        /// </summary>
        public bool IsUseBackgroundImage
        {
            get { return isUseBackgroundImage; }
            set { isUseBackgroundImage = value; }
        }

        private BackgroundImageStyle backgroundImageStyle;
        /// <summary>
        /// 背景图片样式
        /// </summary>
        public BackgroundImageStyle BackgroundImageStyle
        {
            get { return backgroundImageStyle; }
            set { backgroundImageStyle = value; }
        }

        private Dictionary<string, string> definedColours;
        /// <summary>
        /// 定义的色彩
        /// </summary>
        /// <remarks>
        /// key=色彩分类，value=颜色值
        /// </remarks>
        public Dictionary<string, string> DefinedColours
        {
            get { return definedColours; }
            set { definedColours = value; }
        }

        private bool isDark = false;
        /// <summary>
        /// 是不是深色调
        /// </summary>
        public bool IsDark
        {
            get { return isDark; }
            set { isDark = value; }
        }

        private string imageUrl;
        /// <summary>
        /// 配色方案标识图片地址
        /// </summary>
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }


        private string backgroundImage;
        /// <summary>
        /// 背景图片名称
        /// </summary>
        public string BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }


        private DateTime lastModified;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
    }
}
