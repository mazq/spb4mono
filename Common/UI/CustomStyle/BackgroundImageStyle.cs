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
    /// 背景图片样式
    /// </summary>
    public class BackgroundImageStyle
    {
        private string url = string.Empty;
        /// <summary>
        /// 背景图片url
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private bool isRepeat;
        /// <summary>
        /// 是否重复
        /// </summary>
        public bool IsRepeat
        {
            get { return isRepeat; }
            set { isRepeat = value; }
        }

        private bool isFix;
        /// <summary>
        /// 是否固定
        /// </summary>
        public bool IsFix
        {
            get { return isFix; }
            set { isFix = value; }
        }

        private BackgroundPosition backgroundPosition = BackgroundPosition.Center;
        /// <summary>
        /// 背景对齐方式
        /// </summary>
        public BackgroundPosition BackgroundPosition
        {
            get { return backgroundPosition; }
            set { backgroundPosition = value; }
        }
    }

    /// <summary>
    /// 背景对齐方式
    /// </summary>
    public enum BackgroundPosition
    {
        /// <summary>
        /// 居左
        /// </summary>
        Left,

        /// <summary>
        /// 居中
        /// </summary>
        Center,

        /// <summary>
        /// 居右
        /// </summary>
        Right
    }

    /// <summary>
    /// 色彩分类
    /// </summary>
    public enum ColorLabel
    {
        /// <summary>
        /// 页面背景
        /// </summary>
        PageBackground,

        /// <summary>
        /// 页面背景
        /// </summary>
        ContentBackground,

        /// <summary>
        /// 侧栏背景
        /// </summary>
        BorderBackground,

        /// <summary>
        /// 主文字色
        /// </summary>
        MainTextColor,

        /// <summary>
        /// 次文字色
        /// </summary>
        SubTextColor,

        /// <summary>
        /// 主链接色
        /// </summary>
        MainLinkColor,

        /// <summary>
        /// 次链接色
        /// </summary>
        SubLinkColor

    }


}
